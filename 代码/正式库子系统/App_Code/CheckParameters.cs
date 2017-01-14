using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using FMOP.SD;
using FMOP.DB;
/// <summary>
/// CheckParameters 的摘要说明
/// 验证主表与子表的参数格式是否正确
/// </summary>
namespace FMOP.Check
{
    public class CheckParameters
    {
        public CheckParameters()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 验证参数为空
        /// </summary>
        /// <param name="paramArray">要进行非空验证的对象</param>
        /// <returns></returns>
        public static bool CheckEmpty(SaveDepositary[] paramArray)
        {
            for (int index = 0; index < paramArray.Length; index++)
            {
                if (paramArray[index].CanNull == false && paramArray[index].Text == "")
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证特殊参数的特殊规则
        /// </summary>
        /// <param name="paramArray">要验证的对象</param>
        /// <param name="modname">模块名</param>
        /// <returns></returns>
        public static ArrayList CheckSpecial(SaveDepositary[] paramArray, string modname)
        {
            return CheckParametersOther.myCheckSpecial(paramArray, modname);
            //return true;
        }

        public static string CheckSubEmpty(SaveDepositary[] paramArray)
        {
            bool isAllNull = false;
            for (int index = 0; index < paramArray.Length; index++)
            {
                if (paramArray[index].Text == "")
                {
                    isAllNull = true;
                }
                else
                {
                    break;
                }
            }

            if (isAllNull == true)
            {
                return "1";
            }
            else
            {
                for (int index = 0; index < paramArray.Length; index++)
                {
                    if (paramArray[index].CanNull == false && paramArray[index].Text == "")
                    {
                        return "0";
                    }
                    else
                    {
                        isAllNull = true;
                    }
                    if (isAllNull == true)
                    {
                        return "2";
                    }
                }
            }
            return "2";
        }

        /// <summary>
        /// 验证输入内容格式
        /// </summary>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        public static bool CheckType(SaveDepositary[] paramArray)
        {
            return true;
        }

        public static bool isExistsFile(string KeyNumber)
        {
            int count = 0;
            SqlCommand comm = new SqlCommand();
            try
            {
                string sqlCmd = "select Count(*) from FileUp where Number='" + KeyNumber + "'";
                count = DbHelperSQL.QueryInt(sqlCmd);
                if (count < 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
