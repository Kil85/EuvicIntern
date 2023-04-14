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

builder.Services.AddDbContext<EuvicDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));


builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Host.UseNLog();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Registration Task");
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
