using System.Runtime.CompilerServices;
using System.Threading;
using Artemis.Core.Services;
using Artemis.UI.Events;
using Artemis.UI.Services;
using Artemis.UI.Shared.Services;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Stylet;

namespace Artemis.Plugins.ExtendedWebAPI.Controllers
{
    internal class WindowsController : WebApiController
    {

        private readonly IWindowService _windowService;
        private readonly IDebugService _debugService;
        private readonly IEventAggregator _eventAggregator;

        public WindowsController(IWindowService windowService, IWebServerService webServerService, IDebugService debugService, IEventAggregator eventAggregator)
        {
            _windowService = windowService;
            _debugService = debugService;
            _eventAggregator = eventAggregator;
        }

        [Route(HttpVerbs.Post, "/windows/show-debugger")]
        public void PostShowDebugger()
        {
            Execute.PostToUIThread(() => _debugService.ShowDebugger());
        }

        [Route(HttpVerbs.Post, "/windows/show-workshop")]
        public void PostShowWorkshop()
        {
            _windowService.OpenMainWindow();
            Execute.PostToUIThread(() => _eventAggregator.Publish(new RequestSelectSidebarItemEvent("Workshop")));
        }

        [Route(HttpVerbs.Post, "/windows/show-surface-editor")]
        public void PostShowSurfaceEditor()
        {
            _windowService.OpenMainWindow();
            Execute.PostToUIThread(() => _eventAggregator.Publish(new RequestSelectSidebarItemEvent("Surface Editor")));
            
        }

        [Route(HttpVerbs.Post, "/windows/show-settings")]
        public void PostShowSettings()
        {
            _windowService.OpenMainWindow();
            Execute.PostToUIThread(() => _eventAggregator.Publish(new RequestSelectSidebarItemEvent("Settings")));
        }
    }
}