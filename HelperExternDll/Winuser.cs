using System;
using System.Runtime.InteropServices;

namespace HelperExternDll {
    public partial class Winuser {
        public static readonly Int32 GWL_EXSTYLE = -20;
        public static readonly IntPtr WS_EX_NOACTIVATE = (IntPtr)0x08000000;

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, Int32 nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, Int32 nIndex) {
            if (IntPtr.Size == 8) {
                return GetWindowLongPtr64(hWnd, nIndex);
            } else {
                return GetWindowLongPtr32(hWnd, nIndex);
            }
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern Int32 SetWindowLong32(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong) {
            if (IntPtr.Size == 8) {
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            } else {
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
            }
        }
    }

    public partial class Winuser {
        /// <summary>
        /// 设置窗口不获取焦点, 不能放到构造函数里, 否则窗体句柄为 0.
        /// </summary>
        /// <param name="HWND">窗口句柄</param>
        public static void SetWindowNoActivate(IntPtr HWND) {
            SetWindowLongPtr(HWND, GWL_EXSTYLE, (IntPtr)((Int32)GetWindowLongPtr(HWND, GWL_EXSTYLE) | (Int32)WS_EX_NOACTIVATE));
        }
    }
}
