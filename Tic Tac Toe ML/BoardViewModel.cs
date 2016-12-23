using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;

namespace Tic_Tac_Toe_ML
{
    public class BoardViewModel : Window
    {
        private BoardWindow m_visual;

        private HashSet<Game> m_untouched_data = new HashSet<Game>();
        private HashSet<Game> m_included_data = new HashSet<Game>();

        private Player m_player_one;
        private Player m_player_two;
        private Player m_current_player;
        private Player.MoveNumbers m_winners_moves;

        private Move[] m_moves = new Move[9] { new Move(), new Move(), new Move(), new Move(), new Move(), new Move(), new Move(), new Move(), new Move() };
        
        private bool m_winner_exists = false;
        private int m_move_count = 0;
        private int m_clicked = -1;
        private bool m_completed = true;
        private bool m_end_of_game = false;

        public int Clicked
        {
            get
            {
                return m_clicked;
            }

            set
            {
                m_clicked = value;
            }
        }

        internal Player CurrentPlayer
        {
            get
            {
                return m_current_player;
            }

            set
            {
                m_current_player = value;
            }
        }

        internal HashSet<Game> Data
        {
            get
            {
                return m_included_data;
            }

            set
            {
                m_included_data = value;
            }
        }

        public int MoveCount
        {
            get
            {
                return m_move_count;
            }

            set
            {
                m_move_count = value;
            }
        }

        internal Move[] Moves
        {
            get
            {
                return m_moves;
            }

            set
            {
                m_moves = value;
            }
        }

        internal Player PlayerOne
        {
            get
            {
                return m_player_one;
            }
        }

        internal Player PlayerTwo
        {
            get
            {
                return m_player_two;
            }
        }

        public bool Completed
        {
            get
            {
                return m_completed;
            }

            set
            {
                m_completed = value;
            }
        }

        public BoardWindow Visual
        {
            get
            {
                return m_visual;
            }
        }

        public bool WinnerExists
        {
            get
            {
                return m_winner_exists;
            }

            set
            {
                m_winner_exists = value;
            }
        }

        internal HashSet<Game> UntouchedData
        {
            get
            {
                return m_untouched_data;
            }
        }

        internal Player.MoveNumbers WinnersMoves
        {
            get
            {
                return m_winners_moves;
            }

            set
            {
                m_winners_moves = value;
            }
        }

        public bool EndOfGame
        {
            get
            {
                return m_end_of_game;
            }

            set
            {
                m_end_of_game = value;
            }
        }

        public BoardViewModel(BoardWindow parent)
        {
            m_visual = parent;
            // Get data for machine learning.
            getData();
            rotateData();
            flipData();
            diagonalData();
 
            m_player_one = new Player(Player.PlayerTypes.Human, this);
            m_player_two = new Player(Player.PlayerTypes.ML, this);
        }

        private void diagonalData()
        {
            foreach (Game game in UntouchedData)
            {
                // Diagonally transform left top and bottom right.
                for (int i = 0; i < 2; i++)
                {
                    foreach (Move move in game.Moves)
                    {
                        switch (move.Position)
                        {
                            case 0:
                                move.Position = 8;
                                break;
                            case 1:
                                move.Position = 5;
                                break;
                            case 3:
                                move.Position = 7;
                                break;
                            case 5:
                                move.Position = 1;
                                break;
                            case 7:
                                move.Position = 3;
                                break;
                            case 8:
                                move.Position = 0;
                                break;
                        }
                    }
                    Game new_game = new Game(game.Moves);
                    Data.Add(new_game);
                }

                // Diagonally transform right top and bottom left.
                for (int i = 0; i < 2; i++)
                {
                    foreach (Move move in game.Moves)
                    {
                        switch (move.Position)
                        {
                            case 1:
                                move.Position = 3;
                                break;
                            case 2:
                                move.Position = 6;
                                break;
                            case 3:
                                move.Position = 1;
                                break;
                            case 5:
                                move.Position = 7;
                                break;
                            case 6:
                                move.Position = 2;
                                break;
                            case 7:
                                move.Position = 5;
                                break;
                        }
                    }
                    Game new_game = new Game(game.Moves);
                    Data.Add(new_game);
                }
            }
        }

        private void flipData()
        {
            foreach (Game game in UntouchedData)
            {
                // Flip right and then right again.
                for (int i = 0; i < 2; i++)
                {
                    foreach (Move move in game.Moves)
                    {
                        switch (move.Position)
                        {
                            case 0:
                                move.Position = 2;
                                break;
                            case 2:
                                move.Position = 0;
                                break;
                            case 3:
                                move.Position = 5;
                                break;
                            case 5:
                                move.Position = 3;
                                break;
                            case 6:
                                move.Position = 8;
                                break;
                            case 8:
                                move.Position = 6;
                                break;
                        }
                    }
                    Game new_game = new Game(game.Moves);
                    Data.Add(new_game);
                }

                // Flip down then down right again.
                for (int i = 0; i < 2; i++)
                {
                    foreach (Move move in game.Moves)
                    {
                        switch (move.Position)
                        {
                            case 0:
                                move.Position = 6;
                                break;
                            case 1:
                                move.Position = 7;
                                break;
                            case 2:
                                move.Position = 8;
                                break;
                            case 6:
                                move.Position = 0;
                                break;
                            case 7:
                                move.Position = 1;
                                break;
                            case 8:
                                move.Position = 2;
                                break;
                        }
                    }
                    Game new_game = new Game(game.Moves);
                    Data.Add(new_game);
                }
            }
        }

        private void rotateData()
        {
            foreach (Game game in UntouchedData)
            {
                
                // Rotate data right four times leading to beginning.
                for (int i = 0; i < 4; i++)
                {
                    foreach (Move move in game.Moves)
                    {
                        switch (move.Position)
                        {
                            case 0:
                                move.Position = 6;
                                break;
                            case 1:
                                move.Position = 3;
                                break;
                            case 2:
                                move.Position = 0;
                                break;
                            case 3:
                                move.Position = 7;
                                break;
                            case 5:
                                move.Position = 1;
                                break;
                            case 6:
                                move.Position = 8;
                                break;
                            case 7:
                                move.Position = 5;
                                break;
                            case 8:
                                move.Position = 2;
                                break;
                        }
                    }
                    Game new_game = new Game(game.Moves);
                    Data.Add(new_game);
                }
            }
        }

        public void Start()
        {
            // Pick random player to start with.
            Random decider = new Random();
            if (decider.Next(0, 2) == 0)
            {
                PlayerOne.Moves = Player.MoveNumbers.Even;
                PlayerTwo.Moves = Player.MoveNumbers.Odd;
                CurrentPlayer = PlayerOne;
            }
            else
            {
                m_player_two.Moves = Player.MoveNumbers.Even;
                m_player_one.Moves = Player.MoveNumbers.Odd;
                CurrentPlayer = PlayerTwo;
            }

            // Computer moves do not wait for interaction.
            if (CurrentPlayer.PlayerType != Player.PlayerTypes.Human)
            {
                OnMoveChanged();
            }
        }

        private void endGame()
        {
            // If there was a winner, then label player moves according to the winner.
            if (WinnerExists)
            {
                if (WinnersMoves == Player.MoveNumbers.Even)
                {
                    foreach (Move move in m_moves)
                    {
                        if (move.StepNumber % 2 == 0)
                        {
                            move.Result = Move.Outcome.Winner;
                        }
                        else
                        {
                            move.Result = Move.Outcome.Loser;
                        }
                    }
                }
                else if (WinnersMoves == Player.MoveNumbers.Odd)
                {
                    foreach (Move move in m_moves)
                    {
                        if (move.StepNumber % 2 == 1)
                        {
                            move.Result = Move.Outcome.Winner;
                        }
                        else
                        {
                            move.Result = Move.Outcome.Loser;
                        }
                    }
                }
            }

            // Add moves to machine learning data.
            Game new_game = new Game(m_moves);
            UntouchedData.Add(new_game);
            // Save state of data.
            saveData();
            Application.Current.Shutdown();
        }

        private void DoOddMove()
        {
            // Player odd makes a move.
            Moves[MoveCount].StepNumber = MoveCount;
            if (PlayerOne.Moves == Player.MoveNumbers.Odd)
            {
                PlayerOne.makeMove(MoveCount);
            }
            else
            {
                PlayerTwo.makeMove(MoveCount);
            }
        }

        private void DoEvenMove()
        {
            // Player even makes a move.
            Moves[MoveCount].StepNumber = MoveCount;
            if (PlayerOne.Moves == Player.MoveNumbers.Even)
            {
                PlayerOne.makeMove(MoveCount);
            }
            else
            {
                PlayerTwo.makeMove(MoveCount);
            }
        }

        private void saveData()
        {
            // Open file and export new data.
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("Data.data", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, UntouchedData);
            stream.Close();
        }

        private void getData()
        {
            // Check if file exists.
            string data_file = "Data.data";
            if (!File.Exists(data_file))
            {
                File.Create(data_file);
            }

            // Open file and import games to data.
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("Data.data", FileMode.Open, FileAccess.Read, FileShare.Read);
                m_untouched_data = (HashSet<Game>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void OnMoveChanged()
        {
            // If even players turns do even move.
            if (MoveCount % 2 == 0)
            {
                DoEvenMove();
                EndOfGame = checkIfEndOfGame();
            }
            // Otherwise do odd move.
            else
            {
                DoOddMove();
                EndOfGame = checkIfEndOfGame();
            }

            if (EndOfGame)
            {
                endGame();
            }

            // Computer players make moves without waiting for interaction.
            if (!EndOfGame && CurrentPlayer.PlayerType != Player.PlayerTypes.Human)
            {
                OnMoveChanged();
            }
        }

        private bool checkIfEndOfGame()
        {
            bool end_of_game = false;
            bool winner = checkForWinner();
            if (MoveCount == 8)
            {
                // board is full.
                end_of_game = true;
            }
            else
            {
                MoveCount++;
            }

            if (WinnerExists)
            {
                if (PlayerOne.Winner)
                {
                    WinnersMoves = PlayerOne.Moves;
                }
                else if (PlayerTwo.Winner)
                {
                    WinnersMoves = PlayerTwo.Moves;
                }
                end_of_game = true;
            }
            else if (winner)
            {
                end_of_game = true;
            }
            else
            {
                switchCurrentPlayer();
            }
            return end_of_game;
        }

        private void switchCurrentPlayer()
        {
            if (CurrentPlayer == PlayerOne)
            {
                CurrentPlayer = PlayerTwo;
            }
            else
            {
                CurrentPlayer = PlayerOne;
            }
        }

        public void OnButtonClick(int position)
        {
            // Find the related button.
            Button button;
            switch (position)
            {
                case 0:
                    button = Visual.TopLeft;
                    break;
                case 1:
                    button = Visual.TopMiddle;
                    break;
                case 2:
                    button = Visual.TopRight;
                    break;
                case 3:
                    button = Visual.MiddleLeft;
                    break;
                case 4:
                    button = Visual.Center;
                    break;
                case 5:
                    button = Visual.MiddleRight;
                    break;
                case 6:
                    button = Visual.BottomLeft;
                    break;
                case 7:
                    button = Visual.BottomMiddle;
                    break;
                case 8:
                    button = Visual.BottomRight;
                    break;
                default:
                    button = null;
                    break;
            }
            // if valid move then set clicked.
            button.Content = CurrentPlayer.Symbol;
        }

        public bool isValidMove(Button move)
        {
            bool valid = false;
            if ((move.Content as string) == null)
            {
                valid = true;
            }
            return valid;
        }

        public int convertButtonToPosition(Button button)
        {
            int position = -1;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);
            position = 3 * row + col;
            return position;
        }

        private bool checkForWinner()
        {
            bool winner = false;
            char winning_symbol = ' ';
            // Check for eight winning patterns
            if (Visual.TopLeft.Content != null &&
                Visual.TopLeft.Content.Equals(Visual.TopMiddle.Content) &&
                Visual.TopLeft.Content.Equals(Visual.TopRight.Content))
            {
                winner = true;
                winning_symbol = (char)Visual.TopLeft.Content;
            }
            else if (Visual.MiddleLeft.Content != null &&
                Visual.MiddleLeft.Content.Equals(Visual.Center.Content) &&
                Visual.MiddleLeft.Content.Equals(Visual.MiddleRight.Content))
            {
                winner = true;
                winning_symbol = (char)Visual.MiddleLeft.Content;
            }
            else if (Visual.BottomLeft.Content != null &&
                Visual.BottomLeft.Content.Equals(Visual.BottomMiddle.Content) &&
                Visual.BottomLeft.Content.Equals(Visual.BottomRight.Content))
            {
                winner = true;
                winning_symbol = (char)Visual.BottomLeft.Content;
            }
            else if (Visual.TopLeft.Content != null &&
                Visual.TopLeft.Content.Equals(Visual.MiddleLeft.Content) &&
                Visual.TopLeft.Content.Equals(Visual.BottomLeft.Content))
            {
                winner = true;
                winning_symbol = (char)Visual.TopLeft.Content;
            }
            else if (Visual.TopMiddle.Content != null &&
                Visual.TopMiddle.Content.Equals(Visual.Center.Content) &&
                Visual.TopMiddle.Content.Equals(Visual.BottomMiddle.Content))
            {
                winner = true;
                winning_symbol = (char)Visual.TopMiddle.Content;
            }
            else if (Visual.TopRight.Content != null &&
                Visual.TopRight.Content.Equals(Visual.MiddleRight.Content) &&
                Visual.TopRight.Content.Equals(Visual.BottomRight.Content))
            {
                winner = true;
                winning_symbol = (char)Visual.TopRight.Content;
            }
            else if (Visual.TopLeft.Content != null &&
                Visual.TopLeft.Content.Equals(Visual.Center.Content) &&
                Visual.TopLeft.Content.Equals(Visual.BottomRight.Content))
            {
                winner = true;
                winning_symbol = (char)Visual.TopLeft.Content;
            }
            else if (Visual.TopRight.Content != null &&
                Visual.TopRight.Content.Equals(Visual.Center.Content) &&
                Visual.TopRight.Content.Equals(Visual.BottomLeft.Content))
            {
                winner = true;
                winning_symbol = (char)Visual.TopRight.Content;
            }

            if (winner)
            {
                if (PlayerOne.Symbol == winning_symbol)
                {
                    PlayerOne.Winner = true;
                    WinnersMoves = PlayerOne.Moves;
                }
                else
                {
                    PlayerTwo.Winner = true;
                    WinnersMoves = PlayerTwo.Moves;
                }
                WinnerExists = true;
            }

            return winner;
        }
    }
}

