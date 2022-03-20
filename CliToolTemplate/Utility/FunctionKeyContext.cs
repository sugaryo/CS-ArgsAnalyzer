using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliToolTemplate.Utility
{
    public class FunctionKeyContext
    {
        internal static readonly ConsoleKey[] Keys = new[] {
            ConsoleKey.F1,
            ConsoleKey.F2,
            ConsoleKey.F3,
            ConsoleKey.F4,
            ConsoleKey.F5,
            ConsoleKey.F6,
            ConsoleKey.F7,
            ConsoleKey.F8,
            ConsoleKey.F9,
            ConsoleKey.F10,
            ConsoleKey.F11,
            ConsoleKey.F12,
        };

        private readonly Dictionary<ConsoleKey, string> context = new Dictionary<ConsoleKey, string>();

        public void Clear()
        {
            this.context.Clear();
        }

        public ConsoleKey Add( string value )
        {
            // 満杯なら追加しない。
            // nullable ではないので、便宜上 NoName を返しておく。
            if ( 12 == this.context.Count ) return ConsoleKey.NoName;

            // 今の件数が今突っ込むべきインデックス。
            // 今が 0件 なら 1件目 を入れるので index[0]
            // 今が 1件 なら 2件目 を入れるので index[1]
            int index = this.context.Count;
            ConsoleKey key = Keys[index];
            this.context.Add( key, value );

            // 今入れたキー値を返す。
            return key;
        }


        public string this[ConsoleKey key]
        {
            get
            {
                return this.context.ContainsKey( key ) ? this.context[key] : null;
            }
        }

    }
}
