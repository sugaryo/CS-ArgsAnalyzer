using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArgsAnalyzer.Data;

namespace ArgsAnalyzer
{
	/// <summary>
	/// コマンドライン引数解析
	/// </summary>
	public class Arguments
	{
		/// <summary>パラメータ</summary>
		private readonly List<string> parameters = new List<string>();

#warning FIXME：OptionでMap作るよりはValuesとPropertiesで分けた方が良い気がしてきた。
        /// <summary>オプション指定の管理マップ</summary>
        private readonly Dictionary<string, Option> map = new Dictionary<string, Option>();


        #region [propg] Parameterless / NoOptions / Empty
        public bool IsParameterless { get{ return 0 == this.parameters.Count; } }
        public bool IsNoOptions     { get{ return 0 == this.map.Count; } }
        public bool IsEmpty         { get{ return this.IsParameterless && this.IsNoOptions; } }
        #endregion


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
        /// （オプションに関しては元の順序は保持されません）
        /// </para>
        /// <para>
        /// また、<seealso cref="Have"/> で指定した名前のオプションの有無を、
        /// <seealso cref="Prop"/> で指定した名前の属性型オプションの有無と属性値を、
        /// それぞれ取得できます。
        /// </para>
        /// <para>
        /// なおオプション指定でない場合、通常の「パラメータ」として扱います。
        /// パラメータは <seealso cref="AsParameters"/> で列挙を取得できます。
        /// </para>
        /// </remarks>
        /// 
        /// <param name="args">コマンドライン引数</param>
        /// 
        /// <example>
        /// 基本的な使い方。
        /// <code>
        /// <![CDATA[
        /// Arguments arguments = new Arguments();
        /// arguments.Load( args );
        /// 
        /// var parameters = arguments.AsParameters();
        /// var options    = arguments.AsOptions();
        /// ]]>
        /// </code>
        /// _extensions.cs では string[] に拡張メソッドを追加しているので、上記は以下と同等。
        /// <code>
        /// <![CDATA[
        /// Arguments arguments = args.parse();
        /// 
        /// var parameters = arguments.AsParameters();
        /// var options    = arguments.AsOptions();
        /// ]]>
        /// </code>
        /// </example>
        /// 
        /// <seealso cref="AsParameters"/>
        /// <seealso cref="AsOptions"/>
        /// <seealso cref="Have"/>
        /// <seealso cref="Prop"/>
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
		/// <summary>
		/// オプション値の有無判定
		/// </summary>
		/// <param name="names">判定するオプション名（複数指定可）</param>
		/// <returns>解析したコマンドライン引数 <code>args</code> に、
		/// 指定したオプションが含まれる場合は <code>true</code> を、
		/// 含まれていない場合は <code>false</code> を、それぞれ返します。 
		/// </returns>
		public bool Have( params string[] names )
		{
			foreach ( var name in names )
			{
				if ( this.map.ContainsKey( name ) ) return true;
			}
			return false;
		}
		/// <summary>
		/// 属性型オプションの有無判定と属性値の取得
		/// </summary>
		/// <param name="value">属性値の <code>out parameter</code> </param>
		/// <param name="names">判定するオプション名（複数指定可）</param>
		/// <returns>解析したコマンドライン引数 <code>args</code> に、
		/// 指定した属性型オプションが含まれる場合は <code>true</code> を、
		/// 含まれていない場合は <code>false</code> を返し、
		/// 含まれる場合はその <seealso cref="PropertyOption.value"/> を
		/// <code>out parameter</code> に設定します。
		/// </returns>
		/// <seealso cref="PropertyOption"/>
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
