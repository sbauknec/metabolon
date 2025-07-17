namespace metabolon.DTOs;

using System.ComponentModel.DataAnnotations;
using metabolon.Models;

public class DocumentDTO
{
    public User? Author { get; set; }
    [MinLength(1)]
    public string? Object_Reference { get; set; }
    public bool? IsApproved { get; set; }
    public User? Supervisor { get; set; }
    public DateTime? ApprovedAt { get; set; }
}