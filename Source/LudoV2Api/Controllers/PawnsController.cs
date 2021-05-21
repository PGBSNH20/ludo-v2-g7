﻿using System;
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

namespace LudoV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PawnsController : ControllerBase
    {
        private readonly LudoContext _context;
        private List<string> _turnOrder = new() { "red", "blue", "green", "yellow" };
        private int _sixesRolled = 0;

        private Dictionary<string, int> _pawnBases = new()
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

        // GET: api/Pawns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pawn>>> GetPawnSavePoints()
        {
            return await _context.Pawns.ToListAsync();
        }

        // GET: api/Pawns/5
        [HttpGet("{id}")]
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
        public async Task<IActionResult> PutMovePawn(/*int dice, int pawnId, int position, string teamColor, int gameId*/ MovePawnRequest pawnRequest)
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

            Pawn existsOnPosition = _context.Pawns.Where(x => x.Position == newPosition).FirstOrDefault();

            if (existsOnPosition != null)
            {
                Pawn knockedOutPosition = ControllerMethods.KockOutPawn(pawnRequest.TeamColor, existsOnPosition, newPosition);

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

            var game = await _context.Games.FindAsync(pawnRequest.GameId);

            if (pawnRequest.Dice == 6)
            {
                _sixesRolled++;
            }

            if (_sixesRolled >= 2 || _sixesRolled == 0)
            {
                int index = _turnOrder.IndexOf(pawnRequest.TeamColor);

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
                game.CurrentTurn = pawnRequest.TeamColor;
            }

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("movefrombase")]
        public async Task<IActionResult> PutPawnFromBase(/*int gameId, int pawnId, int dice, string teamColor */MovePawnRequest pawnRequest)
        {

            Dictionary<string, int> pawnStartPosition = new()
            {
                { "red", 4 },
                { "blue", 14 },
                { "green", 24 },
                { "yellow", 34 }
            };

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

                int index = _turnOrder.IndexOf(pawnRequest.TeamColor);

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
            else if (pawnRequest.Dice == 6)
            {
                _sixesRolled++;
                pawnToMove.Position = pawnStartPosition[pawnToMove.Color.ToLower()] + 5;
                var game = await _context.Games.FindAsync(pawnRequest.GameId);

                if (_sixesRolled < 2)
                {
                    int index = _turnOrder.IndexOf(pawnRequest.TeamColor);

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
                    game.CurrentTurn = pawnRequest.TeamColor;
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