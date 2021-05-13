﻿using LudoV2Api.Models;
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

            if (currentTurn != teamColor)
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
            if (position > 43 && teamColor == "Red")
            {
                position = 44 + (position - 43);
                var restMoves = position % 47;

                if (restMoves > 0)
                {
                    position = position - restMoves;

                    if (position < 44)
                    {
                        position = 44;
                    }
                }
            }

            else if (position > 13 && teamColor == "Blue")
            {
                position = 48 + (position - 13);
                var restMoves = position % 51;

                if (restMoves > 0)
                {
                    position = position - restMoves;

                    if (position < 48)
                    {
                        position = 48;
                    }
                }
            }

            else if (position > 23 && teamColor == "Green")
            {
                position = 52 + (position - 23);
                var restMoves = position % 55;

                if (restMoves > 0)
                {
                    position = position - restMoves;

                    if (position < 52)
                    {
                        position = 52;
                    }
                }
            }

            else if (position > 33 && teamColor == "Yellow")
            {
                position = 56 + (position - 33);
                var restMoves = position % 59;

                if (restMoves > 0)
                {
                    position = position - restMoves;

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

            if (existsOnPosition.Color == teamColor)
            {
                existsOnPosition.Position = -2;
                return existsOnPosition;
            }

            else if (existsOnPosition.Position == newPosition && existsOnPosition.Color != teamColor)
            {
                if (existsOnPosition.Color == "Red")
                {
                    knockedOutBasePosition = 0;
                    existsOnPosition.EligibleForWin = true;
                }
                else if (existsOnPosition.Color == "Blue")
                {
                    knockedOutBasePosition = 1;
                    existsOnPosition.EligibleForWin = false;
                }
                else if (existsOnPosition.Color == "Green")
                {
                    knockedOutBasePosition = 2;
                    existsOnPosition.EligibleForWin = false;
                }
                else if (existsOnPosition.Color == "Yellow")
                {
                    knockedOutBasePosition = 3;
                    existsOnPosition.EligibleForWin = false;
                }
            }

            existsOnPosition.Position = knockedOutBasePosition;
            return existsOnPosition;
        }
    }

}
