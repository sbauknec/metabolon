namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;

public class ItemDTO
{
    [MinLength(1)]
    public required string Name { get; set; }
    public int Amount { get; set; }
    [MinLength(1)]
    public string? Location { get; set; }
    [MinLength(1)]
    public string? Article_Nr { get; set; }
}