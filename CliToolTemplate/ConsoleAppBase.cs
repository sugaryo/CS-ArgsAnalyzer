using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgsAnalyzer;
using CliToolTemplate.Utility;
using CliToolTemplate.Description;

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


        #region + Execute
        public void Execute()
        {
            try
            {

                if ( this.HasTitle )
                {
                    string bar = this.CreateBar( '=' );
                    Console.WriteLine( bar );
                    Console.WriteLine( this.Title );
                    Console.WriteLine( bar );
                }



                // ■パラメータもオプションもない、正真正銘 psvm args が空の場合。
                if ( this.arguments.IsEmpty )
                {
                    Console.WriteLine( "no parameter or options." );
                    this.ShowHelp();
                }
                // ■ヘルプモードの場合。
                else if ( this.IsHelpMode( this.arguments ) )
                {
                    Console.WriteLine( "help mode." );
                    this.ShowHelp();
                }
                // ■何かしらパラメータかオプションがある場合。
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
        #endregion

        #region IsHelpMode / ShowHelp

        protected virtual bool IsHelpMode(Arguments arguments)
        {
            // デフォルト実装では取り敢えず h help が含まれていたらヘルプモードにする。
            return arguments.Have( "h", "help" );
        }

        protected void ShowHelp()
        {
            string bar = this.CreateBar( '-' );
            var indent = new IndentWriter();
            {
                // アプリケーションのマニュアル情報を生成してコンソールに書き出す。
                var manual = this.CreateAppManual();

                #region manual.summary
                if ( null != manual.Summary )
                {
                    indent.Level = 0;
                    indent.Write( bar );
                    indent.Write( "SUMMARY:" );

                    indent.Level++;
                    indent.Write( 
                        manual.Summary.Label,
                        manual.Summary.Text.lines() );
                }
                #endregion

                #region manual.parameter
                if ( null != manual.MainParameter )
                {
                    indent.Level = 0;
                    indent.Write( bar );
                    indent.Write( "PARAMETER:" );

                    indent.Level++;
                    indent.Write(
                        manual.MainParameter.Label,
                        manual.MainParameter.Text.lines() );
                }
                #endregion

                #region manual.options
                if ( null != manual.Options && 0 < manual.Options.Count )
                {
#warning TODO：あとで折り返しの出力ルールを複数パターン用意してMode列挙体で指定する。
                    int mxlenb = manual.Options.AsEnumerable().Select( x => x.Label.vbLenB() ).Max();


                    indent.Level = 0;
                    indent.Write( bar );
                    indent.Write( "OPTIONS:" );
                    indent.Level++;
                    foreach ( var opt in manual.Options )
                    {
                        int lenb = opt.Label.vbLenB();
                        string tab = new string( ' ', 4 + mxlenb - lenb );
                        string line 
                            = opt.Label
                            + tab
                            + opt.Text.lines().singlify();

                        indent.Write( line );
                    }
                }
                #endregion

                indent.Level = 0;
                indent.Write( bar );
            }
        }

        #endregion

        protected abstract AppManual CreateAppManual();
        
        protected abstract void Execute(Arguments arguments);

        #region CreateBar

        protected string CreateBar(char barchar)
        {
            return new string( barchar, this.Width );
        }

        #endregion

        #region OnExit / OnError

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

        #endregion

    }
}
