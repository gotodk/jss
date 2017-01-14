using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text ;
using System.Collections;
using System.Text.RegularExpressions;
namespace FMOP.SS
{
    /// <summary>
    /// SplitString 分割字符串
    /// </summary>
    public class SplitString
    {
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strtext">字符串</param>
        /// <param name="sign">分隔符</param>
        /// <returns></returns>
        public static string[]  GetArralist(string strtext,string sign)
        {
            string[] getArralist = null;
            if(strtext ==null||sign ==null) return getArralist ;
            //字符串是否包含符号分割
            if (strtext.IndexOf(sign) <= 0) return getArralist;
            //把分割的字符串填充到数组
            getArralist = Regex.Split(strtext, sign, RegexOptions.IgnoreCase);
            return getArralist;
        }
    }
}