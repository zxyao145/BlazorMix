using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorMix.Docs.Internal.Components
{
    public interface IPrismHighlighter
    {
        ValueTask<MarkupString> HighlightAsync(string code, string language);

        Task HighlightAllAsync();
    }
}