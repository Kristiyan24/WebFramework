using WebFramework.MVC.Identity;

namespace WebApplication.Models;

public class User : IdentityUser<string>
{
    public User()
    {
        this.Id = Guid.NewGuid().ToString();
        Submissions = new HashSet<Submission>();
    }
    public virtual ICollection<Submission> Submissions { get; set; }
}
