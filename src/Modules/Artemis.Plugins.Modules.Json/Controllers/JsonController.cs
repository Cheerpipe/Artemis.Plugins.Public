using System.Collections.Generic;
using System.Text;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System.Threading.Tasks;
using Artemis.Plugins.Modules.Json.Services.JsonDataModelServices;

namespace Artemis.Plugins.Modules.Json.Controllers
{
    public class JsonController : WebApiController
    {
        private readonly JsonDataModelServices _jsonDataModelServices;

        public JsonController(JsonDataModelServices jsonDataModelServices)
        {
            _jsonDataModelServices = jsonDataModelServices;
        }

        [Route(HttpVerbs.Get, "/json-datamodel/{key}")]
        public async Task GetDataModelByKey(string key)
        {
            if (_jsonDataModelServices.TryGetJsonByKey(key, out string json))
            {
                HttpContext.Response.ContentType = "application/json";
                await using var writer = HttpContext.OpenResponseText(new UTF8Encoding(false));
                await writer.WriteAsync(json);
            }
            else
            {
                throw HttpException.NotFound($"Json datamodel with key {key} not found");
            }
        }

        [Route(HttpVerbs.Delete, "/json-datamodel/{key}")]
        public string RemoveDataModelByKey(string key)
        {
            if (!_jsonDataModelServices.RemoveByKey(key))
            {
                throw HttpException.NotFound($"Json datamodel with key {key} not found");
            }
            else
            {
                return $"Json datamodel with key {key} removed";
            }
        }

        [Route(HttpVerbs.Post, "/json-datamodel/{key}")]
        public async Task AddOrReplaceJson(string key)
        {
            var json = await HttpContext.GetRequestBodyAsStringAsync();
            HttpContext.Response.ContentType = "application/json";
            _jsonDataModelServices.AddOrReplaceJson(key, json, true);
            await using var writer = HttpContext.OpenResponseText(new UTF8Encoding(false));
            await writer.WriteAsync(json);
        }

        [Route(HttpVerbs.Put, "/json-datamodel/{key}")]
        public async Task AddOrMergeJson(string key)
        {
            var json = await HttpContext.GetRequestBodyAsStringAsync();
            HttpContext.Response.ContentType = "application/json";
            _jsonDataModelServices.AddOrMergeJson(key, json, true);
            await using var writer = HttpContext.OpenResponseText(new UTF8Encoding(false));
            await writer.WriteAsync(json);
        }

        [Route(HttpVerbs.Get, "/json-datamodel")]
        public IEnumerable<string> GetDataModels()
        {
            HttpContext.Response.ContentType = "application/json";
            return _jsonDataModelServices.GetDataModelsKeys();
        }
    }
}
