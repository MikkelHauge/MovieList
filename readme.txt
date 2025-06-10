# C# / .NET Eksamensopgave

MovieList – Af Mikkel Hauge, 21S

Idéen bag programmet / programmerne, er at flere brugere kunne have lister af film, som de
gerne vil se.
Så var tanken at man kunne finde andre, at se den samme film med. Film som er ældre, nye
film, fremtidens film. Hvem ved.

Systemet har dog intet ”brugersystem” med login og den slags.

## For at komme i gang

Du har modtaget en .zip fil, som indeholder de 6 mapper, som projektet sammenlagt består
af:
**BusinessLogicLayer**
Denne indeholder Projektets modeller.

**DataAccessLayer**
Indeholder Projektets Context, adgang til databasen.

**MovieList**
Består af projektets WPF program – Systemets primære funktioner.

**MovieListAPI**
En web app, som kører API‘en


**MovieListWebApp**
En web app, som ligesom WPF programmet, kører systemets primære funktioner

**WPF_api_test_app**
En WPF app som tester de forskellige elementer fra web API’en. (WebAPI’en skal køre på
samme tid)

Åben MovieList mappen, og find filen: MovieList.sln
Den vil åbne projektet op, i visual studio.
Find MovieList projektet over i din solution explorer, og find filen ”App.config”

Du skal bruge din computer + SQL express navn, til en connectionString.

<connectionStrings>
<add name="MovieList" connectionString="Data Source=8700K\SQL;Initial Catalog=MovieList;Integrated Security=SSPI;"
providerName="System.Data.SqlClient" />
</connectionStrings>

Erstat 8700K\SQL med det tilsvarende fra din computer. (Computernavn + sql instans
navn(ofte kaldt ”SQLEXPRESS”))

Gør det samme inde i ”MovieListWebApp” projektet, hvor du skal rette connectionString
inde i filen ”Web.config”

Du burde nu kunne køre fx Web Appen eller MovieList.

Jeg vil anbefale du starter med MovieList. Det er i hvert fald den jeg har testet med, og her
burde den initialisere og oprettet lidt startdata i din database (nogle ”User” og ”Movie”
objekter)

Husk:

Kører du API test appen skal du også køre webAPI projektet. De skal altså køre på samme tid.
Du kan selvfølgelig også teste webAPIen via fx postman.
WebAPI’en starter en hjemmeside op, hvor i der er et link til en ”hjælpe” side som viser de
forskellige api kald.

Hele mit projekt er blevet testet på min egen computer, hvor projektet blev oprettet, samt
på Morten Karlsens computer.

Jeg havde problemet med github, fordi visual studio kun uploadede ET ud af de seks
projekter til github. Hvilket er lidt mærkeligt.

Jeg har ligeledes kørt ”tjeklisten” igennem over krav/point for opgaven og jeg synes selv jeg
har været inde over samtlige punkter. Så er spørgsmålet vel, til hvilken grad jeg har klaret de
forskellige.


## WPF Appen

### Generelt om appen

En WPF App med systemets samlede funktionalitet.
Slette, rette, oprette osv.

### Fejl og mangler

Ingen fejl og mangler fundet under min test.
Man kan dog argumentere at appen burde kunne vise hvilke brugere, som har en bestemt
film på deres liste, fremfor bare et tal.
Denne funktion kan dog findes i web appen.

## Web Appen

### Generelt om appen

I web appen er der ligeledes også samtlige funktioner. Slette, redigere og oprette nye
brugere og film.
Der er også mulighed for at se alle brugeres navn, under en film. Altså de brugere som har
filmen på deres liste.

Derudover er der en søge-funktionalitet i toppen af siden.

Og filmplakater bliver hentet via en ekstern (gratis) API (MovieDatabase:
https://rapidapi.com/SAdrian/api/moviesdatabase)
Dette burde fungere, såfremt filmens titel OG releaseyear er korrekt. (Den søger og henter
det første resultat, og bruger det. Så i enkelte tilfælde henter den den forkerte plakat, og
nogle gange finder den ingenting.
Så det hedder fx ikke ”Star Wars” fra 1977. Den hedder jo ”Star Wars: Episode IV - A New
Hope”

### Fejl og mangler

Ingen fejl, så vidt jeg lige har bemærket.
Dog er der enkelte kosmetiske detaljer jeg er lidt ked af.

(bl.a. under en users details, hvor man tilføjer og fjerner film, fra den enkelte brugers liste,
det ser lidt grimt ud. Men jeg har mistet gejsten til at gøre det pænere)

## Web API + API Test app

### Generelt om api’en

I test appen kan man – via api’en - oprette film, slette film, redigere film, oprette brugere,
slette brugere, redigere brugere, samt tilføje og fjerne film, fra en brugers liste over film.

### Fejl og mangler

Ingen fejl og mangler fundet under min test.


