using System.ComponentModel.DataAnnotations;

namespace PlanIt.Presentation.WebApp.Models;
public class PlanModel : IValidatableObject
{
    [DataType(DataType.Date)]
    public DateTime DateFrom { get; set; }
    [DataType(DataType.Date)]
    public DateTime DateTo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateFrom > DateTo)
        {
            yield return new ValidationResult("The selected date interval is not valid.");
        }
    }
}
