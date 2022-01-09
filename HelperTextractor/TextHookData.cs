using System;
using System.Text.RegularExpressions;

namespace HelperTextractor {
    /// <summary>
    /// TextHookData 数据结构
    /// </summary>
    public class TextHookData {
        /// <summary>
        /// 提取信息的正则表达式
        /// </summary>
        private static readonly Regex patternRegex = new(@"\[\w+?:(\w+?):((\w+?):(\w+?):(\w+?)):(\w+?):(.+)\]\s(.*)", RegexOptions.Compiled);

        /// <summary>
        /// 头部信息内容
        /// </summary>
        public TextHookHeadData HeadData { get; set;}

        /// <summary>
        /// 输出文本内容
        /// </summary>
        public string TextData { set; get; }

        public TextHookData(string? rawTextData) {
            // [3:1C1C:417E20:419E80:18:KiriKiriZ:HW-8*14:-8*0@167E20:ああああ.exe] ああああ
            // [4:1C1C:417E20:419EBA:18:KiriKiriZ:HW-8*14:-8*0@167E20:ああああ.exe] ああああ
            // [2:1C1C:77936D10:4731CD:0:GetTextExtentPoint32W:HQ8@0:gdi32.dll:GetTextExtentPoint32W] ああああ
            // [6:1C1C:77936D10:4733ED:0:GetTextExtentPoint32W:HQ8@0:gdi32.dll:GetTextExtentPoint32W] ああああ
            // [7:1C1C:77934190:47358A:0:GetGlyphOutlineW:HW8@0:gdi32.dll:GetGlyphOutlineW] ああああ
            // [5:1C1C:77934190:473368:0:GetGlyphOutlineW:HW8@0:gdi32.dll:GetGlyphOutlineW] ああああ
            // 方括号内以冒号分隔，长度 >= 7，分别为：
            // 0：Textrator 自己的 handle 序号 ID
            // 1：进程 ID（16进制）
            // 2：Hook 入口地址
            // 3：The context of the hook: by default the first value on stack, usually the return address
            // 4：The subcontext of the hook: 0 by default, generated in a method specific to the hook
            // 5：Textrator 自己的 handler name：为 Console 时代表 Textrator 本体控制台输出，为 Clipboard 时代表从剪贴板获取的文本
            // 6&7&8：特殊码，详细定义见 Textractor

            if (string.IsNullOrEmpty(rawTextData)) {
                throw new Exception("The rawTextData is null or empty!");
            }

            var match = patternRegex.Match(rawTextData);

            if (!match.Success) {
                throw new Exception("The rawTextData parse failed!");
            }

            var matchGroups = match.Groups; // 0 是整个匹配串，从 1 开始才是 group 捕获
            HeadData = new TextHookHeadData(
                int.Parse(matchGroups[1].Value, System.Globalization.NumberStyles.HexNumber),
                matchGroups[3].Value,
                matchGroups[4].Value,
                matchGroups[5].Value,
                matchGroups[6].Value,
                matchGroups[7].Value,
                matchGroups[2].Value
            );
            TextData = matchGroups[8].Value;
        }
    }

    public class TextHookHeadData { // TODO to struct
        /// <summary>
        /// 进程 ID
        /// </summary>
        public int ProcessID { set; get; }

        /// <summary>
        /// Hook 地址
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// The context of the hook
        /// </summary>
        public string Context { set; get; }

        /// <summary>
        /// The subcontext of the hook
        /// </summary>
        public string Context2 { set; get; }

        /// <summary>
        /// Hook 方法名
        /// </summary>
        public string HandlerName { set; get; }

        /// <summary>
        /// 通用特殊码
        /// </summary>
        public string HookCode { set; get; }

        /// <summary>
        /// 自定义唯一标识数据
        /// </summary>
        public string CustomIdentification { set; get; }

        public TextHookHeadData(int processID, string address, string context, string context2, string handlerName, string hookCode, string customIdentification) {
            ProcessID = processID;
            Address = address;
            Context = context;
            Context2 = context2;
            HandlerName = handlerName;
            HookCode = hookCode;
            CustomIdentification = customIdentification;
        }
    }
}
