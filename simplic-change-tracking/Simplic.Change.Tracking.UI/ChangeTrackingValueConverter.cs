using Simplic.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Simplic.Change.Tracking.UI
{
    public class ChangeTrackingValueConverter : IValueConverter
    {
        private ILocalizationService localizationService;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
            if (value is bool || value is Boolean)
            {
                var val = System.Convert.ToBoolean(value);
                return BooleanToStringConverter(val);
            }
            else if(value is string  || value is String )
            {
                string s = System.Convert.ToString(value);
                return StringToStringConverter(s);
            }
            else if (value is CrudType crud)
            {
                return CrudTypeToStringConverter(crud);
            }
            else
            {
                return value;
            }
            
        }

        private object CrudTypeToStringConverter(CrudType crud)
        {
            switch (crud)
            {
                case CrudType.Insert:
                    return localizationService.Translate("insert");
                case CrudType.Update:
                    return localizationService.Translate("update");
                case CrudType.Delete:
                    return localizationService.Translate("delete");
                
            }
            return crud;
        }

        private object StringToStringConverter(string str)
        {
            switch (str)
            {
                case "True":
                    return localizationService.Translate("True");
                case "False":
                    return localizationService.Translate("False");
                
                case "WorkDays":
                    return localizationService.Translate("hr_workdays");




                default:
                    break;
            }
            return str;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private object BooleanToStringConverter(bool value)
        {
            return value ? localizationService.Translate("true") : localizationService.Translate("false");
        }

    }
}
