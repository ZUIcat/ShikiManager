using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Web;

namespace HelperTranslator {
    public class YoudaoTranslator : ITranslator {
        public string? ErrorText { get; private set; }

        public ITranslator Init() {
            return this;
        }

        public async Task<string?> TranslateAsync(string sourceText, string srcLang = "JA", string desLang= "ZH_CN") {
            if (sourceText == "") {
                ErrorText = "The sourceText is empty!";
                return null;
            }

            if (srcLang == "zh") srcLang = "ZH_CN";
            if (desLang == "zh") desLang = "ZH_CN";
            if (srcLang == "jp") srcLang = "JA";
            if (desLang == "jp") desLang = "JA";
            if (srcLang == "ja") srcLang = "JA";
            if (desLang == "ja") desLang = "JA";

            string retString;

            string transType = $"{srcLang}2{desLang}".ToUpper();
            string queryText = HttpUtility.UrlEncode(sourceText);
            string url = "https://fanyi.youdao.com/translate?&doctype=json&type=" + transType + "&i=" + queryText;

            try {
                retString = await TranslatorHelper.Instance.MyHttpClient.GetStringAsync(url);
            } catch (HttpRequestException ex) {
                ErrorText = ex.Message;
                return null;
            } catch (TaskCanceledException ex) {
                ErrorText = ex.Message;
                return null;
            }

            YoudaoTransResult retInfo;
            try {
                retInfo = JsonSerializer.Deserialize<YoudaoTransResult>(retString, TranslatorHelper.Instance.JsonSerializerOptions);
            } catch (JsonException) {
                ErrorText = "The retInfo deserialize failed!";
                return null;
            }

            if (retInfo.ErrorCode == 0) {
                var sb = new StringBuilder(32);
                foreach (var youdaoTransDataList in retInfo.TranslateResult) {
                    foreach (var youdaoTransDataItem in youdaoTransDataList) {
                        sb.Append(youdaoTransDataItem.Tgt);
                    }
                }
                return sb.ToString();
            } else {
                ErrorText = "ErrorID:" + retInfo.ErrorCode;
                return null;
            }
        }

        public string? GetLastError() {
            return ErrorText;
        }
    }

    public struct YoudaoTransResult {
        public string Type { get; set; }
        public int ErrorCode { get; set; }
        public int ElapsedTime { get; set; }
        public YoudaoTransData[][] TranslateResult { get; set; }
    }

    public struct YoudaoTransData {
        public string Src { get; set; }
        public string Tgt { get; set; }
    }
}
