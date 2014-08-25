using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahgora.Api.Model
{
    public class Summary
    {
        public string worked { get; set; }
        public string weeklyDSR { get; set; }
        public string overtime { get; set; }
        public string absent { get; set; }
        public string offwork { get; set; }
        public string discountedDSR { get; set; }
    }
}
