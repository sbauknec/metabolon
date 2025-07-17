namespace metabolon.Models;

using metabolon.Generic;

public class User : IEntity
{
    public int Id { get; set; }
    public string? Name { get; set; } = "";
    public required string Mail { get; set; }
    public string? Password { get; set; } = "";
    public int Present_Room_Id { get; set; }
    //Eager loading
    public Room? Present_Room { get; set; }
    public int Supervisor_Id { get; set; }
    //Eager loading
    public User? Supervisor { get; set; }
    public bool HasTransponder { get; set; } = false;
    public DateTime Transponder_Exp_Date { get; set; }
    public bool IsExpired { get; set; } = false;
    public bool HasDrivingLicense { get; set; } = false;

    //Verifikation
    public string? verificationToken { get; set; }
    public bool IsVerified { get; set; } = false;

    //TODO: Figure out Permissions
}