using System;
using Transversal.Common.Exceptions;

namespace Transversal.Application.Exceptions
{
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
