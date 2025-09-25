namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;

[Route("api/[Controller]")]
[ApiController]

// Controller Interface für Room
//Endpunkt für alle '/room' Anfragen auf die API
//Implementiert die Basis methoden aus GenericControllerBase.cs -> Basis für GET, PUT, DELETE
public class RoomController(AppDbContext context, IMapper mapper) : GenericControllerBase<Room, RoomDTO, RoomCreateDTO, RoomCreateDTO>(context, mapper)
{

    [HttpPut("linkDocument/{id}")]
    public async Task<ActionResult> linkDocument(int id, int DocumentId)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null) return NotFound($"Room under {id} does not exist");

        var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == DocumentId);
        if (document == null) return NotFound($"Document under {DocumentId} does not exist");

        var cross = new documents_rooms();
        cross.Room_Id = id;
        cross.Document_Id = DocumentId;

        _context.Documents_Rooms.Add(cross);
        await _context.SaveChangesAsync();

        return Ok();
    }

    protected override DbSet<Room> GetDbSet() => _context.Rooms;
}
