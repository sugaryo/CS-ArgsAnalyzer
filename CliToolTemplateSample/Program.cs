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
            string[] parameters = new[] {
                "hoge-foo",
                "hoge-bar",
                "hoge-baz",
                "moge",
                "mogemoge",
                "mogemogemoge",
                "piyo",
                "piyopiyo",
                "x:hoge",
                "x:moge",
                "x:piyo",
                "x:poyo",
                "aaax",
                "aaay",
                "aaaz",
            };

            var app = new SampleApp( parameters );
            app.Execute();
        }
    }
}
