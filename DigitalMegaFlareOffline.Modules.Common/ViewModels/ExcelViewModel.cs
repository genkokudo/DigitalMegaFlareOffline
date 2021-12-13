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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class ExcelViewModel : RegionViewModelBase
    {
        private readonly IDirectoryService _directoryService;
        private readonly IExcelService _excelService;

        /// <summary>生成コマンド</summary>
        public DelegateCommand<long?> GenerateCommand { get; private set; }
        /// <summary>Excelを開くコマンド</summary>
        public DelegateCommand<long?> OpenCommand { get; private set; }
        /// <summary>削除コマンド</summary>
        public DelegateCommand<long?> DeleteCommand { get; private set; }
        /// <summary>新規作成コマンド</summary>
        public DelegateCommand CreateCommand { get; private set; }
        /// <summary>名前変更コマンド</summary>
        public DelegateCommand<long?> RenameCommand { get; private set; }

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
            GenerateCommand = new DelegateCommand<long?>(Generate);
            OpenCommand = new DelegateCommand<long?>(Open);
            DeleteCommand = new DelegateCommand<long?>(Delete);
            SetIsEnableNameCommand = new DelegateCommand(SetIsEnableName);
            CreateCommand = new DelegateCommand(Create);
            RenameCommand = new DelegateCommand<long?>(Rename);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Init();
        }

        private void Init()
        {
            IsEnableName = false;
            InputFileName = string.Empty;
            Reload();
        }

        /// <summary>
        /// 新規作成
        /// </summary>
        private void Create()
        {
            MessageBox.Show($"{InputFileName}");
        }

        /// <summary>
        /// 名前変更
        /// </summary>
        private void Rename(long? Id)
        {
            var filename = $"{InputFileName}.xlsx";
            var target = ExcelItems.First(x => x.Id == Id);
            var parentDir = new DirectoryInfo(target.FullPath).Parent.FullName;
            File.Move(target.FullPath, Path.Combine(parentDir, filename));

            Init();   // 表示更新
        }

        /// <summary>
        /// ソース生成
        /// </summary>
        private void Generate(long? Id)
        {
            var res = MessageBox.Show(
                $"？",
                "確認メッセージ",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel
                );
            if (res == MessageBoxResult.Cancel)
            {
                return;
            }
        }

        /// <summary>
        /// ファイルを削除する
        /// </summary>
        private void Delete(long? Id)
        {
            var res = MessageBox.Show(
                $"本当に削除しますか？",
                "確認メッセージ",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel
                );
            if (res == MessageBoxResult.Cancel)
            {
                return;
            }
            var target = ExcelItems.First(x => x.Id == Id);
            File.Delete(target.FullPath);
            ExcelItems.Remove(target);
        }

        /// <summary>
        /// Excelを開く
        /// </summary>
        private void Open(long? Id)
        {
            // Excelブックを開く
            var xlApp = new Microsoft.Office.Interop.Excel.Application();
            var xlBooks = xlApp.Workbooks;
            var xlBook = xlBooks.Open(ExcelItems.First(x => x.Id == Id).FullPath);
            xlApp.Visible = true;

            // Excelアプリケーションを解放
            Marshal.ReleaseComObject(xlBook);
            Marshal.ReleaseComObject(xlBooks);
            Marshal.ReleaseComObject(xlApp);
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

            // 存在しないファイル名であること
            var excelDir = $"./{ModuleSettings.Default.ExcelDirectory}";
            IsEnableName = !File.Exists(Path.Combine(excelDir, $"{InputFileName.ToLower()}.xlsx"));
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
