

function MovePawn(squareID, pawnToMove, baseCircleClasses) {

    const square = document.getElementById(squareID);
    const pawn = document.getElementById(pawnToMove);
    const baseCircle = document.getElementsByClassName("square " + baseCircleClasses);

    let squareTop = square.style.top;
    let squareRight = square.style.right;
    let squareBot = square.style.bottom;
    let squareLeft = square.style.left;

    if (square.id == 60) {
        pawn.style.display = "none";
        pawn.style.removeProperty("z-index");
    }

    if (squareTop && squareRight) {
        pawn.style.top = squareTop;
        pawn.style.right = squareRight
        pawn.style.margin = "8px";
        pawn.style.zIndex = 100;
    }
    else if (squareTop && squareLeft) {
        pawn.style.top = squareTop;
        pawn.style.left = squareLeft
        pawn.style.margin = "8px";
        pawn.style.zIndex = 100;
    }
    else if (squareBot && squareLeft) {
        pawn.style.bottom = squareBot;
        pawn.style.left = squareLeft
        pawn.style.margin = "8px";
        pawn.style.zIndex = 100;
        baseCircle[0].style.display = "none";
    }
    else if (squareBot && squareRight) {
        pawn.style.bottom = squareBot;
        pawn.style.right = squareRight
        pawn.style.margin = "8px";
        pawn.style.zIndex = 100;
    }
}



