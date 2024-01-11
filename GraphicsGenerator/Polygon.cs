using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    /// <summary>
    /// Polygon class provides methods and attributes for creating and drawing polygon objects.
    /// </summary>
    public class Polygon : Shape 
    {
        List<Point> polygonPoints = new List<Point>();

        /// <summary>
        /// Constructor to create polygon objects
        /// </summary>
        /// <param name="point1">First point of origin.</param>
        /// <param name="point2">Second point of origin.</param>
        /// <param name="color">Color of the polygon object.</param>
        /// <param name="fillstatus">Fill status of the polygon.</param>
        /// <param name="values">An integer array with point values for polygon.</param>
        public Polygon(int point1, int point2, string color,string fillstatus, int[]values):base(point1, point2, color, fillstatus)
        {
            polygonPoints.Add(new Point(this.point1, this.point2));
            for (int i = 0;  i < values.Length; i += 2)
            {
                polygonPoints.Add(new Point(values[i], values[i + 1]));
            }
        }

        public override void setValues(string fillstatus, int[] values)
        {
            base.fillStatus = fillstatus;
            polygonPoints.Add(new Point(this.point1, this.point2));
            for (int i = 0; i < values.Length; i += 2)
            {
                polygonPoints.Add(new Point(values[i], values[i + 1]));
            }
        }

        /// <summary>
        /// drawShape draws the polygon object
        /// </summary>
        /// <param name="g">Grpahics object which provides interface to draw</param>
        public override void drawShape(Graphics g)
        {
            Pen p = new Pen(m_color, 2);
            SolidBrush sb = new SolidBrush(m_color);
            g.DrawPolygon(p, polygonPoints.ToArray());
            if (String.Equals(fillStatus.ToUpper(), "ON"))
            {
                g.FillPolygon(sb, polygonPoints.ToArray());
            }
            p.Dispose();
            sb.Dispose();
        }
    }
}
