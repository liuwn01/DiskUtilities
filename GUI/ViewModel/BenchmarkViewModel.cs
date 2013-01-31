using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Benchmark;
using GUI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace GUI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BenchmarkViewModel : ViewModelBase
    {
        #region Properties

        #region ReadSeq

        /// <summary>
        /// The <see cref="ReadSeq" /> property's name.
        /// </summary>
        public const string ReadSeqPropertyName = "ReadSeq";

        private string _readSeq = "00.00";

        /// <summary>
        /// Sets and gets the ReadSeq property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ReadSeq
        {
            get
            {
                return _readSeq;
            }

            set
            {
                if (_readSeq == value)
                {
                    return;
                }

                _readSeq = value;
                RaisePropertyChanged(ReadSeqPropertyName);
            }
        }

        #endregion

        #region WriteSeq

        /// <summary>
        /// The <see cref="WriteSeq" /> property's name.
        /// </summary>
        public const string WriteSeqPropertyName = "WriteSeq";

        private string _writeSeq = "00.00";

        /// <summary>
        /// Sets and gets the WriteSeq property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WriteSeq
        {
            get
            {
                return _writeSeq;
            }

            set
            {
                if (_writeSeq == value)
                {
                    return;
                }

                _writeSeq = value;
                RaisePropertyChanged(WriteSeqPropertyName);
            }
        }

        #endregion

        #region Read512K

        /// <summary>
        /// The <see cref="Read512K" /> property's name.
        /// </summary>
        public const string Read512KPropertyName = "Read512K";

        private string _read512K = "00.00";

        /// <summary>
        /// Sets and gets the Read512K property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Read512K
        {
            get
            {
                return _read512K;
            }

            set
            {
                if (_read512K == value)
                {
                    return;
                }

                _read512K = value;
                RaisePropertyChanged(Read512KPropertyName);
            }
        }

        #endregion

        #region Write512K

        /// <summary>
        /// The <see cref="Write512K" /> property's name.
        /// </summary>
        public const string Write512KPropertyName = "Write512K";

        private string _write512K = "00.00";

        /// <summary>
        /// Sets and gets the Write512K property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Write512K
        {
            get
            {
                return _write512K;
            }

            set
            {
                if (_write512K == value)
                {
                    return;
                }

                _write512K = value;
                RaisePropertyChanged(Write512KPropertyName);
            }
        }

        #endregion

        #region Read4K

        /// <summary>
        /// The <see cref="Read4K" /> property's name.
        /// </summary>
        public const string Read4KPropertyName = "Read4K";

        private string _read4K = "00.00";

        /// <summary>
        /// Sets and gets the Read4K property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Read4K
        {
            get
            {
                return _read4K;
            }

            set
            {
                if (_read4K == value)
                {
                    return;
                }

                _read4K = value;
                RaisePropertyChanged(Read4KPropertyName);
            }
        }

        #endregion

        #region Write4K

        /// <summary>
        /// The <see cref="Write4K" /> property's name.
        /// </summary>
        public const string Write4KPropertyName = "Write4K";

        private string _write4K = "00.00";

        /// <summary>
        /// Sets and gets the Write4K property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Write4K
        {
            get
            {
                return _write4K;
            }

            set
            {
                if (_write4K == value)
                {
                    return;
                }

                _write4K = value;
                RaisePropertyChanged(Write4KPropertyName);
            }
        }

        #endregion

        #region AvailableDisks

        /// <summary>
        /// The <see cref="AvailableDisks" /> property's name.
        /// </summary>
        public const string AvailableDisksPropertyName = "AvailableDisks";

        private ObservableCollection<string> _availableDisks = new ObservableCollection<string>();

        /// <summary>
        /// Sets and gets the AvailableDisks property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<string> AvailableDisks
        {
            get
            {
                return _availableDisks;
            }

            set
            {
                if (_availableDisks == value)
                {
                    return;
                }

                _availableDisks = value;
                RaisePropertyChanged(AvailableDisksPropertyName);
            }
        }

        #endregion

        #region AvailableFileSizes

        /// <summary>
        /// The <see cref="AvailableFileSizes" /> property's name.
        /// </summary>
        public const string AvailableFileSizesPropertyName = "AvailableFileSizes";

        private ObservableCollection<string> _availableFileSizes = new ObservableCollection<string>();

        /// <summary>
        /// Sets and gets the AvailableFileSizes property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<string> AvailableFileSizes
        {
            get
            {
                return _availableFileSizes;
            }

            set
            {
                if (_availableFileSizes == value)
                {
                    return;
                }

                _availableFileSizes = value;
                RaisePropertyChanged(AvailableFileSizesPropertyName);
            }
        }

        #endregion

        #region AvailableAttemptAmounts

        /// <summary>
        /// The <see cref="AvailableAttemptAmounts" /> property's name.
        /// </summary>
        public const string AvailableAttemptAmountsPropertyName = "AvailableAttemptAmounts";

        private ObservableCollection<string> _availableAttemptAmounts = new ObservableCollection<string>();

        /// <summary>
        /// Sets and gets the AvailableAttemptAmounts property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<string> AvailableAttemptAmounts
        {
            get
            {
                return _availableAttemptAmounts;
            }

            set
            {
                if (_availableAttemptAmounts == value)
                {
                    return;
                }

                _availableAttemptAmounts = value;
                RaisePropertyChanged(AvailableAttemptAmountsPropertyName);
            }
        }

        #endregion

        #region SelectedDisk

        /// <summary>
        /// The <see cref="SelectedDisk" /> property's name.
        /// </summary>
        public const string SelectedDiskPropertyName = "SelectedDisk";

        private string _selectedDisk = string.Empty;

        /// <summary>
        /// Sets and gets the SelectedDisk property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedDisk
        {
            get
            {
                return _selectedDisk;
            }

            set
            {
                if (_selectedDisk == value)
                {
                    return;
                }

                RaisePropertyChanged(SelectedDiskPropertyName);
                _selectedDisk = value;
                RaisePropertyChanged(SelectedDiskPropertyName);
            }
        }

        #endregion

        #region SelectedFileSize

        /// <summary>
        /// The <see cref="SelectedFileSize" /> property's name.
        /// </summary>
        public const string SelectedFileSizePropertyName = "SelectedFileSize";

        private string _selectedFileSize = string.Empty;

        /// <summary>
        /// Sets and gets the SelectedFileSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedFileSize
        {
            get
            {
                return _selectedFileSize;
            }

            set
            {
                if (_selectedFileSize == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedFileSizePropertyName);
                _selectedFileSize = value;
                RaisePropertyChanged(SelectedFileSizePropertyName);
            }
        }

        #endregion

        #region SelectedAttemptAmount

        /// <summary>
        /// The <see cref="SelectedAttemptAmount" /> property's name.
        /// </summary>
        public const string SelectedAttemptAmountPropertyName = "SelectedAttemptAmount";

        private string _selectedAttemptAmount = string.Empty;

        /// <summary>
        /// Sets and gets the SelectedAttemptAmount property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedAttemptAmount
        {
            get
            {
                return _selectedAttemptAmount;
            }

            set
            {
                if (_selectedAttemptAmount == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedAttemptAmountPropertyName);
                _selectedAttemptAmount = value;
                RaisePropertyChanged(SelectedAttemptAmountPropertyName);
            }
        }

        #endregion

        #region CanRunTest

        /// <summary>
        /// The <see cref="CanRunTest" /> property's name.
        /// </summary>
        public const string CanRunTestPropertyName = "CanRunTest";

        private bool _canRunTest = true;

        /// <summary>
        /// Sets and gets the CanRunTest property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool CanRunTest
        {
            get
            {
                return _canRunTest;
            }

            set
            {
                if (_canRunTest == value)
                {
                    return;
                }

                _canRunTest = value;
                RaisePropertyChanged(CanRunTestPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        private ICommand _seqTestCommand;
        public ICommand SeqTestCommand
        {
            get { return _seqTestCommand; }
            set
            {
                _seqTestCommand = value;
                RaisePropertyChanged("SeqTestCommand");
            }
        }

        private ICommand __512KTestCommand;
        public ICommand _512KTestCommand
        {
            get { return __512KTestCommand; }
            set
            {
                __512KTestCommand = value;
                RaisePropertyChanged("_512KTestCommand");
            }
        }

        private ICommand __4KTestCommand;
        public ICommand _4KTestCommand
        {
            get { return __4KTestCommand; }
            set
            {
                __4KTestCommand = value;
                RaisePropertyChanged("_4KTestCommand");
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the BenchmarkViewModel class.
        /// </summary>
        public BenchmarkViewModel()
        {
            _seqTestCommand = new RelayCommand(SeqTest);
            __512KTestCommand = new RelayCommand(_512KTest);
            __4KTestCommand = new RelayCommand(_4KTest);
            InitializeOptions();
        }
        
        #endregion

        #region Private methods

        private void InitializeOptions()
        {
            OptionsModel model = new OptionsModel();
            AvailableDisks = model.AvailableDisks;
            AvailableFileSizes = model.AvailableFileSizes;
            AvailableAttemptAmounts = model.AvailableAttemptAmounts;
        }

        private void SeqTest()
        {
            CanRunTest = false;
            Messenger.Default.Send<string>("Benchmark", "UpdateLayout");

            Benchmark.Benchmark benchmark = new Benchmark.Benchmark(SelectedDisk)
            {
                WayToGo = BenchmarkWay.Sequential,
                AmountOfRepeats = Convert.ToInt32(SelectedAttemptAmount),
                FileSize = (FileSize)Enum.Parse(typeof(FileSize),
                SelectedFileSize)
            };
            TestResult result = benchmark.Run();

            ReadSeq = Math.Round(result.ReadSpeedMBps, 2).ToString(CultureInfo.InvariantCulture);
            WriteSeq = Math.Round(result.WriteSpeedMBps, 2).ToString(CultureInfo.InvariantCulture);

            CanRunTest = true;
        }

        private void _512KTest()
        {
            CanRunTest = false;
            Messenger.Default.Send<string>("Benchmark", "UpdateLayout");

            Benchmark.Benchmark benchmark = new Benchmark.Benchmark(SelectedDisk)
            {
                WayToGo = BenchmarkWay._512K,
                AmountOfRepeats = Convert.ToInt32(SelectedAttemptAmount),
                FileSize = (FileSize)Enum.Parse(typeof(FileSize),
                SelectedFileSize)
            };
            TestResult result = benchmark.Run();

            Read512K = Math.Round(result.ReadSpeedMBps, 2).ToString(CultureInfo.InvariantCulture);
            Write512K = Math.Round(result.WriteSpeedMBps, 2).ToString(CultureInfo.InvariantCulture);

            CanRunTest = true;
        }

        private void _4KTest()
        {
            CanRunTest = false;
            Messenger.Default.Send<string>("Benchmark", "UpdateLayout");

            Benchmark.Benchmark benchmark = new Benchmark.Benchmark(SelectedDisk)
            {
                WayToGo = BenchmarkWay._4K,
                AmountOfRepeats = Convert.ToInt32(SelectedAttemptAmount),
                FileSize = (FileSize)Enum.Parse(typeof(FileSize),
                SelectedFileSize)
            };
            TestResult result = benchmark.Run();

            Read4K = Math.Round(result.ReadSpeedMBps, 2).ToString(CultureInfo.InvariantCulture);
            Write4K = Math.Round(result.WriteSpeedMBps, 2).ToString(CultureInfo.InvariantCulture);

            CanRunTest = true;
        }

        #endregion
    }
}