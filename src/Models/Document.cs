namespace metabolon.Models;

using metabolon.Generic;

public class Document : IEntity
{
    public int Id { get; set; }
    public int Author_Id { get; set; }
    public required string Object_Reference { get; set; }
    public bool IsApproved { get; set; } = false;
    public int Supervisor_Id { get; set; }
    public DateTime ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}