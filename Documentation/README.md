# Documentation


##Api

### User Endpoints


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

```

## Exempel på begäran
```
https://localhost:44347/api/Games 
```

Det här är ett exempel på vad som kan skickas med i HTTP request bodyn  
```
    {
      "gameName": "Hejhej",
      "numberOfPlayers": 4,
      "currentTurn": "red"
    }
```


## Exempel på svar:  

Response body
```
    {
      "id": 2,
      "gameName": "Hejhej",
      "numberOfPlayers": 4,
      "currentTurn": "red",
      "firstPlace": null,
      "secondPlace": null,
      "thirdPlace": null,
      "fourthPlace": null,
      "lastSaved": "0001-01-01T00:00:00"
    }
```
Response headers
```
    content-type: application/json; charset=utf-8 
    date: Tue,25 May 2021 14:13:33 GMT 
    location: https://localhost:44347/api/Games/2 
    server: Microsoft-IIS/10.0 
    x-powered-by: ASP.NET 
```



## Frontend

Vi använder RazorPages, HTML, JavaScript, css, JQuery och SignalR.

Läs mer om frontend dokumentationen [här](Frontend.md)
    
### Responsivitet
Vi använder media queries för att göra websidan responsiv. 

Läs mer om det [här](VG_Responsive.md)



Här ser vi hur våran Fiabräda är uppbyggd. Varje nummer är en position.

![image](https://user-images.githubusercontent.com/70013388/118240090-dd8e8400-b49a-11eb-8fc9-409bfd055448.png)



