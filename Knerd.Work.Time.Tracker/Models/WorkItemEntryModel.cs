using MyToolkit.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Knerd.Work.Time.Tracker.Models {

    public class GroupedWorkItemEntryModel : ObservableObject {

        public static IEnumerable<GroupedWorkItemEntryModel> GroupItems(IEnumerable<WorkItemEntryModel> items) {
            var groupedByCustomer = items.GroupBy(k => k.Call);
            foreach (var item in groupedByCustomer) {
                var workedTime = item.First().WorkedTime;
                foreach (var timeItem in item.Skip(1)) {
                    workedTime += timeItem.WorkedTime;
                }
                yield return new GroupedWorkItemEntryModel { CustomerCall = item.First().Customer + Environment.NewLine + item.Key, WorkedTime = workedTime.TotalHours };
            }
        }
        private double workedTime;

        public double WorkedTime {
            get { return workedTime; }
            set {
                if (workedTime != value) {
                    workedTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string customerCall;

        public string CustomerCall {
            get { return customerCall; }
            set {
                if (customerCall != value) {
                    customerCall = value;
                    RaisePropertyChanged();
                }
            }
        }
    }

    public class WorkItemEntryModel : ObservableObject {

        public WorkItemEntryModel() {
        }

        private TimeSpan beginTime = DateTime.Now - DateTime.Now.Date;
        private string call;
        private string customer;
        private DateTimeOffset date;

        private TimeSpan endTime = DateTime.Now - DateTime.Now.Date;

        private string workDone;

        public TimeSpan BeginTime {
            get { return beginTime; }
            set {
                if (beginTime != value) {
                    beginTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Call {
            get { return call; }
            set {
                if (call != value) {
                    call = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Customer {
            get { return customer; }
            set {
                if (customer != value) {
                    customer = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTimeOffset Date {
            get { return date; }
            set {
                if (date != value) {
                    date = value;
                    RaisePropertyChanged();
                }
            }
        }

        public TimeSpan EndTime {
            get { return endTime; }
            set {
                if (endTime != value) {
                    endTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string WorkDone {
            get { return workDone; }
            set {
                if (workDone != value) {
                    workDone = value;
                    RaisePropertyChanged();
                }
            }
        }

        public TimeSpan WorkedTime {
            get {
                return EndTime - BeginTime;
            }
        }
    }
}