using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    static class Game
    {
        private enum Difficulties
        {
            EASY,
            MEDIUM,
            HARD,
            EXTREME
        };

        private static Difficulties difficulty = Difficulties.EASY;

        public static Board GameBoard;

        public static void InitGame(int d)
        { 
            switch (d)
            {
                case 0:
                    GameBoard = new Board(10, 10, 22);
                    break;
                case 1:
                    GameBoard = new Board(14, 14, 44);
                    break;
                case 2:
                    GameBoard = new Board(18, 18, 73);
                    break;
                case 3:
                    GameBoard = new Board(21, 21, 99);
                    break;
                default:
                    GameBoard = new Board(10, 10, 22);
                    break;
            }
        }

        public static int Score { get; set; }
        public static int Time { get; set; }

        public static void Win()
        {
            GameBoard.RevealBoard();
            MainWindow.DisplayWinMsg();
        }

        public static void Lose()
        {
            System.Threading.Thread.Sleep(50);
            GameBoard.RevealBoard();
            MainWindow.DisplayLoseMsg();
        }

        public static void Reset()
        {
            Time = 0;
            GameBoard.ResetBoard();
        }
    }
}
// TODO: Game over, Game win, Flag count, Timer, Reset button, Difficulties, Better visuals, dynamic sized buttons for weird grid sizes
