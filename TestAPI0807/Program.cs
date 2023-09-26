using Microsoft.EntityFrameworkCore;
using TestAPI0807.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Configuration;
using TestAPI0807.Services;

var builder = WebApplication.CreateBuilder(args);

//�]�w�s�u�BSQL���� �U���t�@�س]�w�覡
//string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

string connectionString = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
var serverVersion = ServerVersion.AutoDetect(connectionString);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<UserDataContext>(opt =>
   opt.UseMySql(connectionString, serverVersion)
   );
builder.Services.AddScoped<TodoItemService, TodoItemServiceImpl>();
builder.Services.AddScoped<UserDataService, UserDataServiceImpl>();

//���TodoContext�s�u�B�ϥΤ��s��Ʈw
//builder.Services.AddDbContext<TodoContext>(opt =>
//opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ���\CORS
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 30~40��W�[�����A�U���o���٭n�O�oUse�~�|�ҥ�
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
