using System;

namespace LudoV2Api.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int NumberOfPlayers { get; set; }
        public string CurrentTurn { get; set; }
        public int? FirstPlace { get; set; }
        public int? SecondPlace { get; set; }
        public int? ThirdPlace { get; set; }
        public int? FourthPlace { get; set; }
        public DateTime LastSaved { get; set; }
    }
}