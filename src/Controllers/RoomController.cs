namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;

[Route("api/[Controller]")]
[ApiController]

public class RoomController(AppDbContext context, IMapper mapper) : GenericControllerBase<Room, RoomDTO>(context, mapper)
{
    protected override DbSet<Room> GetDbSet() => _context.Rooms;
}
