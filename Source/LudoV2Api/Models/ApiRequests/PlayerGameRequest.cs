using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoV2Api.Models.ApiRequests
{
    public class PlayerGameRequest
    {
        public int GameId { get; set; }
        public string PlayerName { get; set; }
        public string Color { get; set; }
    }
}
