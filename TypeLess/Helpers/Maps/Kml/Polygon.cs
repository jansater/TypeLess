using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeLess.Helpers.Maps.Kml
{
    public class Polygon
    {
        public string Name { get; set; }
        public Tuple<string, string>[] MetaData { get; set; }
        public List<PolygonPoint> Points { get; set; }
        public List<Polygon> InnerPolygons { get; set; }

        public Polygon()
        {
            Points = new List<PolygonPoint>();
            InnerPolygons = new List<Polygon>(30);
            MetaData = new Tuple<string, string>[0];
        }

        public bool IsEmpty { get { return Points.Count <= 0; } }

        public PolygonPoint ApproxCenter
        {
            get
            {
                int total = Points.Count;

                double X = 0;
                double Y = 0;
                double Z = 0;

                foreach (var i in Points)
                {
                    double lat = i.Lat * Math.PI / 180;
                    double lon = i.Lon * Math.PI / 180;

                    double x = Math.Cos(lat) * Math.Cos(lon);
                    double y = Math.Cos(lat) * Math.Sin(lon);
                    double z = Math.Sin(lat);

                    X += x;
                    Y += y;
                    Z += z;
                }

                X = X / total;
                Y = Y / total;
                Z = Z / total;

                double lonCoord = Math.Atan2(Y, X);
                double hyp = Math.Sqrt(X * X + Y * Y);
                double latCoord = Math.Atan2(Z, hyp);

                return new PolygonPoint()
                {
                    Lat = latCoord * 180 / Math.PI,
                    Lon = lonCoord * 180 / Math.PI
                };
            }
        }

        public PlacemarkStyle Style { get; set; }

        public bool ContainsPosition(PolygonPoint p)
        {
            int n = this.Points.Count();
            if (n <= 0)
            {
                return false;
            }
            List<PolygonPoint> v = Points.ToList();
            v.Add(new PolygonPoint { Lat = v[0].Lat, Lon = v[0].Lon });

            int wn = 0;    // the winding number counter

            // loop through all edges of the polygon
            for (int i = 0; i < n; i++)
            {   // edge from V[i] to V[i+1]
                if (v[i].Lat <= p.Lat)
                {         // start y <= P.y
                    if (v[i + 1].Lat > p.Lat)      // an upward crossing
                        if (isLeft(v[i], v[i + 1], p) > 0)  // P left of edge
                            ++wn;            // have a valid up intersect
                }
                else
                {                       // start y > P.y (no test needed)
                    if (v[i + 1].Lat <= p.Lat)     // a downward crossing
                        if (isLeft(v[i], v[i + 1], p) < 0)  // P right of edge
                            --wn;            // have a valid down intersect
                }
            }

            return wn != 0;
        }

        private static int isLeft(PolygonPoint P0, PolygonPoint P1, PolygonPoint P2)
        {
            double calc = ((P1.Lon - P0.Lon) * (P2.Lat - P0.Lat)
                    - (P2.Lon - P0.Lon) * (P1.Lat - P0.Lat));
            if (calc > 0)
                return 1;
            else if (calc < 0)
                return -1;
            else
                return 0;
        }
    }


}
