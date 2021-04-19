using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Minesweeper
{
    class Tile : Button
    {
        public int PosX { get; set; }
        public int PosY { get; set; }

        public bool IsRevealed { get; set; }
        public bool IsMine { get; set; }
        public bool IsFlagged { get; set; }

        public int NearbyMines { get; set; }

        private static string flagPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\Assets\flag.png");
        private static string minePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\Assets\mine.png");

        public Tile() { }

        public Tile(int x, int y, bool isMine)
        {
            PosX = x;
            PosY = y;
            IsMine = isMine;

            FontWeight = FontWeights.UltraBold;
            FontSize = 14;
        }

        
        private bool DidWin()
        {
            return Game.GameBoard.RevealedCount == ((Game.GameBoard.Width * Game.GameBoard.Height) - Game.GameBoard.MineCount);
        }


        private int CountNearbyMines()
        {
            int count = 0;

            for (int x = PosX - 1; x <= PosX + 1; x++)
            {
                for (int y = PosY - 1; y <= PosY + 1; y++)
                {
                    if (x >= 0 && x < Game.GameBoard.Width && y >= 0 && y < Game.GameBoard.Height)
                    {
                        if (Game.GameBoard.TileBoard[x, y].IsMine)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return count;
        }


        private void SetTextColor(int mines, int x, int y)
        {
            switch (mines)
            {
                case 1:
                    Game.GameBoard.TileBoard[x, y].Foreground = new SolidColorBrush(Color.FromRgb(121, 222, 118));
                    break;
                case 2:
                    Game.GameBoard.TileBoard[x, y].Foreground = new SolidColorBrush(Color.FromRgb(109, 190, 222));
                    break;
                case 3:
                    Game.GameBoard.TileBoard[x, y].Foreground = new SolidColorBrush(Color.FromRgb(255, 195, 56));
                    break;
                case 4:
                    Game.GameBoard.TileBoard[x, y].Foreground = new SolidColorBrush(Color.FromRgb(153, 104, 227));
                    break;
                case 5:
                    Game.GameBoard.TileBoard[x, y].Foreground = new SolidColorBrush(Color.FromRgb(224, 90, 157));
                    break;
                case 6:
                    Game.GameBoard.TileBoard[x, y].Foreground = new SolidColorBrush(Color.FromRgb(76, 247, 252));
                    break;
                case 7:
                    Game.GameBoard.TileBoard[x, y].Foreground = new SolidColorBrush(Color.FromRgb(255, 33, 33));
                    break;
                case 8:
                    Game.GameBoard.TileBoard[x, y].Foreground = new SolidColorBrush(Color.FromRgb(105, 0, 0));
                    break;
            }
        }


        private void Reveal(int x, int y)
        {
            int cordsX;
            int cordsY;
            
            if (Game.GameBoard.TileBoard[x, y].CountNearbyMines() == 0)
            {
                for (int i = -1; i <= 2; i++)
                {
                    for (int j = -1; j <= 2; j++)
                    {
                        cordsX = x;
                        cordsY = y;

                        cordsX += j;
                        cordsY += i;

                        if (cordsX >= 0 && cordsX < Game.GameBoard.Width && cordsY >= 0 && cordsY < Game.GameBoard.Height)
                        {
                            if (!Game.GameBoard.TileBoard[cordsX, cordsY].IsRevealed && !Game.GameBoard.TileBoard[cordsX, cordsY].IsMine && !Game.GameBoard.TileBoard[cordsX, cordsY].IsFlagged)
                            {
                                Game.GameBoard.TileBoard[cordsX, cordsY].IsRevealed = true;
                                Game.GameBoard.TileBoard[cordsX, cordsY].IsEnabled = false;
                                Game.GameBoard.TileBoard[cordsX, cordsY].Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                Game.GameBoard.RevealedCount++;
                                if (DidWin())
                                {
                                    Game.Win();
                                    return;
                                }

                                if (Game.GameBoard.TileBoard[cordsX, cordsY].CountNearbyMines() != 0)
                                {
                                    int nearby = Game.GameBoard.TileBoard[cordsX, cordsY].CountNearbyMines();
                                    Game.GameBoard.TileBoard[cordsX, cordsY].Content = nearby;

                                    SetTextColor(nearby, cordsX, cordsY);
                                }

                                if (Game.GameBoard.TileBoard[cordsX, cordsY].CountNearbyMines() == 0)
                                {
                                    Reveal(cordsX, cordsY);
                                }
                                
                            }
                        }
                    }
                }
            }
            else
            {
                Content = CountNearbyMines();
                IsRevealed = true;
                IsEnabled = false;
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                SetTextColor(CountNearbyMines(), x, y);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (!IsFlagged)
            {
                if (IsMine)
                {
                    Background = new SolidColorBrush(Color.FromRgb(219, 43, 31));
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri(minePath)),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    Console.WriteLine("GAME OVER");
                    Game.Lose();
                    
                    // Do game over
                }
                else
                {
                    Reveal(PosX, PosY);
                    Game.GameBoard.RevealedCount++;
                    // Check if all non-mine tiles are clear, display score
                    if (DidWin())
                    {
                        Game.Win();
                        
                    }
                }
            }
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);
            
            if (!IsRevealed && !IsFlagged)
            {
                IsFlagged = true;
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(flagPath)),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Console.WriteLine($"Flagged Tile at ({PosX}, {PosY}");
                return;
            }
            if (IsFlagged)
            {
                IsFlagged = false;
                Content = " ";
                return;
            }
        }
    }
}
