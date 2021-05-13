using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoV2Api.Models.ApiRequests
{
    public class NewGameRequest
    {
        public int NumberOfPlayers { get; set; }
        public string CurrentTurn { get; set; }
    }
}
