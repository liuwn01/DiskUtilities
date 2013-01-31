using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Benchmark;
using Info;

namespace GUI.Model
{
    public class OptionsModel
    {
        #region Fields

        public ObservableCollection<string> AvailableDisks { get; set; }
        public ObservableCollection<string> AvailableFileSizes { get; set; }
        public ObservableCollection<string> AvailableAttemptAmounts { get; set; }

        #endregion

        #region Constructor

        public OptionsModel()
        {
            DiskInformation info = new DiskInformation();
            AvailableDisks = new ObservableCollection<string>(info.GetLocalDisks());

            AvailableFileSizes = new ObservableCollection<string>();
            foreach (string fileSizeName in Enum.GetNames(typeof(FileSize)))
            {
                AvailableFileSizes.Add(fileSizeName);
            }

            AvailableAttemptAmounts = new ObservableCollection<string>(Enumerable.Range(1, 5).Select(p => p.ToString(CultureInfo.InvariantCulture)));
        }

        #endregion
    }
}
