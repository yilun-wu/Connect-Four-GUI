using Microsoft.Win32;
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

namespace ConnectFour
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private Game theGame = new Game();
        private Ellipse[,] pieces;

        private void AddPieces()
        {
            pieces = new Ellipse[Game.nCols, Game.nRows];
            const int size = 80;
            for(int i=0; i< Game.nCols; ++i)
            {
                for (int j=0; j < Game.nRows; ++j)
                {
                    pieces[i, j] = new Ellipse();
                    pieces[i, j].Width = pieces[i, j].Height = size;
                    pieces[i, j].Fill = Brushes.Black;
                    pieces[i, j].Tag = new Game.Point(i, j);
                    pieces[i, j].MouseDown += Piece_Click;
                    Canvas.SetLeft(pieces[i, j], i * size);
                    Canvas.SetTop(pieces[i, j], (Game.nRows - j - 1) * size);
                    theCanvas.Children.Add(pieces[i, j]);
                }
            }
        }

        private void Piece_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                theGame.PlayAt(Game.PlayerType.FirstPlayer, ((Game.Point)(sender as Ellipse).Tag).Col);
            }
            else
            {
                theGame.PlayAt(Game.PlayerType.SecondPlayer, ((Game.Point)(sender as Ellipse).Tag).Col);
            }
            Refresh();
        }

        private void Refresh()
        {
            var colorMap = new Dictionary<Game.PlayerType, Brush> {
                {Game.PlayerType.Empty, Brushes.Black},
                {Game.PlayerType.FirstPlayer, Brushes.Red},
                {Game.PlayerType.SecondPlayer, Brushes.Blue}
            };
            for (int i = 0; i < Game.nCols; ++i)
            {
                for (int j = 0; j < Game.nRows; ++j)
                {
                    //TODO
                    pieces[i, j].Fill = colorMap[theGame.GetPiece(i, j)];
                }
            }
            tb.Text = String.Format("Scores: {0}/{1}",theGame.CalculateScore(Game.PlayerType.FirstPlayer),
                theGame.CalculateScore(Game.PlayerType.SecondPlayer));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void theCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void ui_mnuNew_Click(object sender, RoutedEventArgs e)
        {
            theGame.Reset();
            Refresh();
        }

        private void ui_mnuLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                theGame.Load(openFileDialog.FileName);
                Refresh();
            }
        }

        private void ui_mnuSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                theGame.Save(saveFileDialog.FileName);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddPieces();
        }


    }
}
