using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIM
{
    [Serializable]
    public class ClassUserInfo
    {
        #region 用户编号
        /// <summary>
        /// 用户编号
        /// </summary>
        private string userID;

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        #endregion

        #region 用户正在登录的主机IP
        /// <summary>
        /// 用户正在登录的主机IP
        /// </summary>
        private string userIp;

        public string UserIP
        {
            get { return userIp; }
            set { userIp = value; }
        }
        #endregion

        #region 用户正在登录的主机端口号
        /// <summary>
        /// 用户正在登录的主机端口号
        /// </summary>
        private string userPort;

        public string UserPort
        {
            get { return userPort; }
            set { userPort = value; }
        }
        #endregion

        #region 用户名
        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        #endregion

        #region  当前用户状态
        /// <summary>
        ///  当前用户状态
        /// </summary>
        private string state;

        public string State
        {
            get { return state; }
            set { state = value; }
        }
        #endregion
    }
}
