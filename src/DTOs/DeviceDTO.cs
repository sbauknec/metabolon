namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;

public class DeviceDTO
{
    public required string Name { get; set; }
    public DateTime? Maintenance_Date { get; set; }
    public bool? IsMaintained { get; set; }
    public string? Location { get; set; }

    //TODO: Figure out Documents JOIN
}