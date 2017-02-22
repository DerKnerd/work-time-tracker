using MyToolkit.Model;
using System;

namespace Knerd.Work.Time.Tracker.Models {

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