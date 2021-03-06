using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LudoV2Api.Models;
using LudoV2Api.Models.DbModels;
using LudoV2Api.Validations;
using LudoV2Api.Models.ApiRequests;
using Microsoft.AspNetCore.Cors;

namespace LudoV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PawnsController : ControllerBase
    {
        private readonly LudoContext _context;
        private List<string> _turnOrder = new() { "red", "blue", "green", "yellow" };
        private int _sixesRolled = 0;

        private readonly Dictionary<string, int> _pawnBases = new()
        {
            { "red", 0 },
            { "blue", 1 },
            { "green", 2 },
            { "yellow", 3 }
        };


        public PawnsController(LudoContext context)
        {
            _context = context;
        }

        //OPTIONS: api/Pawns/move
        [HttpOptions]
        public IActionResult PreflightRoute()
        {
            return NoContent();
        }

        // GET: api/Pawns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pawn>>> GetPawnSavePoints()
        {
            return await _context.Pawns.ToListAsync();
        }

        // GET: api/Pawns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pawn>> GetPawn(int id)
        {
            var pawn = await _context.Pawns.Where(x => x.Id == id).ToListAsync();

            if (pawn == null)
            {
                return NotFound();
            }

            return pawn.First();
        }

        // GET: api/Pawns/game/5
        [HttpGet("game/{id}")]
        public async Task<ActionResult<IEnumerable<Pawn>>> GetPawnsForGame(int id)
        {
            var pawn = await _context.Pawns.Where(x => x.Game.Id == id).ToListAsync();

            if (pawn == null)
            {
                return NotFound();
            }

            return pawn;
        }

        // PUT: api/Pawns/move
        [HttpPut("move")]
        public async Task<ActionResult<MovePawnReturnRequest>> PutMovePawn(MovePawnRequest pawnRequest)
        {
            var canPlay = ControllerMethods.ValidatingCurrentTurn(_context, pawnRequest.GameId, pawnRequest.TeamColor);

            if (!canPlay)
            {
                return BadRequest("It's not your turn");
            }

            var validateDice = ControllerMethods.ValidateDice(pawnRequest.Dice);

            if (!validateDice)
            {
                return BadRequest("You must roll between 1 and 6");
            }

            int basePosition = _pawnBases[pawnRequest.TeamColor.ToLower()] ;

            var pawnsInBase = _context.Pawns.Where(x => x.Position == basePosition).Count();

            if (pawnsInBase == 4 && pawnRequest.Dice != 6 && pawnRequest.Dice != 1)
            {
                return BadRequest("No pawns on the field to move");
            }

            else if (pawnRequest.Position == basePosition)
            {
                return BadRequest("Use /movefrombase to move pawns from base");
            }

            int newPosition = pawnRequest.Position + pawnRequest.Dice;
            var pawn = await _context.Pawns.FindAsync(pawnRequest.PawnId);

            if (newPosition > 43 && pawnRequest.TeamColor.ToLower() != "red" && pawn.EligibleForWin == false)
            {
                newPosition = 4 + (newPosition - 44);
                pawn.EligibleForWin = true;
            }

            if (pawn.EligibleForWin)
            {
                newPosition = ControllerMethods.PawnSafeZone(newPosition, pawnRequest.TeamColor);
            }

            #region "Knockuot the pawn that exists on the position"

            Pawn existsOnPosition = _context.Pawns.Where(x => x.Position == newPosition).FirstOrDefault();

            int pawnExistsOnPosition = ControllerMethods.PawnExistsOnPosition(existsOnPosition, pawnRequest.TeamColor, pawn.Id, newPosition);
            
            if (pawnExistsOnPosition == -1)
            {
                return BadRequest("You can't have two pawns at the same position");
            }
            else if (pawnExistsOnPosition > 0)
            {
                existsOnPosition.Position = pawnExistsOnPosition;
            }
            #endregion

            pawn.Position = newPosition;

            var game = await _context.Games.FindAsync(pawnRequest.GameId);

            #region Check if player won and updates the games table
            var pawns = await _context.Pawns.Where(x => x.Game.Id == pawnRequest.GameId && x.Color == pawnRequest.TeamColor && x.Position == 60).ToListAsync();

            if (pawns.Count > 0)
            {
                if (pawns.Count == 4)
                {
                    if (string.IsNullOrWhiteSpace(game.FirstPlace))
                    {
                        game.FirstPlace = pawnRequest.TeamColor;
                    }
                    else if (string.IsNullOrWhiteSpace(game.SecondPlace))
                    {
                        game.SecondPlace = pawnRequest.TeamColor;

                    }
                    else if (string.IsNullOrWhiteSpace(game.ThirdPlace))
                    {
                        game.ThirdPlace = pawnRequest.TeamColor;

                    }
                    else if(string.IsNullOrWhiteSpace(game.FourthPlace))
                    {
                        game.FourthPlace = pawnRequest.TeamColor;

                    }
                    _context.SaveChanges();

                    int index = ControllerMethods.NextTurn(_turnOrder.IndexOf(pawnRequest.TeamColor), game.NumberOfPlayers);

                    MovePawnReturnRequest movedPawn = new();

                    movedPawn.PawnPosition = pawn.Position;
                    movedPawn.CurrentTurn = _turnOrder[index];
                    movedPawn.KnockedPawnPosition = -1;
                }
            }
            #endregion

            #region Remove Color from turn index when the color has won

            if (!string.IsNullOrWhiteSpace(game.FirstPlace))
            {
               var turn = _turnOrder.IndexOf(game.FirstPlace.ToLower());
                _turnOrder.RemoveAt(turn);
            }
            else if (!string.IsNullOrWhiteSpace(game.SecondPlace))
            {
                var turn = _turnOrder.IndexOf(game.SecondPlace.ToLower());
                _turnOrder.RemoveAt(turn);
            }
            else if (!string.IsNullOrWhiteSpace(game.ThirdPlace))
            {
                var turn = _turnOrder.IndexOf(game.ThirdPlace.ToLower());
                _turnOrder.RemoveAt(turn);
            }
            else if (!string.IsNullOrWhiteSpace(game.FourthPlace))
            {
                var turn = _turnOrder.IndexOf(game.FourthPlace.ToLower());
                _turnOrder.RemoveAt(turn);
            }
            #endregion

            #region Dice rolled 6
            if (pawnRequest.Dice == 6)
            {
                _sixesRolled++;
            }

            if (_sixesRolled >= 2 || _sixesRolled == 0)
            {
                int index = ControllerMethods.NextTurn(_turnOrder.IndexOf(pawnRequest.TeamColor), game.NumberOfPlayers);

                game.CurrentTurn = _turnOrder[index];
                _sixesRolled = 0;
            }
            else if (_sixesRolled > 0)
            {
                game.CurrentTurn = pawnRequest.TeamColor;
            }
            #endregion

            _context.SaveChanges();

            MovePawnReturnRequest movedPawns = new();

            movedPawns.PawnPosition = pawn.Position;
            movedPawns.CurrentTurn = game.CurrentTurn;
            movedPawns.KnockedPawnPosition = -1;


            if (existsOnPosition != null && existsOnPosition.Position != 60)
            {
                movedPawns.KnockedPawnPosition = existsOnPosition.Position;
            }

            return Ok(movedPawns);
        }

        // PUT: api/Pawns/moveformbase
        [HttpPut("movefrombase")]
        public async Task<ActionResult<MovePawnReturnRequest>> PutPawnFromBase(MovePawnRequest pawnRequest)
        {

            Dictionary<string, int> pawnStartPosition = new()
            {
                { "red", 4 },
                { "blue", 14 },
                { "green", 24 },
                { "yellow", 34 }
            };

            MovePawnReturnRequest returnData = new();

            var canPlay = ControllerMethods.ValidatingCurrentTurn(_context, pawnRequest.GameId, pawnRequest.TeamColor);

            if (!canPlay)
            {
                return BadRequest("Not your turn");
            }

            var validateDice = ControllerMethods.ValidateDice(pawnRequest.Dice);

            if (!validateDice)
            {
                return BadRequest("You must roll between 1 and 6");
            }

            var pawnToMove = await _context.Pawns.FindAsync(pawnRequest.PawnId);

            if (pawnRequest.Dice == 1)
            {
                pawnToMove.Position = pawnStartPosition[pawnToMove.Color.ToLower()];
                var game = await _context.Games.FindAsync(pawnRequest.GameId);

                int index = ControllerMethods.NextTurn(_turnOrder.IndexOf(pawnRequest.TeamColor), game.NumberOfPlayers);

                game.CurrentTurn = _turnOrder[index];

                #region "Knockuot the pawn that exists on the position"

                Pawn existsOnPosition = _context.Pawns.Where(x => x.Position == pawnToMove.Position).FirstOrDefault();

                int pawnExistsOnPosition = ControllerMethods.PawnExistsOnPosition(existsOnPosition, pawnRequest.TeamColor, pawnToMove.Id, pawnToMove.Position);


                if (pawnExistsOnPosition == -1)
                {
                    return BadRequest("You can't have two pawns at the same position");
                }
                else if (pawnExistsOnPosition > 0)
                {
                    existsOnPosition.Position = pawnExistsOnPosition;
                }
                #endregion

                await _context.SaveChangesAsync();

                returnData.CurrentTurn = game.CurrentTurn;
                returnData.PawnPosition = pawnToMove.Position;

                return Ok(returnData);
            }
            else if (pawnRequest.Dice == 6)
            {
                _sixesRolled++;
                pawnToMove.Position = pawnStartPosition[pawnToMove.Color.ToLower()] + 5;
                var game = await _context.Games.FindAsync(pawnRequest.GameId);

                #region "Knockuot the pawn that exists on the position"
                Pawn existsOnPosition = _context.Pawns.Where(x => x.Position == pawnToMove.Position).FirstOrDefault();

                int pawnExistsOnPosition = ControllerMethods.PawnExistsOnPosition(existsOnPosition, pawnRequest.TeamColor, pawnToMove.Id, pawnToMove.Position);


                if (pawnExistsOnPosition == -1)
                {
                    return BadRequest("You can't have two pawns at the same position");
                }
                else if (pawnExistsOnPosition > 0)
                {
                    existsOnPosition.Position = pawnExistsOnPosition;
                }
                #endregion

                if (_sixesRolled < 2)
                {
                    int index = ControllerMethods.NextTurn(_turnOrder.IndexOf(pawnRequest.TeamColor), game.NumberOfPlayers);

                    game.CurrentTurn = _turnOrder[index];
                    _sixesRolled = 0;
                }
                else
                {
                    game.CurrentTurn = pawnRequest.TeamColor;
                }

                await _context.SaveChangesAsync();

                returnData.CurrentTurn = game.CurrentTurn;
                returnData.PawnPosition = pawnToMove.Position;

                return Ok(returnData);
            }
            else
            {
                var game = await _context.Games.FindAsync(pawnRequest.GameId);

                int index = ControllerMethods.NextTurn(_turnOrder.IndexOf(pawnRequest.TeamColor), game.NumberOfPlayers);

                game.CurrentTurn = _turnOrder[index];

                await _context.SaveChangesAsync();

                returnData.CurrentTurn = game.CurrentTurn;

                return Ok(returnData);

                //return BadRequest("You can't move any pawns from base");
            }
        }

        // POST: api/Pawns
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pawn>> PostPawn(Pawn pawn)
        {
            _context.Pawns.Add(pawn);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPawn", new { id = pawn.Id }, pawn);
        }

        private bool PawnExists(int id)
        {
            return _context.Pawns.Any(e => e.Id == id);
        }
    }
}
