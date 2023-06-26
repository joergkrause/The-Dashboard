using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDashboard.ViewModels.ViewAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CrossReferenceAttribute : Attribute
{
  public CrossReferenceAttribute(Type referenceType, string referencePropertyName)
  {
    ReferenceType = referenceType;
    ReferencePropertyName = referencePropertyName;
  }


  public Type ReferenceType { get; set; }

  public string ReferencePropertyName { get; set; }

}
