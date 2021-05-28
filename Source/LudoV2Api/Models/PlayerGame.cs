using System.Collections.Generic;

namespace LudoV2Api.Models
{
    public class PlayerGame
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public string Color { get; set; }
    }
}