using Knerd.Work.Time.Tracker.Dialogs;
using Knerd.Work.Time.Tracker.Models;
using MyToolkit.Command;
using MyToolkit.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Knerd.Work.Time.Tracker.ViewModels {

    public class OverviewViewModel : ObservableObject {

        private RelayCommand addNewWorkItemEntry;

        private ObservableCollection<GroupedWorkItemEntryModel> dailyGroupedWorkItems;

        private ObservableCollection<GroupedWorkItemEntryModel> monthlyGroupedWorkItems;

        private DateTimeOffset? selectedDate;

        private ObservableCollection<WorkItemEntryModel> workItems;

        public OverviewViewModel() {
            AddNewWorkItemEntry = new RelayCommand(async () => {
                var diag = new AddWorkItemEntryDialog(SelectedDate.GetValueOrDefault());
                await diag.ShowAsync();
                await ReloadWorkItems();
            });
            WorkItems = new ObservableCollection<Models.WorkItemEntryModel>();
            DailyGroupedWorkItems = new ObservableCollection<Models.GroupedWorkItemEntryModel>();
            MonthlyGroupedWorkItems = new ObservableCollection<Models.GroupedWorkItemEntryModel>();
            SelectedDate = DateTime.Now;
        }

        public RelayCommand AddNewWorkItemEntry {
            get { return addNewWorkItemEntry; }
            set {
                if (addNewWorkItemEntry != value) {
                    addNewWorkItemEntry = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<GroupedWorkItemEntryModel> DailyGroupedWorkItems {
            get { return dailyGroupedWorkItems; }
            set {
                if (dailyGroupedWorkItems != value) {
                    dailyGroupedWorkItems = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<GroupedWorkItemEntryModel> MonthlyGroupedWorkItems {
            get { return monthlyGroupedWorkItems; }
            set {
                if (monthlyGroupedWorkItems != value) {
                    monthlyGroupedWorkItems = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTimeOffset? SelectedDate {
            get { return selectedDate; }
            set {
                if (selectedDate != value) {
                    selectedDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<WorkItemEntryModel> WorkItems {
            get { return workItems; }
            set {
                if (workItems != value) {
                    workItems = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected override async void RaisePropertyChanged(PropertyChangedEventArgs args) {
            base.RaisePropertyChanged(args);
            if (args.IsProperty<OverviewViewModel>(s => s.SelectedDate)) {
                await ReloadWorkItems();
            }
        }

        private async Task ReloadWorkItems() {
            var items = await new SqliteConnector().GetWorkItems(SelectedDate.Value);
            WorkItems.Clear();
            foreach (var item in items) {
                WorkItems.Add(item);
            }

            DailyGroupedWorkItems.Clear();
            foreach (var item in GroupedWorkItemEntryModel.GroupItemsDaily(items)) {
                DailyGroupedWorkItems.Add(item);
            }

            MonthlyGroupedWorkItems.Clear();
            foreach (var item in GroupedWorkItemEntryModel.GroupItemsMonthly(await new SqliteConnector().GetWorkItemsForMonth(SelectedDate.Value))) {
                MonthlyGroupedWorkItems.Add(item);
            }
        }
    }
}