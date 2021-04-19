using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Minesweeper
{
    class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Tile[,] TileBoard { get; set; }

        public int MineCount { get; set; }

        public int RevealedCount { get; set; }


        private static string flagPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\Assets\flag.png");
        private static string minePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\Assets\mine.png");

        public Board() { }

        public Board(int width, int height, int mineCount)
        {
            Width = width;
            Height = height;
            TileBoard = new Tile[width, height];
            MineCount = mineCount;
            GenerateBoard();
            Console.WriteLine($"Created new Board with dimensions ({width}, {height}) and {mineCount} mines.");
        }

        private void GenerateBoard()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    TileBoard[i, j] = new Tile(i, j, false);
                    Console.WriteLine($"Created new Tile at ({i}, {j}).");
                }
            }
            AddMines();
        }

        private void AddMines()
        {
            Random random = new Random();
            int mines = 0;

            for (int i = 0; i < MineCount; i++)
            {
                int rx = random.Next(0, Width);
                int ry = random.Next(0, Height);
                TileBoard[rx, ry].IsMine = true;
                mines++;
                //TileBoard[rx, ry].Content = "X";
                Console.WriteLine($"Replaced Tile at ({rx}, {ry}) with new mine Tile.");
            }
        }

        public void RevealBoard()
        {
            foreach (Tile t in TileBoard)
            {
                t.IsEnabled = false;
            }

            foreach (Tile t in TileBoard)
            {
                if (t.IsMine)
                {
                    t.Content = new Image
                    {
                        Source = new BitmapImage(new Uri(minePath)),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                }
            }
        }

        public void ClearBoard()
        {
            RevealedCount = 0;
            Array.Clear(TileBoard, 0, TileBoard.Length);
            Console.WriteLine("Cleared board");
        }

        public void ResetBoard()
        {
            ClearBoard();
            GenerateBoard();
            foreach (Tile t in TileBoard)
            {
                t.IsEnabled = true;
                t.Content = "";
            }
            Console.WriteLine("Reset Board");
        }
    }
}
