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
    public class LudoModel : PageModel
    {
        [BindProperty]
        public Game Game { get; set; }
        public List<Pawn> Pawns { get; set; }
        public List<Player> Players { get; set; }
        public void OnGet()
        {

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
