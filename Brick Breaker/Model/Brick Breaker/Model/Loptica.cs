using Brick_Breaker.Utils;
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
    class Loptica
    {
        private double ugao;

        public double Ugao
        {
            get { return ugao; }
            set { ugao = value; }
        }
        private int brzina;

        public int Brzina
        {
            get { return brzina; }
            set { brzina = value; }
        }

        private Rectangle ell;

        public Rectangle Ell
        {
            get { return ell; }
            set { ell = value; }
        }

        public Loptica(Rectangle e)
        {
            ell = e;
            brzina = 5;
            Ugao = 0;
        }

        public void Pomeraj(double width, double height, Igrac igrac, ref bool res)
        {
            //MessageBox.Show($"{Canvas.GetLeft(ell)}");
            if(Canvas.GetLeft(ell) < 0) {
                Canvas.SetLeft(Ell, 0);
            }
            if (Canvas.GetLeft(ell) >= width - ell.Width * Math.Sqrt(2) || Canvas.GetLeft(ell) <= 0)
            {
                ugao = - ugao;
            }
            if (Canvas.GetTop(ell) <= 0)
            {
                ugao = 180 - ugao;
            }
            if (ProveraLokacije(igrac))
            {
                ChangeUgao(igrac.Rect);
                //ugao = 180 - ugao;
            }
            res = Reset(igrac,height);
            if(!res)
            {
                double radians = (Math.PI / 180) * ugao;
                Vector vector = new Vector { X = Math.Sin(radians), Y = -Math.Cos(radians) };
                Canvas.SetLeft(ell, Canvas.GetLeft(Ell) + vector.X * brzina);
                Canvas.SetTop(ell, Canvas.GetTop(Ell) + vector.Y * brzina);
            }
        }

        private bool ProveraLokacije(Igrac igrac)
        {
            bool res = false;
            //res = Canvas.GetLeft(ell) + ell.Width >= Canvas.GetLeft(igrac.Rect) && /*(Canvas.GetLeft(ell) >= Canvas.GetLeft(igrac.Rect) &&*/ Canvas.GetLeft(ell) <= Canvas.GetLeft(igrac.Rect) + igrac.Rect.Width && Canvas.GetTop(igrac.Rect) > Canvas.GetTop(ell) && Canvas.GetTop(igrac.Rect) < Canvas.GetTop(ell) + ell.Height;
            SeparatingAxisTheorem sat = new SeparatingAxisTheorem();
            res = sat.DetectCollision(this, igrac.Rect);
            return res;
        }
        private bool Reset(Igrac rectIgrac, double height)
        {
            if (Canvas.GetTop(ell) + ell.Height >= height)
            {
                //timer.Stop();
                Random random = new Random();
                Canvas.SetTop(ell, Canvas.GetTop(rectIgrac.Rect) - ell.ActualHeight*Math.Sqrt(2));
                Canvas.SetLeft(ell, Canvas.GetLeft(rectIgrac.Rect) + rectIgrac.Rect.Width / 2 - ell.Width / 2 + random.NextDouble() * 30);
                rectIgrac.IsLaunched = false;

                return true;
            }
            return false;
        }
        public void BaseSet(Igrac rectIgrac)
        {
            Random random = new Random();
            Canvas.SetTop(ell, Canvas.GetTop(rectIgrac.Rect) - ell.ActualHeight * Math.Sqrt(2));
            Canvas.SetLeft(ell, Canvas.GetLeft(rectIgrac.Rect) + rectIgrac.Rect.Width / 2 - ell.Width / 2 + random.NextDouble() * 30);
            rectIgrac.IsLaunched = false;
        }
        public void ChangeUgao(Rectangle igrac)
        {
            double angle = 65;
            double wig = Canvas.GetLeft(igrac) + igrac.Width / 2;
            double wl = Canvas.GetLeft(ell) + ell.Width / 2;
            double x = wl - wig;

            wig = igrac.Width / 2;
            ugao = x / wig * angle;
            if (ugao > angle)
                ugao = angle;
            if (ugao < -angle)
                ugao = -angle;

        }

        public void Odbij_Gore_Dole()
        {
            ugao = 180 - ugao;
        }
        public void Odbij_Desno_Levo()
        {
            ugao = - ugao;
        }
    }
}
