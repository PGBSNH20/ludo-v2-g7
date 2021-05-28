## Responsivitet med CSS

Det som redan är skapat utav ifrån Razor pages när man gör ett .net core webb applikation är redan  
responsivt så det ända som behövdes göra var att göra spelplanen och speläserna responsiva

## Media queries
De media queries som används för att göra det responsivt är dem här under:
```
/* Small devices width  min-width 320px*/
@media only screen and (min-width: 320px){
    .pawn {
        width: 15px;
        height: 15px;
        margin: 2px !important;
    }
}
/* Small devices width  min-width 375px*/
@media only screen and (min-width: 375px) {
    .pawn {
        width: 18px;
        height: 18px;
        margin: 3px !important;
    }
}

/* Small devices width  min-width 425px*/
@media only screen and (min-width: 425px) {
    .pawn {
        width: 20px;
        height: 20px;
    }
}

/* Small devices width  min-width 520px*/
@media only screen and (min-width: 520px) {
    .pawn {
        width: 25px;
        height: 25px;
        margin: 4px !important;
    }
}
```
## 1:1 aspect ratio för spel planen
I media queryn nedanför så har vi gjort så att spelplanen är en 1:1 aspect ratio
```
/* Extra small devices (phones, 600px and down) */
@media only screen and (max-width: 600px) {
    .gameContainer{
        position: relative;
        width: 100%;
    }
    .game {
        width: auto;
        height: auto;
    }
    .game::before{
        float: left;
        padding-top: 100%;
        content: "";
    }
    .game div{
        float:left;
    }
}

/* Small devices (portrait tablets and large phones, 600px and up) */
@media only screen and (min-width: 600px) {
    .game {
        width: 500px;
        height: 500px;
    }

    .pawn {
        margin: 4px !important;
    }
}
/* Medium devices (landscape tablets, 768px and up) */
@media only screen and (min-width: 768px) {
    .game {
        width: 700px;
        height: 700px;
    }
    .pawn {
        height: 30px;
        width: 30px;
        margin: 8px !important;
    }
}
```
