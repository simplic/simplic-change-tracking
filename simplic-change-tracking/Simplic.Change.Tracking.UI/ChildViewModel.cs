using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simplic.Change.Tracking.Schemas;
using Simplic.Localization;
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
        private ILocalizationService localizationService;
        public ObservableCollection<ChildViewModel> props;
        private Variance variance;
        private IList<Variance> variances;
        private bool isExpanded;
        private bool isExpandable;
        private object oldValue;
        private string propertyName;
        private string localizationKey;
        private object newValue;
        ChangeTrackingObject changeTrackingObject;

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
            localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
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
                if (!(string.IsNullOrEmpty(localizationKey)))
                {
                    return localizationService.Translate(localizationKey);
                }
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
            bool found = false;
            if (this.props == null || this.props.Count() < 1)
            {
                this.props = new ObservableCollection<ChildViewModel>();
                string json = await Task.Run(() => changeTrackingService.GetJson(model.Ident));
                ChildViewModel child = new ChildViewModel();

                if (changeTrackingObject == null)
                {

                    changeTrackingObject = JsonConvert.DeserializeObject<ChangeTrackingObject>(json);

                }

                var data = JObject.Parse(json);
                data = changeTrackingObject.Data;


                var toParse = new List<JToken>();
                toParse.AddRange(data.Children<JToken>());



                while (toParse.Count > 0)
                {
                    var copy = toParse.ToList();
                    var newItems = new List<JToken>();


                    foreach (var token in copy)
                    {
                        var firstvalue = token.Values().FirstOrDefault();
                        child = new ChildViewModel();
                        child.Change = model.CrudType;
                        child.UserName = model.UserName;
                        if (firstvalue.Type == JTokenType.Property)
                        {
                            child.IsExpandable = true;
                            child.PropertyName = firstvalue.ToString();

                            newItems.AddRange(token.Children<JToken>());
                        }
                        else
                        {

                            foreach (var item in changeTrackingObject.Schema.Properties)
                            {
                                if (item.LocalizationKey== null)
                                {
                                    continue;
                                }
                                if (item.Path.Contains(token.Path))
                                {
                                    child.localizationKey = item.LocalizationKey;
                                    

                                }
                            }
                            if (string.IsNullOrWhiteSpace(child.localizationKey))
                            {
                                child.localizationKey = token.Path;
                            }

                           // child.localizationKey = (from properties in changeTrackingObject.Schema.Properties
                           //                          where (properties.LocalizationKey != null )
                           //                          where properties.LocalizationKey.Equals(token.Path)
                           //                          select properties).FirstOrDefault().LocalizationKey ?? "null";

                            if (token.First.HasValues)
                            {
                                child.OldValue = token.First.First;
                                child.NewValue = token.First.Last;

                            }
                            else
                            {
                                child.oldValue = token.First;
                            }
                            child.UserName = "";
                            child.ChangedOn = model.TimeStampChange;
                            child.IsExpandable = false;

                        }
                        props.Add(child);

                    }

                    toParse.Clear();
                    toParse.AddRange(newItems);
                }

                OnPropertyChanged(nameof(propertyName));
                this.OnPropertyChanged(nameof(Properties));
                this.OnPropertyChanged(nameof(propertyName));
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
        public ObservableCollection<ChildViewModel> Properties
        {
            get => props;
            set => props = value;
        }

        /// <summary>
        /// Gets or sets the list of variance 
        /// </summary>
        public IList<Variance> Variances
        {
            get => this.variances;
            set => this.variances = value;
        }

    }
}
