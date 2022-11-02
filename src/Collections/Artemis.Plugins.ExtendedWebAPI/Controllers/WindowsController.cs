using Artemis.Core.Services;
using Artemis.UI.Services.Interfaces;
using Artemis.UI.Shared.Services.MainWindow;
using Avalonia.Threading;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace Artemis.Plugins.ExtendedWebAPI.Controllers
{
    internal class WindowsController : WebApiController
    {
        private readonly IDebugService _debugService;
        private readonly IMainWindowService _mainWindowService;

        private readonly IPluginManagementService _pluginManagementService;

        public WindowsController(
            IDebugService debugService,
            IPluginManagementService pluginManagementService,
            IMainWindowService mainWindowService)
        {
            _debugService = debugService;
            _pluginManagementService = pluginManagementService;
            _mainWindowService = mainWindowService;
        }

        [Route(HttpVerbs.Any, "/extended-rest-api/version")]
        public string GetVersion()
        {
            HttpContext.Response.ContentType = " text/plain";
            string version = _pluginManagementService.GetCallingPlugin().Info.Version.ToString();
            return version;
        }


        [Route(HttpVerbs.Any, "/windows/show-debugger")]
        public void PostShowDebugger()
        {
            Dispatcher.UIThread.InvokeAsync(() => _debugService.ShowDebugger());
        }

        [Route(HttpVerbs.Any, "/windows/show-workshop")]
        public void PostShowWorkshop()
        {

            //_mainWindowService.OpenMainWindow();
            // RootViewModel rootViewModel = _kernel.Get<RootViewModel>();
            // rootViewModel.OpenScreen("Workshop");
            //_windowService.OpenMainWindow();
            //Dispatcher.UIThread.InvokeAsync(() =>
            //{
            //    Type type = typeof(ArtemisBootstrapper);
             //   FieldInfo info = type.GetField("_kernel", BindingFlags.NonPublic | BindingFlags.Static);
            //    StandardKernel standardKernel = (StandardKernel)info.GetValue(null);
            //    var rm = standardKernel.Get<RootViewModel>();
            //    rm.OpenScreen("Workshop");
            //});
        }

        [Route(HttpVerbs.Any, "/windows/show-surface-editor")]
        public void PostShowSurfaceEditor()
        {
            //_windowService.OpenMainWindow();
            //Execute.PostToUIThread(() => _eventAggregator.Publish(new RequestSelectSidebarItemEvent("Surface Editor")));
            //_windowService.ShowWindow();

        }

        [Route(HttpVerbs.Any, "/windows/show-settings")]
        public void PostShowSettings()
        {
            //_windowService.OpenMainWindow();
            //Execute.PostToUIThread(() => _eventAggregator.Publish(new RequestSelectSidebarItemEvent("Settings")));
        }
    }
}