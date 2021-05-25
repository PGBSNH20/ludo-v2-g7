using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using LudoV2Api.Controllers;
using LudoV2Api.Models;
using LudoV2Api.Models.ApiRequests;
using LudoV2Api.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LudoV2Test
{
    public class UnitTestPawnController
    {
        private DbContext _context;
        private PawnsController _controller;

        public UnitTestPawnController()
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

                for (int n = 0; n < 4; n++)
                {
                    Pawn pawn;
                  
                    pawn = new() { Game = game, Color = colors[i], Position = i, EligibleForWin = true };
                    if (colors[i] == "Red")
                    {
                        pawn = new() { Game = game, Color = colors[i], Position = n + 4, EligibleForWin = true };
                    }
                    _context.Add(pawn);
                }
            }
            Pawn yellowPawn = new() { Game = game, Color = "Yellow", Position = 43, EligibleForWin = false };
            _context.Add(yellowPawn);

            yellowPawn = new() { Game = game, Color = "Yellow", Position = 40, EligibleForWin = true };
            _context.Add(yellowPawn);

            yellowPawn = new() { Game = game, Color = "Yellow", Position = 31, EligibleForWin = true };
            _context.Add(yellowPawn);

            yellowPawn = new() { Game = game, Color = "Yellow", Position = 25, EligibleForWin = true };
            _context.Add(yellowPawn);

            _context.SaveChanges();
        }

        [Fact]
        public void MovePawnFromBase_ExpectSuccess()
        {
            MovePawnRequest movePawn = new()
            {
                Dice = 1,
                PawnId = 5,
                Position = 1,
                TeamColor = "blue",
                GameId = 1
            };

            var movePawnFromBase = _controller.PutPawnFromBase(movePawn);

            Assert.IsType<OkObjectResult>(movePawnFromBase.Result.Result);
        }

        [Fact]
        public void MovePawnFromBase_ExpectBadRequestForWrongPlayerTurn()
        {
            MovePawnRequest movePawn = new()
            {
                Dice = 6,
                PawnId = 3,
                Position = 2,
                TeamColor = "blue",
                GameId = 1
            };

            var movePawnFromBase = _controller.PutPawnFromBase(movePawn);

            Assert.IsType<OkObjectResult>(movePawnFromBase.Result.Result);
        }

        [Fact]
        public void MovePawnFromBase_ExpectBadRequestForWrongDiceroll()
        {
            MovePawnRequest movePawn = new()
            {
                Dice = 3,
                PawnId = 2,
                Position = 1,
                TeamColor = "blue",
                GameId = 1
            };

            var movePawnFromBase = _controller.PutPawnFromBase(movePawn);

            Assert.IsType<OkObjectResult>(movePawnFromBase.Result.Result);
        }

        [Fact]
        public async void MovePawn_ExpectPawnToMoveFiveSteps()
        {
            MovePawnRequest movePawn = new()
            {
                Dice = 5,
                PawnId = 16,
                Position = 25,
                TeamColor = "yellow",
                GameId = 2
            };

            await _controller.PutMovePawn(movePawn);

            var getPawn = await _controller.GetPawn(16);

            Assert.Equal(30, getPawn.Value.Position);
        }

        [Fact]
        public async void MovePawn_ExpectPawnToMoveTSafeZone()
        {
            MovePawnRequest movePawn = new()
            {
                Dice = 4,
                PawnId = 15,
                Position = 31,
                TeamColor = "yellow",
                GameId = 2
            };

            await _controller.PutMovePawn(movePawn);

            var getPawn = await _controller.GetPawn(15);

            Assert.Equal(57, getPawn.Value.Position);
        }

        [Fact]
        public async void MovePawn_ExpectPawnToKnockout()
        {
            MovePawnRequest movePawn = new()
            {
                Dice = 2,
                PawnId = 13,
                Position = 43,
                TeamColor = "yellow",
                GameId = 2
            };

            await _controller.PutMovePawn(movePawn);

            var getPawn = await _controller.GetPawn(13);
            var knockedOutPawn = await _controller.GetPawn(2);

            Assert.Equal(5, getPawn.Value.Position);
            Assert.Equal(0, knockedOutPawn.Value.Position);
        }
    }
}
