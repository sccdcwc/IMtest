using System;


namespace MyIM
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ClassMsg
    {
        public String SID = "";//发送方编号
        public String SIP = "";//发送方IP
        public String SPort = "";//发送方端口号

        public String RID = "";//接收方编号
        public String RIP = "";//接收方IP
        public String RPort = "";//接收方端口号

        public SendKind sendKind = SendKind.SendNone;//发送消息类型，默认为无类型
        public MsgCommand msgCommand = MsgCommand.None;//消息命令
        public SendState sendState = SendState.None;//消息发送状态
        public string msgID = "";
        public byte[] Data;
    }
    #region 发送类型
    /// <summary>
    /// 发送类型
    /// </summary>
    public enum SendKind         //定义枚举类型
    {
        /// <summary>
        /// 无类型
        /// </summary>
        SendNone,
        /// <summary>
        /// 发送命令
        /// </summary>
        SendCommand,
        /// <summary>
        /// 发送消息
        /// </summary>
        SendMsg,
        /// <summary>
        /// 发送文件
        /// </summary>
        SendFile
    }
    #endregion

    #region 消息命令
    /// <summary>
    /// 消息命令
    /// </summary>
    public enum MsgCommand
    {
        /// <summary>
        /// 无命令
        /// </summary>
        None,

        /// <summary>
        /// 用户注册
        /// </summary>
        Registering,

        /// <summary>
        /// 用户注册结束
        /// </summary>
        Registered,

        /// <summary>
        /// 用户登录
        /// </summary>
        Logining,

        /// <summary>
        /// 用户登录结束，上线
        /// </summary>
        Logined,

        /// <summary>
        /// 发送单用户
        /// </summary>
        SendToOne,

        /// <summary>
        /// 发送所有用户
        /// </summary>
        SendToAll,

        /// <summary>
        /// 用户列表
        /// </summary>
        UserList,

        /// <summary>
        /// 更新用户状态
        /// </summary>
        UpdateState,

        /// <summary>
        /// 打开视频
        /// </summary>
        VideoOpen,

        /// <summary>
        /// 正在视频
        /// </summary>
        Videoing,

        /// <summary>
        /// 关闭视频
        /// </summary>
        VideoClose,

        /// <summary>
        /// 下线
        /// </summary>
        Close
    }
    #endregion

    #region 发送状态
    /// <summary>
    /// 发送状态
    /// </summary>
    public enum SendState
    {
        /// <summary>
        /// 无状态
        /// </summary>
        None,
        /// <summary>
        /// 单消息或文件
        /// </summary>
        Single,
        /// <summary>
        /// 发送开始生成的文件
        /// </summary>
        Start,
        /// <summary>
        /// 正在发送中，写入文件
        /// </summary>
        Sending,
        /// <summary>
        /// 发送结束
        /// </summary>
        End
    }
    #endregion

    #region 用户注册信息
    /// <summary>
    /// 用户注册信息
    /// </summary>
    [Serializable]
    public class RegisterMsg
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName;
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email;
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone;
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender;
        /// <summary>
        /// 学生或老师
        /// </summary>
        public string SorT;
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName;
    }
    #endregion

    #region 登录反馈信息
    [Serializable]
    public class LoginMsg
    {
        public string UserName;
        public string PassWord;
        public bool IsSuccess = false;
        public bool HasUser = false;
        public bool IsPassRight = false;
    }
    #endregion
}
