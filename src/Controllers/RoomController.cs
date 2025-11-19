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

    [HttpGet("{id}")]
    public override async Task<ActionResult<RoomDTO>> GetById(int id)
    {
        //var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        //if (room == null) return NotFound($"Room under {id} does not exist");

        var outputDTO = await _context.Rooms
                        .Where(r => r.Id == id)
                        .Select(r => new RoomDTO
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Supervisor = new UserQueryDTO
                            {
                                Id = r.Supervisor.Id,
                                Mail = r.Supervisor.Mail,
                                Name = r.Supervisor.Name
                            },
                            Present_Users = _context.Users
                                .Where(u => u.Present_Room_Id == r.Id)
                                .Select(u => new UserQueryDTO
                                {
                                    Id = u.Id,
                                    Mail = u.Mail,
                                    Name = u.Name
                                }).ToList(),
                            Devices = _context.Devices
                                .Where(d => d.Room_Id == r.Id)
                                .Select(d => new DeviceQueryDTO
                                {
                                    Id = d.Id,
                                    Name = d.Name,
                                    Location = d.Location
                                }).ToList(),
                            Materials = _context.Items
                                .Where(i => i.Room_Id == r.Id)
                                .Select(i => new ItemQueryDTO
                                {
                                    Id = i.Id,
                                    Name = i.Name,
                                    Amount = i.Amount,
                                    Location = i.Location
                                }).ToList(),
                            Documents = _context.Documents_Rooms
                                .Where(rd => rd.Room_Id == r.Id)
                                .Select(rd => new DocumentQueryDTO
                                {
                                    Id = rd.Document.Id,
                                    OriginalName = rd.Document.OriginalName,
                                    Author_Id = rd.Document.Author_Id,
                                    IsApproved = rd.Document.IsApproved,
                                    Supervisor_Id = rd.Document.Supervisor_Id,
                                    ApprovedAt = rd.Document.ApprovedAt
                                }).ToList()
                        }).FirstOrDefaultAsync();
        return Ok(_mapper.Map<RoomDTO>(outputDTO));
    }

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
