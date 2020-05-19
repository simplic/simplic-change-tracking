using Simplic.Framework.UI;
using Simplic.Studio.UI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Simplic.Change.Tracking.UI
{
    public class RequestChangeViewModel : ExtendableViewModel
    {
        private DateTime changedOn;
        private bool isExpanded;
        private RequestChange model;
        private string change;
        private ObservableCollection<ChildViewModel> changes;
        private readonly IRequestChangeService requestChangeService;


        /// <summary>
        /// Constructor to get the model 
        /// </summary>
        /// <param name="model"></param>
        public RequestChangeViewModel(RequestChange model)
        {
            this.model = model;
        }

        /// <summary>
        /// Overloading constructor to initialize the model
        /// </summary>
        public RequestChangeViewModel()
        {
            model = new RequestChange();
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











    }

}