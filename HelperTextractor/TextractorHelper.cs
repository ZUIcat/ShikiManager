using System;
using System.Text;
using System.Diagnostics;

namespace HelperTextractor {
    /// <summary>
    /// Textractor 的辅助类
    /// 模仿 MisakaTranslator 写的
    /// 一个程序只有一个 TextractorHelper
    /// 但是一个 TextractorHelper 可以 Hook 多个游戏
    /// </summary>
    public class TextractorHelper {
        private static TextractorHelper instance = null!;

        public static TextractorHelper Instance {
            get {
                instance ??= new TextractorHelper();
                return instance;
            }
        }

        /// <summary>
        /// Textractor 的进程
        /// 一个 TextractorHelper 只允许有一个 Textractor 进程
        /// </summary>
        private Process? TextractorProcess { get; set; }

        /// <summary>
        /// TextractorHelper 输出剧情文本时会调用此事件
        /// </summary>
        public event Action<TextHookData>? TextOutputEvent;

        /// <summary>
        /// Textractor 文本存储字典
        /// </summary>
        public Dictionary<TextHookHeadData, TextHookData> TextractorOutPutDic { get; private set; }

        /// <summary>
        /// TextractorHelper 是否暂停处理文本
        /// </summary>
        public bool IsPause { get; set; }

        private TextractorHelper() {
            TextractorOutPutDic = new Dictionary<TextHookHeadData, TextHookData>();
            IsPause = false;
        }

        /// <summary>
        /// 初始化 TextractorHelper
        /// 在此创建 Textractor 进程
        /// </summary>
        /// <param name="textractorDir">textractor 所在的目录</param>
        /// <param name="x86">是否使用 x86 版本，否则使用 x64 版本</param>
        /// <returns>是否初始化成功</returns>
        public bool Create(string textractorDir, bool x86 = true) {
            if (TextractorProcess != null) {
                Trace.TraceError("TextractorHelper has been created.");
                return false;
            }

            string textractorBinPath = Path.Combine(textractorDir, x86 ? "x86" : "x64");
            string appPath = Environment.CurrentDirectory;
            try {
                Environment.CurrentDirectory = textractorBinPath;

                TextractorProcess = new Process();
                TextractorProcess.StartInfo.FileName = @"TextractorCLI.exe";
                TextractorProcess.StartInfo.CreateNoWindow = true;
                TextractorProcess.StartInfo.UseShellExecute = false;
                TextractorProcess.StartInfo.StandardInputEncoding = new UnicodeEncoding(false, false); // 默认的 Encoding.Unicode 存在 BOM 头，Textractor 那边会解析失败
                TextractorProcess.StartInfo.StandardOutputEncoding = Encoding.Unicode;
                TextractorProcess.StartInfo.StandardErrorEncoding = Encoding.Unicode;
                TextractorProcess.StartInfo.RedirectStandardInput = true;
                TextractorProcess.StartInfo.RedirectStandardOutput = true;
                TextractorProcess.StartInfo.RedirectStandardError = true;
                TextractorProcess.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                TextractorProcess.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

                TextractorProcess.Start();
                TextractorProcess.BeginOutputReadLine();
                TextractorProcess.BeginErrorReadLine();
                return true;
            } catch (Exception e) {
                Trace.TraceError(e.Message);
                TextractorProcess?.Kill();
                TextractorProcess?.Dispose();
                TextractorProcess = null;
                return false;
            } finally {
                Environment.CurrentDirectory = appPath;
            }
        }


        private void OutputHandler(object sender, DataReceivedEventArgs e) {
#if DEBUG
            Trace.TraceInformation("OutputHandler: " + e.Data);
#endif

            if (IsPause || e.Data == null || !e.Data.StartsWith("[")) {
                return;
            }

            // 解析数据生成对象
            TextHookData textHookData;
            try {
                textHookData = new(e.Data); // TODO 一行中多条信息 FactoryMode return array
            } catch (Exception ex) {
                Trace.TraceError(ex.Message);
                return;
            }

            // 加入到文本储存字典
            if(!TextractorOutPutDic.ContainsKey(textHookData.HeadData)) {
                TextractorOutPutDic.Add(textHookData.HeadData, textHookData);
            } else {
                TextractorOutPutDic[textHookData.HeadData] = textHookData;
            }

            // 调用外部事件
            TextOutputEvent?.Invoke(textHookData);

#if DEBUG
            Trace.TraceInformation("OutputHandler: " + textHookData.TextData);
#endif
        }

        private void ErrorHandler(object sender, DataReceivedEventArgs e) {
#if DEBUG
            Trace.TraceInformation("ErrorHandler: " + e.Data);
#endif
        }

        /// <summary>
        /// 向 Textractor 写入命令
        /// 注入进程
        /// </summary>
        /// <param name="pid">游戏进程 PID</param>
        public async Task AttachProcess(int pid) {
            var command = $"attach -P{pid}";
            Trace.TraceInformation($"AttachProcess async: {command}");
            if (TextractorProcess != null) {
                await TextractorProcess.StandardInput.WriteLineAsync(command);
                await TextractorProcess.StandardInput.FlushAsync();
            } else {
                Trace.TraceError("TextractorProcess is null!");
            }
        }

        /// <summary>
        /// 向 Textractor 写入命令
        /// 结束注入进程
        /// </summary>
        /// <param name="pid">游戏进程 PID</param>
        public async Task DetachProcess(int pid) {
            var command = $"detach -P{pid}";
            Trace.TraceInformation($"DetachProcess async: {command}");
            if (TextractorProcess != null) {
                await TextractorProcess.StandardInput.WriteLineAsync(command);
                await TextractorProcess.StandardInput.FlushAsync();
            } else {
                Trace.TraceError("TextractorProcess is null!");
            }
        }

        /// <summary>
        /// 向 Textractor 写入命令
        /// 给定特殊码注入
        /// </summary>
        /// <param name="pid">游戏进程 PID</param>
        /// <param name="hookCode">游戏特殊码</param>
        public async Task AttachProcessByHookCode(int pid, string hookCode) {
            var command = $"{hookCode} -P{pid}";
            Trace.TraceInformation($"AttachProcessByHookCode async: {command}");
            if (TextractorProcess != null) {
                await TextractorProcess.StandardInput.WriteLineAsync(command);
                await TextractorProcess.StandardInput.FlushAsync();
            } else {
                Trace.TraceError("TextractorProcess is null!");
            }
        }

        /// <summary>
        /// 向 Textractor 写入命令
        /// 根据 Hook 入口地址卸载一个 Hook
        /// </summary>
        /// <param name="pid">游戏进程 PID</param>
        /// <param name="hookAddress">游戏 Hook 地址</param>
        public async Task DetachProcessByHookAddress(int pid, string hookAddress) {
            // 这个方法的原理是注入一个用户给定的钩子，给定一个 Hook 地址，由于 Hook 地址存在，Textractor 会自动卸载掉之前的
            // 但是后续给定的模块并不存在，于是 Textractor 再卸载掉这个用户自定义钩子，达到卸载一个指定 Hook 办法
            var command = $"HW0@{hookAddress}:module_which_never_exists -P{pid}";
            Trace.TraceInformation($"DetachProcessByHookAddress async: {command}");
            if (TextractorProcess != null) {
                await TextractorProcess.StandardInput.WriteLineAsync(command);
                await TextractorProcess.StandardInput.FlushAsync();
            } else {
                Trace.TraceError("TextractorProcess is null!");
            }
        }

        /// <summary>
        /// 向 Textractor 写入命令
        /// 根据 TextHookHeadData 卸载一个 Hook
        /// </summary>
        /// <param name="textHookHeadData">TextHook 头部信息内容</param>
        /// <returns></returns>
        public async Task DetachProcessByTextHookHeadData(TextHookHeadData textHookHeadData) {
            int processID = textHookHeadData.ProcessID;
            string hookAddress = textHookHeadData.Address;
            await DetachProcessByHookAddress(processID, hookAddress);
        }

        /// <summary>
        /// 向 Textractor 写入命令
        /// 根据 TextHookHeadData 的集合卸载这些集合之外的其它同进程 Hook
        /// 并删除文本储存字典中相应的键值
        /// </summary>
        /// <param name="textHookData">TextHookHeadData 的集合</param>
        /// <returns></returns>
        public async Task DetachProcessByTextHookData(IEnumerable<TextHookData> textHookData) {
            // 按 ProcessID 分组
            var query = textHookData
                .GroupBy(item => item.HeadData.ProcessID);
            // 按组处理
            foreach (var item in query) {
                // 找出符合条件的
                var query2 = TextractorOutPutDic.Keys
                    .Where(itemA => itemA.ProcessID == item.Key && item.All(itemB => itemB.HeadData.Address != itemA.Address));
                // 按 Address 去重
                var query3 = query2
                    .GroupBy(item => item.Address)
                    .Select(item => item.First());
                // Detach Hook
                foreach (var item2 in query3) {
                    await DetachProcessByTextHookHeadData(item2);
                }
                // 删除文本储存字典中相应的键值
                // 为啥没报错是因为：https://docs.microsoft.com/zh-cn/dotnet/api/system.collections.generic.dictionary-2.enumerator?view=net-6.0
                foreach (var item2 in query2) {
                    TextractorOutPutDic.Remove(item2);
                }
            }
        }

        /// <summary>
        /// 关闭 Textractor 进程，关闭前 Detach 所有 Hook
        /// </summary>
        public async void Destroy() {
            if (TextractorProcess != null && TextractorProcess.HasExited == false) {
                var query = TextractorOutPutDic.Keys
                    .Where(item => item.ProcessID > 0)
                    .Select(item => item.ProcessID)
                    .Distinct();
                foreach (var item in query) {
                    await DetachProcess(item);
                }
                TextractorProcess.Kill();
            }
            TextractorProcess?.Dispose();
            TextractorProcess = null;

            TextOutputEvent = null;
            TextractorOutPutDic.Clear();
            IsPause = false;
        }
    }
}
