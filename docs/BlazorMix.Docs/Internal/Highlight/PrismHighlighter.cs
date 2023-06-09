﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorMix.Docs.Internal.Components
{
    public class PrismHighlighter : IPrismHighlighter
    {
        private readonly IJSRuntime jsRuntime;

        public PrismHighlighter(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async ValueTask<MarkupString> HighlightAsync(string code, string language)
        {
            string highlighted = await jsRuntime.InvokeAsync<string>("Mix.Prism.highlight", code, language);
            return new MarkupString(highlighted);
        }

        public async Task HighlightAllAsync()
        {
            await jsRuntime.InvokeVoidAsync("Mix.Prism.highlightAll");
        }
    }
}