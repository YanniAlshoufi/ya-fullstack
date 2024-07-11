using Data.DbContexts;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Server.Services;
using Shared.Common;
using Shared.Data.Mappers;
using Shared.ServicesInterfaces;

const string corsPolicyName = "AllowClient";  

var builder = WebApplication.CreateBuilder(args);
{
    #region Boilerplate
    var clientUrl = builder.Configuration["ClientUrl"];

    if (clientUrl is null)
    {
        throw new ApplicationException("The client url is not set in server project");
    }

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            name: corsPolicyName,
            policy =>
            {
                policy
                    .WithOrigins(clientUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
    
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
    #endregion
    
    #region Custom Services
    builder.Services.AddScoped<IUsersService, UsersService>();
    #endregion
}

var app = builder.Build();
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(corsPolicyName);
    app.MapControllers();
    app.Run();
}