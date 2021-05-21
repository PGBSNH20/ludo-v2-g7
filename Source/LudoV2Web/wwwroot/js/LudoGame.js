
function PawnMoveHelper(p, bC) {
    p.style.margin = "8px";
    p.style.zIndex = 100;
    bC.style.display = "none";
}

function MovePawn(squareID, pawnToMove, baseCircleClasses) {

    const square = document.getElementById(squareID);
    const pawn = document.getElementsByClassName(pawnToMove);
    const baseCircle = document.getElementsByClassName("basePawn " + baseCircleClasses);

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
    document.getElementById("pawnColorValue").value = event.className.split(' ')[0];
    document.getElementById("pawnPositionValue").value = event.className.split(' ')[3];
    document.getElementById("pawnIdValue").value = event.id;

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

    if (dice != 0 && pawnColor != 0 && pawnPosition != 0) {
        console.log("yes");
        submitButton.disabled = false;
    }
}

$(document).ready(function () {
    $("div.pawn").click(function () {
        GetPawnValues(event.target);
    });
    $("div.basePawn").click(function () {
        GetPawnValues(event.target);
    });
});

