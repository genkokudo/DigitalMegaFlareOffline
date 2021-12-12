using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using DigitalMegaFlareOffline.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 実験・仮実装
namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class LaboViewModel : RegionViewModelBase
    {
        //private string _message;
        //public string Message
        //{
        //    get { return _message; }
        //    set { SetProperty(ref _message, value); }
        //}

        public LaboViewModel(IRegionManager regionManager) :
            base(regionManager)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}
