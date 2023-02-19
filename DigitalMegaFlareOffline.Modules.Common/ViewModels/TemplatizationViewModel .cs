using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using Prism.Commands;
using Prism.Events;
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
    public class TemplatizationViewModel : RegionViewModelBase
    {
        /// <summary>実行するコマンド</summary>
        public DelegateCommand ExecuteCommand { get; private set; }

        /// <summary>他のモジュールに通知する</summary>
        public IEventAggregator EventAggregator { get; set; }

        // 置換前のソース
        private string _templateInput;
        public string TemplateInput
        {
            get { return _templateInput; }
            set { SetProperty(ref _templateInput, value); }
        }

        // 置換後のソース
        private string _templateOutput;
        public string TemplateOutput
        {
            get { return _templateOutput; }
            set { SetProperty(ref _templateOutput, value); }
        }

        // 置換前文字列
        private string _beforeString;
        public string BeforeString
        {
            get { return _beforeString; }
            set { SetProperty(ref _beforeString, value); }
        }

        // 置換後文字列
        private string _afterString;
        public string AfterString
        {
            get { return _afterString; }
            set { SetProperty(ref _afterString, value); }
        }

        public TemplatizationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) :
            base(regionManager)
        {
            EventAggregator = eventAggregator;
            ExecuteCommand = new DelegateCommand(Execute);
        }

        /// <summary>
        /// 保存する
        /// </summary>
        private void Execute()
        {
            MessageBox.Show($"test");
            MessageBox.Show($"{BeforeString} {AfterString}");
            TemplateOutput = TemplateInput;
            //var path = ModuleSettings.Default.GinpayModeFile;
            //IsExistsFile = File.Exists(path);
            //if (IsExistsFile)
            //{
            //    File.WriteAllText(path, TemplatizationInput);
            //    MessageBox.Show($"保存しました。");
            //}
        }
    }
}
