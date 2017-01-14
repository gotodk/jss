using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace 客户端主程序.Support
{
    class ValStr
    {
        /// <summary>
        /// 验证字符串是否符合规范（手机号，穷举模式。可根据运营商号码范围变更自行添加）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isPhone(string str)
        {
            return Regex.IsMatch(str, @"^(13|15|18)[0-9]{9}$");
        }
        /// <summary>
        /// 验证字符串是否符合规范（电子邮箱，.cn需要二次匹配，如有更合适正则，可更换）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isEmail(string str)
        {
            //return Regex.IsMatch(str, @"^([A-Za-z0-9_]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return Regex.IsMatch(str, @"^([A-Za-z0-9_.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})(\]?)$");
        }
        /// <summary>
        /// 验证字符串是否符合规范（身份证号码，15位、18位、17位+X）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isIDCard(string str)
        {
            return Regex.IsMatch(str, @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
        }
        /// <summary>
        /// 验证字符串是否符合规范（平台密码规范，6-16位，字母和数字，区分大小写）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isPTPwd(string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z0-9]{6,16}$");
        }
        /// <summary>
        /// 验证字符串是否符合用户名规范（仅限本平台用户名规范，6-12位，汉字、字母、数字、下划线，不区分大小写）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isUserName(string str)
        {
            return Regex.IsMatch(str, @"^\w{2,16}$");
        }
        /// <summary>
        /// 验证字符串是否为邮政编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isPostalCode(string str)
        {
            return Regex.IsMatch(str, @"^\d{6}$");
        }

        public static bool ValidateQuery(Hashtable queryConditions)
        {
            //构造SQL的注入关键字符
            #region 字符
            string[] strBadChar = {"and ","and%20"
    ,"exec "
    ,"insert "
    ,"select "
    ,"delete "
    ,"update "
    ,"count "
    ,"or "
    ,"%20"
    //,"*"
    ,"%"
    ," :"
    ,"\'"
    ,","
    ,"\""
    ,"chr "
    ,"mid "
    ,"master "
    ,"truncate "
    ,"char "
    ,"declare "
    ,"SiteName "
    ,"net user "
    ,"xp_cmdshell "
    ,"/add"
    ,"exec master.dbo.xp_cmdshell "
    ,"net localgroup administrators "};
            #endregion

            //构造正则表达式
            string str_Regex = ".*(";
            for (int i = 0; i < strBadChar.Length - 1; i++)
            {
                str_Regex += strBadChar[i] + "|";
            }
            str_Regex += strBadChar[strBadChar.Length - 1] + ").*";
            //避免查询条件中_list情况
            foreach (string str in queryConditions.Keys)
            {
                if (str.Substring(str.Length - 5) == "_list")
                {
                    //去掉单引号检验
                    str_Regex = str_Regex.Replace("|'|", "|");
                }
                string tempStr = queryConditions[str].ToString();
                if (Regex.Matches(tempStr.ToString(), str_Regex).Count > 0)
                {
                    //有SQL注入字符
                    return true;
                }
            }
            return false;
        }
    }
}
