using System;
using System.Windows.Forms;

namespace MyIM
{
    /// <summary>
    /// 获取窗体在InnerList列表中进行添加和移除操作，并可以通过指定的索引号，在列表中查找相应的窗体
    /// </summary>
   public class ClassForms:System.Collections.CollectionBase
    {
        public ClassForms()
        {

        }
        /// <summary>
        /// 将接收的窗体添加到列表中
        /// </summary>
        /// <param name="f"></param>
        public void add(Form f)
        {
            base.InnerList.Add(f);
        }
        /// <summary>
        /// 在列表中移除指定的窗口
        /// </summary>
        /// <param name="f"></param>
        public void Remove(Form f)
        {
            base.InnerList.Remove(f);
        }
        /// <summary>
        /// 通过指定索引号找到指定窗口
        /// </summary>
        /// <param name="index">索引号</param>
        /// <returns></returns>
        public Form this[int index]
        {
            get { return (Form)List[index]; }
            set { List[index]=value;}
        }
    }
}
