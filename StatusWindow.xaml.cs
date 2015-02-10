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
using System.Windows.Shapes;

namespace ConnectFour
{
    /// <summary>
    /// StatusWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StatusWindow : Window
    {
        public StatusWindow()
        {
            InitializeComponent();
        }
        private void DrawObjects()
        {
            for (int i = 0; i < 7; ++i)
            {
                Line line = new Line();
                line.X1 = line.X2 = 50 * (i + 1);
                line.Y1 = 0;
                line.Y2 = this.Height;
                line.Stroke = Brushes.Black;
                theCanvas.Children.Add(line);
            }
            for (int i = 0; i < 8; ++i)
            {
                Line line = new Line();
                line.Y1 = line.Y2 = 50 * (i + 1);
                line.X1 = 0;
                line.X2 = this.Width;
                line.Stroke = Brushes.Black;
                theCanvas.Children.Add(line);
            }
        }
        private void CreateRobot()
        {
            robot = new Ellipse();
            robot.Width = robot.Height = 20;
            robot.Fill = Brushes.Green;

            mainArrow = new Line();
            mainArrow.X1 = mainArrow.X2 = 0;
            mainArrow.Y1 = mainArrow.Y2 = 0;
            mainArrow.StrokeThickness = 2;
            mainArrow.Stroke = Brushes.DarkBlue;

            arrowLine1 = new Line();
            arrowLine1.X1 = arrowLine1.X2 = 0;
            arrowLine1.Y1 = arrowLine1.Y2 = 0;
            arrowLine1.StrokeThickness = 2;
            arrowLine1.Stroke = Brushes.DarkBlue;

            arrowLine2 = new Line();
            arrowLine2.X1 = arrowLine2.X2 = 0;
            arrowLine2.Y1 = arrowLine2.Y2 = 0;
            arrowLine2.StrokeThickness = 2;
            arrowLine2.Stroke = Brushes.DarkBlue;
        }

        public void UpdatePosition(int robotX, int robotY, MoveDirection direction)
        {
            if (robot != null)
                theCanvas.Children.Remove(robot);
            if (mainArrow != null)
                theCanvas.Children.Remove(mainArrow);
            if (arrowLine1 != null)
                theCanvas.Children.Remove(arrowLine1);
            if (arrowLine2 != null)
                theCanvas.Children.Remove(arrowLine2);
            CreateRobot();
            Canvas.SetLeft(robot, robotX * 50 + 4 * 50 - 10);
            Canvas.SetTop(robot, -robotY * 50 + 9 * 50 - 10);
            Point forward = moveVector[direction];
            Point left = new Point(), right = new Point();
            if (forward.X == 0)
            {
                left.X = 1; right.X = -1;
                left.Y = right.Y = -forward.Y;
            }
            else
            {
                left.Y = 1; right.Y = -1;
                left.X = right.X = -forward.X;
            }
            mainArrow.X1 = robotX * 50 + 4 * 50;
            mainArrow.Y1 = -robotY * 50 + 9 * 50;
            mainArrow.X2 = mainArrow.X1 + forward.X * 30;
            mainArrow.Y2 = mainArrow.Y1 + forward.Y * 30;
            arrowLine1.X1 = arrowLine2.X1 = mainArrow.X2;
            arrowLine2.Y1 = arrowLine1.Y1 = mainArrow.Y2;
            arrowLine1.X2 = arrowLine1.X1 + left.X * 15;
            arrowLine1.Y2 = arrowLine1.Y1 + left.Y * 15;
            arrowLine2.X2 = arrowLine2.X1 + right.X * 15;
            arrowLine2.Y2 = arrowLine2.Y1 + right.Y * 15;
            theCanvas.Children.Add(mainArrow);
            theCanvas.Children.Add(robot);
            theCanvas.Children.Add(arrowLine1);
            theCanvas.Children.Add(arrowLine2);
        }

        public enum MoveDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        private Dictionary<MoveDirection, Point> moveVector = new Dictionary<MoveDirection, Point>
        {
            {MoveDirection.Up,new Point (0,-1)},
            {MoveDirection.Down,new Point (0,1)},
            {MoveDirection.Left,new Point (-1, 0)},
            {MoveDirection.Right,new Point (1, 0)},
        };
        private Ellipse robot;
        private Line mainArrow;
        private Line arrowLine1;
        private Line arrowLine2;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DrawObjects();
            UpdatePosition(0, 4, MoveDirection.Right);
            UpdatePosition(1, 3, MoveDirection.Left);
        }
    }
}
