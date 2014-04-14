using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace MyIM
{
    /// <summary>
    /// 记录服务器端和客户端的基本信息，并通过自定义方法MyHostIP获取服务器端所有IP地址
    /// 通过自定义方法Get_Windows获取windows目录
    /// </summary>
    public class Publec_Class
    {
        public static string ServerIP = "";                   //定义服务器端的ip
        public static string ServerPort = "";                 //定义服务器端的端口号
        public static string ClientIP = "";                   //定义客户端的IP
        public static string ClientName = "";                 //定义客户端的名称
        public static string UserName;                        //定义用户名称
        public static string UserID;                          //定义用户的ID号
        /// <summary>
        /// 遍历服务器端所有的IP地址
        /// </summary>
        /// <returns></returns>
        public string MyHostIP()
        {
            string hostname = Dns.GetHostName();              //显示主机名
            IPHostEntry hostent = Dns.GetHostEntry(hostname); //主机信息
            Array addrs = hostent.AddressList;                //与主机相关的ip地址列表
            IEnumerator it = addrs.GetEnumerator();
            while (it.MoveNext())                              //循环到下一个IP地址
            {
                IPAddress ip = (IPAddress)it.Current;
                return ip.ToString();
            }
            return "";
        }

        [DllImport("kernel32")]
        public static extern void GetWindowsDirectory(StringBuilder WinDir, int count);
        /// <summary>
        /// 获取windows目录
        /// </summary>
        /// <returns></returns>
        public string Get_windows()
        {
            const int nChars = 255;
            StringBuilder Buff = new StringBuilder(nChars);//定义一个nchar大小的可变字符串
            GetWindowsDirectory(Buff, nChars);
            return Buff.ToString();
        }
    }
}
