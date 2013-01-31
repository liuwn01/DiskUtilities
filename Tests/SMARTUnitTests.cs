using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Text;
using Info;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMART;

namespace Tests
{
    [TestClass]
    public class SMARTUnitTests
    {
        [TestMethod]
        public void GetInformationTest()
        {
            var searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSStorageDriver_ATAPISmartData");
            var thresSearcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSStorageDriver_FailurePredictThresholds");

            var searcherEnumerator = searcher.Get().GetEnumerator();
            var thresSearcherEnumerator = thresSearcher.Get().GetEnumerator();

            while (searcherEnumerator.MoveNext() && thresSearcherEnumerator.MoveNext())
            {
                byte[] arrVendorSpecific = (byte[])searcherEnumerator.Current.GetPropertyValue("VendorSpecific");
                byte[] arrThreshold = (byte[])thresSearcherEnumerator.Current.GetPropertyValue("VendorSpecific");

                Console.WriteLine("-----------------------------------");
                Console.WriteLine("MSStorageDriver_ATAPISmartData instance");
                Console.WriteLine("-----------------------------------");

                // Create SMART data from 'vendor specific' array
                var d = new SmartData(arrVendorSpecific, arrThreshold);
                foreach (var b in d.Attributes)
                {
                    Console.Write("{0} : {1} : {2} : ", b.AttributeType, b.Value, b.Threshold);
                    string rawData = BitConverter.ToString(b.VendorData.Reverse().ToArray()).Replace("-", string.Empty);
                    Console.Write("{0}, {1}", rawData, int.Parse(rawData, NumberStyles.HexNumber));
                    Console.WriteLine();
                }
            }

            searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSStorageDriver_FailurePredictStatus");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                var arrVendorSpecific = (bool) queryObj.GetPropertyValue("PredictFailure");
                Console.Write("IsOK: {0}", !arrVendorSpecific);
            }
        }
    }
}
