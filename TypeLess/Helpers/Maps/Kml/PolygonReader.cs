using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace TypeLess.Helpers.Maps.Kml
{
    public class PolygonReader
    {
        public List<Polygon> ReadKml(string kml, bool onlyOuterPolygon = false)
        {
            XDocument document = XDocument.Parse(kml);

            var ns = document.Root.GetDefaultNamespace().NamespaceName;
            var placemarks = document.Root.Descendants(XName.Get("Placemark", ns)).ToList();
            List<Polygon> polygons = new List<Polygon>();


            foreach (var placemark in placemarks)
            {
                var nameElement = placemark.Element(XName.Get("name", ns));
                string name = null;
                if (nameElement != null)
                {
                    name = nameElement.Value;
                }

                Tuple<string, string>[] metaData = new Tuple<string,string>[0];

                var extendedData = placemark.Element(XName.Get("ExtendedData", ns));
                if (extendedData != null) {
                    metaData = extendedData.Descendants(XName.Get("Data", ns))
                               .Where(x => x.Attribute(XName.Get("name")) != null && x.Descendants(XName.Get("value", ns)).Any())
                               .Select(x => new Tuple<string, string>(x.Attribute(XName.Get("name")).Value, x.Descendants(XName.Get("value", ns)).First().Value)).ToArray();
                }

                var polygonElements = placemark.Descendants(XName.Get("Polygon", ns)).ToList();

                foreach (var pg in polygonElements)
                {
                    var polygon = ParsePolygon(name, pg, ns, onlyOuterPolygon);
                    if (polygon != null)
                    {
                        polygon.MetaData = metaData;
                        polygons.Add(polygon);
                    }
                }
            }
            return polygons;
        }

        private Polygon ParsePolygon(string name, XElement pg, string ns, bool onlyOuter)
        {
            PolygonReader polyReader = new PolygonReader();
            var outerBoundary = pg.Descendants(XName.Get("outerBoundaryIs", ns)).FirstOrDefault();
            var innerPolygons = pg.Descendants(XName.Get("innerBoundaryIs", ns)).ToList();

            if (outerBoundary == null)
            {
                //check if the linear ring has been added without outer/inner boundaries
                var linearRing = pg.Descendants(XName.Get("LinearRing", ns)).FirstOrDefault();
                if (linearRing != null)
                {
                    return polyReader.FromPointList(name, linearRing.Descendants(XName.Get("coordinates", ns)).First().Value);
                }

                return null;
            }

            Polygon p = polyReader.FromPointList(name, outerBoundary.Descendants(XName.Get("coordinates", ns)).First().Value);
            if (!onlyOuter) {
                foreach (var innerPolygon in innerPolygons)
                {
                    var linearRings = innerPolygon.Descendants(XName.Get("LinearRing", ns)).ToList();
                    foreach (var ring in linearRings)
                    {
                        p.InnerPolygons.Add(polyReader.FromPointList(name, ring.Descendants(XName.Get("coordinates", ns)).First().Value));
                    }

                }
            }
            
            return p;
        }

        public Polygon FromPointList(string name, string pointList)
        {
            pointList = pointList.TrimStart('\n', '\t');
            pointList = pointList.TrimEnd('\n', '\t');

            
            var points = pointList.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            
            CultureInfo ci = new CultureInfo("en-US");
            var list = new List<PolygonPoint>(points.Length);
            
            for (int i = 0; i < points.Length; i++) {

                var p = points[i].Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if (p.Length < 2 || p.Length > 3) {
                    throw new Exception("Invalid polygon // invalid point data");
                }

                PolygonPoint pp = new PolygonPoint();
                pp.Lon = double.Parse(p[0], ci);
                pp.Lat = double.Parse(p[1], ci);
                if (p.Length == 3) {
                    pp.Alt = double.Parse(p[2], ci);
                }
                
                
                list.Add(pp);
               
            }
            
            if (!list.First().Equals(list.Last())) {
                throw new Exception("Invalid polygon // First does not equal Last");
            }

            return new Polygon() { Points = list, Name = name };
        }
    }
}
