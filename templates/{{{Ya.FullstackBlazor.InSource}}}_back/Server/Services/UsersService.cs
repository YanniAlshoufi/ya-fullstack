using Data.DbContexts;
using GlobalHelpers.Monads;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Mappers;
using Shared.Data.Models;
using Shared.Exceptions;
using Shared.ServicesInterfaces;

namespace Server.Services;

public class UsersService : IUsersService
{
    private readonly UsersContext _context;
    private readonly UserMapper _mapper;

    public UsersService(UsersContext context, UserMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<User>, Exception>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Result<User, Exception>> GetUserAsync(Guid id) 
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
        {
            throw new NotFoundException();
        }

        return user;
    }

    public async Task<Result<User, Exception>> CreateUserAsync(User tmpUser)
    {
        // var user = _mapper.MapToModel(tmpUser);
        await _context.AddAsync(tmpUser);
        await _context.SaveChangesAsync();
        return tmpUser;
    }

    public async Task<Result<User, Exception>> UpdateUserAsync(User tmpUser)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == tmpUser.Id);

        if (user == null)
        {
            throw new NotFoundException();
        }

        // TODO
        // var newUser = _mapper.MapToModel(tmpUser);
        tmpUser.FirstName = tmpUser?.FirstName?.Trim() ?? throw new NullReferenceException();
        tmpUser.LastName = tmpUser?.LastName?.Trim() ?? throw new NullReferenceException();

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<Option<Exception>> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
        {
            throw new NotFoundException();
        }

        _context.Remove(user);
        await _context.SaveChangesAsync();

        return Option<Exception>.NoneValue;
    }
}