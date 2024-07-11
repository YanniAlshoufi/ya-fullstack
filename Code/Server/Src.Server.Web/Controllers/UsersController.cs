using Microsoft.AspNetCore.Mvc;
using Src.Data.ClassLib.Models;
using Src.Shared.ClassLib.Data.Dtos.Request;
using Src.Shared.ClassLib.ServicesInterfaces;

namespace Src.Web.Controllers;

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
        return Ok(await _service.GetAllUsersAsync());
    }

    [HttpGet("{id:int}", Name = nameof(GetUserById))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(int id)
    {
        var userResult = await _service.GetUserAsync(id);

        return userResult.Match<IActionResult>(
            forOk: Ok,
            forErr: notFound =>
            {
                _logger.LogError("Failed to get user by id: {}", notFound.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser(UserReqDto dto)
    {
        var creationResult = await _service.CreateUserAsync(dto);

        return creationResult.Match<IActionResult>(
            forOk: user => CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user),
            forErr: badRequest =>
            {
                _logger.LogError("Failed to create user: {}", badRequest.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetUser(int id, UserReqDto reqDto)
    {
        var updatedUserResult = await _service.SetUserAsync(id, reqDto);

        return updatedUserResult.Match<IActionResult>(
            forOk: Ok,
            forErr: notFound => NotFound(notFound.Message));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var optionalNotFound = await _service.DeleteUserAsync(id);

        return optionalNotFound.Match<IActionResult>(
            notFound =>
            {
                _logger.LogError("{}", notFound.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            },
            NoContent);
    }
}