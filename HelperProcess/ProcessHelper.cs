using System;
using System.Diagnostics;

namespace HelperProcess {
    public class ProcessHelper {
        private static List<ProcessInfo> processInfoList = new List<ProcessInfo>();
        public static List<ProcessInfo> GetProcessInfoList() {
            processInfoList.Clear();
            foreach (var process in Process.GetProcesses()) {
                if (process.MainWindowHandle != IntPtr.Zero) {
                    processInfoList.Add(new ProcessInfo { Name = process.ProcessName, ID = process.Id, FileName = process.MainModule.FileName, StartTime = process.StartTime});
                }
                process.Dispose();
            }
            return processInfoList;
        }
    }

    public struct ProcessInfo {
        public string Name { get; set; }
        public int ID { get; set; }
        public string FileName { get; set; }
        public DateTime StartTime { get; set; }
    }
}