using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoV2Api.Models.ApiRequests
{
    public class NewGameRequest
    {
        public string GameName { get; set; }
        public int NumberOfPlayers { get; set; }
        public string CurrentTurn { get; set; }
    }
}
