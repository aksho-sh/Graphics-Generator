using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{

    /// <summary>
    /// ShapeFactory class has method to provide the required shape objects.
    /// </summary>
    public class ShapeFactory
    {
        /// <summary>
        /// Returns object based on the passed parameters. 
        /// </summary>
        /// <param name="shape">String representing the shape.</param>
        /// <param name="point1">First point of origin for the shape.</param>
        /// <param name="point2">Second point of origin for the shape.</param>
        /// <param name="color">Color of the shape.</param>
        /// <param name="fillStatus">Status of the fill for the object. ON/OFF</param>
        /// <param name="values">Any other additional values for the object like radius, points, etc.</param>
        /// <returns></returns>
        public static Shape ReturnShape(String shape, int point1, int point2, string color, string fillStatus, params int[] values)
        {
            shape = shape.ToUpper();
            if (shape.Equals("CIRCLE"))
            {
                return new Circle(point1,point2,color,fillStatus, values[0]);
            }
            else if (shape.Equals("RECTANGLE"))
            {
                return new Rectangle(point1, point2, color, fillStatus, values[0],values[1]);
            }
            else if (shape.Equals("TRIANGLE"))
            {
                return new Triangle(point1, point2, color, fillStatus, values[0], values[1], values[2], values[3]);
            }
            else if (shape.Equals("POLYGON"))
            {
                return new Polygon(point1, point2, color, fillStatus, values);
            }
            else
            {
                System.ArgumentException FactEx = new System.ArgumentException("Factory error, " + shape + " shapetype doesn't exist");
                throw FactEx;
            }
        }
    }
}
