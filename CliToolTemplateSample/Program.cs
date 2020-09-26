using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliToolTemplateSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new SampleApp( new[]{ "-h" } );
            app.Execute();
        }
    }
}
