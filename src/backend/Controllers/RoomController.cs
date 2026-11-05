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

    [HttpGet]
    public override async Task<ActionResult<IEnumerable<RoomDTO>>> GetAll(){
        var userId = int.Parse(User.FindFirst("Sub")!.Value);
        var permission = await _context.Users.Where(u => u.Id == userId).Select(u => u.role).FirstOrDefaultAsync();
        if(permission == 0) return Forbid("Not permitted to view this list");
        else{
            var rooms = GetDbSet().ToListAsync();
            return Ok(_mapper.Map<RoomDTO>(rooms));
        }        
    }

    // GET Methode mit ID
    //Liest das angefragte Objekt aus der Datenbank, formt es zu einem DTO und wirft es aus
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu prüfen ob und auf was er Zugriff hat
    //1. Versuche das Objekt anhand der mitgegebenen ID aus der Datenbank zu laden
    //2. Lange Query, um die Daten der im DTO eingelassenen QueryDTOs einzulesen und das Rückgabeobjekt zu formen
    //3. Schicke das resultierende Objekt an den Client zurück
    [HttpGet("{id}")]
    public override async Task<ActionResult<RoomDTO>> GetById(int id)
    {
        var userId = int.Parse(User.FindFirst("Sub")!.Value);
        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.UserId == userId && p.RoomId == id);
        if(permission == null) return Forbid("No Access Permission");

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
                                .Where(d => d.RoomId == r.Id)
                                .Select(d => new DeviceQueryDTO
                                {
                                    Id = d.Id,
                                    Name = d.Name,
                                    Location = d.Location
                                }).ToList(),
                            Materials = _context.Items
                                .Where(i => i.RoomId == r.Id)
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

    [HttpPost]
    public async override Task<ActionResult<RoomDTO>> Create([FromBody] RoomCreateDTO dto){
        var userId = int.Parse(User.FindFirst("Sub")!.Value);
        var permission = await _context.Users.Where(u => u.Id == userId).Select(u => u.role).FirstOrDefaultAsync();
        if(permission == 0) return Forbid("Not permitted to create object");
        else{
            var model = _mapper.Map<Room>(dto);
            GetDbSet().Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetById),
                new { id = model.Id },
                _mapper.Map<RoomDTO>(model)
            );
        }
    }

    [HttpPut("{id}")]
    public async override Task<ActionResult<RoomDTO>> Update([FromBody] RoomCreateDTO dto, int id){
        var userId = int.Parse(User.FindFirst("Sub")!.Value);
        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.UserId == userId && p.RoomId == id);
        if(permission == null || !permission.Write) return Forbid("Can't edit entity");
        else{
            var db_model = await GetDbSet().FirstOrDefaultAsync(r => r.Id == id);
            if(db_model == null) return NotFound();

            if(!ModelState.IsValid) return BadRequest(ModelState);

            foreach(var attribute in typeof(RoomCreateDTO).GetProperties()){
                var inputValue = attribute.GetValue(dto);
                if(inputValue != null){
                    var modelProperty = typeof(Room).GetProperty(attribute.Name);
                    if (modelProperty != null) modelProperty.SetValue(db_model, inputValue);
                }
            }

            return Ok(_mapper.Map<RoomDTO>(db_model));
        }
    }

    [HttpPut("linkDocument/{id}")]
    public async Task<ActionResult> linkDocument(int id, int DocumentId)
    {
        var userId = int.Parse(User.FindFirst("Sub")!.Value);
        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.UserId == userId && p.RoomId == id);
        if(permission == null || !permission.Write) return Forbid("Can't edit entity");

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

    [HttpDelete("{id}")]
    public async override Task<ActionResult> Delete(int id){
        var userId = int.Parse(User.FindFirst("Sub")!.Value);
        var permission = await _context.Users.Where(u => u.Id == userId).Select(u => u.role).FirstOrDefaultAsync();
        if(permission == 0) return Forbid("Not permitted to create object");

        var db_model = await GetDbSet().FirstOrDefaultAsync(r => r.Id == id);
        if (db_model == null) return NotFound();
        else
        {
            db_model.IsDeleted = true;
            db_model.DeletedOn = DateOnly.FromDateTime(DateTime.UtcNow);
            await _context.SaveChangesAsync();

            return Ok("Removed");
        }
    }

    //Helfer Funktionen für die GenericControllerBase 
    // => GetDbSet sagt der Instanz auf welche Tabelle der Datenbank Hauptzugriff erfolgen soll/muss
    // => HasReadPermission nimmt ein Device und eine UserId und schickt zurück ob der angefragte Nutzer das Objekt lesen DARF
    // => HasWritePermission nimmt ein Device und eine UserId und schickt zurück ob der angefragte Nutzer das Objekt bearbeiten DARF
    protected override DbSet<Room> GetDbSet() => _context.Rooms;
    protected override async Task<bool> HasReadPermission(Room r, int UserId){
        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == r.Id && p.UserId == UserId);
        return permission != null;
    }
    protected override async Task<bool> HasWritePermission(Room r, int UserId){
        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == r.Id && p.UserId == UserId);
        if(permission == null) return false;
        else return permission.Write;
    }
}
