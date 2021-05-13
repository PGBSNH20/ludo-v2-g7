using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using LudoV2Api.Controllers;
using LudoV2Api.Models.DbModels;
using LudoV2Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace LudoV2Test
{
    public class UnitTest1
    {
        private DbContext _context;
        private PawnsController _controller;

        public UnitTest1()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LudoContext>().UseInMemoryDatabase(databaseName: "TestDB").Options;
            _context = new LudoContext(dbContextOptions);
            _context.Database.EnsureCreated();

            _controller = new PawnsController((LudoContext)_context);

            var game = new Game { CurrentTurn = "Blue", NumberOfPlayers = 4 };

            _context.Add(game);

            game = new Game { CurrentTurn = "Yellow", NumberOfPlayers = 4 };
            _context.Add(game);

            string[] colors = new[] { "Red", "Blue", "Green", "Yellow" };

            for (int i = 0; i < 3; i++)
            {

                for (int n = 0; n < 3; n++)
                {
                    Pawn pawn;
                  
                    pawn = new() { Game = game, Color = colors[i], Position = i, EligibleForWin = true };
                    if (colors[i] == "Red")
                    {
                        pawn = new() { Game = game, Color = colors[i], Position = i + 4, EligibleForWin = true };
                    }
                    _context.Add(pawn);
                }
            }
            Pawn yellowPawn = new() { Game = game, Color = "Yellow", Position = 43, EligibleForWin = true };
            _context.Add(yellowPawn);

            yellowPawn = new() { Game = game, Color = "Yellow", Position = 40, EligibleForWin = true };
            _context.Add(yellowPawn);

            yellowPawn = new() { Game = game, Color = "Yellow", Position = 35, EligibleForWin = true };
            _context.Add(yellowPawn);

            yellowPawn = new() { Game = game, Color = "Yellow", Position = 25, EligibleForWin = true };
            _context.Add(yellowPawn);

            _context.SaveChanges();
        }

        [Fact]
        public void MovePawnFromBase_ExpectSuccess()
        {
            var movePawnFromBase = _controller.PutPawnFromBase(1, 5, 1, "Blue");

            Assert.IsType<NoContentResult>(movePawnFromBase.Result);
        }

        [Fact]
        public void MovePawnFromBase_ExpectBadRequestForWrongPlayerTurn()
        {
            var movePawnFromBase = _controller.PutPawnFromBase(1, 3, 6, "Green");

            Assert.IsType<BadRequestObjectResult>(movePawnFromBase.Result);
        }

        [Fact]
        public void MovePawnFromBase_ExpectBadRequestForWrongDiceroll()
        {
            var movePawnFromBase = _controller.PutPawnFromBase(1, 2, 3, "Blue");

            Assert.IsType<BadRequestObjectResult>(movePawnFromBase.Result);
        }

        [Fact]
        public async void MovePawn_ExpectPawnToMoveFiveSteps()
        {       
            await _controller.PutMovePawn(5, 16, 25, "Yellow", 2);

            var getPawn = await _controller.GetPawn(16);

            Assert.Equal(30, getPawn.Value.Position);
            
        }
    }
}
