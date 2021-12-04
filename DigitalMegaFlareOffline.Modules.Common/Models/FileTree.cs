using MithrilCube.Prism;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMegaFlareOffline.Modules.Common.Models
{
    // ツリー表示データ
    // XAMLはジェネリクス型が扱えないため、このように型を固定したクラスに再定義することが必要
    public class FileTree : TreeSource<string>
    {
        public FileTree(string value) : base(value)
        {
        }

        // 固有の処理は何も書かない
    }
}
