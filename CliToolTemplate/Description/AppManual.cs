using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliToolTemplate.Description
{
    public class AppManual
    {
        public DescriptionInfo Summary { get; set; }

        public DescriptionInfo MainParameter { get; set; }

        public List<DescriptionInfo> Options { get; set; }

    }
}
