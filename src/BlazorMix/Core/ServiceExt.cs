﻿using Microsoft.Extensions.DependencyInjection;

namespace BlazorMix;

public static class ServiceExt
{
    public static IServiceCollection AddBlazorMix
        (this IServiceCollection services)
    {
        services.AddScoped<ActionSheetService>();
        services.AddScoped<ToastService>();
        services.AddScoped<DialogService>();
        return services;
    }
}
