namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;

[Route("api/[Controller]")]
[ApiController]

public class DeviceController(AppDbContext context, IMapper mapper) : GenericControllerBase<Device, DeviceDTO, DeviceCreateDTO, DeviceDTO>(context, mapper)
{

    [HttpGet("{id}")]
    public override async Task<ActionResult<DeviceDTO>> GetById(int id)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
        if (device == null) return NotFound($"Device under {id} does not exist");

        var output = _mapper.Map<DeviceDTO>(device);

        List<int> documentIds = await _context.Documents_Devices.Where(dd => dd.Device_Id == id).Select(dd => dd.Document_Id).ToListAsync();
        if (documentIds.Count != 0) output.Documents = new List<DocumentQueryDTO>();
        documentIds.ForEach(async Id =>
        {
            var doc = await _context.Documents.FirstOrDefaultAsync(doc => doc.Id == Id);
            output.Documents!.Add(_mapper.Map<DocumentQueryDTO>(doc));
        });

        return Ok(output);
    }

    [HttpPut("linkDocument/{id}")]
    public async Task<ActionResult> linkDocument(int id, int DocumentId)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
        if (device == null) return NotFound($"Device under {id} does not exist");

        var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == DocumentId);
        if (document == null) return NotFound($"Document under {DocumentId} does not exist");

        var cross = new documents_devices();
        cross.Device_Id = id;
        cross.Document_Id = DocumentId;

        _context.Documents_Devices.Add(cross);
        await _context.SaveChangesAsync();

        return Ok();
    }

    protected override DbSet<Device> GetDbSet() => _context.Devices;
}