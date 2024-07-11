using GlobalHelpers.Monads;
using Shared.Data.Models;

namespace Shared.ServicesInterfaces;

// TODO
public interface IUsersService
{
    Task<Result<List<User>, Exception>> GetUsersAsync();
    Task<Result<User, Exception>> GetUserAsync(Guid id);
    Task<Result<User, Exception>> CreateUserAsync(User user);
    Task<Result<User, Exception>> UpdateUserAsync(User updatedUser);
    Task<Option<Exception>> DeleteUserAsync(Guid id);
}
