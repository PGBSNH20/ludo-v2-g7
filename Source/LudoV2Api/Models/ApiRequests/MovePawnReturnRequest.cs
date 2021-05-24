using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoV2Api.Models.ApiRequests
{
    public class MovePawnReturnRequest
    {
        public int PawnPosition { get; set; }
        public string CurrentTurn { get; set; }
        public int KnockedPawnPosition { get; set; }
    }
}
