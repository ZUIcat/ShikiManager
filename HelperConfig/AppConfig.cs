namespace HelperConfig {
    public class AppConfig {
        public string ScreenshotPath { get; set; }

        public LocaleEmulatorConfig LocaleEmulatorConfig { get; set; }
        public MeCabConfig MeCabConfig { get; set; }
        public NtleasConfig NtleasConfig { get; set; }
        public TextractorConfig TextractorConfig { get; set; }

        public AppConfig() {
            ScreenshotPath = @".\Screenshot";

            LocaleEmulatorConfig = new LocaleEmulatorConfig();
            MeCabConfig = new MeCabConfig();
            NtleasConfig = new NtleasConfig();
            TextractorConfig = new TextractorConfig();
        }
    }
}
