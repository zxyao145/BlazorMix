
namespace BlazorMix;
internal static class ValueTaskExt
{
    public static async ValueTask WhenAll(params ValueTask[] tasks)
    {
        ArgumentNullException.ThrowIfNull(tasks);
        if (tasks.Length == 0)
            return;

        List<Exception>? exceptions = null;

        foreach (var valueTask in tasks)
        {
            try
            {
                await valueTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exceptions ??= new List<Exception>(tasks.Length);
                exceptions.Add(ex);
            }
        }

        if (exceptions is not null)
        {
            throw new AggregateException(exceptions);
        }
    }


    public static async ValueTask<T[]> WhenAll<T>(params ValueTask<T>[] tasks)
    {
        ArgumentNullException.ThrowIfNull(tasks);
        if (tasks.Length == 0)
            return Array.Empty<T>();

        List<Exception>? exceptions = null;

        var results = new T[tasks.Length];
        for (var i = 0; i < tasks.Length; i++)
        {
            try
            {
                results[i] = await tasks[i].ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exceptions ??= new List<Exception>(tasks.Length);
                exceptions.Add(ex);
            }
        }

        return exceptions is null
            ? results
            : throw new AggregateException(exceptions);
    }
}
