using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace TheDashboard.Ui;

public sealed class InputBigText : InputBase<string>
{
  protected override void BuildRenderTree(RenderTreeBuilder builder)
  {

    builder.OpenElement(1, "textarea");
    builder.AddMultipleAttributes(2, AdditionalAttributes);
    builder.AddAttribute(3, "rows", "4");
    builder.AddAttribute(4, "class", CssClass);
    builder.AddAttribute(5, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValue = value, CurrentValue ?? string.Empty, culture: null));
    builder.AddContent(6, BindConverter.FormatValue(CurrentValueAsString));
    builder.CloseElement();
  }

  protected override bool TryParseValueFromString(string? value, out string result, out string validationErrorMessage)
  {
    // Let's Blazor convert the value for us 😊
    if (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out string parsedValue))
    {
      result = parsedValue;
      validationErrorMessage = "";
      return true;
    }

    // Map null/empty value to null if the bound object is nullable
    if (string.IsNullOrEmpty(value))
    {
      var nullableType = Nullable.GetUnderlyingType(typeof(bool));
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
  private static string GetDisplayName(string value)
  {
    // Read the Display attribute name
    var member = value.GetType().GetMember(value.ToString())[0];
    var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
    if (displayAttribute != null)
      return displayAttribute.GetName();

    return value.ToString();
  }
}