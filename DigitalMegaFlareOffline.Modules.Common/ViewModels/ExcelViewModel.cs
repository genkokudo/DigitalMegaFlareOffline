using DigitalMegaFlareOffline.Modules.Common.Models;
using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using DigitalMegaFlareOffline.Services;
using MithrilCube.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class ExcelViewModel : RegionViewModelBase
    {
        private readonly IDirectoryService _directoryService;
        private readonly IExcelService _excelService;

        // TODO:行ごとの処理：「開く」「生成」「削除」「名前変更」
        /// <summary>コマンド</summary>
        public DelegateCommand<long?> AaaaCommand { get; private set; }

        /// <summary>名前変更・Excel作成ボタンの有効化を判断するコマンド</summary>
        public DelegateCommand SetIsEnableNameCommand { get; private set; }

        // 出力ボタンが有効であるか
        private bool _isEnableName;
        public bool IsEnableName
        {
            get { return _isEnableName; }
            set { SetProperty(ref _isEnableName, value); }
        }

        // 入力したファイル名
        private string _inputFileName;
        public string InputFileName
        {
            get { return _inputFileName; }
            set { SetProperty(ref _inputFileName, value); }
        }
        
        // Excel一覧
        private ObservableCollection<ExcelItem> _excelItems;
        public ObservableCollection<ExcelItem> ExcelItems
        {
            get { return _excelItems; }
            set { SetProperty(ref _excelItems, value); }
        }

        public ExcelViewModel(IRegionManager regionManager, IDirectoryService directoryService, IExcelService excelService) :
            base(regionManager)
        {
            // DI
            _directoryService = directoryService;
            _excelService = excelService;

            // コマンドの設定
            AaaaCommand = new DelegateCommand<long?>(Aaaa);
            SetIsEnableNameCommand = new DelegateCommand(SetIsEnableName);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsEnableName = false;
            InputFileName = string.Empty;
            Reload();
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

            //MessageBox.Show($"なにもしませんでした。");

            // Excelブックを開く
            var xlApp = new Microsoft.Office.Interop.Excel.Application();
            var xlBooks = xlApp.Workbooks;
            var xlBook = xlBooks.Open(ExcelItems.First(x => x.Id == Id).FullPath);
            xlApp.Visible = true;

        }

        /// <summary>
        /// 入力した名前が有効か判定する
        /// </summary>
        private void SetIsEnableName()
        {
            if (string.IsNullOrWhiteSpace(InputFileName))
            {
                IsEnableName = false;
                return;
            }

            // TODO:Excel一覧に無い名前であること
            IsEnableName = true;

            //ExcelItems.Any(x => x.Name.Trim(".xlsx") == InputFileName);
        }

        /// <summary>
        /// Excelフォルダ内を読んで、一覧を作成する
        /// </summary>
        private void Reload()
        {
            var excelDir = $"./{ModuleSettings.Default.ExcelDirectory}";

            // 指定拡張子のファイルを全て探す
            var fileList = _directoryService.FolderInsiteSearch(excelDir, new string[] { ".xlsx" });

            var id = 0;
            ExcelItems = new ObservableCollection<ExcelItem>();
            foreach (var filePath in fileList)
            {
                // Excelを読み込んで、"A1"を読み取る
                id++;
                var description = _excelService.GetCell(filePath);
                ExcelItems.Add(new ExcelItem { Id = id, FullPath = Path.GetFullPath(filePath), UpdatedDate = File.GetLastWriteTime(filePath), Name = Path.GetFileName(filePath), Description = description });
            }




        }
    }

}
