using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shared.Common;
using Src.Data.ClassLib.DbContexts;
using Src.Shared.ClassLib.ServicesInterfaces;
using Src.Web.Helpers;
using Src.Web.Services;
using UserMapper = Src.Web.Mappers.UserMapper;

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
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Testing Ya-Fullstack API", Version = "v1" });
    });
    
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

    builder.Services.AddScoped<UserMapper>();
    #endregion
    
    ServiceGeneration.AddServicesAutomatically(builder);
}

var app = builder.Build();
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(corsPolicyName);
    app.MapControllers();
    await app.RunAsync();
}