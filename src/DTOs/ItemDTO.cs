namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;

//DTO - Exposure
//Die Felder in dieser Datei werden an den Nutzer zurückgeschickt
//Omit ALLE sensitiven bzw. zwecklosen Felder
//Expose alle Objekte, aber nicht Ihre ID's
//Query alle Sub-Entities
public class ItemDTO
{
    public int Id { get; set; }
    [MinLength(1)]
    public required string Name { get; set; }
    public int Amount { get; set; }
    [MinLength(1)]
    public string? Location { get; set; }
    [MinLength(1)]
    public string? Article_Nr { get; set; }
}

//DTO - Query
//Wenn das Objekt in einem anderen DTO nested ist, z.B. wenn wir im UserDTO ein UserDTO anfragen was THEORETISCH wieder ein DTO anfragt usw.
//Dann wollen wir eher ein Query DTO, um endloses Looping zu vermeiden, da dieses der einfachste Weg ist das System zum Absturz zu treiben
public class ItemQueryDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Amount { get; set; }
    public string? Location { get; set; }
}

//DTO - Create
//Input DTO
//Zum Erstellen ist nur ein Name erforderlich, die anderen Felder sind eher optional
public class ItemCreateDTO
{
    [MinLength(1)]
    public required string Name { get; set; }
    public int? Amount { get; set; }
    [MinLength(1)]
    public string? Location { get; set; }
    [MinLength(1)]
    public string? Article_Nr { get; set; }
}