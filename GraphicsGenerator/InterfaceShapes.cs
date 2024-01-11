using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    /// <summary>
    /// Interface for Shape abstract class with method signatures for method that must be defined.
    /// </summary>
    public interface InterfaceShapes
    {
        /// <summary>
        /// sets the values of a shape
        /// </summary>
        /// <param name="fillstatus">status of fill; on/off</param>
        /// <param name="parameters">parameters of shape</param>
        void setValues(String fillstatus, int[] parameters);

        /// <summary>
        /// set origin point
        /// </summary>
        /// <param name="point1">first point of origin</param>
        /// <param name="point2">second point of origin</param>
        void setPoint(int point1, int point2);

        /// <summary>
        /// method to draw the object
        /// </summary>
        /// <param name="g">the Graphics argument to draw object on</param>
        void drawShape(Graphics g);

        /// <summary>
        /// set the color of the object
        /// </summary>
        /// <param name="Color">String value representing color</param>
        void setColor(string Color);
    }
}
