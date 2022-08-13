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
        public bool TryInput(string message, out string value, TabCompletion complete)
        {
            return this.TryInput( new[] { message }, out value, complete );
        }
        public bool TryInput(string message, out string value)
        {
            return this.TryInput( new[] { message }, out value );
        }

        public bool TryInput(IEnumerable<string> messages, out string value, TabCompletion complete)
        {
            return this.TryInputCore( messages, out value, complete.ReadLine );
        }
        public bool TryInput(IEnumerable<string> messages, out string value)
        {
            return this.TryInputCore( messages, out value, Console.ReadLine );
        }
        
        private bool TryInputCore(
                IEnumerable<string> messages, 
                out string value, 
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
                string message,
                Action<string> input,
                Action<FileInfo> file,
                Action<DirectoryInfo> dir,
                TabCompletion complete)
        {
            return this.TryInputOrPath( new[] { message }, input, file, dir, complete );
        }
        public bool TryInputOrPath(
                string message,
                Action<string> input,
                Action<FileInfo> file,
                Action<DirectoryInfo> dir)
        {
            return this.TryInputOrPath( new[] { message }, input, file, dir );
        }
        public bool TryInputOrPath(
                string message,
                Action<string> input,
                Action<string> path,
                TabCompletion complete)
        {
            return this.TryInputOrPath( new[] { message }, input, path, complete );
        }
        public bool TryInputOrPath(
                string message,
                Action<string> input,
                Action<string> path)
        {
            return this.TryInputOrPath( new[] { message }, input, path );
        }

        public bool TryInputOrPath(
                IEnumerable<string> messages, 
                Action<string> input, 
                Action<FileInfo> file, 
                Action<DirectoryInfo> dir, 
                TabCompletion complete)
        {
            return this.TryInputOrPathCore( messages, input, file, dir, complete.ReadLine );
        }
        public bool TryInputOrPath(
                IEnumerable<string> messages,
                Action<string> input, 
                Action<FileInfo> file, 
                Action<DirectoryInfo> dir)
        {
            return this.TryInputOrPathCore( messages, input, file, dir, Console.ReadLine );
        }
        public bool TryInputOrPath(
                IEnumerable<string> messages, 
                Action<string> input, 
                Action<string> path, 
                TabCompletion complete)
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
                IEnumerable<string> messages, 
                Action<string> input, 
                Action<string> path)
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

        public bool Ask(
                string message,
                out bool answer,
                string yes = "Y",
                string no = "N",
                bool ignorecase = true)
        {
            return this.Ask( new[] { message }, out answer, yes, no, ignorecase );
        }
        public bool Ask(
                string message,
                Action trueCase,
                Action falseCase,
                string yes = "Y",
                string no = "N",
                bool ignorecase = true)
        {
            return this.Ask( new[] { message }, trueCase, falseCase, yes, no, ignorecase );
        }

        public bool Ask(
                IEnumerable<string> messages,
                out bool answer,
                string yes = "Y",
                string no = "N",
                bool ignorecase = true)
        {
            return this.AskCore( messages, out answer, yes, no, ignorecase );
        }
        public bool Ask(
                IEnumerable<string> messages,
                Action trueCase,
                Action falseCase,
                string yes = "Y",
                string no = "N",
                bool ignorecase = true)
        {
            if ( this.AskCore( messages, out bool answer, yes, no, ignorecase ) )
            {
                if ( answer )
                {
                    trueCase();
                }
                else
                {
                    falseCase();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool AskCore(IEnumerable<string> messages,
                out bool answer,
                string yes, 
                string no,
                bool ignorecase)
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
            Console.WriteLine( ignorecase ? " ignore-case" : "" );


            var compare = ignorecase 
                    ? StringComparison.OrdinalIgnoreCase 
                    : StringComparison.Ordinal;

            string str = Console.ReadLine();
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
