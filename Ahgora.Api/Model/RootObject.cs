using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahgora.Api.Model
{
    public class RootObject
    {

        public Summary summary { get; set; }
        public List<Timesheet> timesheet { get; set; }

        public RootObject()
        {
            this.timesheet = new List<Timesheet>();
            this.summary = new Summary();
        }

    }
}
