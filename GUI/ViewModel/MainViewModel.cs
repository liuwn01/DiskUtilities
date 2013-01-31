using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GUI.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;

namespace GUI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        #region FilePath

        /// <summary>
        /// The <see cref="FilePath" /> property's name.
        /// </summary>
        public const string FilePathPropertyName = "FilePath";

        private string _filePath = string.Empty;

        /// <summary>
        /// Sets and gets the FilePath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string FilePath
        {
            get
            {
                return _filePath;
            }

            set
            {
                if (_filePath == value)
                {
                    return;
                }

                _filePath = value;
                RaisePropertyChanged(FilePathPropertyName);
            }
        }

        #endregion

        #region VolumeMap

        /// <summary>
        /// The <see cref="VolumeMap" /> property's name.
        /// </summary>
        public const string VolumeMapPropertyName = "VolumeMap";

        private ObservableCollection<SolidColorBrush> _volumeMap = new ObservableCollection<SolidColorBrush>();

        /// <summary>
        /// Sets and gets the VolumeMap property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<SolidColorBrush> VolumeMap
        {
            get
            {
                return _volumeMap;
            }

            set
            {
                if (_volumeMap == value)
                {
                    return;
                }

                _volumeMap = value;
                RaisePropertyChanged(VolumeMapPropertyName);
            }
        }

        #endregion

        #region CanDefrag

        /// <summary>
        /// The <see cref="CanDefrag" /> property's name.
        /// </summary>
        public const string CanDefragPropertyName = "CanDefrag";

        private bool _canDefrag = false;

        /// <summary>
        /// Sets and gets the CanDefrag property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool CanDefrag
        {
            get
            {
                return _canDefrag;
            }

            set
            {
                if (_canDefrag == value)
                {
                    return;
                }

                _canDefrag = value;
                RaisePropertyChanged(CanDefragPropertyName);
            }
        }

        #endregion

        #region DefragWindowVisibility

        /// <summary>
        /// The <see cref="DefragWindowVisibility" /> property's name.
        /// </summary>
        public const string DefragWindowVisibilityPropertyName = "DefragWindowVisibility";

        private Visibility _defragWindowVisibility = Visibility.Hidden;

        /// <summary>
        /// Sets and gets the DefragWindowVisibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility DefragWindowVisibility
        {
            get
            {
                return _defragWindowVisibility;
            }

            set
            {
                if (_defragWindowVisibility == value)
                {
                    return;
                }

                _defragWindowVisibility = value;
                RaisePropertyChanged(DefragWindowVisibilityPropertyName);
            }
        }

        #endregion

        #region DiskInfo

        /// <summary>
        /// The <see cref="DiskInfo" /> property's name.
        /// </summary>
        public const string DiskInfoPropertyName = "DiskInfo";

        private ObservableCollection<string> _diskInfo = new ObservableCollection<string>();

        /// <summary>
        /// Sets and gets the DiskInfo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<string> DiskInfo
        {
            get
            {
                return _diskInfo;
            }

            set
            {
                if (_diskInfo == value)
                {
                    return;
                }

                _diskInfo = value;
                RaisePropertyChanged(DiskInfoPropertyName);
            }
        }

        #endregion

        #region SMARTStatus

        /// <summary>
        /// The <see cref="SMARTStatus" /> property's name.
        /// </summary>
        public const string SMARTStatusPropertyName = "SMARTStatus";

        private string _sMARTStatus = "UNKNOWN";

        /// <summary>
        /// Sets and gets the SMARTStatus property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SMARTStatus
        {
            get
            {
                return _sMARTStatus;
            }

            set
            {
                if (_sMARTStatus == value)
                {
                    return;
                }

                _sMARTStatus = value;
                SMARTStatusColor = value.Contains("WARNING") ? Brushes.Red : Brushes.Green;
                RaisePropertyChanged(SMARTStatusPropertyName);
            }
        }

        #endregion

        #region SMARTData

        /// <summary>
        /// The <see cref="SMARTData" /> property's name.
        /// </summary>
        public const string SMARTDataPropertyName = "SMARTData";

        private ObservableCollection<SmartRow> _sMARTData = new ObservableCollection<SmartRow>();

        /// <summary>
        /// Sets and gets the SMARTData property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<SmartRow> SMARTData
        {
            get
            {
                return _sMARTData;
            }

            set
            {
                if (_sMARTData == value)
                {
                    return;
                }

                _sMARTData = value;
                RaisePropertyChanged(SMARTDataPropertyName);
            }
        }

        #endregion

        #region SMARTStatusColor

        /// <summary>
        /// The <see cref="SMARTStatusColor" /> property's name.
        /// </summary>
        public const string SMARTStatusColorPropertyName = "SMARTStatusColor";

        private SolidColorBrush _sMARTStatusColor = Brushes.Black;

        /// <summary>
        /// Sets and gets the SMARTStatusColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SolidColorBrush SMARTStatusColor
        {
            get
            {
                return _sMARTStatusColor;
            }

            set
            {
                if (Equals(_sMARTStatusColor, value))
                {
                    return;
                }

                _sMARTStatusColor = value;
                RaisePropertyChanged(SMARTStatusColorPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        private ICommand _openFileDialogCommand;
        public ICommand OpenFileDialogCommand
        {
            get { return _openFileDialogCommand; }
            set
            {
                _openFileDialogCommand = value;
                RaisePropertyChanged("OpenFileDialogCommand");
            }
        }

        private ICommand _defragFileCommand;
        public ICommand DefragFileCommand
        {
            get { return _defragFileCommand; }
            set
            {
                _defragFileCommand = value;
                RaisePropertyChanged("DefragFileCommand");
            }
        }

        private ICommand _closeFileCommand;
        public ICommand CloseCommand
        {
            get { return _closeFileCommand; }
            set
            {
                _closeFileCommand = value;
                RaisePropertyChanged("CloseCommand");
            }
        }

        private ICommand _openBenchmarkCommand;
        public ICommand OpenBenchmarkCommand
        {
            get { return _openBenchmarkCommand; }
            set
            {
                _openBenchmarkCommand = value;
                RaisePropertyChanged("OpenBenchmarkCommand");
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            _openFileDialogCommand = new RelayCommand(OpenTaskDialog);
            _defragFileCommand = new RelayCommand(DefragFile);
            _closeFileCommand = new RelayCommand(CloseApp);
            _openBenchmarkCommand = new RelayCommand(OpenBenchmark);
            GetDiskInfo();
            GetSmartInfo();
        }
        
        #endregion

        #region Command implementation

        public void OpenTaskDialog()
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Task files (*.*)|*.*";
                dlg.InitialDirectory = Environment.CurrentDirectory;
                dlg.Multiselect = false;
                bool? result = dlg.ShowDialog();
                if (!result.Value)
                {
                    throw new OperationCanceledException("No files were picked via OpenFileDialog");
                }

                DefragWindowVisibility = Visibility.Visible;
                Messenger.Default.Send<int>(1, "UpdateLayout");
                MainModel mainModel = new MainModel(dlg.FileName);
                DefragWindowVisibility = Visibility.Hidden;

                FilePath = mainModel.FilePath;
                VolumeMap = mainModel.VolumeMap;
                CanDefrag = mainModel.CanDefrag;
            }
            catch (OperationCanceledException ex)
            {
                //TODO: log
                return;
            }
            catch (Exception ex)
            {
                //TODO: print error
                throw;
            }
        }

        public void DefragFile()
        {
            DefragModel defragModel = new DefragModel();
            DefragWindowVisibility = Visibility.Visible;
            Messenger.Default.Send<int>(1, "UpdateLayout");
            defragModel.DefragFile(FilePath);
            DefragWindowVisibility = Visibility.Hidden;

            MainModel mainModel = new MainModel(FilePath);
            FilePath = mainModel.FilePath;
            VolumeMap = mainModel.VolumeMap;
        }

        #endregion

        #region Private methods

        private void GetDiskInfo()
        {
            DiskInfoModel info = new DiskInfoModel();
            DiskInfo = info.DiskInfo;
        }

        private void GetSmartInfo()
        {
            SmartModel smartModel = new SmartModel();
            SMARTStatus = smartModel.SMARTStatus;
            SMARTData = smartModel.SMARTData;
        }

        private void CloseApp()
        {
            Application.Current.Shutdown(0);
        }

        private void OpenBenchmark()
        {
            BenchmarkView benchmarkView = new BenchmarkView();
            benchmarkView.ShowDialog();
        }

        #endregion
    }
}