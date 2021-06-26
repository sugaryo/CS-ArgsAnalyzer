using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliToolTemplate.Utility
{
    public class TabCompletion
    {
        /// <summary>入力補完する候補データ</summary>
        private readonly IEnumerable<string> data;
        public TabCompletion(IEnumerable<string> data)
        {
            this.data = data;
        }

        /// <summary>
        /// Console 入力中に Tab キーで入力補完する ReadLine 実装。
        /// </summary>
        /// <returns>
        /// Excapeキーで入力を中止する。
        /// Enterキーで入力を確定する。
        /// Tabキーで入力補完を行う。
        /// 入力補完では、候補データから前方一致検索を行い絞り込む。
        /// 絞り込んだ候補で一致している部分まで入力を補完する。
        /// </returns>
        public string ReadLine()
        {
            try
            {
                // 再帰処理をキックして ReadKey しながら補完入力する。
                return this.ReadLineCore( new StringBuilder() );
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private string ReadLineCore(StringBuilder buff)
        {
            // ReadKey を再帰処理して入力を読み取り、補完処理しながら buff に詰めていく。
            var info = Console.ReadKey( true );
            switch ( info.Key )
            {
                #region 特殊系｛ESC／ENTER／BS／DEL｝
                case ConsoleKey.Escape:
                    // ESC 入力だったら中断。
                    return null;

                case ConsoleKey.Enter:
                    // ENTER 入力だったら入力確定。
                    Console.WriteLine();
                    return buff.ToString();

                case ConsoleKey.Backspace:
                    // BS が来たら一文字削除して表示更新。
                    buff.backspace();
                    OverWrite( buff.ToString() );
                    break;

                case ConsoleKey.Delete:
                    // DEL が来たら全消し。
                    buff.Clear();
                    OverWrite( "" );
                    break;
                #endregion
                
                case ConsoleKey.Tab:
                    // TAB 入力だったらここまでの入力で前方一致検索して補完する。
                    this.TabComplete( buff );
                    break;

                default:
                    char c = info.KeyChar;
                    if ( Char.IsControl( c ) )
                    {
                        // 制御文字は無視する。
                    }
                    else
                    {
                        Console.Write( c );
                        buff.Append( c );
                    }
                    break;
            }


            // 抜けてきたら再帰処理。
            return ReadLineCore( buff );
        }

        private void TabComplete(StringBuilder buff)
        {
            // データから前方一致するものを抜き出し。
            string prefix = buff.ToString();
            var matches = this.data
                .Where( x => x.StartsWith( prefix ) )
                .ToArray();


            if ( 1 == matches.Length )
            {
                // 前方一致が１件しか無ければこれで確定。
                string match = matches[0];
                buff.reset( match );
                OverWrite( buff.ToString() );
            }
            else if ( 1 < matches.Length )
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine( $" [入力選択]" );
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach ( var m in matches )
                {
                    Console.WriteLine( $"  - {m}" );
                }
                Console.ResetColor();

                // 前方一致が複数ある場合は共通部分まで入力補完。
                string match = FowardMatcher.Like( matches );
                buff.reset( match );
                OverWrite( buff.ToString() );
            }
            else
            {
                // マッチするものがなかったらこのまま。
            }
        }

        private static void OverWrite(string str)
        {
            // 一旦スペースを書き込んで見た目上消す。
            string clear = new string( ' ', Console.BufferWidth - 1 );
            Console.CursorLeft = 0;
            Console.Write( clear );
            // カーソルを戻して改めて書き込み。
            Console.CursorLeft = 0;
            Console.Write( str );
        }

        #region private static class FowardMatcher
        /// <summary>
        /// 複数の候補データの全てが前方一致している部分を抜き出す。
        /// </summary>
        private static class FowardMatcher
        {
            public static string Like(string[] lines)
            {
                var sb = new StringBuilder();
                foreach ( char c in Each( lines ) )
                {
                    sb.Append( c );
                }
                return sb.ToString();
            }

            private static IEnumerable<char> Each(string[] lines)
            {
                // 構成要素の最小文字数まで。
                int min = lines.Select( x => x.Length ).Min();

                // 文字が一致する限り yield return して、食い違いが検出されたら終わる。
                for ( int i = 0; i < min; i++ )
                {
                    // 番兵法的に lines[0] を基準にして比較する。
                    char c = lines[0][i];
                    for ( int n = 1; n < lines.Length; n++ )
                    {
                        if ( c != lines[n][i] ) yield break;
                    }

                    yield return c;
                }
            }
        }
        #endregion
    }
}
