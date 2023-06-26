using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace TheDashboard.Ui;

public class InputDateTimeNullable : InputBase<DateTime?>
{

  private string DateFormat = "yyyy-MM-dd";


  [Inject]
  public IJSRuntime JSRuntime { get; set; }

  /// <inheritdoc />
  protected override void BuildRenderTree(RenderTreeBuilder builder)
  {
    builder.OpenElement(0, "input");
    builder.AddMultipleAttributes(1, AdditionalAttributes);
    builder.AddAttribute(2, "type", "date");
    builder.AddAttribute(3, "class", CssClass);
    builder.AddAttribute(4, "value", BindConverter.FormatValue(CurrentValueAsString));
    builder.AddAttribute(5, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
    if (GetHint() == "Calendar")
    {
      builder.AddAttribute(6, "type", "date");
    }
    else
    {
      var hasTime = AdditionalAttributes["type"] == "datetime-local";
      if (hasTime)
      {
        DateFormat = "dd.MM.yyyy HH:mm";
      }
      else
      {
        DateFormat = "dd.MM.yyyy";
      }
      builder.AddAttribute(6, "type", "text");
      builder.AddAttribute(7, "placeholder", hasTime ? "TT.MM.JJJJ HH:mm" : "TT.MM.JJJJ");
      builder.AddAttribute(8, "data-mask", hasTime ? "00.00.0000 00:00" : "00.00.0000");
    }
    builder.CloseElement();
  }

  /// <inheritdoc />
  protected override string FormatValueAsString(DateTime? value)
  {
    if (value == null)
    {
      return "";
    }
    return BindConverter.FormatValue(value.GetValueOrDefault(), DateFormat, CultureInfo.InvariantCulture);
  }

  /// <inheritdoc />
  protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
  {

    bool success = TryParseDateTime(value, out DateTime? innerResult);
    result = innerResult.GetValueOrDefault();

    if (success)
    {
      validationErrorMessage = null;
      return true;
    }
    else
    {
      var attr = FieldIdentifier.Model.GetType().GetProperty(FieldIdentifier.FieldName)!.GetCustomAttributes(typeof(DataTypeAttribute), true).OfType<DataTypeAttribute>().SingleOrDefault();
      validationErrorMessage = attr != null ? attr.ErrorMessage ?? $"Das Feld {FieldIdentifier.FieldName} ist ungültig" : "Die Datumsangabe ist ungültig";
      return false;
    }
  }

  private bool TryParseDateTime(string value, out DateTime? result)
  {
    var success = BindConverter.TryConvertToDateTime(value, CultureInfo.InvariantCulture, DateFormat, out var parsedValue);
    if (success)
    {
      result = (DateTime)(object)parsedValue;
      return true;
    }
    else
    {
      success = DateTime.TryParse(value, out var resultLoc);
      result = resultLoc;
      if (success) return true;
      var normalized = value.Replace(".", "");
      success = DateTime.TryParseExact(normalized, "yyyyMMdd", null, DateTimeStyles.None, out resultLoc);
      result = resultLoc;
      if (success) return true;
    }
    return false;
  }


  protected async override Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender && GetHint() != "Calendar")
    {
      var id = AdditionalAttributes["id"];
      var hasTime = AdditionalAttributes["type"] == "datetime-local";
      if (hasTime)
      {
        await JSRuntime.InvokeVoidAsync("mask", id, "00.00.0000 00:00", false, false, DotNetObjectReference.Create(this));
      }
      else
      {
        await JSRuntime.InvokeVoidAsync("mask", id, "00.00.0000", false, false, DotNetObjectReference.Create(this));
      }
    }
  }

  private string GetHint()
  {
    return FieldIdentifier.Model
      .GetType()
      .GetProperty(FieldIdentifier.FieldName)!
      .GetCustomAttributes(typeof(UIHintAttribute), true)
      .OfType<UIHintAttribute>()
      .SingleOrDefault()?
      .UIHint ?? string.Empty;
  }

  [JSInvokable]
  public void returnCurrentValue(string value)
  {
    CurrentValueAsString = value;
  }


}

