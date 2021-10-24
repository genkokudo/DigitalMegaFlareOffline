using DigitalMegaFlareOffline.Core;
using DigitalMegaFlareOffline.Modules.Common.Views;
using Prism.Mvvm;
using Prism.Regions;

namespace DigitalMegaFlareOffline.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "DigitalMegaFlareOffline";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            // 初期画面指定
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ViewA));
        }
    }
}
