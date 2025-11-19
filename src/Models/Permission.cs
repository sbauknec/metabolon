namespace metabolon.Models;
public class Permission
{

    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    public bool? Write { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}