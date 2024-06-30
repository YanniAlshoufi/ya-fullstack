using FluentValidation;
using Shared.Data.Dtos;

namespace Shared.Data.DtoValidators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.TrimmedFirstName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.TrimmedLastName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);
    }
}