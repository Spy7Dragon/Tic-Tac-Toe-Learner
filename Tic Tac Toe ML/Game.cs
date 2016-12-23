using System;
using System.Linq;

namespace Tic_Tac_Toe_ML
{
    [Serializable]
    class Game
    {
        private Move[] moves = new Move[9];
        private Player.MoveNumbers winner;

        public Game(Move[] newMoves)
        {
            if (newMoves != null)
            {
                for (int i = 0; i < newMoves.Count(); i++)
                {
                    moves[i] = new Move(newMoves[i]);
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    moves[i] = new Move();
                }
            }

        }

        internal Move[] Moves
        {
            get
            {
                return moves;
            }
        }

        internal Player.MoveNumbers Winner
        {
            get
            {
                return winner;
            }

            set
            {
                winner = value;
            }
        }

        public override bool Equals(object obj)
        {
            bool equal = false;
            if (obj is Game)
            {
                Game other_game = (Game)obj;
                equal = Enumerable.SequenceEqual(Moves, other_game.Moves);
            }
            return equal;
        }

        public override int GetHashCode()
        {
            int modifier = 1;
            int hash_code = 0;
            for (int i = 0; i < Moves.Count(); i++)
            {
                hash_code += Moves[i].Position * modifier;
                modifier = modifier * 10;
            }
            if (Winner == Player.MoveNumbers.Even)
            {
                hash_code = hash_code * 20;
            }
            else
            {
                hash_code = hash_code * 10;
            }
            return hash_code;
        }
    }
}
