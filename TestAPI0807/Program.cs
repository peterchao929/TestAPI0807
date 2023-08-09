using Microsoft.EntityFrameworkCore;
using TestAPI0807.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddDbContext<TodoContext>(opt =>
//opt.UseInMemoryDatabase("TodoList"));
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddControllers();
builder.Services.AddDbContext<UserDataContext>(opt =>
   opt.UseMySql(connectionString, serverVersion)
   );
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
