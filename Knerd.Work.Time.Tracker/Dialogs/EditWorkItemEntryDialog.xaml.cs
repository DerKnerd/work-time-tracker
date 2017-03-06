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

namespace Knerd.Work.Time.Tracker.Dialogs
{
    public sealed partial class EditWorkItemEntryDialog : ContentDialog
    {
        public EditWorkItemEntryDialog(WorkItemEntryModel model)
        {
            this.InitializeComponent();
            this.DataContext = model;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var entry = (this.DataContext as WorkItemEntryModel);
            if (entry.Save.CanExecute)
            {
                entry.Save.TryExecute();
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
