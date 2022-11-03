using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Artemis.Core;
using Artemis.Plugins.DataModelExpansions.OpenWeather.DataModels;
using Artemis.UI.Shared;
using Artemis.UI.Shared.Services;
using Awesomio.NET.Models.CurrentWeather;
using Awesomio.Weather.NET;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace Artemis.Plugins.DataModelExpansions.OpenWeather.ViewModels
{
    public class OpenWeatherPluginConfigurationViewModel : PluginConfigurationViewModel
    {
        #region Variables definitions

        private readonly IWindowService _windowService;

        #endregion

        #region Constructor

        public OpenWeatherPluginConfigurationViewModel(Plugin plugin, PluginSettings settings, IWindowService windowService) : base(plugin)
        {
            _windowService = windowService;

            ApiKey = settings.GetSetting("ApiKey", String.Empty);
            City = settings.GetSetting("City", String.Empty);
            Unit = settings.GetSetting("Unit", UnitsOfMeasurement.Metric.ToString());

            this.ValidationRule(vm => vm.ApiKey, v => !string.IsNullOrWhiteSpace(v.Value), "An Api Keyis required");
            this.ValidationRule(vm => vm.City, v => !string.IsNullOrWhiteSpace(v.Value), "A city name is required");

            Save = ReactiveCommand.Create(ExecuteSave);
            Cancel = ReactiveCommand.CreateFromTask(ExecuteCancel);
            Test = ReactiveCommand.Create(ExecuteTest);
        }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> Test { get; }

        #endregion

        #region Settings

        public PluginSetting<string> ApiKey { get; }
        public PluginSetting<string> City { get; }
        public PluginSetting<string> Unit { get; }
        public List<string> Units { get => Enum.GetNames(typeof(UnitsOfMeasurement)).ToList(); }

        #endregion

        private async void ExecuteTest()
        {
            TestApi(out string message);
            await _windowService.ShowConfirmContentDialog("API Test", message);
        }

        private bool TestApi(out string message)
        {
            string testResult = string.Empty;

            try
            {
                testResult = string.Empty;
                if (string.IsNullOrEmpty(City.Value))
                {
                    message = "One or more settings are empty. Please fill all settings and try again.";
                    return false;
                }

                string accessKey = ApiKey.Value;
                WeatherClient client = new WeatherClient(accessKey);
                CurrrentWeatherModel data = client.GetCurrentWeatherAsync<CurrrentWeatherModel>(City.Value, "en", Unit.Value.ToString()).Result;
                message = $"Connection successful. Getting weather data for {data.Name} - {data.Sys.Country}";
                return true;
            }
            catch (Exception e)
            {
                message = $"Connection failed with response: {e}";
                return false;
            }
        }


        private void ExecuteSave()
        {
            if (HasErrors)
                return;

            ApiKey.Save();
            City.Save();
            Unit.Save();

            Close();
        }

        private async Task ExecuteCancel()
        {
            if (ApiKey.HasChanged ||
                City.HasChanged ||
                Unit.HasChanged)
            {
                if (!await _windowService.ShowConfirmContentDialog("Discard changes", "Do you want to discard any changes you made?"))
                    return;
            }

            ApiKey.RejectChanges();
            City.RejectChanges();
            Unit.RejectChanges();

            Close();
        }
    }
}
