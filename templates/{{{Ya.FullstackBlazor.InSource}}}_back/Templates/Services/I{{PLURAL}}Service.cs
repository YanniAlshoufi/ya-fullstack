using LanguageExt;
using OneOf.Monads;
using Shared.Data.Dtos;
using Shared.Data.Models;

namespace Shared.ServicesInterfaces;

public interface I{{PLURAL}}Service
{
    Task<Result<Exception, List<{{SINGULAR}}>>> Get{{PLURAL}}Async();
    Task<Result<Exception, {{SINGULAR}}>> Get{{SINGULAR}}Async(Guid id);
    Task<Result<Exception, {{SINGULAR}}>> Create{{SINGULAR}}Async({{DTO}} dto);
    Task<Result<Exception, {{SINGULAR}}>> Update{{SINGULAR}}Async(Guid id, {{DTO}} dto);
    Task<Result<Exception, Unit>> Delete{{SINGULAR}}Async(Guid id);
}