using System.Text.RegularExpressions;
using System.Collections;


namespace GOCE.f_AnalyzeClass
{
    /// <summary>
    /// 字符串处理类
    /// </summary>
    class HTMLAnalyzeClass
    {

        public HTMLAnalyzeClass()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            ;
        }


        /// <summary>
        /// 截取与正则表达式相匹配的字符串动态数组
        /// 如果截取结果只有一个匹配，则数组中只有一个值
        /// 如果截取结果有多个，则数组中有多个值
        /// 当找到匹配后，下一个配置将从已经找到的字符串之后开始配置
        /// </summary>
        /// <param name="inputString">需要截取的字符串</param>
        /// <param name="begin_str">开始截取判定标志</param>
        /// <param name="over_str">停止截取判定标志</param>
        /// <param name="baohan">返回值是否包含判定标志(0为包含，1为不包含)</param>
        /// <param name="mustAa">是否区分大小写(true为区分，false为不区分)</param>
        public ArrayList My_Cut_Str(string inputString, string begin_str, string over_str, int baohan, bool mustAa)
        {
            Regex r;
            Match m;
            ArrayList return_str = new ArrayList();
            if (inputString == "" || begin_str == "" || over_str == "" || inputString == null || begin_str == null || over_str == null)
            {
                return return_str;
            }
            if (mustAa)
            {
                r = new Regex(begin_str + "(.*?)" + over_str, RegexOptions.Compiled | RegexOptions.Singleline);
            }
            else
            {
                r = new Regex(begin_str + "(.*?)" + over_str, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }
            for (m = r.Match(inputString); m.Success; m = m.NextMatch())
            {
                return_str.Add(m.Groups[baohan].Value.ToString());
            }
            return return_str;

        }


        /// <summary>
        /// 根据正则表达式过滤字符串。与正则表达式匹配的字符串将被替换
        /// </summary>
        /// <param name="str">需要过滤的字符串</param>
        /// <param name="RegexStr">需要过滤的匹配规则表达式,可用"◆"来分隔多个需要过滤规则</param>
        /// <param name="TH">需要替换的字符串</param>
        /// <returns>过滤结果</returns>
        public string RegexGL(string str, string RegexStr, string TH)
        {
            string[] tempRegexARR = RegexStr.Split('★');
            string[] tempTHARR = TH.Split('★');
            for (int p = 0; p < tempRegexARR.Length; p++)
            {
                if (str == "" || str == null)
                {
                    return str;
                }

                str = Regex.Replace(str, tempRegexARR[p].ToString(), tempTHARR[p].ToString());
            }

            string returnstr = str;
            if (returnstr == "" || returnstr == null)
            {
                return "";
            }
            else
            {
                return returnstr;
            }
        }

        /// <summary>
        /// 将单引号变成两个单引号，以便数据可以识别
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Encode(string str)
        {
            //str = str.Replace("&", "&amp;");
            //str = str.Replace("<", "&lt;");
            //str = str.Replace(">", "&gt;");
            //str = str.Replace("\"", "&quot;");
            //str = str.Replace(" ", "&nbsp;");
            //str = str.Replace("'", "&apos;");
            str = str.Replace("'", "''");
            return str;
        }


        /// <summary>
        /// 将单引号，大于或小于符号等解码成非HTML语言的字符，与Encode相对．

        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Decode(string str)
        {
            str = str.Replace("&amp;", "&");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&apos;", "'");
            return str;
        }

        /// <summary>
        /// 截取指定长度的字符串(从第一位开始,支持汉字)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public string GetNumStr(string s, int l)
        {
            string temp = s;
            if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l)
            {
                return temp;
            }
            for (int i = temp.Length; i >= 0; i--)
            {
                temp = temp.Substring(0, i);
                if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l - 3)
                {
                    //超长后,加"..."符号表明
                    return temp + "...";
                }
            }
            return "";
        }


    }
}
