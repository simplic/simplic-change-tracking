using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking.UI
{
    public class ChildViewModel
    {
        private string change;
        private DateTime changedOn;
        private int userName;
        ChangeTracking model;
        public ObservableCollection<ChildViewModel> props;
        private Variance variance;

        /// <summary>
        /// Constructor to get the model - type request change
        /// </summary>
        /// <param name="model"></param>
        public ChildViewModel(ChangeTracking model)
        {
            props = new ObservableCollection<ChildViewModel>();
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
               return Difference.Property;
            }
            set
            {
                variance.Property = value;
            }
        }

        /// <summary>
        /// Gets and sets the old value as a string
        /// </summary>
        public string OldValue
        {
            get 
            {
                return Difference.OldValue.ToString();
            }
            set
            {
                variance.OldValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the new value as a string
        /// </summary>
        public string NewValue
        {
            get
            {
                return Difference.NewValue.ToString();
            }
            set
            {
                variance.NewValue = value;
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
        /// <summary>
        /// Gets or sets the child item for the tree view that contains the changes
        /// </summary>
        public ObservableCollection<ChildViewModel> Props
        {
            get => props;
            set => props = value;
        }
    }
}
