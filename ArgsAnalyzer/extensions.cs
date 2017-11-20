using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgsAnalyzer
{
	public static class extensions
	{
		public static bool startsWith( this string s, params string[] with )
		{
			foreach ( string w in with )
			{
				if ( s.StartsWith( w ) ) return true;
			}

			return false;
		}

		public static string[] split( this string s, params string[] by )
		{
			return s.Split( by, StringSplitOptions.None );
		}
	}
}
