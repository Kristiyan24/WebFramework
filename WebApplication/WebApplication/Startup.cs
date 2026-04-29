using WebApplication.Services;
using WebFramework.HTTP.Abstractions;
using WebFramework.MVC.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace WebApplication
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
            serviceCollection.Add<IProblemsService, ProblemsService>();
            serviceCollection.Add<ISubmissionsService, SubmissionsService>();
        }

        public void Configure(IList<Route> routeTable)
        {
            var db = new ApplicationDbContext();
            db.Database.Migrate();
        }
    }
}
