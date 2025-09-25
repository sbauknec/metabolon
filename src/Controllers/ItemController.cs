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
}