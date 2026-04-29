using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Problem> Problems { get; set; }
    public DbSet<Submission> Submissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=WebApplicationDatabase;Integrated Security=True;");
    }
}
