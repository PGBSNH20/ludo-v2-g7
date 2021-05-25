
function PawnMoveHelper(basePawn, baseSquare) {
    basePawn.style.margin = "8px";
    basePawn.style.zIndex = 100;
    if (baseSquare.style.display != "none") {
        baseSquare.style.display = "none";
    }
}

function MovePawn(squareID, pawnToMove, baseCircleClasses) {
    if (squareID == 0) {
        return;
    }
    const square = document.getElementById(squareID);
    const pawn = document.getElementsByClassName(pawnToMove);
    const baseCircle = document.getElementsByClassName(baseCircleClasses + " basePawn");

    let squareTop = square.style.top;
    let squareRight = square.style.right;
    let squareBot = square.style.bottom;
    let squareLeft = square.style.left;

    if (square.id == 60) {
        pawn[0].style.display = "none";
        pawn[0].style.removeProperty("z-index");
    }

    if (squareTop && squareRight) {
        pawn[0].style.bottom = null;
        pawn[0].style.left = null;

        pawn[0].style.top = squareTop;
        pawn[0].style.right = squareRight
        pawn[0].classList.add(squareID);
        PawnMoveHelper(pawn[0], baseCircle[0]);
    }
    else if (squareTop && squareLeft) {
        pawn[0].style.bottom = null;
        pawn[0].style.right = null;

        pawn[0].style.top = squareTop;
        pawn[0].style.left = squareLeft
        pawn[0].classList.add(squareID);
        PawnMoveHelper(pawn[0], baseCircle[0]);
    }
    else if (squareBot && squareLeft) {
        pawn[0].style.top = null;
        pawn[0].style.right = null;

        pawn[0].style.bottom = squareBot;
        pawn[0].style.left = squareLeft
        pawn[0].classList.add(squareID);
        PawnMoveHelper(pawn[0], baseCircle[0]);
    }
    else if (squareBot && squareRight) {
        pawn[0].style.top = null;
        pawn[0].style.left = null;

        pawn[0].style.bottom = squareBot;
        pawn[0].style.right = squareRight
        pawn[0].className = squareID;
        PawnMoveHelper(pawn[0], baseCircle[0]);
    }
}

function GetPawnValues(event) {
    let baseColorAndPosition = event.className.split(' ')[2];
    let basePosition = baseColorAndPosition.charAt(baseColorAndPosition.length - 1);

    document.getElementById("pawnColorValue").value = event.className.split(' ')[0];
    document.getElementById("pawnPositionValue").value = event.className.split(' ')[3];
    document.getElementById("pawnIdValue").value = event.id.split("_")[0];
    document.getElementById("pawnBasePosition").value = "square" + basePosition;

    if (event.classList.contains("basePawn")) {
        document.getElementById("isBase").value = 1;
    }

    RemoveDisableFromButton();
}

function DiceRoll() {
    let randNumber = Math.floor(Math.random() * (7 - 1) + 1);
    
    document.getElementById("diceRoll").textContent = "Dice roll: " + randNumber;
    document.getElementById("diceValue").value = randNumber;
    document.getElementById("diceButton").disabled = true;
    
    RemoveDisableFromButton();
}

function RemoveDisableFromButton() {
    const pawnColor = document.getElementById("pawnColorValue").value;
    const pawnPosition = document.getElementById("pawnPositionValue").value;
    const dice = document.getElementById("diceValue").value;
    const submitButton = document.getElementById("movePawnButton");

    if (dice != 0 && pawnColor != 0 && pawnPosition != -1) {
        submitButton.disabled = false;
    }
}

async function PostLudoData() {
    const diceRoll = document.getElementById("diceValue").value;
    const pawnId = document.getElementById("pawnIdValue").value
    const position = document.getElementById("pawnPositionValue").value
    const teamColor = document.getElementById("pawnColorValue").value
    const gameId = document.getElementById("gameIdValue").value
    const pawn = document.getElementById(pawnId);
    const baseCircleClasses = teamColor + " " + "pawn" + pawn.className.split(" ")[2].slice(-1);
    const isBasePawn = document.getElementById("isBase").value;

    const putData = { Dice: diceRoll, PawnId: pawnId, Position: position, TeamColor: teamColor, GameId: gameId }

    if (isBasePawn == 0) {
        await fetch("https://localhost:5001/api/Pawns/move", {
            method: "PUT",
            headers: {
                "content-Type": "application/json"
            },
            body: JSON.stringify(putData)
        }).then(response => response.json().then(data => {
            if (data.knockedPawnPosition < 0) {
                signalrMove(data.pawnPosition, pawn.className.split(" ")[2], baseCircleClasses, data.currentTurn);
                return;
            }
        }));
    } else if (isBasePawn == 1) {
        await fetch("https://localhost:5001/api/Pawns/movefrombase", {
            method: "PUT",
            headers: {
                "content-Type": "application/json"
            },
            body: JSON.stringify(putData)
        }).then(response => response.json().then(data => {
                signalrMove(data.pawnPosition, pawn.className.split(" ")[2], baseCircleClasses, data.currentTurn);
                return;
        }));
    }
}

function ResetPawnValuesAndDiceAndUpdateCurrentTurn(currentTurn) {
    document.getElementById("pawnColorValue").value = 0;
    document.getElementById("pawnPositionValue").value = 0;
    document.getElementById("pawnIdValue").value = 0;
    document.getElementById("pawnBasePosition").value = 0;
    document.getElementById("diceRoll").textContent = "Dice roll: ";
    document.getElementById("diceValue").value = 0;
    document.getElementById("isBase").value = 0;

    document.getElementById("diceButton").disabled = false;
    document.getElementById("movePawnButton").disabled = true;

    document.getElementById("cuttentTurn").innerHTML = "Current turn " + currentTurn;

}

$(document).ready(function () {
    $("div.pawn").click(function () {
        GetPawnValues(event.target);
    });
    $("div.basePawn").click(function () {
        GetPawnValues(event.target);
    });
});



let connection = new signalR.HubConnectionBuilder().withUrl("/Ludo").build();


connection.on("Move", function (positionValue, pawnToMoveValue, pawnBaseValue, currentTurn) {
    
    let position = positionValue;
    let pawnToMove = pawnToMoveValue;
    let pawnBasePosition = pawnBaseValue;

    MovePawn(position, pawnToMove, pawnBasePosition);
    ResetPawnValuesAndDiceAndUpdateCurrentTurn(currentTurn);
});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.log(err.toString());
});

function signalrMove(positionValue, pawnToMoveValue, pawnBaseValue, currentTurn) {

    const title = document.getElementById("title").textContent;

    connection.invoke("MovePawns", title, positionValue, pawnToMoveValue, pawnBaseValue, currentTurn).catch(function (err) {
        return console.error(err.toString());
    });
};