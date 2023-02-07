using Brick_Breaker.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Brick_Breaker.Model
{
    internal class MyDrop
    {
        private static MyDrop instance = new MyDrop();

        private static Ellipse krug;

        private static bool alive;

        private double pomeraj = 2;

        public static MyDrop Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MyDrop();
                }
                return instance;
            }
        }

        public bool Alive { get { return alive; } set { alive = value; } }
        public Ellipse Krug { get { return krug; } set { krug = value; } }

        private MyDrop()
        {
            krug = new Ellipse();
            alive = false;
        }

        public void Dissappear()
        {
            krug.Visibility = Visibility.Collapsed;
            alive = false;
        }

        public bool Collision(Canvas canMain, Igrac igrac, ref int score, Ellipse[] niz, Blokovi blokovi)
        {
            if (Canvas.GetTop(krug) + krug.ActualHeight >= canMain.Height)
            {
                Dissappear();
                return true;
            }
            SeparatingAxisTheorem sat = new SeparatingAxisTheorem();
            if(Canvas.GetTop(krug) < 0)
            {
                pomeraj = 2;
            }

            for (int i = blokovi.Bricks.GetLength(0) - 1; i > -1; i--)
            {
                for (int j = blokovi.Bricks.GetLength(1) - 1; j > -1; j--)
                {
                    if (blokovi.Bricks[i, j] == null)
                        continue;
                    if (sat.DetectCollision(blokovi.Bricks[i, j].Blok, this))
                    {
                        pomeraj = 2;
                        continue;
                    }
                }
            }

                    foreach (Ellipse el in niz)
            {
                if (sat.DetectCollision(el, this))
                {
                    pomeraj = -2;
                }
            }

            if (sat.DetectCollision(igrac.Rect, this))
            {
                score++;
                
                Dissappear();
                return true;
            }
            return false;
        }

        public void Spawn(Rectangle rectangle, Canvas canMain)
        {
            alive = true;
            Canvas.SetZIndex(krug, 1);
            krug.Visibility = Visibility.Visible;
            Canvas.SetTop(krug, Canvas.GetTop(rectangle) + rectangle.Height / 2);
            Canvas.SetLeft(krug, Canvas.GetLeft(rectangle) + rectangle.Width / 2);
            krug.Width = 19;
            krug.Height = 19;
            krug.Fill = Brushes.AliceBlue;
            if(!canMain.Children.Contains(krug))
                canMain.Children.Add(krug);
        }

        public void Pomeraj()
        {
            Canvas.SetTop(krug, Canvas.GetTop(krug) + pomeraj);
        }
    }
}
