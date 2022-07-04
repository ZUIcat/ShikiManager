using NMeCab.Specialized;
using System.Diagnostics;

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

        public static void PrintMeCabNodes(MeCabUniDic22Node[] meCabNodes) {
            Trace.TraceInformation($"=======================================");
            Trace.TraceInformation(string.Join(" ", meCabNodes.Select(x => x.Surface).ToList()));
            foreach (var meCabNode in meCabNodes) {
                Trace.TraceInformation($"================");
                Trace.TraceInformation($"Surface: {meCabNode.Surface}");
                // 品詞大分類
                Trace.TraceInformation($"Pos1(品詞大分類): {meCabNode.Pos1}");
                // 品詞中分類
                Trace.TraceInformation($"Pos2(品詞中分類): {meCabNode.Pos2}");
                // 品詞小分類
                Trace.TraceInformation($"Pos3(品詞小分類): {meCabNode.Pos3}");
                // 品詞細分類
                Trace.TraceInformation($"Pos4(品詞細分類): {meCabNode.Pos4}");
                // 活用型
                Trace.TraceInformation($"CType(活用型): {meCabNode.CType}");
                // 活用形
                Trace.TraceInformation($"CForm(活用形): {meCabNode.CForm}");
                // 語彙素読み
                Trace.TraceInformation($"LForm(語彙素読み): {meCabNode.LForm}");
                // 語彙素（語彙素表記＋語彙素細分類）
                Trace.TraceInformation($"Lemma(語彙素（語彙素表記＋語彙素細分類）): {meCabNode.Lemma}");
                // 書字形出現形
                Trace.TraceInformation($"Orth(書字形出現形): {meCabNode.Orth}");
                // 発音形出現形
                Trace.TraceInformation($"Pron(発音形出現形): {meCabNode.Pron}");
                // 書字形基本形
                Trace.TraceInformation($"OrthBase(書字形基本形): {meCabNode.OrthBase}");
                // 発音形基本形
                Trace.TraceInformation($"PronBase(発音形基本形): {meCabNode.PronBase}");
                // 語種
                Trace.TraceInformation($"Goshu(語種): {meCabNode.Goshu}");
                // 語頭変化型
                Trace.TraceInformation($"IType(語頭変化型): {meCabNode.IType}");
                // 語頭変化形
                Trace.TraceInformation($"IForm(語頭変化形): {meCabNode.IForm}");
                // 語末変化型
                Trace.TraceInformation($"FType(語末変化型): {meCabNode.FType}");
                // 語末変化形
                Trace.TraceInformation($"FForm(語末変化形): {meCabNode.FForm}");
                // 語頭変化結合形
                Trace.TraceInformation($"IConType(語頭変化結合形): {meCabNode.IConType}");
                // 語末変化結合形
                Trace.TraceInformation($"FConType(語末変化結合形): {meCabNode.FConType}");
                // 語彙素類
                // この項目の「英語」の名前は、 UniDic 2.2.0 や 2.3.0 の配布物に含まれる dicrc ファイルでは「type」となっているが、 UniDic
                // の FAQ (https://unidic.ninjal.ac.jp/faq#col_name) には「lType」と記載されている。
                Trace.TraceInformation($"LType(語彙素類): {meCabNode.LType}");
                // 仮名形出現形
                Trace.TraceInformation($"Kana(仮名形出現形): {meCabNode.Kana}");
                // 仮名形基本形
                Trace.TraceInformation($"KanaBase(仮名形基本形): {meCabNode.KanaBase}");
                // 語形出現形
                Trace.TraceInformation($"Form(語形出現形): {meCabNode.Form}");
                // 語形基本形
                Trace.TraceInformation($"FormBase(語形基本形): {meCabNode.FormBase}");
                // アクセント型
                Trace.TraceInformation($"AType(アクセント型): {meCabNode.AType}");
                // アクセント結合型
                Trace.TraceInformation($"AConType(アクセント結合型): {meCabNode.AConType}");
                // アクセント修飾型
                Trace.TraceInformation($"AModType(アクセント修飾型): {meCabNode.AModType}");
                // 語彙表ID
                Trace.TraceInformation($"LId(語彙表ID): {meCabNode.LId}");
                // 語彙素ID
                Trace.TraceInformation($"LemmaId(語彙素ID): {meCabNode.LemmaId}");
                Trace.TraceInformation($"================");
            }
        }
    }
}
