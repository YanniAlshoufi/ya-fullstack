using Riok.Mapperly.Abstractions;
using Src.Data.ClassLib.Models;
using Src.Shared.ClassLib.Data.Dtos.Request;
using Src.Shared.ClassLib.Data.Dtos.Response;

namespace Src.Web.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial User MapToModel(UserReqDto reqDto);
    public partial UserResDto MapToResDto(User model);
}