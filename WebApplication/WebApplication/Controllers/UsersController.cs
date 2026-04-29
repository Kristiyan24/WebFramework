using System.Net.Mail;
using WebApplication.Services;
using WebApplication.ViewModels.Users;
using WebFramework.HTTP.Abstractions;
using WebFramework.HTTP.Logging;
using WebFramework.MVC;
using WebFramework.MVC.Attributes;

namespace WebApplication.Controllers;

public class UsersController : Controller
{
    private ILogger logger;
    private IUsersService usersService;

    public UsersController(IUsersService usersService, ILogger logger)
    {
        this.logger = logger;
        this.usersService = usersService;
    }

    public HttpResponse Login()
    {
        if (this.IsUserLoggedIn())
        {
            return this.Redirect("/");
        }

        return this.View();
    }

    [HttpPost]
    public HttpResponse Login(string username, string password)
    {
        var userId = usersService.GetUserId(username, password);
        if (userId == null)
        {
            return this.Redirect("/Users/Login");
        }

        this.SignIn(userId);
        logger.Log("User logged in: " + username);
        return this.Redirect("/");
    }

    public HttpResponse Register()
    {
        if (this.IsUserLoggedIn())
        {
            return this.Error("You are already registered.");
        }

        return this.View();
    }

    [HttpPost]
    public HttpResponse Register(RegisterInputModel input)
    {
        if (input.Password != input.ConfirmPassword)
        {
            return this.Error("Passwords should be the same!");
        }

        if (input.Username?.Length < 5 || input.Username?.Length > 20)
        {
            return this.Error("Username should be between 5 and 20 characters .");
        }

        if (input.Password?.Length < 6 || input.Password?.Length > 20)
        {
            return this.Error("Password should be between 6 and 20 characters.");
        }

        if (!IsValid(input.Email))
        {
            return this.Error("Invalid email!");
        }

        if (usersService.IsUsernameUsed(input.Username))
        {
            return this.Error("Username already used!");
        }

        if (usersService.IsEmailUsed(input.Email))
        {
            return this.Error("Email already used!");
        }

        usersService.CreateUser(input.Username, input.Email, input.Password);
        logger.Log("New user: " + input.Username);
        return this.Redirect("/Users/Login");
    }

    public HttpResponse Logout()
    {
        if (!this.IsUserLoggedIn())
        {
            return this.Redirect("/Users/Login");
        }

        this.SignOut();
        return this.Redirect("/");
    }

    private bool IsValid(string emailaddress)
    {
        try
        {
            new MailAddress(emailaddress);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
