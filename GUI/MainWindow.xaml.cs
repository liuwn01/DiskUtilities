using System;
using System.Windows.Threading;
using GUI.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<int>(this, "UpdateLayout", o => Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render, (Action)(UpdateLayout)));
        }

        #endregion
    }
}