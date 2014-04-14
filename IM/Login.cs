using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IM.BLL;
using System.IO;
using System.Runtime.InteropServices;
using MyIM;
using System.Xml;
using System.Net;

namespace IM
{
    public partial class Login : Form
    {
        private bool bSuccess = false;
        public string[] str = { "", "", "", "" };
        //string s = "C://Users//Administrator//Documents//IM_Documents";
        bool IsChange = false;
        string sFirstName = string.Empty;
        string xmlpath = "C://Users//Administrator//Documents//IM_Documents//IMinfo.xml";
        Publec_Class pubec_class = new Publec_Class();
        ClassXml xml = new ClassXml();

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public Login()
        {
            InitializeComponent();
            udpSocket1.DataArrival += new UDPSocket.DataArrivalEventHandler(this.DataArrival);
        }
        #region 监听
        private void socketUDP1_DataArrival(byte[] Data, IPAddress ip, int port)
        {
            DataArrivaldelegate outdelegate = new DataArrivaldelegate(DataArrival);
            this.BeginInvoke(outdelegate, new object[] { Data, ip, port });
        }
        private delegate void DataArrivaldelegate(byte[] Data, System.Net.IPAddress Ip, int Port);

        private void DataArrival(byte[] Data, IPAddress ip, int port)
        {
            ClassMsg msg = new ClassSerializers().DeSerializeBinary((new MemoryStream(Data))) as ClassMsg;
            switch (msg.sendKind)
            {
                case SendKind.SendCommand:
                    {
                        switch (msg.msgCommand)
                        {
                            case MsgCommand.Logining:
                                MessageBox.Show("登录失败，用户名密码错误!");
                                break;
                            case MsgCommand.Logined:
                                main mai = new main();
                                mai.Show();
                                this.Hide();
                                break;
                        }

                        break;
                    }

            }
        }
        #endregion
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regist_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            regist re = new regist();
            re.Show();
            regist_link.LinkVisited = true;
        }

        private void Login_Load(object sender, EventArgs e)
        {

            if (File.Exists(xmlpath))
            {
                string IsRemPass = string.Empty;
                XmlNodeList xNodeList = xml.GetNodeList("UserList/UserInfo", xmlpath);
                foreach (XmlNode node in xNodeList)
                {
                    if (node.Attributes[0].Value == "true")
                    {
                        Username_textbox.Text = node.SelectSingleNode("UserName").InnerText;
                        sFirstName = Username_textbox.Text;
                        //if (node.SelectSingleNode("IsRemPass").InnerText == "true")
                        //{
                        //    Password_textbox.Text = node.SelectSingleNode("PassWord").InnerText;
                        //    remain_password.Checked = true;
                        //}
                        if (node.SelectSingleNode("IsAutoLogin").InnerText == "true")
                        {
                            Password_textbox.Text = node.SelectSingleNode("PassWord").InnerText;
                            login_();
                        }
                        //xml.ChangeAttribute(node, "UserList/UserInfo", "IsLast", "false", xmlpath);
                        break;
                    }
                }
            }
            else
            {
                XmlWriterSettings setting = new XmlWriterSettings();
                setting.Indent = true;
                setting.NewLineOnAttributes = true;
                XmlWriter xmlwrite = XmlWriter.Create(xmlpath, setting);
                xmlwrite.Close();
            }
            IsChange = true;

            /*
            DirectoryInfo Doc = new DirectoryInfo(s);
            if (!Doc.Exists)
            {
                Directory.CreateDirectory(s);
            }
            FileInfo file = new FileInfo(s + "//info.txt");
            if (!file.Exists)
            {
                FileStream f = new FileStream(s + "//info.txt", FileMode.Create);
            }

            StreamReader re = new StreamReader(s + "//info.txt");



            for (int i = 0; i < 4; i++)
            {
                string sr = re.ReadLine();
                if (sr != null)
                {
                    str[i] = sr;
                }
            }
            re.Close();
            if (str[3] == "")
                str[3] = "0";

            if (str[2] == "1" && str[3] == "0")
            {
                Username_textbox.Text = str[0];
                Password_textbox.Text = str[1];
                remain_password.Checked = true;
                autologin.Checked = false;
            }
            if (str[2] == "0" && str[3] == "0")
            {
                Username_textbox.Text = "";
                Password_textbox.Text = "";
                remain_password.Checked = false;
                autologin.Checked = false;
            }
            if (str[3] == "1")
            {
                Username_textbox.Text = str[0];
                Password_textbox.Text = str[1];
                remain_password.Checked = true;
                autologin.Checked = true;
                login_();
            }*/
        }

        private void f_load(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xml.ChangeAttribute(xml.GetNode(Username_textbox.Text, "UserList/UserInfo", xmlpath), "UserList/UserInfo", "IsLast", "true", xmlpath);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool bSuccess = false;
            XmlNodeList NodeList = xml.GetNodeList("UserList/UserInfo", xmlpath);
            foreach (XmlNode node in NodeList)
            {
                if (node.SelectSingleNode("UserName").InnerText == Username_textbox.Text)
                {
                    bSuccess = true;
                }
            }
            if (!bSuccess)
                xml.CreateUser(Username_textbox.Text, Password_textbox.Text, remain_password.Checked, autologin.Checked, xmlpath);
            xml.ChangeAttribute(xml.GetNode(sFirstName, "UserList/UserInfo", xmlpath), "UserList/UserInfo", "IsLast", "true", xmlpath);
            xml.ChangeAttribute(xml.GetNode(Username_textbox.Text, "UserList/UserInfo", xmlpath), "UserList/UserInfo", "IsLast", "true", xmlpath);
            login_();
            /*
            StreamReader re1 = new StreamReader(s + "//info.txt");
            string reName = re1.ReadLine();
            re1.Close();
            if (reName != Username_textbox.Text)
            {
                string[] sArray = { "", "", "", "" };
                if (remain_password.Checked == true)
                {
                    StreamWriter wr = new StreamWriter(s + "//info.txt");
                    wr.WriteLine(Username_textbox.Text);
                    wr.WriteLine(Password_textbox.Text);
                    wr.WriteLine("1");
                    wr.Close();
                }
                else
                {
                    StreamReader re = new StreamReader(s + "//info.txt");
                    sArray[0] = re.ReadLine();
                    sArray[1] = re.ReadLine();
                    sArray[2] = re.ReadLine();
                    sArray[2] = sArray[2].Replace("1", "0");
                    re.Close();
                    File.WriteAllLines(s + "//info.txt", sArray);
                }
            }
             */
        }
        IM.main ma = new main();

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ma != null)
            {
                ma.Show();
            }
            else
            {
                this.Show();
            }
        }

        private void login_title_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 登录函数
        /// </summary>
        private void login_()
        {
            string hostname = string.Empty;
            string serID = string.Empty;
            UserBLL User = new UserBLL();
            User.user.UserName = Username_textbox.Text;
            User.user.PassWord1 = Password_textbox.Text;
            //string[] sMessage = { "", "" };
            //bSuccess = User.Login(User.user.UserName, User.user.PassWord1, ref sMessage);
            //if (bSuccess)
            //{
            #region 发送登录消息
            //向服务器发送登录消息
            hostname = Dns.GetHostName();
            //IPAddress[] ip = Dns.GetHostAddresses(hostname);
            LoginMsg loginmsg = new LoginMsg();
            //RegisterMsg registmsg = new RegisterMsg();
            ClassMsg msg = new ClassMsg();
            //registmsg.UserName = Username_textbox.Text;
            //registmsg.PassWord = Password_textbox.Text;
            loginmsg.UserName = Username_textbox.Text;
            loginmsg.PassWord = Password_textbox.Text;
            byte[] logindata = new ClassSerializers().SerializeBinary(loginmsg).ToArray();
            msg.sendKind = SendKind.SendCommand;
            msg.msgCommand = MsgCommand.Logining;
            msg.Data = logindata;
            serID = "192.168.1.187";                 //服务器IP
            udpSocket1.Send(IPAddress.Parse(serID), 11000, new ClassSerializers().SerializeBinary(msg).ToArray());
            #endregion

            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(sMessage[0]))
            //        MessageBox.Show(sMessage[0]);
            //    else
            //        MessageBox.Show(sMessage[1]);
            //}
        }

        private void remain_password_CheckedChanged(object sender, EventArgs e)
        {
            XmlNode node = xml.GetNode(Username_textbox.Text, "UserList/UserInfo", xmlpath);
            if (node != null)
            {
                if (node.SelectSingleNode("IsRemPass").InnerText == "false" && IsChange)
                {
                    xml.ChangeElement(node, "PassWord", Password_textbox.Text, "UserList/UserInfo", xmlpath);
                    xml.ChangeElement(node, "IsRemPass", "true", "UserList/UserInfo", xmlpath);
                }
                else
                    if (remain_password.Checked == false && IsChange)
                        xml.ChangeElement(node, "IsRemPass", "false", "UserList/UserInfo", xmlpath);
            }
            /*
            string[] sArray = { "", "", "", "" };
            if (remain_password.Checked == true)
            {
                StreamWriter wr = new StreamWriter(s + "//info.txt");
                wr.WriteLine(Username_textbox.Text);
                wr.WriteLine(Password_textbox.Text);
                wr.WriteLine("1");
                wr.Close();
            }
            else
            {
                StreamReader re = new StreamReader(s + "//info.txt");
                sArray[0] = re.ReadLine();
                sArray[1] = re.ReadLine();
                sArray[2] = re.ReadLine();
                sArray[2] = sArray[2].Replace("1", "0");
                re.Close();
                File.WriteAllLines(s + "//info.txt", sArray);
            }*/
        }

        private void autologin_CheckedChanged(object sender, EventArgs e)
        {
            XmlNode Node = xml.GetNode(Username_textbox.Text, "UserList/UserInfo", xmlpath);
            if (Node.SelectSingleNode("IsAutoLogin").InnerText == "true" && IsChange)
                xml.ChangeElement(Node, "IsAutoLogin", "false", "UserList/UserInfo", xmlpath);
            else
                if (IsChange)
                    xml.ChangeElement(Node, "IsAutoLogin", "true", "UserList/UserInfo", xmlpath);

            /*
            string[] sArray = { "", "", "", "" };
            StreamReader re = new StreamReader(s + "//info.txt");
            for (int i = 0; i < 4; i++)
            {
                sArray[i] = re.ReadLine();
            }
            re.Close();
            if (autologin.Checked == true)
            {
                sArray[3] = "1";
            }
            else
            {
                sArray[3] = "0";
            }
            File.WriteAllLines(s + "//info.txt", sArray);
            */
        }

        private void Username_textbox_TextChanged(object sender, EventArgs e)
        {
            Password_textbox.Clear();
            string IsRemPass = string.Empty;
            IsChange = false;
            XmlNodeList xNodeList = xml.GetNodeList("UserList/UserInfo", xmlpath);
            foreach (XmlNode node in xNodeList)
            {
                if (Username_textbox.Text == node.SelectSingleNode("UserName").InnerText)
                {
                    if (node.SelectSingleNode("IsRemPass").InnerText == "true")
                    {
                        Password_textbox.Text = node.SelectSingleNode("PassWord").InnerText;
                        remain_password.Checked = true;
                    }
                    else
                        remain_password.Checked = false;
                    if (node.SelectSingleNode("IsAutoLogin").InnerText == "true")
                    {
                        Password_textbox.Text = node.SelectSingleNode("PassWord").InnerText;
                    }
                    //xml.ChangeAttribute(node, "UserList/UserInfo", "IsLast", "false", xmlpath);
                    break;
                }
            }
            IsChange = true;
        }

        private void Login_Leave(object sender, EventArgs e)
        {
            xml.ChangeAttribute(xml.GetNode(Username_textbox.Text, "UserList/UserInfo", xmlpath), "UserList/UserInfo", "IsLast", "true", xmlpath);
        }
    }
}
