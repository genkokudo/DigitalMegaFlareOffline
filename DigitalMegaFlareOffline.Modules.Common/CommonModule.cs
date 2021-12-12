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
            containerRegistry.RegisterForNavigation<Labo>();
            containerRegistry.RegisterForNavigation<RazorLoad>();
        }
    }

    /// <summary>
    /// このモジュールに含まれるView名を定数にしておく
    /// </summary>
    public static class ViewNames
    {
        public const string ViewConfig = "Config";
        public const string ViewExcel = "Excel";
        public const string ViewHome = "Home";
        public const string ViewRazorLoad = "RazorLoad";
        public const string ViewLabo = "Labo";
    }
}