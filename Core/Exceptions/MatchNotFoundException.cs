namespace Galaxon.Core.Exceptions;

/// <summary>
/// This exception type is similar to ArgumentOutOfRangeException, except intended for use in switch
/// statements and expressions, when the value being tested matches none of the patterns.
/// It could also be used for when a string doesn't match a regular expression.
/// </summary>
public class MatchNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public MatchNotFoundException() { }

    /// <summary>
    /// Initializes a new instance with an error message, and (optionally) a reference to the inner
    /// exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public MatchNotFoundException(string? message, Exception? innerException = null)
        : base(message, innerException) { }
}
