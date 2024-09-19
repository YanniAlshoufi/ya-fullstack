using LanguageExt;

namespace Helpers.Assertions;

public static class Assertions
{
    public static void Assert(bool assertionCondition, Option<string> messageIfError = default)
    {
        if (!assertionCondition)
        {
            throw new AssertionException( messageIfError
                    .Match(
                        error => $"Assertion failed: {error}",
                        () => "Assertion failed."));
        }
    }
}