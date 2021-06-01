using Artemis.Core.DataModelExpansions;
using SkiaSharp;
using System;
using System.ComponentModel;

namespace Artemis.Plugins.DataModelExpansions.YTMdesktop.DataModels
{
    public class YTMdesktopDataModel : DataModel
    {
        public YTMdesktopPlayerDataModel Player { get; set; } = new();
        public YTMdesktopTrackDataModel Track { get; set; } = new();

        public void Empty()
        {
            Player?.Empty();
            Track?.Empty();
        }
    }

    public class YTMdesktopPlayerDataModel : DataModel
    {
        public bool IsRunning { get; set; }
        public bool hasSong { get; set; }
        public bool isPaused { get; set; }
        public int volumePercent { get; set; }
        public double seekbarCurrentPosition { get; set; }
        public TimeSpan seekbarCurrentPositionHuman { get; set; }
        public double statePercent { get; set; }
        public string likeStatus { get; set; }
        public RepeatState repeatType { get; set; }

        public void Empty()
        {
            IsRunning = false;
            hasSong = false;
            isPaused = true;
            volumePercent = 0;
            seekbarCurrentPosition = 0;
            seekbarCurrentPositionHuman = TimeSpan.Zero;
            statePercent = 0;
            likeStatus = string.Empty;
            repeatType = RepeatState.None;
        }
    }

    public class YTMdesktopTrackDataModel : DataModel
    {
        public string author { get; set; }
        public string title { get; set; }
        public string album { get; set; }
        public string cover { get; set; }
        public double duration { get; set; }
        public TimeSpan durationHuman { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public bool isVideo { get; set; }
        public bool isAdvertisement { get; set; }
        public bool inLibrary { get; set; }

        public TrackColorsDataModel Colors { get; set; } = new();

        public void Empty()
        {
            author = string.Empty;
            title = string.Empty;
            album = string.Empty;
            cover = string.Empty;
            duration = 0;
            durationHuman = TimeSpan.Zero;
            url = string.Empty;
            id = string.Empty;
            isVideo = false;
            isAdvertisement = false;
            inLibrary = false;
            Colors?.Empty();
        }
    }

    public class TrackColorsDataModel : DataModel
    {
        public SKColor Vibrant { get; set; }
        public SKColor LightVibrant { get; set; }
        public SKColor DarkVibrant { get; set; }
        public SKColor Muted { get; set; }
        public SKColor LightMuted { get; set; }
        public SKColor DarkMuted { get; set; }

        public void Empty()
        {
            Vibrant = SKColors.Transparent;
            LightVibrant = SKColors.Transparent;
            DarkVibrant = SKColors.Transparent;
            Muted = SKColors.Transparent;
            LightMuted = SKColors.Transparent;
            DarkMuted = SKColors.Transparent;
        }

    }

    public enum Key
    {
        None = -1,
        C = 0,
        [Description("C#")]
        Cs = 1,
        D = 2,
        [Description("D#")]
        Ds = 3,
        E = 4,
        F = 5,
        [Description("F#")]
        Fs = 6,
        G = 7,
        [Description("G#")]
        Gs = 8,
        A = 9,
        [Description("A#")]
        As = 10,
        B = 11,
    }

    public enum Mode
    {
        Minor = 0,
        Major = 1
    }

    public enum RepeatState
    {
        None,
        All,
        One
    }
}