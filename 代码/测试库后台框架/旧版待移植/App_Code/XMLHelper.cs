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
using System.Collections;
using FMOP.DB;

/// <summary>
/// XMLReader 的摘要说明
/// </summary>
/// 2007.12.24 张伟修改，添加xn[name].innerXML不为空的判断,防止 <xml/>引起异常
namespace FMOP.XHelp
{
    public class XMLHelper
    {
        public static string GetSingleString(XmlNode xn, string name)
        {
            if (xn[name] != null && xn[name].InnerXml != "")
            {
                return xn[name].InnerXml;
            }
            else
            {
                return "";
            }
        }

        public static float GetSingleFloat(XmlNode xn, string name)
        {
            return float.Parse(xn[name].InnerXml);
        }

        public static int GetSingleInt(XmlNode xn, string name)
        {
            return int.Parse(xn[name].InnerXml);
        }

        public static bool GetSingleBool(XmlNode xn, string name)
        {
            bool Flag = true;
            if (xn[name] != null && xn[name].InnerXml.ToLower() == "true")
            {
                Flag = true;
            }
            else
            {
                Flag = false;
            }
            return Flag;
        }

        public static DateTime GetSingleDateTime(XmlNode xn, string name)
        {
            DateTime dt;
            try
            {
                dt = DateTime.Parse(xn[name].InnerXml);
            }
            catch
            {
                dt = DateTime.Parse("2000-1-1");
            }
            return dt;
        }

        public static Hashtable GetHashTable(XmlNode xn, string name)
        {
            Hashtable items = new Hashtable();
            string[] filed = xn[name].InnerXml.Split(',');

            foreach (string index in filed)
            {
                items.Add(index, index);
            }
            return items;
        }

        public static ArrayList GetArrayList(XmlNode xn, string name)
        {
            ArrayList items = new ArrayList();
            string[] filed = xn[name].InnerXml.Split(',');
            for (int index = 0; index < filed.Length; index++)
            {
                items.Add(filed[index].ToString());
            }
            return items;
        }

        public static int GetSingleInt(XmlNode xn, string name, int defaultValue)
        {
            if (xn != null)
            {
                if (xn[name] != null && xn[name].InnerXml != "")
                {
                    return int.Parse(xn[name].InnerXml);
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        public static bool GetSingleBool(XmlNode xn, string name, bool defaultValue)
        {
            bool Flag = true;
            if (xn != null)
            {
                if (xn[name] != null && xn[name].InnerXml != "")
                {
                    try
                    {
                        if (xn[name].InnerXml.ToLower() == "true")
                        {
                            Flag = true;
                        }
                        else
                        {
                            Flag = false;
                        }
                    }
                    catch
                    {
                        Flag = false;
                    }
                }
                else
                {
                    Flag = defaultValue;
                }
            }
            else
            {
                Flag = defaultValue;
            }
            return Flag;
        }

        public static string GetSingleString(XmlNode xn, string name, string defaultValue)
        {
            if (xn != null)
            {
                if (xn[name] != null && xn[name].InnerXml != "")
                {
                    return xn[name].InnerXml;
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        public static float GetSingleFloat(XmlNode xn, string name, float defaultValue)
        {
            if (xn != null)
            {
                if (xn[name] != null && xn[name].InnerXml != "")
                {
                    return float.Parse(xn[name].InnerXml);
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        public static Hashtable GetHashTable(XmlNode xn, string comboValued, string comboSql)
        {
            Hashtable hashTable = null;
            DataSet result = new DataSet();

            //判断SQL语句和绑定的Value字段是否存在于XML中
            if (xn[comboSql] != null && comboValued != null)
            {
                string strSql = xn[comboSql].InnerXml;
                //执行查询
                result = DbHelperSQL.Query(strSql);
                if (result != null && result.Tables[0] !=null )
                {
                    hashTable = new Hashtable();
                    //根据纪录数，把值存入HashTable.
                    foreach (DataRow rowIndex in result.Tables[0].Rows)
                    {
                        foreach (DataColumn colmIndex in result.Tables[0].Columns)
                        {
                            hashTable.Add(rowIndex[colmIndex].ToString(), rowIndex[colmIndex].ToString());
                        }
                    }
                }
            }
            return hashTable;
        }

        public static ArrayList GetArrayList(XmlNode xn, string comboValued, string comboSql)
        {
            ArrayList aryList = new ArrayList();
            DataSet result = new DataSet();

            //判断SQL语句和绑定的Value字段是否存在于XML中
            if (xn[comboSql] != null && comboValued != null)
            {
                string strSql = xn[comboSql].InnerXml;
                //执行查询
                result = DbHelperSQL.Query(strSql);
                if (result != null && result.Tables[0] != null)
                {
                    foreach (DataRow rowIndex in result.Tables[0].Rows)
                    {
                        foreach (DataColumn colmIndex in result.Tables[0].Columns)
                        {
                            aryList.Add(rowIndex[colmIndex].ToString());
                        }
                    }
                }
            }
            return aryList;
        }
    }
}
