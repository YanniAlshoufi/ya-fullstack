using Data.DbContexts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Dtos;
using Shared.Data.Mappers;
using Shared.Data.Models;

namespace Server.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private readonly UsersContext _usersCx;

    public UsersController(UsersContext usersCx)
    {
        _usersCx = usersCx;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _usersCx.Users.ToListAsync());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> CreateUser(UserDto dto, UserMapper mapper)
    {
        var user = mapper.MapToModel(dto);
        await _usersCx.AddAsync(user);
        await _usersCx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }
    
    [HttpGet("{id:guid}", Name = nameof(GetUserById))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _usersCx.Users.FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }
}