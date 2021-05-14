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
namespace LudoV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PawnsController : ControllerBase
    {
        private readonly LudoContext _context;
        private List<string> _turnOrder = new() { "Red", "Blue", "Green", "Yellow" };
        private int _sixesRolled = 0;

        private Dictionary<string, int> _pawnBases = new()
        {
            { "Red", 0 },
            { "Blue", 1 },
            { "Green", 2 },
            { "Yellow", 3 }
        };


        public PawnsController(LudoContext context)
        {
            _context = context;
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
            var pawn = await _context.Pawns.FindAsync(id);

            if (pawn == null)
            {
                return NotFound();
            }

            return pawn;
        }

        // PUT: api/Pawns/move
        [HttpPut("move")]
        public async Task<IActionResult> PutMovePawn(int dice, int id, int position, string teamColor, int gameId)
        {

            var canPlay = ControllerMethods.ValidatingCurrentTurn(_context, gameId, teamColor);

            if (!canPlay)
            {
                return BadRequest("It's not your turn");
            }

            var validateDice = ControllerMethods.ValidateDice(dice);

            if (!validateDice)
            {
                return BadRequest("You must roll between 1 and 6");
            }

            int basePosition = _pawnBases[teamColor];

            var pawnsInBase = _context.Pawns.Where(x => x.Position == basePosition).Count();

            if (pawnsInBase == 4 && dice != 6 && dice != 1)
            {
                return BadRequest("No pawns on the field to move");
            }

            else if (position == basePosition)
            {
                return BadRequest("Use /movefrombase to move pawns from base");
            }

            int newPosition = position + dice;
            var pawn = await _context.Pawns.FindAsync(id);

            if (newPosition > 43 && teamColor != "Red" && pawn.EligibleForWin == false)
            {
                newPosition = 4 + (newPosition - 44);
                pawn.EligibleForWin = true;
            }

            if (pawn.EligibleForWin)
            {
                newPosition = ControllerMethods.PawnSafeZone(newPosition, teamColor);
            }

            Pawn existsOnPosition = _context.Pawns.Where(x => x.Position == newPosition).FirstOrDefault();

            if (existsOnPosition != null)
            {
                Pawn knockedOutPosition = ControllerMethods.KockOutPawn(teamColor, existsOnPosition, newPosition);

                if (knockedOutPosition.Position == -2)
                {
                    return BadRequest("You can't have two pawns at the same position");
                }
                else if (knockedOutPosition.Position >= 0)
                {
                    existsOnPosition.Position = knockedOutPosition.Position;
                }
            }

            pawn.Position = newPosition;

            var game = await _context.Games.FindAsync(gameId);

            if (dice == 6)
            {
                _sixesRolled++;
            }

            if (_sixesRolled >= 2 || _sixesRolled == 0)
            {
                int index = _turnOrder.IndexOf(teamColor);

                if (index + 1 > game.NumberOfPlayers - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }

                game.CurrentTurn = _turnOrder[index];
                _sixesRolled = 0;
            }
            else if (_sixesRolled > 0)
            {
                game.CurrentTurn = teamColor;
            }

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("movefrombase")]
        public async Task<IActionResult> PutPawnFromBase(int gameId, int pawnId, int dice, string teamColor)
        {

            Dictionary<string, int> pawnStartPosition = new()
            {
                { "Red", 4 },
                { "Blue", 14 },
                { "Green", 24 },
                { "Yellow", 34 }
            };

            var canPlay = ControllerMethods.ValidatingCurrentTurn(_context, gameId, teamColor);

            if (!canPlay)
            {
                return BadRequest("Not your turn");
            }

            var validateDice = ControllerMethods.ValidateDice(dice);

            if (!validateDice)
            {
                return BadRequest("You must roll between 1 and 6");
            }

            var pawnToMove = await _context.Pawns.FindAsync(pawnId);

            if (dice == 1)
            {
                pawnToMove.Position = pawnStartPosition[pawnToMove.Color];
                var game = await _context.Games.FindAsync(gameId);

                int index = _turnOrder.IndexOf(teamColor);

                if (index + 1 > 3)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }

                game.CurrentTurn = _turnOrder[index];

                await _context.SaveChangesAsync();
                return NoContent();
            }
            else if (dice == 6)
            {
                _sixesRolled++;
                pawnToMove.Position = pawnStartPosition[pawnToMove.Color] + 5;
                var game = await _context.Games.FindAsync(gameId);

                if (_sixesRolled < 2)
                {
                    int index = _turnOrder.IndexOf(teamColor);

                    if (index + 1 > game.NumberOfPlayers - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }

                    game.CurrentTurn = _turnOrder[index];
                    _sixesRolled = 0;
                }
                else
                {
                    game.CurrentTurn = teamColor;
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest("You can't move any pawns from base");
            }
        }

        // PUT: api/Pawns/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPawn(int id, Pawn pawn)
        {
            if (id != pawn.Id)
            {
                return BadRequest();
            }

            _context.Entry(pawn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PawnExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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

        // DELETE: api/Pawns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePawn(int id)
        {
            var pawn = await _context.Pawns.FindAsync(id);
            if (pawn == null)
            {
                return NotFound();
            }

            _context.Pawns.Remove(pawn);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PawnExists(int id)
        {
            return _context.Pawns.Any(e => e.Id == id);
        }
    }
}
