using Knerd.Work.Time.Tracker.Pages;
using MyToolkit.Paging;
using System;

namespace Knerd.Work.Time.Tracker {

    /// <summary>
    /// Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
    /// </summary>
    sealed partial class App : MtApplication {

        /// <summary>
        /// Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
        /// und daher das logische Äquivalent von main() bzw. WinMain().
        /// </summary>
        public App() {
            this.InitializeComponent();
        }

        public override Type StartPageType {
            get {
                return typeof(OverviewPage);
            }
        }
    }
}