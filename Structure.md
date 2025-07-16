Generelle Dateienstruktur des Projektes<br>
Metabolon [root]<br>
├── Dokumentation/                  (Technische Daten, Diagramme, Nutzerhinweise und Handbuch)<br>
├── src/<br>
│   ├── Controllers/                (WebAPI Endpunkte benannt nach dem DB Set, das dieser primär verändert)<br>
│   ├── DTOs/                       ([D]ata [T]ransfer [O]bjects | Mappable Komponenten, die den Empfang von Daten aus dem Frontend erleichtert)<br>
│   ├── Generic/                    (Generische Komponenten zur Wiederverwendung, DoNotTouch es sei denn es ist kritisch)<br>
│   ├── Migrations/                 (Logs der vergangenen Migrationsausführungen, in denen die Struktur der Datenbank verändert wurde)<br>
│   ├── Models/                     (Objektklassen/Datenhaltungsklassen, die für die Einspeisung in die Datenbank optimiert sind)<br>
│   ├── obj/                        (Klassen und Settings für die Module des Systems)<br>
│   ├── Properties/                 (Starteinstellungen für das System)<br>
│   ├── Services/                   (Modulerweiterungen für Services bspw. E-mail Funktionalität)<br>
│   ├── AppDbContext.cs             (DBSets für die Datenbankanbindung)<br>
│   ├── appsettings.json            (Und appsettings.Development.json, Laufzeiteinstellungen, automatisch generiert, in Ruhe lassen)<br>
│   ├── MapperProfile.cs            (Mapping-Profile für die Übertragung von DTO auf Model und vice versa)<br>
│   ├── metabolon.csproj            (Gradle-einstellungen, registriert die .NET Version des Projekts und alle erforderlichen Module, nur im Härtefall anfassen)<br>
│   ├── metabolon.http              (Host- und Porteinstellungen)<br>
│   ├── metabolon.sln               (Automatisch erstellt, da bin ich mir selber nicht so sicher was das eigentlich ist, in Ruhe lassen)<br>
│   ├── Program.cs                  (Startdatei des Systems, Service-Initialisierung, Datenbankanbindung, SwaggerUI zur Dokumentation und für Tests wird initialisiert etc.)<br>
├── Readme.md                       (Mission Statement, Feature-Liste, Tech Stack)<br>
├── Structure.md                    (Diese Datei :>)<br>
