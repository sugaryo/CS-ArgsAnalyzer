using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliToolTemplate.Description
{
    public class DescriptionInfo
    {
        public string Label { get; set; }
        public string Text { get; set; }

        public DescriptionInfo()
        {

        }
        public DescriptionInfo(string label, string text)
        {
            this.Label = label;
            this.Text = text;
        }
    }
}
