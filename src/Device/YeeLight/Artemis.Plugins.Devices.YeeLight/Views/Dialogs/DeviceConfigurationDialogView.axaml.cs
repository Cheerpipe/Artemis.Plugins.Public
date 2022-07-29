using Artemis.Plugins.Devices.YeeLight.ViewModels.Dialogs;
using Artemis.UI.Shared;
using Avalonia.Markup.Xaml;

namespace Artemis.Plugins.Devices.YeeLight.Views.Dialogs;

public partial class DeviceConfigurationDialogView : ReactiveCoreWindow<DeviceConfigurationDialogViewModel>
{
    public DeviceConfigurationDialogView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}