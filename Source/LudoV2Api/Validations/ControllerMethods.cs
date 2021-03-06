using LudoV2Api.Models;
using LudoV2Api.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoV2Api.Validations
{
    public static class ControllerMethods
    {

        public static bool ValidatingCurrentTurn(LudoContext context, int gameid, string teamColor)
        {
            var currentTurn = context.Games.Where(v => v.Id == gameid).Select(x => x.CurrentTurn).FirstOrDefault();

            if (currentTurn.ToLower() != teamColor.ToLower())
            {
                return false;
            }
            return true;
        }

        public static bool ValidateDice(int dice)
        {
            if (dice > 6 && dice < 1)
            {
                return false;
            }
            return true;
        }

        public static int PawnSafeZone(int position, string teamColor)
        {
            if (position > 43 && teamColor.ToLower() == "red")
            {
                var restMoves = position - 47;

                if (restMoves == 1)
                {
                    position = 60;
                }
                else if (restMoves > 1)
                {
                    position = 47 - restMoves;

                    if (position < 44)
                    {
                        position = 44;
                    }
                }
            }

            else if (position > 13 && teamColor.ToLower() == "blue")
            {
                if (position < 48)
                {
                    position = 48 + (position - 14);
                }

                var restMoves = position - 51;

                if (restMoves == 1)
                {
                    position = 60;
                }
                else if (restMoves > 0)
                {
                    position = 51 - restMoves;

                    if (position < 48)
                    {
                        position = 48;
                    }
                }
            }

            else if (position > 23 && teamColor.ToLower() == "green")
            {
                if (position < 52)
                {
                    position = 52 + (position - 24);
                }

                var restMoves = position - 55;

                if (restMoves == 1)
                {
                    position = 60;
                }
                else if (restMoves > 0)
                {
                    position = 55 - restMoves;

                    if (position < 52)
                    {
                        position = 52;
                    }
                }
            }

            else if (position > 33 && teamColor.ToLower() == "yellow")
            {
                if (position < 56)
                {
                    position = 56 + (position - 34);
                }

                var restMoves = position - 59;

                if (restMoves == 1)
                {
                    position = 60;
                }
                else if (restMoves > 0)
                {
                    position = 59 - restMoves;

                    if (position < 56)
                    {
                        position = 56;
                    }
                }
            }

            return position;
        }

        public static Pawn KockOutPawn(string teamColor, Pawn existsOnPosition, int newPosition)
        {
            int knockedOutBasePosition = -1;

            if (existsOnPosition.Color.ToLower() == teamColor.ToLower())
            {
                existsOnPosition.Position = -2;
                return existsOnPosition;
            }

            else if (existsOnPosition.Position == newPosition && existsOnPosition.Color.ToLower() != teamColor.ToLower())
            {
                if (existsOnPosition.Color.ToLower() == "red")
                {
                    knockedOutBasePosition = 0;
                    existsOnPosition.EligibleForWin = true;
                }
                else if (existsOnPosition.Color.ToLower() == "blue")
                {
                    knockedOutBasePosition = 1;
                    existsOnPosition.EligibleForWin = false;
                }
                else if (existsOnPosition.Color.ToLower() == "green")
                {
                    knockedOutBasePosition = 2;
                    existsOnPosition.EligibleForWin = false;
                }
                else if (existsOnPosition.Color.ToLower() == "yellow")
                {
                    knockedOutBasePosition = 3;
                    existsOnPosition.EligibleForWin = false;
                }
            }

            existsOnPosition.Position = knockedOutBasePosition;
            return existsOnPosition;
        }

        public static int NextTurn(int turnOrder, int numberOfPlayers)
        {
            if (turnOrder + 1 > numberOfPlayers - 1)
            {
                turnOrder = 0;
                return turnOrder;
            }
            else
            {
                turnOrder++;
                return turnOrder;
            }
        }

        public static int PawnExistsOnPosition(Pawn existsOnPosition, string pawnRequestTeamColor, int pawnId, int newPosition)
        {
            if (existsOnPosition != null && existsOnPosition.Id != pawnId && existsOnPosition.Position != 60)
            {
                Pawn knockedOutPosition = ControllerMethods.KockOutPawn(pawnRequestTeamColor, existsOnPosition, newPosition);

               if (knockedOutPosition.Position >= 0)
               {
                    return knockedOutPosition.Position;
               }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -2;
            }
        }
    }

}

