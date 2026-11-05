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
    //Helfer Funktionen für die GenericControllerBase 
    // => GetDbSet sagt der Instanz auf welche Tabelle der Datenbank Hauptzugriff erfolgen soll/muss
    // => HasReadPermission nimmt ein Device und eine UserId und schickt zurück ob der angefragte Nutzer das Objekt lesen DARF
    // => HasWritePermission nimmt ein Device und eine UserId und schickt zurück ob der angefragte Nutzer das Objekt bearbeiten DARF
    protected override DbSet<Item> GetDbSet() => _context.Items;
    protected override async Task<bool> HasReadPermission(Item d, int UserId){
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == d.RoomId);
        if(room == null) return false;

        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == room.Id && p.UserId == UserId);
        return permission != null;
    }
    protected override async Task<bool> HasWritePermission(Item d, int UserId){
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == d.RoomId);
        if(room == null) return false;

        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == room.Id && p.UserId == UserId);
        if(permission == null) return false;
        else return permission.Write;
    }
}