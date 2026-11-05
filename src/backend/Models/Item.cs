namespace metabolon.Models;

using metabolon.Generic;

public class Item : IEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Amount { get; set; } = 0;
    public string? Location { get; set; } = "";
    public string? Article_Nr { get; set; } = "";
    public int? RoomId { get; set; }
    
    //Archivierung statt Löschen
    public bool IsDeleted { get; set; } = false;
    public DateOnly? DeletedOn { get; set; }
}