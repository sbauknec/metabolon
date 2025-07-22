namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;
using metabolon.Models;

//DTO - Exposure
//Die Felder in dieser Datei werden an den Nutzer zurückgeschickt
//Omit ALLE sensitiven bzw. zwecklosen Felder
//Expose alle Objekte, aber nicht Ihre ID's
//Query alle Sub-Entities
public class DocumentDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public UserQueryDTO? Author { get; set; }
    [MinLength(1)]
    public required string Object_Reference { get; set; }
    public bool? IsApproved { get; set; }
    public UserQueryDTO? Supervisor { get; set; }
    public DateTime? ApprovedAt { get; set; }
}

//DTO - Query
//Wenn das Objekt in einem anderen DTO nested ist, z.B. wenn wir im UserDTO ein UserDTO anfragen was THEORETISCH wieder ein DTO anfragt usw.
//Dann wollen wir eher ein Query DTO, um endloses Looping zu vermeiden, da dieses der einfachste Weg ist das System zum Absturz zu treiben
public class DocumentQueryDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Author_Id { get; set; }
    public required string Object_Reference { get; set; }
    public bool? IsApproved { get; set; }
    public int Supervisor_Id { get; set; }
    public DateTime? ApprovedAt { get; set; }
}

//DTO - Create
//Input DTO
//Zum Erstellen ist nur ein Name erforderlich, die anderen Felder sind eher optional
public class DocumentCreateDTO
{
    public required string Name { get; set; }
    public required string Object_Reference { get; set; }
    public bool? IsApproved { get; set; }
}