using DigitalMegaFlareOffline.Modules.Common.Mvvm;
using DocumentFormat.OpenXml.Wordprocessing;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public DelegateCommand SetBeforeStringCommand { get; private set; }

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
            set
            {
                SetProperty(ref _beforeString, value);
            }
        }
        
        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
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
            SetBeforeStringCommand = new DelegateCommand(SetBeforeString);
        }

        private void SetBeforeString()
        {
            IsEnabled = !string.IsNullOrWhiteSpace(_beforeString);
        }

        /// <summary>
        /// 保存する
        /// </summary>
        private void Execute()
        {
            if (string.IsNullOrWhiteSpace(BeforeString))
            {
                return;
            }
            var isDefault = string.IsNullOrWhiteSpace(AfterString);

            var inf = new Inflector.Inflector(new CultureInfo("en-US"));
            // BeforeStringは単数形のPascalCaseで入力される
            var pascalSingularBefore = BeforeString.Trim();
            var pascalSingularAfter = isDefault ? "$PascalSingular$" : AfterString.Trim();

            var camelSingularBefore = inf.Camelize(pascalSingularBefore);
            var camelSingularAfter = isDefault ? "$CamelSingular$" : inf.Camelize(pascalSingularAfter);
            var kebabSingularBefore = inf.Underscore(pascalSingularBefore).Replace("_", "-");
            var kebabSingularAfter = isDefault ? "$KebabSingular$" : inf.Underscore(pascalSingularAfter).Replace("_", "-");
            
            var pascalPluralBefore = inf.Pluralize(BeforeString);    // 複数形
            var pascalPluralAfter = isDefault ? "$PascalPlural$" : inf.Pluralize(AfterString);
            var camelPluralBefore = inf.Camelize(pascalPluralBefore);
            var camelPluralAfter = isDefault ? "$CamelPlural$" : inf.Camelize(pascalPluralAfter);
            var kebabPluralBefore = inf.Underscore(pascalPluralBefore).Replace("_", "-");
            var kebabPluralAfter = isDefault ? "$KebabPlural$" : inf.Underscore(pascalPluralAfter).Replace("_", "-");
            
            TemplateOutput = TemplateInput
                .Replace(pascalPluralBefore, pascalPluralAfter)
                .Replace(camelPluralBefore, camelPluralAfter)
                .Replace(kebabPluralBefore, kebabPluralAfter)
                .Replace(pascalSingularBefore, pascalSingularAfter)
                .Replace(camelSingularBefore, camelSingularAfter)
                .Replace(kebabSingularBefore, kebabSingularAfter);
        }
    }
}
