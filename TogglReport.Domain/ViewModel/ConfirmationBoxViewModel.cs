using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglReport.Domain.ViewModel
{
    public class ConfirmationBoxViewModel : Screen
    {
        public void OK()
        {
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        } 
    }
}
