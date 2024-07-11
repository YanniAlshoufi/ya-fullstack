using Src.Shared.ClassLib.Data.Dtos.Request;
using Src.Shared.ClassLib.Data.Dtos.Response;
using Src.Shared.ClassLib.HttpErrors;
using YaMonads;

namespace Src.Shared.ClassLib.ServicesInterfaces;

public interface IUsersService
{
    /// <summary>
    /// Gets all users form the database
    /// </summary>
    /// <returns>The users' response dto enumerable</returns>
    Task<IEnumerable<UserResDto>> GetAllUsersAsync();

    /// <summary>
    /// Gets a user by id from the database
    /// </summary>
    /// <param name="id">The id of the user</param>
    /// <returns>The user response dto or <see cref="NotFound"/> if the user was not found</returns>
    Task<Result<UserResDto, NotFound>> GetUserAsync(int id);

    /// <summary>
    /// Creates a user in the database
    /// </summary>
    /// <param name="reqDto">The user request dto</param>
    /// <returns>The created user response dto or <see cref="BadRequest"/>
    /// if the <see cref="reqDto"/> provided is invalid</returns>
    Task<Result<UserResDto, BadRequest>> CreateUserAsync(UserReqDto reqDto);

    /// <summary>
    /// Sets a user in the database with an idempotent operation
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <param name="reqDto">The user request dto or <see cref="NotFound"/>
    ///     if the user was not found</param>
    /// <returns></returns>
    Task<Result<UserResDto, NotFound>> SetUserAsync(int id, UserReqDto reqDto);

    /// <summary>
    /// Deletes a user from the database
    /// </summary>
    /// <param name="id">The id of the user to delete</param>
    /// <returns>If the user was not found, then <see cref="NotFound"/></returns>
    Task<Option<NotFound>> DeleteUserAsync(int id);
}
