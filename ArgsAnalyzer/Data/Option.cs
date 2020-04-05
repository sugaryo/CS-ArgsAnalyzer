using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgsAnalyzer.Data
{
    public abstract class Option
    {
        public string Name { get { return this.GetName(); } }
        protected abstract string GetName();
    }
}
