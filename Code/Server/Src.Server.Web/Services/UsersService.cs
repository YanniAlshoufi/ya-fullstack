using Microsoft.EntityFrameworkCore;
using Src.Data.ClassLib.DbContexts;
using Src.Shared.ClassLib.Data.Dtos.Request;
using Src.Shared.ClassLib.Data.Dtos.Response;
using Src.Shared.ClassLib.HttpErrors;
using Src.Shared.ClassLib.ServicesInterfaces;
using Src.Web.Mappers;
using YaMonads;

namespace Src.Web.Services;

public class UsersService : IUsersService
{
    private readonly UsersContext _context;
    private readonly UserMapper _mapper;

    public UsersService(UsersContext context, UserMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserResDto>> GetAllUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(user => _mapper.MapToResDto(user));
    }

    public async Task<Result<UserResDto, NotFound>> GetUserAsync(int id)
    {
        var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        
        if (user is null)
        {
            return new NotFound();
        }
        
        return _mapper.MapToResDto(user);
    }

    public async Task<Result<UserResDto, BadRequest>> CreateUserAsync(UserReqDto reqDto)
    {
        var user = _mapper.MapToModel(reqDto);
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        return _mapper.MapToResDto(user);
    }

    public async Task<Result<UserResDto, NotFound>> SetUserAsync(int id, UserReqDto reqDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            return new NotFound();
        }

        user.FirstName = reqDto.FirstName.Trim();
        user.LastName = reqDto.LastName.Trim();
        
        await _context.SaveChangesAsync();

        return _mapper.MapToResDto(user);
    }

    public async Task<Option<NotFound>> DeleteUserAsync(int id)
    {
        var countDeleted = await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
        
        if (countDeleted <= 0)
        {
            return new NotFound();
        }

        await _context.SaveChangesAsync();
        
        return Option<NotFound>.NoneValue;
    }
}