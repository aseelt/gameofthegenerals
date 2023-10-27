using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generals.Classes
{
    public static class Log
    {
        public static List<string[]> Moves = new List<string[]>();

        public static void AddToLog(string attackingPlayer, string location, Piece winningPiece, Piece losingPiece, string outcome)
        {
            string[] move = new string[5];
            move[0] = attackingPlayer;
            move[1] = location;
            move[2] = winningPiece.GetName();
            move[3] = losingPiece.GetName();
            move[4] = outcome;

            Moves.Add(move);
        }
        public static void AddToLog(string attackingPlayer, string location, string outcome)
        {
            string[] move = new string[5];
            move[0] = attackingPlayer;
            move[1] = location;
            move[2] = "Flag";
            move[3] = "N/A";
            move[4] = outcome;

            Moves.Add(move);
        }
    }
}
