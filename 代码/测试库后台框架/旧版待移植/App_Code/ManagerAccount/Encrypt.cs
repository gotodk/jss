using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
/// <summary>
/// Encrypt 的摘要说明
/// </summary>
namespace FM.ManagerAccount
{
    public class Encrypt
    {
        public Encrypt()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 返回16位的加密后的密文
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String getCryptograph(String text)
        {
            String Cryptograph = "";

            Cryptograph = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(text, "MD5").ToLower().Substring(8, 16);
            return Cryptograph;
        }
    }
}
