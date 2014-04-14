using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using MyIM;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace IM_Server
{
    /// <summary>
    /// 
    /// </sumsmary>
    /// <param name="?"></param>
    public delegate void LoadCurrUserList();
    
    public partial class F_Server : Form
    {
        public F_Server()
        {
            InitializeComponent();
            udpSocket1.DataArrival+=new UDPSocket.DataArrivalEventHandler(this.DataArrival);
        }

        public void LoadUsrLst()
        {
            LoadCurrUserList lcUlist = new LoadCurrUserList(UpdateUser);
            this.Invoke(lcUlist);
        }
        /// <summary>
        /// 发送信息给所有用户
        /// </summary>
        /// <param name="msg"></param>
        private void SendMsgToAll(ClassMsg msg)
        {
            try
            {
                foreach (ListViewItem item in this.LV_SysUser.Items)
                {
                    IPAddress ip = IPAddress.Parse(item.SubItems[1].Text);
                    int port = Convert.ToInt32(item.SubItems[2].Text);
                    MemoryStream stream = new ClassSerializers().SerializeBinary(msg);
                    UDPSocket udp = new UDPSocket();
                    udp.Send(ip, port, stream.ToArray());
                }
            }
            catch
            { }
        }
        private void SendMsgToOne(IPAddress ip, int port, ClassMsg msg)
        {
            try
            {
                MemoryStream stream = new ClassSerializers().SerializeBinary(msg);
                UDPSocket udp = new UDPSocket();
                udp.Send(ip, port, stream.ToArray());
            }
            catch
            { }
        }
        private void F_Server_Load(object sender, EventArgs e)
        {
            UpdateUser();
        }
        /// <summary>
        /// 更新用户列表
        /// </summary>
        public void UpdateUser()
        {
            MyIM.ClassOptionData optiondata = new MyIM.ClassOptionData();
            DataTable datareader = optiondata.ExSQLReDr("select * from CurreneyUser");
            LV_SysUser.Items.Clear();
            int i = 0;
            while (datareader.Rows.Count != i)
            {
                DataRow dtr = datareader.NewRow();
                dtr = datareader.Rows[i];
                ListViewItem listItem = new ListViewItem();
                listItem.Text = dtr["ID"].ToString();
                listItem.SubItems.Add(dtr["IP"].ToString());
                listItem.SubItems.Add(dtr["Port"].ToString());
                listItem.SubItems.Add(dtr["Name"].ToString());
                listItem.SubItems.Add(dtr["Sign"].ToString());

                LV_SysUser.Items.Add(listItem);
                i++;
            }
            optiondata.Dispose();

        }
        /// <summary>
        /// 打开监听或关闭监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tool_Open(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Text == "开始服务")
            {
                ((ToolStripMenuItem)sender).Text = "结束服务";
                udpSocket1.Active = true;
                
            }
            else
            {
                ((ToolStripMenuItem)sender).Text = "开始服务";
                udpSocket1.Active = false;
            }
        }

        private void socketUDP1_DataArrival(byte[] Data, IPAddress ip, int port)
        {
            DataArrivaldelegate outdelegate = new DataArrivaldelegate(DataArrival);
            this.BeginInvoke(outdelegate, new object[] { Data, ip, port });
        }
        private delegate void DataArrivaldelegate(byte[] Data, System.Net.IPAddress Ip, int Port);

        /// <summary>
        /// 当有数据到达后的处理进程
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private void DataArrival(byte[] Data, IPAddress ip, int port)
        {
            try
            {
                ClassMsg msg = new ClassSerializers().DeSerializeBinary((new MemoryStream(Data))) as ClassMsg;
                switch (msg.sendKind)
                {
                    case SendKind.SendCommand:  //命令
                        {
                            switch (msg.msgCommand)
                            {
                                case MsgCommand.Registering://注册用户
                                    RegisterUser(msg, ip, port);
                                    break;
                                case MsgCommand.Logining://登录用户
                                    UserLogin(msg, ip, port, 1);
                                    break;
                                case MsgCommand.UserList://用户列表
                                    SendUserList(msg, ip, port);
                                    break;
                                case MsgCommand.SendToOne://发送消息给单用户
                                    SendUserMsg(msg, ip, port);
                                    break;
                                case MsgCommand.Close://下线
                                    UpdateUserState(msg, ip, port);
                                    break;
                            }
                            break;
                        }
                    case SendKind.SendMsg://消息
                        {
                            switch (msg.msgCommand)
                            {
                                case MsgCommand.SendToOne://发送消息给单用户
                                    SendUserMsg(msg, ip, port);
                                    break;
                            }
                            break;
                        }
                    case SendKind.SendFile://文件
                        {
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="state"></param>
        private void UserLogin(ClassMsg msg, IPAddress ip, int port, int state)
        {
             
            LoginMsg loginmsg = (LoginMsg)new ClassSerializers().DeSerializeBinary(new MemoryStream(msg.Data));
            ClassOptionData OptionData = new ClassOptionData();//创建并引用ClassOptionData
            MsgCommand msgState = msg.msgCommand;   //获取接收消息的命令
            String UserName = loginmsg.UserName;//登录用户名称
            String PassWord = loginmsg.PassWord;//用户密码
            String vIP = ip.ToString();//用户IP地址

            DataTable DataReader = OptionData.ExSQLReDr("Select * From user Where UserAccount = " + "'" + UserName + "'" + " and UserPassWord = "
                            + "'" + PassWord + "'");//在数据库中通过用户名和密码进行查找


            if (DataReader.Rows.Count != 0)//当DataReader中有记录信息时
            {
                
                string ID = DataReader.Rows[0]["UserID"].ToString();//获取第一条记录中的ID字段值
                //修改当前记录的标识为上线状态
                OptionData.ExSQL("Update tb_CurreneyUser Set Sign = " + Convert.ToString((int)(MsgCommand.Logined)) + ",IP = " + "'" + vIP + "',Port = " + "'" + port.ToString() + "'" + " Where ID = " + ID);
                msg.msgCommand = MsgCommand.Logined;//设置为上线命令
                msg.SID = ID;//用户ID值
                SendMsgToOne(ip, port, msg);//将消息返回给发送用户
                UpdateUserState(msg, ip, port);//更新用户在线状态
            }
            else
            {
                SendMsgToOne(ip, port, msg);
            }
            OptionData.Dispose();
            UpdateUser();//更新用户列表
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        private void RegisterUser(ClassMsg msg, System.Net.IPAddress Ip, int Port)
        {
            msg = InsertUser(msg, Ip, Port);
            UpdateUserList(msg, Ip, Port);
        }
        /// <summary>
        /// 插入用户
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private ClassMsg InsertUser(ClassMsg msg, IPAddress ip, int port)
        {
            RegisterMsg registermsg = (RegisterMsg)new ClassSerializers().DeSerializeBinary(new MemoryStream(msg.Data));

            ClassOptionData OptionData = new ClassOptionData();
            MsgCommand Sate = msg.msgCommand;
            string UserName = registermsg.UserName;     // 注册用户名称
            string PassWord = registermsg.PassWord;     //注册用户密码
            string vIP = ip.ToString();                 //注册用户的IP地址
            //向数据库中添加注册信息
            OptionData.ExSQL("insert into CurreneyUser (IP,Port,Name,PassWord,Sign) values ('" + vIP + "'," +
                port.ToString() + ",'" + UserName + "','" + PassWord + "'," + Convert.ToString((int)(MsgCommand.Registered)) + ")");
            DataTable DataReader = OptionData.ExSQLReDr("select * from CurreneyUser");

            LoadUsrLst();
            //UpdateUser();//更新用户列表
            OptionData.Dispose();
            msg.msgCommand = MsgCommand.Registered;//用户注册结束命令
            SendMsgToOne(ip, port, msg);//将注册命令返回给注册用户
            return msg;
        }
        /// <summary>
        /// 更新用户列表
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private void UpdateUserList(ClassMsg msg, IPAddress ip, int port)
        {
            ClassUsers Users = new ClassUsers();
            ClassOptionData OptionData = new ClassOptionData();
            DataTable DataReader = OptionData.ExSQLReDr("select * from CurreneyUser");
            int i = 0;
            while (DataReader.Rows.Count != i)
            {
                DataRow dtr = DataReader.NewRow();
                dtr = DataReader.Rows[i];
                ClassUserInfo UserItem = new ClassUserInfo();              //创建并引用ClassUserInfo类
                UserItem.UserID = dtr["ID"].ToString();                   //记录用户用编号
                UserItem.UserIP = dtr["IP"].ToString();                 //记录用户的IP地址
                UserItem.UserPort = dtr["Port"].ToString();               //记录端口号
                UserItem.UserName = dtr["Name"].ToString();               //记录用户名称
                UserItem.State = dtr["Sign"].ToString();                  //记录当前状态
                Users.add(UserItem);                                       //将单用户信息添加到用户列表中
                i++;
            }
            
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();      //创建一个内存流存储区
            serializer.Serialize(memStream, Users);         //将对象序列化为二进制流

            msg.Data = memStream.ToArray();//new ClassSerializers().SerializeBinary(Users).ToArray();//将用户列表写入到二进制流中
            //查找当前已上线的用户
            DataReader = OptionData.ExSQLReDr("Select * From CurreneyUser Where Sign = " + MsgCommand.Logined);
            i = 0;
            while (DataReader.Rows.Count==i)//向所有上线用户发送用户列表
            {

                udpSocket1.Send(IPAddress.Parse(DataReader.Rows[i]["IP"].ToString()),Convert.ToInt16( DataReader.Rows[i]["Port"]), new ClassSerializers().SerializeBinary(msg).ToArray());
                i++;
            }
            OptionData.Dispose();
        }
        /// <summary>
        /// 发送用户信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private void SendUserList(ClassMsg msg, IPAddress ip, int port)
        {
            ClassUsers Users = new ClassUsers();
            ClassOptionData OptionData = new ClassOptionData();
            DataTable DataReader = OptionData.ExSQLReDr("Select * From CurreneyUser");
            int i = 0;
            while (DataReader.Rows.Count != i)
            {
                DataRow dtr = DataReader.NewRow();
                dtr = DataReader.Rows[i];
                ClassUserInfo UserItem = new ClassUserInfo();              //创建并引用ClassUserInfo类
                UserItem.UserID = dtr["ID"].ToString();                   //记录用户用编号
                UserItem.UserIP = dtr["IP"].ToString();                 //记录用户的IP地址
                UserItem.UserPort = dtr["Port"].ToString();               //记录端口号
                UserItem.UserName = dtr["Name"].ToString();               //记录用户名称
                UserItem.State = dtr["Sign"].ToString();                  //记录当前状态
                Users.add(UserItem);                                       //将单用户信息添加到用户列表中
                i++;
            }
            OptionData.Dispose();
            msg.Data = new ClassSerializers().SerializeBinary(Users).ToArray();
            udpSocket1.Send(ip, port, new ClassSerializers().SerializeBinary(msg).ToArray());
        }

        private void F_Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            udpSocket1.Active = false;
            ClassOptionData OptionData = new ClassOptionData();
            OptionData.ExSQL("Update tb_CurreneyUser Set Sign =12 Where ID >0");
            OptionData.Dispose();
        }

        /// <summary>
        /// 向用户发送信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        private void SendUserMsg(ClassMsg msg, IPAddress Ip, int Port)
        {
            ClassOptionData OptionData = new ClassOptionData();
            DataTable DataReader = OptionData.ExSQLReDr("Select * From CurreneyUser Where ID = " + msg.RID);
            string ip = DataReader.Rows[0]["IP"].ToString();
            int port = Convert.ToInt16(DataReader.Rows[0]["Port"]);
            udpSocket1.Send(IPAddress.Parse(ip), port, new ClassSerializers().SerializeBinary(msg).ToArray());
            OptionData.Dispose();
            DataReader.Dispose();
        }
        /// <summary>
        /// 更新用户在线状态
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        private void UpdateUserState(ClassMsg msg, IPAddress Ip, int Port)
        {

            ClassOptionData OptionData = new ClassOptionData();
            OptionData.ExSQL("Update CurreneyUser Set Sign = " + Convert.ToString((int)(msg.msgCommand)) + " Where ID = " + msg.SID);
            DataTable DataReader = OptionData.ExSQLReDr("Select * From CurreneyUser Where Sign = " + Convert.ToString((int)(MsgCommand.Logined)));
            if (msg.msgCommand == MsgCommand.Close)
                msg.msgID = "Down";
            else if (msg.msgCommand == MsgCommand.Logined)
                msg.msgID = "Up";
            msg.msgCommand = MsgCommand.UpdateState;
            int i = 0;
            while (DataReader.Rows.Count==i)
            {
                udpSocket1.Send(IPAddress.Parse(DataReader.Rows[i]["IP"].ToString()),
                        Convert.ToInt16( DataReader.Rows[i]["Port"]), new ClassSerializers().SerializeBinary(msg).ToArray());
                i++;
            }
            OptionData.Dispose();
            UpdateUser();
        }

        private void 推出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
