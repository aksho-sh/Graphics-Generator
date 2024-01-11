using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    /// <summary>
    /// Shape abstract class has methods and attributes for inheritance for a concrete shape class.
    /// </summary>
    public abstract class Shape:InterfaceShapes
    {
        //point of origin
        protected int point1 = 50;
        protected int point2 = 50;
        /// <summary>
        /// the default fillStatus of a shape object
        /// </summary>
        public String fillStatus = "OFF";
        /// <summary>
        /// The default pen for the shape object
        /// </summary>
        protected Pen myPen = new Pen(Color.Black, 2);
        /// <summary>
        /// The default color of a sahpe object
        /// </summary>
        public Color m_color=Color.Black;
        /// <summary>
        /// The default flash value for a shape object
        /// </summary>
        public bool flash = false;
        /// <summary>
        /// The default primary Color for flashing object
        /// </summary>
        public Color primaryColor = Color.Transparent;
        /// <summary>
        /// The default secondary Color for flashing shapes
        /// </summary>
        public Color secondaryColor = Color.Transparent;

        /// <summary>
        /// drawShape method draws the object in a PictureBox. This must be overriden by a concrete class dervied from Shape class.
        /// </summary>
        /// <param name="g"></param>
        public abstract void drawShape(Graphics g);
        public abstract void setValues(String fillstatus,params int[] values);

        /// <summary>
        /// Shape constructor
        /// </summary>
        /// <param name="point1">first origin point</param>
        /// <param name="point2">second origin point</param>
        /// <param name="color">color</param>
        /// <param name="fillStatus">status of the fill</param>
        public Shape(int point1, int point2, string color, string fillStatus)
        {
            this.point1 = point1;
            this.point2 = point2;
            if (color.ToUpper() == "RED")
            {
                m_color = Color.Red;
            }
            else if (color.ToUpper() == "GREEN")
            {
                m_color = Color.Green;
            }
            else if (color.ToUpper() == "BLUE")
            {
                m_color = Color.Blue;
            }
            else if (color.ToUpper() == "BLACK")
            {
                m_color = Color.Black;
            }
            else if (color.ToUpper() == "REDGREEN")
            {
                this.primaryColor = Color.Red;
                this.secondaryColor = Color.Green;
                this.flash = true;
            }
            else if (color.ToUpper() == "BLUEYELLOW")
            {
                this.primaryColor = Color.Blue;
                this.secondaryColor = Color.Yellow;
                this.flash = true;
            }
            else if(color.ToUpper() == "BLACKWHITE")
            {
                this.primaryColor = Color.Black;
                this.secondaryColor = Color.White;
                this.flash = true;
            }
            this.fillStatus = fillStatus;
        }
        /// <summary>
        /// Sets the color
        /// </summary>
        /// <param name="color">The string value that represents the color</param>
        public void setColor(String color)
        {
            if (color.ToUpper()=="RED")
            {
                m_color = Color.Red;
            }else if (color.ToUpper() == "GREEN")
            {
                m_color = Color.Green;
            }else if (color.ToUpper() == "BLUE")
            {
                m_color = Color.Blue;
            }else if (color.ToUpper() == "BLACK")
            {
                m_color = Color.Black;
            }
        }
        /// <summary>
        /// Sets the origin point
        /// </summary>
        /// <param name="point1">first point of the origin</param>
        /// <param name="point2">first point of the origin</param>
        public void setPoint(int point1, int point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }
    }
}
