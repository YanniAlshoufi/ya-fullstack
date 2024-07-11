using FluentValidation;
using Shared.Data.Dtos;

namespace Shared.Data.DtoValidators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmptyAndWithAMaxLengthOf50();

        RuleFor(x => x.LastName)
            .NotEmptyAndWithAMaxLengthOf50();
    }
}

internal static class Extensions
{
    internal static IRuleBuilderOptions<T, string?> NotEmptyAndWithAMaxLengthOf50<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must(x =>
        {
            var trimmed = x?.Trim();
            return (trimmed?.Length ?? int.MaxValue) <= 50;
        });
    }
}