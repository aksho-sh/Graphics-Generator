using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    /// <summary>
    /// Command class for saving the file
    /// </summary>
    public class SaveCommand:InterfaceCommand
    {
        private Document document;

        /// <summary>
        /// Constructor for SaveCommmand class
        /// </summary>
        /// <param name="doc">The Document object needed to call document methods</param>
        public SaveCommand(Document doc)
        {
            document = doc;
        }

        /// <summary>
        /// Execute executes document method to save file
        /// </summary>
        /// <param name="sender">Object argumetn that contains controller information</param>
        /// <param name="e">Argument that contains event information.</param>
        /// <param name="MultilineCommand">Argument that contains reference to textBox.</param>
        public void Execute(object sender, EventArgs e, TextBox MultilineCommand)
        {
            document.saveFile(sender, e, MultilineCommand);
        }
    }
}
