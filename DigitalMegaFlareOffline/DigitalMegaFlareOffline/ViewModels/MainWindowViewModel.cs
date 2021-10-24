using DigitalMegaFlareOffline.Modules.Common.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace DigitalMegaFlareOffline.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        /// <summary>画面遷移するコマンド</summary>
        public DelegateCommand<string> NavigateCommand { get; private set; }
        private readonly IRegionManager _regionManager;

        private string _title = "DigitalMegaFlareOffline";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            // 初期画面指定
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Home));

            // コマンド設定
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        /// <summary>
        /// 画面遷移する
        /// </summary>
        /// <param name="navigatePath">View名</param>
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate(RegionNames.ContentRegion, navigatePath);
        }
    }
}
