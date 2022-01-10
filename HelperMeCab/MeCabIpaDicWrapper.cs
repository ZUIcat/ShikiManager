using NMeCab.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperMeCab {
    public class MeCabIpaDicWrapper {
        //https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/attributes/nullable-analysis
        //https://docs.microsoft.com/zh-cn/ef/core/miscellaneous/nullable-reference-types
        private MeCabIpaDicTagger IpaDicTagger { get; set; }

        public MeCabIpaDicWrapper(string dicDir, string[]? userDirDics = null) {
            IpaDicTagger = MeCabIpaDicTagger.Create(dicDir, userDirDics);
        }

        public void Destroy() {
            IpaDicTagger.Dispose();
        }
    }
}
