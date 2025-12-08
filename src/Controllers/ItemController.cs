namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;

[Route("api/[Controller]")]
[ApiController]

//TODO: Update Item Create DTO
public class ItemController(AppDbContext context, IMapper mapper) : GenericControllerBase<Item, ItemDTO, ItemCreateDTO, ItemDTO>(context, mapper)
{
    protected override DbSet<Item> GetDbSet() => _context.Items;
    protected override Task<bool> HasReadPermission(Item d, int UserId){
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == d.RoomId);
        if(room == null) return NotFound();

        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == room.Id && p.UserId == UserId);
        return permission != null;
    }
    protected override Task<bool> HasWritePermission(Item d, int UserId){
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == d.RoomId);
        if(room == null) return NotFound();

        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == room.Id && p.UserId == UserId);
        if(permission == null) return false;
        else return permission.Write;
    }
}