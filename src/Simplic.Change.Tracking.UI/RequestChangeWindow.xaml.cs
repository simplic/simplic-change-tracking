using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simplic.Change.Tracking.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ChangeTrackingWindow : DefaultRibbonWindow, IChangeTrackingWindow
    {
        private ChangeTrackingKey key;
        public ChangeTrackingWindow()
        {
            InitializeComponent();
            RadRibbonDataGroup.Visibility = Visibility.Collapsed;
            if (key != null)
            {
                DataContext = new ChangeTrackingViewModel(key);
            }
            
        }

        public override void OnOpenPage(WindowOpenPageEventArg e)
        {
            if (e.CurrentObject is ChangeTrackingKey model)
            {
                DataContext = new ChangeTrackingViewModel(model);
            }
            else
            {
                DataContext = new ChangeTrackingViewModel(new ChangeTracking());
            }
            
            base.OnOpenPage(e);
        }

        void IChangeTrackingWindow.ShowDialog()
        {
            throw new NotImplementedException();
        }
        public void SetKey(ChangeTrackingKey key)
        {
            this.key = key;
            DataContext = new ChangeTrackingViewModel(key);
        }
        
    }

}
