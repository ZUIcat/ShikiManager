namespace HelperConfig {
    public class TranslatorConfig {
        public string NowTranslator { get; set; }
        public BaiduConfig BaiduConfig { get; set; }

        public TranslatorConfig() {
            NowTranslator = string.Empty;
            BaiduConfig = new BaiduConfig() {
                AppId = string.Empty,
                SecretKey = string.Empty
            };
        }
    }

    public struct BaiduConfig {
        public string AppId { get; set; }
        public string SecretKey { get; set; }
    }
}
