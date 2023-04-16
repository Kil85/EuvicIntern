using EuvicIntern;
using EuvicIntern.Entities;
using EuvicIntern.Middleware;
using EuvicIntern.Models;
using EuvicIntern.Models.Validators;
using EuvicIntern.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<EuvicDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
);

builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<AccountSeeder>();

builder.Host.UseNLog();

// Configure the HTTP request pipeline.

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<AccountSeeder>();

seeder.Seeder();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Registration Task");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
