using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LudoV2Api.Models;
using LudoV2Api.Models.ApiRequests;
using LudoV2Api.Models.DbModels;

namespace LudoV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly LudoContext _context;

        public GamesController(LudoContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game, int dice)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
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

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NewGameRequest>> PostGame(NewGameRequest gameRequest)
        {
            Game game = new() { CurrentTurn = gameRequest.CurrentTurn, NumberOfPlayers = gameRequest.NumberOfPlayers, GameName = gameRequest.GameName };

            string[] colors = new[] { "Red", "Blue", "Green", "Yellow" };

            _context.Games.Add(game);

            for (int i = 0; i < game.NumberOfPlayers; i++)
            {

                for (int n = 0; n < 4; n++)
                {
                    Pawn pawn;

                    pawn = new() { Game = game, Color = colors[i], Position = i, EligibleForWin = false };
                    if (colors[i] == "Red")
                    {
                        pawn = new() { Game = game, Color = colors[i], Position = i, EligibleForWin = true };
                    }
                    _context.Pawns.Add(pawn);
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
