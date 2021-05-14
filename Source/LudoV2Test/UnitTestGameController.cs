using LudoV2Api.Controllers;
using LudoV2Api.Models.ApiRequests;
using LudoV2Api.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LudoV2Test
{
    public class UnitTestGameController
    {
        private LudoContext _context;
        private GamesController _controller;

        public UnitTestGameController()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LudoContext>().UseInMemoryDatabase(databaseName: "TestDB").Options;
            _context = new LudoContext(dbContextOptions);
            _context.Database.EnsureCreated();

           _controller = new GamesController((LudoContext) _context);
        }

        [Fact]
        public async void PostGame_ExpectGameToBePut()
        {
            NewGameRequest newGame = new() { CurrentTurn = "Red", NumberOfPlayers = 4 };

            var game = await _controller.PostGame(newGame);

            Assert.IsType<ActionResult<NewGameRequest>>(game);
        }
    }
}
