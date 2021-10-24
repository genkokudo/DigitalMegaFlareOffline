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

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class ExcelViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ExcelViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            Message = messageService.GetMessage() + GetType().Name;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
