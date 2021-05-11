using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoV2Api
{
    public class GameLogic
    {

        public void MoveFromBase(int dice, string color)
        {
            if (dice == 1)
            {
                // Får flytta 1 pjäs på brädan
            }
            else if (dice == 6)
            {
                // Får flytta ut 1 pjäs 6 steg framåt
            }
        }

        public void MovePawn(int dice, string color)
        {
            // Flytta pjäsen x antal steg, kolla om någon annan står på rutan du landar på
            // Om en annan spelares pjäs är på rutan du landar på så knuffas hen ut
            // Uppdatera positionerna i databasen
        }

        public void KnockOutPawn(string color, int position)
        {
            // Kolla databasen om en annan spelares pjäs finns på 'position'
            // om det gör det så skickas den spelaren tillbaka till basen
            // Uppdatera positionerna i databasen
        }

    }
}
