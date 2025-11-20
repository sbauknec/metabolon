namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;

[Route("api/[Controller]")]
[ApiController]

//TODO: Update Document Create DTO
public class DocumentController(AppDbContext context, IMapper mapper, AppSettings settings, IEmailService emailService) : GenericControllerBase<Document, DocumentDTO, DocumentCreateDTO, DocumentDTO>(context, mapper)
{

    private readonly AppSettings _settings = settings;
    private readonly IEmailService _emailService = emailService;

    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(int id)
    {
        var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);
        if (document == null) return NotFound($"Document under {id} does not exist");

        var path = document.FilePath;
        if (!System.IO.File.Exists(path)) return NotFound($"File under {path} does not exist");

        var contentType = "";
        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(Path.GetFileName(path), out contentType)) contentType = "application/octet-stream";

        var bytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(bytes, contentType, Path.GetFileName(path));
    }

    [HttpPost("withFile")]
    public async Task<ActionResult<DocumentDTO>> CreateFromFile(IFormFile file, [FromBody] DocumentCreateDTO document)
    {
        if (file == null || file.Length == 0) return BadRequest("No file attached");

        var userId = int.Parse(User.FindFirst("Sub")!.Value);
        if (userId == null) return Unauthorized();

        var InternalStorageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var path = Path.Combine(_settings.BaseFilePath!, "Documents", "Pending", InternalStorageName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var model = _mapper.Map<Document>(document);

        model.CreatedAt = DateTime.UtcNow;
        model.Author_Id = int.Parse(userId);

        model.OriginalName = file.FileName;
        model.StorageName = InternalStorageName;
        model.FilePath = path;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
        if (user == null) return NotFound($"User under {userId} does not exist");

        var supervisor = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Supervisor_Id);
        if (supervisor == null) return NotFound($"User under {user.Supervisor_Id} does not exist");

        model.Supervisor_Id = supervisor.Id;

        _context.Documents.Add(model);
        await _context.SaveChangesAsync();

        var values = new Dictionary<String, String>
        {
            { "name", supervisor.Name! },
            { "requestingName", user.Name! },
            { "documentId", model.Id.ToString() },
            { "toEmail", supervisor.Mail }
        };

        await _emailService.SendMailAsync(1, values);

        return CreatedAtAction(nameof(GetById), new { id = model.Id }, _mapper.Map<DocumentDTO>(model));
    }


    [HttpPost("upload")]
    public async Task<ActionResult> Upload(IFormFile file, [FromBody] DocumentCreateDTO document)
    {
        if (file == null || file.Length == 0) return BadRequest("No file attached");

        //Der User für Author_Id kommt aus dem Webtoken, der in jeder Request mitfliegt
        var userId = User.FindFirst("Sub")?.Value;
        if (userId == null) return Unauthorized();

        var InternalStorageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var path = Path.Combine(_settings.BaseFilePath!, "Documents", "Accepted", InternalStorageName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        //Normaler Create Workflow aber das Dokument ist bereits bestätigt und wartet nicht auf Prüfung
        var model = _mapper.Map<Document>(document);
        model.IsApproved = true;
        model.ApprovedAt = DateTime.UtcNow;
        model.CreatedAt = DateTime.UtcNow;
        model.Author_Id = int.Parse(userId);

        model.OriginalName = file.FileName;
        model.StorageName = InternalStorageName;
        model.FilePath = path;

        _context.Documents.Add(model);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("approve/{id}")]
    public async Task<ActionResult<DocumentDTO>> ApproveDocument(int id)
    {
        var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);
        if (document == null) return NotFound($"Document under {id} does not exist");
        if (!System.IO.File.Exists(document.FilePath)) return NotFound($"No file under {document.FilePath}");
        if (document.IsApproved) return BadRequest($"No approval pending for {id}");

        var newPath = Path.Combine(_settings.BaseFilePath!, "Documents", "Accepted", document.StorageName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(newPath)!);

            System.IO.File.Move(document.FilePath, newPath);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error moving file: {ex}");
        }

        document.FilePath = newPath;

        document.IsApproved = true;
        document.ApprovedAt = DateTime.UtcNow;

        return Ok(_mapper.Map<DocumentDTO>(document));
    }

    protected override DbSet<Document> GetDbSet() => _context.Documents;
}