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
        public async Task<IActionResult> PutMovePawn(int dice, int id, int position, string teamColor, int gameid)
        {

            //  redSafeZone = 44-47; 
            //  blueSafeZone = 48-51;
            //  greenSafeZone = 52-55;
            //  yellowSafeZone = 56-59;

            var canPlay = ControllerMethods.ValidatingCurrentTurn(_context, gameid, teamColor);

            if (!canPlay)
            {
                BadRequest("It's not your turn");
            }

            var validateDice = ControllerMethods.ValidateDice(dice);

            if (!validateDice)
            {
                return BadRequest("You must roll between 1 and 6");
            }

            int basePosition = _pawnBases[teamColor];

            var pawnsInBase = _context.Pawns.Where(x => x.Position == basePosition).Count();

            if (pawnsInBase == 4 && dice != 6 || dice != 1)
            {
                return BadRequest("No pawns on the field to move");
            }
            int newPosition = position + dice;

            if (newPosition > 43 && teamColor != "Red")
            {
                newPosition = 4 + (newPosition - 43);
            }

            newPosition = ControllerMethods.PawnSafeZone(newPosition, teamColor);

            var existsOnPosition = _context.Pawns.Where(x => x.Position == newPosition).FirstOrDefault();

            int knockedOutPosition = ControllerMethods.KockOutPawn(teamColor, existsOnPosition, newPosition);

            if (knockedOutPosition == -2)
            {
                return BadRequest("You can't have two pawns at the same position");
            }
            else if (knockedOutPosition >= 0)
            {
                existsOnPosition.Position = knockedOutPosition;
            }

            var pawn = await _context.Pawns.FindAsync(id);
            pawn.Position = newPosition;
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

                await _context.SaveChangesAsync();
                return NoContent();
            }
            else if (dice ==  6)
            {
                pawnToMove.Position = pawnStartPosition[pawnToMove.Color] + 5;

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
