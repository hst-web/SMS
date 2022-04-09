/*----------------------------------------------------------------

// 文件名：SerializationHelper.cs
// 功能描述：序列化帮助类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace HST.Utillity
{
    public enum SerializationType
    {
        Xml,
        Json,
        DataContract,
        Binary
    }

    [System.Serializable]
    public static class SerializationHelper
    {

        //private SerializationHelper()
        //{
        //}

        #region ========== XmlSerializer ==========
        /// <summary>
        /// 序列化，使用标准的XmlSerializer，优先考虑使用。
        /// 不能序列化IDictionary接口.
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void XmlSerialize(object obj, string filename)
        {
            FileStream fs = null;
            // serialize it...
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// 反序列化，使用标准的XmlSerializer，优先考虑使用。
        /// 不能序列化IDictionary接口.
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns>type类型的对象实例</returns>
        public static object XmlDeserializeFromFile(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public static string XmlSerialize(object obj, System.Text.Encoding ecoding)
        {
            if (obj == null)
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();//制定编码和磁盘文件 
            StreamWriter sWriter = new StreamWriter(stream, ecoding);
            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            //empty namespaces
            xsn.Add(String.Empty, String.Empty);

            ser.Serialize(sWriter, obj, xsn);//序列化 

            string str = ecoding.GetString(stream.ToArray());

            stream.Close();

            return str;

        }

        public static string XmlSerialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            StringWriter sWriter = new StringWriter();
            ser.Serialize(sWriter, obj);
            return sWriter.ToString();
        }

        public static object XmlDeserialize(Type type, string xmlStr)
        {
            if (xmlStr == null || xmlStr.Trim() == "")
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(type);
            StringReader sWriter = new StringReader(xmlStr);
            return ser.Deserialize(sWriter);
        }

        //把接收到的XML转为字典
        public static Dictionary<string, string> XmlModel(string xmlStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            Dictionary<string, string> mo = new Dictionary<string, string>();
            var data = doc.DocumentElement.ChildNodes;
            //.SelectNodes("xml");
            for (int i = 0; i < data.Count; i++)
            {
                mo.Add(data.Item(i).LocalName, data.Item(i).InnerText);
            }
            return mo;
        }

        //从字典中读取指定的值
        public static string ReadModel(string key, Dictionary<string, string> model)
        {
            string str = "";
            model.TryGetValue(key, out str);
            if (str == null)
                str = "";

            return str;
        }

        #endregion

        #region ========== DataContractSerializer ==========
        public static string DataContractSerialize(object o)
        {
            if (o == null)
            {
                return null;
            }
            MemoryStream stream = new MemoryStream();
            DataContractSerializer ser = new DataContractSerializer(o.GetType());
            ser.WriteObject(stream, o);

            string ret = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            return ret;
        }

        public static object DataContractDeserialize(Type type, string xmlStr)
        {
            if (xmlStr == null || xmlStr.Trim() == "")
            {
                return null;
            }
            DataContractSerializer ser = new DataContractSerializer(type);
            MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));//new StringReader(xmlStr);
            return ser.ReadObject(stream);
        }

        #endregion

        #region ========== BinaryBytes ==========
        /// <summary>
        /// 将对象使用二进制格式序列化成byte数组
        /// </summary>
        /// <param name="obj">待保存的对象</param>
        /// <returns>byte数组</returns>
        public static byte[] SaveToBinaryBytes(object obj)
        {
            //将对象序列化到MemoryStream中
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            //从MemoryStream中获取获取byte数组
            return ms.ToArray();
        }

        /// <summary>
        /// 将使用二进制格式保存的byte数组反序列化成对象
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <returns>对象</returns>
        public static object LoadFromBinaryBytes(byte[] bytes)
        {
            object result = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (bytes != null)
            {
                MemoryStream ms = new MemoryStream(bytes);
                result = formatter.Deserialize(ms);
            }
            return result;
        }
        #endregion

        #region ========= other ==========
        /// <summary>
        /// 使用BinaryFormatter将对象系列化到MemoryStream中。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>保存对象的MemoryStream</returns>
        public static MemoryStream SaveToMemoryStream(object obj)
        {
            MemoryStream ms = new MemoryStream();
            BufferedStream stream = new BufferedStream(ms);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            return ms;
        }

        #endregion

    }
}
