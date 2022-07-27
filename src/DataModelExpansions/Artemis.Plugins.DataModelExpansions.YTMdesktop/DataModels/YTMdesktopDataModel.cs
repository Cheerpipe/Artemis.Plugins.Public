using Artemis.Core.Modules;
using Artemis.Core.Services;
using System;
// ReSharper disable InconsistentNaming

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
        public bool HasSong { get; set; }
        public bool IsPaused { get; set; }
        public float VolumePercent { get; set; }
        public double SeekbarCurrentPosition { get; set; }
        public TimeSpan SeekbarCurrentPositionHuman { get; set; }
        public double StatePercent { get; set; }
        public string LikeStatus { get; set; }
        public RepeatState RepeatType { get; set; }

        public void Empty()
        {
            IsRunning = false;
            HasSong = false;
            IsPaused = true;
            VolumePercent = 0;
            SeekbarCurrentPosition = 0;
            SeekbarCurrentPositionHuman = TimeSpan.Zero;
            StatePercent = 0;
            LikeStatus = string.Empty;
            RepeatType = RepeatState.None;
        }
    }

    public class YTMdesktopTrackDataModel : DataModel
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Cover { get; set; }
        public double Duration { get; set; }
        public TimeSpan DurationHuman { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public bool IsVideo { get; set; }
        public bool IsAdvertisement { get; set; }
        public bool InLibrary { get; set; }

        public ColorSwatch Colors { get; set; } = new();

        public void Empty()
        {
            Author = string.Empty;
            Title = string.Empty;
            Album = string.Empty;
            Cover = string.Empty;
            Duration = 0;
            DurationHuman = TimeSpan.Zero;
            Url = string.Empty;
            Id = string.Empty;
            IsVideo = false;
            IsAdvertisement = false;
            InLibrary = false;
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