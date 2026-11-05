namespace metabolon.DTOs;

public class PermissionDTO{

    public int Id { get; set; }
    public int UserId { get; set; }
    public UserQueryDTO User { get; set; }
    public int RoomId { get; set; }
    public RoomQueryDTO Room { get; set; }
    public bool? Write { get; set; } = false;

}

public class PermissionCreateDTO{
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public bool? Write { get; set; } = false;
}