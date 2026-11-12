namespace metabolon.Models;

using System.ComponentModel.DataAnnotations.Schema;

public class documents_rooms
{
    public int Id { get; set; }
    public int Room_Id { get; set; }

    [ForeignKey(nameof(Room_Id))]
    public Room? Room { get; set; }
    public int Document_Id { get; set; }

    [ForeignKey(nameof(Document_Id))]
    public Document? Document { get; set; }
    
    //Archivierung statt Löschen
    public bool IsDeleted { get; set; } = false;
    public DateOnly? DeletedOn { get; set; }
}

public class documents_devices
{
    public int Id { get; set; }
    public int Device_Id { get; set; }

    [ForeignKey(nameof(Device_Id))]
    public Device? Device { get; set; }
    public int Document_Id { get; set; }

    [ForeignKey(nameof(Document_Id))]
    public Document? Document { get; set; }
    
    //Archivierung statt Löschen
    public bool IsDeleted { get; set; } = false;
    public DateOnly? DeletedOn { get; set; }
}