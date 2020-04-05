using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgsAnalyzer;
using CliToolTemplateSample.Description;

namespace CliToolTemplate
{
    public abstract class ConsoleAppBase
    {
#warning TODO：プロパティはどちらかと言うと、ビルダーパターンでImmutableな設定VOを食わせる形にしたい。
        #region properties

        public string Title { get; set; } = null;
        public bool HasTitle { get { return this.Title.notNullOrWS(); } }


        public const int DEFAULT_WIDTH = 80;
        public const int MIN_WIDTH = 20;
        public const int MAX_WIDTH = 1200;


        private int width = DEFAULT_WIDTH;
        public int Width
        {
            get { return width; }
            set { width = value.min( MIN_WIDTH ).max( MAX_WIDTH ); }
        }


        public char BarChar { get; set; } = '-';
        public string Bar { get { return new string( this.BarChar, this.width ); } }

        public const string DEFAULT_EXIT_MESSAGE = "press any key to exit.";
        private string onExitMessage = DEFAULT_EXIT_MESSAGE;
        public string OnExitMessage
        {
            get { return onExitMessage; }
            set { onExitMessage = value.nvl( DEFAULT_EXIT_MESSAGE ); }
        }


        public const string DEFAULT_ERROR_MESSAGE = "!! ERROR !!";
        private string onErrorMessage = DEFAULT_ERROR_MESSAGE;
        public string OnErrorMessage
        {
            get { return onErrorMessage; }
            set { onErrorMessage = value.nvl( DEFAULT_ERROR_MESSAGE ); }
        }

        #endregion


        private readonly Arguments arguments;


        #region ctor
        public ConsoleAppBase(string[] args)
        {
            this.arguments = args.parse();
        }
        #endregion


        public void Execute()
        {
            try
            {
                string bar = this.Bar;

                if ( this.HasTitle )
                {
                    Console.WriteLine( bar );
                    Console.WriteLine( this.Title );
                    Console.WriteLine( bar );
                }



                if ( this.arguments.IsEmpty )
                {
                    // パラメータが完全に無かった場合はアプリケーションの説明を表示する。
                    this.ShowApplicationInfo();
                }
                else if ( this.arguments.IsParameterless )
                {
                    // オプション指定はあるがパラメータがない場合はNoData
                    this.ExecuteNoData( arguments );
                }
                else
                {
                    this.Execute( this.arguments );
                }
            }
            catch ( Exception ex )
            {
                this.OnError( ex );
            }

            this.OnExit();
        }

        private void ShowApplicationInfo()
        {
            string bar = this.Bar;

            Console.WriteLine( "no parameter or options." );
            {
                // アプリケーションのマニュアル情報を生成してコンソールに書き出す。
                var manual = this.CreateAppManual();

                if ( null != manual.Summary )
                {
                    Console.WriteLine( bar );
                    IndentWrite( 0, "SUMMARY:" );

                    int indent = 1;
                    indent += IndentWrite( indent, manual.Summary.Label )        ? 1 : 0;
                    indent += IndentWrite( indent, manual.Summary.Text.lines() ) ? 1 : 0;
                }

                if ( null != manual.MainParameter )
                {
                    Console.WriteLine( bar );
                    IndentWrite( 0, "PARAMETER:" );

                    int indent = 1;
                    indent += IndentWrite( indent, manual.MainParameter.Label )        ? 1 : 0;
                    indent += IndentWrite( indent, manual.MainParameter.Text.lines() ) ? 1 : 0;
                }

                if ( null != manual.Options && 0 < manual.Options.Count )
                {
                    Console.WriteLine( bar );
#warning TODO：あとで折り返しの出力ルールを複数パターン用意してMode列挙体で指定する。

                    int mxlenb = manual.Options.AsEnumerable().Select( x => x.Label.vbLenB() ).Max();

                    IndentWrite( 0, "OPTIONS:" );
                    foreach ( var opt in manual.Options )
                    {
                        int lenb = opt.Label.vbLenB();
                        string tab = new string( ' ', 4 + mxlenb - lenb );
                        string line 
                            = opt.Label
                            + tab
                            + opt.Text.lines().singlify();
                        IndentWrite( 1, line );
                    }
                }
            }
            Console.WriteLine( bar );
        }

        protected abstract AppManual CreateAppManual();

        private void ExecuteNoData(Arguments arguments)
        {
#warning TODO：入力パラメータを画面から受け取る、対話形式処理からExecuteに流す。
        }

        protected abstract void Execute(Arguments arguments);


        protected virtual void OnExit()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine( this.OnExitMessage );
            Console.ReadKey();
        }

        protected virtual void OnError(Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine( this.OnErrorMessage );
            Console.WriteLine( ex.Message );
            Console.WriteLine( ex.StackTrace );
        }

        protected static bool IndentWrite(int level, string line)
        {
            // 空文字の場合は書き込みをスキップ。
            if ( line.isNullOrEmpty() ) return false;

            string indent = new string( ' ', level * 4 );

            Console.Write( indent );
            Console.WriteLine( line );
            return true;
        }
        protected static bool IndentWrite(int level, string[] lines)
        {
            // 空配列の場合は書き込みをスキップ。
            if ( null == lines || 0 == lines.Length ) return false;

            string indent = new string( ' ', level * 4 );

            foreach ( var line in lines )
            {
                Console.Write( indent );
                Console.WriteLine( line );
            }
            return true;
        }
    }
}
