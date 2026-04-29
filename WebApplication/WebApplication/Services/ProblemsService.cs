using WebApplication.Models;

namespace WebApplication.Services;

public class ProblemsService : IProblemsService
{
    private readonly ApplicationDbContext db;

    public ProblemsService(ApplicationDbContext db)
    {
        this.db = db;
    }

    public void CreateProblem(string name, int points)
    {
        var problem = new Problem
        {
            Name = name,
            Points = points,
        };
        db.Problems.Add(problem);
        db.SaveChanges();
    }
}
