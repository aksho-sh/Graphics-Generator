using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    /// <summary>
    /// Triangle class provides methods and attributes for creating and drawing triangle objects.
    /// </summary>
    public class Triangle : Shape
    {
        //Vertices of triangles
        Point p1;
        Point p2;
        Point p3;
        
        /// <summary>
        /// Constructor of the triangle class
        /// </summary>
        /// <param name="point1">first point of the origin</param>
        /// <param name="point2">second point of the origin</param>
        /// <param name="color">color of the object</param>
        /// <param name="fillStatus">status of fill</param>
        /// <param name="values">values for the vertices</param>
        public Triangle(int point1, int point2, string color, string fillStatus, params int[] values):base(point1, point2, color, fillStatus)
        {
            p1 = new Point(values[0], values[1]);
            p2 = new Point(values[2], values[3]);
            p3 = new Point(point1, point2);
        }
        //Method to set values of vertices
        public override void setValues(string fillstatus, params int[] values)
        {
            base.fillStatus = fillstatus;
            p1 = new Point(values[0], values[1]);
            p2 = new Point(values[2], values[3]);
            p3 = new Point(point1, point2);
        }
        
        /// <summary>
        /// Draws the triangle object
        /// </summary>
        /// <param name="g">graphics object which provides the interface to draw</param>
        public override void drawShape(Graphics g)
        {
            Pen p = new Pen(m_color, 2);
            SolidBrush sb = new SolidBrush(m_color);
            Point[] trianglePoints = { p1, p2, p3 };
            g.DrawPolygon(p, trianglePoints);
            if (String.Equals(fillStatus.ToUpper(), "ON"))
            {
                g.FillPolygon(sb, trianglePoints);
            }
            p.Dispose();
            sb.Dispose();
        }
    }
}
