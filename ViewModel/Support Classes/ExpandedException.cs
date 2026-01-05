using System;

namespace ViewModel
{
    public class ExpandedException : Exception
    {
        public string? SqlErrorText { get; }

        /// <summary>
        /// Single constructor to handle all error scenarios:
        /// 1. Message only (404)
        /// 2. Message + SQL
        /// 3. Message + Inner Exception
        /// 4. Message + SQL + Inner Exception
        /// </summary>
        public ExpandedException(
            string message,
            string? sqlStatement = null,
            Exception? innerException = null)
            : base(message, innerException){
            SqlErrorText = sqlStatement;
        }
    }
}