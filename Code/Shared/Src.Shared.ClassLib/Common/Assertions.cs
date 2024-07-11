namespace Src.Shared.ClassLib.Common;

public static class Assertions
{
    public static void Assert(bool condition, string message = "Assertion failed!")
    {
        if (condition == false)
        {
            throw new AssertionException(message);
        }
    }
}