using System;

namespace LudoV2Api.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public int NumberOfPlayers { get; set; }
        public string CurrentTurn { get; set; }
        public string FirstPlace { get; set; }
        public string SecondPlace { get; set; }
        public string ThirdPlace { get; set; }
        public string FourthPlace { get; set; }
        public DateTime LastSaved { get; set; }
    }
}