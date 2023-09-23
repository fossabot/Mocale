using System.ComponentModel;
using Ardalis.GuardClauses;
using Mocale.Managers;

namespace Mocale.Markup;

[ContentProperty(nameof(Key))]
public class LocalizeExtension : IMarkupExtension<BindingBase>
{
    private readonly ITranslatorManager translatorManager;

    public string? Key { get; set; }

    public IValueConverter? Converter { get; set; }

    public string? Parameter { get; set; }

    public LocalizeExtension()
        : this(MocaleLocator.TranslatorManager)
    {
    }

    public LocalizeExtension(ITranslatorManager translatorManager)
    {
        this.translatorManager = Guard.Against.Null(translatorManager, nameof(translatorManager));
    }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(Parameter))
        {
            return new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Key}]",
                Source = translatorManager,
                Converter = Converter,
            };
        }
        else
        {
            var parameterTranslatorManager = new ParameterTranslatorManager(translatorManager);

            parameterTranslatorManager.SetParameters(new object[] { Parameter });

            return new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Key}]",
                Source = parameterTranslatorManager,
                Converter = Converter,
            };
        }
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
