using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using FirstFloor.ModernUI.Presentation;
using System.ComponentModel;
using FirstFloor.ModernUI;
using System.Windows.Threading;

namespace Work.Time.Tracker {

    [Serializable]
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;



        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName) {
            var handler = this.PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Serializable]
    public class WorkItem : NotifyPropertyChanged, ISerializable {

        public WorkItem() {
            var sTime = DateTime.Now.TimeOfDay;
            this.StartTime = new TimeSpan(sTime.Hours, sTime.Minutes, sTime.Seconds);
            this.IsTouched = false;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => {
                var time = DateTime.Now.TimeOfDay;
                this.StartTime = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
            };
            timer.Start();
        }

        public WorkItem(SerializationInfo info, StreamingContext context) {
            this.Project = info.GetString("Project");
            this.Description = info.GetString("Description");
            this.EndTime = (TimeSpan)info.GetValue("EndTime", typeof(TimeSpan));
            this.StartTime = (TimeSpan)info.GetValue("StartTime", typeof(TimeSpan));
            this.WorkedTime = (TimeSpan)info.GetValue("WorkedTime", typeof(TimeSpan));
        }

        [NonSerialized]
        private DispatcherTimer timer;

        [NonSerialized]
        private bool isTouched;

        public bool IsTouched {
            get { return isTouched; }
            set {
                if (isTouched != value) {
                    isTouched = value;
                    if (timer != null) {
                        timer.Stop();
                    }
                    OnPropertyChanged("IsTouched");
                }
            }
        }

        private string project;

        public string Project {
            get { return project; }
            set {
                if (project != value) {
                    project = value;
                    IsTouched = true;
                    OnPropertyChanged("Project");
                }
            }
        }

        private string description;

        public string Description {
            get { return description; }
            set {
                if (description != value) {
                    description = value;
                    IsTouched = true;
                    OnPropertyChanged("Description");
                }
            }
        }

        private TimeSpan startTime;

        public TimeSpan StartTime {
            get { return startTime; }
            set {
                if (startTime != value) {
                    startTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }

        private TimeSpan endTime;

        public TimeSpan EndTime {
            get { return endTime; }
            set {
                if (endTime != value) {
                    endTime = value;
                    WorkedTime = EndTime.Subtract(StartTime);
                    OnPropertyChanged("EndTime");
                }
            }
        }

        private TimeSpan workedTime;

        public TimeSpan WorkedTime {
            get { return workedTime; }
            set {
                if (workedTime != value) {
                    workedTime = value;
                    OnPropertyChanged("WorkedTime");
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("Project", Project);
            info.AddValue("Description", Description);
            info.AddValue("StartTime", StartTime);
            info.AddValue("EndTime", EndTime);
            info.AddValue("WorkedTime", WorkedTime);
        }
    }

    [Serializable]
    public class DayItem : NotifyPropertyChanged, ISerializable {

        public DayItem() {
            this.WorkItems = new ObservableCollection<WorkItem>();
            this.Day = DateTime.Now.Date;
        }

        public DayItem(SerializationInfo info, StreamingContext context) {
            this.Day = info.GetDateTime("Day");
            this.WorkItems = info.GetValue("WorkItems", typeof(ObservableCollection<WorkItem>)) as ObservableCollection<WorkItem>;
        }

        private ObservableCollection<WorkItem> workItems;

        public ObservableCollection<WorkItem> WorkItems {
            get { return workItems; }
            set {
                if (workItems != value) {
                    workItems = value;
                    OnPropertyChanged("WorkItems");
                }
            }
        }

        private DateTime day;

        public DateTime Day {
            get { return day; }
            set {
                if (day != value) {
                    day = value;
                    OnPropertyChanged("Day");
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("WorkItems", WorkItems);
            info.AddValue("Day", Day);
        }
    }

    [Serializable]
    public class MainWindowModel : NotifyPropertyChanged, ISerializable {

        public MainWindowModel() {
            this.DayItems = new ObservableCollection<DayItem>();
        }

        public MainWindowModel(SerializationInfo info, StreamingContext context) {
            this.DayItems = info.GetValue("DayItems", typeof(ObservableCollection<DayItem>)) as ObservableCollection<DayItem>;
        }

        private ObservableCollection<DayItem> dayItems;

        public ObservableCollection<DayItem> DayItems {
            get { return dayItems; }
            set {
                if (dayItems != value) {
                    dayItems = value;
                    OnPropertyChanged("DayItems");
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("DayItems", DayItems);
        }
    }

    [Serializable]
    public class MainWindowViewModel : NotifyPropertyChanged {

        public MainWindowViewModel() {
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Imanuel", "worktimetracker.xml");
            var stream = default(Stream);
            var serializer = new BinaryFormatter();
            this.Model = new MainWindowModel();
            this.NewWorkItem = new WorkItem();
            if (File.Exists(file)) {
                using (stream = File.OpenRead(file)) {
                    try {
                        this.Model = serializer.Deserialize(stream) as MainWindowModel;
                    } catch {
                    }
                }
            }
            this.SelectedDate = DateTime.Now.Date;
            if (ModernUIHelper.IsInDesignMode) {
                var item = new DayItem {
                    Day = DateTime.Now.Date
                };
                item.WorkItems = new ObservableCollection<WorkItem>();
                item.WorkItems.Add(new WorkItem {
                    WorkedTime = TimeSpan.FromMinutes(200),
                    Project = "Test"
                });
                this.Model.DayItems.Add(item);
                this.SelectedDate = DateTime.Now.Date;
            }
            this.SaveCommand = new RelayCommand((sender) => {
                if (!File.Exists(file)) {
                    Directory.CreateDirectory(Path.GetDirectoryName(file));
                    stream = File.Create(file);
                } else {
                    stream = File.OpenWrite(file);
                }
                using (stream) {
                    serializer.Serialize(stream, Model);
                }
            });
            this.AddWorkItemCommand = new RelayCommand((sender) => {
                var item = Model.DayItems.Where(d => d.Day == SelectedDate).FirstOrDefault();
                var idx = Model.DayItems.IndexOf(item);
                if (item == null) {
                    item = new DayItem();
                } else {
                    idx = Model.DayItems.IndexOf(item);
                }
                item.WorkItems.Add(NewWorkItem);

                if (idx != -1) {
                    Model.DayItems[idx] = item;
                } else {
                    this.Model.DayItems.Add(item);
                }

                this.NewWorkItem = new WorkItem() { StartTime = this.NewWorkItem.EndTime };
            }, (sender) => {
                var res = !string.IsNullOrWhiteSpace(this.NewWorkItem.Project) && !string.IsNullOrWhiteSpace(this.NewWorkItem.Description);
                res &= this.NewWorkItem.EndTime > this.NewWorkItem.StartTime;
                return res;
            });
            this.SetStartTimeNow = new RelayCommand((sender) => {
                var time = DateTime.Now.TimeOfDay;
                this.NewWorkItem.StartTime = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
            });
            this.SetEndTimeNow = new RelayCommand((sender) => {
                var time = DateTime.Now.TimeOfDay;
                this.NewWorkItem.EndTime = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
            });
        }

        private MainWindowModel model;

        public MainWindowModel Model {
            get { return model; }
            set {
                if (model != value) {
                    model = value;
                    OnPropertyChanged("Model");
                }
            }
        }

        [NonSerialized]
        private WorkItem newWorkItem;

        public WorkItem NewWorkItem {
            get { return newWorkItem; }
            set {
                if (newWorkItem != value) {
                    newWorkItem = value;
                    OnPropertyChanged("NewWorkItem");
                }
            }
        }

        [NonSerialized]
        private RelayCommand saveCommand;

        public RelayCommand SaveCommand {
            get { return saveCommand; }
            set {
                if (saveCommand != value) {
                    saveCommand = value;
                    OnPropertyChanged("SaveCommand");
                }
            }
        }

        [NonSerialized]
        private RelayCommand addWorkItemCommand;

        public RelayCommand AddWorkItemCommand {
            get { return addWorkItemCommand; }
            set {
                if (addWorkItemCommand != value) {
                    addWorkItemCommand = value;
                    OnPropertyChanged("AddWorkItemCommand");
                }
            }
        }

        [NonSerialized]
        private DayItem selectedItem;

        public DayItem SelectedItem {
            get { return selectedItem; }
            set {
                if (selectedItem != value) {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        [NonSerialized]
        private DateTime selectedDate;

        public DateTime SelectedDate {
            get { return selectedDate; }
            set {
                if (selectedDate != value) {
                    selectedDate = value;
                    SelectedItem = Model.DayItems.Where(d => d.Day == SelectedDate).FirstOrDefault();
                    OnPropertyChanged("SelectedDate");
                }
            }
        }

        [NonSerialized]
        private RelayCommand setStartTimeNow;

        public RelayCommand SetStartTimeNow {
            get { return setStartTimeNow; }
            set {
                if (setStartTimeNow != value) {
                    setStartTimeNow = value;
                    OnPropertyChanged("SetStartTimeNow");
                }
            }
        }

        [NonSerialized]
        private RelayCommand setEndTimeNow;

        public RelayCommand SetEndTimeNow {
            get { return setEndTimeNow; }
            set {
                if (setEndTimeNow != value) {
                    setEndTimeNow = value;
                    OnPropertyChanged("SetEndTimeNow");
                }
            }
        }
    }
}