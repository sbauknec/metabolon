namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;

[Route("api/[Controller]")]
[ApiController]

public class DeviceController(AppDbContext context, IMapper mapper) : GenericControllerBase<Device, DeviceDTO, DeviceCreateDTO, DeviceDTO>(context, mapper)
{

    // GET Methode mit ID und Anhangfunktionalität für Dokumente
    //Liest das Device aus der Datenbank, mappt es auf DTO um, und hängt zum Schluss die Liste aller angehörigen Dokumente an
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu prüfen ob und auf was er Zugriff hat
    //1. Finde das Device in der Datenbank
    //2. Mappt die Liste der Devices auf DeviceDTO (ausgabeoptimiert) um
    //3. Finde alle DokumentIDS des Devices durch querien der JOIN Tabelle
    //4. Finde alle Dokumente durch querien der Documents Tabelle und füge sie an das DTO an, dann schick es an den Client zurück
    [HttpGet("{id}")]
    public override async Task<ActionResult<DeviceDTO>> GetById(int id)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
        if (device == null) return NotFound($"Device under {id} does not exist");

        var output = _mapper.Map<DeviceDTO>(device);

        List<int> documentIds = await _context.Documents_Devices.Where(dd => dd.Device_Id == id).Select(dd => dd.Document_Id).ToListAsync();
        if (documentIds.Count != 0) output.Documents = new List<DocumentQueryDTO>();
        documentIds.ForEach(async Id =>
        {
            var doc = await _context.Documents.FirstOrDefaultAsync(doc => doc.Id == Id);
            output.Documents!.Add(_mapper.Map<DocumentQueryDTO>(doc));
        });

        return Ok(output);
    }

    // Link Methode um Dokumente an ein Device unter ID anzuhängen
    //Findet ein Device und ein Dokument aus der Datenbank und erstellt ein Objekt in der JOIN Tabelle, das den Anhang bildet
    //0. Check den mitgegebenen Token der Session des Nutzers, um zu prüfen ob und auf was er Zugriff hat
    //1. Finde das Device unter id und das Dokument unter DocumentId
    //2. Erstelle ein Objekt vom Typ documents_devices und schreibe beide IDs hinein
    //3. Lege das Objekt in die Datenbank und schicke einen leeren Success-Token an den Client zurück
    [HttpPut("linkDocument/{id}")]
    public async Task<ActionResult> linkDocument(int id, int DocumentId)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
        if (device == null) return NotFound($"Device under {id} does not exist");

        var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == DocumentId);
        if (document == null) return NotFound($"Document under {DocumentId} does not exist");

        var cross = new documents_devices();
        cross.Device_Id = id;
        cross.Document_Id = DocumentId;

        _context.Documents_Devices.Add(cross);
        await _context.SaveChangesAsync();

        return Ok();
    }

    //Helfer Funktionen für die GenericControllerBase 
    // => GetDbSet sagt der Instanz auf welche Tabelle der Datenbank Hauptzugriff erfolgen soll/muss
    // => HasReadPermission nimmt ein Device und eine UserId und schickt zurück ob der angefragte Nutzer das Objekt lesen DARF
    // => HasWritePermission nimmt ein Device und eine UserId und schickt zurück ob der angefragte Nutzer das Objekt bearbeiten DARF
    protected override DbSet<Device> GetDbSet() => _context.Devices;
    protected override async Task<bool> HasReadPermission(Device d, int UserId){
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == d.RoomId);
        if(room == null) return false;

        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == room.Id && p.UserId == UserId);
        return permission != null;
    }
    protected override async Task<bool> HasWritePermission(Device d, int UserId){
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == d.RoomId);
        if(room == null) return false;

        var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.RoomId == room.Id && p.UserId == UserId);
        if(permission == null) return false;
        else return permission.Write;
    }
}