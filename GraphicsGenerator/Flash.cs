using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment1
{
    /// <summary>
    /// Flash class has attributes and methods required to carry out threaded operations required to make flashing shapes.
    /// </summary>
    public class Flash
    {
        Form1 form;
        ArrayList shapes;
        bool flashFlag = false;
        public static Thread thread;
        /// <summary>
        /// constructor for Flash class.
        /// </summary>
        /// <param name="form">The form on which the threaded operation is to be done.</param>
        /// <param name="shapes">The arraylist of shapes which could be flashed.</param>
        public Flash(Form1 form, ArrayList shapes)
        {
            this.form = form;
            this.shapes = shapes;
            thread = new Thread(flashshapes);
            thread.Start();
        }

        /// <summary>
        /// flashShapes method flashes the shapes based on flags assigned over 500 ms interval
        /// </summary>
        public void flashshapes()
        {
            while (true)
            {
                foreach (Shape shape in shapes)
                {
                    if (flashFlag == false)
                    {
                        if (shape.flash)
                        {
                            shape.fillStatus = "ON";
                            shape.m_color = shape.primaryColor;
                        }
                    }
                    else
                    {
                        if (shape.flash)
                        {
                            shape.fillStatus = "ON";
                            shape.m_color = shape.secondaryColor;
                        }
                    }
                }

                flashFlag = !flashFlag; //alternate colors
                form.refreshPictureBox();
                Thread.Sleep(500);
            }
        }
    }
}
