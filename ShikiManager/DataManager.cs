using HelperConfig;
using HelperTextractor;
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

        // Share Manager
        public ConfigHelper ConfigHelper { get; private set; }

        private DataManager() {
            SelectHeadDataList = new List<TextHookHeadData>();
        }

        /// <summary>
        /// 创建 DataManager
        /// 用于初始化构造函数内无法初始化或不方便初始化的东西
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Create() {
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
            return true;
        }
    }
}
