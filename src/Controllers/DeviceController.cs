namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;

[Route("api/[Controller]")]
[ApiController]

public class DeviceController(AppDbContext context, IMapper mapper) : GenericControllerBase<Device, DeviceDTO, DeviceDTO>(context, mapper)
{
    protected override DbSet<Device> GetDbSet() => _context.Devices;
}