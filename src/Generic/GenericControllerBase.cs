
namespace metabolon.Generic;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using metabolon.Models;

// Generische Controller Basisklasse
//Implementiert 'virtual' Methoden für alle Basisfunktionen der CRUD Operationen [GET, POST, PUT, DELETE]
//Abstract hier bedeutet, dass die spezifischen API Endpunkte, hier Controller, diese Klasse inheriten, d.h. Zugriff auf ihre Methoden haben
//Virtual in den einzelnen Methoden bedeutet, dass die Controller die Möglichkeit haben diese Basisfunktionen vollständig zu überschreiben, aber nicht dazu verpflichtet sind
//Im Fall, dass sie überschreiben wird ihre Logik für die Methoden benutzt, und die in dieser Klasse ignoriert. Falls sie das nicht tun, wird diese Logik als Default verwendet
//Hier existiert nur der extremste Basisfall, bspw. die Create() Methode checkt nur, ob das Objekt ordentlich erstellt wurde und schiebt es dann in die Datenbank
public abstract class GenericControllerBase<TEntity, TDTO, TCreateDTO> : ControllerBase
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


    // Generische GET Methode
    //Liest die Objekte aus der angefragten TEntity Model-Tabelle aus der Datenbank, formt sie zu DTOs und wirft sie vollständig aus
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu prüfen ob und auf was er Zugriff hat
    //1. Nimm die gesamte Liste aller Einträge aus dem mitgegebenen Datenbankset aus
    //2. Mappt die Liste von TEntity Typ Objekten auf TDTO Typ Objekten (Ausgabeoptimiert) und schickt sie an den Client zurück
    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TDTO>>> GetAll()
    {
        //TODO: apply middleware for checking Session Token

        var listOfEntities = await GetDbSet().ToListAsync();
        return Ok(_mapper.Map<List<TDTO>>(listOfEntities));
    }

    // Generische GET Methode mit ID
    //Liest das angefragte Objekt aus der angefragten TEntity Model-Tabelle aus der Datenbank, formt es zu einem DTO und wirft es aus
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu prüfen ob und auf was er Zugriff hat
    //1. Versuche das Objekt anhand der mitgegebenen ID aus der Datenbank zu laden
    //2. Falls das Objekt / die ID in der Datenbank nicht existiert, schmeiß einen 404 Not Found error an den Client zurück
    //3. Falls es existiert, schick das Objekt an den Client zurück
    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TEntity>> GetById(int id)
    {
        //TODO: apply middleware for checking Session Token

        var entity = await GetDbSet().FirstOrDefaultAsync(o => o.Id == id);
        if (entity == null) return NotFound();
        else return Ok(_mapper.Map<TDTO>(entity));
    }

    // Generische POST Methode mit Body Input
    //Checkt die Formate und Datentypen aller Attribute und speichert das Model in die Datenbank
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu prüfen ob und auf was er Zugriff hat
    //1. Das DTO kommt vom Body der HTTPRequest rein, und wird es überprüft, dass alle Attribute vom korrekten Datentyp und im korrekten Format vorhanden sind
    //2. Das DTO wird zu einem Model gemappt
    //3. Das Model wird in die Datenbank gespeichert und an den Client zurückgeschickt
    [HttpPost]
    public virtual async Task<ActionResult<TDTO>> Create([FromBody] TCreateDTO dto)
    {
        //TODO: apply middleware for checking Session Token

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var model = _mapper.Map<TEntity>(dto);
        GetDbSet().Add(model);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.Id }, _mapper.Map<TDTO>(model));
    }

    // Generische PUT Methode mit ID und Body Input
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TDTO>> Update([FromBody] TDTO dto, int id)
    {
        var db_model = await GetDbSet().FirstOrDefaultAsync(o => o.Id == id);
        if (db_model == null) return NotFound();

        if (!ModelState.IsValid) return BadRequest(ModelState);
        _context.Entry(db_model).CurrentValues.SetValues(dto);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<TDTO>(db_model));
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(int id)
    {
        var db_model = await GetDbSet().FirstOrDefaultAsync(o => o.Id == id);
        if (db_model == null) return NotFound();
        else return Ok(GetDbSet().Remove(db_model));
    }

    protected abstract DbSet<TEntity> GetDbSet();
}
