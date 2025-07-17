namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;
using metabolon.Models;

public class RoomDTO
{
    [MinLength(1)]
    public required string Name { get; set; }
    public User? Supervisor { get; set; }
    
    //TODO: Machines, Material, Documents, loading / JOIN
}