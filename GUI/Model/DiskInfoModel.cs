using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Info;

namespace GUI.Model
{
    public class DiskInfoModel
    {
        #region Fields

        public ObservableCollection<string> DiskInfo { get; set; }

        #endregion

        #region Constructor

        public DiskInfoModel()
        {
            DiskInfo = new ObservableCollection<string>();
            DiskInformation info = new DiskInformation();
            List<Tuple<string, string>> res = info.GetInformation();

            foreach (Tuple<string, string> tuple in res)
            {
                DiskInfo.Add(tuple.Item1 + ": " + tuple.Item2);
            }
        }

        #endregion
    }
}
