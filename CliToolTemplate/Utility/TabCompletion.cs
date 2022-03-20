﻿using System;
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

        // インデントリスト表示をオプションに変更。（デフォルトオフ）
        public bool Indent { get; set; } = false;

        #region ctor
        public TabCompletion(IEnumerable<string> data)
        {
            this.data = data;
        }
        #endregion


        #region ReadLine()
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

        // Console.ReadKey char 単位の読み込みループ
        // - [ESC]   入力中断  
        //           ※ 再帰処理終了
        // - [ENTER] 入力確定  
        //           ※ 再帰処理終了
        // - [BS]    一文字削除  
        //           ※ カレット移動に対応出来てないので、末尾一文字削除しか出来ない。
        // - [DEL]   全消し
        // - [TAB]   前方一致補完：TabComplete
        // - else    Char.IsControl なら無視し、それ以外は通常入力として buff に詰める。
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

        // [TAB] 入力時の補完処理：
        // - this.data の中から string.StartsWith で前方一致判定するものを抜き出す。
        // - 抜き出した件数に応じて分岐
        //   - １件ヒットした場合は確定なので、ヒットしたものを入力する。
        //   - 複数ヒットした場合は、それらの共通部分まで入力補完する。
        //   - ヒットしなかった場合は何もしない。
        private void TabComplete(StringBuilder buff)
        {
            // データから前方一致するものを抜き出し、ヒットした件数に応じてコンソール処理する。
            string input = buff.ToString();
            var matches = this.data
                .Where( x => x.StartsWith( input ) )
                .ToArray();


            // ■１件（確定）
            if ( 1 == matches.Length )
            {
                // 前方一致が１件しか無ければこれで確定。
                string match = matches[0];
                buff.reset( match );
                OverWrite( buff.ToString() );
            }
            // ■複数（絞り込まれたものの中から共通部分まで入力を補完する）
            else if ( 1 < matches.Length )
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine( $" [入力選択]" );
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach ( var m in matches )
                {
                    if ( this.Indent )
                    {
                        Console.WriteLine( $"  - {m}" );
                    }
                    else
                    {
                        Console.WriteLine( $"{m}" );
                    }
                }
                Console.ResetColor();

                // 前方一致が複数ある場合は共通部分まで入力補完。
                string prefix = FowardMatcher.Like( matches );
                buff.reset( prefix );
                OverWrite( buff.ToString() );
            }
            // ■ヒットするものなし（なにもしない）
            else
            {
                // マッチするものがなかったらこのまま。
            }
        }
        #endregion
        

        #region static OverWrite()
        // 指定した文字列 str で完全に上書きする。
        // Consoler 自体にそういう機能はなかったので、
        // 一旦行全体にスペース書き込みして見た目消して、その上から str を書き込む事にした。
        // ※ 全角半角問題があるので str.Length は使えないので Console.BufferWidth を採用。
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
        #endregion

        #region static class FowardMatcher
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