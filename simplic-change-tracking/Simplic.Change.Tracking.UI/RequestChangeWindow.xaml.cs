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
        public ChangeTrackingWindow()
        {
            InitializeComponent();
        }

        public override void OnOpenPage(WindowOpenPageEventArg e)
        {
            if (e.CurrentObject is ChangeTrackingKey model)
            {
                DataContext = new ChangeTrackingViewModel(model);
            }
            base.OnOpenPage(e);
        }

        void IChangeTrackingWindow.ShowDialog()
        {
            throw new NotImplementedException();
        }
    }

}
