using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Xml;
using FMOP.XHelp;
using FMOP.DB;
using FMOP.SD;
using System.IO;
/// <summary>
/// collectionNodeObject
/// 定义主表与子表所需要的参数与参数相应的属性
/// </summary>
namespace FMOP.CollectionNode
{
    public class CollectionNodeObject
    {
        private string strXml = string.Empty;
        private string xmlAssert = "<?xml version=\"1.0\" encoding=\"GB2312\"?>";

        /// <summary>
        /// 主表的Node个数
        /// </summary>
        private int masterParamNo = 0;

        /// <summary>
        /// 主表的Node个数
        /// </summary>
        public int MasterParamNo
        {
            get
            {
                return masterParamNo;
            }
            set
            {
                masterParamNo = value;
            }
        }

        /// <summary>
        /// 从表的Node个数
        /// </summary>
        private int subParamNo = 0;

        /// <summary>
        /// 从表的Node个数
        /// </summary>
        public int SubParamNo
        {
            get
            {
                return subParamNo;
            }
            set
            {
                subParamNo = value;
            }
        }

        /// <summary>
        /// 上传控件的个数
        /// </summary>
        private int fileParamNo = 0;

        /// <summary>
        /// 上传控件的个数
        /// </summary>
        public int FileParamNo
        {
            set
            {
                fileParamNo = value;
            }
            get
            {
                return fileParamNo;
            }
        }

        /// <summary>
        /// XMLDocument 对象
        /// </summary>
        private XmlDocument xmlDoc = new XmlDocument();

        /// <summary>
        /// 默认的构造函数
        /// </summary>
        public CollectionNodeObject()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 重载构造函数，查询指定模块的XML配置文件
        /// </summary>
        /// <param name="Module"></param>
        public CollectionNodeObject(string Module)
        {
            string sqlCmd = string.Empty;
            sqlCmd = "SELECT Configuration.query('/WorkFlowModule/Data') from system_Modules where name ='" + Module + "'";
            strXml = DbHelperSQL.GetSingle(sqlCmd).ToString();
        }

        /// <summary>
        /// 把XML中Data-Filed中的所有控件节点存放到对象中
        /// </summary>
        /// <returns>返回包括SaveDepositary对象的列表</returns>
        public SaveDepositary[] GetMasterCollection()
        {
            int No = 0;
            XmlNodeList MasterList = null;

            Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlAssert + strXml));
            xmlDoc.Load(stream);
            MasterList = xmlDoc.SelectNodes("//Data-Field");
            SaveDepositary[] MasterArray = new SaveDepositary[MasterParamNo];
            foreach (XmlNode node in MasterList)
            {
                if (node.SelectSingleNode("type").InnerXml != "SubTable" && node.SelectSingleNode("type").InnerXml != "AddOnFiles")
                {
                    MasterArray[No] = new SaveDepositary();
                    MasterArray[No].Name = XMLHelper.GetSingleString(node, "name");
                    MasterArray[No].CanNull = XMLHelper.GetSingleBool(node, "canNull");
                    MasterArray[No].Type = XMLHelper.GetSingleString(node, "type");
                    MasterArray[No].Length = XMLHelper.GetSingleInt(node, "length", 50);
                    No++;
                }
            }
            if (No != masterParamNo)
            {
                throw new Exception("创建主表控件数组出错了!");
            }
            return MasterArray;
        }

        /// <summary>
        /// 把XML中Data-Filed中的所有控件节点存放到对象中
        /// </summary>
        /// <returns>返回包括SaveDepositary对象的列表</returns>
        public SaveDepositary[] GetSubrCollection()
        {
            XmlNodeList subNode = null;
            int Total = 0;

            //子表名
            string subTable = string.Empty;
            Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlAssert + strXml));
            xmlDoc.Load(stream);

            subNode = xmlDoc.SelectNodes("//Data/Data-Field");
            SaveDepositary[] subArray = new SaveDepositary[SubParamNo];

            foreach (XmlNode node in subNode)
            {
                if (node.SelectSingleNode("type").InnerXml == "SubTable")
                {
                    subTable = XMLHelper.GetSingleString(node, "name");

                    XmlNodeList subList = node.SelectNodes("subTable/Sub-Field");

                    foreach (XmlNode nd in subList)
                    {
                        subArray[Total] = new SaveDepositary();
                        subArray[Total].Name = XMLHelper.GetSingleString(nd, "name");
                        subArray[Total].CanNull = XMLHelper.GetSingleBool(nd, "canNull");
                        subArray[Total].Type = XMLHelper.GetSingleString(nd, "type");
                        subArray[Total].Length = XMLHelper.GetSingleInt(nd, "length", 50);
                        subArray[Total].SubTable = subTable;
                        Total++;
                    }
                }
            }

            if (Total != SubParamNo)
            {
                throw new Exception("创建子表数组出错!");
            }
            return subArray;
        }

        /// <summary>
        /// 把XML中属于上传文件的类型集中在一起,生成对象
        /// </summary>
        /// <returns></returns>
        public SaveDepositary[] GetFileCollection()
        {
            XmlNodeList FileList = null;
            int Total = 0;

            Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlAssert + strXml));
            xmlDoc.Load(stream);

            //根据类型生成不同的结点集合
            FileList = xmlDoc.SelectNodes("//Data/Data-Field");

            //创建子表应获取多少个控件的值
            SaveDepositary[] FileArray = new SaveDepositary[FileParamNo];

            //获取控件的名称，状态等属性
            foreach (XmlNode node in FileList)
            {
                if ( node["type"] != null &&  node["type"].InnerXml == "AddOnFiles")
                {
                    FileArray[Total] = new SaveDepositary();
                    FileArray[Total].Name = XMLHelper.GetSingleString(node, "name");
                    FileArray[Total].CanNull = XMLHelper.GetSingleBool(node, "canNull");
                    FileArray[Total].Type = XMLHelper.GetSingleString(node, "type");
                    FileArray[Total].Length = XMLHelper.GetSingleInt(node, "length", 50);
                    Total++;
                }
            }
            if (Total != fileParamNo)
            {
                throw new Exception("创建上传文件的数组出错了!");
            }
            return FileArray;
        }
        /// <summary>
        /// 返回主表,子表,上传控件的Node个数
        /// </summary>
        /// <returns></returns>
        public void GetNodeNo()
        {
            Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlAssert + strXml));
           // int Total = 0;
            xmlDoc.Load(stream);

            //根据类型生成不同的结点集合
            //统计子表的节点个数
            SubParamNo = xmlDoc.SelectNodes("//Data-Field/subTable").Count;

            //统计主表节点的个数
            foreach(XmlNode xn in xmlDoc.SelectNodes("//Data-Field"))
            {
                if (xn["type"] != null && xn["type"].InnerXml != "AddOnFiles" && xn["type"].InnerXml != "SubTable")
                {
                    MasterParamNo = MasterParamNo + 1;
                }

                //统计上传文件的个数
                if (xn["type"] != null && xn["type"].InnerXml == "AddOnFiles")
                {
                    FileParamNo = FileParamNo + 1;
                }
            }

            //统计子表节点的个数
            SubParamNo = xmlDoc.SelectNodes("//Data-Field/subTable/Sub-Field").Count;
        }
    }
}
