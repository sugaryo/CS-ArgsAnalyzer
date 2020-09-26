using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CliToolTemplate.Utility.ConsoleAppUtil;

namespace CliToolTemplate.Utility
{
    public class IndentWriter
    {
        public int Level{ get; set; } = 0;

        public bool Write(string line)
        {
            return IndentWrite( this.Level, line );
        }
        public bool Write(string[] lines)
        {
            return IndentWrite( this.Level, lines );
        }

        public bool Write(string subtitle, string[] lines)
        {
            if ( IndentWrite( this.Level, subtitle ) )
            {
                IndentWrite( this.Level + 1, lines );
                return true;
            }
            else
            {
                return IndentWrite( this.Level, lines );
            }
        }
    }
}
