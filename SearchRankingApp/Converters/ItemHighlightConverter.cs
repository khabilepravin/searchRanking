using APIProxy.ResposeModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SearchRankingApp.Converters
{
    public class ItemHighlightConverter : IMultiValueConverter
    {        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 1)
            {
                var currentResult = values[0] as SearchRankingResult;
                var searchedDomain = values[1] as string;

                return currentResult?.Domain.IndexOf(searchedDomain, StringComparison.InvariantCultureIgnoreCase) >= 0;
            }
            else { return false; }
        }       

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
