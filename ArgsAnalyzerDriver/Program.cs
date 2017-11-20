using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArgsAnalyzer.Options;

namespace ArgsAnalyzerDriver
{
	/// <summary>
	/// テスト用、及びサンプルコード代わりのドライバ実装。
	/// </summary>
	class Program
	{
		static void Main( string[] args )
		{
			// Debug args:
			//   hoge
			//   moge
			//   piyo
			//   -a
			//   -b
			//   /c
			//   /d
			//   -x=111
			//   /y=222,333,444
			ArgsAnalyzer.ArgsAnalyzer aa = new ArgsAnalyzer.ArgsAnalyzer();
			aa.Load( args );

			var parameters = aa.AsParameters();
			foreach ( string p in parameters )
			{
				Console.WriteLine( "parameter: " + p );
			}

			var v_options = aa.AsValueOptions();
			foreach ( ValueOption vo in v_options )
			{
				Console.WriteLine( "value-option: " + vo.value );
			}

			var p_options = aa.AsPropertyOptions();
			foreach ( PropertyOption po in p_options )
			{
				Console.WriteLine( "property-option: " + po.key + " = " + po.value );
			}
			
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("press any key to exit.");

			Console.ReadKey( false );
		}
	}
}
