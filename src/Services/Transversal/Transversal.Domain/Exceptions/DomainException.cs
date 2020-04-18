using System;
using Transversal.Common.Exceptions;

namespace Transversal.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown in the Domain context.
    /// </summary>
    public class DomainException : DefaultException
    {
        public DomainException()
        {
        }

        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
