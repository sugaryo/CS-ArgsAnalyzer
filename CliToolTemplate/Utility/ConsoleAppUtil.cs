using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliToolTemplate.Utility
{
    public static class ConsoleAppUtil
    {

        public static bool IndentWrite(int level, string line)
        {
            // 空文字の場合は書き込みをスキップ。
            if ( line.isNullOrEmpty() ) return false;

            string indent = new string( ' ', level * 4 );

            Console.Write( indent );
            Console.WriteLine( line );
            return true;
        }

        public static bool IndentWrite(int level, string[] lines)
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
