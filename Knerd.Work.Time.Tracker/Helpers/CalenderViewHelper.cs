using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Knerd.Work.Time.Tracker.Helpers {

    public static class CalendarViewHelper {

        public static DateTimeOffset GetSelectedDate(DependencyObject obj) {
            return (DateTimeOffset)obj.GetValue(SelectedDateProperty);
        }

        public static void SetSelectedDate(DependencyObject obj, DateTimeOffset value) {
            obj.SetValue(SelectedDateProperty, value);
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.RegisterAttached("SelectedDate", typeof(DateTimeOffset), typeof(CalendarView),
                new PropertyMetadata(null, (d, e) => {
                    var cv = d as CalendarView;
                    var dates = e.NewValue as IList<DateTimeOffset>;

                    if (cv != null && dates != null && dates.Count > 0) {
                        cv.SelectedDates.Add(dates[0]);
                    }
                }));
    }
}