using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LudoV2Api.Models;
using LudoV2Api.Models.DbModels;
using LudoV2Api.Models.ApiRequests;

namespace LudoV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly LudoContext _context;

        public PlayersController(LudoContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // GET: api/Players/game/5
        [HttpGet("game/{id}")]
        public async Task<ActionResult<IEnumerable<PlayerGame>>> GetPlayersInGame(int id)
        {
            return await _context.GamePlayers.Where(x => x.GameId == id).ToListAsync();
        }


        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
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

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NewPlayerRequest>> PostPlayer(NewPlayerRequest newPlayer)
        {
            Player player = new() { PlayerName = newPlayer.PlayerName };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        // POST: api/Players/game
        [HttpPost("game")]
        public async Task<ActionResult<PlayerGameRequest>> PostPlayerGame(PlayerGameRequest newPlayerGame)
        {
            Player player = _context.Players.Where(x => x.PlayerName.ToLower() == newPlayerGame.PlayerName.ToLower()).FirstOrDefault();

            if (player.Id != 0)
            {
                PlayerGame playerGame = new() { GameId = newPlayerGame.GameId, Color = newPlayerGame.Color, PlayerId = player.Id };
                _context.GamePlayers.Add(playerGame);
                await _context.SaveChangesAsync();
                return Created("api/Players/game", newPlayerGame);
            }
            else
            {
                return BadRequest("The player you are trying to add to the game dose not exist");
            }
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
