using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoV2Api.Models.ApiRequests
{
    public class MovePawnRequest
    {
        public int Dice { get; set; }
        public int PawnId { get; set; }
        public int Position { get; set; }
        public string TeamColor { get; set; }
        public int GameId { get; set; }
    }
}
