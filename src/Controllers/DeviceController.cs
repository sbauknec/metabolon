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