using Microsoft.AspNetCore.Mvc;
using Shared.Data.Dtos;
using Shared.Data.Mappers;
using Shared.Data.Models;
using Shared.Exceptions;
using Shared.ServicesInterfaces;

namespace Server.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _service;
    private readonly ILogger<User> _logger;

    public UsersController(IUsersService service, ILogger<User> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
    {
        var usersResult = await _service.GetUsersAsync();

        return usersResult.Match<IActionResult>(
            users => Ok(users),
            exception =>
            {
                _logger.LogError("Failed to get all users: {}", exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser(UserDto dto, UserMapper mapper)
    {
        var user = mapper.MapToModel(dto);
        var creationResult = await _service.CreateUserAsync(user);

        return creationResult.Match<IActionResult>(
            user => CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user),
            exception =>
            {
                _logger.LogError("Failed to create user: {}", exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpGet("{id:guid}", Name = nameof(GetUserById))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var userResult = await _service.GetUserAsync(id);
        return userResult.Match<IActionResult>(
            user => Ok(user),
            exception =>
            {
                _logger.LogError("Failed to get user by id: {}", exception.Message);

                if (exception is NotFoundException)
                {
                    return NotFound();
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var deletionResult = await _service.DeleteUserAsync(id);

        return deletionResult.Match<IActionResult>(
            exception =>
            {
                if (exception is NotFoundException)
                {
                    return NotFound();
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            },
            NoContent);
    }
}