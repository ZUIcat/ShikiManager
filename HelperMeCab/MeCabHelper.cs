using System;
using NMeCab.Specialized;

namespace HelperMeCab {
    public class MeCabHelper {
        //private static MeCabIpaDicTagger? ipaDicTagger = null;
        //private static MeCabUniDic22Tagger? uniDicTagger = null;

        //public static MeCabIpaDicTagger CreateIpaDicTagger(string dicDir, string[]? userDirDics = null) {
        //    if (ipaDicTagger != null) {
        //        ipaDicTagger.Dispose();
        //    }
        //    if (ipaDicTagger == null) {
        //        ipaDicTagger = MeCabIpaDicTagger.Create(dicDir, userDirDics);
        //    }
        //    return ipaDicTagger;
        //}

        //public static MeCabUniDic22Tagger CreateUniDicTagger(string dicDir, string[]? userDirDics = null) {
        //    if (ipaDicTagger != null) {
        //        ipaDicTagger.Dispose();
        //    }
        //    if (uniDicTagger == null) {
        //        uniDicTagger = MeCabUniDic22Tagger.Create(dicDir, userDirDics);
        //    }
        //    return uniDicTagger;
        //}

        public MeCabHelper() {
            
        }

        /// <summary>
        /// 初始化 MeCabHelper
        /// 在此创建 MeCabTagger
        /// </summary>
        /// <param name="dicDir">字典文件所在的目录</param>
        /// <param name="isIpadic">是否使用 ipadic 字典，否则使用 unidic 字典</param>
        /// <returns>是否初始化成功</returns>
        public bool Init(string dicDir, bool isIpadic = false) {
            return true;
        }

        public static void ParseSentence(string sentence) {
            
        }
    }
}
