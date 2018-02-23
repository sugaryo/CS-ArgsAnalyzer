using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgsAnalyzer
{
	/// <summary>
	/// 拡張メソッド
	/// </summary>
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
		public static string[] split( this string s, 
				params string[] by )
		{
			return s.Split( by, StringSplitOptions.None );
		}
		public static string[] split( this string s, 
				StringSplitOptions option, 
				params string[] by )
		{
			return s.Split( by, option );
		}

		public static Arguments parse( this string[] args )
		{
			var arguments = new Arguments();
			arguments.Load( args );
			return arguments;
		}
	}
}
