using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoV2Api.Models;
using LudoV2Api.Models.ApiRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace LudoV2Web.Pages
{
    public class NewGameModel : PageModel
    {
        [BindProperty]
        public NewGameRequest NewGame { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(NewGameRequest newGame)
        {
            string startColor = Request.Form["startColor"];
            int players = Convert.ToInt32(Request.Form["players"]);
            JObject test = new();
            test.Add("GameName", newGame.GameName);
            test.Add("NumberOfPlayers", players);
            test.Add("CurrentTurn", startColor);

            var client = new RestClient("https://localhost:5001/api/");

            var request = new RestRequest("Games", Method.POST);

            request.AddParameter("application/Json", test, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            await client.ExecuteAsync(request);

            return RedirectToPage("/Ludo", new { id = newGame.GameName });
        }
    }
}
