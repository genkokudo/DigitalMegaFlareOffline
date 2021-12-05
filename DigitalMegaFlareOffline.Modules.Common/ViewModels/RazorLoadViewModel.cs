using DigitalMegaFlareOffline.Modules.Common.Models;
using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using DigitalMegaFlareOffline.Services;
using MithrilCube.Prism;
using MithrilCube.Prism.Services;
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

// 課題
// ・Treeの選択状態を解除したいけど、ここから制御できないのでは？
// https://stackoverflow.com/questions/7153813/wpf-mvvm-treeview-selecteditem
// https://stackoverflow.com/questions/9143107/get-selected-treeviewitem-using-mvvm


// ・全体再取得すると、開閉状態が消える。本当に全体再取得でいいの？

// ・nullの時にボタンを無効にしたい
// ・https://stackoverflow.com/questions/51162551/disable-button-if-value-is-null-via-databinding
// ・https://stackoverflow.com/questions/7140662/isenabled-false-if-binding-source-is-not-available
// ↑DataTriggerが複数の場合はMultiDataTriggerを使う、またはDataConverterを作って頑張る

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class RazorLoadViewModel : RegionViewModelBase
    {
        private readonly IWpfDirectoryService _wpfDirectoryService;

        /// <summary>空のファイルを作成するコマンド</summary>
        public DelegateCommand MakeFileCommand { get; private set; }
        /// <summary>フォルダを作成するコマンド</summary>
        public DelegateCommand MakeFolderCommand { get; private set; }
        /// <summary>ファイルを編集するコマンド</summary>
        public DelegateCommand EditCommand { get; private set; }
        /// <summary>ファイル・フォルダを作成するコマンド</summary>
        public DelegateCommand DeleteCommand { get; private set; }
        /// <summary>ツリー選択時コマンド</summary>
        public DelegateCommand<TreeSource<FileData>> TreeSelectCommand { get; private set; }

        /// <summary>
        /// 現在選択中のファイル
        /// 初期及び何か処理をした時はnull
        /// </summary>
        private FileData _selectedFileData;
        public FileData SelectedFileData
        {
            get { return _selectedFileData; }
            set { SetProperty(ref _selectedFileData, value); }
        }

        // TODO:フォルダを選択している場合true
        // 作成ボタンが有効か
        private bool _isEnableMakeButton;
        public bool IsEnableMakeButton
        {
            get { return _isEnableMakeButton; }
            set { SetProperty(ref _isEnableMakeButton, value); }
        }

        // TODO:ファイル（＝子なし項目）を選択している場合true
        // 編集ボタンが有効か
        private bool _isEnableEditButton;
        public bool IsEnableEditButton
        {
            get { return SelectedFileData != null; }
            set { SetProperty(ref _isEnableEditButton, value); }
        }

        // TODO:ファイルまたはフォルダを選択している場合true
        // 削除ボタンが有効か
        private bool _isEnableDeleteButton;
        public bool IsEnableDeleteButton
        {
            get { return _isEnableDeleteButton; }
            set { SetProperty(ref _isEnableDeleteButton, value); }
        }

        // Razorフォルダ階層データ
        // ツリーのデータ
        private FileTree _treeRoot;
        public FileTree TreeRoot
        {
            get { return _treeRoot; }
            set { SetProperty(ref _treeRoot, value); }
        }

        public RazorLoadViewModel(IRegionManager regionManager, IWpfDirectoryService wpfDirectoryService) :
            base(regionManager)
        {
            // DI
            _wpfDirectoryService = wpfDirectoryService;

            // コマンドの設定
            MakeFileCommand = new DelegateCommand(MakeFile);
            MakeFolderCommand = new DelegateCommand(MakeFolder);
            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);
            TreeSelectCommand = new DelegateCommand<TreeSource<FileData>>(TreeSelect);

            // ボタンの状態
            IsEnableMakeButton = false;
            IsEnableEditButton = false;
            IsEnableDeleteButton = false;

            // ディレクトリ階層を読み込み
            var razorDir = $"./{ModuleSettings.Default.RazorDirectory}";
            TreeRoot = _wpfDirectoryService.GetDirectoryFileTree(razorDir);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// ファイルまたはフォルダを選択
        /// </summary>
        private void TreeSelect(TreeSource<FileData> selectedFile)
        {
            SelectedFileData = selectedFile.Value;

            IsEnableMakeButton = SelectedFileData != null;
        }

        /// <summary>
        /// 空のファイルを作成する
        /// </summary>
        private void MakeFile()
        {
            // ファイルを作成する

            // 作成完了
            SelectedFileData = null;
            IsEnableMakeButton = SelectedFileData != null;
        }

        /// <summary>
        /// フォルダを作成する
        /// </summary>
        private void MakeFolder()
        {
            SelectedFileData = null;
            IsEnableMakeButton = SelectedFileData != null;
        }

        /// <summary>
        /// 編集画面へ遷移する
        /// ファイル情報を編集画面へ渡す
        /// </summary>
        private void Edit()
        {
            // 遷移処理
            var param = new NavigationParameters
            {
                //{ "SnippetFullPath", Snippet.FullPath }     // TODO:何を渡すべき？フルパスだけで良い？→クラスごと渡せば安心。
            };
            RegionNavigation.RequestNavigate(ViewNames.ViewRazorEdit, param);

            // TODO:同一Region内で画面遷移をしないので、IRegionNavigationServiceにすること。RegionViewModelBaseを変更すること。
        }

        /// <summary>
        /// ファイル・フォルダを削除する
        /// </summary>
        private void Delete()
        {
            IsEnableMakeButton = false;
            IsEnableEditButton = false;
            IsEnableDeleteButton = false;

            SelectedFileData = null;
        }
    }
}
