using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoV2Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace LudoV2Web.Pages
{
    public class LudoModel : PageModel
    {
        [BindProperty]
        public Game Game { get; set; }
        public List<Pawn> Pawns { get; set; }
        public List<Player> Players { get; set; }
        public void OnGet()
        {
            var client = new RestClient("https://localhost:5001/api/");
            var request = new RestRequest("Games/9", DataFormat.Json);
            var response = client.Get<Game>(request);

            Game = response.Data;

            var request2 = new RestRequest("Pawns/" + Game.Id, DataFormat.Json);
            var response2 = client.Get<List<Pawn>>(request2);

            Pawns = response2.Data;
        }

        public IActionResult OnPost(Game game)
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }
            Game = game;

            return Page();
        }

    }
}
