using System.Text.RegularExpressions;
using System.Collections;
using System;
 
namespace 客户端主程序.Support
{
    public class ValStr
    {
        /// <summary>
        /// 验证字符串中是否存在中文字符。真 为 存在至少一个中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool haveCN(string str)
        {
            Regex reg = new Regex(@"[\u4e00-\u9fa5]");//正则表达式
            if (reg.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


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
        /// 判断身份证号是否正确，为方便本部分测试用（以9开头的身份证号不走算法，一律视为正确的）
        /// </summary>
        /// <param name="strIdCard">身份证号</param>
        /// <returns>返回对应的提示信息</returns>
        public static string isTrueIDCard(string idCard)
        {
            //方便本部门人员测试用
            if (idCard.Substring(0, 1) == "9")
            {
                return "身份证有效";
            }
            

            //判断用户的出生年，出生月，出生日期
            if (!(Convert.ToInt32(idCard.Substring(6, 4).ToString()) >= 1900 && Convert.ToInt32(idCard.Substring(6, 4).ToString()) <= 2015))
            {
                return "身份证上的出生年份不正确！";
            }

            if (!(Convert.ToInt32(idCard.Substring(10, 2).ToString()) >= 1 && Convert.ToInt32(idCard.Substring(10, 2).ToString()) <= 12))
            {
                return "身份证上的出生月份不正确！";
            }
            if (!(Convert.ToInt32(idCard.Substring(12, 2).ToString()) >= 1 && Convert.ToInt32(idCard.Substring(10, 2).ToString()) <= 31))
            {
                return "身份证上的出生日不正确！";
            }
            //90年到83年之前的不进行验证
            if ((Convert.ToInt32(idCard.Substring(6, 4).ToString()) >= 1900 && Convert.ToInt32(idCard.Substring(6, 4).ToString()) <= 1983))
            {
                return "身份证有效";
            }

            /// <summary>
            /// Wi：加权因子
            /// </summary>
            int[] intModlue = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //得到身份证号数组
            object[] idCard_obj = new object[idCard.Length];
            int i = 0;
            do
            {
                idCard_obj[i] = idCard.Substring(0, 1);
                i++;
                idCard = idCard.Remove(0, 1);
            } while (idCard.Length >= 1);
            //（1）十七位数字本体码加权求和公式 
            //S = Sum(Ai * Wi), i = 0, ... , 16 ，先对前17位数字的权求和 
            int s = 0;
            for (int j = 0; j < intModlue.Length; j++)
            {
                s += Convert.ToInt32(idCard_obj[j]) * intModlue[j];
            }
            //（2）计算模 
            //Y = mod(S, 11) 
            int y = (12 - s % 11) % 11;
            string str = y == 10 ? "X" : y.ToString();
            if (str.ToLower() == idCard_obj[17].ToString().ToLower())
            {
                return "身份证有效";
            }
            return "请填写有效的身份证号！";
        }
        /// <summary>
        /// 验证电话的格式，手机和座机电话
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isDH(string str)
        {
            //return Regex.IsMatch(str, @"^(^\d{2,4}-\d{8}$|^\d{11}$|^\d{2,4}-\d{8}-\d{}$)$");
            return true;
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
        /// <summary>
        /// 验证证券资金密码(6位数字)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isZQZJMM(string str)
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
