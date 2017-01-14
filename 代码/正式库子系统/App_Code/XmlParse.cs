using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections;
using FMOP.DB;
using FMOP.XHelp;
using Hesion.Brick.Core;
/// <summary>
/// xmlParse 的摘要说明
/// </summary>
namespace FMOP.XParse
{
    /// <summary>
    /// 张伟
    /// 2007.11.29
    /// </summary>
    public class xmlParse
    {
        /// <summary>
        /// XML文档声明部分
        /// </summary>
        private static string xmlAssert = "<?xml version=\"1.0\" encoding=\"GB2312\"?>";
	    public xmlParse()
	    {
		    //
		    // TODO: 在此处添加构造函数逻辑
		    //
        }
        
        #region 解析XML
        /// <summary>
        /// 解析指定模块XML，并返回XML集合列表
        /// </summary>
        /// <param name="ModuleXml">模块XML文件内容</param>
        /// <returns>指定XML节点列表</returns>
        public static XmlNodeList XmlParseModule(string ModuleXml,string Node)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlist =null;
            try
            {
                Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlAssert + ModuleXml));
                xmlDoc.Load(stream);
                xmlist = xmlDoc.SelectNodes(Node);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xmlist;
        }

        /// <summary>
        /// 返回查找指定节点的第一个
        /// </summary>
        /// <param name="moduleXml">模块XML内容</param>
        /// <param name="Node">要查询的节点</param>
        /// <returns></returns>
        public static XmlNode XmlFirstNode(string moduleXml, string Node)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode xmlist = null;
            try
            {
                Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlAssert + moduleXml));
                xmlDoc.Load(stream);
                xmlist = xmlDoc.SelectSingleNode(Node);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xmlist;
        }

        /// <summary>
        /// 根据模块名，要查询的节点名称，返回值
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="node">节点名</param>
        /// <returns></returns>
        public static string XmlGetNodeText(string module,string node)
        {
            string moduleXml = "";
            string sqlCmd = string.Empty;
            string returnValue = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode xmlNode = null;
            try
            {
                sqlCmd = "SELECT Configuration FROM system_Modules WHERE name ='" + module + "'";
                moduleXml = DbHelperSQL.GetSingle(sqlCmd).ToString();
                Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlAssert + moduleXml));
                xmlDoc.Load(stream);
                switch (node)
                {
                    case "Add":
                        xmlNode = xmlDoc.SelectSingleNode("//WorkFlowModule/Ridirect_AfterAdd");
                        break;
                    case "Delete":
                        xmlNode = xmlDoc.SelectSingleNode("//WorkFlowModule/Ridirect_AfterDelete");
                        break;
                    case "Update":
                        xmlNode = xmlDoc.SelectSingleNode("//WorkFlowModule/Ridirect_AfterModify");
                        break;
                }

                if (xmlNode != null)
                {
                    returnValue = xmlNode.InnerXml;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


        /// <summary>
        /// 取节点集合
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="nodeName">节点名，如："//WorkFlowModule/Data/Data-Field"</param>
        /// <returns>节点集合</returns>
        public static XmlNodeList XmlGetNode(string module, string nodeName)
        {
            try
            {
                XmlNodeList xlist = GetXmlDoc(module).SelectNodes(nodeName);
                return xlist;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 取节点属性
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="nodeName">节点名，如："//WorkFlowModule/Data/Data-Field"</param>
        /// <returns>节点属性</returns>
        public static string XmlGetNodeInnerXml(string module, string nodeName)
        {
            try
            {
                XmlNode xmlNode = GetXmlDoc(module).SelectSingleNode(nodeName);
                if (xmlNode != null)
                {
                    return xmlNode.InnerXml;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 取xml文档字符串
        /// </summary>
        /// <param name="module">模块名</param>
        /// <returns></returns>
        public static string GetConfiguration(string module)
        {
            string configuration = string.Empty;  //xml数据容器
            string sqlstr = "SELECT Configuration FROM system_Modules WHERE name ='" + module + "'";
            object config = FMOP.DB.DbHelperSQL.GetSingle(sqlstr);
            if (config != null)
            {
                configuration = xmlAssert + config.ToString();
                return configuration;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 去xmldoc对象
        /// </summary>
        /// <param name="module">模块名</param>
        /// <returns></returns>
        public static XmlDataDocument GetXmlDoc(string module)
        {
            try
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(GetConfiguration(module)));
                xmldoc.Load(stream);
                return xmldoc;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }



        #endregion

        #region 弹出主窗口页面

        /// <summary>
        /// 产生弹出页面（主表）
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="fieldName">弹出页面的位置</param>
        /// <returns></returns>
        public static DataSet PopMasterRecordset(string module, string fieldName)
        {
            DataSet result = null;
            string sqlCmd = "";

                try
                {
                    sqlCmd = PopMasterSql(module, fieldName);
                    if (sqlCmd != "")
                    {
                        //执行查询
                        result = DbHelperSQL.Query(sqlCmd);
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            return result;
        }

        /// <summary>
        /// 弹出页面(子表)
        /// </summary>
        /// <param name="module"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DataSet PopSubRecordset(string module, string fieldName, string subName)
        {
            DataSet result = null;
            string sqlCmd = "";

            try
            {
                sqlCmd = PopSubSql(module, fieldName,subName);
                if (sqlCmd != "")
                {
                    //执行查询
                    result = DbHelperSQL.Query(sqlCmd);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        /// <summary>
        /// 返回xml中子窗口的SQL语句
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="fieldName">弹出页面的触发点</param>
        /// <returns></returns>
        public static string PopMasterSql(string module, string fieldName)
        {
            XmlNodeList xlist = null;
            string sqlCmd = "";

            try
            {
                //根据模块名称，查询XML内容
                if (module != "")
                {
                    xlist = GetXmlNodeList(module, "//WorkFlowModule/Data/Data-Field");
                    foreach (XmlNode item in xlist)
                    {
                        //寻找name节点
                        if (item.SelectSingleNode("name") != null && item["name"].InnerText == fieldName)
                        {
                            sqlCmd = item["modelSelectSQL"].InnerXml;
                        }
                    }
                }
                //返回查询到的SQL语句
                return sqlCmd;
            }
            catch (Exception ex)
            {
                throw ex;
                
            }
           
        }
        /// <summary>
        /// 得到Data-Field节点下某字段的值,不包括映射列表。 刘杰 2010-09-10
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="nodename">
        /// 节点名
        /// type,name,caption,canNull,isInListTable,style,onblur,
        /// readonly,length,maxValue,minvalue,defaultValue,items,
        /// comboSQL,comboValueField,hasModelButton,modelSelectSQL</param>
        /// <returns>string</returns>
          
        public static string GetDataFieldValue(string module, string fieldName,string nodename)
        {
            XmlNodeList xlist = null;
            string sqlCmd = "";

            try
            {
                //根据模块名称，查询XML内容
                if (module != "")
                {
                    xlist = GetXmlNodeList(module, "//WorkFlowModule/Data/Data-Field");
                    foreach (XmlNode item in xlist)
                    {
                        //寻找name节点
                        if (item.SelectSingleNode(nodename) != null && item["name"].InnerText == fieldName)
                        {
                            sqlCmd = item[nodename].InnerXml;
                        }

                    }
                }
                //返回查询到的SQL语句
                return sqlCmd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回subTable/Sub-Field某节点的值，不支持节点列表。刘杰2010-09-26
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="fieldName">主表字段名</param>
        /// <param name="subfieldName">子表字段名</param>
        /// <param name="nodename">节点名</param>
        /// <returns>string</returns>
        public static string GetSubTableNodeValue(string module, string fieldName, string subfieldName,string nodename)
        {
            XmlNodeList xlist = null;
            string nodevalue= "";

            try
            {
                //根据模块名称，查询XML内容
                if (module != "")
                {
                    xlist = GetXmlNodeList(module, "//WorkFlowModule/Data/Data-Field");
                    foreach (XmlNode item in xlist)  //遍历Data-Field节点
                    {
                        string dtype = XMLHelper.GetSingleString(item, "type");
                        string dname = XMLHelper.GetSingleString(item, "name");
                        if((dtype=="SubTable")&&(dname==fieldName))
                        {
                            foreach (XmlNode ditem in item.SelectNodes("subTable/Sub-Field")) //遍历subTable/Sub-Field节点
                            {
                                string sname = XMLHelper.GetSingleString(ditem,"name");
                                if((sname==subfieldName)&&(ditem.SelectSingleNode(nodename)!=null))
                                {
                                    nodevalue=ditem[nodename].InnerXml;
                                }  
  
                            }
                        }


                    }
                }
                return nodevalue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回xml中子窗口的SQL语句
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="fieldName">弹出页面的触发点</param>
        /// <returns></returns>
        public static string PopSubSql(string module, string fieldName,string subName)
        {
            XmlNodeList xlist = null;
            string sqlCmd = "";
            string type = string.Empty;
            string name = string.Empty;

            try
            {
                //根据模块名称，查询XML内容
                if (module != "")
                {
                    xlist = GetXmlNodeList(module, "//WorkFlowModule/Data/Data-Field");
                    foreach (XmlNode item in xlist)
                    {
                        type = XMLHelper.GetSingleString(item, "type");
                        name = XMLHelper.GetSingleString(item, "name");
                        if (type == "SubTable" && name == subName)
                        {
                            foreach (XmlNode node in item.SelectNodes("subTable/Sub-Field"))
                            {
                                if (node.SelectSingleNode("name") != null && subName + node["name"].InnerText == fieldName)
                                {
                                    if (node["modelSelectSQL"] != null && node["modelSelectSQL"].InnerXml != "")
                                    {
                                        sqlCmd = node["modelSelectSQL"].InnerXml;
                                        return sqlCmd;
                                    }
                                }
                            }
                        }
                    }
                }
                //返回查询到的SQL语句
                return sqlCmd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// 返回映射的节点名称(主要用于弹出页面)
        /// </summary>
        /// <param name="module">模块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>包含映射节点的哈希表</returns>
        public static Hashtable parseXmlChild(string module ,string fieldName)
        {
            Hashtable mapField =new Hashtable(); //存放源字段名和返回到上一页面的字段名
            XmlNodeList xlist = null;

            xlist = GetXmlNodeList(module, "//WorkFlowModule/Data/Data-Field");
            try
            {
                foreach (XmlNode item in xlist)
                {
                    //寻找name节点
                    if (item.SelectSingleNode("name") != null && item["name"].InnerXml == fieldName)
                    {
                        //取要映射赋值的字段,返回集合
                        XmlNodeList mapList = item.SelectNodes("//ModelField-Mappings/Mapping");
                        foreach (XmlNode mapItem in mapList)
                        {
                            //如果找到source元素和local元素，则添加到HashTable
                            if (mapItem["source"] != null && mapItem["local"] != null)
                            {
                                if (mapItem["title"] != null)
                                {
                                    //判断字段是否存在中文名，若存在中文名，则以 英文字段名:中文字段名的形式与要返回的Local一块添加到HashTable中
                                    mapField.Add(mapItem["source"].InnerXml + ":" + mapItem["title"].InnerXml, mapItem["local"].InnerXml);
                                }
                                else
                                {
                                    mapField.Add(mapItem["source"].InnerXml, mapItem["local"].InnerXml);
                                }
                            }
                        }
                        return mapField;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mapField;
        
        }

        /// <summary>
        /// 返回弹出页面所需要的字段
        /// </summary>
        /// <param name="sqlCmd">查询相应模块的 XML</param>
        /// <param name="fieldName">弹出页面的触发点</param>
        /// <returns></returns>
        public static ArrayList popXmlparse(string module, string fieldName)
        {
            ArrayList popField = new ArrayList(); //存放源字段名和返回到上一页面的字段名
            XmlNodeList xlist = null;

            xlist = GetXmlNodeList(module, "//WorkFlowModule/Data/Data-Field");
            foreach (XmlNode item in xlist)
            {
                //寻找name节点
                if (item.SelectSingleNode("name") != null && item["name"].InnerXml == fieldName)
                {
                    try
                    {
                        //取要映射赋值的字段,返回集合
                        XmlNodeList mapList = item.SelectNodes("ModelField-Mappings/Mapping");
                        foreach (XmlNode mapItem in mapList)
                        {
                            //如果找到source元素和local元素，则添加到HashTable
                            if (mapItem["source"] != null && mapItem["local"] != null)
                            {
                                if (mapItem["title"] != null)
                                {
                                    //判断字段是否存在中文名，若存在中文名，则以 英文字段名:中文字段名的形式与要返回的Local一块添加到HashTable中
                                    //popField.Add(mapItem["source"].InnerXml + ":" + mapItem["title"].InnerXml+":"+mapItem["local"].InnerXml); // 原算法
                                    ////新算法，加入了映射内容的扩充。刘杰 2010-09-17
                                    if (mapItem["url"] != null && mapItem["modulename"] != null && mapItem["posttext"] != null && mapItem["poptext"] != null)
                                    {
                                        popField.Add(mapItem["source"].InnerXml + ":" + mapItem["title"].InnerXml + ":" + mapItem["local"].InnerXml + ":" + mapItem["url"].InnerXml + ":" + mapItem["modulename"].InnerXml + ":" + mapItem["posttext"].InnerXml + ":" + mapItem["poptext"].InnerXml);
                                    }
                                    else
                                    {
                                        popField.Add(mapItem["source"].InnerXml + ":" + mapItem["title"].InnerXml + ":" + mapItem["local"].InnerXml + "::::");
                                    }
                                  
                                }
                                else
                                {
                                    popField.Add(mapItem["source"].InnerXml+":"+""+":"+mapItem["local"].InnerXml);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    //如果找到，则立即退出
                    return popField;
                }
            }
            return popField;
        }

        /// <summary>
        /// 返回弹出页面所需要的字段,子表的处理
        /// </summary>
        /// <param name="sqlCmd">查询相应模块的 XML</param>
        /// <param name="fieldName">弹出页面的触发点</param>
        /// <returns></returns>
        public static ArrayList popXmlparse(string module, string fieldName,string subName)
        {
            ArrayList popField = new ArrayList(); //存放源字段名和返回到上一页面的字段名
            XmlNodeList xlist = null;
            string type = string.Empty;
            string name = string.Empty;

            try
            {
                xlist = GetXmlNodeList(module, "//WorkFlowModule/Data/Data-Field");
                foreach (XmlNode item in xlist)
                {
                    type = XMLHelper.GetSingleString(item, "type");
                    name = XMLHelper.GetSingleString(item, "name");
                    if (type == "SubTable" && name == subName)
                    {
                        foreach (XmlNode node in item.SelectNodes("subTable/Sub-Field"))
                        {
                            //寻找name节点
                            name = subName + XMLHelper.GetSingleString(node,"name","");
                            if (name == fieldName)
                            {
                                
                                //取要映射赋值的字段,返回集合
                                XmlNodeList mapList = node.SelectNodes("ModelField-Mappings/Mapping");
                                foreach (XmlNode mapItem in mapList)
                                {
                                    //如果找到source元素和local元素，则添加到HashTable
                                    if (mapItem["source"] != null && mapItem["local"] != null)
                                    {
                                        if (mapItem["title"] != null)
                                        {
                                            //判断字段是否存在中文名，若存在中文名，则以 英文字段名:中文字段名的形式与要返回的Local一块添加到HashTable中
                                          //popField.Add(mapItem["source"].InnerXml + ":" + mapItem["title"].InnerXml + ":" + subName +mapItem["local"].InnerXml);

                                            //新算法，加入了映射内容的扩充。刘杰 2010-09-27
                                          if (mapItem["url"] != null && mapItem["modulename"] != null && mapItem["posttext"] != null && mapItem["poptext"] != null)
                                          {
                                              popField.Add(mapItem["source"].InnerXml + ":" + mapItem["title"].InnerXml + ":" + subName + mapItem["local"].InnerXml + ":" + mapItem["url"].InnerXml + ":" + mapItem["modulename"].InnerXml + ":" + mapItem["posttext"].InnerXml + ":" + mapItem["poptext"].InnerXml);
                                          }
                                          else
                                          {
                                              popField.Add(mapItem["source"].InnerXml + ":" + mapItem["title"].InnerXml + ":" + subName + mapItem["local"].InnerXml + "::::");
                                          }
                              
                                        }
                                        else
                                        {
                                            popField.Add(mapItem["source"].InnerXml + ":" + "" + ":" + subName + mapItem["local"].InnerXml);
                                        }
                                    }
                                }
                                //如果找到，则立即退出
                                if (popField != null && popField.Count > 0)
                                {
                                    return popField;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return popField;
        }

        /// <summary>
        /// 传送指定模块,返回配置XML
        /// </summary>
        /// <param name="ModuleName">模块名 </param>
        /// <returns></returns>
        public static string GetXml(string module)
        {
            try
            {
                if (module != "")
                {
                    module = "SELECT configuration.query('/WorkFlowModule') from system_Modules where name='" + module + "'";
                    module = DbHelperSQL.GetSingle(module).ToString();
                }
                return module;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsAuto(string module)
        {
            try
            {
                if (module != "")
                {
                    module = "SELECT configuration.query('/WorkFlowModule/NumberFormat') from system_Modules where name='" + module + "'";
                    module = DbHelperSQL.GetSingle(module).ToString();
                    Stream stream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlAssert + module));
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(stream);
                    module = xmldoc.SelectSingleNode("//type").InnerXml;
                    if (module == "auto")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回指定模块中指定节点列表
        /// </summary>
        /// <param name="module">模块名</param>
        /// <param name="NodeName">节点名</param>
        /// <returns></returns>
        public static XmlNodeList GetXmlNodeList(string module,string nodeName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xList = null;
            Stream xmlStream = null;
            string xmlstr = "";

            try
            {
                xmlstr = xmlAssert + GetXml(module);
                xmlStream = new MemoryStream(System.Text.UTF8Encoding.Default.GetBytes(xmlstr));
                xmlDoc.Load(xmlStream);
                xList = xmlDoc.SelectNodes(nodeName);

                return xList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}