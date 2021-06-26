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
                IEnumerable<string> messages, 
                Action<string> input, Action<FileInfo> file, Action<DirectoryInfo> dir, TabCompletion complete)
        {
            return this.TryInputOrPathCore( messages, input, file, dir, complete.ReadLine );
        }
        public bool TryInputOrPath(
            IEnumerable<string> messages,
            Action<string> input, Action<FileInfo> file, Action<DirectoryInfo> dir)
        {
            return this.TryInputOrPathCore( messages, input, file, dir, Console.ReadLine );
        }
        
        public bool TryInputOrPath(
                IEnumerable<string> messages, 
                Action<string> input, Action<string> path, TabCompletion complete)
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
                Action<string> input, Action<string> path)
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
    }
}
