using Brick_Breaker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            res = new Point(Math.Abs(res.Y) / length, Math.Abs(res.X) / length);
            return res;
        }

        private Point[] GetVectors(Point[] vertices)
        {
            Point[] res = new Point[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                Point p = UnitVector(vertices[i], vertices[(i + 1) % vertices.Length]);
                res[i] = p;
            }
            return res;
        }

        public bool DetectCollision(Loptica loptica, Igrac igrac)
        {
            /*Canvas.SetTop(loptica.Ell, 0);
            Canvas.SetLeft(loptica.Ell, 0);

            Canvas.SetTop(igrac.Rect, 0);
            Canvas.SetLeft(igrac.Rect, 0);*/
            Point[] verticesLoptica = GetVertices(loptica);
            Point[] verticesIgrac = GetVertices(igrac.Rect);

            verticesIgrac = GetVectors(verticesIgrac);
            verticesLoptica = GetVectors(verticesLoptica);
            
            return false;
        }

    }
}
