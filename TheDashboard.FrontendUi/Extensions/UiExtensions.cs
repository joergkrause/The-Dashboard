using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace TheDashboard.FrontendUi.Extensions;

public static class UiExtensions
{
  public static ValueTask NavigateToFragmentAsync(this NavigationManager navigationManager, IJSRuntime jSRuntime)
  {
    var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

    if (uri.Fragment.Length == 0)
      return default;

    return jSRuntime.InvokeVoidAsync("blazorHelpers.scrollToFragment", uri.Fragment.Substring(1));
  }

  public static string ToGerman(this bool value)
  {
    return value ? "Ja" : "Nein";
  }

  public static string ToLocalGermanDate(this DateTime date, bool @short = true)
  {
    var germany = GetAgnosticTimeZone();
    var utcDate = date.ToUniversalTime();
    DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
    var localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, germany);
    return @short ? localDate.ToShortDateString() : localDate.ToLongDateString();
  }

  public static string ToLocalGermanTime(this DateTime date, bool @short = true)
  {
    var germany = GetAgnosticTimeZone();
    var utcDate = date.ToUniversalTime();
    DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
    var localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, germany);
    return @short ? localDate.ToShortTimeString() : localDate.ToLongTimeString();
  }

  public static string ToLocalGermanDateTime(this DateTime utcDate, bool @short = true)
  {
    return $"{utcDate.ToLocalGermanDate(@short)} {utcDate.ToLocalGermanTime(@short)}";
  }

  private static TimeZoneInfo GetAgnosticTimeZone()
  {
    TimeZoneInfo tz = TimeZoneInfo.Local;
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      tz = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
    }
    if (RuntimeInformation.IsOSPlatform(@OSPlatform.Linux))
    {
      tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
    }
    return tz;
  }

}
