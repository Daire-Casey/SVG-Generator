using System;
using System.Collections.Generic;
using System.IO;

namespace a4
{
    public class Shapes
    {
        // the following methods allow the updating of shapes on the canvas
        public static void UpdateCircle(Circle shp, int x, int y, int r, string s, string sw, string f)
        {
            shp.X = x;
            shp.Y = y;
            shp.R = r;
            shp.S = s;
            shp.SW = sw;
            shp.F = f;

            Console.WriteLine("Shape was updated.");
        }
        public static void UpdateRectangle(Rectangle shp, int x, int y, int w, int l, string s, string sw, string f)
        {
            shp.X = x;
            shp.Y = y;
            shp.W = w;
            shp.L = l;
            shp.S = s;
            shp.SW = sw;
            shp.F = f;

            Console.WriteLine("Shape was updated.");
        }
        public static void UpdateEllipse(Ellipse shp, int x, int y, int r, int ry, string s, string sw, string f)
        {
            shp.X = x;
            shp.Y = y;
            shp.R = r;
            shp.RY = ry;
            shp.S = s;
            shp.SW = sw;
            shp.F = f;

            Console.WriteLine("Shape was updated.");
        }
        public static void UpdateLine(Line shp, int x, int y, int x2, int y2, string s, string sw, string f)
        {
            shp.X = x;
            shp.Y = y;
            shp.X2 = x2;
            shp.Y2 = y2;
            shp.S = s;
            shp.SW = sw;
            shp.F = f;

            Console.WriteLine("Shape was updated.");
        }
        public static void UpdatePolyline(Polyline shp, int x, int y, List<int> p, string s, string sw, string f)
        {
            shp.X = x;
            shp.Y = y;
            shp.pLine = p;
            shp.S = s;
            shp.SW = sw;
            shp.F = f;

            Console.WriteLine("Shape was updated.");
        }
        public static void UpdatePolygon(Polygon shp, int x, int y, List<int> p, string s, string sw, string f)
        {
            shp.X = x;
            shp.Y = y;
            shp.pLine = p;
            shp.S = s;
            shp.SW = sw;
            shp.F = f;

            Console.WriteLine("Shape was updated.");
        }
    }

    public class Shape // shape superclass
    {
        public static Random rand = new Random(); // used for randomising the positioning, size, and colour
        public int X { get; set; }
        public int Y { get; set; }
        public string S { get; set; } // stroke
        public string SW { get; set; } // stroke width
        public string F { get; set; } // fill

        public Shape()
        {
            X = rand.Next(1000);
            Y = rand.Next(500);
            S = "black"; // default styles
            SW = "3";
            F = "rgb(" + rand.Next(256) + "," + rand.Next(256) + "," + rand.Next(256) + ")";
        }

        public Shape(int x, int y, string s, string sw, string f)
        {
            X = x;
            Y = y;
            S = s;
            SW = sw;
            F = f;
        }
    }

    public class Circle : Shape // inherits shape class
        {
            public int R { get; set; }

            public Circle()
            {
                R = rand.Next(5,150);
            }

            public Circle(int r)
            {
                R = r;
            }

            public Circle(int x, int y, int r, string s, string sw, string f) : base(x, y, s, sw, f)
            {
                R = r;
            }

            public override string ToString()
            {
                return String.Format(@"<circle cx=""{0}"" cy=""{1}"" r=""{2}"" stroke=""{3}"" stroke-width=""{4}"" fill=""{5}"" />", X, Y, R, S, SW, F);
            }
        }

    public class Rectangle : Shape
        {
            public int W { get; set; }
            public int L { get; set; }

            public Rectangle()
            {
                W = rand.Next(15,300);
                L = rand.Next(15,300);
            }

            public Rectangle(int w, int l)
            {
                W = w;
                L = l;
            }

            public Rectangle(int x, int y, int w, int l, string s, string sw, string f) : base(x, y, s, sw, f)
            {
                W = w;
                L = l;
            }

            public override string ToString()
            {
                return String.Format(@"<rect x=""{0}"" y=""{1}"" width=""{2}"" height=""{3}"" stroke=""{4}"" stroke-width=""{5}"" fill=""{6}"" />", X, Y, W, L, S, SW, F);
            }
        }

        public class Ellipse : Circle
        {
            public int RY { get; set; }

            public Ellipse()
            {
                RY = rand.Next(5,150);
            }

            public Ellipse(int ry)
            {
                RY = ry;
            }

            public Ellipse(int x, int y, int r, int ry, string s, string sw, string f) : base(x, y, r, s, sw, f)
            {
                RY = ry;
            }

            public override string ToString()
            {
                return String.Format(@"<ellipse cx=""{0}"" cy=""{1}"" rx=""{2}"" ry=""{3}"" stroke=""{4}"" stroke-width=""{5}"" fill=""{6}"" />", X, Y, R, RY, S, SW, F);
            }
        }

        public class Line : Shape
        {
            public int X2 { get; set; }
            public int Y2 { get; set; }

            public Line()
            {
                X2 = rand.Next(1000);
                Y2 = rand.Next(500);
            }

            public Line(int x2, int y2)
            {
                X2 = x2;
                Y2 = y2;
            }

            public Line(int x, int y, int x2, int y2, string s, string sw, string f) : base(x, y, s, sw, f)
            {
                X2 = x2;
                Y2 = y2;
            }

            public override string ToString()
            {
                return String.Format(@"<line x1=""{0}"" y1=""{1}"" x2=""{2}"" y2=""{3}"" stroke=""{4}"" stroke-width=""{5}"" fill=""none"" />", X, Y, X2, Y2, S, SW);
            }
        }

        public class Polyline : Shape
        {
            public List<int> pLine = new List<int>();

            public Polyline()
            {
                pLine.Add(X);
                pLine.Add(Y);

                int j = (rand.Next(2,6) * 2);
                for(int i = 0; i < j; i++) {
                    pLine.Add(rand.Next(500));
                }
            }

            public Polyline(int x, int y, List<int> p, string s, string sw, string f) : base(x, y, s, sw, f)
            {
                pLine.Add(x);
                pLine.Add(y);

                foreach(int n in p)
                {
                    pLine.Add(n);
                }
            }

            public override string ToString()
            {
                string points = new string("");
                for(int i = 0; i < pLine.Count; i += 2)
                {
                    points += pLine[i] + "," + pLine[i+1] + " ";
                }
                return String.Format(@"<polyline points=""{0}"" stroke=""{1}"" stroke-width=""{2}"" fill=""none"" />", points, S, SW);
            }
        }

        public class Polygon : Shape
        {
            public List<int> pLine = new List<int>();

            public Polygon()
            {
                pLine.Add(X);
                pLine.Add(Y);

                int j = (rand.Next(2,6) * 2);
                for(int i = 0; i < j; i++) {
                    pLine.Add(rand.Next(500));
                }
            }

            public Polygon(int x, int y, List<int> p, string s, string sw, string f) : base(x, y, s, sw, f)
            {
                pLine.Add(x);
                pLine.Add(y);

                foreach(int n in p)
                {
                    pLine.Add(n);
                }
            }

            public override string ToString()
            {
                string points = new string("");
                for(int i = 0; i < pLine.Count; i += 2)
                {
                    points += pLine[i] + "," + pLine[i+1] + " ";
                }
                return String.Format(@"<polygon points=""{0}"" stroke=""{1}"" stroke-width=""{2}"" fill=""{3}"" />", points, S, SW, F);
            }
        }
}