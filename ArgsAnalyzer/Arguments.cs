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

        private readonly List<Option> options = new List<Option>();

        private readonly Dictionary<string, ValueOption> values = new Dictionary<string, ValueOption>();
        private readonly Dictionary<string, PropertyOption> properties = new Dictionary<string, PropertyOption>();

        #region [propg] Parameterless / NoOptions / Empty
        public bool IsParameterless { get{ return 0 == this.parameters.Count; } }
        public bool IsNoOptions     { get{ return 0 == this.options.Count; } }
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

#warning FIXME：プロパティ型のパース処理がイケてない（バグってる）ので修正する。

                if ( option.Contains( ":" ) )
				{
					string[] token = option.split( ":" );

                    PropertyOption prop = new PropertyOption( token[0], token[1] );
                    if ( !this.properties.ContainsKey( prop.key ) )
                    {
                        this.properties.Add( prop.key, prop );
                        this.options.Add( prop );
                    }
                }
				else if ( option.Contains( "=" ) )
				{
					string[] token = option.split( "=" );

					PropertyOption prop = new PropertyOption( token[0], token[1] );
                    if ( !this.properties.ContainsKey( prop.Name ) )
                    {
                        this.properties.Add( prop.Name, prop );
                        this.options.Add( prop );
                    }
                }
				else
				{
					ValueOption value = new ValueOption( option );
                    if ( !this.values.ContainsKey( value.Name ) )
                    {
                        this.values.Add( value.Name, value );
                        this.options.Add( value );
                    }
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


        #region Clear(deplecated)
        [Obsolete("正直 command line args って変化しないから一回Loadしたらもうそれで良い（Clear出来る意味がない）よね。")]
		/// <summary>
		/// このインスタンスが保持している「パラメータ」及び「オプション指定」を全て削除します。
		/// </summary>
		public void Clear()
		{
			this.parameters.Clear();
			this.options.Clear();
            this.properties.Clear();
            this.values.Clear();
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
#warning FIXME：HaveはAnyとAllの２種類用意したほうが良い。
            foreach ( var name in names )
            {
                if ( this.properties.ContainsKey( name ) ) return true;
                if ( this.values.ContainsKey( name ) ) return true;
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
				if ( this.properties.ContainsKey( name ) )
				{
                    PropertyOption prop = this.properties[name];
					value = prop.value;
					return true;
				}
			}
			value = null;
			return false;
		}
		#endregion


		#region [IEnumerable] AsParameters / AsOptions / AsValueOptions / AsPropertyOptions

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
			return this.options.AsEnumerable();
		}
		/// <summary>
		/// 「値型オプション指定」の列挙を返します。
		/// </summary>
		/// <returns>値型オプション指定列挙</returns>
		public IEnumerable<ValueOption> AsValueOptions()
		{
            return this.values.Values.AsEnumerable();
		}
		/// <summary>
		/// 「属性型オプション指定」の列挙を返します。
		/// </summary>
		/// <returns>属性型オプション指定列挙</returns>
		public IEnumerable<PropertyOption> AsPropertyOptions()
		{
            return this.properties.Values.AsEnumerable();
		}

		#endregion
	}
}
