using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;

namespace TQuery.Net
{
    /// <summary>
    /// 对象序列化操作类
    /// </summary>    
    public class SerializeHelper
    {
        #region 将对象序列化到XML文件中
        /// <summary>
        /// 将对象序列化到XML文件中
        /// </summary>
        /// <typeparam name="T">要序列化的类，即instance的类名</typeparam>
        /// <param name="instance">要序列化的对象</param>
        /// <param name="xmlFile">Xml文件名，表示保存序列化数据的位置</param>
        public static void SerializeXML<T>(object instance, string xmlFile)
        {
            try
            {
                //创建XML序列化对象
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                //创建文件流
                using (FileStream fs = new FileStream(xmlFile, FileMode.Create))
                {
                    //开始序列化对象
                    serializer.Serialize(fs, instance);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 将XML文件反序列化为对象
        /// <summary>
        /// 将XML文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="xmlFile">Xml文件名，即保存序列化数据的位置</param>        
        public static T DeSerializeXML<T>(string xmlFile)
        {
            try
            {
                //创建XML序列化对象
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                //创建文件流
                using (FileStream fs = new FileStream(xmlFile, FileMode.Open))
                {
                    //开始反序列化对象
                    return (T)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 将对象序列化到Soap格式的XML文件中
        /// <summary>
        /// 将对象序列化到Soap格式的XML文件中
        /// </summary>        
        /// <param name="instance">要序列化的对象</param>
        /// <param name="xmlFile">Xml文件名，表示保存序列化数据的位置</param>
        public static void SerializeSoapXML(object instance, string xmlFile)
        {
            try
            {
                //创建Soap序列化对象
                SoapFormatter serializer = new SoapFormatter();

                //创建文件流
                using (FileStream fs = new FileStream(xmlFile, FileMode.Create))
                {
                    //开始序列化对象
                    serializer.Serialize(fs, instance);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 将Soap格式的XML文件反序列化为对象
        /// <summary>
        /// 将Soap格式的XML文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="xmlFile">Xml文件名，即保存序列化数据的位置</param>        
        public static T DeSerializeSoapXML<T>(string xmlFile)
        {
            try
            {
                //创建Soap序列化对象
                SoapFormatter serializer = new SoapFormatter();

                //创建文件流
                using (FileStream fs = new FileStream(xmlFile, FileMode.Open))
                {
                    //开始反序列化对象
                    return (T)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 将对象序列化到二进制文件中
        /// <summary>
        /// 将对象序列化到二进制文件中
        /// </summary>
        /// <param name="instance">要序列化的对象</param>
        /// <param name="fileName">文件名，保存二进制序列化数据的位置,后缀一般为.bin</param>
        public static void SerializeBinary(object instance, string fileName)
        {
            try
            {
                //创建二进制序列化对象
                BinaryFormatter serializer = new BinaryFormatter();

                //创建文件流
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    //开始序列化对象
                    serializer.Serialize(fs, instance);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 将二进制文件反序列化为对象
        /// <summary>
        /// 将二进制文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="fileName">文件名，保存二进制序列化数据的位置</param>        
        public static T DeSerializeBinary<T>(string fileName)
        {
            try
            {
                //创建二进制序列化对象
                BinaryFormatter serializer = new BinaryFormatter();

                //创建文件流
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    //开始反序列化对象
                    return (T)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 将对象序列化到字节数组中
        /// <summary>
        /// 将对象序列化到字节数组中
        /// </summary>
        /// <param name="instance">要序列化的对象</param>        
        public static byte[] SerializeBytes(object instance)
        {
            try
            {
                //序列化后的字节数组
                byte[] buffer;

                //创建二进制序列化对象
                BinaryFormatter serializer = new BinaryFormatter();

                //创建一个内存流
                using (MemoryStream ms = new MemoryStream())
                {
                    //将对象序列化到内存流中
                    serializer.Serialize(ms, instance);

                    //重置内存流的当前位置
                    ms.Position = 0;

                    //初始化缓冲区
                    buffer = new byte[ms.Length];

                    //读取内存流数据到缓冲区中
                    ms.Read(buffer, 0, buffer.Length);

                    return buffer;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 将字节数组反序列化为对象
        /// <summary>
        /// 将字节数组反序列化为对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="buffer">要反序列化的字节数组</param>        
        public static T DeSerializeBytes<T>(byte[] buffer)
        {
            try
            {
                //创建二进制序列化对象
                BinaryFormatter serializer = new BinaryFormatter();

                //创建一个内存流
                using (MemoryStream ms = new MemoryStream())
                {
                    //将缓冲区数据写入内存流
                    ms.Write(buffer, 0, buffer.Length);

                    //重置内存流的当前位置
                    ms.Position = 0;

                    //开始反序列化对象
                    return (T)serializer.Deserialize(ms);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
