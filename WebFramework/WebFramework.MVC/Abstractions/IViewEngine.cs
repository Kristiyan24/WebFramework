namespace WebFramework.MVC.Abstractions;

public interface IViewEngine
{
    string GetHtml(string templateHtml, object model, string user);
}