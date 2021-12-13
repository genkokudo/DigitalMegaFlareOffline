using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using MithrilCubeWpf.Prism;
using MithrilCubeWpf.Prism.Services;
using Prism.Commands;
using Prism.Regions;
using System.IO;
using System.Windows;

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
        /// <summary>ファイルを更新するコマンド</summary>
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
        public TreeSource<FileData> SelectedFileOrDirectory
        {
            get { return _selectedFileData; }
            set { SetProperty(ref _selectedFileData, value); }
        }

        // 入力中のファイル・フォルダ名
        private string _fileOrFolderName;
        public string FileOrFolderName
        {
            get { return _fileOrFolderName; }
            set { SetProperty(ref _fileOrFolderName, value); }
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

        // プレビュー
        private string _previewText;
        public string PreviewText
        {
            get { return _previewText; }
            set { SetProperty(ref _previewText, value); }
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

            // ツリー取得
            // ディレクトリ階層を読み込み
            var razorDir = $"./{ModuleSettings.Default.RazorDirectory}";
            TreeRoot = _wpfDirectoryService.GetDirectoryFileTree(razorDir);

            // 画面の状態
            IsEnableMakeFileButton = false;
            IsEnableMakeDirectoryButton = false;
            IsEnableEditButton = false;
            IsEnableDeleteButton = false;
            IsEnableChangeButton = false;
            FileOrFolderName = string.Empty;
            PreviewText = string.Empty;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 画面の状態はリセットしない
        }

        /// <summary>
        /// ファイルまたはフォルダを選択
        /// </summary>
        private void TreeSelect(TreeSource<FileData> selectedFile)
        {
            SelectedFileOrDirectory = selectedFile;

            // ボタン有効化
            CheckAvailableName();

            // ファイルの場合は内容を表示する
            if (selectedFile.Value.IsDirectory)
            {
                PreviewText = string.Empty;
            }
            else
            {
                PreviewText = File.ReadAllText(selectedFile.Value.FullPath);
            }
        }

        /// <summary>
        /// ファイル名と、選択中のディレクトリでボタンを有効にするか判断する
        /// </summary>
        /// <returns></returns>
        private void CheckAvailableName()
        {
            if (SelectedFileOrDirectory == null)
            {
                IsEnableMakeFileButton = false;
                IsEnableMakeDirectoryButton = false;
                IsEnableEditButton = false;
                IsEnableDeleteButton = false;
                IsEnableChangeButton = false;
                return;
            }

            IsEnableDeleteButton = true;
            IsEnableEditButton = !SelectedFileOrDirectory.Value.IsDirectory;

            if (string.IsNullOrWhiteSpace(FileOrFolderName))
            {
                IsEnableMakeFileButton = false;
                IsEnableMakeDirectoryButton = false;
                IsEnableChangeButton = false;
                return;
            }

            if (SelectedFileOrDirectory.Value.IsDirectory)
            {
                // ディレクトリを選択している場合、その中に指定された名前が無いこと
                IsEnableMakeFileButton = !File.Exists(Path.Combine(SelectedFileOrDirectory.Value.FullPath, $"{FileOrFolderName}.razor"));
                IsEnableMakeDirectoryButton = !Directory.Exists(Path.Combine(SelectedFileOrDirectory.Value.FullPath, $"{FileOrFolderName}"));

                // 名前変更ボタン：同一ディレクトリに同名のディレクトリが無いこと
                IsEnableChangeButton = !Directory.Exists(Path.Combine(Path.GetDirectoryName(SelectedFileOrDirectory.Value.FullPath), $"{FileOrFolderName}"));
            }
            else
            {
                // ファイルを選択している場合、同一ディレクトリ内に指定された名前が無いこと
                IsEnableMakeFileButton = !File.Exists(Path.Combine(Path.GetDirectoryName(SelectedFileOrDirectory.Value.FullPath), $"{FileOrFolderName}.razor"));
                IsEnableMakeDirectoryButton = !Directory.Exists(Path.Combine(Path.GetDirectoryName(SelectedFileOrDirectory.Value.FullPath), $"{FileOrFolderName}"));

                // 名前変更ボタン：同一ディレクトリに同名のファイルが無いこと
                IsEnableChangeButton = !File.Exists(Path.Combine(Path.GetDirectoryName(SelectedFileOrDirectory.Value.FullPath), $"{FileOrFolderName}.razor"));
            }
        }

        /// <summary>
        /// 空のファイルを作成する
        /// </summary>
        private void MakeFile()
        {
            var filename = $"{FileOrFolderName}.razor";
            if (SelectedFileOrDirectory.Value.IsDirectory)
            {
                // 選択ディレクトリの中に作成
                var fullpath = Path.Combine(SelectedFileOrDirectory.Value.FullPath, filename);
                File.Create(fullpath).Dispose();    // 解放しないと掴みっぱなしになり、その状態でカーソルを合わせると読み込めなくて落ちる

                // ツリー表示を更新
                SelectedFileOrDirectory.AddChild(new TreeSource<FileData>(new FileData { IsDirectory = false, Name = filename, FullPath = fullpath }));
                SelectedFileOrDirectory.IsExpanded = true;
            }
            else
            {
                // 選択ファイルと同じディレクトリに作成
                var fullpath = Path.Combine(Path.GetDirectoryName(SelectedFileOrDirectory.Value.FullPath), filename);
                File.Create(fullpath).Dispose();

                // ツリー表示を更新
                SelectedFileOrDirectory.Parent.AddChild(new TreeSource<FileData>(new FileData { IsDirectory = false, Name = filename, FullPath = fullpath }));
            }

            // ボタン更新
            CheckAvailableName();
        }

        /// <summary>
        /// 空のディレクトリを作成する
        /// </summary>
        private void MakeDirectory()
        {
            var dirname = $"{FileOrFolderName}";
            if (SelectedFileOrDirectory.Value.IsDirectory)
            {
                var fullpath = Path.Combine(SelectedFileOrDirectory.Value.FullPath, dirname);
                Directory.CreateDirectory(fullpath);

                // ツリー表示を更新
                SelectedFileOrDirectory.AddChild(new TreeSource<FileData>(new FileData { IsDirectory = true, Name = dirname, FullPath = fullpath }));
                SelectedFileOrDirectory.IsExpanded = true;
            }
            else
            {
                var fullpath = Path.Combine(Path.GetDirectoryName(SelectedFileOrDirectory.Value.FullPath), dirname);
                Directory.CreateDirectory(fullpath);

                // ツリー表示を更新
                SelectedFileOrDirectory.Parent.AddChild(new TreeSource<FileData>(new FileData { IsDirectory = true, Name = dirname, FullPath = fullpath }));
            }

            // ボタン更新
            CheckAvailableName();
        }

        /// <summary>
        /// ファイルを更新する
        /// </summary>
        private void Edit()
        {
            var res = MessageBox.Show(
                "上書きしてしまいますが\nよろしいですか？",
                "確認メッセージ",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel
                );
            if (res == MessageBoxResult.Cancel)
            {
                return;
            }

            File.WriteAllText(SelectedFileOrDirectory.Value.FullPath, PreviewText);

            MessageBox.Show("保存しました。", "結果", MessageBoxButton.OK, MessageBoxImage.Information);
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

            if (SelectedFileOrDirectory.Value.IsDirectory)
            {
                // ディレクトリ削除
                Directory.Delete(SelectedFileOrDirectory.Value.FullPath, true);
            }
            else
            {
                // ファイル削除
                File.Delete(SelectedFileOrDirectory.Value.FullPath);
            }
            // 表示更新
            SelectedFileOrDirectory.RemoveOwn();

            // 削除した時、カーソルが自動的に親に移動してファイル選択イベントが実行されるので、ここでボタンの更新などは不要
            MessageBox.Show("削除しました。", "結果", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// ファイル・フォルダの名前を変更する
        /// </summary>
        private void Rename()
        {
            // 親ディレクトリのパスを取得
            var parentDir = new DirectoryInfo(SelectedFileOrDirectory.Value.FullPath).Parent.FullName;
            if (SelectedFileOrDirectory.Value.IsDirectory)
            {
                var destName = $"{FileOrFolderName}";
                var deatPath = Path.Combine(parentDir, destName);
                Directory.Move(SelectedFileOrDirectory.Value.FullPath, deatPath);

                // ツリー表示を更新
                SelectedFileOrDirectory.Value = new FileData { IsDirectory = true, FullPath = deatPath, Name = destName };
            }
            else
            {
                var destName = $"{FileOrFolderName}.razor";
                var deatPath = Path.Combine(parentDir, destName);
                File.Move(SelectedFileOrDirectory.Value.FullPath, deatPath);

                // ツリー表示を更新
                SelectedFileOrDirectory.Value = new FileData { IsDirectory = false, FullPath = deatPath, Name = destName };
            }

            // ボタン更新
            CheckAvailableName();
        }

        // ファイル・フォルダ名の入力時
        private void TextChanged()
        {
            CheckAvailableName();
        }
    }
}
