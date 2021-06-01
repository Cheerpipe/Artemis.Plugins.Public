using Artemis.Core;
using Serilog;
using System;
using Artemis.Plugins.DataModelExpansions.OpenWeather.DataModels;
using Awesomio.Weather.NET;
using Awesomio.NET.Models.CurrentWeather;
using System.Linq;
using Artemis.Core.Modules;

namespace Artemis.Plugins.DataModelExpansions.DisplaySettings
{
    public class DisplayModeDataModelExpansion : Module<OpenWeatherDataModel>
    {
        private readonly PluginSetting<string> _apiKeySetting;
        private readonly PluginSetting<string> _citySetting;
        private readonly PluginSetting<string> _unitOfMeasurementSetting;
        private readonly ILogger _logger;

        public DisplayModeDataModelExpansion(PluginSettings pluginSettings, ILogger logger)
        {
            _logger = logger;
            _apiKeySetting = pluginSettings.GetSetting("OpenWeatherApiKey", string.Empty);
            _citySetting = pluginSettings.GetSetting("OpenWeatherCity", string.Empty);
            _unitOfMeasurementSetting = pluginSettings.GetSetting("OpenWeatherUnitOfMeasurement", Enum.GetNames(typeof(UnitsOfMeasurement)).FirstOrDefault());

            _apiKeySetting.PropertyChanged += _OpenWeatherSettingsChanged_PropertyChanged;
            _citySetting.PropertyChanged += _OpenWeatherSettingsChanged_PropertyChanged;
            _unitOfMeasurementSetting.PropertyChanged += _OpenWeatherSettingsChanged_PropertyChanged;
            IsAlwaysAvailable = true;
        }

        private void _OpenWeatherSettingsChanged_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateWeatherData();
        }

        public override void Enable()
        {
            AddTimedUpdate(TimeSpan.FromMinutes(10), _ => UpdateWeatherData());
            UpdateWeatherData();
            IsAlwaysAvailable = true;
        }

        public override void Disable() { }

        public override void Update(double deltaTime) { }

        public void UpdateWeatherData()
        {
            try
            {
                //If no settings, do nothing
                if (string.IsNullOrEmpty(_citySetting.Value) || string.IsNullOrEmpty(_unitOfMeasurementSetting.Value))
                    return;

                //TODO: Use settings
                string accessKey = _apiKeySetting.Value;
                WeatherClient client = new WeatherClient(accessKey);
                CurrrentWeatherModel data = client.GetCurrentWeatherAsync<CurrrentWeatherModel>(_citySetting.Value, "en", _unitOfMeasurementSetting.Value).Result;

                // Weather Measurements
                DataModel.Weather = (WeatherConditions)Enum.Parse(typeof(WeatherConditions), data.Weather.FirstOrDefault().Main);
                DataModel.Temp = data.Main.Temp;
                DataModel.FeelsLike = data.Main.FeelsLike;
                DataModel.TempMin = data.Main.TempMin;
                DataModel.TempMax = data.Main.TempMax;
                DataModel.Pressure = data.Main.Pressure;
                DataModel.Humidity = data.Main.Humidity;

                // Visibility
                DataModel.Clouds = data.Clouds.All; // Cloudiness
                DataModel.Visibility = data.Visibility; // Meters

                // Sunrise Sunset


                DataModel.Sunrise = DateTimeOffset.FromUnixTimeSeconds(data.Sys.Sunrise).DateTime.ToLocalTime(); // unix, UTC
                DataModel.Sunset = DateTimeOffset.FromUnixTimeSeconds(data.Sys.Sunset).DateTime.ToLocalTime(); // unix, UTC
                // unix, UTC

                // Wind
                DataModel.Wind.Speed = data.Wind.Speed;
                DataModel.Wind.Deg = data.Wind.Deg;
                DataModel.Wind.WindDirection = (WindDirectionCodes)Enum.Parse(typeof(WindDirectionCodes), data.Wind.WindDirection);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
        }
    }
}//Enum.Parse(typeof(Colors), colorName));