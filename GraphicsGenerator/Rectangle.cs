using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    /// <summary>
    /// Rectangle class provides methods and attributes for creating and drawing rectangle objects.
    /// </summary>
    public class Rectangle :Shape
    {
        //variable to store width and height
        int width;
        int height;

        /// <summary>
        /// Constructor for rectangle object
        /// </summary>
        /// <param name="point1">first point of origin</param>
        /// <param name="point2">second point of origin</param>
        /// <param name="color">color of the object</param>
        /// <param name="fillStatus">fill status of the object</param>
        /// <param name="width">width of the rectangle</param>
        /// <param name="height">height of the rectangle</param>
        public Rectangle(int point1, int point2, string color, string fillStatus, int width, int height) :base(point1, point2, color, fillStatus)
        {
            this.width = width;
            this.height = height;
        }
        //Constructor of the rectangle class
        public override void setValues(string fillstatus, params int[] values)
        {
            base.fillStatus = fillstatus;
            width = values[0];
            width = values[1];
        }

        /// <summary>
        /// This method draws the rectangle
        /// </summary>
        /// <param name="g">graphics object which provides interface to draw</param>
        public override void drawShape(Graphics g)
        {
            Pen p = new Pen(m_color,2);
            SolidBrush sb = new SolidBrush(m_color);
            g.DrawRectangle(p, point1, point2, width, height);
            if (String.Equals(fillStatus.ToUpper(), "ON"))
            {
                g.FillRectangle(sb, base.point1, base.point2, width, height); 
            }
            p.Dispose();
            sb.Dispose();
        }
    }
}
