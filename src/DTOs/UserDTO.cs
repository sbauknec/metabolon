namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;

public class UserDTO
{
    [MinLength(1)]
    public required string Mail { get; set; }
    [MinLength(1)]
    public string? Name { get; set; } = "";
    [MinLength(1)]
    public string? Password { get; set; } = "";
    public int Present_Room_Id { get; set; }
    public int Supervisor_Id { get; set; }
    public bool HasTransponder { get; set; }
    public DateTime Transponder_Exp_Date { get; set; }
    public bool IsExpired { get; set; }
    public bool HasDrivingLicense { get; set; }
}