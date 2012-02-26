using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Media;

namespace SlidingBlockPlatformer
{
    public class Polygon 
    {
        public Line[] Lines;

        public Polygon(params Line[] lines)
        {
            Lines = lines;   
        }
        public Polygon(params Vector2[] vertices)
        {
            List<Line> lns = new List<Line>();
            for (int i = 0; i < vertices.Length; i++)
            {
                lns.Add(new Line(vertices[i], vertices[(i + 1) % vertices.Length]));
            }
            
            Lines = lns.ToArray<Line>();
        }

        /// <summary>
        /// Point in polygon check via ray test
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="polygon">The polygon.</param>
        /// <returns>True if point is inside, false otherwise.</returns>
        public bool PointInPolygon(Point point, Polygon polygon) {
            bool inside = false;

            foreach (var side in polygon.Lines)
            {
                if (point.Y > Math.Min(side.Start.Y, side.End.Y))
                    if (point.Y <= Math.Max(side.Start.Y, side.End.Y))
                        if (point.X <= Math.Max(side.Start.X, side.End.X))
                        {
                            float xIntersection = side.Start.X + ((point.Y - side.Start.Y) / (side.End.Y - side.Start.Y)) * (side.End.X - side.Start.X);
                            if (point.X <= xIntersection)
                                inside = !inside;
                        }
            }

            return inside;
        }

        /// <summary>
        /// Checks if two polygons intersect
        /// </summary>
        /// <param name="other">The polygon being compared to this one</param>
        /// <returns>True if the two polygons intersect</returns>
        public bool Intersects(Polygon other)
        {
            foreach (var side in other.Lines)
            {
                if (PointInPolygon(side.Start, this))
                    return true;
            }
            foreach (var side in this.Lines)
            {
                if (PointInPolygon(side.Start, other))
                    return true;
            }

            return false;
        }
    }

    public class Line   
    {
        public Point Start;
        public Point End;

        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }
        public Line(Vector2 start, Vector2 end)
        {
            Start = new Point((int)start.X, (int)start.Y);
            End = new Point((int)end.X, (int)end.Y);
        }
    }

}
