﻿using ChatApp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatApp
{
    /// <summary>
    /// A converter that takes in a date converts to a user friendly time
    /// </summary>
    public class TimeToDisplayTimeConverter : BaseValueConverter<TimeToDisplayTimeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the time passed in 
            var time = (DateTimeOffset)value;

            // If it is today...
            if (time.Date == DateTimeOffset.UtcNow.Date)
                return time.ToLocalTime().ToString("HH:mm");

            // Otherwise, return a full date
            return time.ToLocalTime().ToString("HH:mm, MMM yyyy");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
