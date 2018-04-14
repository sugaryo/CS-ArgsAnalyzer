using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgsAnalyzer;
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
			Console.WriteLine( "psvm args:" );
			foreach ( var a in args )
			{
				Console.WriteLine( " -" + a );
			}
			
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
			// コピペ用：hoge moge piyo -a -b /c /d -x=111 /y=222,333,444
			Arguments arguments = args.parse();

			Console.WriteLine( "parameters:" );
			var parameters = arguments.AsParameters();
			foreach ( string p in parameters )
			{
				Console.WriteLine( " -" + p );
			}

			
			Console.WriteLine( "options(value):" );
			var v_options = arguments.AsValueOptions();
			foreach ( ValueOption vo in v_options )
			{
				Console.WriteLine( " -" + vo.value );
			}
			
			Console.WriteLine( "options(property):" );
			var p_options = arguments.AsPropertyOptions();
			foreach ( PropertyOption po in p_options )
			{
				Console.WriteLine( " -" + po.key + " = " + po.value );
			}

			// Have
			Console.WriteLine( "have option:" );
			Console.WriteLine( $" - args[a] : {arguments.Have( "a" )}" );

			// Prop
			Console.WriteLine( "prop option:" );
			string x;
			arguments.Prop( out x, "x" );
			Console.WriteLine( $" - args[x] : {x ?? "null"}" );


			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine( "press any key to exit." );

			Console.ReadKey( false );
		}
	}
}
