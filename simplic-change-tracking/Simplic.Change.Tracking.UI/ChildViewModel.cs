using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Simplic.Change.Tracking.UI
{
    public class ChildViewModel : ViewModelBase
    {
  
        ChangeTracking model;
        private IChangeTrackingService changeTrackingService;
        public ObservableCollection<ChildViewModel> props;
        private Variance variance;
        private IList<Variance> variances;
        private bool isExpanded;
        private bool isExpandable;
        private object oldValue;
        private string propertyName;
        private object newValue;

        /// <summary>
        /// Constructor to get the model - type request change
        /// </summary>
        /// <param name="model"></param>
        public ChildViewModel(ChangeTracking model)
        {

            this.model = model;
            init();

        }
        /// <summary>
        /// Only to use it in this class 
        /// </summary>
        private ChildViewModel()
        {
            model = new ChangeTracking();
            init();
        }

        private void init()
        {
            changeTrackingService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IChangeTrackingService>();
            props = new ObservableCollection<ChildViewModel>();
            variances = new List<Variance>();
            this.isExpandable = true;
        }

        /// <summary>
        /// Gets or sets the crud type of the change
        /// 0 = Insert
        /// 1 = Update
        /// 2 = Delete
        /// </summary>
        public CrudType Change
        {
            get
            {
                return model.CrudType;
            }
            set
            {
                model.CrudType = value;
            }
        }

        /// <summary>
        /// Gets or sets the date time when the change occured 
        /// </summary>
        public DateTime ChangedOn
        {
            get
            {
                return model.TimeStampChange;
            }
            set
            {
                model.TimeStampChange = value;
            }
        }

        /// <summary>
        /// Gets or sets the user name as a string
        /// </summary>
        public string UserName
        {
            get
            {
                return model.UserName;
            }
            set
            {
                model.UserName = value;
            }
        }

        /// <summary>
        /// Gets or sets the old value as a string 
        /// </summary>
        public string PropertyName
        {
            get
            {
                return this.propertyName;
            }
            set
            {
                this.propertyName = value;
            }
        }

        /// <summary>
        /// Gets and sets the old value as a string
        /// </summary>
        public object OldValue
        {
            get
            {
                return oldValue;
            }
            set
            {
                this.oldValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the new value as a string
        /// </summary>
        public object NewValue
        {
            get
            {
                return this.newValue;
            }
            set
            {
                this.newValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the variance to see the differences
        /// </summary>
        public Variance Difference
        {
            get
            {
                if (variance == null)
                {
                    return variance = JsonConvert.DeserializeObject<Variance>(model.JsonObject);
                }
                return variance;
            }
            set
            {
                variance = value;
            }

        }

        public bool IsExpanded
        {
            get => this.isExpanded;
            set
            {
                if (this.isExpanded != value)
                {
                    this.isExpanded = value;

                    _ = this.LoadChildren();

                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        private async Task LoadChildren()
        {
            if (this.props == null || this.props.Count() < 1)
            {
                this.props = new ObservableCollection<ChildViewModel>();
                string json = await Task.Run(() => changeTrackingService.GetJson(model.Ident));

                variances = JsonConvert.DeserializeObject<List<Variance>>(json);
                foreach (var item in variances)
                {
                    if (!(item.Property.Equals("Snapshot")))
                    {


                        var child = new ChildViewModel
                        {
                            Change = model.CrudType,
                            PropertyName = item.Property,
                            NewValue = item.NewValue,
                            OldValue = item.OldValue,
                            UserName = model.UserName,
                            ChangedOn = model.TimeStampChange,
                            isExpandable = false

                        };
                       
                        props.Add(child);
                    }

                }



                this.OnPropertyChanged("Props");
                return;
            }
        }
        public bool IsExpandable
        {
            get
            {
                return isExpandable;
            }
            set => isExpandable = value;
        }
        /// <summary>
        /// Gets or sets the child item for the tree view that contains the changes
        /// </summary>
        public ObservableCollection<ChildViewModel> Props
        {
            get => props;
            set => props = value;
        }
        public IList<Variance> Variances
        {
            get => this.variances;
            set => this.variances = value;
        }
    }
}
