using Knerd.Work.Time.Tracker.Dialogs;
using MyToolkit.Command;
using MyToolkit.Model;
using System;
using System.Collections.ObjectModel;

namespace Knerd.Work.Time.Tracker.ViewModels {

    public class OverviewViewModel : ObservableObject {

        public OverviewViewModel() {
            AddNewWorkItemEntry = new RelayCommand(async () => {
                var diag = new AddWorkItemEntryDialog();
                await diag.ShowAsync();
            });
        }


        private DateTimeOffset? selectedDate;

        public DateTimeOffset? SelectedDate {
            get { return selectedDate; }
            set {
                if (selectedDate != value) {
                    selectedDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        private RelayCommand addNewWorkItemEntry;

        public RelayCommand AddNewWorkItemEntry {
            get { return addNewWorkItemEntry; }
            set {
                if (addNewWorkItemEntry != value) {
                    addNewWorkItemEntry = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<WorkItemEntryViewModel> workItems;

        public ObservableCollection<WorkItemEntryViewModel> WorkItems {
            get { return workItems; }
            set {
                if (workItems != value) {
                    workItems = value;
                    RaisePropertyChanged();
                }
            }
        }
    }

    public class WorkItemEntryViewModel : ObservableObject {
        private DateTime beginTime;
        private string customer;
        private DateTime date;
        private DateTime endTime;
        private string task;
        private string tfsCall;

        private DateTime timeWorked;

        public DateTime BeginTime {
            get { return beginTime; }
            set {
                if (beginTime != value) {
                    beginTime = value;
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

        public DateTime Date {
            get { return date; }
            set {
                if (date != value) {
                    date = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime EndTime {
            get { return endTime; }
            set {
                if (endTime != value) {
                    endTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Task {
            get { return task; }
            set {
                if (task != value) {
                    task = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string TfsCall {
            get { return tfsCall; }
            set {
                if (tfsCall != value) {
                    tfsCall = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DateTime TimeWorked {
            get { return timeWorked; }
            set {
                if (timeWorked != value) {
                    timeWorked = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}