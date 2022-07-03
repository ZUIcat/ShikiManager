namespace HelperConfig {
    public class TranslatorConfig {
        public string NowTranslator { get; set; }
        public BaiduConfig BaiduConfig { get; set; }

        public TranslatorConfig() {
            NowTranslator = "Youdao_Public";
            BaiduConfig = new();
        }
    }

    public struct BaiduConfig {
        public string AppId { get; set; }
        public string SecretKey { get; set; }
    }
}
