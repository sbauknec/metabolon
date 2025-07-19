namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;

//DTO - Exposure
//Die Felder in dieser Datei werden an den Nutzer zurückgeschickt
//Omit ALLE sensitiven Felder, bspw. Passwort
//Expose alle Objekte, aber nicht Ihre ID's

public class UserDTO
{
    public int Id { get; set; }
    [MinLength(1)]
    public required string Mail { get; set; }
    [MinLength(1)]
    public string? Name { get; set; } = "";
    public RoomQueryDTO? Present_Room { get; set; }
    public UserQueryDTO? Supervisor { get; set; }
    public bool HasTransponder { get; set; }
    public DateTime Transponder_Exp_Date { get; set; }
    public bool IsExpired { get; set; }
    public bool HasDrivingLicense { get; set; }
}

//DTO - Auth
//Registrierung, Login, Passwortveränderung, etc.
//REINES INPUT DTO
//Wird niemals exposet, d.h. an den Nutzer geschickt, ist nur rein Inputschema

public class UserAuthDTO
{
    [MinLength(1)]
    public required string Mail { get; set; }
    [MinLength(1)]
    public required string Password { get; set; }
}

//DTO - Query
//Wenn das Objekt in einem anderen DTO nested ist, z.B. wenn wir im UserDTO ein UserDTO anfragen was THEORETISCH wieder ein DTO anfragt usw.
//Dann wollen wir eher ein Query DTO, um endloses Looping zu vermeiden, da dieses der einfachste Weg ist das System zum Absturz zu treiben

public class UserQueryDTO
{
    public int Id { get; set; }
    public required string Mail { get; set; }
    public string? Name { get; set; }
}

//DTO - Create
//Input DTO
//Beim Anlegen eines Users ist eigentlich nur die Mail erforderlich, der Name dabei optional
public class UserCreateDTO
{
    [MinLength(1)]
    public required string Mail { get; set; }
    public string? Name { get; set; }
}