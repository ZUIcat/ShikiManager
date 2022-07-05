using System.Text;
using System.Text.Json;
using System.Web;

namespace HelperTranslator {
    public class BaiduTranslator : ITranslator {
        public const string NAME = "Baidu";
        private const string BaiduAPI = @"https://fanyi-api.baidu.com/api/trans/vip/translate?";
        private readonly Dictionary<string, string> Error_code_DesText = new() {
            { "52000", "成功。" },
            { "52001", "请求超时，请重试！" },
            { "52002", "系统错误，请重试！" },
            { "52003", "未授权用户，请检查appid是否正确或者服务是否开通！" },
            { "54000", "必填参数为空，请检查是否少传参数！" },
            { "54001", "签名错误，请检查您的签名生成方法！" },
            { "54003", "访问频率受限，请降低您的调用频率，或进行身份认证后切换为高级版/尊享版！" },
            { "54004", "账户余额不足，请前往管理控制台为账户充值！" },
            { "54005", "长query请求频繁，请降低长query的发送频率，3s后再试！" },
            { "58000", "客户端IP非法，检查个人资料里填写的IP地址是否正确，可前往开发者信息-基本信息修改！" },
            { "58001", "译文语言方向不支持，检查译文语言是否在语言列表里！" },
            { "58002", "服务当前已关闭，请前往管理控制台开启服务！" },
            { "90107", "认证未通过或未生效，请前往我的认证查看认证进度！" },
        };

        public string? ErrorText { get; private set; }

        private readonly StringBuilder tmpSB = new(32);
        private string appId = string.Empty;
        private string secretKey = string.Empty;

        public ITranslator? Init(string parm1, string parm2) {
            appId = parm1;
            secretKey = parm2;
            return this;
        }

        public async Task<string?> TranslateAsync(string sourceText, string srcLang = "jp", string desLang = "zh") {
            if (sourceText == "") {
                ErrorText = "The sourceText is empty!";
                return null;
            }

            string salt = new Random().Next(100000).ToString();
            string sign = TranslatorHelper.EncryptString($"{appId}{sourceText}{salt}{secretKey}");
            string queryUrl = $"{BaiduAPI}q={HttpUtility.UrlEncode(sourceText)}&from={srcLang}&to={desLang}&appid={appId}&salt={salt}&sign={sign}";

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

            BaiduTransOutInfo retInfo;
            try {
                retInfo = JsonSerializer.Deserialize<BaiduTransOutInfo>(retData, TranslatorHelper.Instance.JsonSerializerOptions);
            } catch (JsonException ex) {
                ErrorText = $"The retInfo deserialize failed!\n{ex.Message}";
                return null;
            } catch (Exception ex) {
                ErrorText = ex.Message;
                return null;
            }

            if (retInfo.Error_code != null && retInfo.Error_code != "52000") {
                Error_code_DesText.TryGetValue(retInfo.Error_code, out string? myDesText);
                ErrorText = $"ErrorID: {retInfo.Error_code}\n{myDesText ?? "Unkown Error!"}";
                return null;
            }

            tmpSB.Clear();
            foreach (var baiduTransResultList in retInfo.Trans_result) {
                tmpSB.Append(baiduTransResultList.Dst);
            }
            return tmpSB.ToString();
        }

        public string? GetLastError() {
            return ErrorText;
        }
    }

    public struct BaiduTransOutInfo {
        public string From { get; set; }
        public string To { get; set; }
        public BaiduTransResult[] Trans_result { get; set; }
        public string Error_code { get; set; }
    }

    public struct BaiduTransResult {
        public string Src { get; set; }
        public string Dst { get; set; }
    }
}
