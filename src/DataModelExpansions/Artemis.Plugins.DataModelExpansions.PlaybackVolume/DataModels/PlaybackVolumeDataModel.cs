using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using CSCore.CoreAudioAPI;

namespace Artemis.Plugins.DataModelExpansions.PlaybackVolume
{
    public class PlaybackVolumeDataModel : DataModel
    {
        public string DefaultDeviceName { get; set; }
        public int ChannelCount { get; set; }
        public float Volume { get; set; }
        public float VolumeNormalized { get; set; }
        public bool Muted { get; set; }
        public float PeakVolume { get; set; }
        public float PeakVolumeNormalized { get; set; }
        public DeviceState DeviceState { get; set; }
        //public AudioSessionsDataModel AudioSessions { get; set; } = new AudioSessionsDataModel();
        public ChannelsDataModel Channels { get; set; } = new ChannelsDataModel();
        public DataModelEvent VolumeChanged { get; set; } = new DataModelEvent();
        public DataModelEvent SoundPlayed { get; set; } = new DataModelEvent();
    }

    public class ChannelsDataModel : DataModel { }
    public class ChannelDataModel : DataModel
    {
        public int ChannelIndex { get; set; }
        public float Volume { get; set; }
        public float VolumeNormalized { get; set; }
        public float PeakVolume { get; set; }
        public float PeakVolumeNormalized { get; set; }
    }
}