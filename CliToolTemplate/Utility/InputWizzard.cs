using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliToolTemplate.Utility
{
    public class InputWizzard
    {
        public List<string> CancelKeywords { get; set; }


        public InputWizzard()
        {
            this.CancelKeywords = new List<string>() { "exit", "cancel" };
        }
        public InputWizzard( IEnumerable<string> cancelKeywords )
        {
            this.CancelKeywords = new List<string>( cancelKeywords );
        }

        public bool TryInput(string message, out string value)
        {
            Console.WriteLine( message );
            string str = Console.ReadLine();

            if ( this.CancelKeywords.Contains( str ) )
            {
                value = null;
                return false;
            }
            else
            {
                value = str;
                return true;
            }
        }

        public bool TryInputOrPath(string message, Action<string> input, Action<string> path)
        {
            return this.TryInputOrPath( message, input, f => path( f ), d => path( d ) );
        }
        public bool TryInputOrPath(string message, Action<string> input, Action<string> file, Action<string> dir)
        {
            Console.WriteLine( message );
            string str = Console.ReadLine();

            if ( this.CancelKeywords.Contains( str ) )
            {
                return false;
            }

            if ( File.Exists( str ) )
            {
                file( str );
                return true;
            }

            if ( Directory.Exists( str ) )
            {
                dir( str );
                return true;
            }

            input( str );
            return true;
        }
    }
}
