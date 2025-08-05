namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;

[Route("api/[Controller]")]
[ApiController]

//TODO: Update Document Create DTO
public class DocumentController(AppDbContext context, IMapper mapper) : GenericControllerBase<Document, DocumentDTO, DocumentDTO, DocumentDTO>(context, mapper)
{
    protected override DbSet<Document> GetDbSet() => _context.Documents;
}