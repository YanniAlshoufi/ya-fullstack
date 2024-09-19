using System.Runtime.Serialization;

namespace Helpers.Assertions;

public class AssertionException : Exception
{
    public AssertionException()
    {
    }

    [Obsolete("Obsolete")]
    protected AssertionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public AssertionException(string? message)
        : base(message)
    {
    }

    public AssertionException(string? message, Exception? innerException)
        : base(message, innerException)
    
    {
    }
}