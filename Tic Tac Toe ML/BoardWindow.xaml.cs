using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tic_Tac_Toe_ML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private BoardViewModel m_view_model;

        private List<Button> m_button_list = new List<Button>();

        public BoardViewModel ViewModel
        {
            get
            {
                return m_view_model;
            }
        }

        public List<Button> ButtonList
        {
            get
            {
                return m_button_list;
            }
        }

        public BoardWindow()
        {
            InitializeComponent();
        }

        private async void Window_ContentRendered(object sender, EventArgs e)
        {
            // Add listiner to each button.
            foreach (Button button in FindBoardObjects<Button>(this))
            {
                button.Click += new RoutedEventHandler(OnButtonClick);
                ButtonList.Add(button);
            }

            m_view_model = new BoardViewModel(this);
 
            await Dispatcher.InvokeAsync(() =>
            {
                ViewModel.Start();
            });
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int potential_move = ViewModel.convertButtonToPosition(button);
            // if valid move then set clicked.
            if (ViewModel.isValidMove(button))
            {
                ViewModel.Clicked = potential_move;
                button.Content = ViewModel.CurrentPlayer.Symbol;
                ViewModel.OnMoveChanged();
            }
        }

        public IEnumerable<T> FindBoardObjects<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindBoardObjects<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
