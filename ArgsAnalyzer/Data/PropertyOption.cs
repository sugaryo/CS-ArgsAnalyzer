using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgsAnalyzer.Data
{
    /// <summary>
    /// 属性型オプション
    /// </summary>
    /// <remarks>
    /// 属性型オプションは、キー<code>key</code> と キーに紐づく値<code>value</code> のペアです。
    /// </remarks>
    public class PropertyOption : Option
    {
        /// <summary>キー</summary>
        public readonly string key;

        /// <summary>値</summary>
        public readonly string value;

        public PropertyOption(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        protected override string GetName()
        {
            return this.key;
        }

        public override string ToString()
        {
            return this.key + "=" + this.value;
        }
    }
}
