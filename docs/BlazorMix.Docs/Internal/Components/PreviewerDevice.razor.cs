using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QRCoder;
using System;

namespace BlazorMix.Docs.Internal.Components;

public partial class PreviewerDevice
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IJSRuntime JsRuntime { get; set; } = default!;

    /// <summary>
    /// Demos/Components/Button/Demos/Demo1
    /// </summary>
    [Parameter, EditorRequired]
    public string Src { get; set; } = null!;

    private string _iframeSrc { get; set; } = "";

    //public Type? _type { get; set; }
    private string _qrCodeBase64 = "";
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        var srcInfo = Src.Split("/");
        if (srcInfo.Length == 5)
        {
            _iframeSrc = $"{NavigationManager.BaseUri}components-preview/{srcInfo[2]}/{srcInfo[4]}";
            _qrCodeBase64 = GetQrCodeBase64(_iframeSrc);
        }
        //var executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        //var typeInfo = Src.Replace("/", ".");
        //if (typeInfo.StartsWith("."))
        //{
        //    typeInfo = typeInfo.Substring(1);
        //}
        //_type = Type.GetType($"{executingAssemblyName}.{typeInfo}");
        StateHasChanged();
    }

    private async Task OnShareClick()
    {
        await JsRuntime.InvokeVoidAsync("open", new string[2] { _iframeSrc, "_blank" });
    }

    private string _display = "none";
    private void QrCodeClick()
    {
        if (_display == "none")
        {
            _display = "block";
        }
        else
        {
            _display = "none";
        }
    }

    private static string GetQrCodeBase64(string txt)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(txt, QRCodeGenerator.ECCLevel.Q);
        
        var pngByteQRCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeAsPngByteArr = pngByteQRCode.GetGraphic(20);
        var imgStr = Convert.ToBase64String(qrCodeAsPngByteArr);
        return imgStr;
    }
}
