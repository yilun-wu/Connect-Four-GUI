﻿using Microsoft.Win32;
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
using System.IO.Ports;
using System.Threading;



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
        private StatusWindow statusWindow = new StatusWindow();

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
            //TODO: statusWindow.UpdatePosition
            tb.Text = String.Format("Scores: {0}/{1}",theGame.CalculateScore(Game.PlayerType.FirstPlayer),
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
                    for (int k = j; k <= j+3; ++k)
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
                        if (theGame.GetPiece(j-k, i+k) == Game.PlayerType.SecondPlayer) { num_spot++; }
                        if (theGame.GetPiece(j-k, i+k) == Game.PlayerType.FirstPlayer) { dead = true; break; }
                    }
                    if (dead == false && num_spot >= 1)
                    {
                        for (int k = 0; k <4; ++k)
                        {
                            if (theGame.GetPiece(j-k, i+k) == Game.PlayerType.Empty) { theGame.SetLabel(Game.LabelType.Warning, j-k, i+k); }
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
          //  Updated upstream;
            statusWindow.Show();

           // InitializeSerialCommunication();
        }

        static bool _continue;
        static SerialPort _serialPort;

        public static void InitializeSerialCommunication()
        {
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = SetPortName(_serialPort.PortName);
            _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
            _serialPort.Parity = SetPortParity(_serialPort.Parity);
            _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
            _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
            _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            _continue = true;
            readThread.Start();

            Console.Write("Name: ");
            name = Console.ReadLine();

            Console.WriteLine("Type QUIT to exit");

            while (_continue)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.WriteLine(
                        String.Format("<{0}>: {1}", name, message));
                }
            }

            readThread.Join();
            _serialPort.Close();
        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }

        // Display Port values and prompt user to enter a port. 
        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }
        // Display BaudRate values and prompt user to enter a value. 
        public static int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
            baudRate = Console.ReadLine();

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
            //Stashed changes;
        }

        // Display PortParity values and prompt user to enter a value. 
        public static Parity SetPortParity(Parity defaultPortParity)
        {
            string parity;

            Console.WriteLine("Available Parity options:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString(), true);
            parity = Console.ReadLine();

            if (parity == "")
            {
                parity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parity, true);
        }
        // Display DataBits values and prompt user to enter a value. 
        public static int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits;

            Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
            dataBits = Console.ReadLine();

            if (dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits.ToUpperInvariant());
        }

        // Display StopBits values and prompt user to enter a value. 
        public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits;

            Console.WriteLine("Available StopBits options:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter StopBits value (None is not supported and \n" +
             "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
            stopBits = Console.ReadLine();

            if (stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
        }
        public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            string handshake;

            Console.WriteLine("Available Handshake options:");
            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("End Handshake value (Default: {0}):", defaultPortHandshake.ToString());
            handshake = Console.ReadLine();

            if (handshake == "")
            {
                handshake = defaultPortHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
        }

    }


}

