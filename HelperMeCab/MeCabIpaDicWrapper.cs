using NMeCab.Specialized;

namespace HelperMeCab {
    // TODO 感觉这类毫无必要啊
    public class MeCabIpaDicWrapper {
        private MeCabIpaDicTagger IpaDicTagger { get; set; }

        public MeCabIpaDicWrapper(string dicDir, string[]? userDirDics = null) {
            IpaDicTagger = MeCabIpaDicTagger.Create(dicDir, userDirDics);
        }

        public MeCabIpaDicNode[] ParseSentence(string sentence) {
            return IpaDicTagger.Parse(sentence);
        }

        public void Destroy() {
            IpaDicTagger.Dispose();
        }
    }
}
