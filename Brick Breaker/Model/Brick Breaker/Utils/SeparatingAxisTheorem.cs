using Brick_Breaker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Brick_Breaker.Utils
{
    class SeparatingAxisTheorem
    {
        public SeparatingAxisTheorem() { }

        ///<summary>
        /// Left Top Right Bottom   
        ///</summary>
        private Point[] GetVertices(Loptica loptica)
        {
            double d = loptica.Ell.ActualWidth * Math.Sqrt(2);
            Rectangle rect = loptica.Ell;
            double levo = Canvas.GetLeft(rect);
            double gore = Canvas.GetTop(rect);
            Point[] vertices = new Point[] { new Point(levo, gore + d / 2), new Point(levo + d/2, gore), new Point(levo + d, gore + d/2), new Point(levo + d/2, gore + d) };
            return vertices;
        }

        private Point[] GetVertices(Rectangle rect)
        {
            double width = rect.Width;
            double height = rect.Height;
            double levo = Canvas.GetLeft(rect);
            double gore = Canvas.GetTop(rect);
            Point[] res = new Point[] { new Point(levo, gore), new Point(levo + width, gore), new Point(levo + width, gore + height), new Point(levo, gore + height) };
            return res;
        }

        private Point UnitVector(Point point1, Point point2)
        {
            Point res = (Point)(point1 - point2);
            double length = ((Vector)res).Length;
            res = new Point(-(res.Y) / length, (res.X) / length);
            return res;
        }

        private Point[] GetNormals(Point[] vertices)
        {
            Point[] res = new Point[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                Point p = UnitVector(vertices[i], vertices[(i + 1) % vertices.Length]);
                res[i] = p;
            }
            return res;
        }

        private Point DotProjection(Point p1, Point p2)
        {
            double intenzitet = p1.X * p2.X + p1.Y * p2.Y;
            return new Point(p1.X * intenzitet, p1.Y * intenzitet);
        }

        private Point[] GetMinAndMaxProjection(Point axis, Loptica rect)
        {
            Point[] vertices = GetVertices(rect);
            Point min = DotProjection(axis, vertices[0]);
            Point max = min;
            for (int i = 1; i < vertices.Length; i++)
            {
                Point tmp = DotProjection(axis, vertices[i]);
                if (((Vector)tmp).Length < ((Vector)min).Length)
                    min = tmp;
                if (((Vector)tmp).Length > ((Vector)max).Length)
                    max = tmp;
            }
            return new Point[] { min, max };
        }

        private Point[] GetMinAndMaxProjection(Point axis, Rectangle rect)
        {
            Point[] vertices = GetVertices(rect);
            Point min = DotProjection(axis, vertices[0]);
            Point max = min;
            for (int i = 1; i < vertices.Length; i++)
            {
                Point tmp = DotProjection(axis, vertices[i]);
                if (((Vector)tmp).Length < ((Vector)min).Length)
                    min = tmp;
                if (((Vector)tmp).Length > ((Vector)max).Length)
                    max = tmp;
            }
            return new Point[] {min, max };
        }

        private Point GetMinAndMaxProjection(Point axis, Ellipse krug)
        {
            Point center = new Point(Canvas.GetLeft(krug) + krug.ActualWidth / 2, Canvas.GetTop(krug) + krug.ActualHeight / 2);
            Point point = DotProjection(axis, center);
            return point;
        }


        private bool Interact(Point[] first, Point[] second)
        {
            double len1 = ((Vector)first[0]).Length;
            double len2 = ((Vector)first[1]).Length;
            double len3 = ((Vector)second[0]).Length;
            double len4 = ((Vector)second[1]).Length;
            if (len2 < len3)
                return false;
            if (len4 < len1)
                return false;
            return true;
        }

        private bool Interact(Point[] first, Point second, double r)
        {
            double len1 = ((Vector)first[0]).Length;
            double len2 = ((Vector)first[1]).Length;
            double len3 = ((Vector)second).Length - r;
            double len4 = ((Vector)second).Length + r;
            if (len2 < len3)
                return false;
            if (len4 < len1)
                return false;
            return true;
        }

        public bool DetectCollision(Loptica loptica, Rectangle rect) 
        {
            //Canvas.SetTop(loptica.Ell, 0);
            //Canvas.SetLeft(loptica.Ell, 0);

            /*Canvas.SetTop(igrac.Rect, 0);
            Canvas.SetLeft(igrac.Rect, 0);*/
            Point[] verticesLoptica = GetVertices(loptica);
            Point[] verticesIgrac = GetVertices(rect);

            Point[] normalsIgrac = GetNormals(verticesIgrac);
            Point[] normalsLoptica = GetNormals(verticesLoptica);

            foreach (Point normal in normalsIgrac)
            {
                Point[] minMaxRect = GetMinAndMaxProjection(normal, rect);
                Point[] minMaxLoptica = GetMinAndMaxProjection(normal, loptica);
                if (!Interact(minMaxRect, minMaxLoptica))
                {
                    return false;
                }
            }
            foreach (Point normal in normalsLoptica)
            {
                Point[] minMaxRect = GetMinAndMaxProjection(normal, rect);
                Point[] minMaxLoptica = GetMinAndMaxProjection(normal, loptica);
                if (!Interact(minMaxRect, minMaxLoptica))
                {
                    return false;
                }
            }

            return true;
        }

        public bool DetectCollision(Igrac igrac, MyDrop drop)
        {
            Point[] verticesIgrac = GetVertices(igrac.Rect);
            Point[] normalsIgrac = GetNormals(verticesIgrac);

            Point normalDrop = GetClosest(drop.Krug, verticesIgrac);

            foreach (Point normal in normalsIgrac)
            {
                Point[] minMaxRect = GetMinAndMaxProjection(normal, igrac.Rect);
                Point centerDrop = GetMinAndMaxProjection(normal, drop.Krug);
                if (!Interact(minMaxRect, centerDrop, drop.Krug.ActualWidth / 2))
                {
                    return false;
                }
            }
            return true;
        }

        private Point GetClosest(Ellipse krug, Point[] verticesIgrac)
        {
            Point min = verticesIgrac[0];
            foreach (Point point in verticesIgrac)
            {
                Point tmp = new Point(point.X - min.X, point.Y - min.Y);
                if (((Vector)min).Length > ((Vector)tmp).Length)
                    min = tmp;
            }
            double len = ((Vector)min).Length;
            return new Point(-min.Y / len, min.X / len);
        }
    }
}
