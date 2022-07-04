using HelperConfig;
using HelperMeCab;
using HelperProcess;
using HelperTextractor;
using HelperTranslator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShikiManager {
    /// <summary>
    /// 不知道该如何组织代码
    /// 这次试验一下
    /// 只有一个 DataManager 的单例
    /// 其它的 Manager 都是普通类
    /// 在这里 Create 后暴露 public 的属性
    /// </summary>
    public class DataManager {
        private static DataManager dataManager = null!;
        public static DataManager Instance {
            get {
                dataManager ??= new DataManager();
                return dataManager;
            }
        }

        // Share Property
        public List<TextHookHeadData> SelectHeadDataList { get; private set; }
        public ProcessInfo ProcessInfo { get; set; }
        public Func<string, string>? TextFilterFunc { get; set; }
        public ITranslator NowTranslator { get; private set; } = null!;

        // Share Manager
        public ConfigHelper ConfigHelper { get; private set; }
        public TextractorHelper TextractorHelper { get; private set; }
        public TranslatorHelper TranslatorHelper { get; private set; }
        public MeCabUniDic22Wrapper MeCabUniDic22Wrapper { get; private set; } // TODO IpaDic

        private DataManager() {
            SelectHeadDataList = new List<TextHookHeadData>();

            ConfigHelper = ConfigHelper.Instance;

            TextractorHelper = TextractorHelper.Instance;

            TranslatorHelper = TranslatorHelper.Instance;

            MeCabUniDic22Wrapper = null!;
        }

        /// <summary>
        /// 创建 DataManager
        /// 用于初始化构造函数内无法初始化或不方便初始化的东西
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Create() {
            ConfigHelper.ReadAppConfig();

            TextractorHelper.Create(ConfigHelper.AppConfig.TextractorConfig.DirPath);

            // TranslatorHelper.Create();
            TranslatorHelper.Translators.Add("Youdao_Public", new YoudaoTranslator().Init(null!, null!));
            TranslatorHelper.Translators.Add("Baidu", new BaiduTranslator().Init(ConfigHelper.AppConfig.TranslatorConfig.BaiduConfig.AppId, ConfigHelper.AppConfig.TranslatorConfig.BaiduConfig.SecretKey));
            TranslatorHelper.Translators.TryGetValue(ConfigHelper.AppConfig.TranslatorConfig.NowTranslator, out ITranslator? tmpTranslator);
            NowTranslator = tmpTranslator ?? TranslatorHelper.Translators["Youdao_Public"]!;

            MeCabUniDic22Wrapper = new MeCabUniDic22Wrapper(ConfigHelper.AppConfig.MeCabConfig.DirPath);

            return true;
        }

        /// <summary>
        /// 销毁 DataManager
        /// 用于销毁析构函数内无法销毁或不方便销毁的东西
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Destroy() {
            SelectHeadDataList.Clear();
            SelectHeadDataList = null!;

            ConfigHelper.WriteAppConfig();

            TextractorHelper.Destroy();

            TranslatorHelper.Destroy();

            MeCabUniDic22Wrapper.Destroy();

            return true;
        }
    }
}
