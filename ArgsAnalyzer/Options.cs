using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgsAnalyzer
{
	/// <summary>
	/// コマンドラインオプション
	/// </summary>
	/// <remarks>
	/// コマンドライン引数のうち、処理へのパラメータではなく、
	/// 動作オプションを指定する引数を示します。
	/// </remarks>
	public static class Options
	{
		public abstract class Option
		{
			public abstract string Get();
		};

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

			public ValueOption( string value )
			{
				this.value = value;
			}

			public override string Get()
			{
				return this.value;
			}

			public override string ToString()
			{
				return this.value;
			}
		}

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

			public PropertyOption( string key, string value )
			{
				this.key = key;
				this.value = value;
			}

			public override string Get()
			{
				return this.key;
			}

			public override string ToString()
			{
				return this.key + "=" + this.value;
			}
		}
	}
}
