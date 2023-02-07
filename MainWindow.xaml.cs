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
using Brick_Breaker.Model;
using Brick_Breaker.Utils;

namespace Brick_Breaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int lifes = 5;
        int score = 0;
        const int brLifes = 5;

        DispatcherTimer timerLoptica = new DispatcherTimer();
        DispatcherTimer timerMove = new DispatcherTimer();

        Blokovi bricks;
        Igrac igrac;
        Loptica lopta;

        bool leftPress = false;
        bool rightPress = false;
        bool rad = false;
        public MainWindow()
        {
            InitializeComponent();
            //Width = canMain.Width + 16;
            //Width = gridMain.Width;
            ResizeMode = ResizeMode.NoResize;
            rectIgrac.RadiusX = 10;
            rectIgrac.RadiusY = 20;
            igrac = new Igrac(rectIgrac, canMain.Width);
            lopta = new Loptica(elLoptica);
            bricks = new Blokovi(canMain, lbLevel, igrac);

            Canvas.SetLeft(lopta.Ell, 140 - lopta.Ell.Width / 2);
            Canvas.SetLeft(igrac.Rect, 140 - igrac.Rect.Width / 2);


            Console.WriteLine(Width);

            timerLoptica.Interval = TimeSpan.FromMilliseconds(1000 / 100);

            timerMove.Interval = TimeSpan.FromMilliseconds(1000/120);
            timerMove.Tick += TimerMove_Tick;
            timerMove.Start();

        }

        private void TimerMove_Tick(object sender, EventArgs e)
        {
            if (leftPress)
                igrac.MoveLeft(lopta);
            else
                if (rightPress)
                    igrac.MoveRight(canMain.Width,lopta);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                leftPress = true;
            }
            else
            {
                if (e.Key == Key.Right)
                    {
                        rightPress = true;
                    }
                //igrac.MoveRight(canMain.Width);

            }
            if(e.Key == Key.Space)
            {
                if(!rad)
                {
                    rad = true;
                    timerLoptica.Start();
                    timerLoptica.Tick += TimerLoptica_Tick;
                    timerLoptica.Tick += TimerLoptica_Tick_Pogodjen;
                    lopta.ChangeUgao(igrac.Rect);
                    
                    igrac.IsLaunched = true;
                    
                }
            }
                //igrac.MoveLeft();
        }

        private void TimerLoptica_Tick_Pogodjen(object sender, EventArgs e)
        {
            if(bricks.Pogodjen(lopta, igrac, ref score,lbScore, ref rad))
            {
                timerLoptica.Tick -= TimerLoptica_Tick;
            }
        }

        private void TimerLoptica_Tick(object sender, EventArgs e)
        {
            lopta.Pomeraj(canMain.Width, canMain.Height, igrac, ref rad);
            if (rad == true)
                rad = false;
            else
            if (rad == false)
                rad = true;
            if (!rad)
            {
                timerLoptica.Stop();
                timerLoptica.Tick -= TimerLoptica_Tick;
                lifes--;
                lbLifes.Content = lifes + "/" + brLifes;
                if (lifes == 0)
                {
                    GameOver();
                    MessageBox.Show("game over");
                }

            }
            
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                leftPress = false;
            }
            else
            {
                if (e.Key == Key.Right)
                {
                    rightPress = false;
                }
                //igrac.MoveRight(canMain.Width);
            }

        }
        private void Init()
        {
            score = 0;
            lifes = brLifes;
            lbScore.Content = score;
            lbLifes.Content = lifes + "/" + brLifes;
            lbLevel.Content = 1;
            igrac.BaseSet();
            lopta.BaseSet(igrac);
            SeparatingAxisTheorem sat = new SeparatingAxisTheorem();
            sat.DetectCollision(lopta, igrac);
        }
        private void Brick_Breeaker_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            btnReset.Content = "PLAY AGAIN";
            btnReset.Visibility = Visibility.Collapsed;
            //lbHighScore

        }
        private void GameOver()
        {
            timerLoptica.Tick -= TimerLoptica_Tick;
            rad = true;
            timerMove.Tick -= TimerMove_Tick;
            btnReset.Visibility = Visibility.Visible;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            rad = false;
            timerMove.Tick += TimerMove_Tick;
            btnReset.Visibility = Visibility.Collapsed;
            Init();
            bricks.Reset();
        }
    }
}
