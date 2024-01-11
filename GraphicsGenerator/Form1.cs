using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    /// <summary>
    /// Form1 class has attributes and methods needed to run the GUI application form.
    /// </summary>
    public partial class Form1 : Form
    {
        //Use of command objects to enfore command design pattern
        Document doc = new Document();
        InterfaceCommand openCommand;
        InterfaceCommand saveCommand;
        MenuOperation operation;

        //Use of command elements to parse the commands
        CommandParser cs = new CommandParser();
        Graphics g;
        ArrayList shapes = new ArrayList();
        bool errFlag = false;

        //Attributes for threading
        public static Thread colorflash;    
        Flash flash;

        /// <summary>
        /// A constructor for Form1 class
        /// </summary>
        public Form1()
        {
            openCommand = new OpenCommand(doc);
            saveCommand = new SaveCommand(doc);
            operation = new MenuOperation(openCommand, saveCommand);
            PictureBox.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            flash = new Flash(this, cs.shapelist);
        }

        //event handler for execute button
        private void button1_Click(object sender, EventArgs e)
        {
            //Text taken from SingleLineCommand textbox
            String singleCommand = SingleLineCommand.Text;
            String multiCommands;
            //run in singleline
            try
            {
                if (String.Equals(singleCommand.ToUpper(), "RUN") && errFlag is false)
                {
                    //Text taken from MutilineCommand textbox
                    multiCommands = MultilineCommand.Text;
                    shapes = cs.parseSequence(multiCommands);
                }
                else if(String.Equals(singleCommand.ToUpper(),"RUN") && errFlag is true)
                {
                    MessageBox.Show("Please debug your errors", "Error found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //clear in singleine
                else if (String.Equals(singleCommand.ToUpper(), "CLEAR"))
                {
                    cs.resetShapes();
                }
                //reset in singleline
                else if (String.Equals(singleCommand.ToUpper(), "RESET"))
                {
                    cs.resetPoints();
                }
                else if (String.IsNullOrEmpty(singleCommand) || String.IsNullOrWhiteSpace(singleCommand))
                {
                    richTextBox1.Text = "Please enter a single line command.";
                }
                else
                {
                    shapes = cs.parseSequence(singleCommand);
                }
            }
            catch (InvalidSyntaxException err)
            {
                richTextBox1.Text = err.Message;
                return;
            }
            catch (DivideByZeroException err)
            {
                richTextBox1.Text = "The given expression isn't a valid integer expression.";
            }
            catch (FormatException err)
            {
                richTextBox1.Text = "The given expression isn't a valid integer expression.";
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        /// <summary>
        /// Refreshes the displayarea
        /// </summary>
        public void refreshPictureBox()
        {
            DisplayArea.Refresh();
        }

        //event handler for load menu item
        private void loadCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            operation.clickOpen(sender, e, MultilineCommand);
        }

        //event handler for save menu item
        private void saveCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            operation.clickSave(sender, e, MultilineCommand);
        }

        //event handler for help menu item
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String messageBoxContent = "Interface:" + Environment.NewLine + Environment.NewLine + "Program Code: Type codes involving multiple commands here." + Environment.NewLine + "Draw Area: Objects drawn from commands are shown here" + Environment.NewLine +
                "Single Line command: Exclusively runs commands: run, reset, clear as well as other single commands" + Environment.NewLine +
                "Code Feedback: Provides code feedback" + Environment.NewLine + Environment.NewLine + "Commands:" + Environment.NewLine + Environment.NewLine + "fill: uses on and off parameters to specify fill status" + Environment.NewLine +
                "pen: uses red,green,blue and black parameters to specify color for object" + Environment.NewLine + "moveto: moves origin to the specified point" + Environment.NewLine + "drawto: redraws the last shape to a new specified point" + Environment.NewLine + Environment.NewLine +
                "Shape Commands:" + Environment.NewLine + Environment.NewLine + "Circle: takes radius as a parameter" + Environment.NewLine + "Rectangle: takes width and height as parameters" + Environment.NewLine + "Triangle: Takes 4 points for each vertices" + Environment.NewLine + Environment.NewLine +
                "Single Line Commands:" + Environment.NewLine + Environment.NewLine + "Run: runs the code in the code area" + Environment.NewLine + "Clear: clears the drawing area" + Environment.NewLine + "Reset: resets the position of the origin";

            MessageBox.Show(messageBoxContent, "Operation Guide");
        }

        //Syntax debugging when a text value is changed in textBox
        private void MultilineCommand_TextChanged(object sender, EventArgs e)
        {
            CommandParser testParser = new CommandParser();
            String multiLine = MultilineCommand.Text;
            richTextBox1.Text = "";
            errFlag = false;
            try
            {
                testParser.parseSequence(multiLine);
            }
            catch (FormatException err)
            {
                richTextBox1.Text = "The given expression isn't a valid integer expression.";
                errFlag = true;
            }
            catch (IndexOutOfRangeException err)
            {
                richTextBox1.Text = "The expression is incomplete or invalid.";
                errFlag = true;
            }
            catch (InvalidSyntaxException err)
            {
                richTextBox1.Text = err.Message;
                errFlag = true;
            }
            testParser = null;
        }

        //Drawing shapes in the arraylist
        private void DisplayArea_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            foreach(Shape shape in shapes)
            {
                Shape s = (Shape)shape;

                if (s != null)
                {
                    s.drawShape(g);
                }
            }
        }

    }
}
