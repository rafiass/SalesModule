using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesModule;

namespace SalesModuleTester
{
    [TestClass]
    public class EngineTester
    {
        private ISalesEngine _eng;

        public EngineTester()
        {
            _eng = new Wrapper().CreateEngine();
        }

        [TestMethod]
        public void TestMethod1()
        {
            _eng.Initialize();


        }
    }
}
