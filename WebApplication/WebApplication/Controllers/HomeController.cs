using WebFramework.MVC;
using WebFramework.MVC.Attributes;
using WebFramework.HTTP.Logging;
using WebFramework.HTTP.Abstractions;
using WebApplication.ViewModels.Home;

namespace WebApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger logger;
    private readonly ApplicationDbContext db;

    public HomeController(ILogger logger, ApplicationDbContext db)
    {
        this.db = db;
        this.logger = logger;
    }

    [HttpGet("/")]
    public HttpResponse Index()
    {
        if (this.IsUserLoggedIn())
        {
            var problems = db.Problems.Select(x => new IndexProblemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Count = x.Submissions.Count(),
            }).ToList();

            var loggedInViewModel = new LoggedInViewModel()
            {
                Problems = problems,
            };

            return this.View(loggedInViewModel, "IndexLoggedIn");
        }

        logger.Log("Hello from Index");
        var viewModel = new IndexViewModel
        {
            Message = "Welcome to WebApplication Platform!",
            Year = DateTime.UtcNow.Year,
        };
        return this.View(viewModel);
    }
}