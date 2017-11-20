using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ArgsAnalyzer.Options;

namespace ArgsAnalyzer
{
    public class ArgsAnalyzer
    {
		private readonly List<string> parameters = new List<string>();
		private readonly List<Option> options = new List<Option>();
		

		public void Load( IEnumerable<string> args )
		{	
			foreach ( string arg in args )
			{
				if ( arg.startsWith( "/", "-" ) )
				{
					string option = arg.TrimStart( '/', '-' );
					
					if ( option.Contains( "=" ) )
					{
						string[] token = option.split( "=" );

						Option o = new PropertyOption( token[0], token[1] );
						this.options.Add( o );
					}
					else
					{
						Option o = new ValueOption( option );
						this.options.Add( o );
					}
				}
				else
				{
					this.parameters.Add( arg );
				}
			}
		}

		public void Clear()
		{
			this.parameters.Clear();
			this.options.Clear();
		}


		public IEnumerable<string> AsParameters()
		{
			return this.parameters.AsEnumerable();
		}

		public IEnumerable<Option> AsOptions()
		{
			return this.options.AsEnumerable();
		}
		public IEnumerable<ValueOption> AsValueOptions()
		{
			return this.options
				.Where( x => x is ValueOption )
				.Select( x => x as ValueOption );
		}
		public IEnumerable<PropertyOption> AsPropertyOptions()
		{
			return this.options
				.Where( x => x is PropertyOption )
				.Select( x => x as PropertyOption );
		}
    }
}
