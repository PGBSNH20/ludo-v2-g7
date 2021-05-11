using System;


namespace LudoV2Api.Models
{
    public class PawnSavePoint
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public string Color { get; set; }
        public int PlayerType { get; set; }
        public int Position { get; set; }
    }
}
