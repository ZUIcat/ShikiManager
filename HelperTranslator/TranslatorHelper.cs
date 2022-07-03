using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HelperTranslator {
    public class TranslatorHelper {
        private static TranslatorHelper instance = null!;
        public static TranslatorHelper Instance {
            get {
                instance ??= new TranslatorHelper();
                return instance;
            }
        }

        public HttpClient MyHttpClient { get; private set; }

        public JsonSerializerOptions JsonSerializerOptions { get; private set; }

        public Dictionary<string, ITranslator> Translators { get; private set; }


        private TranslatorHelper() {
            MyHttpClient = new() {
                Timeout = TimeSpan.FromSeconds(5)
            };
            var headers = MyHttpClient.DefaultRequestHeaders;
            headers.UserAgent.ParseAdd("ShiKiManager");
            headers.Connection.ParseAdd("keep-alive");
            // System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12; // For FX4.7 // TODO 这是啥啊

            JsonSerializerOptions = new() {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            Translators = new() {
                { "Youdao_Public", new YoudaoTranslator().Init() }
            };
        }

        ~TranslatorHelper() {
            MyHttpClient?.Dispose(); // TODO 这玩意应该不起作用
        }
    }
}