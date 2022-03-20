using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CliToolTemplate;
using ArgsAnalyzer;
using CliToolTemplate.Description;
using CliToolTemplate.Utility;


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
            var wizzard = new InputWizzard();
            Demo1( wizzard );
            Demo2( wizzard, arguments );
        }

        private static void Demo1(InputWizzard wizzard)
        {
            Console.WriteLine();
            Console.WriteLine( "■機能１：入力振り分け（文字列、ファイルパス、フォルダ）■" );

            string mode = "";
            if ( wizzard.TryInputOrPath(
                    new[] {
                        "適当に入力するか、ファイル・フォルダのパスを指定してください。",
                        @"(中止するには ""exit"" 若しくは ""cancel"" を入力します。)"
                    },
                    input =>
                    {
                        mode = "string";
                        Console.WriteLine( $"{input}が入力されました" );
                    },
                    file =>
                    {
                        mode = "file";
                        Console.WriteLine( $"ファイル[{file.FullName}]が渡されました。" );
                    },
                    dir =>
                    {
                        mode = "folder";
                        Console.WriteLine( $"フォルダ[{dir.FullName}]が渡されました。" );
                    }
                ) )
            {
                Console.WriteLine( $"mode={mode}" );
            }
            else
            {
                Console.WriteLine( "入力がキャンセルされました" );
            }
        }

        private static void Demo2(InputWizzard wizzard, Arguments arguments)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine( "■機能２：Tab補完入力のデモ動作■" );

            TabCompletion tab = new TabCompletion( arguments.AsParameters() );

            string value;
            if ( wizzard.TryInput( 
                new[] {
                        "Tab補完入力のデモ動作。",
                        @"(中止するには ""exit"" 若しくは ""cancel"" を入力します。)"
                    },
                out value, tab ) )
            {
                Console.WriteLine( $"入力 : {value}" );
            }
            else
            {
                Console.WriteLine( "入力がキャンセルされました" );
            }
        }
    }
}
