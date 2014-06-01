using System;

namespace TypeLess.Helpers.Maps.Kml
{
    public class PolygonPoint 
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public double? Alt { get; set; }

        public PolygonPoint()
        {

        }

        public PolygonPoint(double lat, double lon, double? alt = null)
        {
            Lon = lon;
            Lat = lat;
            Alt = alt;
        }

        public double DistanceTo(PolygonPoint p)
        {
            return (Math.Pow(this.Lat - p.Lat, 2) + Math.Pow(this.Lon - p.Lon, 2));
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var p2 = (PolygonPoint)obj;
            return Lon == p2.Lon && Lat == p2.Lat && Alt == p2.Alt;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Lon.GetHashCode() ^ Lat.GetHashCode() ^ Alt.GetHashCode();
        }

        
    }
}
