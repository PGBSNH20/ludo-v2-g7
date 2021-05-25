using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoV2Api.Models;
using LudoV2Api.Models.ApiRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace LudoV2Web.Pages
{
    public class LudoModel : PageModel
    {
        public string Title { get; set; }
        public int Dice { get; set; }
        [BindProperty]
        public Game Game { get; set; }
        [BindProperty]
        public Pawn Pawn { get; set; }
        [BindProperty]
        public List<Pawn> Pawns { get; set; }
        public List<Player> Players { get; set; }
        [BindProperty]
        public MovePawnRequest MovePawn { get; set; }
        public void OnGet(string title, int gameId)
        {
            Title = title;
            var client = new RestClient("https://localhost:5001/api/");
            var request = new RestRequest("Games/" + gameId, DataFormat.Json);
            var response = client.Get<Game>(request);

            Game = response.Data;


            var pawnList = FindPawns(Game.Id);
            Pawns = pawnList.Result;
        }

        //public async Task<IActionResult> OnPost()
        //{
        //    //if (ModelState.IsValid == false)
        //    //{
        //    //    return Page();
        //    //}

        //    var client = new RestClient("https://localhost:5001/api/");
        //    if (MovePawn.Position > 4)
        //    {
        //        var request = new RestRequest("Pawns/move", Method.PUT);

        //        request.AddJsonBody(MovePawn);

        //        var response = await client.ExecuteAsync(request);

        //        var responseContent = response.Content;
        //    }
        //    else
        //    {
        //        var request = new RestRequest("Pawns/movefrombase", Method.PUT);

        //        request.AddJsonBody(MovePawn);

        //        var response = await client.ExecuteAsync(request);

        //        var responseContent = response.Content;
        //    }

        //    return RedirectToPage("/Ludo", new { gameId = MovePawn.GameId });
        //}

        public async Task<List<Pawn>> FindPawns(int gameId)
        {
            var client = new RestClient("https://localhost:5001/api/");
            var request = new RestRequest("Pawns/game/" + gameId, DataFormat.Json);
            var response = await client.GetAsync<List<Pawn>>(request);

            return response;
        }
    }
}
