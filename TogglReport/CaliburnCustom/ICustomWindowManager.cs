using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglReport.CaliburnCustom
{
    public interface ICustomWindowManager : IWindowManager
    {
        /// <summary>
        /// Shows a toast notification for the specified model.
        /// </summary>
        /// <param name="rootModel">The root model.</param>
        /// <param name="durationInMilliseconds">How long the notification should appear for.</param>
        /// <param name="settings">The optional notification settings.</param>
        /// <param name="context">The context.</param>
        void ShowNotification(object rootModel, int durationInMilliseconds, object context = null, IDictionary<string, object> settings = null);
    }
}
