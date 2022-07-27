using Artemis.Plugins.Devices.Adalight.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Artemis.Plugins.Devices.Adalight.Views;

public partial class AdalightConfigurationView : ReactiveUserControl<AdalightConfigurationViewModel>
{
    public AdalightConfigurationView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}