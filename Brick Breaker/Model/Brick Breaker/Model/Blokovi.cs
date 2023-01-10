using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Brick_Breaker.Utils;

namespace Brick_Breaker.Model
{
    class Blokovi
    {
        Label lb;
        Igrac player;

        const int def_w = 4;
        const int def_h = 3;

        private int w = def_w;
        private int h = def_h;

        private int level;

        public int Level
        {
            get { return level = 1; }
            set { level = value; }
        }


        private Canvas cnv;

        public Canvas Cnv
        {
            get { return cnv; }
        }


        private int razmak = 2;
        private Cigla[,] bricks;

        public Cigla[,] Bricks
        {
            get { return bricks; }
            set { bricks = value; }
        }

        public Blokovi(Canvas c, Label lbLvl, Igrac ig)
        {
            cnv = c;
            MakeMatrix();
            lb = lbLvl;
            level = 1;
            player = ig;
        }


        private void MakeMatrix()
        {
            Random r = new Random();

            double sirina = cnv.Width - (h - 1) * razmak;
            sirina /= h;
            double visina = cnv.Height / 3 - (w - 1) * razmak;
            visina /= w;
            bricks = new Cigla[h, w];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    int tmp = r.Next(1, 3);
                    //MessageBox.Show(Convert.ToString( tmp));
                    Rectangle rect = new Rectangle();
                    rect.Width = sirina;
                    rect.Height = visina;
                    Canvas.SetLeft(rect, i * rect.Width + razmak * i);
                    Canvas.SetTop(rect, j * rect.Height + razmak * j);
                    bricks[i, j] = new Cigla(rect, tmp);
                    cnv.Children.Add(rect);
                }
            }
        }

        public void ClearAll()
        {
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if(bricks[i,j]!=null)
                    cnv.Children.Remove(bricks[i, j].Blok);
                }
            }
        }
        public void NoviNivo()
        {
            level++;
            if (level % 2 == 0)
            {
                w++;
            }
            else h++;
            MakeMatrix();
        }
        public bool Provera()
        {
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if(bricks[i,j]!=null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool Pogodjen(Loptica ball, Igrac igrac, ref int score, Label lb, ref bool rad)
        {
            SeparatingAxisTheorem sat = new SeparatingAxisTheorem();
            for (int i = bricks.GetLength(0) - 1; i > -1; i--)
            {
                for (int j = bricks.GetLength(1) - 1; j > -1; j--)
                {
                    if (bricks[i, j] == null)
                    {
                        continue;
                    }
                    Rectangle blok = bricks[i, j].Blok;
                    if (sat.DetectCollision(ball, blok))
                    {
                        Odbij(ball, blok);

                        if (bricks[i, j].Pogodjen(ref score, lb))
                        {
                            MyDrop d = MyDrop.Instance;
                            if (!d.Alive)
                                d.Spawn(bricks[i, j].Blok, cnv);

                            cnv.Children.Remove(bricks[i, j].Blok);
                            bricks[i, j] = null;
                        }

                        bool p = false;
                        ball.Pomeraj(cnv.Width, cnv.Height, igrac, ref p);
                        return LevelUp_Myb(ball, ref rad);
                    }
                }
            }
            return false;
            
        }
        public void Odbij(Loptica loptica, Rectangle rectangle)
        {
            Point centarLoptica = new Point(Canvas.GetLeft(loptica.Ell) + loptica.Ell.ActualWidth * Math.Sqrt(2) / 2, Canvas.GetTop(loptica.Ell) + loptica.Ell.ActualHeight * Math.Sqrt(2) / 2);
            Point centarRect = new Point(Canvas.GetLeft(rectangle) + rectangle.ActualWidth / 2, Canvas.GetTop(rectangle) + rectangle.ActualHeight / 2);
            if (centarLoptica.X >= Canvas.GetLeft(rectangle) && centarLoptica.X <= Canvas.GetLeft(rectangle) + rectangle.ActualWidth)
            {
                loptica.Odbij_Gore_Dole();
                return;
            }
            if(centarLoptica.Y >= Canvas.GetTop(rectangle) && centarLoptica.Y <= Canvas.GetTop(rectangle) + rectangle.ActualHeight)
            {
                loptica.Odbij_Desno_Levo();
                return;
            }
            loptica.Odbij_Gore_Dole();
            loptica.Odbij_Desno_Levo();
            return;
        }

        public bool LevelUp_Myb(Loptica ball, ref bool rad)
        {
            if(Provera())
            {
                MessageBox.Show($"NIVO {level} USPESNO RESEN", "BRAVO");
                NoviNivo();
                lb.Content = $"{level}";
                player.BaseSet();
                ball.BaseSet(player);
                rad = false;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            ClearAll();
            level = 1;
            w = def_w;
            h = def_h;
            MakeMatrix();
        }

    }
}
