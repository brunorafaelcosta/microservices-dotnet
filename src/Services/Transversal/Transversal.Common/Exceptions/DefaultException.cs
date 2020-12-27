using System;

namespace Transversal.Common.Exceptions
{
    /// <summary>
    /// Default exception.
    /// </summary>
    public class DefaultException : Exception
    {
        public DefaultException()
        {
        }

        public DefaultException(string message)
            : base(message)
        {
        }

        public DefaultException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
