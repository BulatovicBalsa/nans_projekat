using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Brick_Breaker.Model
{
    class Cigla
    {
        private Rectangle blok;

        public Rectangle Blok
        {
            get { return blok; }
            set { blok = value; }
        }

        private int health;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public void ChangeColor()
        {
            SolidColorBrush boja;
            if (health == 2)
                boja = Brushes.DarkGray;
            else
                boja = Brushes.Red;
            blok.Fill = boja;
        }
        public Cigla(Rectangle r, int h)
        {
            blok = r;
            health = h;
            ChangeColor();
            
        }
        public bool Pogodjen(ref int score, Label lb)
        {
            score ++;
            lb.Content = score;
            health--;
            ChangeColor();
            if(health == 0)
            {
                return true;
            }
            return false;
        }
    }
}
