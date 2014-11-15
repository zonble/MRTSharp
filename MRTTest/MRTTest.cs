using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MRTLib;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class MRTTest
    {
        [TestMethod]
        public void TestLoadMap()
        {
            MRTMap map = MRTMap.SharedMap;
            Dictionary<string, MRTExit> exits = map.exits;
            string[] keys = exits.Keys.ToArray();
            string from = keys[0];

            for (int i = 1; i < keys.Count(); i++)
            {
                string to = keys[i];
                MRTRoute[] routes = map.FindRoutes(from, to);
                Assert.IsTrue(routes.Count() > 0, string.Format("{0} {1}", from, to));
            }


        }
    }
}
