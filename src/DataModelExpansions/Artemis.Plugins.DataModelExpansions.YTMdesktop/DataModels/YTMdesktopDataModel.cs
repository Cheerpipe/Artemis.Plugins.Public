using Artemis.Core.Modules;
using Artemis.Core.Services;
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

        public ColorSwatch Colors { get; set; } = new();

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
        }
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