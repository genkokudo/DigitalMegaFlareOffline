using Prism.Regions;
using System;

namespace DigitalMegaFlareOffline.Modules.Common.Mvvm
{
    public class RegionViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest
    {
        // TODO:このプログラムは他のRegionを指定することは無いので、要らないと思う
        protected IRegionManager RegionManager { get; private set; }

        /// <summary>同一のRegionで画面遷移をするサービス</summary>
        protected IRegionNavigationService RegionNavigation { get; private set; }

        public RegionViewModelBase(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            RegionNavigation = navigationContext.NavigationService;
        }
    }
}
