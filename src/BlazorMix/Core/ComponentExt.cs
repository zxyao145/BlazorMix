namespace BlazorMix;

public static class ComponentExt
{
    public static EventCallback CreateEventCallback
    (
        this object obj,
        EventCallback action
    )
    {
        return EventCallback.Factory.Create(obj, action);
    }

    public static EventCallback CreateEventCallback
    (
        this object obj,
        Func<Task> func
    )
    {
        return EventCallback.Factory.Create(obj, func);
    }

    public static EventCallback<T> CreateEventCallback<T>
        (
            this object obj,
            Action<T> action
        )
    {
        return EventCallback.Factory.Create(obj, action);
    }

    public static EventCallback<T> CreateEventCallback<T>
    (
        this object obj,
        Func<T, Task> func
    )
    {
        return EventCallback.Factory.Create(obj, func);
    }


}
