
/**
namespace metabolon.Generic;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using metabolon.Models;

public abstract class GenericControllerBase<TEntity, TDTO> : GenericControllerBase
    where TEntity : class, IEntity
    where TDTO : class
{
    protected readonly AppDbContext _context;
    protected readonly IMapper _mapper;

    public GenericControllerBase(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TDTO>>> GetAll()
    {

    }

}
*/