using Src.Shared.ClassLib.Data.Dtos.Request;
using Src.Shared.ClassLib.Data.Dtos.Response;
using Src.Shared.ClassLib.HttpErrors;
using YaMonads;

namespace Src.Shared.ClassLib.ServicesInterfaces;

public interface I{{PLURAL}}Service
{
    /// <summary>
    /// Gets all {{PLURALVAR}} form the database
    /// </summary>
    /// <returns>The {{PLURALVAR}}' response dto enumerable</returns>
    Task<IEnumerable<{{SINGULAR}}ResDto>> GetAll{{PLURAL}}Async();

    /// <summary>
    /// Gets a {{SINGULARVAR}} by id from the database
    /// </summary>
    /// <param name="id">The id of the {{SINGULARVAR}}</param>
    /// <returns>The {{SINGULARVAR}} response dto or <see cref="NotFound"/> if the {{SINGULARVAR}} was not found</returns>
    Task<Result<{{SINGULAR}}ResDto, NotFound>> Get{{SINGULAR}}Async(int id);

    /// <summary>
    /// Creates a {{SINGULARVAR}} in the database
    /// </summary>
    /// <param name="reqDto">The {{SINGULARVAR}} request dto</param>
    /// <returns>The created {{SINGULARVAR}} response dto or <see cref="BadRequest"/>
    /// if the <see cref="reqDto"/> provided is invalid</returns>
    Task<Result<{{SINGULAR}}ResDto, BadRequest>> Create{{SINGULAR}}Async({{SINGULAR}}ReqDto reqDto);

    /// <summary>
    /// Sets a {{SINGULARVAR}} in the database with an idempotent operation
    /// </summary>
    /// <param name="id">Id of the {{SINGULARVAR}}</param>
    /// <param name="reqDto">The {{SINGULARVAR}} request dto or <see cref="NotFound"/>
    ///     if the {{SINGULARVAR}} was not found</param>
    /// <returns></returns>
    Task<Result<{{SINGULAR}}ResDto, NotFound>> Set{{SINGULAR}}Async(int id, {{SINGULAR}}ReqDto reqDto);

    /// <summary>
    /// Deletes a {{SINGULARVAR}} from the database
    /// </summary>
    /// <param name="id">The id of the {{SINGULARVAR}} to delete</param>
    /// <returns>If the {{SINGULARVAR}} was not found, then <see cref="NotFound"/></returns>
    Task<Option<NotFound>> Delete{{SINGULAR}}Async(int id);
}