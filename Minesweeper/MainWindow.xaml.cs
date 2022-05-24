using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Game.InitGame(1);
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            
            for (int i = 0; i < Game.GameBoard.Width; i++)
            {
                GridBoard.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int j = 0; j < Game.GameBoard.Height; j++)
            {
                GridBoard.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < Game.GameBoard.Width; i++)
            {
                for (int j = 0; j < Game.GameBoard.Height; j++)
                {
                    Game.GameBoard.TileBoard[i, j].Width = GridBoard.Width / Game.GameBoard.Width;
                    Game.GameBoard.TileBoard[i, j].Height = GridBoard.Height / Game.GameBoard.Height;

                    Grid.SetColumn(Game.GameBoard.TileBoard[i, j], i);
                    Grid.SetRow(Game.GameBoard.TileBoard[i, j], j);
                    GridBoard.Children.Add(Game.GameBoard.TileBoard[i, j]);
                    //Console.WriteLine("Added Tile to window");
                }
            }
        }

        private void Reset()
        {
            Game.Reset();

            for (int i = 0; i < Game.GameBoard.Width; i++)
            {
                for (int j = 0; j < Game.GameBoard.Height; j++)
                {
                    Game.GameBoard.TileBoard[i, j].Width = GridBoard.Width / Game.GameBoard.Width;
                    Game.GameBoard.TileBoard[i, j].Height = GridBoard.Height / Game.GameBoard.Height;

                    Grid.SetColumn(Game.GameBoard.TileBoard[i, j], i);
                    Grid.SetRow(Game.GameBoard.TileBoard[i, j], j);
                    GridBoard.Children.Add(Game.GameBoard.TileBoard[i, j]);
                }
            }
        }

        public static void DisplayLoseMsg()
        {
            MessageBox.Show("Game Over :(");
            
        }

        public static void DisplayWinMsg()
        {
            MessageBox.Show("You Win!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Game.Reset();
            Reset();
            Console.WriteLine(CbDifficulty.SelectedIndex.ToString());
            Game.InitGame(CbDifficulty.SelectedIndex);
            InitializeBoard();
        }
    }
}
