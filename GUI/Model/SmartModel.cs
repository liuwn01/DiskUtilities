using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management;
using SMART;

namespace GUI.Model
{
    public class SmartModel
    {
        #region Fields

        public string SMARTStatus { get; set; }
        public ObservableCollection<SmartRow> SMARTData { get; set; }
        
        #endregion

        #region Constructor

        public SmartModel()
        {
            SMARTData = new ObservableCollection<SmartRow>();

            using (var searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSStorageDriver_ATAPISmartData"))
            {
                using (var thresSearcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSStorageDriver_FailurePredictThresholds"))
                {
                    var searcherEnumerator = searcher.Get().GetEnumerator();
                    var thresSearcherEnumerator = thresSearcher.Get().GetEnumerator();

                    while (searcherEnumerator.MoveNext() && thresSearcherEnumerator.MoveNext())
                    {
                        byte[] arrVendorSpecific = (byte[])searcherEnumerator.Current.GetPropertyValue("VendorSpecific");
                        byte[] arrThreshold = (byte[])thresSearcherEnumerator.Current.GetPropertyValue("VendorSpecific");

                        /* Create SMART data from 'vendor specific' array */
                        var d = new SmartData(arrVendorSpecific, arrThreshold);
                        foreach (var b in d.Attributes)
                        {
                            string rawData = BitConverter.ToString(b.VendorData.Reverse().ToArray()).Replace("-", string.Empty);
                            SMARTData.Add(new SmartRow(b.AttributeType.ToString(),
                                                       b.Value.ToString(CultureInfo.InvariantCulture),
                                                       b.Threshold.ToString(CultureInfo.InvariantCulture),
                                                       rawData,
                                                       int.Parse(rawData, NumberStyles.HexNumber).ToString(CultureInfo.InvariantCulture)));
                        }
                    }
                }
            }

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSStorageDriver_FailurePredictStatus"))
            {
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    SMARTStatus += (bool)queryObj.GetPropertyValue("PredictFailure") ? "WARNING " : "OK ";
                }
            }
        }

        #endregion
    }
}
