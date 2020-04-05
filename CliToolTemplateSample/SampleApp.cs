using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CliToolTemplate;
using ArgsAnalyzer;
using CliToolTemplateSample.Description;

namespace CliToolTemplateSample
{
    class SampleApp : ConsoleAppBase
    {
        #region ctor
        public SampleApp(string[] args) : base(args)
        {
            base.Title = "sample app.";
            base.Width = 100;
        }
        #endregion

        protected override AppManual CreateAppManual()
        {
            var manual = new AppManual()
            {
                Summary = new DescriptionInfo(
                    "sample application summary.",
                    @"ConsoleAppBaseの参考実装です。
サンプルアプリの説明をざっくり書きます。
複数行も書けるのでヒアドキュメント推奨。" ),
                MainParameter = new DescriptionInfo(
                    "description of main parameters.",
                    @"メインのパラメータに関する説明。
処理するテキストファイルのパスを渡すとか、
処理するディレクトリパスを渡すとか、
処理データとなる文字列を直接渡すとか、
或いはエンドポイントURLを渡すとか。" ),
                Options = new List<DescriptionInfo>()
                {
                    new DescriptionInfo( "-a -auto"   , "aオプションの説明" ),
                    new DescriptionInfo( "-r -reverse", "rオプションの説明" ),
                    new DescriptionInfo( "-c -count"  , "cオプションの説明" ),
                    new DescriptionInfo( "/x=xxx"     , "xオプションの説明" ),
                    new DescriptionInfo( "/y=yyy"     , "yオプションの説明" ),
                    new DescriptionInfo( "/z=zzz"     , "zオプションの説明" ),
                    new DescriptionInfo( "-crlf"      , @"なお、
オプション説明は
改行を入れても
無視します。" ),
                },
            };

            return manual;
        }

        protected override void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
