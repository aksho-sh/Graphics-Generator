using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    /// <summary>
    /// CommandParser class has methods required for parsing commands from string entered by a user and executing them.
    /// </summary>
    public class CommandParser
    {
        //Array for the commands in the program
        String[] commands = new String[] { "CIRCLE", "RECTANGLE", "TRIANGLE", "MOVETO", "DRAWTO", "PEN", "FILL", "POLYGON" };

        //Array for modifiers associated with fill and color command
        String[] colorModifiers = new string[] { "GREEN", "RED", "BLUE", "BLACK", "REDGREEN", "BLUEYELLOW", "BLACKWHITE" };

        //Array for fill modifiers
        String[] fillModifiers = new string[] { "ON", "OFF" };

        //Array for sequence commands
        String[] sequenceCommands = new string[] { "IF", "WHILE", "METHOD" };

        //Array for sequence terminators
        String[] sequenceTerminators = new string[] { "ENDIF", "ENDLOOP", "ENDMETHOD" };

        /// <summary>
        /// shapelist stores the array of Shapes drawn by the User.
        /// </summary>
        public ArrayList shapelist = new ArrayList();

        //Dictionary to store new variable name and their values
        Dictionary<string, int> variables = new Dictionary<string, int>();

        //Dictionary to store method name and method body
        Dictionary<string, string> methods = new Dictionary<string, string>();
        NameValueCollection indivCommands = new NameValueCollection();

        //default color values
        String m_color = "BLACK";
        /// <summary>
        /// The current fillStatus
        /// </summary>
        public String fillStatus = "OFF";

        //Origin points
        int position1 = 0;
        int position2 = 0;

        //integer for testing purpose
        int intTester;

        /// <summary>
        /// parseSequece method obtains multiline string from the textBox and parses from multiline the sequences IF, WHILE, METHOD declaration and
        /// non sequential part of the code.
        /// </summary>
        /// <param name="multiline">The string obtained from </param>
        /// <returns></returns>
        public ArrayList parseSequence(String multiline)
        {
            if (String.IsNullOrEmpty(multiline) || String.IsNullOrWhiteSpace(multiline))
            {
                String errorMsg = "Can't pass empty value";
                throw new InvalidSyntaxException(errorMsg);
            }
            var lines = multiline.Split(new String[] { Environment.NewLine, " "}, StringSplitOptions.RemoveEmptyEntries);
            String seqCommands = "";
            String commands = "";
            int commandFlag = 0;
            int ifFlag = 0;
            int loopFlag = 0;
            int methodFlag = 0;
            for (int i = 0; i<lines.Length; i++)
            {
                //find the type of sequence
                if (lines[i].ToUpper() == "IF" && ifFlag == 0 && loopFlag == 0 && methodFlag ==0)
                {
                    if(commandFlag == 0 && !String.IsNullOrEmpty(commands) && !String.IsNullOrWhiteSpace(commands))
                    {
                        parseCommands(commands);
                        commands = "";
                    }
                    commandFlag = 1;
                    ifFlag++;
                }
                else if(lines[i].ToUpper()=="IF" && ifFlag == 1 && loopFlag==0 && methodFlag==0)
                {
                    commandFlag = 1;
                    parseIf(seqCommands);
                    seqCommands = "";
                }

                if (lines[i].ToUpper() == "WHILE" && ifFlag == 0 && loopFlag == 0 && methodFlag == 0)
                {
                    if(commandFlag == 0 && !String.IsNullOrEmpty(commands) && !String.IsNullOrWhiteSpace(commands))
                    {
                        parseCommands(commands);
                        commands = "";
                    }
                    commandFlag = 1;
                    loopFlag++;
                }
                else if (lines[i].ToUpper() == "WHILE" && ifFlag == 1 && loopFlag == 0 && methodFlag == 0)
                {
                    ifFlag--;
                    commandFlag = 1;
                    parseIf(seqCommands);
                    seqCommands = "";
                }

                if (lines[i].ToUpper() == "METHOD" && ifFlag == 0 && loopFlag == 0 && methodFlag == 0)
                {
                    if (commandFlag == 0 && !String.IsNullOrEmpty(commands) && !String.IsNullOrWhiteSpace(commands))
                    {
                        parseCommands(commands);
                        commands = "";
                    }
                    commandFlag = 1;
                    methodFlag++;
                }
                else if (lines[i].ToUpper() == "METHOD" && ifFlag == 1 && loopFlag == 0 && methodFlag == 0)
                {
                    ifFlag--;
                    commandFlag = 1;
                    parseIf(seqCommands);
                    seqCommands = "";
                }

                //add commmands not in a sequence
                if (commandFlag == 1)
                {
                    seqCommands= seqCommands + " " + lines[i];
                }
                else
                {
                    commands = commands + " " + lines[i];
                }

                //find end of if sequence
                if (lines[i].ToUpper() == "ENDIF" && ifFlag==1) 
                {
                    commandFlag = 0;
                    ifFlag--;
                    parseIf(seqCommands);
                    seqCommands = "";
                }
                else if(lines[i].ToUpper() == "ENDIF" && ifFlag <=0)
                {
                    String errorMsg = "Missing IF keyword.";
                    throw new InvalidSyntaxException(errorMsg);
                }
                //find end of loop sequence
                if (lines[i].ToUpper() == "ENDLOOP" && loopFlag==1)
                {
                    commandFlag = 0;
                    loopFlag--;
                    parseLoop(seqCommands);
                    seqCommands = "";
                }
                else if(lines[i].ToUpper() == "ENDLOOP" && loopFlag <=0)
                {
                    String errorMsg = "Missing WHILE keyword.";
                    throw new InvalidSyntaxException(errorMsg);
                }
                //find end of method sequence
                if (lines[i].ToUpper() == "ENDMETHOD" && methodFlag==1)
                {
                    commandFlag = 0;
                    methodFlag--;
                    parseMethod(seqCommands);
                    seqCommands = "";
                }
                else if(lines[i].ToUpper() == "ENDMETHOD" && methodFlag <=0)
                {
                    String errorMsg = "Missing METHOD keyword.";
                    throw new InvalidSyntaxException(errorMsg);
                }

                //If the last line is reached
                if(i == lines.Length -1)
                {
                    if (!String.IsNullOrEmpty(commands) || !String.IsNullOrWhiteSpace(commands))
                    {
                        parseCommands(commands);
                        commands = "";
                    }
                    if (ifFlag != 0)
                    {
                        parseIf(seqCommands);
                    }
                    if(loopFlag != 0)
                    {
                        String errorMsg = "Loop not ended";
                        throw new InvalidSyntaxException(errorMsg);
                    }
                    if(methodFlag != 0)
                    {
                        String errorMsg = "Method declaration not ended.";
                        throw new InvalidSyntaxException(errorMsg);
                    }
                }
            }
            return shapelist;
        }

        /// <summary>
        /// parseIf method obtains string belonging to IF sequence from parseSequence. It checks the condition of the if statement and forwards
        /// all the statement inside the if sequence to parseCommands method
        /// </summary>
        /// <param name="commands">The string representing IF sequence obtained from parseSequence</param>
        public void parseIf(String commands) 
        {
            String ifCommand = commands.Trim();
            var command = ifCommand.Split(new String[] { Environment.NewLine, ",", " " }, StringSplitOptions.RemoveEmptyEntries);

            //Regex that matches a valid IF condition
            Regex condition = new Regex(@"^[A-Za-z0-9]+(([<>]{1}[=]?)|([=]{1})|([!][=])){1}[A-Za-z0-9]+$");
            int validFlag = 0;
            DataTable dt = new DataTable();
            String expression = "";
            bool expressionTrue = false;
            String returnValue = "";

            //If the expression is valid to the above regex executes
            if (condition.IsMatch(command[1]))
            {
                validFlag = 1;
            }
            else
            {
                String errMsg = "Invalid IF comparison.";
                throw new InvalidSyntaxException(errMsg);
            }

            //If the expression has execution statement executes
            if (command.Length > 3)
            {
                validFlag = 1;
            }
            else
            {
                String errMsg = "No statements inside IF sequence";
                throw new InvalidSyntaxException(errMsg);
            }

            //If both above conditions were true parse the expression
            if(validFlag == 1)
            {
                String[] operators = new string[] { "=", "<", ">", "!" };
                String checker;
                String[] comparisionValues = Regex.Split(command[1], @"(?<=[<>!=])|(?=[<>!=])");
                for(int i=0; i<comparisionValues.Length; i++)
                {
                    checker = comparisionValues[i];
                    if (Array.Exists(operators, element=> element == checker) || int.TryParse(comparisionValues[i], out intTester))
                    {
                        expression += comparisionValues[i];
                    }
                    else
                    {
                        if (checkVariable(comparisionValues[i]) == 1)
                        {
                            expression += getVariable(comparisionValues[i]);
                        }
                        else
                        {
                            String errMsg  = comparisionValues[i]+" "+"Not a valid variable";
                            validFlag = 0;
                            throw new InvalidSyntaxException(errMsg);
                        }
                    }
                }
            }

            if(validFlag != 0)
            {
                expressionTrue = (bool)dt.Compute(expression, "");
            }

            //If the logical result of the expression is true return the commands in the statement
            if(expressionTrue is true)
            {
                for(int i = 2; i<command.Length; i++)
                {
                    if (command[i].ToUpper() == "ENDIF") { break; }
                    returnValue = returnValue + " " + command[i];                    
                }
                parseCommands(returnValue);
            }
        }

        /// <summary>
        /// parseLoop method obtains string belonging to WHILE sequence from parseSequence. It checks the condition of the WHILE statement and forwards
        /// all the statement inside the WHILE sequence to parseCommands in a loop until the condition is no longer true
        /// </summary>
        /// <param name="commands">The string representing WHILE sequence obtained from parseSequence</param>
        public void parseLoop(String commands)
        {
            String ifCommand = commands.Trim();
            var command = ifCommand.Split(new String[] { Environment.NewLine, ",", " " }, StringSplitOptions.RemoveEmptyEntries);

            //Regex to match the valid loop condition
            Regex condition = new Regex(@"^[A-Za-z]+[0-9]*(([<>]{1}[=]?)|([=]{1})|([!][=])){1}[A-Za-z0-9]+$");
            int validFlag = 0;
            DataTable dt = new DataTable();
            String expression = "";
            bool expressionTrue = false;
            String returnValue = "";

            //If the loopcondition matches with the above regex
            if (condition.IsMatch(command[1]))
            {
                validFlag = 1;
            }
            else
            {
                String errMsg  = "Invalid LOOP condition.";
                validFlag = 0;
                throw new InvalidSyntaxException(errMsg);
            }

            //If the loop has statements to execute
            if (command.Length > 3)
            {
                validFlag = 1;
            }
            else
            {
                String errMsg  = "No statements inside LOOP sequence";
                throw new InvalidSyntaxException(errMsg);
            }

            //If the both above if statement is valid parse the expression
            if (validFlag == 1)
            {
                expression = getExpression(command[1]);
                if (expression == "false")
                {
                    String errMsg  = "Invalid LOOP condition";
                    validFlag = 0;
                    throw new InvalidSyntaxException(errMsg);
                }
                else
                {
                    validFlag = 1;
                }
            }

            //If the expression is true start the loop
            if (validFlag == 1)
            {
                expressionTrue = (bool)dt.Compute(expression, "");
                while (expressionTrue is true)
                {
                    returnValue = "";
                    //sends the command for one loop
                    for (int i = 2; i < command.Length; i++)
                    {
                        if (command[i].ToUpper() == "ENDLOOP") { break; }
                        returnValue = returnValue + " " + command[i];
                    }
                    parseCommands(returnValue);
                    expression = getExpression(command[1]);
                    //checks if the expression is true after the statement is run
                    expressionTrue = (bool)dt.Compute(expression, "");
                }
            }
        }

        /// <summary>
        /// parseMethod method obtains string belong to METHOD declaration from parseSequence. It checks the validity of the METHOD declaration and adds
        /// a new method if the METHOD declaration is valid and updates the method if new declaration is valid.
        /// </summary>
        /// <param name="commands">The string representing METHOD declaration sequence obtained from parseSequence</param>
        public void parseMethod(String commands)
        {
            String methodCommand = commands.Trim();
            var isolateParameter = methodCommand.Split(new String[] { Environment.NewLine, " " }, StringSplitOptions.RemoveEmptyEntries);
            String methodName = "";
            var isolateBody = methodCommand.Split(new String[] { Environment.NewLine, " ","," }, StringSplitOptions.RemoveEmptyEntries);
            int signatureEnd = 0;

            //Regex condition to check Method signature
            Regex condition = new Regex(@"^[(]{1}(([A-Za-z]+[0-9]*)*|([A-Za-z]+[0-9]*[,]{1}[A-Za-z]+[0-9]*)*)[)]{1}$");
            //Regex to check if the method name is valid
            Regex methodNamecheck = new Regex(@"^[A-Za-z]+[0-9]*$");
            int validFlag = 0;
            String expression = "";
            String methodInfo = "";

            //If the method name is valid execute
            if (methodNamecheck.IsMatch(isolateParameter[1]))
            {
                validFlag = 1;
                methodName = isolateParameter[1];
            }
            else
            {
                String errMsg = "Invalid method name, "+isolateParameter[1]+ " method name must be alphanumeric only. Method signature should be of format abc (a,b).";
                validFlag = 0;
                throw new InvalidSyntaxException(errMsg);
            }

            //If the method signature is valid execute
            if (condition.IsMatch(isolateParameter[2]))
            {
                validFlag = 1;
            }
            else
            {
                String errMsg = "Invalid method signature "+isolateParameter[2]+" .";
                validFlag = 0;
                throw new InvalidSyntaxException(errMsg);
            }

            //If both above condition is valid isolate the method body and store it in a dictionary
            if(validFlag == 1)
            {
                var parameters = isolateParameter[2].Split(new String[] { "(", ",", ")" }, StringSplitOptions.RemoveEmptyEntries);
                methodInfo = methodInfo +" "+ parameters.Length;
                if (parameters.Length > 0) {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        methodInfo = methodInfo + " " + parameters[i];
                    }
                }

                for(int i = 0; i< isolateBody.Length; i++)
                {
                    if (isolateBody[i].ToUpper() == "ENDMETHOD")
                    {
                        break;
                    }   
                    if(signatureEnd == 1)
                    {
                        methodInfo = methodInfo + " " + isolateBody[i];
                    }
                    if (isolateBody[i].Contains(")"))
                    {
                        signatureEnd = 1;
                    }
                }
                if (checkMethod(methodName) == 1)
                {
                    methods[methodName] = methodInfo.Trim();
                }
                else
                {
                    methods.Add(methodName, methodInfo.Trim());
                }
            }


        }

        /// <summary>
        /// parseCommands method obtains non sequential group of commands and parses them into singular commands. The singular command is then forwarded to
        /// parseIndivCommand method.
        /// </summary>
        /// <param name="allCommands">The string representing a group of individual non sequential commands.</param>
        public void parseCommands(String allCommands)
        {
            String indivCommand = "";
            int commandFlag = 0;
            int methodFlag = 0;
            var command = (allCommands.Trim()).Split(new String[] { Environment.NewLine, ",", " "}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i<command.Length; i++)
            {
                String currentString = command[i];
                if (Array.Exists(commands, element => element == command[i].ToUpper())&& methodFlag==0)
                {
                    //if the string is not null send the command to parseCommand function
                    if (!String.IsNullOrEmpty(indivCommand) || !String.IsNullOrWhiteSpace(indivCommand))
                    {
                        parseIndivCommand(indivCommand);
                    }
                    //The indivCommand String is then reset and commandFlag is set to 1 to indicate the start of a valid command
                    indivCommand = "";
                    commandFlag = 1;
                }
                else if(Array.Exists(commands, element => element == command[i].ToUpper()) && methodFlag != 0)
                {
                    String errormsg = "Commands can't be given inside method calls";
                    throw new InvalidSyntaxException(errormsg);
                }
                //Regex to check for method brackets for method calls
                Regex oneParameter = new Regex(@"^.+[(].+[)]$");
                Regex noParameter = new Regex(@"^.+[(][)]$");

                //If the individual command is not a valid command
                if (!(int.TryParse(command[i], out intTester)) && !Array.Exists(colorModifiers, element => element == command[i].ToUpper()) &&
                    !Array.Exists(fillModifiers, element => element == command[i].ToUpper()))
                {
                    //Regex to check for variable assignment with = sign
                    Regex equalsFirst = new Regex(@"^=.+$");
                    Regex equalsOnly = new Regex(@"^=$");
                    Regex equalsLast = new Regex(@"^.+=$");
                    Regex expression = new Regex(@"^.+[=]{1}.+$");
                    bool assignmentTest;

                    //check for variable based on position of the string
                    if (i != command.Length-1)
                    {
                        assignmentTest = equalsLast.IsMatch(command[i]) || expression.IsMatch(command[i]) || 
                            equalsOnly.IsMatch(command[i + 1]) || equalsFirst.IsMatch(command[i + 1]);
                    }
                    else
                    {
                        assignmentTest = expression.IsMatch(command[i]);
                    }

                    //If the regex matches start a flag for an individual command
                    if (assignmentTest is true && methodFlag==0)
                    {
                        //if the string is not null send the command to parseCommand function
                        if (!String.IsNullOrEmpty(indivCommand) || !String.IsNullOrWhiteSpace(indivCommand))
                        {
                            parseIndivCommand(indivCommand);
                        }
                        indivCommand = "";
                        commandFlag = 1;
                    }
                    else if(assignmentTest is true && methodFlag>0)
                    {
                        String errormsg = "Variable can't be assigned inside a method";
                        throw new InvalidSyntaxException(errormsg);
                    }

                    //Regex to check method call
                    Regex bracketBefore = new Regex(@"^[(].*[)]?$");
                    Regex bracketAfter = new Regex(@"^.+[(]$");
                    Regex bracketOnly = new Regex(@"^[(]$");
                    Regex bracketBetween = new Regex(@"^.+[(].+$");
                    bool methodTest;

                    //check for method based on string position
                    if(i != command.Length-1)
                    {
                        methodTest = bracketAfter.IsMatch(command[i]) || oneParameter.IsMatch(command[i]) || bracketBetween.IsMatch(command[i]) 
                            || bracketBefore.IsMatch(command[i + 1]) || bracketOnly.IsMatch(command[i + 1]) ||noParameter.IsMatch(command[i]);
                    }
                    else
                    {
                        methodTest = oneParameter.IsMatch(command[i]) || noParameter.IsMatch(command[i]);
                    }

                    //If the method Test is true than start a new command for method call
                    if (methodTest is true && methodFlag== 0)
                    {
                        if (!String.IsNullOrEmpty(indivCommand) || !String.IsNullOrWhiteSpace(indivCommand))
                        {
                            parseIndivCommand(indivCommand);
                        }
                        indivCommand = "";
                        commandFlag = 1;
                        methodFlag++;
                    }
                    else if(methodTest is true && methodFlag>0)
                    {
                        methodFlag++;
                        String errormsg = "Method not declarable inside method";
                        throw new InvalidSyntaxException(errormsg);
                    }

                    //if none of the condition matches then the keyword is not valid in any context
                    if(methodFlag==0 && assignmentTest is false && methodTest is false && commandFlag==0)
                    {
                        String errormsg = "Invalid token "+command[i];
                        throw new InvalidSyntaxException(errormsg);
                    }
                }

                //Regex to check for method call end
                Regex endBracketAfter = new Regex(@"^.*[)]$");
                Regex endBracketBefore = new Regex(@"^[)].+$");

                bool methodEndTest;

                //check for the ) based on string position
                if (i != command.Length - 1)
                {
                    methodEndTest = endBracketAfter.IsMatch(command[i]) || endBracketBefore.IsMatch(command[i + 1])
                        || oneParameter.IsMatch(command[i]) || noParameter.IsMatch(command[i]);
                }
                else
                {
                    methodEndTest = oneParameter.IsMatch(command[i]) || noParameter.IsMatch(command[i])||endBracketAfter.IsMatch(command[i]);
                }
                
                //If method test is true forwards the current method command to another method
                if(methodEndTest is true && methodFlag==1)
                {
                    methodFlag--;
                    indivCommand = indivCommand + " " + command[i];
                    callMethod(indivCommand);
                    indivCommand = "";
                    commandFlag = 0;
                }
                else if(methodEndTest is true && methodFlag==0)
                {
                    String errormsg = "Unpaired paranthesis";
                    throw new InvalidSyntaxException(errormsg);
                }
                //If the commandFlag is 1, each succeeding value is added to individual command
                if (commandFlag == 1)
                {
                    indivCommand = indivCommand + " " + command[i];
                }
                //If this is the last string in the array, the current indivCommand is passed to parseCommand
                if (i == command.Length - 1)
                {
                    if (!String.IsNullOrEmpty(indivCommand) || !String.IsNullOrWhiteSpace(indivCommand))
                    {
                        parseIndivCommand(indivCommand);
                    }
                }
            }
        }

        /// <summary>
        /// callMethod method obtains method signature representing a method call from parseMethod, checks for the method body in the method dictionary and forwards
        /// the method body to parseCommands method.
        /// </summary>
        /// <param name="command">The string representing method call with the required values</param>
        public void callMethod(String command)
        {
            String methodHead = command.Trim();
            String[] methodParameters = methodHead.Split(new String[] { " ", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
            
            //Isolate the methodname and parameters
            String methodName = methodParameters[0];
            int methodParameterNum = methodParameters.Length - 1;
                        
            var methodBodyString = "";
            String methodBody = "";
            String finalString;

            int validFlag = 0;
            //Dictionary to store local variables for the method
            Dictionary<string, string> localVariables = new Dictionary<string, string>();

            String[] methodBodyValues;
            int methodParameterNo=0;

            //check if the methodname exists
            if (checkMethod(methodName) == 1)
            {
                validFlag = 1;
                methodBody = methods[methodName];
            }
            else
            {
                validFlag = 0;
                String errMsg = "The method "+methodName+ " doesn't exist";
                throw new InvalidSyntaxException(errMsg);
            }          

            //if the above check is valid execute
            if (validFlag == 1)
            {
                methodBodyValues = methodBody.Split(new String[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
                methodParameterNo = int.Parse(methodBodyValues[0]);

                //Check if the method call has required parameters
                if (methodParameterNo == methodParameterNum && validFlag == 1)
                {
                    validFlag = 1;
                }
                else
                {
                    validFlag = 0;
                    String errMsg = "Invalid no. of parameters for the method"+" "+methodName;
                    throw new InvalidSyntaxException(errMsg);
                }

                //If the call is valid parse the method
                if (methodParameterNo == 0 && validFlag == 1)
                {
                    for (int i = 1; i < methodBodyValues.Length; i++)
                    {
                        methodBodyString = methodBodyString + " " + methodBodyValues[i];
                    }
                }else if(methodParameterNo > 0 && validFlag==1)
                {
                    for(int i = 1; i<methodParameterNo+1; i++)
                    {
                        localVariables.Add(methodBodyValues[i], methodParameters[i]);
                    }
                    for (int i = methodParameterNo+1; i< methodBodyValues.Length; i++)
                    {
                        methodBodyString = methodBodyString + " " + methodBodyValues[i];
                    }
                    foreach (KeyValuePair<string,string> variableValue in localVariables)
                    {
                        String variable = variableValue.Key;
                        String value = variableValue.Value;
                        finalString = methodBodyString.Replace(variable, value);
                        methodBodyString = finalString;
                    }
                }
            }
            //Forward the parsed method to parseCommand
            if (!String.IsNullOrWhiteSpace(methodBodyString) || !String.IsNullOrEmpty(methodBodyString)) 
            {
                parseCommands(methodBodyString);
            }
        }

        /// <summary>
        /// parseIndivCommand obtains splitted strings from parseCommands and forwards the individual commands to either checkVariableAssignment method or 
        /// check command method based on the string.
        /// </summary>
        /// <param name="command">Individual command obtained from parseCommands method</param>
        public void parseIndivCommand(String command)
        {
            String indivCommand = command.Trim();
            //If the individual command contains = adds or appends the variable if the expression is valid
            if (indivCommand.Contains("="))
            {
                if (checkVariableAssignment(indivCommand) !="false")
                {
                    String variableAndValue = checkVariableAssignment(indivCommand);
                    var values = variableAndValue.Split(new String[] {" " }, StringSplitOptions.RemoveEmptyEntries);
                    if (checkVariable(values[0]) == 1)
                    {
                        variables[values[0]] = int.Parse(values[1]);
                    }
                    else
                    {
                        variables.Add(values[0], int.Parse(values[1]));
                    }
                }
            }
            //else sends it to be parsed by checkCommand which checks for individual command codes
            else
            {
                checkCommand(indivCommand);
            }
        }

        /// <summary>
        /// checkVariableAssignment obtains an assignment command from parseIndivCommand. It assigns or updates the variable if the command is valid.
        /// </summary>
        /// <param name="command">String represnting a variable assignment.</param>
        /// <returns></returns>
        public string checkVariableAssignment(String command)
        {
            int validFlag = 0;
            //Regex to check for valid identifier
            Regex identifier = new Regex(@"^[A-Za-z]+[A-Za-z0-9]*$");

            //Regex to check for valid expression
            Regex numericExpression = new Regex(@"^([-+*/]?[0-9]+)+$");
            Regex variableExpression = new Regex(@"^([-+*/]?[a-zA-Z0-9]+)+$");

            //Replaces all the spaces in the string with no space
            String noSpaceCommand = Regex.Replace(command, @"\s+", "");
            String expressionString = "";
            var expression = noSpaceCommand.Split(new String[] {"="}, StringSplitOptions.RemoveEmptyEntries);

            //Datatable to compute the expression
            DataTable dt = new DataTable();

            //If the variable assignment has required no. of expression execute
            if (expression.Length ==2)
            {
                validFlag = 1;
            }
            else if(expression.Length !=2)
            {
                String errorMsg = "Invalid variable assignment"+ " " +noSpaceCommand;
                throw new InvalidSyntaxException(errorMsg);
            }

            //if the identifier name is valid execute
            if (validFlag == 1 && identifier.IsMatch(expression[0]))
            {
                validFlag = 1;
            }
            else if(validFlag==1 && !identifier.IsMatch(expression[0]))
            {
                String errorMsg = "Variable identifiers must start with alphabet and can only be alphanumeric.";
                throw new InvalidSyntaxException(errorMsg);
            }

            String[] symbols = new String[] { "+", "-", "/", "*" };
            int intTester;

            //Checks if the expression is numeric or has a variable
            if(validFlag==1 && numericExpression.IsMatch(expression[1])){
                validFlag = 1;
                expressionString = expression[1];
                var expressionValue = dt.Compute(expression[1], "");
                expressionString = expressionValue.ToString();
            }
            //parses the expression with numbers if it has variables
            else if (validFlag==1 && variableExpression.IsMatch(expression[1]))
            {
                validFlag = 1;
                String[] characters = Regex.Split(expression[1], @"(?<=[-+*/])|(?=[-+*/])");
                String derivedExpression = "";
                for(int i =0; i<characters.Length; i++)
                {
                    if (!Array.Exists(symbols, element=> element == characters[i])&&!(int.TryParse(characters[i],out intTester)))
                    {
                        if (checkVariable(characters[i]) == 1)
                        {
                            int identifierValue = getVariable(characters[i]);
                            derivedExpression += identifierValue;
                        }
                        else
                        {
                            validFlag = 0;
                            String errorMsg = "Variable "+characters[i]+ " doesn't exist";
                            throw new InvalidSyntaxException(errorMsg);
                        }
                    }
                    else
                    {
                        derivedExpression += characters[i];
                    }
                }                
                if (validFlag == 1)
                {
                    var expressionValue = dt.Compute(derivedExpression, "");
                    expressionString = expressionValue.ToString();
                }
            }

            //returns the variable identifier and the parsed value
            if (validFlag == 1)
            {
                return expression[0] + " " + expressionString;
            }
            else
            {
                return "false";
            }
        }

        /// <summary>
        /// checkVariable checks if the given variable exists or not.
        /// </summary>
        /// <param name="identifier">Identifier of the variable to be checked.</param>
        /// <returns></returns>
        public int checkVariable(String identifier)
        {
            if (variables.ContainsKey(identifier))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// getVariable gets the variable value from the variables dictionary based on the identifier given.
        /// </summary>
        /// <param name="identifier">String representing the identifier of the variable.</param>
        /// <returns></returns>
        public int getVariable(String identifier)
        {
            int identifierValue;
            variables.TryGetValue(identifier, out identifierValue);            
            return identifierValue;
        }

        /// <summary>
        /// checkCommand method obtains individual shape commands from parseCommands method and executes them if they are valid.
        /// </summary>
        /// <param name="commands">Individual shape command obtained from parseCommand</param>
        public void checkCommand(String commands)
        {
            String indivCommand = commands.Trim();
            String[] command = indivCommand.Split(new String[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            Shape indivShape = null;
            int validFlag = 0;

            //validates a pen command and executes if valid
            if (String.Equals(command[0].ToUpper(), "PEN") && command.Length == 2)
            {
                if (Array.Exists(colorModifiers, element => element == command[1].ToUpper()))
                {
                    m_color = command[1];
                    validFlag = 1;
                }
                else
                {
                    String errMsg = "Invalid parameter for pen command";
                    throw new InvalidSyntaxException(errMsg);
                }
            }
            else if (String.Equals(command[0].ToUpper(), "PEN") && command.Length != 2)
            {
                String errMsg = "Invalid no. of parameter for pen command";
                throw new InvalidSyntaxException(errMsg);
            }

            //validates a pen fill and executes if valid
            if (String.Equals(command[0].ToUpper(), "FILL") && command.Length == 2)
            {
                if (Array.Exists(fillModifiers, element => element == command[1].ToUpper()))
                {
                    fillStatus = command[1];
                    validFlag = 1;
                }
                else
                {
                    String errMsg = "Invalid parameter for fill command";
                    throw new InvalidSyntaxException(errMsg);
                }
            }
            else if (String.Equals(command[0].ToUpper(), "FILL") && command.Length != 2)
            {
                String errMsg = "Invalid no. of parameter for fill command";
                throw new InvalidSyntaxException(errMsg);
            }

            //validates a moveto command and executes if valid
            if (String.Equals(command[0].ToUpper(), "MOVETO") && command.Length == 3)
            {
                if (int.TryParse(command[1], out intTester) && int.TryParse(command[1], out intTester))
                {
                    position1 = int.Parse(command[1]);
                    position2 = int.Parse(command[2]);
                    validFlag = 1;
                }
                else
                {
                    if(checkVariable(command[1])==1 && checkVariable(command[2]) == 1)
                    {
                        position1 = getVariable(command[1]);
                        position2 = getVariable(command[2]);
                        validFlag = 1;
                    }
                    else
                    {
                        String errMsg = "Invalid command for MOVETO command.";
                        throw new InvalidSyntaxException(errMsg);
                    }
                }
            }
            else if (String.Equals(command[0].ToUpper(), "MOVETO") && command.Length != 3)
            {
                String errMsg = "Invalid no. of commands for MOVETO command.";
                throw new InvalidSyntaxException(errMsg);
            }

            //validates a circle command and executes if valid
            if (String.Equals(command[0].ToUpper(), "CIRCLE") && command.Length == 2)
            {
                if (int.TryParse(command[1], out intTester))
                {
                    indivShape = ShapeFactory.ReturnShape("CIRCLE", position1, position2, m_color, fillStatus,int.Parse(command[1]));
                    shapelist.Add(indivShape);
                    validFlag = 1;
                }
                else
                {
                    if (checkVariable(command[1]) == 1)
                    {
                        indivShape = ShapeFactory.ReturnShape("CIRCLE", position1, position2, m_color, fillStatus, getVariable(command[1]));
                        shapelist.Add(indivShape);
                        validFlag = 1;
                    }
                    else
                    {
                        String errMsg = "Invalid variable name";
                        throw new InvalidSyntaxException(errMsg);
                    }
                }
            }
            else if (String.Equals(command[0].ToUpper(), "CIRCLE") && command.Length != 2)
            {
                String errMsg = "Invalid no. of commands for CIRCLE command.";
                throw new InvalidSyntaxException(errMsg);
            }

            //validates a rectangle command and executes if valid
            if (String.Equals(command[0].ToUpper(), "RECTANGLE") && command.Length == 3)
            {
                for (int i = 1; i < command.Length; i++)
                {
                    if (int.TryParse(command[i], out intTester))
                    {
                        validFlag = 1;
                    }
                    else if (checkVariable(command[i]) == 1)
                    {
                        command[i] = getVariable(command[i]).ToString();
                        validFlag = 1;
                    }
                    else
                    {
                        validFlag = 0;
                        String errMsg = "Invalid parameter for the rectangle command";
                        throw new InvalidSyntaxException(errMsg);
                    }
                }
                if(validFlag == 1)
                {
                    indivShape = ShapeFactory.ReturnShape("RECTANGLE", position1, position2, m_color, fillStatus, int.Parse(command[1]), int.Parse(command[2]));
                    shapelist.Add(indivShape);
                }
            }
            else if (String.Equals(command[0].ToUpper(), "RECTANGLE") && command.Length != 3)
            {
                String errMsg = "Invalid no. of commands for RECTANGLE command.";
                throw new InvalidSyntaxException(errMsg);
            }

            //validates a triangle command and executes if true
            if (String.Equals(command[0].ToUpper(), "TRIANGLE") && command.Length == 5)
            {
                for (int i = 1; i < command.Length; i++)
                {
                    if (int.TryParse(command[i], out intTester))
                    {
                        validFlag = 1;
                    }
                    else if (checkVariable(command[i]) == 1)
                    {
                        command[i] = getVariable(command[i]).ToString();
                        validFlag = 1;
                    }
                    else
                    {
                        validFlag = 0;
                        String errMsg = "Invalid parameter for the triangle command";
                        throw new InvalidSyntaxException(errMsg);
                    }
                }
                if (validFlag == 1)
                {
                    indivShape = ShapeFactory.ReturnShape("TRIANGLE", position1, position2, m_color, fillStatus, int.Parse(command[1]), int.Parse(command[2]), int.Parse(command[3]), int.Parse(command[4]));
                    shapelist.Add(indivShape);
                }
            }
            else if (String.Equals(command[0].ToUpper(), "TRIANGLE") && command.Length != 5)
            {
                String errMsg = "Invalid no. of commands for RECTANGLE command.";
                validFlag = 0;
                throw new InvalidSyntaxException(errMsg);
            }

            //validates a polygon command and executes if true
            if(String.Equals(command[0].ToUpper(),"POLYGON") && command.Length % 2 == 1 && command.Length>6)
            {
                int[] polygonPoints = new int[command.Length - 1];
                for (int i = 1; i < command.Length; i++)
                {
                    if (int.TryParse(command[i], out intTester))
                    {
                        validFlag = 1;
                        polygonPoints[i - 1] = int.Parse(command[i]);
                    }
                    else if (checkVariable(command[i]) == 1)
                    {
                        command[i] = getVariable(command[i]).ToString();
                        validFlag = 1;
                        polygonPoints[i - 1] = int.Parse(command[i]);
                    }
                    else
                    {
                        validFlag = 0;
                        String errMsg = "Invalid parameter for the POLYGON command";
                        throw new InvalidSyntaxException(errMsg);
                    }
                }
                if (validFlag == 1)
                {
                    indivShape = ShapeFactory.ReturnShape("POLYGON", position1, position2, m_color, fillStatus,polygonPoints);
                    shapelist.Add(indivShape);
                }
            }
            else if(String.Equals(command[0].ToUpper(),"POLYGON")&& command.Length%2 != 1)
            {
                String errMsg = "Invalid no. of parameters for POLYGON command.";
                validFlag = 0;
                throw new InvalidSyntaxException(errMsg);
            }

            //validates a drawto command and executees if true
            if (String.Equals(command[0].ToUpper(), "DRAWTO") && command.Length == 3)
            {
                if (int.TryParse(command[1], out intTester) && int.TryParse(command[1], out intTester))
                {
                    int count = 0;
                    foreach (Shape s in shapelist) { count++; };
                    if (count > 0)
                    {
                        Shape shape2 = (Shape)shapelist[count - 1];
                        shape2.setPoint(int.Parse(command[1]), int.Parse(command[2]));
                        validFlag = 1;
                    }
                }
                else
                {
                    if (checkVariable(command[1]) == 1 && checkVariable(command[2]) == 1)
                    {
                        int count = 0;
                        foreach (Shape s in shapelist) { count++; };
                        if (count > 0)
                        {
                            Shape shape2 = (Shape)shapelist[count - 1];
                            shape2.setPoint(getVariable(command[1]), getVariable(command[2]));
                            validFlag = 1;
                        }
                    }
                    else
                    {
                        String errMsg = "Invalid variable";
                        validFlag = 0;
                        throw new InvalidSyntaxException(errMsg);
                    }
                }
            }
            else if (String.Equals(command[0].ToUpper(), "DRAWTO") && command.Length != 3)
            {
                String errMsg = "Invalid parameters for DRAWTO command";
                validFlag = 0;
                throw new InvalidSyntaxException(errMsg);
            }            
        }

        /// <summary>
        /// checkMethod method checks if the method name exists in methods dictionary and returns value based on the result.
        /// </summary>
        /// <param name="identifier">The string representing method name.</param>
        /// <returns></returns>
        public int checkMethod(String identifier)
        {
            if (methods.ContainsKey(identifier))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// getMethod method returns method body if based on the method identifier given
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public String getMethod(String identifier)
        {
            String identifierValue;
            methods.TryGetValue(identifier, out identifierValue);
            return identifierValue;
        }

        /// <summary>
        /// getExpresion method takes an logical expression with a variable and returns a mathematical expression.
        /// </summary>
        /// <param name="condition">Condition represents a logical expression with variables instead of numbers.</param>
        /// <returns></returns>
        public String getExpression (String condition)
        {
            String[] operators = new string[] { "=", "<", ">", "!" };
            String checker;
            String[] comparisionValues = Regex.Split(condition, @"(?<=[<>!=])|(?=[<>!=])");
            String expression="";
            int validFlag = 0;
            for (int i = 0; i < comparisionValues.Length; i++)
            {
                checker = comparisionValues[i];
                if (Array.Exists(operators, element => element == checker) || int.TryParse(comparisionValues[i], out intTester))
                {
                    expression += comparisionValues[i];
                    validFlag = 1;
                }
                else
                {
                    if (checkVariable(comparisionValues[i]) == 1)
                    {
                        expression += getVariable(comparisionValues[i]);
                        validFlag = 1;
                    }
                    else
                    {
                        String errMsg = "Not a valid variable";
                        throw new InvalidSyntaxException(errMsg);
                    }
                }
            }

            if(validFlag == 1)
            {
                return expression;
            }
            else
            {
                return "false";
            }
        }

        /// <summary>
        /// resets the origin to top left corner
        /// </summary>
        public void resetPoints()
        {
            position1 = 0;
            position2 = 0;
        }

        /// <summary>
        /// clear the shapelist array
        /// </summary>
        public void resetShapes()
        {
            shapelist.Clear();
            fillStatus = "OFF";
            m_color = "BLACK";
            position1 = 0;
            position2 = 0;
        }
    }
}
