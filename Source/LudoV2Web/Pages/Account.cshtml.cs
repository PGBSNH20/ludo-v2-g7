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
    public class AccountModel : PageModel
    {
        public void OnGet()
        {

        }

        public async void OnPost()
        {
            if (Request.Form["login"] == 0)
            {
                Player newPlayer = new() { PlayerName = Request.Form["username"] };

                var client = new RestClient("https://localhost:5001/api/");

                var request = new RestRequest("Players", DataFormat.Json);
                request.AddJsonBody(newPlayer);

                await client.PostAsync<Player>(request);
            }
            else
            {

            }
        }
    }
}
