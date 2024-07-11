using System.Net.Http.Json;
using Src.Client.BlazorWasm.Constants;
using Src.Shared.ClassLib.Common;
using Src.Shared.ClassLib.Data.Dtos.Request;
using Src.Shared.ClassLib.Data.Dtos.Response;
using Src.Shared.ClassLib.HttpErrors;
using Src.Shared.ClassLib.ServicesInterfaces;
using YaMonads;

namespace Src.Client.BlazorWasm.Services;

public class {{PLURAL}}Service : I{{PLURAL}}Service
{
    private readonly HttpClient _client;
    private readonly ILogger<{{PLURAL}}Service> _logger;

    public {{PLURAL}}Service(HttpClient client, ILogger<{{PLURAL}}Service> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IEnumerable<{{SINGULAR}}ResDto>> GetAll{{PLURAL}}Async()
    {
        var {{PLURALVAR}}Res = await _client.GetAsync($"{Urls.ServerUrl}/{{PLURALVAR}}");
        {{PLURALVAR}}Res.EnsureSuccessStatusCode();
        var {{PLURALVAR}} = await {{PLURALVAR}}Res.Content.ReadFromJsonAsync<{{SINGULAR}}ResDto[]>();
        Assertions.Assert({{PLURALVAR}} is not null, "{{PLURAL}}' response dtos must not be null");
        return {{PLURALVAR}}!;
    }

    public async Task<Result<{{SINGULAR}}ResDto, NotFound>> Get{{SINGULAR}}Async(int id)
    {
        var {{SINGULARVAR}}Res = await _client.GetAsync($"{Urls.ServerUrl}/{{PLURALVAR}}/{id}");
        if (!{{SINGULARVAR}}Res.IsSuccessStatusCode) { return new NotFound();}
        var {{SINGULARVAR}}ResDto = await {{SINGULARVAR}}Res.Content.ReadFromJsonAsync<{{SINGULAR}}ResDto>();
        Assertions.Assert({{SINGULARVAR}}ResDto is not null, "{{SINGULAR}} response dto mut not be null");
        return {{SINGULARVAR}}ResDto!;
    }

    public async Task<Result<{{SINGULAR}}ResDto, BadRequest>> Create{{SINGULAR}}Async({{SINGULAR}}ReqDto reqDto)
    {
        var {{SINGULARVAR}}Res = await _client.PostAsJsonAsync($"{Urls.ServerUrl}/{{PLURALVAR}}", reqDto);
        if (!{{SINGULARVAR}}Res.IsSuccessStatusCode) { return new BadRequest();}
        var {{SINGULARVAR}}ResDto = await {{SINGULARVAR}}Res.Content.ReadFromJsonAsync<{{SINGULAR}}ResDto>();
        Assertions.Assert({{SINGULARVAR}}ResDto is not null, "{{SINGULAR}} response dto mut not be null");
        return {{SINGULARVAR}}ResDto!;
    }

    public async Task<Result<{{SINGULAR}}ResDto, NotFound>> Set{{SINGULAR}}Async(int id, {{SINGULAR}}ReqDto reqDto)
    {
        var {{SINGULARVAR}}Res = await _client.PutAsJsonAsync($"{Urls.ServerUrl}/{{PLURALVAR}}/{id}", reqDto);
        if (!{{SINGULARVAR}}Res.IsSuccessStatusCode) { return new NotFound();}
        var {{SINGULARVAR}}ResDto = await {{SINGULARVAR}}Res.Content.ReadFromJsonAsync<{{SINGULAR}}ResDto>();
        Assertions.Assert({{SINGULARVAR}}ResDto is not null, "{{SINGULAR}} response dto mut not be null");
        return {{SINGULARVAR}}ResDto!;
    }

    public async Task<Option<NotFound>> Delete{{SINGULAR}}Async(int id)
    {
        var deletionRes = await _client.DeleteAsync($"{Urls.ServerUrl}/{{PLURALVAR}}/{id}");
        if (!deletionRes.IsSuccessStatusCode) { return new NotFound();}
        return Option<NotFound>.NoneValue;
    }
}