using Artemis.Core.DataModelExpansions;
using CSCore.CoreAudioAPI;
using Serilog;
using System;

namespace Artemis.Plugins.DataModelExpansions.PlaybackVolume
{

    public class PlaybackVolumeDataModelExpansion : DataModelExpansion<PlaybackVolumeDataModel>
    {
        private MMDeviceEnumerator _enumerator;
        private MMDevice _playbackDevice;
        private AudioEndpointVolume _audioEndpointVolume;
        private INotificationClient _notificationClient;
        private IMMNotificationClient _notifyClient;
        private bool _playbackDeviceChanged = false;
        private readonly ILogger _logger;
        private readonly object _audioEventLock = new object();

        public PlaybackVolumeDataModelExpansion(ILogger logger)
        {
            _logger = logger;
        }

        public override void Enable()
        {
            _enumerator = new MMDeviceEnumerator();
            _notificationClient = new INotificationClient();
            _notifyClient = (IMMNotificationClient)_notificationClient;
            _notificationClient.DefaultDeviceChanged += NotificationClient_DefaultDeviceChanged;
            _enumerator.RegisterEndpointNotificationCallback(_notifyClient);
            UpdatePlaybackDevice(true);
            AddTimedUpdate(TimeSpan.FromMilliseconds(10), UpdatePeakVolume);
        }

        private void UpdatePeakVolume(double deltaTime)
        {
            if (this.IsEnabled == false)
            {
                // To avoid null object exception on _enumerator use after plugin is disabled.
                return;
            }

            if (_playbackDevice == null)
            {
                // To avoid null object exception on device change
                return;
            }

            // Update Main Voulume Peak
            lock (_audioEventLock) // To avoid query an Device/EndPoint that is not the current anymore o has more or less channels
            {
                using (var meter = AudioMeterInformation.FromDevice(_playbackDevice))
                {
                    float _peakVolumeNormalized = meter.PeakValue;
                    DataModel.PeakVolumeNormalized = _peakVolumeNormalized * 2f;
                    DataModel.PeakVolume = _peakVolumeNormalized * 100f * 2f;

                    //Update Channels Peak
                    var channelsVolumeNormalized = meter.GetChannelsPeakValues();
                    for (int i = 0; i < DataModel.Channels.DynamicChildren.Count; i++)
                    {
                        var channelDataModel = DataModel.Channels.GetDynamicChild<ChannelDataModel>(string.Format("Channel {0}", i));
                        channelDataModel.Value.PeakVolumeNormalized = channelsVolumeNormalized[i] * 2f;
                        channelDataModel.Value.PeakVolume = channelsVolumeNormalized[i] * 100f * 2f;
                    }
                }
            }
        }

        private void UpdateVolumeDataModel()
        {
            DataModel.VolumeChanged.Trigger();
            DataModel.VolumeNormalized = (_audioEndpointVolume.MasterVolumeLevelScalar);
            DataModel.Volume = DataModel.VolumeNormalized * 100f;
            DataModel.ChannelCount = _audioEndpointVolume.ChannelCount;
            DataModel.DeviceState = _playbackDevice.DeviceState;
            DataModel.Muted = _audioEndpointVolume.IsMuted;
            lock (_audioEventLock)
            {
                foreach (AudioEndpointVolumeChannel channel in _audioEndpointVolume.Channels)
                {
                    var channelDataModel = DataModel.Channels.GetDynamicChild<ChannelDataModel>(string.Format("Channel {0}", channel.ChannelIndex));
                    float volumeNormalized = channel.VolumeScalar;
                    channelDataModel.Value.VolumeNormalized = volumeNormalized;
                    channelDataModel.Value.Volume = volumeNormalized * 100f;
                }
            }
        }

        private void PopulateChannels()
        {
            DataModel.Channels.ClearDynamicChildren();
            _logger.Information(string.Format("Playback device {0} channel list cleared", _playbackDevice.FriendlyName));
            foreach (AudioEndpointVolumeChannel channel in _audioEndpointVolume.Channels)
            {
                DataModel.Channels.AddDynamicChild(
                    string.Format("Channel {0}", channel.ChannelIndex),
                    new ChannelDataModel()
                    {
                        ChannelIndex = channel.ChannelIndex
                    },
                    string.Format("Channel {0}", channel.ChannelIndex)
                    );
                /*
                DataModel.Channels.AddDynamicChild(
                    new ChannelDataModel()
                    {
                        ChannelIndex = channel.ChannelIndex
                    },
                    channel.ChannelIndex.ToString(),
                    string.Format("Channel {0}", channel.ChannelIndex)
                    );
                */
                _logger.Information(string.Format("Playback device {0} channel {1} populated", _playbackDevice.FriendlyName, channel.ChannelIndex));
            }
        }

        private void NotificationClient_DefaultDeviceChanged()
        {
            _playbackDeviceChanged = true;
            // Workarround. MMDevice won't dispose if Dispose() is called from 
            // non parent thread and NaudioNotificationClient callbacks come from another thread.
        }

        private AudioSessionManager2 GetDefaultAudioSessionManager2(DataFlow dataFlow)
        {
            try
            {
                using (var device = _enumerator.GetDefaultAudioEndpoint(dataFlow, Role.Multimedia))
                {
                    var sessionManager = AudioSessionManager2.FromMMDevice(device);
                    return sessionManager;
                }
            }
            catch
            {
                return null;
            }
        }

        private void UpdatePlaybackDevice(bool firstRun = false)
        {
            lock (_audioEventLock)
            {
                if (!firstRun) { FreePlaybackDevice(); };
                SetPlaybackDevice();
                PopulateChannels();
                _playbackDeviceChanged = false;
                UpdateVolumeDataModel();
            }
        }

        public override void Disable()
        {
            _notificationClient.DefaultDeviceChanged -= NotificationClient_DefaultDeviceChanged;
            _enumerator.UnregisterEndpointNotificationCallback(_notifyClient);
            _audioEndpointVolume.Dispose();
            FreePlaybackDevice();
            _enumerator.Dispose();
        }

        private void FreePlaybackDevice()
        {
            string disposingPlaybackDeviceFriendlyName = _playbackDevice?.FriendlyName ?? "Unknown";
            _audioEndpointVolume?.Dispose();
            _playbackDevice?.Dispose();
            _playbackDevice = null;
            _logger.Information(string.Format("Playback device {0} unregistered as source device to fill Playback volume data model", disposingPlaybackDeviceFriendlyName));
        }

        private void SetPlaybackDevice()
        {
            _playbackDevice = _enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            if (_playbackDevice == null)
            {
                _logger.Warning("No audio device found with Console role");
                return;
            }
            _audioEndpointVolume = AudioEndpointVolume.FromDevice(_playbackDevice);

            AudioEndpointVolumeCallback audioEndpointVolumeCallback = new AudioEndpointVolumeCallback();
            audioEndpointVolumeCallback.NotifyRecived += (s, e) =>
            {
                UpdateVolumeDataModel();
            };
            _audioEndpointVolume.RegisterControlChangeNotify(audioEndpointVolumeCallback);
            DataModel.DefaultDeviceName = _playbackDevice.FriendlyName;
            _logger.Information(string.Format("Playback device {0} registered to to fill Playback volume data model", _playbackDevice.FriendlyName));
        }

        public override void Update(double deltaTime)
        {
            if (_playbackDeviceChanged)
            {
                UpdatePlaybackDevice();
            }
        }
    }
}