using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace TheDashboard.Ui;

// TODO: https://blazorfiddle.com/s/xnc9tubr
public sealed class InputCheck : InputBase<bool>
{
  protected override void BuildRenderTree(RenderTreeBuilder builder)
  {
    var uniqueId = $"_{Guid.NewGuid()}";
    builder.OpenElement(1, "div");
    builder.AddAttribute(2, "class", $"form-check {GetAdditionalClass()}");
    builder.OpenElement(3, "input");
    builder.AddMultipleAttributes(4, AdditionalAttributes);
    builder.AddAttribute(5, "type", "checkbox");
    builder.AddAttribute(6, "class", CssClass);
    builder.AddAttribute(7, "value", BindConverter.FormatValue(CurrentValueAsString));
    builder.AddAttribute(8, "onchange", EventCallback.Factory.CreateBinder<bool>(this, value => CurrentValue = value, CurrentValue, culture: null));
    builder.AddAttribute(9, "id", uniqueId);
    builder.CloseElement();
    builder.OpenElement(9, "label");
    builder.AddMultipleAttributes(11, new Dictionary<string, object> { { "class", "form-check-label w-75" }, { "for", uniqueId } });
    builder.AddContent(10, GetDisplayName());
    builder.CloseElement();
    builder.CloseElement();
  }

  protected override bool TryParseValueFromString(string? value, out bool result, out string validationErrorMessage)
  {
    // Let's Blazor convert the value for us 😊
    if (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out bool parsedValue))
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

  private string GetDisplayName()
  {
    return FieldIdentifier.Model
      .GetType()
      .GetProperty(FieldIdentifier.FieldName)!
      .GetCustomAttributes(typeof(DisplayAttribute), true)
      .OfType<DisplayAttribute>()
      .SingleOrDefault()?
      .Name ?? "";
  }

  private string GetAdditionalClass()
  {
    return FieldIdentifier.Model
      .GetType()
      .GetProperty(FieldIdentifier.FieldName)!
      .GetCustomAttributes(typeof(UIHintAttribute), true)
      .OfType<UIHintAttribute>()
      .SingleOrDefault()?
      .UIHint ?? string.Empty;
  }

}