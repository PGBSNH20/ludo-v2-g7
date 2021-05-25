# Documentation



## User Endpoints


```
GameEndpoints:

GET: api/Games
Listar alla spel som finns.

GET: api/Games/Id
Hämtar spelet med de specifika id man skriver in.

PUT: api/Games/Id
Uppdaterar spelet med det specifika id.

POST: api/Games
Skapar upp ett nytt spel.

----------------------------------------------------------------------------

PawnEndpoints:

GET: api/Pawns/Id
Hämtar spelpjäs med specifikt id.

GET: api/Pawns/Game/Id
Hämtar alla spelpjäser för ett specifikt spel.

PUT: api/Pawns/Move
Flyttar en specifik spelpjäs och knffar ut en annan om det behövs.

PUT: apu/Pawns/movefrombase
Flyttar ut en spelpjäs från startposition.

----------------------------------------------------------------------------

## Frontend

Vi använder RazorPages, HTML, JavaScript, css, JQuery och SignalR.  
    
```



Här ser vi hur våran Fiabräda är uppbyggd. Varje nummer är en position.

![image](https://user-images.githubusercontent.com/70013388/118240090-dd8e8400-b49a-11eb-8fc9-409bfd055448.png)



