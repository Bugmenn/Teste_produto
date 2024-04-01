// É o arquivo de contexto de banco de dados, sendo a classe principal que coordena
// a funcionalidade do Entity Framework para um modelo de dados.
// Essa classe é criada derivando-a da classe Microsoft.EntityFrameworkCore.DbContext

using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}