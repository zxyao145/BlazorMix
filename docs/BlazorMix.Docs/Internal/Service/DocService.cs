using System.Collections.Concurrent;


namespace BlazorMix.Docs.Core.Services;

public class DocService
{
    private readonly static ConcurrentDictionary<string, ValueTask<string>> 
        demoSourceCache = new();

    private readonly static ConcurrentDictionary<string, ValueTask<string>>
            docsCache = new();

    private readonly HttpClient _httpClient;


    public DocService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("docsHttp");
    }

    public async Task<string> ReadExampleAsync(string src)
    {
        var key = "pages/";
        if (src.StartsWith("Demos/"))
        {
            key += src.Substring("Demos/".Length);
        }

        try
        {
            return await demoSourceCache.GetOrAdd(key,
                async _ => await _httpClient.GetStringAsync($"{key}.txt"));
        }
        catch (Exception e)
        {
            // TODO: log only in dev environment
            Console.WriteLine(e);
        }

        return string.Empty;
    }


    public async Task<string> ReadMdDocsAsync(string src)
    {
        var key = src;

        try
        {
            return await docsCache.GetOrAdd(key,
                async _ => await _httpClient.GetStringAsync($"{key}"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return string.Empty;
    }
}
