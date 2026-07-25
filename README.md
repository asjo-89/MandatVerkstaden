# ValKvoten

ValKvoten är en webbapplikation för laborering och analys av resultat vid kommunval. 
- Analysera resultatet innan det färdiga valresultatet kommit för att se hur mandat kan komma att fördelas. 
- Sortera partier i olika partigrupper och laborera med olika värden för att se hur mandatfördelningen förändras.
- Spara dina analyser antingen som PDF eller genom att skapa ett konto.

## Teknikstack
React/Vite med Vanilla JavaScript för frontend
ASP.NET Web Api
ASP.NET Core / .Net 10
Entity Framework Core
SQL Server Express

## Tabeller
- Elections = valets datum.
- Municipalitys = kommun.
- PoliticalPartys = politiskt parti.
- ElectionResults = original valresultat.
- CouncilSeatAllocations = originalversion av platstilldelning i kommunfullmäktige.
- CouncilSeatAllocationsScenarios = scenarier som användaren skapat och valt att spara gällande platstilldelning i kommunfullmäktige.
- Users = information om användare.

