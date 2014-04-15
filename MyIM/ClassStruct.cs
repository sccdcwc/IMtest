using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIM
{
    public struct Group
    {
        public string GroupName { get; set; }
        public string GroupID { get; set; }
        public Friend[] Friend{get;set;}
    }
    public struct Friend
    {
        public string NickName { get; set; }
        public string UserID { get; set; }
        public string AlternateName { get; set; }
        public string UserPersonalMessage { get; set; }
        public string HeadPicture { get; set; }
    }

    public struct FriendList
    {
        public Group[] Group { get; set; }
    }
    public class ClassStruct
    {
    }
}
