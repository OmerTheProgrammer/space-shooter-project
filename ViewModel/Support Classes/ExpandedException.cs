using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ExpandedException : Exception
    {
        public string? SqlErrorText { get; }

        /// <summary>
        /// use this constructor when there is SQL error text to provide
        /// and an inner Exception (c# error)
        /// and your message(note)
        /// usefull for 409,400,500 scenarios
        ///</summary>
        public ExpandedException(string message, string sqlStatement, Exception innerException)
            : base(message, innerException)
        {
            this.SqlErrorText = sqlStatement;
        }

        /// <summary>
        /// use this constructor when there is SQL error text to provide
        /// but no Exception (c# error)
        /// and your message(note)
        ///</summary>
        public ExpandedException(string message, string sqlStatement)
            : base(message)
        {
            this.SqlErrorText = sqlStatement;
        }

        /// <summary>
        /// use this constructor when there c# error and
        /// your message(note) and
        /// no SQL error text to provide
        ///</summary>
        public ExpandedException(string message, Exception ex)
            : base(message)
        {
            this.SqlErrorText = null;
        }

        /// <summary>
        /// use this constructor when there is no SQL error text to provide
        /// just your message(note)
        /// usefull for 404 - Not Found scenarios
        /// </summary>
        /// <param name="message"></param>
        public ExpandedException(string message)
            : base(message)
        {
            this.SqlErrorText = null;
        }
    }
}
