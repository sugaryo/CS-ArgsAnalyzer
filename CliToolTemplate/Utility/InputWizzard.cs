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

        #region ctor
        public InputWizzard()
        {
            this.CancelKeywords = new List<string>() { "exit", "cancel" };
        }
        public InputWizzard( IEnumerable<string> cancelKeywords )
        {
            this.CancelKeywords = new List<string>( cancelKeywords );
        }
        #endregion


        #region TryInput

        public bool TryInput(out string value, params string[] messages)
        {
            return this.TryInputCore( out value, messages, Console.ReadLine );
        }
        
        public bool TryInput(out string value, TabCompletion complete, params string[] messages)
        {
            return this.TryInputCore( out value, messages, complete.ReadLine );
        }
        
        private bool TryInputCore( out string value, 
                IEnumerable<string> messages, 
                Func<string> readline)
        {
            foreach ( var message in messages )
            {
                Console.WriteLine( message );
            }
            string str = readline();

            if ( null == str || this.CancelKeywords.Contains( str ) )
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
        #endregion

        #region TryInputOrPath

        public bool TryInputOrPath(
                Action<string> input,
                Action<FileInfo> file,
                Action<DirectoryInfo> dir,
                TabCompletion complete,
                params string[] messages)
        {
            return this.TryInputOrPathCore( messages, input, file, dir, complete.ReadLine );
        }
        public bool TryInputOrPath(
                Action<string> input,
                Action<FileInfo> file,
                Action<DirectoryInfo> dir,
                params string[] messages)
        {
            return this.TryInputOrPathCore( messages, input, file, dir, Console.ReadLine );
        }
        public bool TryInputOrPath(
                Action<string> input, 
                Action<string> path, 
                TabCompletion complete,
                params string[] messages)
        {
            void FileCallback(FileInfo f)
            {
                path( f.FullName );
            }
            void DirCallback(DirectoryInfo d)
            {
                path( d.FullName );
            }
            return this.TryInputOrPathCore( messages, input, FileCallback, DirCallback, complete.ReadLine );
        }
        public bool TryInputOrPath(
                Action<string> input, 
                Action<string> path,
                params string[] messages)
        {
            void FileCallback(FileInfo f)
            {
                path( f.FullName );
            }
            void DirCallback(DirectoryInfo d)
            {
                path( d.FullName );
            }
            return this.TryInputOrPathCore( messages, input, FileCallback, DirCallback, Console.ReadLine );
        }

        private bool TryInputOrPathCore(
                IEnumerable<string> messages, 
                Action<string> input, 
                Action<FileInfo> file, 
                Action<DirectoryInfo> dir, 
                Func<string> readline)
        {
            foreach ( var message in messages )
            {
                Console.WriteLine( message );
            }
            string str = readline();

            if ( null == str || this.CancelKeywords.Contains( str ) )
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
        #endregion


        #region Ask(Yes/No)

        public bool Ask( out bool answer, params string[] messages)
        {
            return this.AskCore( out answer, messages, "Y", "N", true );
        }
        public bool Ask(out bool answer,
                bool ignorecase,
                params string[] messages)
        {
            return this.AskCore( out answer, messages, "Y", "N", ignorecase );
        }
        public bool Ask(out bool answer,
                string yes,
                string no,
                params string[] messages)
        {
            return this.AskCore( out answer, messages, yes, no, true );
        }
        public bool Ask( out bool answer,
                string yes,
                string no,
                bool ignorecase,
                params string[] messages )
        {
            return this.AskCore( out answer, messages, yes, no, ignorecase );
        }


        private bool AskCore( out bool answer,
                IEnumerable<string> messages,
                string yes, 
                string no,
                bool ignorecase )
        {
            foreach ( var message in messages )
            {
                Console.WriteLine( message );
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write( "select which case " );
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write( "[" + yes + "]" );
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write( " / " );
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write( "[" + no + "]" );
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine( ignorecase ? "  ※ ignore-case" : "" );
            Console.ResetColor();
            string str = Console.ReadLine();

            var compare = ignorecase 
                    ? StringComparison.OrdinalIgnoreCase 
                    : StringComparison.Ordinal;
            if ( null == str || this.CancelKeywords.Contains( str ) )
            {
                // 入力無し
            }
            else if ( str.Equals( yes, compare ) )
            {
                answer = true;
                return true;
            }
            else if ( str.Equals( no, compare ) )
            {
                answer = false;
                return true;
            }
            answer = false;
            return false;
        }
        #endregion
    }
}
