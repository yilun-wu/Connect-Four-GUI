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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

namespace ConnectFour
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.IO.Ports.SerialPort serialPort1;
        string RxString;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartUSBCommunication()
        {
            this.serialPort1 = new System.IO.Ports.SerialPort(new System.ComponentModel.Container());
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);


            serialPort1.PortName = "COM4";
            serialPort1.BaudRate = 9600;

            serialPort1.Open();
        }





        private Game theGame = new Game();
        private Ellipse[,] pieces;

        private void AddPieces()
        {
            pieces = new Ellipse[Game.nCols, Game.nRows];
            const int size = 80;
            for (int i = 0; i < Game.nCols; ++i)
            {
                for (int j = 0; j < Game.nRows; ++j)
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

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RxString = serialPort1.ReadExisting();
            char number = RxString[0];
            int val;
            switch (number)
            {
                case '1': val = 0; break;
                case '2': val = 1; break;
                case '3': val = 2; break;
                case '4': val = 3; break;
                case '5': val = 4; break;
                case '6': val = 5; break;
                case '7': val = 6; break;
                default: val = 3; break;
            }
            theGame.PlayAt(Game.PlayerType.SecondPlayer, val);
            this.Dispatcher.Invoke(Refresh);
        }

        private void Refresh()
        {
            reset_label();
            check_warn_ver();
            check_warn_hor();
            check_warn_dia();
            var colorMap = new Dictionary<Game.PlayerType, Brush> {
                {Game.PlayerType.Empty, Brushes.Black},
                {Game.PlayerType.FirstPlayer, Brushes.Red},
                {Game.PlayerType.SecondPlayer, Brushes.Blue}
            };
            var colorMap2 = new Dictionary<Game.LabelType, Brush> {
                {Game.LabelType.Warning,Brushes.Orange}
            };

            for (int i = 0; i < Game.nCols; ++i)
            {
                for (int j = 0; j < Game.nRows; ++j)
                {
                    //TODO
                    pieces[i, j].Fill = colorMap[theGame.GetPiece(i, j)];
                    if (theGame.GetLabel(i, j) != Game.LabelType.None) pieces[i, j].Fill = colorMap2[theGame.GetLabel(i, j)];
                }
            }
            tb.Text = String.Format("Scores: {0}/{1}", theGame.CalculateScore(Game.PlayerType.FirstPlayer),
                theGame.CalculateScore(Game.PlayerType.SecondPlayer));
        }

        void reset_label()
        {
            for (int i = 0; i < Game.nCols; ++i)
            {
                for (int j = 0; j < Game.nRows; ++j)
                {
                    theGame.SetLabel(Game.LabelType.None, i, j);
                }
            }
        }

        void check_warn_ver()
        {
            for (int i = 0; i < Game.nCols; ++i)
            {
                for (int j = 0; j < Game.nRows - 3; ++j)
                {
                    if (theGame.GetPiece(i, j) == Game.PlayerType.SecondPlayer && theGame.GetPiece(i, j + 1) == Game.PlayerType.SecondPlayer && theGame.GetPiece(i, j + 2) == Game.PlayerType.SecondPlayer && theGame.GetPiece(i, j + 3) == Game.PlayerType.Empty)
                    {
                        theGame.SetLabel(Game.LabelType.Warning, i, j + 3);
                    }
                    //else theGame.SetLabel(Game.LabelType.None, i, j + 3);
                }
            }
        }

        void check_warn_hor()
        {
            for (int i = 0; i < Game.nRows; ++i)
            {
                for (int j = 0; j < Game.nCols - 3; ++j)
                {
                    bool dead = false;
                    int num_spot = 0;
                    for (int k = j; k <= j + 3; ++k)
                    {
                        if (theGame.GetPiece(k, i) == Game.PlayerType.SecondPlayer) { num_spot++; }
                        if (theGame.GetPiece(k, i) == Game.PlayerType.FirstPlayer) { dead = true; break; }
                    }
                    if (dead == false && num_spot >= 1)
                    {
                        for (int k = j; k <= j + 3; ++k)
                        {
                            if (theGame.GetPiece(k, i) == Game.PlayerType.Empty) { theGame.SetLabel(Game.LabelType.Warning, k, i); }
                        }

                    }
                }
            }
        }

        void check_warn_dia()
        {
            for (int i = 0; i < 3; ++i) //row number
            {
                for (int j = 3; j < Game.nCols; ++j)
                {
                    bool dead = false;
                    int num_spot = 0;
                    for (int k = 0; k < 4; ++k)
                    {
                        if (theGame.GetPiece(j - k, i + k) == Game.PlayerType.SecondPlayer) { num_spot++; }
                        if (theGame.GetPiece(j - k, i + k) == Game.PlayerType.FirstPlayer) { dead = true; break; }
                    }
                    if (dead == false && num_spot >= 1)
                    {
                        for (int k = 0; k < 4; ++k)
                        {
                            if (theGame.GetPiece(j - k, i + k) == Game.PlayerType.Empty) { theGame.SetLabel(Game.LabelType.Warning, j - k, i + k); }
                        }

                    }
                }
            }

            for (int i = 0; i < 3; ++i) //row number
            {
                for (int j = 3; j > 0; --j)
                {
                    bool dead = false;
                    int num_spot = 0;
                    for (int k = 0; k < 4; ++k)
                    {
                        if (theGame.GetPiece(j + k, i + k) == Game.PlayerType.SecondPlayer) { num_spot++; }
                        if (theGame.GetPiece(j + k, i + k) == Game.PlayerType.FirstPlayer) { dead = true; break; }
                    }
                    if (dead == false && num_spot >= 1)
                    {
                        for (int k = 0; k < 4; ++k)
                        {
                            if (theGame.GetPiece(j + k, i + k) == Game.PlayerType.Empty) { theGame.SetLabel(Game.LabelType.Warning, j + k, i + k); }
                        }

                    }
                }
            }
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
            StartUSBCommunication();
        }


    }
}
