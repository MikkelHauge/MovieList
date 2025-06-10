Her er teksten jeg sendte med, til afleveringen af opgaven, som beskriver lidt opsætning, så underviseren kan køre det på deres computer, samt lidt andet info så som fejl og mangler.
Dette er en eksamensopgave fra 4. semester på datamatikeruddannelsen.

Jeg fik 10 for denne opgave.

# C# / .NET Eksamensopgave

**MovieList – Af Mikkel Hauge, 21S**

MovieList er et system hvor flere brugere kan have lister over film de gerne vil se. Tanken er, at man kan finde nogen at se en bestemt film med – uanset om det er en gammel klassiker eller en ny film.  
Der er ikke noget brugersystem med login og adgangskontrol.

---

## Kom i gang

Du har modtaget en .zip-fil med følgende 6 projekter:

- **BusinessLogicLayer**  
  Indeholder projektets modeller
- **DataAccessLayer**  
  Indeholder `DbContext` og databaseadgang
- **MovieList**  
  WPF-programmet – systemets primære grænseflade
- **MovieListAPI**  
  Web API-applikation
- **MovieListWebApp**  
  Web app med funktionalitet svarende til WPF-programmet
- **WPF_api_test_app**  
  WPF-app til test af API'et (API’en skal være i gang samtidig)

---

## Opsætning

1. Åbn `MovieList`-mappen  
2. Dobbeltklik på `MovieList.sln` for at åbne hele løsningen i Visual Studio  
3. Find `App.config` i `MovieList`-projektet

Rediger connection string til at matche din lokale SQL Express instans. Eksempel:

```xml
<connectionStrings>
  <add name="MovieList" connectionString="Data Source=8700K\SQL;Initial Catalog=MovieList;Integrated Security=SSPI;" providerName="System.Data.SqlClient" />
</connectionStrings>
```

Erstat `8700K\SQL` med dit eget computernavn og instansnavn (typisk `SQLEXPRESS`).  

Gør det samme i `Web.config` i `MovieListWebApp`.

---

## Anbefalet start

Start med at køre `MovieList`-projektet. Det vil forsøge at initialisere databasen med testdata (nogle `User` og `Movie` objekter).

> Hvis du vil bruge test-appen, skal både API’et og test-appen køre samtidig.  
> Alternativt kan API’et testes med fx Postman.  
> WebAPI’en viser en hjælpeside med overblik over endpoints.

---

## Kvalitetssikring

Projektet er testet på:

- Min egen computer (udviklingsmiljø)  
- Morten Karlsens computer (ekstern test)

Der var problemer med GitHub, hvor kun ét af seks projekter blev uploadet, hvilket Visual Studio forårsagede.

Jeg har gennemgået opgavens krav og vurderer, at jeg har dækket samtlige punkter.  
Hvor godt hvert punkt er løst, må vurderes af censor.

---

## WPF App

### Funktionalitet

WPF-appen understøtter fuld CRUD på brugere og film.

### Fejl og mangler

Ingen funktionelle fejl fundet.  
Dog kunne det være bedre, hvis appen viste hvilke brugere der har en bestemt film – den viser kun antal.  
Denne funktion findes i Web Appen.

---

## Web App

### Funktionalitet

Webappen tilbyder også fuld CRUD-funktionalitet.  
Der vises brugernavne for hver film, og der er en søgefunktion i toppen af siden.

Filmplakater hentes via en ekstern API:

[https://rapidapi.com/SAdrian/api/moviesdatabase](https://rapidapi.com/SAdrian/api/moviesdatabase)

API’en søger efter titel og udgivelsesår, og henter det første match.  
Fejlkilder: Forkerte plakater eller ingen resultater ved upræcis titel/år.

Eksempel:  
Den originale "Star Wars" (1977) skal søges som **"Star Wars: Episode IV - A New Hope"**.

### Fejl og mangler

Ingen funktionelle fejl.  
Der er nogle kosmetiske ting, f.eks. visning af film i en brugers detaljer, som ser lidt rodet ud. Jeg prioriterede funktionalitet frem for design her.

---

## Web API + Test App

### Funktionalitet

API’en understøtter:

- CRUD på film og brugere  
- Tilføj/fjern film til/fra brugeres lister

Dette kan testes via:

- WPF testappen (kræver at API’et kører)  
- Postman  
- WebAPI’ens dokumentationsside

### Fejl og mangler

Ingen fundet under test.
