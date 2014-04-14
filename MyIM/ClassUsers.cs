using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIM
{
    [Serializable]
    public class ClassUsers : System.Collections.CollectionBase
    {
        public ClassUsers()
        { }
        /// <summary>
        /// 添加用户到Innerlist列表中
        /// </summary>
        /// <param name="userinfo"></param>
        public void add(ClassUserInfo userinfo)
        {
            base.InnerList.Add(userinfo);
        }
        /// <summary>
        /// 删除Innerlist列表中的用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        public void Romove(ClassUserInfo userInfo)
        {
            base.InnerList.Remove(userInfo);
        }
        /// <summary>
        /// 根据索引号，在列表中查找指定的用户信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ClassUserInfo this[int index]
        {
            get
            {
                return ((ClassUserInfo)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }
    }
}
