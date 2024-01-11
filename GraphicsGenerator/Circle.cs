using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    /// <summary>
    /// Circle class allows creation of circle objects with specified properties and has a method to draw the circle.
    /// </summary>
    public class Circle:Shape
    {
        //radius of the circle
        int radius;

        /// <summary>
        /// Constructor of the circle class
        /// </summary>
        /// <param name="point1">first point of the origin</param>
        /// <param name="point2">second point of the origin</param>
        /// <param name="color">color of the circle object</param>
        /// <param name="fillStatus">status of fill</param>
        /// <param name="radius">radius of the circle</param>
        public Circle(int point1, int point2, string color, string fillStatus, int radius):base(point1, point2, color, fillStatus)
        {
            this.radius = radius;
        }
        //method to set circle values
        public override void setValues(String fillstatus, params int[] values)
        {
            base.fillStatus = fillstatus;
            radius = values[0];
        }

        /// <summary>
        /// Method to draw the circle object
        /// </summary>
        /// <param name="g">the grapic object which provides an interface to draw</param>
        public override void drawShape(Graphics g)
        {
            SolidBrush sb = new SolidBrush(base.m_color);
            Pen p = new Pen(base.m_color, 2);

            g.DrawEllipse(p, base.point1, base.point2, radius * 2, radius * 2);
            if (String.Equals(fillStatus.ToUpper(), "ON"))
            {
                g.FillEllipse(sb, base.point1, base.point2, radius * 2, radius * 2);
            }
            else 
            {
                g.DrawEllipse(p, base.point1, base.point2, radius * 2, radius * 2);
            }            

            p.Dispose();
            sb.Dispose();
        }
    }
}
