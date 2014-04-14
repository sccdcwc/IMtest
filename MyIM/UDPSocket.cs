using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace MyIM
{
    public partial class UDPSocket : Component
    {
        private IPEndPoint ServerEndPoint = null;                      //定义网络端点
        private UdpClient UDP_Server = new UdpClient();                //创建网络服务，也就是UDP的Socket
        private System.Threading.Thread thdUdp;                        //创建一个线程
        //定义一个托管方法
        public delegate void DataArrivalEventHandler(byte[] Data, IPAddress Ip, int Port);
        //通过托管在控件中定义一个事件
        public event DataArrivalEventHandler DataArrival;
        private string localHost = "127.0.0.1";                        //设置默认的IP地址
        [Browsable(true), Category("Local"), Description("本地IP地址")]//在属性窗口中显示LocalHost属性
        public string LocalHost                                        //封装字段
        {
            get { return localHost; }
            set { localHost = value; }
        }
        private int localPort = 11000;
        [Browsable(true), Category("Local"), Description("本地端口号")]//在属性窗口中显示LocalHost属性
        public int LocalPort                                        //封装字段
        {
            get { return localPort; }
            set { localPort = value; }
        }
        private bool active = false;
        [Browsable(true), Category("Local"), Description("激活监听")]//在属性窗口中显示LocalHost属性
        public bool Active                                        //封装字段
        {
            get { return active; }
            set
            {
                active = value;
                if (active)
                {
                    OpenSocket();                              //打开监听
                }
                else
                {
                    CloseSocket();                             //关闭监听
                }
            }
        }
        public UDPSocket()
        {
            InitializeComponent();
        }

        public UDPSocket(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        /// <summary>
        /// 激活Socket
        /// </summary>
        private void OpenSocket()
        {
            Listener();                                               //调用监听方法
        }
        /// <summary>
        /// 关闭Socket
        /// </summary>
        private void CloseSocket()
        {
            if (UDP_Server != null)
                UDP_Server.Close();                                  //若Socket不为空则关闭Socket
            if (thdUdp != null)
            {
                Thread.Sleep(30);                                    //使主线程睡眠
                thdUdp.Abort();                                      //中断子线程
            }
        }
        /// <summary>
        /// 监听
        /// </summary>
        protected void Listener()
        {
            ServerEndPoint = new IPEndPoint(IPAddress.Any, localPort);             //将IP地址和端点号以网络端点存储
            if (UDP_Server != null)
                UDP_Server.Close();                                                //关闭初始化Socket
            UDP_Server = new UdpClient(localPort);
            try
            {
                thdUdp = new Thread(new ThreadStart(GetUDPData));
                thdUdp.Start();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        //private void CallBackMethod(IAsyncResult ar)
        //{
        //    //从异步状态ar.AsyncState中，获取委托对象
        //    DataArrivalEventHandler dn = (DataArrivalEventHandler)ar.AsyncState;
        //    //一定要EndInvoke，否则你的下场很惨
        //    dn.EndInvoke(ar);
        //}

        /// <summary>
        /// 获取当前接收的消息
        /// </summary>
        private void GetUDPData()
        {
            while (active)
            {
                try
                {
                    byte[] Data = UDP_Server.Receive(ref ServerEndPoint);   //储存获取的远程消息
                    if (DataArrival != null)
                    {
                        //调用DataArrival事件
                        DataArrival(Data, ServerEndPoint.Address, ServerEndPoint.Port);
                    }
                    Thread.Sleep(0);
                }
                catch { }
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="Port"></param>
        /// <param name="Data"></param>
        public void Send(IPAddress Host, int Port, byte[] Data)
        {
            try 
            {
                IPEndPoint server = new IPEndPoint(Host, Port);         //用指定的IP地址和端口号初始化Server
                UDP_Server.Send(Data, Data.Length, server);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        
    }
}
