namespace metabolon.Models;

using metabolon.Generic;

//TODO: Figure out eager loading viability on fields based on computing needs

public class User : IEntity
{
    public int Id { get; set; }
    public string? Name { get; set; } = "";
    public required string Mail { get; set; }
    public string? Password { get; set; } = "";
    public int Present_Room_Id { get; set; }
    public int Supervisor_Id { get; set; }
    public bool HasTransponder { get; set; } = false;
    public DateTime Transponder_Exp_Date { get; set; }
    public bool IsExpired { get; set; } = false;
    public bool HasDrivingLicense { get; set; } = false;

    //Verifikation
    public string? verificationToken { get; set; }
    public bool IsVerified { get; set; } = false;
    public bool? IsDeleted { get; set; } = false;
    public DateOnly? DeletedOn { get; set; }

    //TODO: Figure out Permissions
}