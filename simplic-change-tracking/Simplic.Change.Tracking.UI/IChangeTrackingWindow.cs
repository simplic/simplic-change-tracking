using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking.UI
{
    public interface IChangeTrackingWindow
    {
        void Show();
        void AddPagingObject(object pagingObject);
        bool? ShowDialog();
        void SetKey(ChangeTrackingKey key);
        
    }
}
