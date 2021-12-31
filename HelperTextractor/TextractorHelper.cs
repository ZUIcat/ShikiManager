using System;
using System.Text;
using System.Diagnostics;
using HelperConfig;

namespace HelperTextractor {
    /// <summary>
    /// 模仿(照抄) MisakaTranslator 写的
    /// </summary>
    public class TextractorHelper {
        /// <summary>
        /// Textractor 的历史文本队列的最大长度
        /// </summary>
        private const int OUTPUT_HISTORY_MAX_LEN = 1000;

        /// <summary>
        /// Textractor 的进程
        /// </summary>
        private Process textractorProcess;

        /// <summary>
        /// Textractor 的历史文本队列
        /// </summary>
        private Queue<string> textractorOutPutHistory;

        /// <summary>
        /// Hook 的游戏进程 PID
        /// </summary>
        private int gamePID;

        /// <summary>
        /// 初始化 TextractorHelper
        /// </summary>
        /// <param name="x86">是否是 x86 程序</param>
        /// <returns>是否初始化成功</returns>
        public bool Init(bool x86 = true) {
            string textractorDir = ConfigHelper.Instance.AppConfig.TextractorConfig.DirPath;
            string textractorBinPath = Path.Combine(textractorDir, @"\lib\TextHook\", x86 ? "x86" : "x64");

            string currentPath = Environment.CurrentDirectory;
            try {
                Environment.CurrentDirectory = textractorBinPath;
            } catch (System.IO.DirectoryNotFoundException ex) {
                return false;
            } finally {
                Environment.CurrentDirectory = currentPath;
            }

            textractorProcess = new Process();
            textractorProcess.StartInfo.FileName = "TextractorCLI.exe";
            textractorProcess.StartInfo.CreateNoWindow = true;
            textractorProcess.StartInfo.UseShellExecute = false; // TODO
            textractorProcess.StartInfo.StandardOutputEncoding = Encoding.Unicode;
            textractorProcess.StartInfo.RedirectStandardInput = true;
            textractorProcess.StartInfo.RedirectStandardOutput = true;
            //processTextractor.OutputDataReceived += new DataReceivedEventHandler(OutputHandler); // TODO

            try {
                bool res = textractorProcess.Start();
                textractorProcess.BeginOutputReadLine();
                return res;
            } catch (System.ComponentModel.Win32Exception ex) {
                return false;
            } finally {
                Environment.CurrentDirectory = currentPath;
            }
        }

        /// <summary>
        /// 向 Textractor CLI 写入命令
        /// 注入进程
        /// </summary>
        /// <param name="pid">游戏进程 PID</param>
        private async Task AttachProcess(int pid) {
            await textractorProcess.StandardInput.WriteLineAsync("attach -P" + pid);
            await textractorProcess.StandardInput.FlushAsync();
        }

        /// <summary>
        /// 向 Textractor CLI 写入命令
        /// 结束注入进程
        /// </summary>
        /// <param name="pid">游戏进程 PID</param>
        private async Task DetachProcess(int pid) {
            await textractorProcess.StandardInput.WriteLineAsync("detach -P" + pid);
            await textractorProcess.StandardInput.FlushAsync();
        }

        /// <summary>
        /// 向 Textractor CLI 写入命令
        /// 给定特殊码注入
        /// </summary>
        /// <param name="pid">游戏进程 PID</param>
        /// <param name="hookCode">游戏特殊码</param>
        private async Task AttachProcessByHookCode(int pid, string hookCode) {
            await textractorProcess.StandardInput.WriteLineAsync(hookCode + " -P" + pid);
            await textractorProcess.StandardInput.FlushAsync();
        }

        /// <summary>
        /// 向 Textractor CLI 写入命令
        /// 根据 Hook 入口地址卸载一个 Hook
        /// </summary>
        /// <param name="pid">游戏进程 PID</param>
        /// <param name="hookAddress">游戏 Hook 地址</param>
        private async Task DetachProcessByHookAddress(int pid, string hookAddress) {
            // 这个方法的原理是注入一个用户给定的钩子，给定一个 Hook 地址，由于 Hook 地址存在，Textractor会自动卸载掉之前的
            // 但是后续给定的模块并不存在，于是 Textractor 再卸载掉这个用户自定义钩子，达到卸载一个指定 Hook 办法
            await textractorProcess.StandardInput.WriteLineAsync("HW0@" + hookAddress + ":module_which_never_exists" + " -P" + pid);
            await textractorProcess.StandardInput.FlushAsync();
        }

        /// <summary>
        /// 关闭 Textractor 进程，关闭前 Detach 所有 Hook
        /// </summary>
        public async void Close() {
            if (textractorProcess != null && textractorProcess.HasExited == false) {
                await DetachProcess(gamePID);
                textractorProcess.Kill();
            }
            textractorProcess = null;
        }
    }
}
