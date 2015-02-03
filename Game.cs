using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace ConnectFour
{
    class Game
    {
        public enum PlayerType
        {
            Empty,
            FirstPlayer,
            SecondPlayer
        }

        public struct Point
        {
            public int Col;
            public int Row;

            public Point(int col, int row)
            {
                Col = col;
                Row = row;
            }
        }

        private PlayerType[,] board;
        public const int nRows = 6;
        public const int nCols = 7;
        private List<List<Point>> lines = new List<List<Point>>();
        
        public Game()
        {
            InitializeLines();
            Reset();
        }

        public void Reset()
        {
            board = new PlayerType[nCols, nRows];
            
            for (int i = 0; i < nCols; ++i)
            {
                for (int j = 0; j < nRows; ++j)
                {
                    board[i, j] = PlayerType.Empty;
                }
            }
        }

        public PlayerType GetPiece(int nCol, int nRow)
        {
            return board[nCol, nRow];
        }

        private void InitializeLines()
        {
            List<Point> currentLine;
            for (int i = 0; i < nCols; ++i)
            {
                currentLine = new List<Point>();
                for (int j = 0; j < nRows; ++j)
                {
                    currentLine.Add(new Point(i, j));
                }
                lines.Add((currentLine));
            }
            for (int j = 0; j < nRows; ++j)
            {
                currentLine = new List<Point>();
                for (int i = 0; i < nCols; ++i)
                {
                    currentLine.Add(new Point(i, j));
                }
                lines.Add((currentLine));
            }

            for (int j = 0; j < nRows; ++j)
            {
                Point p = new Point(0, j);
                currentLine = new List<Point>();
                while (p.Col >= 0 && p.Col < nCols && p.Row >= 0 && p.Row < nRows)
                {
                    currentLine.Add(p);
                    p.Col ++;
                    p.Row ++;
                }
                lines.Add(currentLine);

                p = new Point(0, j);
                currentLine = new List<Point>();
                while (p.Col >= 0 && p.Col < nCols && p.Row >= 0 && p.Row < nRows)
                {
                    currentLine.Add(p);
                    p.Col++;
                    p.Row--;
                }
                lines.Add(currentLine);

                p = new Point(nCols - 1, j);
                currentLine = new List<Point>();
                while (p.Col >= 0 && p.Col < nCols && p.Row >= 0 && p.Row < nRows)
                {
                    currentLine.Add(p);
                    p.Col--;
                    p.Row++;
                }
                lines.Add(currentLine);

                p = new Point(nCols - 1, j);
                currentLine = new List<Point>();
                while (p.Col >= 0 && p.Col < nCols && p.Row >= 0 && p.Row < nRows)
                {
                    currentLine.Add(p);
                    p.Col--;
                    p.Row--;
                }
                lines.Add(currentLine);
            }
            lines = lines.Where(r => r.Count >= 4).ToList();
        }

        public bool PlayAt(PlayerType player, int nCol)
        {
            if (nCol < 0 || nCol >= nCols) 
                return false;
            for (int i = 0; i < nRows; ++i)
            {
                if (board[nCol, i] == PlayerType.Empty)
                {
                    board[nCol, i] = player;
                    return true;
                }
            }
            return false;
        }


        public int CalculateSets(PlayerType player)
        {
            int sets = 0;
            foreach (var line in lines)
            {
                int pieceInLine = 0;
                foreach (var piece in line)
                {
                    if (board[piece.Col, piece.Row] == player)
                        pieceInLine ++;
                    else
                        pieceInLine = 0;
                    if (pieceInLine >= 4)
                    {
                        sets ++;
                        break;
                    }
                }
            }
            return sets;
        }

        public int CalculateNumberOfPieces(PlayerType player)
        {
            int pieces = 0;
            for (int i = 0; i < nCols; ++i)
            {
                for (int j = 0; j < nRows; ++j)
                {
                    if (board[i, j] == player)
                        pieces++;
                }
            }
            return pieces;
        }

        public int CalculateScore(PlayerType player)
        {
            if (player == PlayerType.Empty)
                throw new Exception();
            PlayerType opponent = player == PlayerType.FirstPlayer ? PlayerType.SecondPlayer : PlayerType.FirstPlayer;
            return CalculateNumberOfPieces(player) + Math.Max(0, 4 * CalculateSets(player) - 2 * CalculateSets(opponent));
        }

        public void Load(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Open))
            {
                using (StreamReader sw = new StreamReader(fs))
                {
                    for (int i = 0; i < nCols; ++i)
                    {
                        for (int j = 0; j < nRows; ++j)
                        {
                            board[i, j] = (PlayerType)sw.Read();
                        }
                    }
                }
            }
        }

        public void Save(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.CreateNew))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (int i = 0; i < nCols; ++i)
                    {
                        for (int j = 0; j < nRows; ++j)
                        {
                            sw.Write((char)board[i, j]);
                        }
                    }
                }
            }
        }

    }
}
