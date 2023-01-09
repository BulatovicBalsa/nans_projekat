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

        /*public Blokovi(int w, int h, double sirina, double height)
        {
            bricks = new Cigla[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Random r = new Random();
                    int tmp = r.Next(1, 3);
                    Rectangle rect = new Rectangle();
                    rect.Width = sirina - (w - 1) * razmak;
                    rect.Width /= w;
                    rect.Height = height / 2 - (h - 1) * razmak;
                    rect.Height /= h;
                    Canvas.SetLeft(rect,j * rect.Width + razmak * j);
                    Canvas.SetTop(rect, i * rect.Height + razmak * i);
                    bricks[i, j] = new Cigla(rect, tmp);
                }
            }
        }*/
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
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j] == null)
                    {
                        continue;
                    }
                    Rectangle blok = bricks[i, j].Blok;
                    Rectangle lopta = ball.Ell;
                    double circleR = lopta.Width / 2;

                    double levo_blok = Canvas.GetLeft(bricks[i, j].Blok);
                    double gore_blok = Canvas.GetTop(bricks[i, j].Blok);

                    double levo_lopta = Canvas.GetLeft(ball.Ell);
                    double gore_lopta = Canvas.GetTop(ball.Ell);

                    double halfX = blok.Width / 2;
                    double halfY = blok.Height / 2;

                    double lopta_centarX = levo_lopta + lopta.Width / 2;
                    double lopta_centarY = gore_lopta + lopta.Height / 2;

                    double centerX = lopta_centarX - (levo_blok + halfX);
                    double centerY = lopta_centarY - (gore_blok + halfY);

                    double sideX = Math.Abs(centerX) - halfX;
                    double sideY = Math.Abs(centerY) - halfY;

                    if(sideX < 0 || sideY < 0)
                    {
                        if(Math.Abs(sideX)< circleR && sideY < 0)
                        {
                            ball.Odbij_Desno_Levo();

                            if (bricks[i, j].Pogodjen(ref score, lb))
                            {
                                cnv.Children.Remove(bricks[i, j].Blok);
                                bricks[i, j] = null;
                            }
                            bool p = false;
                            ball.Pomeraj(cnv.Width, cnv.Height, igrac, ref p);
                            return LevelUp_Myb(ball,ref rad);
                            
                        }
                        else if(Math.Abs(sideY)<circleR && sideX<0)
                        {
                            ball.Odbij_Gore_Dole();

                            if (bricks[i, j].Pogodjen(ref score, lb))
                            {
                                cnv.Children.Remove(bricks[i, j].Blok);
                                bricks[i, j] = null;
                            }
                            bool p = false;
                            ball.Pomeraj(cnv.Width, cnv.Height, igrac, ref p);
                            return LevelUp_Myb(ball,ref rad);
                        } 
                    }
                    if(sideX*sideX + sideY*sideY < circleR*circleR)
                    {
                        ball.Odbij_Gore_Dole();
                        ball.Odbij_Desno_Levo();

                        if (bricks[i, j].Pogodjen(ref score,lb))
                        {
                            cnv.Children.Remove(bricks[i, j].Blok);
                            bricks[i, j] = null;
                        }
                        bool p = false;
                        ball.Pomeraj(cnv.Width, cnv.Height, igrac, ref p);
                        return LevelUp_Myb(ball, ref rad);
                        
                    }

                    /*if (gore_blok + blok.Height >= gore_lopta && gore_blok <= gore_lopta + lopta.Height)
                    {
                        //MessageBox.Show("cao");
                        if (levo_blok + blok.Width >= levo_lopta && levo_blok <= levo_lopta + lopta.Width)
                        {
                            //if (!Cosak(ball, i, j))
                            {
                                if (levo_blok + blok.Width <= levo_lopta + lopta.Width || levo_lopta < levo_blok)
                                    ball.Odbij_Desno_Levo();
                                if (gore_lopta + lopta.Height > gore_blok || gore_lopta < gore_blok)
                                    ball.Odbij_Gore_Dole();
                            }

                            double dy = gore_blok - gore_lopta;
                            dy = Math.Abs(dy);
                            if(dy<blok.Height-3)
                            {
                                ball.Odbij_Desno_Levo();
                            }
                            else
                            {
                                if (gore_lopta + lopta.Height > gore_blok || gore_lopta < gore_blok)
                                    ball.Odbij_Gore_Dole();
                            }

                            //if (gore_lopta + lopta.Height > gore_blok || gore_lopta < gore_blok)
                                ball.Odbij_Gore_Dole();
                            else
                            if (levo_blok + blok.Width <= levo_lopta + lopta.Width || levo_lopta < levo_blok)
                                ball.Odbij_Desno_Levo();
                            


                            if (bricks[i, j].Pogodjen())
                            {
                                cnv.Children.Remove(bricks[i, j].Blok);
                                bricks[i, j] = null;
                            }
                            bool p = false;
                            ball.Pomeraj(cnv.Width,cnv.Height,igrac,ref p);
                            return;
                        }
                    }*/
                }
            }
            return false;
            
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

        private bool Cosak(Loptica ball, int i ,int j)
        {
            Rectangle blok = bricks[i, j].Blok;
            Rectangle lopta = ball.Ell;

            double levo_blok = Canvas.GetLeft(bricks[i, j].Blok);
            double gore_blok = Canvas.GetTop(bricks[i, j].Blok);

            double levo_lopta = Canvas.GetLeft(ball.Ell);
            double gore_lopta = Canvas.GetTop(ball.Ell);
            if ((levo_blok + blok.Width <= levo_lopta + lopta.Width && (gore_lopta + lopta.Height > gore_blok || gore_lopta < gore_blok)) || (levo_blok <= levo_lopta + lopta.Width && (gore_lopta + lopta.Height > gore_blok || gore_lopta < gore_blok)))
            {
                
                if(ball.Ugao >= 0)
                {
                    if (levo_blok + blok.Width <= levo_lopta + lopta.Width)
                        ball.Odbij_Gore_Dole();
                    if (levo_blok <= levo_lopta + lopta.Width)
                        ball.Ugao = 180 + ball.Ugao;
                }
                else
                {
                    if (levo_blok + blok.Width <= levo_lopta + lopta.Width)
                    {
                        ball.Ugao = 180 + ball.Ugao;
                    }
                    if (levo_blok <= levo_lopta + lopta.Width)
                        ball.Odbij_Gore_Dole();
                }
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
