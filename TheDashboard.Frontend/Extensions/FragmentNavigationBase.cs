using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace TheDashboard.Frontend.Extensions;

public class FragmentNavigationBase : ComponentBase, IDisposable
{
  [Inject] NavigationManager NavManager { get; set; } = default!;
  [Inject] IJSRuntime JsRuntime { get; set; } = default!;

  protected override void OnInitialized()
  {
    NavManager.LocationChanged += TryFragmentNavigation;
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    await NavManager.NavigateToFragmentAsync(JsRuntime);
  }

  private async void TryFragmentNavigation(object sender, LocationChangedEventArgs args)
  {
    await NavManager.NavigateToFragmentAsync(JsRuntime);
  }

  public void Dispose()
  {
    NavManager.LocationChanged -= TryFragmentNavigation;
  }
}
