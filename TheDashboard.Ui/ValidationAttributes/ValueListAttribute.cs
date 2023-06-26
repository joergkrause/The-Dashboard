using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDashboard.Ui.ValidationAttributes;

public class ValueListAttribute : ValidationAttribute
{
  public ValueListAttribute(params string[] valueList)
  {
    ValueList = valueList;
  }


  public string[] ValueList { get; set; }

}
