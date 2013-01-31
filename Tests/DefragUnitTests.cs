using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class DefragUnitTests
    {
        [TestMethod]
        public void OpenVolumeTest()
        {
            Defrag.Defrag defrag = new Defrag.Defrag();

            //defrag.MoveFile("H:", @"H:\EGU.rar", 1000, 35000, 1);

            defrag.FragmentFile(@"H:\EGU.rar");
            //defrag.FragmentFile(@"H:\CPU-Z.zip");
            
            //defrag.DefragmentFile(@"H:\EGU.rar");
        }
    }
}
