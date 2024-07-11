using FluentValidation;

namespace Src.Shared.ClassLib.Data.DtoValidators;

internal static class Extensions
{
    internal static IRuleBuilderOptions<T, string> TrimmedNotEmpty<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(x =>
        {
            var trimmed = x.Trim();
            return trimmed.Length > 0;
        });
    }
}
