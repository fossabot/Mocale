using Microsoft.Extensions.Logging;
using Mocale.Abstractions;
using Mocale.Managers;
using Mocale.Models;

namespace Mocale;

/// <summary>
/// Host build extensions for Mocale
/// </summary>
public static class AppBuilderExtensions
{
    public static MauiAppBuilder UseMocale(
        this MauiAppBuilder mauiAppBuilder,
        Action<MocaleBuilder> builder = default)
    {
        var mocaleBuilder = new MocaleBuilder()
        {
            AppBuilder = mauiAppBuilder, // Give the builders a reference so they can register things
        };

        // Invoke mocaleConfiguration action
        builder?.Invoke(mocaleBuilder);

        // Default config if the consumer doesn't call WithConfiguration(...)
        mocaleBuilder.ConfigurationManager ??= new ConfigurationManager<IMocaleConfiguration>(new MocaleConfiguration());

        mauiAppBuilder.Services.AddSingleton<IConfigurationManager<IMocaleConfiguration>>(mocaleBuilder.ConfigurationManager);

        var serviceProvider = mauiAppBuilder.Services.BuildServiceProvider();

        var localizationManager = new LocalizationManager(
            mocaleBuilder.ConfigurationManager,
            serviceProvider.GetRequiredService<ILocalizationProvider>(),
            serviceProvider.GetRequiredService<ILogger<LocalizationManager>>());

        var t = localizationManager.InitializeAsync();
        t.Wait();

        mauiAppBuilder.Services.AddSingleton<ILocalizationManager>(localizationManager);

        return mauiAppBuilder;
    }
}
