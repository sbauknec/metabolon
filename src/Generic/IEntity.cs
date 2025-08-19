namespace metabolon.Generic;

public interface IEntity
{
    int Id { get; set; }

    //Archivierung statt Löschen
    bool IsDeleted { get; set; }
    DateOnly? DeletedOn { get; set; }
}