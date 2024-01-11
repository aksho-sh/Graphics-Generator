using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    /// <summary>
    /// MenuOperation class allows the creation of invoker object to execute methods in the main class.
    /// </summary>
    public class MenuOperation
    {
        private InterfaceCommand openCommand;
        private InterfaceCommand saveCommand;

        /// <summary>
        /// MenuOperation constructor
        /// </summary>
        /// <param name="open">InterfaceCommand reference to OpenCommand object</param>
        /// <param name="save">InterfaceCommand reference to SaveCommand object</param>
        public MenuOperation(InterfaceCommand open, InterfaceCommand save)
        {
            this.openCommand = open;
            this.saveCommand = save;
        }

        /// <summary>
        /// Executes method to open a file in main method.
        /// </summary>
        /// <param name="sender">Object argumetn that contains controller information</param>
        /// <param name="e">Argument that contains event information.</param>
        /// <param name="MultilineCommand">Argument that contains reference to textBox.</param>
        public void clickOpen(object sender, EventArgs e, TextBox MultilineCommand)
        {
            openCommand.Execute(sender, e, MultilineCommand);
        }

        /// <summary>
        /// Executes method to save a file in main method.
        /// </summary>
        /// <param name="sender">Object argumetn that contains controller information</param>
        /// <param name="e">Argument that contains event information.</param>
        /// <param name="MultilineCommand">Argument that contains reference to textBox.</param>
        public void clickSave(object sender, EventArgs e, TextBox MultilineCommand)
        {
            saveCommand.Execute(sender, e, MultilineCommand);
        }
    }
}
