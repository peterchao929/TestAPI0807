using Microsoft.EntityFrameworkCore;
using TestAPI0807.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

//設定連線、SQL版本
//string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

string connectionString = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
var serverVersion = ServerVersion.AutoDetect(connectionString);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<UserDataContext>(opt =>
   opt.UseMySql(connectionString, serverVersion)
   );

//原生TodoContext連線、使用內存資料庫
//builder.Services.AddDbContext<TodoContext>(opt =>
//opt.UseInMemoryDatabase("TodoList"));
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
