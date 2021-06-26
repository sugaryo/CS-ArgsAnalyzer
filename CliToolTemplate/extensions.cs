using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliToolTemplate
{
    public static class extensions
    {
        // いわゆる S-JIS エンコ
        private static readonly Encoding MS932 = Encoding.GetEncoding( 932 );

        public static string[] lines(this string s)
        {
            return s.Split( new[] { "\r\n", "\n" }, StringSplitOptions.None );
        }

        public static string singlify(this string[] lines)
        {
            var sb = new StringBuilder();
            foreach ( var line in lines )
            {
                sb.Append( line.Trim() );
            }
            return sb.ToString();
        }


        public static int vbLenB(this string s)
        {
            return MS932.GetByteCount( s );
        }

    

        public static bool isNull(this string s)
        {
            return null == s;
        }
        public static bool notNull(this string s)
        {
            return null != s;
        }

        public static bool isNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        public static bool notNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static bool isNullOrWS(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
        public static bool notNullOrWS(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        public enum NvlMode
        {
            NullOnly,
            Empty,
            WhiteSpace,
        }
        public static string nvl(this string s, string alt, NvlMode mode = NvlMode.WhiteSpace)
        {
            Predicate<string> isnull;
            switch ( mode )
            {
                case NvlMode.NullOnly:
                    isnull = isNull;
                    break;

                case NvlMode.Empty:
                    isnull = isNullOrEmpty;
                    break;

                case NvlMode.WhiteSpace:
                    isnull = isNullOrWS;
                    break;

                default:
                    // 有り得ないケースなので例外でも良いが適当に固定値ラムダで埋めておく。
                    isnull = (string x) => { return false; };
                    break;
            }

            return isnull( s ) ? alt : s;
        }


        public static int min(this int value, int minimum)
        {
            return value < minimum ? minimum : value;
        }
        public static int max(this int value, int maximum)
        {
            return maximum < value ? maximum : value;
        }

        public static void backspace(this StringBuilder sb, int n = 1)
        {
            if ( sb.Length <= n )
            {
                sb.Clear();
            }
            else
            {
                sb.Remove( sb.Length - n, n );
            }
        }
        public static void reset(this StringBuilder sb, string str)
        {
            sb.Clear();
            sb.Append( str );
        }
    }
}
