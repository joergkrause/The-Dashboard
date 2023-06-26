using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDashboard.Ui.ValidationAttributes;

public class OccurenceValidationAttribute : ValidationAttribute
{

  public OccurenceValidationAttribute(int minOccurence, int maxOCccurence)
  {
    MinOccurence = minOccurence;
    MaxOCccurence = maxOCccurence;
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    var occurences = validationContext.ObjectInstance.GetType().GetProperties().Where(p => p.Name == validationContext.MemberName).Count();
    var check = occurences >= MinOccurence && occurences <= MaxOCccurence;
    return check ? ValidationResult.Success : new ValidationResult($"This element must exists between {MinOccurence} and {MaxOCccurence}");
  }

  public int MinOccurence { get; private set; }

  public int MaxOCccurence { get; private set; }

}
