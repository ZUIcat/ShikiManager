using System;
using System.Diagnostics;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HelperConfig {
    public class ConfigHelper {
        private static ConfigHelper instance = null!;

        public static ConfigHelper Instance {
            get {
                instance ??= new ConfigHelper();
                return instance;
            }
        }

        public const string dataPath = @".\Data";
        public const string appConfigFileName = "AppConfig.json";
        public const string gameConfigFileName = "ShikiGameConfig.json";

        private readonly JsonSerializerOptions jsonSerializerOptions;

        public string AppBasePath { get; private set; }
        public string AppDataPath { get; private set; }
        public string AppConfigPath { get; private set; }

        public AppConfig AppConfig { get; private set; }

        private ConfigHelper() {
            AppBasePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
#if DEBUG
            AppBasePath = Path.Combine(AppBasePath, "../../../../");
            Environment.CurrentDirectory = AppBasePath;
#endif
            AppDataPath = Path.Combine(AppBasePath, dataPath);
            AppConfigPath = Path.Combine(AppDataPath, appConfigFileName);
            jsonSerializerOptions = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
            };
        }

        public AppConfig ReadAppConfig() {
            if (File.Exists(AppConfigPath)) {
                string jsonString = File.ReadAllText(AppConfigPath);
                AppConfig = JsonSerializer.Deserialize<AppConfig>(jsonString, jsonSerializerOptions);
            } else {
                AppConfig = new AppConfig();
            }
            return AppConfig;
        }

        public void WriteAppConfig() {
            Directory.CreateDirectory(AppDataPath);
            string jsonString = JsonSerializer.Serialize(AppConfig, jsonSerializerOptions);
            File.WriteAllText(AppConfigPath, jsonString);
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
