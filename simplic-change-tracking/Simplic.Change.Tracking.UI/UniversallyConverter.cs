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
    public class UniversallyConverter : IValueConverter
    {
        private ILocalizationService localizationService;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return BooleanToStringConverter(val);
            }
            else if(value is string str)
            {
                return StringToStringConverter(str);
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
                    return "Erstellt";
                case CrudType.Update:
                    return "Bearbeitet";
                case CrudType.Delete:
                    return "Gelöscht";
                
            }
            return crud;
        }

        private object StringToStringConverter(string str)
        {
            switch (str)
            {
                case "True":
                    return "Ja";
                case "False":
                    return "Nein";
                case "IsMonday":
                    return "Montag";
                case "IsTuesday":
                    return "Dienstag";
                case "IsWednesday":
                    return "Mittwoch";
                case "IsThursday":
                    return "Donnerstag";
                case "IsFriday":
                    return "Freitag";
                case "IsSaturday":
                    return "Samstag";
                case "IsSunday":
                    return "Sonntag";
                case "Monday":
                    return "Montag";
                case "Tuesday":
                    return "Dienstag";
                case "Wednesday":
                    return "Mittwoch";
                case "Thursday":
                    return "Donnerstag";
                case "Friday":
                    return "Freitag";
                case "Saturday":
                    return "Samstag";
                case "Sunday":
                    return "Sonntag";
                case "WorkDays":
                    return "Arbeitstage";




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
            return value ? "Ja" : "Nein";
        }

    }
}
