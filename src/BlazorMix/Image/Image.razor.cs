
using BlazorMix.Core;

namespace BlazorMix;

/// <summary>
/// 
/// </summary>
public partial class Image
{
    /// <summary>
    /// 
    /// </summary>
    public const string ClsPrefix = "bm-image";

    /// <summary>
    /// 图片地址	
    /// </summary>
    [Parameter, EditorRequired]
    public string Src { get; set; } = "";

    /// <summary>
    /// 图片描述	
    /// </summary>
    [Parameter]
    public string Alt { get; set; } = "";


    /// <summary>
    /// 图片宽度，如果传入数字则单位为 <code>px</code>
    /// </summary>
    [Parameter]
    public OneOf<int, string>? Width { get; set; }

    /// <summary>
    /// 图片高度，如果传入数字则单位为 <code>px</code>
    /// </summary>
    [Parameter]
    public OneOf<int, string>? Height { get; set; }

    /// <summary>
    /// 图片填充模式
    /// </summary>
    [Parameter]
    public ImageFit Fit { get; set; }

    /// <summary>
    /// 加载时的占位
    /// </summary>
    /// <default> ImageIcon </default>
    [Parameter]
    public StringOrRenderFragment Placeholder { get; set; } = ImageDefaultValues.DefaultPlaceholder;

    /// <summary>
    /// 加载失败的占位	
    /// </summary>
    /// <default> BrokenImageIcon </default>
    [Parameter]
    public StringOrRenderFragment Fallback { get; set; } = ImageDefaultValues.DefaultFallback;

    /// <summary>
    /// 是否懒加载图片	
    /// </summary>
    [Parameter]
    public bool Lazy { get; set; }

    //[Parameter]
    //public bool Draggable { get; set; }

    /// <summary>
    /// 图片点击事件。只有图片<b>加载成功</b>后，才可以被触发。
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// 加载失败时触发	
    /// </summary>
    [Parameter]
    public EventCallback OnError { get; set; }

    /// <summary>
    /// 图片加载完时触发
    /// </summary>
    [Parameter]
    public EventCallback OnLoad { get; set; }

    /// <summary>
    /// 容器点击事件
    /// </summary>
    [Parameter]
    public EventCallback OnContainerClick { get; set; }

    /// <summary>
    /// override
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (!Lazy && !_initialized)
        {
            _initialized = true;
        }

        _styleBuilder
            .Add(() =>
            {
                string width = "";
                if (Width != null)
                {
                    if (Width.Value.IsT0)
                    {
                        width = $"width: {Width.Value.AsT0}px; --bm-image-width:{Width.Value.AsT0}px;";
                    }
                    else
                    {
                        width = $"width: {Width.Value.AsT1}; --bm-image-width:{Width.Value.AsT1};";
                    }
                }

                return width;
            })
            .Add(() =>
            {
                string height = "";
                if (Height != null)
                {
                    if (Height.Value.IsT0)
                    {
                        height = $"height: {Height.Value.AsT0}px; --bm-image-height:{Height.Value.AsT0}px;";
                    }
                    else
                    {
                        height = $"height: {Height.Value.AsT1}; --bm-image-height:{Height.Value.AsT1};";
                    }
                }

                return height;
            })
            .Add(() => Style?.ToString() ?? "");
    }


    private ElementReference _root;
    private DotNetObjectReference<Image>? _obj;
    private bool _initialized = false;
    private bool _loaded = false;
    private bool _failed = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _obj = DotNetObjectReference.Create(this);
            if (!_initialized)
            {
                await JsRuntime.ObserveInViewportOnce(
                    CallbackFactory.Create(async () =>
                    {
                        this._initialized = true;
                        await InvokeStateHasChangedAsync();
                    }), 
                    _root);
                //await JsRuntime.InvokeVoidAsync(JsConstants.ObserveInViewportOnce,
                //    CallbackFactory.Create(async () => {
                //        this._initialized = true;
                //        await InvokeStateHasChangedAsync();
                //    }), _root);
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task HandleContainerClick()
    {
        if (OnContainerClick.HasDelegate)
        {
            await OnContainerClick.InvokeAsync();
        }
    }

    private async Task HandleImageClick()
    {
        if (_loaded && !_failed && OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }

    private async Task SetLoaded()
    {
        if (!this._loaded)
        {
            this._loaded = true;
            await InvokeStateHasChangedAsync();
        }
        
        if (OnLoad.HasDelegate)
        {
            await OnLoad.InvokeAsync();
        }
    }

    private async Task SetFailed()
    {
        if (!this._failed)
        {
            this._failed = true;
            await InvokeStateHasChangedAsync();
        }

        if (OnError.HasDelegate)
        {
            await OnError.InvokeAsync();
        }
    }

    protected override void Dispose(bool disposing)
    {
        _obj?.Dispose();
        base.Dispose(disposing);
    }
}
