using Artemis.Plugins.DataModelExpansions.OpenWeather.ViewModels;
using Artemis.UI.Shared;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Artemis.Plugins.DataModelExpansions.OpenWeather.Views;

public partial class OpenWeatherPluginConfigurationView : ReactiveUserControl<OpenWeatherPluginConfigurationViewModel>
{
    public OpenWeatherPluginConfigurationView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}