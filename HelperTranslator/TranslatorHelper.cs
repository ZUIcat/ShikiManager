using System.Security.Cryptography;
using System.Text;
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

        public Dictionary<string, ITranslator?> Translators { get; private set; }

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
                PropertyNameCaseInsensitive = true,
            };

            Translators = new();
        }

        public void Create() {
        }

        public void Destroy() {
            MyHttpClient?.Dispose();
        }

        private static readonly MD5 md5 = MD5.Create();
        private static readonly StringBuilder encryptSB = new();

        /// <summary>
        /// 计算 MD5 值
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptString(string str) {
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            encryptSB.Clear();
            foreach (byte b in byteNew) {
                // 将字节转换成 16 进制表示的字符串，
                encryptSB.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return encryptSB.ToString();
        }
    }
}