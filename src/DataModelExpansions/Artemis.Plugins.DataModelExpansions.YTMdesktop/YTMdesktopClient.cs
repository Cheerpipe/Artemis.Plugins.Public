using Newtonsoft.Json;
using RestSharp;

namespace Artemis.Plugins.DataModelExpansions.YTMdesktop
{

    public class YTMDesktopClient
    {
        RestClient _client;
        RestRequest _queryTrackInfoRequest;
        RestRequest _queryPlayerInfoRequest;
        YTMDesktopTrackInfo _trackInfo;
        YTMDesktopPlayerInfo _playerInfo;

        public YTMDesktopClient()
        {
            _client = new RestClient("http://127.0.0.1:9863");
            _queryTrackInfoRequest = new RestRequest("query/track", Method.GET);
            _queryPlayerInfoRequest = new RestRequest("query/player", Method.GET);
        }

        public YTMDesktopTrackInfo TrackInfo
        {
            get
            {
                return _trackInfo;
            }
        }

        public YTMDesktopPlayerInfo PlayerInfo
        {
            get
            {
                return _playerInfo;
            }
        }

        public void UpdateTrackInfo()
        {
            try
            {
                var response = _client.Execute(_queryTrackInfoRequest);
                _trackInfo = JsonConvert.DeserializeObject<YTMDesktopTrackInfo>(response.Content);
            }
            catch
            {
                _trackInfo = null;
            }
        }

        public void UpdatePlayerInfo()
        {
            try
            {
                var response = _client.Execute(_queryPlayerInfoRequest);
                _playerInfo = JsonConvert.DeserializeObject<YTMDesktopPlayerInfo>(response.Content);
            }
            catch
            {
                _playerInfo = null;
            }
        }
    }

    public class YTMDesktopTrackInfo
    {
        public string author { get; set; }
        public string title { get; set; }
        public string album { get; set; }
        public string cover { get; set; }
        public int duration { get; set; }
        public string durationHuman { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public bool isVideo { get; set; }
        public bool isAdvertisement { get; set; }
        public bool inLibrary { get; set; }
    }
    public class YTMDesktopPlayerInfo
    {
        public bool hasSong { get; set; }
        public bool isPaused { get; set; }
        public int volumePercent { get; set; }
        public int seekbarCurrentPosition { get; set; }
        public string seekbarCurrentPositionHuman { get; set; }
        public double statePercent { get; set; }
        public string likeStatus { get; set; }
        public string repeatType { get; set; }
    }
}
