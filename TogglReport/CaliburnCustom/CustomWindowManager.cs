using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TogglReport.CaliburnCustom
{
    public class CustomWindowManager : WindowManager, ICustomWindowManager
    {
        public void ShowNotification(object rootModel, int durationInMilliseconds, object context = null, IDictionary<string, object> settings = null)
        {
            //var window = new NotificationWindow();
            //var view = ViewLocator.LocateForModel(rootModel, window, context);

            //ViewModelBinder.Bind(rootModel, view, null);
            //window.Content = (FrameworkElement)view;

            //ApplySettings(window, settings);

            //var activator = rootModel as IActivate;
            //if (activator != null)
            //{
            //    activator.Activate();
            //}

            //var deactivator = rootModel as IDeactivate;
            //if (deactivator != null)
            //{
            //    window.Closed += delegate { deactivator.Deactivate(true); };
            //}

            //window.Show(durationInMilliseconds);
        }
    }
}
