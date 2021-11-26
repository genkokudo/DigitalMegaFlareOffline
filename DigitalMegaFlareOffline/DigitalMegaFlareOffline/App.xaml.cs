using DigitalMegaFlareOffline.Modules.Common;
using DigitalMegaFlareOffline.Services;
using DigitalMegaFlareOffline.Views;
using MithrilCube.Services;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace DigitalMegaFlareOffline
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.Register<IDirectoryService, DirectoryService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CommonModule>();
        }
    }
}
