using Evergreen.Core.src.Abstraction;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.Service;
using Evergreen.Service.src.Shared;
using Evergreen.WebAPI.src.Database;
using Evergreen.WebAPI.src.Middleware;
using Evergreen.WebAPI.src.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//declare services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//add automapper dependency injection
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

builder.Services.AddTransient<ExceptionMiddleware>();

// Add database contect service

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql());

//add authentication

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();

//app.UseAuthorization();

app.MapControllers();

app.Run();
