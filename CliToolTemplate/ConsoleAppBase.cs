using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgsAnalyzer;

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
#warning TODO：コマンド情報を生成、表示する処理。
        }

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
    }
}
