using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using TheDashboard.Ui.ValidationAttributes;

namespace TheDashboard.Ui;

public sealed class InputDropDown<TValueList> : InputBase<string>
{

  private string[] valuelistValues = new string[0];

  protected override void BuildRenderTree(RenderTreeBuilder builder)
  {
    builder.OpenElement(0, "select");
    builder.AddMultipleAttributes(1, AdditionalAttributes);
    builder.AddAttribute(2, "class", CssClass);
    builder.AddAttribute(3, "value", BindConverter.FormatValue(CurrentValueAsString));
    builder.AddAttribute(4, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString, culture: null));

    // Add an option element per enum value
    valuelistValues = GetValuelistValues(typeof(TValueList));

    var optCount = 5;
    foreach (string value in valuelistValues)
    {
      builder.OpenElement(optCount++, "option");
      builder.AddAttribute(optCount++, "value", value.ToString());
      builder.AddContent(optCount++, value);
      builder.CloseElement();
    }

    builder.CloseElement(); // close the select element
  }

  protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string result, [NotNullWhen(false)] out string? validationErrorMessage)
  {
    if (string.IsNullOrEmpty(value))
    {
      validationErrorMessage = "The value is required.";
      result = default;
      return false;
    }
    if (!valuelistValues.Contains(value))
    {
      validationErrorMessage = $"The value {value} is not in the list of allowed values.";
      result = value;
      return false;
    }
    result = value;
    validationErrorMessage = "";
    return true;
  }

  private static string[] GetValuelistValues(Type typeInfo)
  {
    var vlAttribute = typeInfo.GetCustomAttribute<ValueListAttribute>();
    return vlAttribute?.ValueList ?? Array.Empty<string>();
  }

}