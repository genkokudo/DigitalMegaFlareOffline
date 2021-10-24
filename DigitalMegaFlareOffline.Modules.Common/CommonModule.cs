using DigitalMegaFlareOffline.Modules.Common.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace DigitalMegaFlareOffline.Modules.Common
{
    public class CommonModule: IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Config>();
            containerRegistry.RegisterForNavigation<Excel>();
            containerRegistry.RegisterForNavigation<Home>();
            containerRegistry.RegisterForNavigation<RazorEdit>();
            containerRegistry.RegisterForNavigation<RazorLoad>();
        }
    }

    /// <summary>
    /// このモジュールに含まれるView名を定数にしておく
    /// </summary>
    public static class ViewNames
    {
        public const string ViewHome = "Home";
    }
}