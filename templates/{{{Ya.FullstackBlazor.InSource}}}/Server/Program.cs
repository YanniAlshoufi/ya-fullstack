using Data.DbContexts;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shared.Common;
using Shared.Data.Mappers;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddDbContext<UsersContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlite(connectionString);
    });
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "{{{Ya.FullstackBlazor.InSource.restApiName}}}", Version = "v1" });
    });
    
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

    builder.Services.AddScoped<UserMapper>();
}

var app = builder.Build();
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
    app.Run();
}