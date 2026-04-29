namespace WebFramework.MVC.Abstractions;

public interface IView
{
    string GetHtml(object model, string user);
}
