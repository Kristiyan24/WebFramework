namespace WebApplication.ViewModels.Problems;

public class DetailsViewModel
{
    public string Name { get; set; }

    public IEnumerable<ProblemDetailsSubmissionViewModel> Problems { get; set; }
}
