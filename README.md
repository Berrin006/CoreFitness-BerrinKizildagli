CoreFitness Web Portal
CoreFitness är en modern webbportal för ett gym, utvecklad som en del av kursen Programmering med ASP.NET 1. Projektet är byggt med .NET 10 och följer principerna för Clean Architecture och Domain-Driven Design (DDD) för att skapa en robust, säker och skalbar lösning.

🚀 Funktioner
Användarkonto: Registrering, inloggning och utloggning via ASP.NET Core Identity.

Tredjepartsinloggning: Integration med GitHub för snabbare autentisering.

Medlemskap: Inloggade användare kan teckna medlemskap (Silver/Guld) och se sin status.

Klassbokning: Komplett system för att visa tillgängliga träningspass, boka och avboka.

Admin-funktionalitet: Administratörer kan skapa och radera träningspass direkt via gränssnittet.

Profilhantering: Användare kan uppdatera sina personuppgifter eller radera sitt konto (GDPR-vänligt).

🏗 Arkitektur & Designmönster
Lösningen är strikt uppdelad i lager för att separera ansvar:

Domain: Innehåller kärnlogik, Aggregates och Domain Exceptions.

Application: Hanterar tjänster (Services) och Result Pattern för felhantering.

Infrastructure: Implementerar datalagring med EF Core och Identity.

Presentation: En ASP.NET Core MVC-applikation med kebab-case routing och dynamisk menynavigering.

Använda mönster:
Repository Pattern: Generisk bas för databasåtkomst.

Unit of Work: (Via EF Core Context).

Dependency Injection: För samtliga tjänster och repositories.

Result Pattern: För att hantera operationers utfall utan onödiga exceptions.

🛠 Teknikstack
Framework: .NET 10 (ASP.NET Core MVC)

ORM: Entity Framework Core 10

Databas: * Produktion: SQL Server

Tester/Utveckling: SQLite / InMemory

Frontend: Razor Views, CSS, JavaScript

🏁 Kom igång lokalt
Förutsättningar
.NET 10 SDK

SQL Server (LocalDB fungerar utmärkt)

Installation
Klona repot:

Bash
git clone https://github.com/Berrin006/CoreFitness-BerrinKizildagli
Konfigurera databasen:
Kontrollera appsettings.json så att ConnectionString stämmer överens med din lokala miljö.

Uppdatera databasen:

Bash
dotnet ef database update --project Infrastructure --startup-project Presentation.WebApp
Kör projektet:

Bash
dotnet run --project Presentation.WebApp
🧪 Testning
Projektet innehåller ett xUnit-testprojekt för både enhets- och integrationstester. Integrationstesterna använder en SQLite-konfiguration i minnet för att verifiera databasinteraktioner.

Kör alla tester med:

Bash
dotnet test
👤 Admin-åtkomst
Vid första uppstart seedas en administratör automatiskt baserat på e-postadressen definierad i Program.cs (berrin.kizildagli@yh.nackademin.se). Logga in med detta konto (efter registrering) för att få tillgång till admin-verktygen för träningspass.
