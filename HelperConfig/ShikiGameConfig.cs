namespace HelperConfig {
    public class ShikiGameConfig {
        public IList<string> Title { get; set; } // 恋愛×ロワイアル
        public IList<string> Version { get; set; } // v1.02; rev 1623; adult v1.0
        public IList<string> Company { get; set; } // ASa Project
        public IList<string> Date { get; set; } // 20201127
        public IList<string> Package { get; set; } // Package/DL/Steam
        public IList<string> IncludedIn { get; set; } // None/某合集
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
        public string Name { get; set; } // 开始游戏
        public string ExePath { get; set; } // ./yysy.exe
        public string SpecialCode { get; set; } // #HDF:668t
        public string SaveFolder { get; set; } // $appdata$/AAA/bbb
        public int LocaleEmu { get; set; } // 0 为没有，1 为 LocaleEmulator，2 为 Ntleas

        public ExeConfig() {
        }
    }
}
