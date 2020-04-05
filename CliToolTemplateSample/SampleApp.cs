using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CliToolTemplate;
using ArgsAnalyzer;

namespace CliToolTemplateSample
{
    class SampleApp : ConsoleAppBase
    {
        #region ctor
        public SampleApp(string[] args) : base(args)
        {
            base.Title = "sample app.";
            base.Width = 100;
        }
        #endregion

        protected override void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
