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

    // Download Methode
    //Liest ein Objekt aus der Dokumente Tabelle, findet die physische Datei auf der Festplatte, und stellt die Datei zum Download zur Verfügung
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu prüfen ob und auf was er Zugriff hat
    //1. Finde das Dokument in der Datenbank unter ID
    //2. Lese den FilePath aus dem Objekt und prüfe ob unter dem Pfad eine Datei existiert
    //3. Erstelle den Stream für die Datei und ziehe ihn in bytes
    //4. Setze die Bytes zu einem File zusammen und schicke es zurück an den Client
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

    // Upload Methode mit Anfrage zur Überprüfung
    //Lädt ein File hoch mit Create DTO, legt die DB Zeile an, Speichert das Dokument an die korrekte Stelle an der Festplatte, und schickt eine Mail an die Aufsichtsperson
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu prüfen ob und auf was er Zugriff hat
    //1. Prüfe ob das File existiert, das DTO ordentlich geformt ist
    //2. Speichere das File unter ~/Documents/Pending/[File] ab
    //3. Baue mit den Informationen das Documents Objekt für die Datenbank und lege es ab
    //4. Suche den Supervisor / Aufsichtsperson des anlegenden Users, und baue die Dictionary für Mail Senden
    //5. Schicke die Mail ab und schicke das erstellte DB Objekt an den Client zurück
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
        model.Author_Id = userId;

        model.OriginalName = file.FileName;
        model.StorageName = InternalStorageName;
        model.FilePath = path;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
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

    // Upload Methode ohne Prüfung (Für Systempflege / Administrative Dokumentenpflege)
    //Lädt ein File hoch mit Create DTO, legt die DB Zeile an, und Speichert das Dokument an die korrekte Stelle an der Festplatte
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu prüfen ob und auf was er Zugriff hat
    //1. Prüfe ob das File existiert und das DTO ordentlich geformt ist
    //2. Speichere das File unter ~/Documents/Accepted/[File] ab
    //3. Baue mit den Informationen das Documents Objekt für die Datenbank und lege es ab
    //3.a) In diesem Workflow ist IsApproved = true => braucht keine externe Zustimmung von Aufsichtsperson
    //4. Schicke das erstellte DB Objekt an den Client zurück
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

    // Validierungsfunktion für Dokumente
    //Validiert ein Dokument auf {id}, schiebt die zugehörige Datei von ~/Pending nach ~/Accepted und setzt die Felder in der DB auf die entsprechenden Werte
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu Prüfen ob und auf was er Zugriff hat
    //1. Prüfe ob das Dokument in der DB existiert, dann prüfe ob das Dokument bereits validiert wurde, dann prüfe ob die Datei unter dem eingespeicherten Dateipfad existiert
    //2. Baue den neuen Dateipfad und schiebe die vollständige datei von ~/Documents/Pending/[File] nach ~/Documents/Accepted/[File]
    //3. Setze die neuen Werte (neuer Dateipfad, IsApproved, ApprovedAt) im DB Objekt, speichere es ab
    //4. Schicke das veränderte Dokument an den Client zurück
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

        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<DocumentDTO>(document));
    }

    //Helfer Funktionen für die GenericControllerBase 
    // => GetDbSet sagt der Instanz auf welche Tabelle der Datenbank Hauptzugriff erfolgen soll/muss
    // => HasReadPermission nimmt ein Device und eine UserId und schickt zurück ob der angefragte Nutzer das Objekt lesen DARF
    // => HasWritePermission nimmt ein Device und eine UserId und schickt zurück ob der angefragte Nutzer das Objekt bearbeiten DARF
    protected override DbSet<Document> GetDbSet() => _context.Documents;
    protected override async Task<bool> HasReadPermission(Document d, int UserId){
        var RD = await _context.Documents_Rooms.FirstOrDefaultAsync(rd => rd.Document_Id == d.Id);
        var DD = await _context.Documents_Devices.FirstOrDefaultAsync(dd => dd.Document_Id == d.Id);
        if(RD == null && DD == null) return false;

        var roomId = -1;
        if(DD != null){
            var device = await _context.Devices.FirstOrDefaultAsync(device => device.Id == DD.Device_Id);
            roomId = device.RoomId.Value;
        }
        else roomId = RD.Room_Id;

        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == roomId && p.UserId == UserId);
        return permission != null;
    }
    protected override async Task<bool> HasWritePermission(Document d, int UserId){
        var RD = await _context.Documents_Rooms.FirstOrDefaultAsync(rd => rd.Document_Id == d.Id);
        var DD = await _context.Documents_Devices.FirstOrDefaultAsync(dd => dd.Document_Id == d.Id);
        if(RD == null && DD == null) return false;

        var roomId = -1;
        if(DD != null){
            var device = await _context.Devices.FirstOrDefaultAsync(device => device.Id == DD.Device_Id);
            roomId = device.RoomId.Value;
        }
        else roomId = RD.Room_Id;

        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == roomId && p.UserId == UserId);
        if(permission == null) return false;
        else return permission.Write;
    }
}