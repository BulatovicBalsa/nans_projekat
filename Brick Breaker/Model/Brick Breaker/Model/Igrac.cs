using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Brick_Breaker.Model
{
    class Igrac
    {
        private double wid;

        private double visina = 395;

        private Rectangle rect;

        public Rectangle Rect
        {
            get { return rect; }
            //set { rect = value; }
        }

        private int pomeraj;

        public int Pomeraj
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
