using Simplic.Framework.UI;
using Simplic.Studio.UI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Telerik.Windows.Controls;

namespace Simplic.Change.Tracking.UI
{
    public class ChangeTrackingViewModel : ViewModelBase
    {
        private DateTime changedOn;
        private bool isExpanded;
        private ChangeTracking model;
        private string change;
        private ObservableCollection<ChildViewModel> changes;
        private ChangeTrackingKey changeTrackingKey;
        private readonly IChangeTrackingService requestChangeService;


        /// <summary>
        /// Constructor to get the model 
        /// </summary>
        /// <param name="model"></param>
        public ChangeTrackingViewModel(ChangeTracking model)
        {
            this.model = model;
        }

        /// <summary>
        /// Overloading constructor to initialize the model
        /// </summary>
        public ChangeTrackingViewModel(ChangeTrackingKey changeTrackingKey)
        {
            this.changeTrackingKey = changeTrackingKey; 
        }

        /// <summary>
        /// Gets or sets the crud type
        /// </summary>
        public CrudType Change
        {
            get
            {
                return model.Type;
            }
            set
            {
                 model.Type = value;
            }
           
        }

        /// <summary>
        /// Gets or sets the date time when the change occured 
        /// </summary>
        public DateTime ChangedOn
        {
            get
            {
                return this.changedOn;
            }
            set
            {
                this.changedOn = value;
            }
        }

        /// <summary>
        /// Gets or sets the user name
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
        /// Gets or sets the changes 
        /// </summary>
        public ObservableCollection<ChildViewModel> Changes
        {
            get
            {
                return this.changes;
            }
        }
        

        [Display(AutoGenerateField = false)]
        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }
            set
            {
                if (this.isExpanded != value)
                {
                    this.isExpanded = value;

                    this.LoadChildren();

                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        public void LoadChildren()
        {
            if (this.changes == null)
            {

                this.changes = new ObservableCollection<ChildViewModel>();//Add IEnumerable inside brackets 
                this.OnPropertyChanged("Items");
            }
        }
    }
}

