using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using DigitalMegaFlareOffline.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class ExcelViewModel : RegionViewModelBase
    {

        /// <summary>コマンド</summary>
        public DelegateCommand<long?> AaaaCommand { get; private set; }

        private ObservableCollection<ExcelItem> _excelItems;
        public ObservableCollection<ExcelItem> ExcelItems
        {
            get { return _excelItems; }
            set { SetProperty(ref _excelItems, value); }
        }

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

            ExcelItems = new ObservableCollection<ExcelItem>
            {
                new ExcelItem {Id=1, Description = "aaaa", Name = "bbbb", UpdatedDate = DateTime.Now },
                new ExcelItem {Id=2, Description = "cccc", Name = "dddd", UpdatedDate = DateTime.Now }
            };

            // コマンドの設定
            AaaaCommand = new DelegateCommand<long?>(Aaaa);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }

        /// <summary>
        /// ????
        /// </summary>
        private void Aaaa(long? Id)
        {
            var res = MessageBox.Show(
                $"IDは{Id}です。",
                "確認メッセージ",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel
                );
            if (res == MessageBoxResult.Cancel)
            {
                return;
            }

            MessageBox.Show($"サンプルデータを作成しました。");
        }
    }

    public class ExcelItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Description { get; set; }
    }
}
