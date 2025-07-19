namespace metabolon.Models;

using metabolon.Generic;

public class Device : IEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime Maintenance_Date { get; set; } = DateTime.UtcNow;
    public bool IsMaintained { get; set; } = false;
    public string? Location { get; set; } = "";
    public int Room_id { get; set; }

    //TODO: Figure out Documents JOIN
}