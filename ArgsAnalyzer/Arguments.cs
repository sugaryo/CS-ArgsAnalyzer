using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ArgsAnalyzer.Options;

namespace ArgsAnalyzer
{
	/// <summary>
	/// コマンドライン引数解析
	/// </summary>
	public class Arguments
	{
		/// <summary>パラメータ</summary>
		private readonly List<string> parameters = new List<string>();
		
		/// <summary>オプション指定の管理マップ</summary>
		private readonly Dictionary<string, Option> map = new Dictionary<string, Option>();

		#region Load / Add
		/// <summary>
		/// コマンドライン引数 <paramref name="args"/> を読み込み、
		/// 「パラメータ」と「オプション指定」に振り分けます。
		/// </summary>
		/// <remarks>
		/// <para>
		/// <code>"/"</code> または <code>"-"</code> で始まる引数は「オプション指定」として扱います。
		/// 先頭の <code>"/"</code> または <code>"-"</code> は除去されます。
		/// オプション指定は <seealso cref="AsOptions"/> で列挙を取得できます。
		/// </para>
		/// <para>
		/// なおオプション指定は
		/// 「値型オプション（<seealso cref="ValueOption"/>）」と
		/// 「属性型オプション（<seealso cref="PropertyOption"/>）」の２種類に分かれます。
		/// 前者は単一の値、後者はキーと値のペアで構成されます。
		/// 値型オプションと属性型オプションは、それぞれ
		/// <seealso cref="AsValueOptions"/> と
		/// <seealso cref="AsPropertyOptions"/> で列挙を取得できます。
		/// </para>
		/// <para>
		/// オプション指定でない場合、通常の「パラメータ」として扱います。
		/// パラメータは <seealso cref="AsParameters"/> で列挙を取得できます。
		/// </para>
		/// </remarks>
		/// 
		/// <param name="args">コマンドライン引数</param>
		/// 
		/// <example>
		/// <code>
		/// <![CDATA[
		/// var aa = new ArgsAnalyzer();
		/// aa.Load( args );
		/// 
		/// var parameters = aa.AsParameters();
		/// var options = aa.AsOptions();
		/// ]]>
		/// </code>
		/// </example>
		/// 
		/// <seealso cref="AsParameters"/>
		/// <seealso cref="AsOptions"/>
		/// <seealso cref="ValueOption"/>
		/// <seealso cref="PropertyOption"/>
		public void Load( IEnumerable<string> args )
		{
			foreach ( string arg in args )
			{
				this.Add( arg );
			}
		}
		public void Add( string arg )
		{
			if ( arg.startsWith( "/", "-" ) )
			{
				#region オプション
				string option = arg.TrimStart( '/', '-' );

				Option o;
				if ( option.Contains( ":" ) )
				{
					string[] token = option.split( ":" );

					o = new PropertyOption( token[0], token[1] );
				}
				else if ( option.Contains( "=" ) )
				{
					string[] token = option.split( "=" );

					o = new PropertyOption( token[0], token[1] );
				}
				else
				{
					o = new ValueOption( option );
				}
				
				if ( !this.map.ContainsKey( o.Name ) )
				{
					this.map.Add( o.Name, o );
				}
				#endregion
			}
			else
			{
				#region パラメータ
				this.parameters.Add( arg );
				#endregion
			}
		}
		#endregion

		#region Clear
		[Obsolete("正直 command line args って変化しないから一回Loadしたらもうそれで良い（Clear出来る意味がない）よね。")]
		/// <summary>
		/// このインスタンスが保持している「パラメータ」及び「オプション指定」を全て削除します。
		/// </summary>
		public void Clear()
		{
			this.parameters.Clear();
			this.map.Clear();
		}
		#endregion


		#region Have / Prop
		public bool Have( params string[] names )
		{
			foreach ( var name in names )
			{
				if ( this.map.ContainsKey( name ) ) return true;
			}
			return false;
		}
		public bool Prop( out string value, params string[] names )
		{
			foreach ( var name in names )
			{
				if ( this.map.ContainsKey( name ) )
				{
					Option o = this.map[name];
					if ( o is PropertyOption )
					{
						PropertyOption prop = o as PropertyOption;
						value = prop.value;
						return true;
					}
				}
			}
			value = null;
			return false;
		}
		#endregion


		#region IEnumerable

		/// <summary>
		/// 「パラメータ」の列挙を返します。
		/// </summary>
		/// <returns>パラメータ列挙</returns>
		public IEnumerable<string> AsParameters()
		{
			return this.parameters.AsEnumerable();
		}


		/// <summary>
		/// 「オプション指定」の列挙を返します。
		/// </summary>
		/// <returns>オプション指定列挙</returns>
		public IEnumerable<Option> AsOptions()
		{
			return this.map.Values.AsEnumerable();
		}
		/// <summary>
		/// 「値型オプション指定」の列挙を返します。
		/// </summary>
		/// <returns>値型オプション指定列挙</returns>
		public IEnumerable<ValueOption> AsValueOptions()
		{
			return this.AsOptions()
				.Where( x => x is ValueOption )
				.Select( x => x as ValueOption );
		}
		/// <summary>
		/// 「属性型オプション指定」の列挙を返します。
		/// </summary>
		/// <returns>属性型オプション指定列挙</returns>
		public IEnumerable<PropertyOption> AsPropertyOptions()
		{
			return this.AsOptions()
				.Where( x => x is PropertyOption )
				.Select( x => x as PropertyOption );
		}

		#endregion
	}
}
