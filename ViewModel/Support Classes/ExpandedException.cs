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
        /// and an inner ExpandedException (c# error)
        ///</summary>
        public ExpandedException(string message, string sqlStatement, Exception innerException)
            : base(message, innerException)
        {
            this.SqlErrorText = sqlStatement;
        }

        /// <summary>
        /// use this constructor when there is SQL error text to provide
        /// but no inner ExpandedException (c# error)
        ///</summary>
        public ExpandedException(string message, string sqlStatement)
            : base(message)
        {
            this.SqlErrorText = sqlStatement;
        }

        /// <summary>
        /// use this constructor when there is no SQL error text to provide
        /// 404 - Not Found scenarios
        /// </summary>
        /// <param name="message"></param>
        public ExpandedException(string message)
            : base(message)
        {
            this.SqlErrorText = null;
        }
    }
}
