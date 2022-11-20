using Microsoft.Extensions.Logging;
using Mocale.Abstractions;
using Mocale.Managers;
using Mocale.Models;

namespace Mocale;

public class MocaleBuilder
{
    public ILocalizationProvider LocalizationProvider { get; set; }

    internal MauiAppBuilder AppBuilder { get; set; }

    internal ConfigurationManager<IMocaleConfiguration>? ConfigurationManager { get; set; }

    public MocaleBuilder WithConfiguration(Action<MocaleConfiguration> configureMocale)
    {
        var config = new MocaleConfiguration();

        configureMocale.Invoke(config);

        ConfigurationManager = new ConfigurationManager<IMocaleConfiguration>(config);

        return this;
    }
}

public static class MocaleBuilderExtensions
{
    public static MocaleBuilder WithLogging(this MocaleBuilder builder, Action<ILoggingBuilder> loggingBuilder)
    {
        loggingBuilder.Invoke(builder.AppBuilder.Logging);

        builder.AppBuilder.Logging.AddFilter("Mocale", LogLevel.Trace);

        return builder;
    }
}
