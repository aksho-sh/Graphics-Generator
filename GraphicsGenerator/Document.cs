using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    /// <summary>
    /// Document class has methods required to load and save the user commands from the textbox.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// openFile method gets the commands from a text file to the TextBox
        /// </summary>
        /// <param name="sender">A parameter that contains reference to the control that raised the event</param>
        /// <param name="e">Parameter that contains event data.</param>
        /// <param name="MultilineCommand">TextBox on which the command is appended to.</param>
        public void openFile(object sender, EventArgs e, TextBox MultilineCommand)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                openFile.InitialDirectory = "c:\\";
                openFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFile.FilterIndex = 1;
                openFile.RestoreDirectory = true;
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFile.FileName;

                    var fileStream = openFile.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                        MultilineCommand.Text = fileContent;
                    }
                }
            }
        }

        /// <summary>
        /// saveFile method saves commands to a file.
        /// </summary>
        /// <param name="sender">A parameter that contains reference to the control that raised the event</param>
        /// <param name="e">Parameter that contains event data</param>
        /// <param name="MultilineCommand">TextBox on which the command is appended to</param>
        public void saveFile(object sender, EventArgs e, TextBox MultilineCommand)
        {
            SaveFileDialog SaveMenu = new SaveFileDialog();

            SaveMenu.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            SaveMenu.FilterIndex = 2;
            SaveMenu.RestoreDirectory = true;
            if (SaveMenu.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(SaveMenu.FileName, MultilineCommand.Text);
            }
        }
    }
}
