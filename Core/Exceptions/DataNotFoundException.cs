using System.Data;

namespace Galaxon.Core.Exceptions;

/// <summary>
/// Exception for when an attempt is made to obtain data from a database, but it isn't found.
/// It's a bit more specific than DataException, and sometimes ObjectNotFoundException is not
/// exactly what is meant.
/// </summary>
public class DataNotFoundException : DataException
{
    /// <summary>
    /// Initializes a new instance of the DataNotFoundException class with an error message
    /// (optional), and a reference to the inner exception that is the cause of this exception
    /// (optional).
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public DataNotFoundException(string? message = null, Exception? innerException = null)
        : base(message, innerException) { }
}
