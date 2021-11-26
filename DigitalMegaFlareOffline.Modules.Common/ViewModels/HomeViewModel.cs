using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using DigitalMegaFlareOffline.Services;
using MithrilCube.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class HomeViewModel : RegionViewModelBase
    {
        private readonly IDirectoryService _directoryService;

        /// <summary>サンプルデータを作成するコマンド</summary>
        public DelegateCommand MakeSampleCommand { get; private set; }

        // サンプルデータ作成ボタンが有効か
        private bool _isEnableSampleButton;
        public bool IsEnableSampleButton
        {
            get { return _isEnableSampleButton; }
            set { SetProperty(ref _isEnableSampleButton, value); }
        }
        

        public HomeViewModel(IRegionManager regionManager, IDirectoryService directoryService) :
            base(regionManager)
        {
            // DI
            _directoryService = directoryService;

            // コマンドの設定
            MakeSampleCommand = new DelegateCommand(MakeSample);

            // TODO:サンプルデータ作成済みかを判定する
            IsEnableSampleButton = true;

            // 起動時処理
            // データフォルダが無ければ作成する
            CheckDataFolder();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// 必要なフォルダの存在確認を行い、なければ作成する
        /// </summary>
        private void CheckDataFolder()
        {
            var excelDir = $"./{ModuleSettings.Default.ExcelDirectory}";
            var razorDir = $"./{ModuleSettings.Default.RazorDirectory}";
            if (!Directory.Exists(excelDir) || !Directory.Exists(razorDir))
            {
                MessageBox.Show($"初回起動なのでフォルダを作成します。");
                _directoryService.SafeCreateDirectory(excelDir);
                _directoryService.SafeCreateDirectory(razorDir);
                MessageBox.Show($"フォルダを作成しました。");
            }
        }

        /// <summary>
        /// サンプルデータを作成する
        /// </summary>
        private void MakeSample()
        {
            var res = MessageBox.Show(
                "サンプルデータを作成します。",
                "確認メッセージ",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel
                );
            if (res == MessageBoxResult.Cancel)
            {
                return;
            }

            // フォルダチェック・作成
            CheckDataFolder();

            // TODO:サンプルデータを作成する

            // 作成完了
            IsEnableSampleButton = false;
            MessageBox.Show($"サンプルデータを作成しました。");
        }
    }
}
