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

        public bool TryInput(IEnumerable<string> messages, out string value)
        {
            foreach ( var message in messages )
            {
                Console.WriteLine( message );
            }
            Console.Write( "> " );
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

        public bool TryInputOrPath(IEnumerable<string> messages, Action<string> input, Action<string> path)
        {
            return this.TryInputOrPath( 
                messages,
                input,
                (FileInfo f) => path( f.FullName ),
                (DirectoryInfo d) => path( d.FullName ) );
        }
        public bool TryInputOrPath(IEnumerable<string> messages, Action<string> input, Action<FileInfo> file, Action<DirectoryInfo> dir)
        {
            foreach ( var message in messages )
            {
                Console.WriteLine( message );
            }
            Console.Write( "> " );
            string str = Console.ReadLine();

            if ( this.CancelKeywords.Contains( str ) )
            {
                return false;
            }

            if ( File.Exists( str ) )
            {
                file( new FileInfo( str ) );
                return true;
            }

            if ( Directory.Exists( str ) )
            {
                dir( new DirectoryInfo( str ) );
                return true;
            }

            input( str );
            return true;
        }
    }
}
