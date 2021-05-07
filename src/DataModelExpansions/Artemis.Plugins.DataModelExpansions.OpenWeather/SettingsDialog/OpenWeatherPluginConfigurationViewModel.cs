using Artemis.Core;
using Artemis.Plugins.DataModelExpansions.OpenWeather.DataModels;
using Artemis.UI.Shared;
using Awesomio.NET.Models.CurrentWeather;
using Awesomio.Weather.NET;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Artemis.Plugins.DataModelExpansions.OpenWeather
{
    public class OpenWeatherPluginConfigurationViewModel : PluginConfigurationViewModel
    {
        private string _apiKey;
        private string _city;
        private string _testResult;
        private string _unitOfMeasurement;
        private List<Tuple<string, int>> _unitOfMeasurements;

        private readonly PluginSetting<string> _apiKeySetting;
        private readonly PluginSetting<string> _citySetting;
        private readonly PluginSetting<string> _unitOfMeasurementSetting;

        public string ApiKey
        {
            get => _apiKey;
            set => SetAndNotify(ref _apiKey, value);
        }
        public string City
        {
            get => _city;
            set => SetAndNotify(ref _city, value);
        }

        public List<Tuple<string, int>> UnitsOfMeasurements
        {
            get => _unitOfMeasurements;
            set => SetAndNotify(ref _unitOfMeasurements, value);
        }

        public string TestResult
        {
            // How to make it works?
            get => _testResult;
            set => SetAndNotify(ref _testResult, value);
        }

        public Tuple<string, int> SelectedUnitOfMeasurement
        {
            get => UnitsOfMeasurements.FirstOrDefault(uom => uom.Item1 == UnitOfMeasurement);
            set => UnitOfMeasurement = value.Item1;
        }
        public string UnitOfMeasurement
        {
            get => _unitOfMeasurement;
            set => SetAndNotify(ref _unitOfMeasurement, value);
        }

        public OpenWeatherPluginConfigurationViewModel(Plugin plugin, PluginSettings pluginSettings) : base(plugin)
        {
            _apiKeySetting = pluginSettings.GetSetting<string>("OpenWeatherApiKey", string.Empty);
            _citySetting = pluginSettings.GetSetting<string>("OpenWeatherCity", string.Empty);
            _unitOfMeasurementSetting = pluginSettings.GetSetting<string>("OpenWeatherUnitOfMeasurement", Enum.GetNames(typeof(UnitsOfMeasurement)).FirstOrDefault());

            _apiKey = _apiKeySetting.Value;
            _city = _citySetting.Value;
            _unitOfMeasurement = _unitOfMeasurementSetting.Value;

            // Populate measurements units
            _unitOfMeasurements = Enum.GetValues(typeof(UnitsOfMeasurement)).Cast<UnitsOfMeasurement>().Select(oum => new Tuple<string, int>(
                  oum.ToString(),
                  (int)oum)
            ).ToList();
        }

        public void Save()
        {
            _apiKeySetting.Value = _apiKey?.Trim();
            _apiKeySetting.Save();

            _citySetting.Value = _city?.Trim();
            _citySetting.Save();

            _unitOfMeasurementSetting.Value = _unitOfMeasurement;
            _unitOfMeasurementSetting.Save();
            RequestClose();
        }

        public bool Test()
        {
            try
            {
                TestResult = string.Empty;
                if (string.IsNullOrEmpty(City) || string.IsNullOrEmpty(UnitOfMeasurement))
                {
                    _testResult = string.Format("One or more settings are empty. Please fill all settings and try again.");
                    return false;
                }


                //TODO: Use settings
                string accessKey = ApiKey;
                WeatherClient client = new WeatherClient(accessKey);
                CurrrentWeatherModel data = client.GetCurrentWeatherAsync<CurrrentWeatherModel>(City, "en", UnitOfMeasurement).Result;
                TestResult = string.Format("Connection successful. Getting weather data for {0} - {1}", data.Name, data.Sys.Country);
                return true;
            }
            catch (Exception e)
            {
                TestResult = string.Format("Connection failed with response: {0}", e.ToString());
                return false;
            }
        }

        public void Reset()
        {
            ApiKey = null;
            City = null;

            _apiKeySetting.Value = string.Empty;
            _apiKeySetting.Save();

            _citySetting.Value = string.Empty;
            _citySetting.Save();

            _unitOfMeasurementSetting.Value = Enum.GetNames(typeof(UnitsOfMeasurement)).FirstOrDefault();
            _citySetting.Save();
        }

        public void Cancel()
        {
            RequestClose();
        }
    }
}
