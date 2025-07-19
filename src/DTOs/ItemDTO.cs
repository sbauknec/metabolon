namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;

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

public class ItemQueryDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Amount { get; set; }
    public string? Location { get; set; }   
}