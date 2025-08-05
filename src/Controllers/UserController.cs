namespace metabolon.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using metabolon.DTOs;
using metabolon.Generic;
using metabolon.Models;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;

[Route("api/[Controller]")]
[ApiController]

// Controller Interface für User
//Endpunkt für alle '/user' Anfragen auf die API
//Implementiert die Basis methoden aus GenericControllerBase.cs -> Basis für GET, PUT, DELETE
//Fügt eine '/login' Route hinzu, mit der die Nutzer sich einloggen
//Überschreibt die POST methode mit der Registrierungslogik -> Die Registrierung erfolgt über E-Mail und schickt einen Token an die Target Mail
//Fügt eine '/verify' Route hinzu, mit der die Verifikation erfolgt, indem der eingehende Token geprüft wird

public class UserController(AppDbContext context, IMapper mapper) : GenericControllerBase<User, UserDTO, UserCreateDTO, UserPutDTO>(context, mapper)
{
    //Helfermethode für die GenericControllerBase Logik
    protected override DbSet<User> GetDbSet() => _context.Users;
    //Simple Generierung für den Verifikations-Token, für die Registrierung
    private string GenerateVerificationToken() => Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

    //Hasher für Passwort und Auth management
    private PasswordHasher<User> hasher = new PasswordHasher<User>();

    //Login Methode
    //1. Das DTO kommt als Email - Passwort rein, und es wird überprüft
    //2. Es wird aus der DB ein Record gesucht, in dem die Email übereinstimmt
    //3. Es wird mit dem PasswordHasher Modul überprüft, ob die Passwörter übereinstimmt (Sowohl das einkommende Passwort als auch das gespeicherte liegt nur in Hash-Form vor)
    //4. Der vollständiger User-Record wird als DTO an den Client zurückgeschickt
    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] UserAuthDTO user)
    {
        //TODO: Token logic to determine login / password update

        if (!ModelState.IsValid) return BadRequest(ModelState);
        var entry = await _context.Users.FirstOrDefaultAsync(u => u.Mail == user.Mail);

        //TODO: Hashing Logic

        if (entry == null) return NotFound();
        else if (hasher.VerifyHashedPassword(entry, entry.Password, user.Password) == PasswordVerificationResult.Success) return Ok(_mapper.Map<UserDTO>(entry));
        else return Unauthorized("Invalid Password");
    }

    [HttpPost("password")]
    public async Task<ActionResult> SetPassword([FromBody] UserAuthDTO user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entry = await GetDbSet().FirstOrDefaultAsync(u => u.Mail == user.Mail);
        if (entry == null) return NotFound();

        entry.Password = user.Password;
        return Ok();
    }

    //POST Methode, semantisch: Registrierung eines neuen Nutzers
    //1. Das DTO kommt vom Body der HTTPRequest rein, und es wird überprüft, dass alle Attribute vom korrekten Datentyp und im korrekten Format vorhanden sind
    //2. Das DTO wird aufs Model gemappt, indem den Attributen, die nicht vorhanden sind, entweder Default Werte oder generierte Werte angefügt werden
    //3. Dem Model wird in der Datenbank ein Parameter für den Verifikationstoken angefügt, der selbe Token wird in einen Link eingefügt und per E-Mail verschickt
    //4. Das Model wird in die Datenbank eingespeichert und eine Success Response zurück an den Client geschickt
    [HttpPost]
    public async override Task<ActionResult<UserDTO>> Create([FromBody] UserCreateDTO user)
    {
        
        Console.WriteLine("In Override!");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var model = _mapper.Map<User>(user);
        //TODO: Add Default value assignment if applicable here

        model.verificationToken = GenerateVerificationToken();
        //TODO: Add Email sending logic

        _context.Users.Add(model);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.Id }, _mapper.Map<UserDTO>(model));
    }

    //Verify Methode, verifiziert die Email
    //0. Im POST wird eine Mail geschickt in der ein Link mit dem verificationToken enthalten ist
    //1. Der Token kommt als string rein
    //2. Aus der DB wird ein Record gesucht, in dem dieser Token existiert
    //3. IsVerified wird umgesetzt, was Webzugriff gewährt, der Token wird aus dem Record gelöscht
    //4. Es geht ein einfaches 200 - OK zurück, ohne Inhalt => Auf der Client Seite wird hier weitergeleitet zum Passwort setzen
    [HttpGet("verify")]
    public async Task<ActionResult> VerifyUser(string token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.verificationToken == token);
        if (user == null) return BadRequest("Invalider Token");

        user.IsVerified = true;
        user.verificationToken = "";

        await _context.SaveChangesAsync();
        return Ok();
    }
}