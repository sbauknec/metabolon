namespace metabolon.Models;

using metabolon.Generic;

public class Document : IEntity
{
    public int Id { get; set; }
    public required string OriginalName { get; set; }
    public required string StorageName { get; set; }
    public required string FilePath { get; set; }
    public int Author_Id { get; set; }
    public bool IsApproved { get; set; } = false;
    public int Supervisor_Id { get; set; }
    public DateTime ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    //Archivierung statt Löschen
    public bool IsDeleted { get; set; } = false;
    public DateOnly? DeletedOn { get; set; }
}