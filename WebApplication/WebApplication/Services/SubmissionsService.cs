using WebApplication.Models;

namespace WebApplication.Services;

public class SubmissionsService : ISubmissionsService
{
    private readonly ApplicationDbContext db;
    private readonly Random random;

    public SubmissionsService(ApplicationDbContext db, Random random)
    {
        this.db = db;
        this.random = random;
    }

    public void Create(string userId, string problemId, string code)
    {
        var problem = db.Problems.FirstOrDefault(x => x.Id == problemId);
        var submission = new Submission()
        {
            CreatedOn = DateTime.UtcNow,
            UserId = userId,
            ProblemId = problemId,
            Code = code,
            AchievedResult = random.Next(0, problem.Points + 1),
        };

        db.Submissions.Add(submission);
        db.SaveChanges();
    }

    public void Delete(string id)
    {
        var submission = db.Submissions.Find(id);
        db.Remove(submission);
        db.SaveChanges();
    }
}
