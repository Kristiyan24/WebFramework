using WebFramework.MVC;

namespace WebApplication
{
    public static class Program
    {
        public static async Task Main()
        {
            await WebHost.StartAsync(new Startup(), 5000);
        }
    }
}
