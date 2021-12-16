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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class HomeViewModel : RegionViewModelBase
    {
        private readonly IDirectoryService _directoryService;

        /// <summary>フォルダを開くコマンド</summary>
        public DelegateCommand<string> OpenFolderCommand { get; private set; }

        public HomeViewModel(IRegionManager regionManager, IDirectoryService directoryService) :
            base(regionManager)
        {
            // DI
            _directoryService = directoryService;

            // コマンドの設定
            OpenFolderCommand = new DelegateCommand<string>(OpenFolder);

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
            var outDir = $"./{ModuleSettings.Default.OutDirectory}";
            if ((!Directory.Exists("./Sample") && !Directory.Exists(excelDir) || !Directory.Exists(razorDir) || !Directory.Exists(outDir)))
            {
                MessageBox.Show($"初回起動なのでフォルダとサンプルデータを作成します。");
                _directoryService.SafeCreateDirectory(outDir);

                // サンプルデータを作成する
                // 埋め込みリソースから外に出す
                _directoryService.CopyResources(Assembly.GetExecutingAssembly());

                // 出力したサンプルファイルをフォルダごと移動する
                new DirectoryInfo($"./Sample/Razor").MoveTo(razorDir);
                new DirectoryInfo($"./Sample/Excel").MoveTo(excelDir);

                // Sampleフォルダを削除する
                Directory.Delete("./Sample", true);

                MessageBox.Show($"フォルダとサンプルデータを作成しました。");
            }
        }

        /// <summary>
        /// フォルダを開く
        /// </summary>
        private void OpenFolder(string folder)
        {
            string path = Environment.CurrentDirectory;
            System.Diagnostics.Process.Start("explorer.exe", $"{Path.Combine(path, folder)}");
        }
    }
}
