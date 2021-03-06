using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoV2Api.Models;
using LudoV2Api.Models.ApiRequests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace LudoV2Web.Pages
{
    public class LudoModel : PageModel
    {
        [ViewData]
        public string Username { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public int Dice { get; set; }
        public Game Game { get; set; }
        public Pawn Pawn { get; set; }
        public List<Pawn> Pawns { get; set; }
        public List<PlayerGame> Players { get; set; }
        public MovePawnRequest MovePawn { get; set; }
        public List<string> TeamColors { get;} = new() { "Red", "Blue", "Green", "Yellow" };
        public void OnGet(string title)
        {
            var gameId = HttpContext.Session.GetInt32("gameId");

            Title = title;
            Username = HttpContext.Session.GetString("username");
            UserId = Convert.ToInt32(HttpContext.Session.GetInt32("userId"));

            RestClient client = new("https://localhost:5001/api/");
            RestRequest request;

            if (gameId == null)
            {
                request = new RestRequest("Games", DataFormat.Json);
                var responseGameId = client.Get<List<Game>>(request);

                var findGame = responseGameId.Data.Find(x => x.GameName.ToLower() == title.ToLower());

                gameId = findGame.Id;
            }

            request = new RestRequest("Games/" + gameId, DataFormat.Json);
            var response = client.Get<Game>(request);

            Game = response.Data;


            var pawnList = FindPawns(Game.Id);
            Pawns = pawnList.Result;

            request = new RestRequest("Players/game/" + gameId, DataFormat.Json);
            var playerGameResponse = client.Get<List<PlayerGame>>(request);

            Players = playerGameResponse.Data;
        }

        public async Task<List<Pawn>> FindPawns(int gameId)
        {
            RestClient client = new("https://localhost:5001/api/");
            RestRequest request = new("Pawns/game/" + gameId, DataFormat.Json);
            var response = await client.GetAsync<List<Pawn>>(request);

            return response;
        }
    }
}
