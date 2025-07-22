namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;
using metabolon.Models;


//DTO - Exposure
//Die Felder in dieser Datei werden an den Nutzer zurückgeschickt
//Omit ALLE sensitiven bzw. zwecklosen Felder
//Expose alle Objekte, aber nicht Ihre ID's
//Query alle Sub-Entities
public class RoomDTO
{
    public int Id { get; set; }
    [MinLength(1)]
    public required string Name { get; set; }
    public UserQueryDTO? Supervisor { get; set; }
    public List<UserQueryDTO>? Present_Users { get; set; }
    public List<DeviceQueryDTO>? Devices { get; set; }
    public List<ItemQueryDTO>? Materials { get; set; }

    //TODO: Machines, Material, Documents, loading / JOIN
}

//DTO - Query
//Wenn das Objekt in einem anderen DTO nested ist, z.B. wenn wir im UserDTO ein UserDTO anfragen was THEORETISCH wieder ein DTO anfragt usw.
//Dann wollen wir eher ein Query DTO, um endloses Looping zu vermeiden, da dieses der einfachste Weg ist das System zum Absturz zu treiben
public class RoomQueryDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Supervisor_Id { get; set; }
}

//DTO - Create
//Input DTO
//Beim Anlegen ist nur der Name erforderlich, die Aufsichtsperson dabei optional oder nachreichbar
public class RoomCreateDTO
{
    [MinLength(1)]
    public required string Name { get; set; }
    public int? Supervisor_Id { get; set; }
}