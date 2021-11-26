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
using System.Windows;

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class RazorLoadViewModel : RegionViewModelBase
    {
        /// <summary>空のファイルを作成するコマンド</summary>
        public DelegateCommand MakeFileCommand { get; private set; }
        /// <summary>フォルダを作成するコマンド</summary>
        public DelegateCommand MakeFolderCommand { get; private set; }
        /// <summary>ファイルを編集するコマンド</summary>
        public DelegateCommand EditCommand { get; private set; }
        /// <summary>ファイル・フォルダを作成するコマンド</summary>
        public DelegateCommand DeleteCommand { get; private set; }

        // TODO:ボタンの有効状態は、選択中の項目によって判定すべき
        // 選択中が何かを示すプロパティ1つ置けば良いのでは？

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
            get { return _isEnableEditButton; }
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

            // コマンドの設定
            MakeFileCommand = new DelegateCommand(MakeFile);
            MakeFolderCommand = new DelegateCommand(MakeFolder);
            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);

            // ボタンの状態
            IsEnableMakeButton = false;
            IsEnableEditButton = false;
            IsEnableDeleteButton = false;

            // データ読み込み
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
        }

        /// <summary>
        /// 空のファイルを作成する
        /// </summary>
        private void MakeFile()
        {
            // ファイルを作成する

            // 作成完了
        }

        /// <summary>
        /// フォルダを作成する
        /// </summary>
        private void MakeFolder()
        {
            IsEnableMakeButton = false;
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
        }
    }
}
