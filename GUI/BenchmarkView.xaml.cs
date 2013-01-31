using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;

namespace GUI
{
    /// <summary>
    /// Description for BenchmarkView.
    /// </summary>
    public partial class BenchmarkView
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the BenchmarkView class.
        /// </summary>
        public BenchmarkView()
        {
            InitializeComponent();
            Messenger.Default.Register<string>(this, "UpdateLayout", o => Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render, (Action)(UpdateLayout)));
        }

        #endregion
    }
}