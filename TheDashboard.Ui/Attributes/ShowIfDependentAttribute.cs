
using System.Linq.Expressions;

namespace TheDashboard.Ui.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ShowIfDependentAttribute : Attribute
{

  public ShowIfDependentAttribute(string checkProperty)
  {
    CheckProperty = checkProperty;
  }

  public string CheckProperty { get; set; }

  /// <summary>
  /// If true the checkproperty must be boolean and true
  /// </summary>
  public object CompareToValue { get; set; }

  /// <summary>
  /// Negate the comparision
  /// </summary>
  public bool ExcludeCompareValue { get; set; }

}