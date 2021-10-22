using Artemis.Core.Modules;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Artemis.Plugins.DataModelExpansions.OpenWeather.DataModels
{
    public class OpenWeatherDataModel : DataModel
    {
        // Weather Measurements
        public WeatherConditions Weather { get; set; }
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }

        // Visibility
        public int Clouds { get; set; }
        public int Visibility { get; set; }
        // Sunrise Sunset
        public DateTime Sunrise { get; set; } // unix, UTC
        public DateTime Sunset { get; set; } // unix, UTC

        // Wind
        public WeatherWindDataModel Wind { get; set; } = new();
    }

    public class WeatherDataModel : DataModel
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class WeatherWindDataModel : DataModel
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
        public WindDirectionCodes WindDirection { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum WindDirectionCodes
    {
        Unknown,
        N,
        NNE,
        NE,
        ENE,
        E,
        ESE,
        SE,
        SSE,
        S,
        SSW,
        SW,
        WSW,
        W,
        WNW,
        NW,
        NNW
    }

    public enum WeatherConditions
    {
        Unknown,
        Thunderstorm,
        Drizzle,
        Rain,
        Snow,
        Mist,
        Smoke,
        Haze,
        Dust,
        Fog,
        Sand,
        Ash,
        Squall,
        Tornado,
        Clear,
        Clouds
    }

    public enum UnitsOfMeasurement
    {
        Standard,
        Metric,
        Imperial
    }
}