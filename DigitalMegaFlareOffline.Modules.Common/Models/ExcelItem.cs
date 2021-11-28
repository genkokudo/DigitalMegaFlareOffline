using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMegaFlareOffline.Modules.Common.Models
{
    public class ExcelItem
    {
        /// <summary>処理に使用する通し番号</summary>
        public long Id { get; set; }

        /// <summary>ファイル名</summary>
        public string Name { get; set; }

        /// <summary>更新日</summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>説明</summary>
        public string Description { get; set; }

        /// <summary>ファイルのフルパス</summary>
        public string FullPath { get; set; }
    }
}
