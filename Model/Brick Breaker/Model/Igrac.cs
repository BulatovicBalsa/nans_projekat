using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Brick_Breaker.Model
{
    class Igrac
    {
        private double wid;

        private double visina = 369;

        private Rectangle rect;

        public Rectangle Rect
        {
            get { return rect; }
            //set { rect = value; }
        }

        private double pomeraj;

        public double Pomeraj
        {
            get { return pomeraj; }
            set { pomeraj = value; }
        }

        public Igrac(Rectangle r, double width)
        {
            pomeraj = 3;
            rect = r;
            isLaunched = false;
            wid = width;
            BaseSet();
        }

        private bool isLaunched;

        public bool IsLaunched
        {
            get { return isLaunched; }
            set { isLaunched = value; }
        }

        public void EulerPomeraj(double dt, ref Point vi, Point F, double m)
        {
            Point pi = new Point(Canvas.GetLeft(rect), Canvas.GetTop(rect));
            Point dv = new Point(vi.X * dt, vi.Y * dt);
            pi = new Point(pi.X + dv.X, pi.Y + dv.Y);
            Canvas.SetLeft(rect, pi.X);
            Canvas.SetTop(rect, pi.Y);

            dv = new Point(dt*(F.X / m), dt*(F.Y / m));
            vi.X += dv.X;
            vi.Y += dv.Y;
        }

        public void MoveRight(double width, Loptica el, double dt, ref Point vi, Point F, double m)
        {
            EulerPomeraj(dt, ref vi, F, m);
            Boundries(el, ref vi, width);
        }

        public void MoveLeft(double width, Loptica el, double dt, ref Point vi, Point F, double m)
        {
            F.X = -F.X;
            F.Y = -F.Y;
            EulerPomeraj(dt, ref vi, F, m);
            Boundries(el, ref vi, width);
        }
        private void Boundries(Loptica el, ref Point vi, double width)
        {
            if (Canvas.GetLeft(rect) < 0)
            {
                Canvas.SetLeft(rect, 0);
                vi.X = vi.Y = 0;
            }
            if (Canvas.GetLeft(rect) + rect.Width > width)
            {
                Canvas.SetLeft(rect, width - rect.Width);
                vi.X = vi.Y = 0;
            }
            if (!isLaunched)
            {
                if (Canvas.GetLeft(rect) + rect.Width < Canvas.GetLeft(el.Ell) + el.Ell.Width)
                {
                    Canvas.SetLeft(rect, Canvas.GetLeft(el.Ell) + el.Ell.Width-rect.Width);
                    vi.X = vi.Y = 0;
                }

                if (Canvas.GetLeft(rect) > Canvas.GetLeft(el.Ell))
                {
                    Canvas.SetLeft(rect, Canvas.GetLeft(el.Ell));
                    vi.X = vi.Y = 0;
                }
            }
        }
        public void MoveRight(double width,Loptica el)
        {
            if (Canvas.GetLeft(rect) + rect.Width + pomeraj <= width)
                Canvas.SetLeft(rect, Canvas.GetLeft(rect) + pomeraj);
            if(!isLaunched)
            {
                if (Canvas.GetLeft(rect) >= Canvas.GetLeft(el.Ell))
                    Canvas.SetLeft(rect, Canvas.GetLeft(rect) - pomeraj);
            }
        }
        public void MoveLeft(Loptica el)
        {
            if (Canvas.GetLeft(rect) - pomeraj > 0)
                Canvas.SetLeft(rect, Canvas.GetLeft(rect) - pomeraj);
            if (!isLaunched)
            {
                if (Canvas.GetLeft(rect) + rect.Width <= Canvas.GetLeft(el.Ell) + el.Ell.Width)
                    Canvas.SetLeft(rect, Canvas.GetLeft(rect) + pomeraj);
            }
        }
        public void BaseSet()
        {
            Canvas.SetLeft(rect, wid / 2 - rect.Width / 2);
            Canvas.SetTop(rect, visina);
        }
    }
}
