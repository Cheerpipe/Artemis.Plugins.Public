using Artemis.Core.Modules;
using Artemis.Core.Services;
using Artemis.Plugins.Modules.Json.Controllers;
using Artemis.Plugins.Modules.Json.DataModels;
using System.Collections.Generic;
using Artemis.Plugins.Modules.Json.Services.JsonDataModelServices;

namespace Artemis.Plugins.Modules.Json
{
    public class Json : Module<JsonDataModel>
    {
        private readonly IWebServerService _webServerService;
        private readonly JsonDataModelServices _jsonDataModelServices;
        private WebApiControllerRegistration _webApiControllerRegistration;

        public Json(JsonDataModelServices jsonDataModelServices, IWebServerService webServerService)
        {
            _jsonDataModelServices = jsonDataModelServices;
            _webServerService = webServerService;
        }

        public override List<IModuleActivationRequirement> ActivationRequirements => null;

        public override void Enable()
        {
            _jsonDataModelServices.Initialize(DataModel);
            _jsonDataModelServices.LoadFromRepository();
            _webApiControllerRegistration = _webServerService.AddController<JsonController>(this);
        }

        public override void Disable()
        {
            _webServerService.RemoveController(_webApiControllerRegistration);
        }
        public override void Update(double deltaTime) { }
    }
}