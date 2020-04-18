using System;
using Transversal.Common.Exceptions;

namespace Transversal.Application.Exceptions
{
    /// <summary>
    /// Exception thrown in the Application context.
    /// </summary>
    public class AppException : DefaultException
    {
        public AppException()
        {
        }

        public AppException(string message)
            : base(message)
        {
        }

        public AppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
