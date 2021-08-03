using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperConfig {
    public class MeCabConfig {
        public string DirPath { get; set; }
        public MeCabDicType DicType { get; set; }

        public MeCabConfig() {
            DirPath = @".\MeCabDic\ipadic-2.7.0-20070801";
            DicType = MeCabDicType.IPADIC;
        }
    }

    public enum MeCabDicType {
        IPADIC,
        UNIDIC
    }
}
