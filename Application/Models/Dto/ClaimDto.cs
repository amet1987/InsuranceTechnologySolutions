using Shared.Classes;

namespace Application.Models.Dto;

public class ClaimDto
{
    public string Id { get; set; }
    public string CoverId { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public ClaimType Type { get; set; }
    public decimal DamageCost { get; set; }
}
