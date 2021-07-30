using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HelperConfig {
    public class ConfigHelper {
        // Instance
        private static ConfigHelper instance;

        // Get instance
        public static ConfigHelper Instance {
            get {
                if (instance == null) instance = new ConfigHelper();
                return instance;
            }
        }

        // Const
        private const string dataPath = @".\Data";
        private const string appConfigFileName = "AppConfig.json";
        private const string gameConfigFileName = "ShikiGameConfig.json";

        // Readonly
        private readonly string appBasePath;
        private readonly string appDataPath;
        private readonly string appConfigPath;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        // Value
        private AppConfig appConfig;

        // Get Property
        public string AppBasePath { get => appBasePath; }
        public string AppDataPath { get => appDataPath; }
        public string AppConfigPath { get => appConfigPath; }
        public AppConfig AppConfig { get => appConfig; }

        private ConfigHelper() {
            appBasePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
#if DEBUG
            appBasePath = Path.Combine(appBasePath, "../../../../");
            System.Windows.MessageBox.Show("Debug Mode.");
#endif
            appDataPath = Path.Combine(appBasePath, dataPath);
            appConfigPath = Path.Combine(appDataPath, appConfigFileName);
            jsonSerializerOptions = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
        }

        public AppConfig ReadAppConfig() {
            if (File.Exists(appConfigPath)) {
                string jsonString = File.ReadAllText(appConfigPath);
                appConfig = JsonSerializer.Deserialize<AppConfig>(jsonString, jsonSerializerOptions);
            } else {
                appConfig = new AppConfig();
            }
            return appConfig;
        }

        public void WriteAppConfig() {
            Directory.CreateDirectory(appDataPath);
            string jsonString = JsonSerializer.Serialize<AppConfig>(appConfig, jsonSerializerOptions);
            File.WriteAllText(appConfigPath, jsonString);
        }

        public ShikiGameConfig ReadGameConfig(string gameDirPath) {
            ShikiGameConfig gameConfig;
            string gameConfigPath = Path.Combine(gameDirPath, gameConfigFileName);
            if (File.Exists(gameConfigPath)) {
                string jsonString = File.ReadAllText(gameConfigPath);
                gameConfig = JsonSerializer.Deserialize<ShikiGameConfig>(jsonString, jsonSerializerOptions);
            } else {
                gameConfig = new ShikiGameConfig();
            }
            return gameConfig;
        }

        public void WriteGameConfig(string gameDirPath, ShikiGameConfig gameConfig) {
            string gameConfigPath = Path.Combine(gameDirPath, gameConfigFileName);
            Directory.CreateDirectory(gameDirPath);
            string jsonString = JsonSerializer.Serialize<ShikiGameConfig>(gameConfig, jsonSerializerOptions);
            File.WriteAllText(gameConfigPath, jsonString);
        }
    }
}
