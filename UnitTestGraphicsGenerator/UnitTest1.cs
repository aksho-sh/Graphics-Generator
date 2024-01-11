using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Assignment1;
using System.Collections;


namespace UnitTestAssignment1
{
    /// <summary>
    /// Test class for testing various methods in Assignment1 project
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Tests if the factory class returns circle with the required parameters
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            string shape = "CIRCLE";
            int point1 = 0;
            int point2 = 0;
            string color = "RED";
            string fillstatus = "ON";
            int radius = 4;
            Shape s;
            Boolean expected = true;
            Boolean actual;

            //Act
            s = ShapeFactory.ReturnShape(shape, point1, point2, color, fillstatus, radius);
            if (s.GetType() == typeof(Circle))
            {
                actual = true;
            }
            else
            {
                actual = false;
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// Tests if the factory class returns rectangle with the required parameters
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            //Arrange
            string shape = "RECTANGLE";
            int point1 = 0;
            int point2 = 0;
            string color = "RED";
            string fillstatus = "ON";
            int width = 4;
            int height = 4;
            Shape s;
            Boolean expected = true;
            Boolean actual;

            //Act
            s = ShapeFactory.ReturnShape(shape, point1, point2, color, fillstatus, width, height);
            if (s.GetType() == typeof(Rectangle))
            {
                actual = true;
            }
            else
            {
                actual = false;
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests if the factory class returns triangle with the given parameters
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            //Arrange
            string shape = "TRIANGLE";
            int point1 = 0;
            int point2 = 0;
            int ver1 = 0;
            int ver2 = 30;
            int ver3 = 30;
            int ver4 = 40;
            string color = "RED";
            string fillstatus = "ON";
            Shape s;
            Boolean expected = true;
            Boolean actual;

            //Act
            s = ShapeFactory.ReturnShape(shape, point1, point2, color, fillstatus, ver1, ver2, ver3, ver4);
            if (s.GetType() == typeof(Triangle))
            {
                actual = true;
            }
            else
            {
                actual = false;
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests if the factory class returns polygon with the given parameters
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            //Arrange
            string shape = "POLYGON";
            int point1 = 0;
            int point2 = 0;
            int ver1 = 0;
            int ver2 = 30;
            int ver3 = 30;
            int ver4 = 40;
            string color = "RED";
            string fillstatus = "ON";
            Shape s;
            Boolean expected = true;
            Boolean actual;

            //Act
            s = ShapeFactory.ReturnShape(shape, point1, point2, color, fillstatus, ver1, ver2, ver3, ver4);
            if (s.GetType() == typeof(Polygon))
            {
                actual = true;
            }
            else
            {
                actual = false;
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests if the command parser class returns the shape object with the given command
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            //Arrange
            string command = "Circle 40";
            CommandParser c1 = new CommandParser();
            ArrayList a1 = c1.parseSequence(command);
            bool expected = true;
            bool actual;
            //Act
            if (a1[0].GetType() == typeof(Circle))
            {
                actual = true;
            }
            else
            {
                actual = false;
            }
            //Assertion
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests if commandParser class enterprets the valid expression as valid
        /// </summary>
        [TestMethod]
        public void TestMethod6()
        {
            //Arrange
            string command = "a=10* 20";
            CommandParser c1 = new CommandParser();
            string expected = "a 200";
            string actual;
            //Act
            actual = c1.checkVariableAssignment(command);
            //Assertion
            Assert.AreEqual(expected, actual);
        }

        
        /// <summary>
        /// Tests if variable exists after passing the  variable assigning command to commandparser
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            //Arrange
            string command = "a=10";
            CommandParser c1 = new CommandParser();
            c1.parseSequence(command);
            int expected = 1;
            int actual;
            //Act
            actual = c1.checkVariable("a");
            //Assertion
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests if accurate variable value is returned by getVariable method after running the command in the commandParser
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            //Arrange
            string command = "a=10";
            CommandParser c1 = new CommandParser();
            c1.parseSequence(command);
            int expected = 10;
            int actual;
            //Act
            actual = c1.getVariable("a");
            //Assertion
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests if method exists after declaring the method through parser.
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            //Arrange
            string command = "method abc (ab,bc) circle ab circle bc endmethod";
            CommandParser c1 = new CommandParser();
            c1.parseSequence(command);
            int expected = 1;
            int actual;
            //Act
            actual = c1.checkMethod("abc");
            //Assertion
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests if the method body returned after declaration of method through command parser is accurate.
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            //Arrange
            String command = "method abc (ab,bc) circle ab circle bc endmethod";
            CommandParser c1 = new CommandParser();
            c1.parseSequence(command);
            string expected = "2 ab bc circle ab circle bc";
            string actual;
            //Act
            actual = c1.getMethod("abc");
            //Assertion
            Assert.AreEqual(expected, actual);
        }
    }
}
