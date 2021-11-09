using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using DigitalMegaFlareOffline.Services;
using MithrilCube.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class RazorLoadViewModel : RegionViewModelBase
    {
        public ObservableCollection<TreeSource> TreeRoot { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public RazorLoadViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            Message = messageService.GetMessage() + GetType().Name;

            TreeRoot = new ObservableCollection<TreeSource>();
            var item1 = new TreeSource() { Text = "Item1", IsExpanded = true };
            var item11 = new TreeSource() { Text = "Item1-1", IsExpanded = true };
            var item12 = new TreeSource() { Text = "Item1-2", IsExpanded = true };
            var item2 = new TreeSource() { Text = "Item2", IsExpanded = false };
            var item21 = new TreeSource() { Text = "Item2-1", IsExpanded = true };
            var item211 = new TreeSource() { Text = "Item2-1-1", IsExpanded = true };
            var item212 = new TreeSource() { Text = "Item2-1-2", IsExpanded = true };
            TreeRoot.Add(item1);
            TreeRoot.Add(item2);
            item1.Add(item11);
            item1.Add(item12);
            item2.Add(item21);
            item21.Add(item211);
            item21.Add(item212);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
