using Data.DbContexts;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using OneOf.Monads;
using Shared.Data.Dtos;
using Shared.Data.Mappers;
using Shared.Data.Models;
using Shared.Exceptions;
using Shared.ServicesInterfaces;

namespace Server.Services;

public class {{PLURAL}}Service : I{{PLURAL}}Service
{
    private readonly {{PLURAL}}Context _context;
    private readonly {{SINGULAR}}Mapper _mapper;

    public {{PLURAL}}Service({{PLURAL}}Context context, {{SINGULAR}}Mapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<Exception, List<{{SINGULAR}}>Exception>> Get{{PLURAL}}Async()
    {
        try
        {
            return await _context.{{PLURAL}}.ToListAsync();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<Exception, {{SINGULAR}}Exception>> Get{{SINGULAR}}Async(Guid id)
    {
        try
        {
            var {{SINGULARVAR}} = await _context.{{PLURAL}}.FirstOrDefaultAsync({{SINGULARVAR}} => {{SINGULARVAR}}.Id == id);
            
            if ({{SINGULARVAR}} == null)
            {
                return new NotFoundException();
            }

            return {{SINGULARVAR}};
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<Exception, {{SINGULAR}}Exception>> Create{{SINGULAR}}Async({{SINGULAR}}Dto dto)
    {
        try
        {
            var {{SINGULARVAR}} = _mapper.MapToModel(dto);
            await _context.AddAsync({{SINGULARVAR}});
            await _context.SaveChangesAsync();
            return {{SINGULARVAR}};
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<Exception, {{SINGULAR}}Exception>> Update{{SINGULAR}}Async(Guid id, {{SINGULAR}}Dto dto)
    {
        try
        {
            var {{SINGULARVAR}} = await _context.{{PLURAL}}.FirstOrDefaultAsync({{SINGULARVAR}} => {{SINGULARVAR}}.Id == id);

            if ({{SINGULARVAR}} == null)
            {
                return new NotFoundException();
            }
            
            // TODO
            var new{{SINGULAR}} = _mapper.MapToModel(dto);
            new{{SINGULAR}}.FirstName = dto.TrimmedFirstName!;
            new{{SINGULAR}}.LastName = dto.TrimmedLastName!;
            
            await _context.SaveChangesAsync();
            
            return {{SINGULARVAR}};
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<Exception, UnitException>> Delete{{SINGULAR}}Async(Guid id)
    {
        try
        {
            var {{SINGULARVAR}} = await _context.{{PLURAL}}.FirstOrDefaultAsync({{SINGULARVAR}} => {{SINGULARVAR}}.Id == id);

            if ({{SINGULARVAR}} == null)
            {
                return new NotFoundException();
            }
            
            _context.Remove({{SINGULARVAR}});
            await _context.SaveChangesAsync();
            
            return Unit.Default;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}