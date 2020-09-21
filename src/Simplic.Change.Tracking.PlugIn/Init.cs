using Simplic.Change.Tracking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Simplic.Change.Tracking.PlugIn
{
    public class Init
    {
        public static void Initialize()
        {
            var container = CommonServiceLocator.ServiceLocator.Current.GetInstance<IUnityContainer>();
            container.RegisterType<IChangeTrackingService, ChangeTrackingService>();
        }

    }
}
