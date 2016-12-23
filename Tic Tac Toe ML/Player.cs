using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace Tic_Tac_Toe_ML
{
    class Player
    {
        public enum MoveNumbers { Even, Odd };

        public enum PlayerTypes { Human, AI, ML };

        private BoardViewModel m_creator;

        private MoveNumbers m_moves;
        private PlayerTypes m_player_type;

        private bool m_winner = false;
        private char m_symbol = ' ';        

        public Player(PlayerTypes type, BoardViewModel origin)
        {
            m_player_type = type;
            m_creator = origin;
        }

        public PlayerTypes PlayerType
        {
            get
            {
                return m_player_type;
            }
        }

        public MoveNumbers Moves
        {
            get
            {
                return m_moves;
            }

            set
            {
                m_moves = value;
                if (m_moves == MoveNumbers.Odd)
                {
                    m_symbol = 'X';
                }
                else if (m_moves == MoveNumbers.Even)
                {
                    m_symbol = 'O';
                }
            }
        }

        public bool Winner
        {
            get
            {
                return m_winner;
            }

            set
            {
                m_winner = value;
            }
        }

        public char Symbol
        {
            get
            {
                return m_symbol;
            }
        }

        public BoardViewModel Creator
        {
            get
            {
                return m_creator;
            }
        }

        internal void makeMove(int count)
        {
            int position = -1;
            switch (PlayerType)
            {
                case PlayerTypes.AI:
                    position = makeAIMove();
                    break;
                case PlayerTypes.ML:
                    position = makeMLMove();
                    break;
                default:
                    position = makeHumanMove();
                    break;
            }
            Creator.Moves[count].Position = position;
        }

        private int makeHumanMove()
        {
            int position = Creator.Clicked;
            Creator.CurrentPlayer = this;
            return position;
        }

        private int makeMLMove()
        {
            int position = -1;
            Creator.CurrentPlayer = this;
            // Try to find a move.
            if ((position = findMove()) == -1)
            {
                position = pickRandomValidMove();
            }

            Creator.OnButtonClick(position);

            return position;
        }

        private int pickRandomValidMove()
        {
            // make random valid move.
            int position = -1;
            List<int> available_positions = new List<int>();
            foreach (Button button in Creator.Visual.ButtonList)
            {
                if (button.Content == null)
                {
                    available_positions.Add(Creator.convertButtonToPosition(button));
                }
            }
            Random random = new Random();
            int index = random.Next(available_positions.Count());
            position = available_positions[index];
            return position;
        }

        private int pickRandomValidMove(IEnumerable<Move> losing_positions)
        {
            // make random valid move.
            int position = -1;
            List<int> available_positions = new List<int>();
            foreach (Button button in Creator.Visual.ButtonList)
            {
                if (button.Content == null)
                {
                    available_positions.Add(Creator.convertButtonToPosition(button));
                }
            }
            foreach (var item in losing_positions)
            {
                available_positions.Remove(item.Position);
            }
            
            if (available_positions.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(available_positions.Count());
                position = available_positions[index];
            }           
            return position;
        }

        private int findMove()
        {
            // Get winning moves.
            int move = -1;
            ObservableCollection<Game> game_list = new ObservableCollection<Game>(Creator.Data);
            var winning_list = game_list.Where(i => i.Moves[Creator.MoveCount].Result == Move.Outcome.Winner).ToList();
            int previous = Creator.MoveCount - 1;
            while (previous > -1)
            {
                winning_list = winning_list.Where(i => i.Moves[previous].Position == Creator.Moves[previous].Position).ToList();
                previous--;
            }

            // Get the losing moves.
            var losing_list = game_list.Where(i => i.Moves[Creator.MoveCount].Result == Move.Outcome.Loser).ToList();
            previous = Creator.MoveCount - 1;
            while (previous > -1)
            {
                losing_list = losing_list.Where(i => i.Moves[previous].Position == Creator.Moves[previous].Position).ToList();
                previous--;
            }

            // Get the games ending in a draw.
            var draw_list = game_list.Where(i => i.Moves[Creator.MoveCount].Result == Move.Outcome.Draw).ToList();
            previous = Creator.MoveCount - 1;
            while (previous > -1)
            {
                draw_list = draw_list.Where(i => i.Moves[previous].Position == Creator.Moves[previous].Position).ToList();
                previous--;
            }

            // Pick the move from the winning list with the highest win ratio.
            var full_list = winning_list.Concat(losing_list).Concat(draw_list);
            List<Move> all_known_moves = new List<Move>();

            foreach (var item in full_list)
            {
                all_known_moves.Add(item.Moves[Creator.MoveCount]);
            }
            
            Move move_item = null;
            double winning_probability = 0;
            if (all_known_moves.Count() > 0)
            {
                foreach (var item in all_known_moves.Distinct())
                {
                    int wins = all_known_moves.Where(i => i.Equals(item) && i.Result == Move.Outcome.Winner).Count();
                    int losses = all_known_moves.Where(i => i.Equals(item) && i.Result == Move.Outcome.Loser).Count();
                    int draws = all_known_moves.Where(i => i.Equals(item) && i.Result == Move.Outcome.Draw).Count();
                    double new_probability = (wins + 0.5 * draws) / (wins + (double)draws + losses);

                    if (new_probability > winning_probability)
                    {
                        move_item = item;
                        winning_probability = new_probability;
                    }
                }
            }

            // If a loss is expected choose random of remaining.
            if (winning_probability == 0)
            {
                var losers = all_known_moves.Where(i => i.Result == Move.Outcome.Loser).ToList();
                move = pickRandomValidMove(losers);
            }
            // If a move is found then assign it;
            else if (move_item != null)
            {
                move = move_item.Position;
            }

            if(!isValidMove(move))
            {
                move = -1;
            }
            return move;
        }

        private bool isValidMove(int move)
        {
            bool valid = false;
            foreach (Button button in Creator.Visual.ButtonList)
            {                
                int row = Grid.GetRow(button);
                int col = Grid.GetColumn(button);
                if ( 3 * row + col == move && button.Content == null)
                {
                    valid = true;
                    break;
                }
            }
            return valid;
        }

        private int makeAIMove()
        {
            throw new NotImplementedException();
        }
    }
}
