using Riok.Mapperly.Abstractions;
using Shared.Data.Dtos;
using Shared.Data.Models;

namespace Shared.Data.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial User MapToModel(UserDto model);
}