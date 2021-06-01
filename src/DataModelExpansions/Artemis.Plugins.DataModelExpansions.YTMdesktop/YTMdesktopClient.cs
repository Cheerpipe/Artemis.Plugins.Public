using Newtonsoft.Json;
using RestSharp;

namespace Artemis.Plugins.DataModelExpansions.YTMdesktop
{

    public class YTMDesktopClient
    {
        RestClient _client;
        RestRequest _queryRootInfoRequest;

        RootInfo _rootInfo;

        public YTMDesktopClient()
        {
            _client = new RestClient("http://127.0.0.1:9863");
            _queryRootInfoRequest = new RestRequest("query", Method.GET);
        }

        public RootInfo Data => _rootInfo;

        public void Update()
        {

            try
            {
                var response = _client.Execute(_queryRootInfoRequest);
                _rootInfo = JsonConvert.DeserializeObject<RootInfo>(response.Content);
            }
            catch
            {
                _rootInfo = null;
            }
        }
    }

    public class PlayerInfo
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

    public class TrackInfo
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

    public class RootInfo
    {
        public PlayerInfo player { get; set; }
        public TrackInfo track { get; set; }
    }
}
