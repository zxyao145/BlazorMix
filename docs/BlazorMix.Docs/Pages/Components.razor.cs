using BlazorMix.Docs.Core.Services;
using BlazorMix.Docs.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace BlazorMix.Docs.Pages;

public partial class Components: IDisposable
{
    [Inject]
    public DocService DocService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public string ComponentName { get; set; } = "";


    [Parameter]
    public string Language { get; set; } = "zh-cn";

    [CascadingParameter(Name = "TocRefWrapper")]
    public EleRefWrapper TocRefWrapper { get; set; } = default!;


    private string _content = "loading...";

    private ElementReference _articleRef;

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (string.IsNullOrWhiteSpace(ComponentName))
        {
            ComponentName = "Button";
        }

        var url = $"pages/Components/{ComponentName}/Docs/index.{Language}.html";
        _content = await DocService.ReadMdDocsAsync(url);
        StateHasChanged();
    }

    private bool secondRender = true;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            if(secondRender) 
            {
                secondRender = false;
                await JSRuntime.InvokeVoidAsync("Mix.Docs.renderToc", _articleRef, TocRefWrapper!.EleRef);
            }
        }
        await ScrollToFragment();
    }



    private async void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        await ScrollToFragment();
    }

    private async Task ScrollToFragment()
    {
        var uri = new Uri(NavigationManager.Uri, UriKind.Absolute);
        var fragment = uri.Fragment;
        if (fragment.StartsWith('#'))
        {
            // Handle text fragment (https://example.org/#test:~:text=foo)
            // https://github.com/WICG/scroll-to-text-fragment/
            var elementId = fragment.Substring(1);
            var index = elementId.IndexOf(":~:", StringComparison.Ordinal);
            if (index > 0)
            {
                elementId = elementId.Substring(0, index);
            }

            if (!string.IsNullOrEmpty(elementId))
            {
                await JSRuntime.InvokeVoidAsync("Mix.Docs.blazorScrollToId", elementId);
            }
        }
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}
