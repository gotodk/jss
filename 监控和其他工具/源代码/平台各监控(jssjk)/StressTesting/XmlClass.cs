using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace StressTesting
{
    class XmlClass : XmlDocument
    {
       
        string filePath = "";
        public XmlClass(string filePath)
        {
            this.filePath = filePath;
            this.Load(filePath);
        }
        /// <summary>
        /// 得到节点列表
        /// </summary>
        /// <param name="NodeName">节点名</param>
        /// <returns></returns>
        public XmlNodeList GetNodeList(string NodeName)
        {
            XmlNodeList nList = this.SelectSingleNode(NodeName).ChildNodes;
            return nList;
        }
        /// <summary>
        /// 返回指定节点的值
        /// </summary>
        /// <param name="NodeName"></param>
        /// <returns></returns>
        public string GetNodeValue(string NodeName)
        {
            XmlNode xNode = this.SelectSingleNode(NodeName);
            return xNode.InnerText;
        }
        public void Savexml()
        {
            this.Save(this.filePath);
        }
        /// <summary>
        /// 创建配置文件
        /// </summary>
        /// <param name="path">要保存的路径</param>
        public static void CreateXmlFile(string path)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement appsetting = doc.CreateElement("Appsetting");
           
            XmlElement connectString = doc.CreateElement("ConnectString");
            XmlElement serverName = doc.CreateElement("ServerName");
            serverName.InnerText = "";
            XmlElement dbName = doc.CreateElement("DbName");
            XmlElement userName = doc.CreateElement("UserName");
            XmlElement password = doc.CreateElement("Password");
            connectString.AppendChild(serverName);
            connectString.AppendChild(dbName);
            connectString.AppendChild(userName);
            connectString.AppendChild(password);
            appsetting.AppendChild(connectString);

            XmlElement times = doc.CreateElement("Times");

            appsetting.AppendChild(times);
            doc.AppendChild(appsetting);

            doc.Save(path);
        }
    }
}
