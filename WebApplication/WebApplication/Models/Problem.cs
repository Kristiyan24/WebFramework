using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models;

public class Problem
{
    public Problem()
    {
        Id = Guid.NewGuid().ToString();
        Submissions = new HashSet<Submission>();
    }

    public string Id { get; set; }

    [MaxLength(20)]
    [Required]
    public string Name { get; set; }

    public int Points { get; set; }

    public virtual ICollection<Submission> Submissions { get; set; }
}
