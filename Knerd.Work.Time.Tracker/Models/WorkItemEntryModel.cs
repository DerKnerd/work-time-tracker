using MyToolkit.Command;
using MyToolkit.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Knerd.Work.Time.Tracker.Models {

    public class GroupedWorkItemEntryModel : ObservableObject {

        private string customerCall;

        private double workedTime;

        public string CustomerCall
        {
            get { return customerCall; }
            set
            {
                if (customerCall != value)
                {
                    customerCall = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double WorkedTime
        {
            get { return workedTime; }
            set
            {
                if (workedTime != value)
                {
                    workedTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        public static IEnumerable<GroupedWorkItemEntryModel> GroupItemsDaily(IEnumerable<WorkItemEntryModel> items)
        {
            var groupedByCustomer = items.GroupBy(g => new { g.Customer, g.Call });
            foreach (var item in groupedByCustomer) {
                var workedTime = item.First().WorkedTime;
                foreach (var timeItem in item.Skip(1)) {
                    workedTime += timeItem.WorkedTime;
                }
                yield return new GroupedWorkItemEntryModel { CustomerCall = item.Key.Customer + Environment.NewLine + item.Key.Call, WorkedTime = workedTime.TotalHours };
            }
        }
        public static IEnumerable<GroupedWorkItemEntryModel> GroupItemsMonthly(IEnumerable<WorkItemEntryModel> items) {
            var groupedByCustomer = items.GroupBy(g => new { g.Customer, g.Call });
            foreach (var item in groupedByCustomer) {
                var workedTime = item.First().WorkedTime;
                foreach (var timeItem in item.Skip(1)) {
                    workedTime += timeItem.WorkedTime;
                }
                yield return new GroupedWorkItemEntryModel { CustomerCall = item.Key.Customer + Environment.NewLine + item.Key.Call, WorkedTime = workedTime.TotalHours };
            }
        }
    }

    public class WorkItemEntryModel : ObservableObject {

        private TimeSpan beginTime = DateTime.Now - DateTime.Now.Date;

        private string call;

        private string customer;

        private DateTimeOffset date;

        private TimeSpan endTime = DateTime.Now - DateTime.Now.Date;

        private long? id;

        private RelayCommand save;
        
        private string workDone;

        public WorkItemEntryModel()
        {
            Save = new RelayCommand(async () => {
                var connector = new SqliteConnector();
                if (Id.HasValue)
                {
                    await connector.UpdateWorkItemEntryAsync(this);
                }
                else
                {
                    Id = await connector.CreateWorkItemEntryAsync(this);
                }
            }, () => !string.IsNullOrEmpty(call) && !string.IsNullOrEmpty(customer) && !string.IsNullOrEmpty(workDone));
        }
        public TimeSpan BeginTime
        {
            get { return beginTime; }
            set
            {
                if (beginTime != value)
                {
                    beginTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Call
        {
            get { return call; }
            set
            {
                if (call != value)
                {
                    call = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Customer
        {
            get { return customer; }
            set
            {
                if (customer != value)
                {
                    customer = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTimeOffset Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    RaisePropertyChanged();
                }
            }
        }

        public TimeSpan EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        public long? Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RelayCommand Save
        {
            get { return save; }
            set {
                if (save != value) {
                    save = value;
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