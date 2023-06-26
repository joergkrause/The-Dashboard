using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDashboard.Ui.Attributes;

public class CheckBoxRequiredAttribute : ValidationAttribute
{

  public override bool IsValid(object? value)
  {
    if (value == null) return false;
    if (value is bool boolValue) return boolValue;
    return false;
  }

}
