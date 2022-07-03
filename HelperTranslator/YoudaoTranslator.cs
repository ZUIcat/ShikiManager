using System.Text;
using System.Text.Json;
using System.Web;

namespace HelperTranslator {
    public class YoudaoTranslator : ITranslator {
        public string? ErrorText { get; private set; }

        private StringBuilder tmpSB = new(32);

        public ITranslator? Init(string parm1, string parm2) {
            return this;
        }

        public async Task<string?> TranslateAsync(string sourceText, string srcLang = "JA", string desLang= "ZH_CN") {
            if (sourceText == "") {
                ErrorText = "The sourceText is empty!";
                return null;
            }

            if (srcLang == "jp") srcLang = "JA";
            if (desLang == "jp") desLang = "JA";
            if (srcLang == "zh") srcLang = "ZH_CN";
            if (desLang == "zh") desLang = "ZH_CN";

            string transType = $"{srcLang}2{desLang}".ToUpper();
            string queryText = HttpUtility.UrlEncode(sourceText);
            string queryUrl = $"https://fanyi.youdao.com/translate?&doctype=json&type={transType}&i={queryText}";

            string retData;
            try {
                retData = await TranslatorHelper.Instance.MyHttpClient.GetStringAsync(queryUrl);
            } catch (HttpRequestException ex) {
                ErrorText = $"There is a HttpRequestException!\n{ex.Message}";
                return null;
            } catch (TaskCanceledException ex) {
                ErrorText = $"There is a TaskCanceledException!\n{ex.Message}";
                return null;
            } catch (Exception ex) {
                ErrorText = ex.Message;
                return null;
            }

            YoudaoTransResult retInfo;
            try {
                retInfo = JsonSerializer.Deserialize<YoudaoTransResult>(retData, TranslatorHelper.Instance.JsonSerializerOptions);
            } catch (JsonException ex) {
                ErrorText = $"The retInfo deserialize failed!\n{ex.Message}";
                return null;
            } catch (Exception ex) {
                ErrorText = ex.Message;
                return null;
            }

            if (retInfo.ErrorCode != 0) {
                ErrorText = $"ErrorID: {retInfo.ErrorCode}";
                return null;
            }

            tmpSB.Clear();
            foreach (var youdaoTransDataList in retInfo.TranslateResult) {
                foreach (var youdaoTransDataItem in youdaoTransDataList) {
                    tmpSB.Append(youdaoTransDataItem.Tgt);
                }
            }
            return tmpSB.ToString();
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
