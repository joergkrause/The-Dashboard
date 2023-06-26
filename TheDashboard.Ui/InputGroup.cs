using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace TheDashboard.Ui;

public sealed class InputGroup : InputBase<float>
{
  protected override void BuildRenderTree(RenderTreeBuilder builder)
  {

    // deliver settings through AdditionalAttributes
    var leftText = AdditionalAttributes!.ContainsKey("group-start") ? AdditionalAttributes?.SingleOrDefault(a => a.Key == "group-start").Value.ToString() : null;
    var rightText = AdditionalAttributes!.ContainsKey("group-end") ? AdditionalAttributes?.SingleOrDefault(a => a.Key == "group-end").Value.ToString() : null;

    builder.OpenElement(0, "div");
    builder.AddAttribute(1, "class", "input-group");
    if (leftText != null)
    {
      builder.OpenElement(2, "span");
      builder.AddAttribute(3, "class", "input-group-text");
      builder.AddContent(4, "");
      builder.CloseElement();
    }
    builder.OpenElement(5, "input");
    builder.AddMultipleAttributes(6, AdditionalAttributes);
    builder.AddAttribute(7, "class", CssClass);
    builder.AddAttribute(8, "value", BindConverter.FormatValue(CurrentValueAsString));
    builder.AddAttribute(9, "onchange", EventCallback.Factory.CreateBinder<float>(this, value => CurrentValue = value, CurrentValue, culture: null));
    builder.CloseElement();
    if (rightText != null)
    {
      builder.OpenElement(10, "span");
      builder.AddAttribute(11, "class", "input-group-text");
      builder.AddContent(12, "°C");
      builder.CloseElement();
    }
    builder.CloseElement(); // close the select element
  }

  protected override bool TryParseValueFromString(string? value, out float result, out string validationErrorMessage)
  {
    // Let's Blazor convert the value for us 😊
    if (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out float parsedValue))
    {
      result = parsedValue;
      validationErrorMessage = "";
      return true;
    }

    // Map null/empty value to null if the bound object is nullable
    if (string.IsNullOrEmpty(value))
    {
      var nullableType = Nullable.GetUnderlyingType(typeof(float));
      if (nullableType != null)
      {
        result = default;
        validationErrorMessage = "";
        return true;
      }
    }

    // The value is invalid => set the error message
    result = default;
    validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";
    return false;
  }

  // Get the display text for an enum value:
  // - Use the DisplayAttribute if set on the enum member, so this support localization
  // - Fallback on Humanizer to decamelize the enum member name
  private static string GetDisplayName(float value)
  {
    // Read the Display attribute name
    var member = value.GetType().GetMember(value.ToString())[0];
    var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
    if (displayAttribute != null)
      return displayAttribute.GetName();

    return value.ToString();
  }
}