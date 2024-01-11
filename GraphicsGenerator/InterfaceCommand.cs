using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    /// <summary>
    /// Interface for command classes.
    /// </summary>
    public interface InterfaceCommand
    {
        /// <summary>
        /// A method that needs to be overridden by the dervied classes.
        /// </summary>
        /// <param name="sender">object argument that contains the controller information</param>
        /// <param name="e">argument that contains the event information</param>
        /// <param name="MultilineCommand">argument that contains the textBox reference</param>
        void Execute(object sender, EventArgs e, TextBox MultilineCommand);
    }
}
