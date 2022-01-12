using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelperTextractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTextractor.Tests {
    [TestClass()]
    public class TextDataRegxHandlerTests {
        [TestMethod()]
        public void Remove2SameCharTest() {
            var ss1 = "[[112233]]";
            var ss2 = "[123]";

            Assert.AreEqual(TextDataFilter.Remove2SameChar(ss1), ss2);
        }

        [TestMethod()]
        public void Remove3SameCharTest() {
            var ss1 = "[[[111222333]]]";
            var ss2 = "[123]";

            Assert.AreEqual(TextDataFilter.Remove3SameChar(ss1), ss2);
        }
    }
}