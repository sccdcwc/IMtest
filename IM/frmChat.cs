using System;
using System.Data;
using System.Windows.Forms;
using IM.BLL;
using System.Net;
using MyIM;
using System.Text;
using System.IO;

namespace IM
{
    public partial class frmChat1 : Form
    {
        public int ChatUserID;
        string hostname;
        string serID;
        public string s = "C://Users//Administrator//Documents//IM_Documents";
        public frmChat1()
        {
            InitializeComponent();
            udpSocket1.Active = true;
        }

        private void toolStripButtonWenJian_Click(object sender, EventArgs e)
        {
            sendfile sendf = new sendfile();
            sendf.Show();
        }

        private void toolStripButtonLiaoTianJiLu_Click(object sender, EventArgs e)
        {
            frmClassroom frmc = new frmClassroom();
            frmc.Show();
        }

        private void frmChat_Load(object sender, EventArgs e)
        {
            UserBLL udb = new UserBLL();
            //聊天对象信息表
            DataTable dt = new DataTable();
            //用户好友列表
            DataTable dt1 = new DataTable();
            //用户信息表
            DataTable dt2 = new DataTable();
            dt = udb.Userinfo(ChatUserID);
            dt1 = udb.GetFriendShipInfo(udb.user.UserID.ToString(), ChatUserID.ToString());
            dt2 = udb.Userinfo(udb.user.UserID);
            Text = "与" + dt1.Rows[0]["AlternateName"] + "聊天中";
            FriendName.Text = dt1.Rows[0]["AlternateName"].ToString();
            UserName.Text = dt2.Rows[0]["UserNickName"].ToString();
            if (string.IsNullOrEmpty(dt.Rows[0]["HeadPicture"].ToString()))
            {
                FriendHead.ImageLocation = s + "\\search_teacher.jpg";
            }
            else
            {
                FriendHead.ImageLocation = dt.Rows[0]["HeadPicture"].ToString();
            }
            if (string.IsNullOrEmpty(dt2.Rows[0]["HeadPicture"].ToString()))
            {
                UserHead.ImageLocation = s + "\\search_teacher.jpg";
            }
            else
            {
                UserHead.ImageLocation = dt2.Rows[0]["HeadPicture"].ToString();
            }
            hostname = Dns.GetHostName();
            IPAddress[] ip = Dns.GetHostAddresses(hostname);
            RegisterMsg registmsg = new RegisterMsg();
            ClassMsg msg = new ClassMsg();
            registmsg.UserName = "cwc";
            registmsg.PassWord = "123";
            byte[] registdata = new ClassSerializers().SerializeBinary(registmsg).ToArray();
            msg.sendKind = SendKind.SendCommand;
            msg.msgCommand = MsgCommand.Registering;
            msg.Data = registdata;
            serID = "192.168.1.187";
            udpSocket1.Send(IPAddress.Parse(serID), 11000, new ClassSerializers().SerializeBinary(msg).ToArray());
        }

        private void toolStripButtonZiTi_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == fontDialog1.ShowDialog())
            {
                this.MessageTextBox.Font=fontDialog1.Font;
                this.MessageTextBox.ForeColor = fontDialog1.Color;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse("192.168.1.187");//服务器端的IP地址
            string port = "8081";//端口号
            //string revid = ((this.Tag as TreeNode).Tag as ClassUserInfo).UserID;//接收ID号
            string revid = "2";
            //string sid = Publec_Class.UserID;//发送ID
            string sid = "1";
            string msgid = Guid.NewGuid().ToString();//设置全局惟一标识

            byte[] data = Encoding.Unicode.GetBytes(MessageTextBox.Rtf);//将当前要发送的信息转换成二进制流
            ClassMsg msg = new ClassMsg();
            msg.sendKind = SendKind.SendMsg;//发送的消息
            msg.msgCommand = MsgCommand.SendToOne;//发送的是单用户信息
            msg.SID = sid;//发送ID
            msg.RID = revid;//接收ID
            msg.Data = data;//发送的信息
            msg.msgID = msgid;

            if (data.Length <= 1024)//如果发送信息的长度小于等于1024
            {
                msg.sendState = SendState.Single;
                //将信息直接发送给远程客户端
                //udpsocket.Send(ip, Convert.ToInt32(port), new ClassSerializers().SerializeBinary(msg).ToArray());
            }
            else
            {
                ClassMsg start = new ClassMsg();
                start.sendKind = SendKind.SendMsg;
                start.sendState = SendState.Start;//文件发送开始命令
                start.msgCommand = MsgCommand.SendToOne;//发送单用户命令
                start.SID = sid;
                start.RID = revid;
                start.Data = Encoding.Unicode.GetBytes("");
                start.msgID = msgid;
                udpSocket1.Send(ip, Convert.ToInt32(port), new ClassSerializers().SerializeBinary(start).ToArray());
                MemoryStream stream = new MemoryStream(data);//将二进制流存储到内存流中
                int sendlen = 1024; //设置文件每块发送的长度
                long sunlen = (stream.Length);//整个文件的大小
                int offset = 0;//设置文件发送的起始位置
                while (sunlen > 0)  //分流发送
                {
                    sendlen = 1024;
                    if (sunlen <= sendlen)
                        sendlen = Convert.ToInt32(sunlen);
                    byte[] msgdata = new byte[sendlen];
                    stream.Read(msgdata, offset, sendlen);//读取要发送的字节块
                    msg.sendState = SendState.Sending;//发送状态为文件发送中
                    msg.Data = msgdata;
                    udpSocket1.Send(ip, Convert.ToInt32(port), new ClassSerializers().SerializeBinary(msg).ToArray());//发送当前块的信息
                    sunlen = sunlen - sendlen;//记录下一块的起始位置
                }
                ClassMsg end = new ClassMsg();
                end.sendKind = SendKind.SendMsg;
                end.sendState = SendState.End;//文件发送结束命令
                end.msgCommand = MsgCommand.SendToOne;
                end.SID = sid;
                end.RID = revid;
                end.Data = Encoding.Unicode.GetBytes("");
                end.msgID = msgid;
                udpSocket1.Send(ip, Convert.ToInt32(port), new ClassSerializers().SerializeBinary(end).ToArray());//发送信息，通知文件发送结束



                string dir = System.Environment.CurrentDirectory;
                dir = dir.Substring(0, dir.Length - 9);
                //pictureBox1.Load(dir + @"Image\QQ12.jpg");
            }

            richTextBoxRMsg.SelectionStart = 0;//将文本的起始点设为0
            richTextBoxRMsg.AppendText("cwc");//将当前用户名添加到文本框中
            richTextBoxRMsg.AppendText("  " + DateTime.Now.ToString());//将当前发送的时间添加到文本框中
            richTextBoxRMsg.AppendText("\r\n");//换行回车
            richTextBoxRMsg.SelectedRtf = MessageTextBox.Rtf;//将发送信息添加到接收文本框中
            MessageTextBox.Clear();//清空发送文本框
        }

    }
}
