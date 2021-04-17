using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using Artemis.Core.Services;
using Artemis.Plugins.DataModelExpansions.YTMdesktop.DataModels;
using Serilog;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Linq;

namespace Artemis.Plugins.DataModelExpansions.YTMdesktop
{
    public class YTMdesktopDataModelExpansion : DataModelExpansion<YTMdesktopDataModel>
    {
        #region Constructor and readonly fields
        private readonly ILogger _logger;
        private readonly IColorQuantizerService _colorQuantizer;
        private readonly IProcessMonitorService _processMonitorService;
        private readonly HttpClient _httpClient;
        private readonly ConcurrentDictionary<string, TrackColorsDataModel> albumArtColorCache;
        private TimedUpdateRegistration queryServerUpdateRegistration;

        private const string YTMD_PROCESS_NAME = "youtube music desktop app";

        public YTMdesktopDataModelExpansion(PluginSettings settings, ILogger logger, IColorQuantizerService colorQuantizer, IProcessMonitorService processMonitorService)
        {
            _processMonitorService = processMonitorService;
            _logger = logger;
            _colorQuantizer = colorQuantizer;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(1)
            };
            albumArtColorCache = new ConcurrentDictionary<string, TrackColorsDataModel>();
        }
        #endregion

        private YTMDesktopClient _YTMDesktopClient;
        private YTMDesktopTrackInfo _YTMDesktopTrackInfo;
        private YTMDesktopPlayerInfo _YTMDesktopPlayerInfo;
        private string _trackId;
        private string _albumArtUrl;

        #region Plugin Methods
        public override void Enable()
        {
            if (queryServerUpdateRegistration == null)
                queryServerUpdateRegistration = AddTimedUpdate(TimeSpan.FromSeconds(1), UpdateData);
            _YTMDesktopClient = new YTMDesktopClient();
        }

        public override void Disable()
        {
            _YTMDesktopClient = null;
            _trackId = null;
            _albumArtUrl = null;
            queryServerUpdateRegistration.Stop();
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
            if (!_processMonitorService.GetRunningProcesses().Any(p => p.ProcessName.ToLower() == YTMD_PROCESS_NAME))
            {
                // Don't query server if YTMD proccess is down.
                DataModel.Empty();
                return;
            }

            try
            {
                // Update Player Data Model
                _YTMDesktopClient?.UpdatePlayerInfo();
                _YTMDesktopPlayerInfo = _YTMDesktopClient?.PlayerInfo;

                if (_YTMDesktopPlayerInfo != null)
                {
                    UpdatePlayerInfo(_YTMDesktopPlayerInfo);
                }
                else
                {
                    DataModel.Empty();
                    return;
                }

                // We don't need to query 
                _YTMDesktopClient?.UpdateTrackInfo();
                _YTMDesktopTrackInfo = _YTMDesktopClient?.TrackInfo;

                if (_YTMDesktopTrackInfo != null)
                {
                    await UpdateTrackInfo(_YTMDesktopTrackInfo);
                }
                else
                {
                    DataModel.Track.Empty();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
        }

        private async Task UpdateTrackInfo(YTMDesktopTrackInfo track)
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


        private void UpdatePlayerInfo(YTMDesktopPlayerInfo player)
        {
            DataModel.Player.IsRunning = true;
            DataModel.Player.hasSong = player.hasSong;
            DataModel.Player.isPaused = player.isPaused;
            DataModel.Player.volumePercent = player.volumePercent;
            DataModel.Player.seekbarCurrentPosition = player.seekbarCurrentPosition;
            DataModel.Player.seekbarCurrentPositionHuman = TimeSpan.ParseExact(
                player.seekbarCurrentPositionHuman,
                new string[] { "s", "ss", "m\\:s", "h\\:m\\:s" },
                CultureInfo.CurrentCulture
                );
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
                    using Stream stream = await response.Content.ReadAsStreamAsync();
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
                    _logger.Error("Failed to get album art colors: " + exception.ToString());
                    throw;
                }
            }

            if (albumArtColorCache.TryGetValue(albumArtUrl, out var colorDataModel))
                DataModel.Track.Colors = colorDataModel;
        }

        private void UpdateBasicTrackInfo(YTMDesktopTrackInfo track)
        {
            DataModel.Track.author = track.author;
            DataModel.Track.title = track.title;
            DataModel.Track.album = track.album;
            DataModel.Track.cover = track.cover;
            DataModel.Track.duration = track.duration;
            DataModel.Track.durationHuman = TimeSpan.ParseExact(
                track.durationHuman,
                new string[] { "s", "ss", "m\\:s", "h\\:m\\:s" },
                CultureInfo.CurrentCulture
                );
            DataModel.Track.url = track.url;
            DataModel.Track.id = track.id;
            DataModel.Track.isVideo = track.isVideo;
            DataModel.Track.isAdvertisement = track.isAdvertisement;
            DataModel.Track.inLibrary = track.inLibrary;
        }

        #endregion
    }
}