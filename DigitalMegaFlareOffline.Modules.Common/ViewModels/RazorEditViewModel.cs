using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using DigitalMegaFlareOffline.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMegaFlareOffline.Modules.Common.ViewModels
{
    public class RazorEditViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public RazorEditViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            Message = messageService.GetMessage() + GetType().Name;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 読み込み画面からの遷移の場合、読み込むファイルパスを受け取る
            var snippetFullPath = navigationContext.Parameters["SnippetFullPath"] as string;        // TODO:stringではなく、ファイル情報クラスとして受け取ること
            if (!string.IsNullOrWhiteSpace(snippetFullPath))
            {
                //// スニペットを読み込んで、画面に反映する
                //var snippetDocument = SnippetService.ReadSnippet(snippetFullPath);
                //Shortcut = snippetDocument.Shortcut;
                //Language = snippetDocument.Language;
                //Delimiter = snippetDocument.Delimiter;
                //Description = snippetDocument.Description;
                //TemplateInput = snippetDocument.Code;

                //// 変数
                //foreach (var declaration in snippetDocument.Declarations)
                //{
                //    Variables.Add(new TemplateVariable { Name = declaration.Id, Description = declaration.ToolTip, DefValue = declaration.Default, IsClassName = declaration.Function == Function.ClassName });
                //}

                //// 出力ボタン
                //SetIsEnableOutput();

                //// 内容を他のモジュールに通知
                //EventAggregator.GetEvent<InputTemplateEvent>()
                //    .Publish(new InputTemplate { InputText = snippetDocument.Code, SendFromViewModelName = GetType().Name });
            }
        }
    }
}
