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

        public OverviewViewModel() {
            AddNewWorkItemEntry = new RelayCommand(async () => {
                var diag = new AddWorkItemEntryDialog(SelectedDate.GetValueOrDefault());
                await diag.ShowAsync();
                await ReloadWorkItems();
            });
            WorkItems = new ObservableCollection<Models.WorkItemEntryModel>();
            GroupedWorkItems = new ObservableCollection<Models.GroupedWorkItemEntryModel>();
            SelectedDate = DateTime.Now;
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

            GroupedWorkItems.Clear();
            foreach (var item in GroupedWorkItemEntryModel.GroupItems(items)) {
                GroupedWorkItems.Add(item);
            }
        }

        private ObservableCollection<GroupedWorkItemEntryModel> groupedWorkItems;

        public ObservableCollection<GroupedWorkItemEntryModel> GroupedWorkItems {
            get { return groupedWorkItems; }
            set {
                if (groupedWorkItems != value) {
                    groupedWorkItems = value;
                    RaisePropertyChanged();
                }
            }
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

        private ObservableCollection<WorkItemEntryModel> workItems;

        public ObservableCollection<WorkItemEntryModel> WorkItems {
            get { return workItems; }
            set {
                if (workItems != value) {
                    workItems = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}