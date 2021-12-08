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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// 課題
// ■全体再取得すると、開閉状態が消える。本当に全体再取得でいいの？

// まず、追加した時にツリー表示反映するか確認。それから考える。



namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class RazorLoadViewModel : RegionViewModelBase
    {
        private readonly IWpfDirectoryService _wpfDirectoryService;

        /// <summary>空のファイルを作成するコマンド</summary>
        public DelegateCommand MakeFileCommand { get; private set; }
        /// <summary>フォルダを作成するコマンド</summary>
        public DelegateCommand MakeDirectoryCommand { get; private set; }
        /// <summary>名前を変更するコマンド</summary>
        public DelegateCommand RenameCommand { get; private set; }
        /// <summary>ファイルを編集するコマンド</summary>
        public DelegateCommand EditCommand { get; private set; }
        /// <summary>ファイル・フォルダを作成するコマンド</summary>
        public DelegateCommand DeleteCommand { get; private set; }
        /// <summary>ファイル・フォルダ名入力時のコマンド</summary>
        public DelegateCommand TextChangedCommand { get; private set; }
        /// <summary>ツリー選択時コマンド</summary>
        public DelegateCommand<TreeSource<FileData>> TreeSelectCommand { get; private set; }

        /// <summary>
        /// 現在選択中のファイル
        /// 初期及び何か処理をした時はnull
        /// </summary>
        private TreeSource<FileData> _selectedFileData;
        public TreeSource<FileData> SelectedFile
        {
            get { return _selectedFileData; }
            set { SetProperty(ref _selectedFileData, value); }
        }

        // 入力中のファイル・フォルダ名
        private string _fileOrFolderName;
        public string FileOrFolderName
        {
            get { return _fileOrFolderName; }
            set { SetProperty(ref _fileOrFolderName, value); CheckAvailableName(); }
        }

        // ファイル作成ボタンが有効か
        private bool _isEnableMakeFileButton;
        public bool IsEnableMakeFileButton
        {
            get { return _isEnableMakeFileButton; }
            set { SetProperty(ref _isEnableMakeFileButton, value); }
        }

        // フォルダ作成ボタンが有効か
        private bool _isEnableMakeDirectoryButton;
        public bool IsEnableMakeDirectoryButton
        {
            get { return _isEnableMakeDirectoryButton; }
            set { SetProperty(ref _isEnableMakeDirectoryButton, value); }
        }

        // 名前変更ボタンが有効か
        private bool _isEnableChangeButton;
        public bool IsEnableChangeButton
        {
            get { return _isEnableChangeButton; }
            set { SetProperty(ref _isEnableChangeButton, value); }
        }

        // 編集ボタンが有効か
        private bool _isEnableEditButton;
        public bool IsEnableEditButton
        {
            get { return _isEnableEditButton; }
            set { SetProperty(ref _isEnableEditButton, value); }
        }

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
            MakeDirectoryCommand = new DelegateCommand(MakeDirectory);
            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);
            TreeSelectCommand = new DelegateCommand<TreeSource<FileData>>(TreeSelect);
            TextChangedCommand = new DelegateCommand(TextChanged);
            RenameCommand = new DelegateCommand(Rename);

            // 画面の状態
            IsEnableMakeFileButton = false;
            IsEnableMakeDirectoryButton = false;
            IsEnableEditButton = false;
            IsEnableDeleteButton = false;
            IsEnableChangeButton = false;
            _fileOrFolderName = string.Empty;

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
            SelectedFile = selectedFile;

            // ボタン有効化
            CheckAvailableName();
        }

        /// <summary>
        /// ファイル名と、選択中のディレクトリでボタンを有効にするか判断する
        /// </summary>
        /// <returns></returns>
        private void CheckAvailableName()
        {
            IsEnableDeleteButton = true;
            IsEnableEditButton = !SelectedFile.Value.IsDirectory;

            if (string.IsNullOrWhiteSpace(FileOrFolderName))
            {
                IsEnableMakeFileButton = false;
                IsEnableMakeDirectoryButton = false;
                IsEnableChangeButton = false;
                return;
            }

            if (SelectedFile.Value.IsDirectory)
            {
                // ディレクトリを選択している場合、その中に指定された名前が無いこと
                IsEnableMakeFileButton = !File.Exists(Path.Combine(SelectedFile.Value.FullPath, $"{FileOrFolderName}.razor"));
                IsEnableMakeDirectoryButton = !Directory.Exists(Path.Combine(SelectedFile.Value.FullPath, $"{FileOrFolderName}"));

                // 名前変更ボタン：同一ディレクトリに同名のディレクトリが無いこと
                IsEnableChangeButton = !Directory.Exists(Path.Combine(Path.GetDirectoryName(SelectedFile.Value.FullPath), $"{FileOrFolderName}"));
            }
            else
            {
                // ファイルを選択している場合、同一ディレクトリ内に指定された名前が無いこと
                IsEnableMakeFileButton = !File.Exists(Path.Combine(Path.GetDirectoryName(SelectedFile.Value.FullPath), $"{FileOrFolderName}.razor"));
                IsEnableMakeDirectoryButton = !Directory.Exists(Path.Combine(Path.GetDirectoryName(SelectedFile.Value.FullPath), $"{FileOrFolderName}"));

                // 名前変更ボタン：同一ディレクトリに同名のファイルが無いこと
                IsEnableChangeButton = !File.Exists(Path.Combine(Path.GetDirectoryName(SelectedFile.Value.FullPath), $"{FileOrFolderName}.razor"));
            }
        }

        /// <summary>
        /// 空のファイルを作成する
        /// </summary>
        private void MakeFile()
        {
            if (SelectedFile.Value.IsDirectory)
            {
                File.Create(Path.Combine(SelectedFile.Value.FullPath, $"{FileOrFolderName}.razor"));
            }
            else
            {
                File.Create(Path.Combine(Path.GetDirectoryName(SelectedFile.Value.FullPath), $"{FileOrFolderName}.razor"));
            }
            MessageBox.Show("ファイルを作成する");

            // TODO:ツリー表示を更新（どうやって？）

            // ボタン更新
            CheckAvailableName();
        }

        /// <summary>
        /// 空のフォルダを作成する
        /// </summary>
        private void MakeDirectory()
        {
            if (SelectedFile.Value.IsDirectory)
            {
                Directory.CreateDirectory(Path.Combine(SelectedFile.Value.FullPath, $"{FileOrFolderName}"));
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(SelectedFile.Value.FullPath), $"{FileOrFolderName}"));
            }
            MessageBox.Show("フォルダを作成する");

            // TODO:ツリー表示を更新（どうやって？）

            // ボタン更新
            CheckAvailableName();
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

        }

        /// <summary>
        /// ファイル・フォルダを削除する
        /// </summary>
        private void Delete()
        {
            var res = MessageBox.Show(
                "本当に削除してしまいますが\nよろしいですか？",
                "確認メッセージ",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel
                );
            if (res == MessageBoxResult.Cancel)
            {
                return;
            }

            if (SelectedFile.Value.IsDirectory)
            {
                // ディレクトリ削除
                Directory.Delete(SelectedFile.Value.FullPath, true);
            }
            else
            {
                // ファイル削除
                File.Delete(SelectedFile.Value.FullPath);
            }
            // TODO:表示更新を実装

            IsEnableMakeFileButton = false;
            IsEnableMakeDirectoryButton = false;
            IsEnableEditButton = false;
            IsEnableDeleteButton = false;
            IsEnableChangeButton = false;
            //CheckAvailableName();     // TODO:表示更新を実装したら、なるべくこっちで制御すること

            SelectedFile = null;
            MessageBox.Show("削除しました。", "結果", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// ファイル・フォルダの名前を変更する
        /// </summary>
        private void Rename()
        {
            // 多分、選択状態が保たれないので、選択し直すこと（IsSelected=true）
        }

        // ファイル・フォルダ名の入力時
        private void TextChanged()
        {
            // FileOrFolderName
        }
    }
}
