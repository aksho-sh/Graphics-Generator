using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    /// <summary>
    /// InvalidSyntaxException is a usermade exception to throw syntax errors.
    /// </summary>
    public class InvalidSyntaxException:Exception
    {
        /// <summary>
        /// Constructor for InvalidSyntaxException class
        /// </summary>
        /// <param name="message">Error message associated with the exception.</param>
        public InvalidSyntaxException(String message):base(message)
        {

        }
    }
}
