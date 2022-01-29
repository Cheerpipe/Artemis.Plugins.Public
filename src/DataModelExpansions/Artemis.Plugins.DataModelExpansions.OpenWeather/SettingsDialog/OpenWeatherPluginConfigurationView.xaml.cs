using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Artemis.Plugins.DataModelExpansions.OpenWeather.SettingsDialog
{
    /// <summary>
    /// Interaction logic for OpenWeatherPluginConfigurationView.xaml
    /// </summary>
    public partial class OpenWeatherPluginConfigurationView : UserControl
    {
        public OpenWeatherPluginConfigurationView()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            var ps = new ProcessStartInfo(((Hyperlink)sender).NavigateUri.ToString())
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }
    }
}
