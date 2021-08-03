using System;
using System.Runtime.InteropServices;

namespace HelperExternDll {
    public partial class Winuser {
        // -- //
        public static readonly Int32 GWL_EXSTYLE = -20;
        public static readonly UInt32 WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr_PI(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr_PI(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

        // -- //
        public static readonly Int32 SM_CXSCREEN = 0;
        public static readonly Int32 SM_CYSCREEN = 1;
        public static readonly Int32 SM_XVIRTUALSCREEN = 76;
        public static readonly Int32 SM_YVIRTUALSCREEN = 77;

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        private static extern int GetSystemMetrics_PI(Int32 index);

        // -- //
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int X;
            public int Y;
            public POINT(int x, int y) {
                X = x;
                Y = y;
            }
        }

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean GetCursorPos_PI(out POINT lpPoint);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetCursorPos_PI(Int32 x, Int32 y);
    }

    public partial class Winuser {
        /// <summary>
        /// 设置窗口不获取焦点, 不能放到构造函数里, 否则窗体句柄为 0.
        /// </summary>
        /// <param name="HWND">窗口句柄</param>
        /// <returns></returns>
        public static IntPtr SetWindowNoActivate(IntPtr HWND) {
            return SetWindowLongPtr_PI(HWND, GWL_EXSTYLE, (IntPtr)((UInt64)GetWindowLongPtr_PI(HWND, GWL_EXSTYLE) | (UInt64)WS_EX_NOACTIVATE));
        }

        /// <summary>
        /// 获取主显示监视器屏幕的宽度, 单位是像素.
        /// </summary>
        /// <returns>宽度</returns>
        public static Int32 GetPrimaryScreenWidth() {
            return GetSystemMetrics_PI(SM_CXSCREEN);
        }

        /// <summary>
        /// 获取主显示监视器屏幕的高度, 单位是像素.
        /// </summary>
        /// <returns>高度</returns>
        public static Int32 GetPrimaryScreenHeight() {
            return GetSystemMetrics_PI(SM_CYSCREEN);
        }

        /// <summary>
        /// 获取虚拟监视器屏幕的宽度, 单位是像素.
        /// </summary>
        /// <returns>宽度</returns>
        public static Int32 GetVirtualScreenWidth() {
            return GetSystemMetrics_PI(SM_XVIRTUALSCREEN);
        }

        /// <summary>
        /// 获取虚拟显示监视器屏幕的高度, 单位是像素.
        /// </summary>
        /// <returns>高度</returns>
        public static Int32 GetVirtualScreenHeight() {
            return GetSystemMetrics_PI(SM_YVIRTUALSCREEN);
        }

        /// <summary>
        /// 获取鼠标现在的位置, 单位是像素.
        /// </summary>
        /// <param name="lpPoint">存储鼠标位置的结构体</param>
        /// <returns></returns>
        public static Boolean GetCursorPos(out POINT lpPoint) {
            return GetCursorPos_PI(out lpPoint);
        }

        /// <summary>
        /// 设置鼠标现在的位置, 单位是像素.
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns></returns>
        public static Boolean SetCursorPos(Int32 x, Int32 y) {
            return SetCursorPos_PI(x, y);
        }
    }
}
