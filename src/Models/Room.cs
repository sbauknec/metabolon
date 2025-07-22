namespace metabolon.Models;

using metabolon.DTOs;
using metabolon.Generic;

public class Room : IEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Supervisor_Id { get; set; }

    //FK zu Geräten, Material und Dokumenten existiert auf der MANY seite bzw. in einer JOIN Tabelle
    //TODO: Generate new DBSet Diagram and implement eager loading
}   