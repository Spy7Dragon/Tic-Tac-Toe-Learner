using System;

namespace Tic_Tac_Toe_ML
{
    [Serializable]
    class Move
    {
        public enum Outcome { Winner, Loser, Draw };

        private int step_number;
        private Outcome result;
        private int position;

        public Move()
        {
            result = Outcome.Draw;
            step_number = -1;
            position = -1;
        }

        public Move(Move anotherMove)
        {
            result = anotherMove.Result;
            step_number = anotherMove.StepNumber;
            position = anotherMove.Position;
        }
        
        public int StepNumber
        {
            get
            {
                return step_number;
            }
            set
            {
                step_number = value;
            }
        }

        public Outcome Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        public int Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public override bool Equals(object other_move)
        {
            if (other_move is Move)
            {
                Move compare = (Move)other_move;
                if (StepNumber == compare.StepNumber &&
                    Position == compare.Position)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash_code = 0;
            switch (Result)
            {
                case Outcome.Loser:
                    hash_code += 100;
                    break;
                case Outcome.Draw:
                    hash_code += 200;
                    break;
                case Outcome.Winner:
                    hash_code += 300;
                    break;
            }
            hash_code += StepNumber * 10;
            hash_code += position;

            return hash_code;
        }
    }
}
