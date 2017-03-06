using Knerd.Work.Time.Tracker.Models;
using Knerd.Work.Time.Tracker.Pages;
using MyToolkit.Paging;
using System;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using System.Threading.Tasks;

namespace Knerd.Work.Time.Tracker
{

    /// <summary>
    /// Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
    /// </summary>
    sealed partial class App : MtApplication
    {

        /// <summary>
        /// Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
        /// und daher das logische Äquivalent von main() bzw. WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        public override Type StartPageType
        {
            get
            {
                return typeof(OverviewPage);
            }
        }

        public override async Task OnInitializedAsync(MtFrame frame, ApplicationExecutionState args)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var roamingFolder = ApplicationData.Current.RoamingFolder;
            var fileName = "workitems.db";
            SqliteConnector.AfterEdit += async () =>
            {
                var localFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                var roamingFile = await roamingFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                await localFile.CopyAndReplaceAsync(roamingFile);
            };
            ApplicationData.Current.DataChanged += async (appData, sender) =>
            {
                await loadFileFromRoaming();
            };
            await loadFileFromRoaming();
        }

        private async Task loadFileFromRoaming()
        {
            var roamingFolder = ApplicationData.Current.RoamingFolder;
            var localFolder = ApplicationData.Current.LocalFolder;
            var fileName = "workitems.db";
            var localFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            var roamingFile = await roamingFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await roamingFile.CopyAndReplaceAsync(localFile);
        }
    }
}