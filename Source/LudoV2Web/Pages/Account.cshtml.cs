using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoV2Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp;
using LudoV2Web.Cookies;
using Microsoft.AspNetCore.Http;

namespace LudoV2Web.Pages
{
    public class AccountModel : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<ActionResult> OnPost()
        {
            if (Request.Form["login"].ToString() == "0")
            {
                Player player = new() { PlayerName = Request.Form["username"] };

                var client = new RestClient("https://localhost:5001/api/");

                var request = new RestRequest("Players", DataFormat.Json);
                request.AddJsonBody(player);

                var response = await client.PostAsync<Player>(request);

                HttpContext.Session.SetString("username", response.PlayerName);
                HttpContext.Session.SetInt32("userId", response.Id);

                return RedirectToPage("index");
            }
            else
            {
                var client = new RestClient ("https://localhost:5001/api/");

                var request = new RestRequest("Players", DataFormat.Json);

                try
                {
                    var response = await client.GetAsync<List<Player>>(request);

                    var user = response.Find( x => x.PlayerName == Request.Form["username"]);
                


                if (user != null)
                {
                    var options = CustomCookiesOptions.CustomCookieOptions();

                    HttpContext.Session.SetString("username", user.PlayerName);
                    HttpContext.Session.SetInt32("userId", user.Id);


                    return RedirectToPage("index");
                }
                else
                {
                    return Page();
                }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }
    }
}
