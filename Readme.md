Name > :metabolon Dokumenten- und Geräteverwaltungs-Software Backend und Datenverarbeitung
Goal > Anlage und Verwaltung von Dokumenten bezogen auf Labore, Arbeitsräume und Gerätschaften vorort, sowie Bestandszählung und Verwaltung von Lagerobjekten und Verwaltung von Nutzerkonten der Studenten bzw. des Personals
Audience > Personal, Studenten und Administratoren des :metabolon Forschungsprojekts
Outcome > Ein funktionierendes Backend, das dazu in der Lage ist Räume und Geräte aufzunehmen, die Dokumente dazu zu verwalten, das Lager dynamisch zu updaten und das Personal dazu zuzuordnen

Tech-Stack > MsSQL Datenbank + Asp.NET Core Systemkern [C#]
RESTful API Design mit Schnittstellen für CRUD Operationen

Core-Features:
<1> Users
<1.1> Auth
<1.1.1> Admins können Nutzer anlegen via E-mail
<1.1.2> Nutzer können via E-mail Token ein Passwort setzen oder generieren lassen
<1.1.3> Nutzer können sich via E-mail und Passwort einloggen
<1.2> Verwaltung
<1.2.1> Admins können Nutzern eine(n) Mentor/Aufsichtsperson zuweisen
<1.2.2> Admins können Nutzern eine Rolle/Rechte zuweisen
<1.2.3> Admins können Nutzern einen Transponder zuweisen sowie das Enddatum des Selben setzen
<1.2.4> Admins können Nutzern die Flag "HatFührerschein" zuweisen
<1.2.5> Sowohl Admins als auch Nutzer können Nutzern Ansichtsnamen zuweisen
<1.2.6> Nutzer können im System angeben, in welchem Raum sie gerade sind, bzw. sich aus der Raumliste austragen
<1.2.7> Admins können Nutzern Räume zuweisen, auf welche diese Zugriff haben

<2> Räume
<2.1> Anlage
<2.1.1> Admins können Räume erstellen via Name
<2.1.2> Admins können Räumen Aufsichtspersonen [User] zuteilen
<2.1.3> Admins können Räumen Geräte [Machine] zuteilen
<2.1.4> Admins können Räumen Material [Item] zuteilen
<2.2> Verwaltung
<2.2.1> Admins und Nutzer können Räumen Dokumente anfügen und diese bearbeiten
<2.2.2> Admins können den Namen von Räumen ändern

<3> Geräte
<3.1> Anlage
<3.1.1> Admins können Geräte erstellen via Name
<3.2> Verwaltung
<3.2.1> Admins können den Namen von Geräten ändern
<3.2.2> Admins können das Datum der nächsten Wartung updaten
<3.2.3> Admins können die Position des Geräts im Raum angeben und ändern
<3.2.4> Admins und Nutzer können Geräten Dokumente anfügen und diese bearbeiten
<3.3> Automatisierung
<3.3.1> Wenn das Wartungs-Datum erreicht ist kann das System Admins eine Warnung schicken
<3.3.2> Wenn das Wartungs-Datum ge-updated wurde setzt das System den Wartungsstatus entsprechend um

<4> Material
<4.1> Anlage
<4.1.1> Admins können Material via Name erstellen
<4.1.2> Admins können die Artikelnummer des Materials zuteilen und verändern
<4.1.3> Admins können die Menge des vorhandenen Materials angeben
<4.2> Verwaltung
<4.2.1> Admins und Nutzer können die Menge des Materials verändern
<4.2.2> Admins können die Position des Materials im Raum angeben und verändern
<4.2.3> Admins können den Namen des Materials verändern

<5> Dokumente
<5.1> Anlage
<5.1.1> Admins und Nutzer können Dokumente via Autor und Name erstellen
<5.1.2> Das System weist je nach Nutzer-Rang den Status "geprüft" zu
<5.1.3> Das System weist je nach Nutzer den Mentor / die Aufsichtsperson zur Prüfung zu
<5.1.4> Das System weist dem Dokument eine Referenz zu, in welcher Sektion das Dokument erstellt wurde
<5.2> Verwaltung
<5.2.1> Admins können den Namen von Dokumenten ändern
<5.2.2> Der Mentor kann den "geprüft" Status verändern oder das Dokument zurückweisen
<5.2.3> Das System setzt wenn "geprüft" verändert wurde das Datum an dem das Dokument angenommen wurde
<5.2.4> Das System weist wenn "geprüft" verändert wurde das Dokument dem Referenzobjekt zu


