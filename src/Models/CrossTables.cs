namespace metabolon.Models;

public class documents_rooms
{
    public int Id { get; set; }
    public int Room_Id { get; set; }
    public int Document_Id { get; set; }
    
    //Archivierung statt Löschen
    public bool IsDeleted { get; set; } = false;
    public DateOnly? DeletedOn { get; set; }
}

public class documents_devices
{
    public int Id { get; set; }
    public int Device_Id { get; set; }
    public int Document_Id { get; set; }
    
    //Archivierung statt Löschen
    public bool IsDeleted { get; set; } = false;
    public DateOnly? DeletedOn { get; set; }
}