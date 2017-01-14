using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace FMOP.Module
{
    /// <summary>
    /// fieldTool 的摘要说明
    /// </summary>
    public class fieldTool
    {
        public fieldTool()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// fieldType 字段类型赋值
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="fieldName"></param>
        public static string[] addfieldType(DataSet ds,string[] fieldName)
        {
            string[] fieldType = new string[fieldName.Length];
            for (int i = 0; i < fieldName.Length; i++)
            {
                if (fieldName[i] == null || fieldName[i].Equals(string.Empty))
                {
                    break;
                }
                else
                {
                    fieldType[i] = ds.Tables[0].Columns[fieldName[i]].DataType.ToString();
                }
            }
            return fieldType;
        }

    }
}
