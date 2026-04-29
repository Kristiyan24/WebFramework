using WebFramework.MVC;
using WebFramework.MVC.Attributes;
using WebFramework.HTTP.Abstractions;
using WebApplication.ViewModels.Submissions;
using WebApplication.Services;

namespace WebApplication.Controllers;

public class SubmissionsController : Controller
{
    private readonly ApplicationDbContext db;
    private readonly ISubmissionsService submissionsService;

    public SubmissionsController(ApplicationDbContext db, ISubmissionsService submissionsService)
    {
        this.db = db;
        this.submissionsService = submissionsService;
    }

    public HttpResponse Create(string id)
    {
        if (!this.IsUserLoggedIn()) {
            return this.Redirect("/Users/Login");
        }

        var problem = db.Problems
            .Where(x => x.Id == id)
            .Select(x => new CreateFormViewModel
            {
                Name = x.Name,
                ProblemId = x.Id,
            }).FirstOrDefault();
        if (problem == null)
        {
            return this.Error("Problem not found!");
        }

        return this.View(problem);
    }

    [HttpPost]
    public HttpResponse Create(string problemId, string code)
    {
        if (!this.IsUserLoggedIn())
        {
            return this.Redirect("/Users/Login");
        }

        if (code == null || code.Length < 30)
        {
            return this.Error("Please provide code with at least 30 characters.");
        }

        submissionsService.Create(this.User, problemId, code);

        return Redirect("/");
    }

    public HttpResponse Delete(string id)
    {
        if (!this.IsUserLoggedIn())
        {
            return this.Redirect("/Users/Login");
        }

        submissionsService.Delete(id);

        return this.Redirect("/");
    }
}
