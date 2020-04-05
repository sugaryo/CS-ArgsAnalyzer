using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgsAnalyzer.Data
{
    /// <summary>
    /// 値型オプション
    /// </summary>
    /// <remarks>
    /// 値型オプションは、値 <code>value</code> だけを持つシンプルなオプションです。
    /// </remarks>
    public class ValueOption : Option
    {
        /// <summary>値</summary>
        public readonly string value;

        public ValueOption(string value)
        {
            this.value = value;
        }

        protected override string GetName()
        {
            return this.value;
        }

        public override string ToString()
        {
            return this.value;
        }
    }
}
