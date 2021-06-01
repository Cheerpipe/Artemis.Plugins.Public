using Artemis.Core.Services;
using Artemis.Plugins.DataModelExpansions.YTMdesktop.DataModels;
using Serilog;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Artemis.Core.Modules;

namespace Artemis.Plugins.DataModelExpansions.YTMdesktop
{
    public class YTMdesktopDataModelExpansion : Module<YTMdesktopDataModel>
    {
        #region Variables declarations

        private readonly ILogger _logger;
        private readonly IColorQuantizerService _colorQuantizer;
        private readonly IProcessMonitorService _processMonitorService;
        private readonly HttpClient _httpClient;
        private readonly ConcurrentDictionary<string, TrackColorsDataModel> albumArtColorCache;
        private bool _youtubeIsRunning = false;
        private const string YTMD_PROCESS_NAME = "YouTube Music Desktop App";
        private YTMDesktopClient _YTMDesktopClient;
        private RootInfo _rootInfo;
        private string _trackId;
        private string _albumArtUrl;

        #endregion

        #region Constructor

        public YTMdesktopDataModelExpansion(ILogger logger, IColorQuantizerService colorQuantizer, IProcessMonitorService processMonitorService)
        {
            _processMonitorService = processMonitorService;
            _logger = logger;
            _colorQuantizer = colorQuantizer;

            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(1)
            };
            albumArtColorCache = new ConcurrentDictionary<string, TrackColorsDataModel>();
            IsAlwaysAvailable = true;
        }

        #endregion

        #region Plugin Methods
        public override void Enable()
        {
            AddTimedUpdate(TimeSpan.FromSeconds(1), UpdateData);

            _YTMDesktopClient = new YTMDesktopClient();
            _youtubeIsRunning = YoutubeIsRunning();
            _processMonitorService.ProcessStarted += _processMonitorService_ProcessStarted;
            _processMonitorService.ProcessStopped += _processMonitorService_ProcessStopped;
        }

        private void _processMonitorService_ProcessStopped(object sender, ProcessEventArgs e)
        {
            if (e.Process.ProcessName == YTMD_PROCESS_NAME)
            {
                _youtubeIsRunning = YoutubeIsRunning();
            }
        }

        private void _processMonitorService_ProcessStarted(object sender, ProcessEventArgs e)
        {
            Debug.WriteLine(e.Process.ProcessName);
            if (e.Process.ProcessName == YTMD_PROCESS_NAME)
            {
                _youtubeIsRunning = YoutubeIsRunning();
            }
        }

        private bool YoutubeIsRunning()
        {
            return _processMonitorService.GetRunningProcesses().Any(p => p.ProcessName == YTMD_PROCESS_NAME);
        }

        public override void Disable()
        {
            _YTMDesktopClient = null;
            _trackId = null;
            _albumArtUrl = null;
            _processMonitorService.ProcessStarted -= _processMonitorService_ProcessStarted;
            _processMonitorService.ProcessStopped -= _processMonitorService_ProcessStopped;
        }

        public override void Update(double deltaTime)
        {
            if (DataModel.Player.isPaused)
                return;
            if (DataModel.Track.duration == 0)
                return;

            DataModel.Player.seekbarCurrentPositionHuman = DataModel.Player.seekbarCurrentPositionHuman.Add(TimeSpan.FromMilliseconds(deltaTime * 1000));
            DataModel.Player.seekbarCurrentPosition = DataModel.Player.seekbarCurrentPositionHuman.TotalSeconds;
            DataModel.Player.statePercent = DataModel.Player.seekbarCurrentPosition / DataModel.Track.duration;
        }
        #endregion

        #region DataModel update methods

        private async Task UpdateData(double deltaTime)
        {
            UpdateYTMDekstopInfo();
        }

        private void UpdateYTMDekstopInfo()
        {
            if (!_youtubeIsRunning)
            {
                // Don't query server if YTMD proccess is down.
                DataModel.Empty();
                return;
            }

            try
            {
                // Update DataModel using /query API
                _YTMDesktopClient?.Update();
                _rootInfo = _YTMDesktopClient?.Data;

                if (_rootInfo != null)
                {
                    UpdateInfo(_rootInfo);
                }
                else
                {
                    DataModel.Empty();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
        }

        private void UpdateInfo(RootInfo data)
        {
            UpdatePlayerInfo(data.player);

            if (data.player.hasSong && data.track != null)
                UpdateTrackInfo(data.track);
            else
            {
                DataModel.Track.Empty();
            }
        }

        private async Task UpdateTrackInfo(TrackInfo track)
        {
            if (track.id != _trackId)
            {
                UpdateBasicTrackInfo(track);

                if (track.cover != _albumArtUrl)
                {
                    if (string.IsNullOrEmpty(track.cover))
                        return;
                    await UpdateAlbumArtColors(track.cover);
                    _albumArtUrl = track.cover;
                }

                _trackId = track.id;
            }
        }

        private void UpdatePlayerInfo(PlayerInfo player)
        {
            DataModel.Player.IsRunning = true;
            DataModel.Player.hasSong = player.hasSong;
            DataModel.Player.isPaused = player.isPaused;
            DataModel.Player.volumePercent = player.volumePercent;
            DataModel.Player.seekbarCurrentPosition = player.seekbarCurrentPosition;
            DataModel.Player.seekbarCurrentPositionHuman = TimeSpan.FromSeconds(player.seekbarCurrentPosition);
            DataModel.Player.statePercent = player.statePercent;
            DataModel.Player.likeStatus = player.likeStatus;
            DataModel.Player.repeatType = Enum.Parse<RepeatState>(player.repeatType, true);
        }

        private async Task UpdateAlbumArtColors(string albumArtUrl)
        {
            if (!albumArtColorCache.ContainsKey(albumArtUrl))
            {
                try
                {
                    using HttpResponseMessage response = await _httpClient.GetAsync(albumArtUrl);
                    await using Stream stream = await response.Content.ReadAsStreamAsync();
                    using SKBitmap skbm = SKBitmap.Decode(stream);
                    stream.Dispose();
                    SKColor[] skClrs = _colorQuantizer.Quantize(skbm.Pixels, 256);
                    albumArtColorCache[albumArtUrl] = new TrackColorsDataModel
                    {
                        Vibrant = _colorQuantizer.FindColorVariation(skClrs, ColorType.Vibrant, true),
                        LightVibrant = _colorQuantizer.FindColorVariation(skClrs, ColorType.LightVibrant, true),
                        DarkVibrant = _colorQuantizer.FindColorVariation(skClrs, ColorType.DarkVibrant, true),
                        Muted = _colorQuantizer.FindColorVariation(skClrs, ColorType.Muted, true),
                        LightMuted = _colorQuantizer.FindColorVariation(skClrs, ColorType.LightMuted, true),
                        DarkMuted = _colorQuantizer.FindColorVariation(skClrs, ColorType.DarkMuted, true),
                    };
                }
                catch (Exception exception)
                {
                    _logger.Error("Failed to get album art colors: " + exception);
                    throw;
                }
            }

            if (albumArtColorCache.TryGetValue(albumArtUrl, out var colorDataModel))
                DataModel.Track.Colors = colorDataModel;
        }

        private void UpdateBasicTrackInfo(TrackInfo track)
        {
            DataModel.Track.author = track.author;
            DataModel.Track.title = track.title;
            DataModel.Track.album = track.album;
            DataModel.Track.cover = track.cover;
            DataModel.Track.duration = track.duration;
            DataModel.Track.durationHuman = TimeSpan.FromSeconds(track.duration);
            DataModel.Track.url = track.url;
            DataModel.Track.id = track.id;
            DataModel.Track.isVideo = track.isVideo;
            DataModel.Track.isAdvertisement = track.isAdvertisement;
            DataModel.Track.inLibrary = track.inLibrary;
        }

        #endregion
    }
}