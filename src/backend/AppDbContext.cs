using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;
namespace metabolon.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //Hier kommen die DbSets hin - Jede Entity, sprich jede Tabelle in der Datenbank, bekommt ein DbSet
    //Dies ermöglicht die Kommunikation von Objekten im System zu Records in der Datenbank
    //Und ermöglicht dem System die Übertragung von Objekten in Records mit gleichen (gleichnamigen) Attributfeldern
    // { get; set; } bedeutet hier, dass das System in der Lage ist records in der Datenbank anzulegen, zu verändern und zu lesen

    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<documents_devices> Documents_Devices { get; set; }
    public DbSet<documents_rooms> Documents_Rooms { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    //Middleware-Funktion die immer dann ausgeführt wird wenn eine Record aus der Datenbank ins System geht, egal zu welchem Zweck
    //Hier wird gefiltert, um die "Soft-Deletion" zu verwirklichen:
    //Wenn eine Record "Deletet" wird, wird stattdessen eine Flag "IsDeleted" gesetzt, quasi "archiviert"
    //Wenn eine Liste gerufen wird, die nicht explizit das "Archiv" aufruft, wird hier diese Liste gefiltert, und alle Records mit "IsDeleted" auf true werden vor übergabe verworfen, sprich werden in Listenansicht nicht angezeigt
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Room>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<Device>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<Item>().HasQueryFilter(i => !i.IsDeleted);
        modelBuilder.Entity<Document>().HasQueryFilter(dc => !dc.IsDeleted);
        modelBuilder.Entity<documents_devices>().HasQueryFilter(dd => !dd.IsDeleted);
        modelBuilder.Entity<documents_rooms>().HasQueryFilter(dr => !dr.IsDeleted);
    }
    
}