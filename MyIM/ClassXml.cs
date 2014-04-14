using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyIM
{
    public class ClassXml
    {
        /// <summary>
        /// 获取xml节点内容
        /// </summary>
        /// <param name="sEleName">节点名称</param>
        /// <param name="sPath">xml路径</param>
        /// <returns></returns>
        public string GetXmlElement(string sEleName, string sPath)
        {
            XmlDocument XmlRead = new XmlDocument();
            string sEleValue = string.Empty;
            XmlRead.Load(sPath);
            XmlNode xNode = XmlRead.SelectSingleNode(sEleName);
            sEleValue = xNode.InnerText;
            return sEleValue;
        }
        /// <summary>
        /// 获取节点属性内容
        /// </summary>
        /// <param name="sEleName">节点名称</param>
        /// <param name="sAttName">属性名称</param>
        /// <param name="sPath">xml路径</param>
        /// <returns></returns>
        public string GetXmlAttribute(string sEleName, string sAttName, string sPath)
        {
            XmlDocument XmlRead = new XmlDocument();
            string sAttValue = string.Empty;
            XmlRead.Load(sPath);
            XmlNode xNode = XmlRead.SelectSingleNode(sAttName);
            sAttValue = xNode.Attributes[sAttName].Value;
            return sAttValue;
        }
        /// <summary>
        /// 获取节点列表
        /// </summary>
        /// <param name="sName">根节点名称</param>
        /// <param name="sPath">xml路径</param>
        /// <returns></returns>
        public XmlNodeList GetNodeList(string sName, string sPath)
        {
            XmlDocument XmlRead = new XmlDocument();
            XmlRead.Load(sPath);
            XmlNodeList xNodeList = XmlRead.SelectNodes(sName);
            return xNodeList;
        }
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="sUserName">用户名称</param>
        /// <param name="sName">根节点名称</param>
        /// <param name="sPath">xml路径</param>
        /// <returns></returns>
        public XmlNode GetNode(string sUserName, string sName, string sPath)
        {
            XmlDocument XmlRead = new XmlDocument();
            XmlNode node1 = null;
            XmlRead.Load(sPath);
            XmlNodeList xNodeList = XmlRead.SelectNodes(sName);
            foreach (XmlNode node in xNodeList)
            {
                if (node.SelectSingleNode("UserName").InnerText == sUserName)
                {
                    node1 = node;
                    break;
                }
            }
            return node1;
        }
        /// <summary>
        /// 创建用户节点
        /// </summary>
        /// <param name="sUserName">用户名</param>
        /// <param name="sPassWord">密码</param>
        /// <param name="sIsRem">是否记住密码</param>
        /// <param name="sAuto">是否自动登录</param>
        /// <param name="sPath">xml路径</param>
        public void CreateUser(string sUserName, string sPassWord, bool sIsRem, bool sAuto, string sPath)
        {
            XmlDocument xmlRead = new XmlDocument();
            xmlRead.Load(sPath);
            XmlNode node = xmlRead.SelectSingleNode("UserList");
            XmlElement xe = xmlRead.CreateElement("UserInfo");
            xe.SetAttribute("IsLast", "true");
            XmlElement xesub = xmlRead.CreateElement("UserName");
            xesub.InnerText = sUserName;
            xe.AppendChild(xesub);
            XmlElement xesub2 = xmlRead.CreateElement("PassWord");
            xesub2.InnerText = sPassWord;
            xe.AppendChild(xesub2);
            XmlElement xesub3 = xmlRead.CreateElement("IsRemPass");
            if (sIsRem)
                xesub3.InnerText = "true";
            else
                xesub3.InnerText = "false";
            xe.AppendChild(xesub3);
            XmlElement xesub4 = xmlRead.CreateElement("IsAutoLogin");
            if (sAuto)
                xesub4.InnerText = "true";
            else
                xesub4.InnerText = "false";
            xe.AppendChild(xesub4);
            node.AppendChild(xe);
            xmlRead.Save(sPath);

        }
        /// <summary>
        /// 修改节点信息
        /// </summary>
        /// <param name="Node">要修改的节点</param>
        /// <param name="sElementName">节点名称</param>
        /// <param name="sElementValue">节点内容</param>
        /// <param name="sName">根节点名称</param>
        /// <param name="sPath">xml路径</param>
        public void ChangeElement(XmlNode Node, string sElementName, string sElementValue, string sName, string sPath)
        {
            XmlDocument xmlRead = new XmlDocument();
            xmlRead.Load(sPath);
            XmlNodeList NodeList = xmlRead.SelectNodes(sName);
            foreach (XmlNode xNode in NodeList)
            {
                if (xNode.InnerText == Node.InnerText)
                {
                    xNode.SelectSingleNode(sElementName).InnerText = sElementValue;
                }
            }
            xmlRead.Save(sPath);

        }
        /// <summary>
        /// 修改节点属性
        /// </summary>
        /// <param name="node">需修改的节点</param>
        /// <param name="sElementName">节点名称</param>
        /// <param name="sName">跟节点名称</param>
        /// <param name="sAtrributeName">属性名称</param>
        /// <param name="sAttributeValue">属性值</param>
        /// <param name="sPath">xml路径</param>
        public void ChangeAttribute(XmlNode node, string sName,string sAtrributeName,string sAttributeValue,string sPath)
        {
            XmlDocument xmlread=new XmlDocument ();
            xmlread.Load(sPath);
            XmlNodeList xNodeList = xmlread.SelectNodes(sName);
            foreach (XmlNode xnode in xNodeList)
            {
                if (xnode.InnerText == node.InnerText)
                {
                    xnode.Attributes[sAtrributeName].Value = sAttributeValue;
                }
            }
            xmlread.Save(sPath);
        }

    }
}
