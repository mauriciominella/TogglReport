using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglReport.Domain.ViewModel
{
    [Export(typeof(IScreen))]
    public class ReportViewModel : Screen
    {
        public ReportViewModel()
        {
            DisplayName = "Reports";
        }
    }
}
