using NMeCab.Specialized;

namespace HelperMeCab {
    public class MeCabUniDic22Wrapper {
        private MeCabUniDic22Tagger UniDic22Tagger { get; set; }

        public MeCabUniDic22Wrapper(string dicDir, string[]? userDirDics = null) {
            UniDic22Tagger = MeCabUniDic22Tagger.Create(dicDir, userDirDics);
        }

        public MeCabUniDic22Node[] ParseSentence(string sentence) {
            return UniDic22Tagger.Parse(sentence);
        }

        public void Destroy() {
            UniDic22Tagger.Dispose();
        }
    }
}
