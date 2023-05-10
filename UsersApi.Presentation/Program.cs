using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using UsersApi.Application.Handlers;
using UsersApi.Application.Repositories;
using UsersApi.Application.Services.Authentication;
using UsersApi.Application.Services.Helpers;
using UsersApi.Application.Services.UsersOperations;
using UsersApi.Infrastructure.Data;
using UsersApi.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAuthenticateService, AuthenticateService>();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<IUserAddChecker, UserAddChecker>();
builder.Services.AddTransient<IPasswordHashService, PasswordHashService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();