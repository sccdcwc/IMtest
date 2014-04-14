using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Drawing.Design;


namespace MyIM
{
    public class VideoAPI
    {
        //视频API调用
        [DllImport("avicap32.dll")]
        public static extern IntPtr capCreateCaptureWindowA(byte[] IpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWhdParent, int nID);
        [DllImport("avicap32.dll")]
        public static extern bool capGetDriverDescriptionA(short wDriver, byte[] lpszName, int cbName, byte[] lpszVer, int cbVer);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, short wParam, int lParam);

        //  常量
        public const int WM_USER = 0x400;
        public const int WS_CHILD = 0x40000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int SWP_NOMOVE = 0x2;
        public const int SWP_NOZORDER = 0x4;
        public const int WM_CAP_DRIVER_CONNECT = WM_USER + 10;
        public const int WM_CAP_DRIVER_DISCONNECT = WM_USER + 11;
        public const int WM_CAP_SET_CALLBACK_FRAME = WM_USER + 5;
        public const int WM_CAP_SET_PREVIEW = WM_USER + 50;
        public const int WM_CAP_SET_PREVIEWRATE = WM_USER + 52;
        public const int WM_CAP_SET_VIDEOFORMAT = WM_USER + 45;
        public const int WM_CAP_START = WM_USER;
        public const int WM_CAP_SAVEDIB = WM_CAP_START + 25;
    }
    public class cVideo  //视频类
    {
        /// <summary>
        /// 保存无符号句柄
        /// </summary>
        private IntPtr IwndC;
        /// <summary>
        /// 保存管理指示器
        /// </summary>
        private IntPtr mControlPtr;
        private int mWidth;
        private int mHeight;
        public cVideo(IntPtr handle, int width, int height)
        {
            mControlPtr = handle;     //显示视频控件的句柄
            mWidth = width;           //视频宽度
            mHeight = height;         //视频高度
        }
        public void StartWebCam()     //打开视频设备
        {
            byte[] IpszName = new byte[100];
            byte[] IpszVer = new byte[100];
            VideoAPI.capGetDriverDescriptionA(0, IpszName, 100, IpszVer, 100);
            this.IwndC = VideoAPI.capCreateCaptureWindowA(IpszName, VideoAPI.WS_CHILD | VideoAPI.WS_VISIBLE, 0, 0, mWidth, mHeight, mControlPtr, 0);
            if (VideoAPI.SendMessage(IwndC, VideoAPI.WM_CAP_DRIVER_CONNECT, 0, 0))
            {
                VideoAPI.SendMessage(IwndC, VideoAPI.WM_CAP_SET_PREVIEWRATE, 100, 0);
                VideoAPI.SendMessage(IwndC, VideoAPI.WM_CAP_SET_PREVIEW, true, 0);
            }
        }

        public void CloseWebcam()   //关闭视频设备
        {
            VideoAPI.SendMessage(IwndC, VideoAPI.WM_CAP_DRIVER_DISCONNECT, 0, 0);
        }

        public void GrabImage(IntPtr hWndC, string path)   //拍照
        {
            IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
            VideoAPI.SendMessage(IwndC, VideoAPI.WM_CAP_SAVEDIB, 0, hBmp.ToInt32());
        }
    }
}
