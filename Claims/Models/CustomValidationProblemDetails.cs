using Microsoft.AspNetCore.Mvc;

namespace Claims.Models;

public class CustomValidationProblemDetails : ProblemDetails
{
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
}
