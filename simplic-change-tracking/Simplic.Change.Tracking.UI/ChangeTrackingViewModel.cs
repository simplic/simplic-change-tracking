using Simplic.Framework.UI;
using Simplic.Studio.UI;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Telerik.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Simplic.Change.Tracking.UI
{
    public class ChangeTrackingViewModel : ViewModelBase
    {
        private DateTime changedOn;
        private bool isExpanded;
        private ChangeTracking model;
        private string change;
        private ObservableCollection<ChildViewModel> changes = new ObservableCollection<ChildViewModel>();
        private ChangeTrackingKey changeTrackingKey;
        private IChangeTrackingService changeTrackingService;
        private bool isEmpty;
        private bool isExpandable;


        /// <summary>
        /// Constructor to get the model 
        /// </summary>
        /// <param name="model"></param>
        public ChangeTrackingViewModel(ChangeTracking model)
        {
            this.model = model;
            init();
            changeTrackingService.GetChanges(Guid.Parse("DA9347AC-6C49-4E27-9DC7-60591E842AA4"));
            
            
        }

        private void init()
        {
            changeTrackingService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IChangeTrackingService>();
            var collection = changeTrackingService.GetChanges(Guid.Parse("DA9347AC-6C49-4E27-9DC7-60591E842AA4"));
            foreach (var item in collection)
            {
                var model = new ChildViewModel(item);

                changes.Add(model);
            };
        }

        /// <summary>
        /// Overloading constructor to initialize the model
        /// </summary>
        public ChangeTrackingViewModel(ChangeTrackingKey changeTrackingKey)
        {
            this.changeTrackingKey = changeTrackingKey;
            init();
            model = new ChangeTracking();
            
        }

        /// <summary>
        /// Gets or sets the crud type
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
            set
            {
                this.changes = value;
                OnPropertyChanged("Changes");
            }
        }


        [Display(AutoGenerateField = false)]
        public bool IsExpanded
        {
            get
            {
                return false;
            }
            set
            {
                if (this.isExpanded != value)
                {
                    this.isExpanded = value;

                    

                    OnPropertyChanged("IsExpanded");
                }
            }
        }
        [Display(AutoGenerateField = false)]
        public bool IsExpandable
        {
            get
            {
                return this.isExpandable ;
            }
            set
            {
                this.isExpandable = value;
            }
           
        }



    }
}

