using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgsAnalyzer
{
	public static class Options
	{
		public interface Option { };

		public class ValueOption : Option
		{
			public readonly string value;

			public ValueOption(string value)
			{
				this.value = value;
			}

			public override string ToString()
			{
				return this.value;
			}
		}

		public class PropertyOption : Option
		{
			public readonly string key;
			public readonly string value;

			public PropertyOption(string key, string value)
			{
				this.key = key;
				this.value = value;
			}

			public override string ToString()
			{
				return this.key + "=" + this.value;
			}

		}
	}
}
