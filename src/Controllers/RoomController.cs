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
public class RoomController(AppDbContext context, IMapper mapper) : GenericControllerBase<Room, RoomDTO, RoomCreateDTO>(context, mapper)
{
    protected override DbSet<Room> GetDbSet() => _context.Rooms;
}
