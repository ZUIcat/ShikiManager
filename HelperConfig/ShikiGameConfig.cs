using System;
using System.Collections.Generic;

namespace HelperConfig {
    public class ShikiGameConfig {
        public IList<string> Title { get; set; } // 恋愛×ロワイアル
        public IList<string> Version { get; set; } // v1.02; rev 1623
        public IList<string> Company { get; set; } // ASa Project
        public IList<string> Date { get; set; } // 20201127
        public IList<string> Package { get; set; } // Package/DL/Steam
        public IList<string> Included { get; set; } // None
        public IList<string> Description { get; set; } // 私、転校生である花丸まりは、小町ひろたかくんのことが好きですっ付き合ってください！
        public IList<string> Tag { get; set; } // 高森奈津美; 山岡ゆり
        public IList<string> Note { get; set; } // 有点无聊……别忘了还有俩 FD。
        public IList<ExeConfig> ShikiConfig { get; set; }
        public float ConfigVersion { get; set; } // 1.0

        public ShikiGameConfig() {
            //Base:
            //Edit infomation
            //Search in bgm / 2dFun

            //Open Game Folder
            //Open Save Folder


            //Advance:
            //Initial Action
            //Before Action
            //After Action
            //Remove Action

            //Add Language Patch
            //Remove Language Patch
        }
    }

    public class ExeConfig {
        public string Name { get; set; }
        public string ExePath { get; set; }
        public string SpecialCode { get; set; }

        public ExeConfig() {
        }
    }
}
