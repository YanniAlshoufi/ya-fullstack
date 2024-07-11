using FluentValidation;
using Src.Shared.ClassLib.Data.Dtos.Request;
using Src.Shared.ClassLib.Data.Dtos.Response;

namespace Src.Shared.ClassLib.Data.DtoValidators;

public class UserReqDtoValidator : AbstractValidator<UserReqDto>
{
    public UserReqDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .TrimmedNotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .TrimmedNotEmpty()
            .MaximumLength(50);
    }
}

public class UserResDtoValidator : AbstractValidator<UserResDto>
{
    public UserResDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .TrimmedNotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .TrimmedNotEmpty()
            .MaximumLength(50);
    }
}
