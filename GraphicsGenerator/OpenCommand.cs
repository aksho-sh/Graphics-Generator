using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    /// <summary>
    /// Command class for opening a saved file.
    /// </summary>
    public class OpenCommand:InterfaceCommand
    {
        private Document document;

        /// <summary>
        /// Constructor for SaveCommmand class
        /// </summary>
        /// <param name="doc">The Document object needed to call document methods</param>
        public OpenCommand(Document doc)
        {
            document = doc;
        }
        /// <summary>
        /// Execute executes document method to load file
        /// </summary>
        /// <param name="sender">Object argumetn that contains controller information</param>
        /// <param name="e">Argument that contains event information.</param>
        /// <param name="MultilineCommand">Argument that contains reference to textBox.</param>
        public void Execute(object sender, EventArgs e, TextBox MultilineCommand)
        {
            document.openFile(sender, e, MultilineCommand);
        }
    }
}
