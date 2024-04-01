// Serviços como o contexto de BD precisam ser registrados no contêiner de DI (injeção de dependência).
// Ou seja, se utiliza o contexto e a classe para registrar o BD. 
// O contêiner fornece o serviço aos controladores.

// Adiciona diretivas using
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Adiciona o contexto de banco de dados ao contêiner de DI.
// Especifica que o contexto de banco de dados usará um banco de dados em memória.
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
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
