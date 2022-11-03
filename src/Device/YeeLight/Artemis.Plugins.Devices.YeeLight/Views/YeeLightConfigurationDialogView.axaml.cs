using Artemis.Plugins.Devices.YeeLight.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Artemis.Plugins.Devices.YeeLight.Views;

public partial class YeeLightConfigurationView : ReactiveUserControl<YeeLightConfigurationViewModel>
{
    public YeeLightConfigurationView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}