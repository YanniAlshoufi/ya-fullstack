using System.Net.Http.Json;
using Src.Client.BlazorWasm.Constants;
using Src.Shared.ClassLib.Common;
using Src.Shared.ClassLib.Data.Dtos.Request;
using Src.Shared.ClassLib.Data.Dtos.Response;
using Src.Shared.ClassLib.HttpErrors;
using Src.Shared.ClassLib.ServicesInterfaces;
using YaMonads;

namespace Src.Client.BlazorWasm.Services;

public class UsersService : IUsersService
{
    private readonly HttpClient _client;
    private readonly ILogger<UsersService> _logger;

    public UsersService(HttpClient client, ILogger<UsersService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IEnumerable<UserResDto>> GetAllUsersAsync()
    {
        var usersRes = await _client.GetAsync($"{Urls.ServerUrl}/users");
        usersRes.EnsureSuccessStatusCode();
        var users = await usersRes.Content.ReadFromJsonAsync<UserResDto[]>();
        Assertions.Assert(users is not null, "Users' response dtos must not be null");
        return users!;
    }

    public async Task<Result<UserResDto, NotFound>> GetUserAsync(int id)
    {
        var userRes = await _client.GetAsync($"{Urls.ServerUrl}/users/{id}");
        if (!userRes.IsSuccessStatusCode) { return new NotFound();}
        var userResDto = await userRes.Content.ReadFromJsonAsync<UserResDto>();
        Assertions.Assert(userResDto is not null, "User response dto mut not be null");
        return userResDto!;
    }

    public async Task<Result<UserResDto, BadRequest>> CreateUserAsync(UserReqDto reqDto)
    {
        var userRes = await _client.PostAsJsonAsync($"{Urls.ServerUrl}/users", reqDto);
        if (!userRes.IsSuccessStatusCode) { return new BadRequest();}
        var userResDto = await userRes.Content.ReadFromJsonAsync<UserResDto>();
        Assertions.Assert(userResDto is not null, "User response dto mut not be null");
        return userResDto!;
    }

    public async Task<Result<UserResDto, NotFound>> SetUserAsync(int id, UserReqDto reqDto)
    {
        var userRes = await _client.PutAsJsonAsync($"{Urls.ServerUrl}/users/{id}", reqDto);
        if (!userRes.IsSuccessStatusCode) { return new NotFound();}
        var userResDto = await userRes.Content.ReadFromJsonAsync<UserResDto>();
        Assertions.Assert(userResDto is not null, "User response dto mut not be null");
        return userResDto!;
    }

    public async Task<Option<NotFound>> DeleteUserAsync(int id)
    {
        var deletionRes = await _client.DeleteAsync($"{Urls.ServerUrl}/users/{id}");
        if (!deletionRes.IsSuccessStatusCode) { return new NotFound();}
        return Option<NotFound>.NoneValue;
    }
}