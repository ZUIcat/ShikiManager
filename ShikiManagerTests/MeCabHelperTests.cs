using HelperMeCab;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShikiManagerTests {
    [TestClass()]
    public class MeCabHelperTests {
        [TestMethod()]
        public void Remove2SameCharTest() {
            var uniDicPath = @"C:\_MyWorkSpace\_WorkSpace\ShikiManager\Data\MeCabDic\unidic-csj-3.1.0";
            MeCabUniDic22Wrapper myMeCabUniDic22Wrapper = new(uniDicPath);

            var ss = "新しい世界と、小さなアパートの部屋で、僕らは二人暮らしを始めた。";
            var meCabNodes = myMeCabUniDic22Wrapper.ParseSentence(ss);
            MeCabUniDic22Wrapper.PrintMeCabNodes(meCabNodes);

            myMeCabUniDic22Wrapper.Destroy();
        }
    }
}
