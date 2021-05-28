using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;

namespace LudoV2Web.Pages
{
    public class UserGamesModel : PageModel
    {
        [ViewData]
        public string Username { get; set; }
        public int UserId { get; set; }
        public List<Game> Games { get; set; }
        [BindProperty]
        public Game Game { get; set; }
        public void OnGet()
        {
            UserId = Convert.ToInt32(HttpContext.Session.GetInt32("userId"));
            Username = HttpContext.Session.GetString("username");

            RestClient client = new("https://localhost:5001/api/");

            RestRequest request = new("Games/player/" + UserId, DataFormat.Json);

            var response = client.Get<List<Game>>(request);


            Games = response.Data;

        }
    }
}
