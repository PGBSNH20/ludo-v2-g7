using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoV2Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;

namespace LudoV2Web.Pages
{
    public class ExistingGamesModel : PageModel
    {
        public List<Game> Games { get; set; }
        [BindProperty]
        public Game Game { get; set; }
        public void OnGet()
        {
            var client = new RestClient("https://localhost:5001/api/");

            var request = new RestRequest("Games", DataFormat.Json);

            var response = client.Get<List<Game>>(request);

            Games = response.Data;
        }
    }
}
