using Microsoft.EntityFrameworkCore;
using Src.Data.ClassLib.DbContexts;
using Src.Shared.ClassLib.Data.Dtos.Request;
using Src.Shared.ClassLib.Data.Dtos.Response;
using Src.Shared.ClassLib.HttpErrors;
using Src.Shared.ClassLib.ServicesInterfaces;
using Src.Web.Mappers;
using YaMonads;

namespace Src.Web.Services;

public class {{PLURAL}}Service : I{{PLURAL}}Service
{
    private readonly {{PLURAL}}Context _context;
    private readonly {{SINGULAR}}Mapper _mapper;

    public {{PLURAL}}Service({{PLURAL}}Context context, {{SINGULAR}}Mapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<{{SINGULAR}}ResDto>> GetAll{{PLURAL}}Async()
    {
        var {{PLURALVAR}} = await _context.{{PLURAL}}.ToListAsync();
        return {{PLURALVAR}}.Select({{SINGULARVAR}} => _mapper.MapToResDto({{SINGULARVAR}}));
    }

    public async Task<Result<{{SINGULAR}}ResDto, NotFound>> Get{{SINGULAR}}Async(int id)
    {
        var {{SINGULARVAR}} = await _context.{{PLURAL}}.Where(x => x.Id == id).FirstOrDefaultAsync();
        
        if ({{SINGULARVAR}} is null)
        {
            return new NotFound();
        }
        
        return _mapper.MapToResDto({{SINGULARVAR}});
    }

    public async Task<Result<{{SINGULAR}}ResDto, BadRequest>> Create{{SINGULAR}}Async({{SINGULAR}}ReqDto reqDto)
    {
        var {{SINGULARVAR}} = _mapper.MapToModel(reqDto);
        await _context.AddAsync({{SINGULARVAR}});
        await _context.SaveChangesAsync();
        return _mapper.MapToResDto({{SINGULARVAR}});
    }

    public async Task<Result<{{SINGULAR}}ResDto, NotFound>> Set{{SINGULAR}}Async(int id, {{SINGULAR}}ReqDto reqDto)
    {
        var {{SINGULARVAR}} = await _context.{{PLURAL}}.FirstOrDefaultAsync(x => x.Id == id);

        if ({{SINGULARVAR}} is null)
        {
            return new NotFound();
        }

        
        UPDATE ALL HERE
        {{SINGULARVAR}}.Name = reqDto.Name.Trim();
        
        await _context.SaveChangesAsync();

        return _mapper.MapToResDto({{SINGULARVAR}});
    }

    public async Task<Option<NotFound>> Delete{{SINGULAR}}Async(int id)
    {
        var countDeleted = await _context.{{PLURAL}}.Where(x => x.Id == id).ExecuteDeleteAsync();
        
        if (countDeleted <= 0)
        {
            return new NotFound();
        }

        await _context.SaveChangesAsync();
        
        return Option<NotFound>.NoneValue;
    }
}