using System;
using System.Collections.Generic;
using System.Management;

namespace Info
{
    public class DiskInformation
    {
        public List<Tuple<string, string>> GetInformation()
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();

            ManagementScope scope = new ManagementScope(@"\\.\ROOT\CIMV2");
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_DiskDrive");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection queryCollection = searcher.Get())
                {
                    foreach (ManagementBaseObject obj in queryCollection)
                    {
                        result.Add(new Tuple<string, string>("DeviceID", obj["DeviceID"].ToString()));
                        result.Add(new Tuple<string, string>("Caption", obj["Caption"].ToString()));
                        result.Add(new Tuple<string, string>("FirmwareRevision", obj["FirmwareRevision"].ToString()));
                        result.Add(new Tuple<string, string>("SerialNumber", obj["SerialNumber"].ToString()));
                        result.Add(new Tuple<string, string>("Size", obj["Size"].ToString()));
                        result.Add(new Tuple<string, string>("InterfaceType", obj["InterfaceType"].ToString()));
                    }
                }
            }

            scope = new ManagementScope(@"\\.\ROOT\CIMV2");
            query = new ObjectQuery("SELECT * FROM Win32_LogicalDisk");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection queryCollection = searcher.Get())
                {
                    foreach (ManagementBaseObject obj in queryCollection)
                    {
                        result.Add(new Tuple<string, string>("Logical disk", obj["Caption"] + " (" + obj["FileSystem"] + ", " + obj["Size"] + ")"));
                    }
                }
            }

            return result;
        }

        public List<string> GetLocalDisks()
        {
            List<string> result = new List<string>();

            var scope = new ManagementScope(@"\\.\ROOT\CIMV2");
            var query = new ObjectQuery("SELECT * FROM Win32_LogicalDisk");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection queryCollection = searcher.Get())
                {
                    foreach (ManagementBaseObject obj in queryCollection)
                    {
                        result.Add(obj["Caption"].ToString());
                    }
                }
            }

            return result;
        }
    }
}
