using System.Net.Http.Json;
using GlobalHelpers.Monads;
using Shared.Data.Models;
using Shared.ServicesInterfaces;

namespace Client.Services;

public class UsersService : IUsersService
{
    private const string ServerUrl = "https://localhost:7201";
    
    private readonly HttpClient _client;

    public UsersService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Result<List<User>, Exception>> GetUsersAsync()
    {
        var usersRes = await _client.GetAsync($"{ServerUrl}/users");
        usersRes.EnsureSuccessStatusCode();
        var users = await usersRes.Content.ReadFromJsonAsync<User[]>();

        if (users == null)
        {
            throw new Exception("Failed to fetch users");
        }

        List<User> usersList = [..users];
        return usersList;
    }

    public async Task<Result<User, Exception>> GetUserAsync(Guid id)
    {
        var usersRes = await _client.GetAsync($"{ServerUrl}/users/{id}");
        usersRes.EnsureSuccessStatusCode();
        var user = await usersRes.Content.ReadFromJsonAsync<User>();

        if (user == null)
        {
            throw new Exception("Failed to fetch user");
        }

        return user;
    }

    public async Task<Result<User, Exception>> CreateUserAsync(User tmpUser)
    {
        var userRes = await _client.PostAsJsonAsync($"{ServerUrl}/users", tmpUser);
        userRes.EnsureSuccessStatusCode();
        var user = await userRes.Content.ReadFromJsonAsync<User>();

        if (user == null)
        {
            throw new Exception("Failed to create user");
        }

        return user;
    }

    public async Task<Result<User, Exception>> UpdateUserAsync(User tmpUser)
    {
        var userRes = await _client.PutAsJsonAsync($"{ServerUrl}/users/{tmpUser.Id}", tmpUser);
        userRes.EnsureSuccessStatusCode();
        var user = await userRes.Content.ReadFromJsonAsync<User>();

        if (user == null)
        {
            throw new Exception("Failed to set user");
        }

        return user;
    }

    public async Task<Option<Exception>> DeleteUserAsync(Guid id)
    {
        var userRes = await _client.DeleteAsync($"{ServerUrl}/users/{id}");
        userRes.EnsureSuccessStatusCode();
        return Option<Exception>.NoneValue;
    }
}
