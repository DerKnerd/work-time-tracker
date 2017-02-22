using Knerd.Work.Time.Tracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Inhaltsdialog" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Knerd.Work.Time.Tracker.Dialogs {
    public sealed partial class AddWorkItemEntryDialog : ContentDialog {
        public AddWorkItemEntryDialog(DateTimeOffset date) {
            this.InitializeComponent();
            (this.DataContext as WorkItemEntryModel).Date = date;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            var connector = new SqliteConnector();
            await connector.SaveWorkItemEntryAsync(this.DataContext as WorkItemEntryModel);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
        }
    }
}
