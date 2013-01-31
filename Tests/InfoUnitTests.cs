using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Info;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class InfoUnitTests
    {
        [TestMethod]
        public void GetInformationTest()
        {
            DiskInformation info = new DiskInformation();
            info.GetInformation();
        }
    }
}
