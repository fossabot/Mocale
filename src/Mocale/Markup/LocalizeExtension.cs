using System.Globalization;
using Ardalis.GuardClauses;
using Mocale.Managers;

namespace Mocale.Markup;

[ContentProperty(nameof(Key))]
public class LocalizeExtension : IMarkupExtension<BindingBase>
{
    private readonly ITranslatorManager translatorManager;

    public string? Key { get; set; }

    public BindingBase? Binding { get; set; }

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
        if (!string.IsNullOrEmpty(Parameter))
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
        else if (Binding is Binding originalBinding)
        {
            //var parameterTranslatorManager = new ParameterTranslatorManager(translatorManager);

            //parameterTranslatorManager.SetParameters(new object[] { MyBinding });

            var parameter = translatorManager.Translate(Key);

            var converter = new MyBindingValueConverter();

            return new Binding()
            {
                Source = originalBinding.Source,
                Path = originalBinding.Path,
                Converter = converter,
                ConverterParameter = parameter,
                FallbackValue = originalBinding.FallbackValue,
                Mode = originalBinding.Mode,
                StringFormat = originalBinding.StringFormat,
                TargetNullValue = originalBinding.TargetNullValue,
            };

            //return MyBinding;
            //var multiBinding = new MultiBinding();
            //multiBinding.Bindings.Add(MyBinding);
            //multiBinding.Bindings.Add(new Binding
            //{
            //    Mode = BindingMode.OneWay,
            //    Path = $"[{Key}]",
            //    Source = parameterTranslatorManager,
            //    Converter = Converter,
            //});

            //return multiBinding;
        }
        else
        {
            return new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Key}]",
                Source = translatorManager,
                Converter = Converter,
            };
        }
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}

public class MyBindingValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is string translation)
        {
            return string.Format(translation, value);

        }

        throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
