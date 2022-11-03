using Newtonsoft.Json;
using RestSharp;

namespace Artemis.Plugins.DataModelExpansions.YTMdesktop
{

    // ReSharper disable once InconsistentNaming
    public class YTMDesktopClient
    {
        readonly RestClient _client;
        readonly RestRequest _queryRootInfoRequest;

        RootInfo _rootInfo;

        public YTMDesktopClient()
        {
            _client = new RestClient("http://127.0.0.1:9863");
            _queryRootInfoRequest = new RestRequest("query", Method.GET);
        }

        public RootInfo Data => _rootInfo;

        public void Update()
        {
            var response = _client.Execute(_queryRootInfoRequest);
            _rootInfo = JsonConvert.DeserializeObject<RootInfo>(response.Content);
        }
    }

    public class PlayerInfo
    {
        public bool HasSong { get; set; }
        public bool IsPaused { get; set; }
        public float VolumePercent { get; set; }
        public int SeekbarCurrentPosition { get; set; }
        public string SeekbarCurrentPositionHuman { get; set; }
        public double StatePercent { get; set; }
        public string LikeStatus { get; set; }
        public string RepeatType { get; set; }
    }

    public class TrackInfo
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Cover { get; set; }
        public int Duration { get; set; }
        public string DurationHuman { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public bool IsVideo { get; set; }
        public bool IsAdvertisement { get; set; }
        public bool InLibrary { get; set; }
    }

    public class RootInfo
    {
        public PlayerInfo Player { get; set; }
        public TrackInfo Track { get; set; }
    }
}
