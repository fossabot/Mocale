using System.Globalization;
using MvvmHelpers;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;

namespace Mocale.Samples.ViewModels;

public partial class IntroductionPageViewModel : ObservableObject
{
    private readonly ILocalizationManager localizationManager;

    public ObservableRangeCollection<string> Locales { get; }

    private string selectedLocale;

    public string SelectedLocale
    {
        get => selectedLocale;
        set
        {
            var oldValue = selectedLocale;

            if (SetProperty(ref selectedLocale, value))
            {
                RaiseLocaleSelected(oldValue, value);
            }
        }
    }

    [ObservableProperty]
    private int temperature;

    public IntroductionPageViewModel(ILocalizationManager localizationManager)
    {
        this.localizationManager = localizationManager;

        Locales = new ObservableRangeCollection<string>(new[]
        {
            "en-GB",
            "fr-FR",
            "it-IT",
        });

        var selectedLocale = Locales.FirstOrDefault(localizationManager.CurrentCulture.Name.Equals);

        SelectedLocale = selectedLocale ?? Locales[0];

        var random = new Random();
        temperature = random.Next(1, 100);
    }

    private async void RaiseLocaleSelected(string oldValue, string newValue)
    {
        if (string.IsNullOrEmpty(oldValue))
        {
            return;
        }

        if (oldValue.Equals(newValue, StringComparison.Ordinal))
        {
            return;
        }

        var culture = new CultureInfo(newValue);

        if (culture.Equals(localizationManager.CurrentCulture))
        {
            return;
        }

        var loaded = await localizationManager.SetCultureAsync(culture);

        if (!loaded)
        {
            SelectedLocale = oldValue;
        }
    }
}
