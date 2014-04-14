using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyIM
{
    public class ClassSerializers
    {
        public ClassSerializers()
        { }
        /// <summary>
        /// 将对象序列化为二进制流
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public MemoryStream SerializeBinary(object request)
        {
 
                BinaryFormatter serializer = new BinaryFormatter();
                MemoryStream memStream = new MemoryStream();      //创建一个内存流存储区
                serializer.Serialize(memStream, request);         //将对象序列化为二进制流
                return memStream;

        }
        /// <summary>
        /// 将二进制流反序列化为对象
        /// </summary>
        /// <param name="memStream"></param>
        /// <returns></returns>
        public object DeSerializeBinary(MemoryStream memStream)
        {
            memStream.Position = 0;
            BinaryFormatter deserializer = new BinaryFormatter();
            object newobj = deserializer.Deserialize(memStream); //将二进制流反序列化为对象
            memStream.Close();                                   //关闭内存流，并释放
            return newobj;
        }
    }
}
