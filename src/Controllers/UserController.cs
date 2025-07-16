namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;

[Route("api/[Controller]")]
[ApiController]

// Controller Interface für User
//Endpunkt für alle '/user' Anfragen auf die API
//Implementiert die Basis methoden aus GenericControllerBase.cs -> Basis für GET, PUT, DELETE
//Überschreibt die POST methode mit der Registrierungslogik -> Die Registrierung erfolgt über E-Mail und schickt einen Token an die Target Mail
//Fügt eine '/verify' Route hinzu, mit der die Verifikation erfolgt, indem der eingehende Token geprüft wird

public class UserController(AppDbContext context, IMapper mapper) : GenericControllerBase<User, UserDTO>(context, mapper)
{
    //Helfermethode für die GenericControllerBase Logik
    protected override DbSet<User> GetDbSet() => _context.Users;
    //Simple Generierung für den Verifikations-Token, für die Registrierung
    private string GenerateVerificationToken() => Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

    //POST Methode, semantisch: Registrierung eines neuen Nutzers
    //1. Das DTO kommt vom Body der HTTPRequest rein, und es wird überprüft, dass alle Attribute vom korrekten Datentyp und im korrekten Format vorhanden sind
    //2. Das DTO wird aufs Model gemappt, indem den Attributen, die nicht vorhanden sind, entweder Default Werte oder generierte Werte angefügt werden
    //3. Dem Model wird in der Datenbank ein Parameter für den Verifikationstoken angefügt, der selbe Token wird in einen Link eingefügt und per E-Mail verschickt
    //4. Das Model wird in die Datenbank eingespeichert und eine Success Response zurück an den Client geschickt
    [HttpPost]
    public async override Task<ActionResult<UserDTO>> Create([FromBody] UserDTO user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var model = _mapper.Map<User>(user);
        //TODO: Add Default value assignment if applicable here

        //TODO: Add Token saving and Email verification functionality on register

        _context.Users.Add(model);
        //TODO: Think about the validity of sending Model vs sending DTO
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }
}