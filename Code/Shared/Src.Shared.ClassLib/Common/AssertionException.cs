namespace Src.Shared.ClassLib.Common;

public class AssertionException : Exception
{
    public AssertionException() : base() {}
    public AssertionException(string message) : base(message) {}
    public AssertionException(string message, Exception e) : base(message, e) {}
}