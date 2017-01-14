using System.Text.RegularExpressions;
using System.Collections;


    /// <summary>
    /// 字符串处理类
    /// </summary>
    public class HTMLAnalyzeClass
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


        /// <summary>
        /// 过滤字符串中的html标签。
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string wipescript(string html)
        {
            html = html.Replace("\r\n", "");
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+?</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *([""|']) *javascript:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*?=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@" style *= *([""|'])[^\1]+?expression[^\1]+?\1 ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, " href=$1"); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disabledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex5.Replace(html, ""); //过滤frameset
            html = regex6.Replace(html, " "); //过滤style里面的expression
            return html;
        }


         public     string   StripHTML(string   strHtml){
                  string   []   aryReg   ={
                              @"<script[^>]*?>.*?</script>",
                              @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                              @"([\r\n])[\s]+",
                              @"&(quot|#34);",
                              @"&(amp|#38);",
                              @"&(lt|#60);",
                              @"&(gt|#62);",
                              @"&(nbsp|#160);",
                              @"&(iexcl|#161);",
                              @"&(cent|#162);",
                              @"&(pound|#163);",
                              @"&(copy|#169);",
                              @"&#(\d+);",
                              @"-->",
                              @"<!--.*\n"
                            };
                  string   []   aryRep   =   {
                                "",
                                "",
                                "",
                                "\"",
                                "&",
                                "<",
                                ">",
                                "   ",
                                "\xa1",//chr(161),
                                "\xa2",//chr(162),
                                "\xa3",//chr(163),
                                "\xa9",//chr(169),
                                "",
                                "\r\n",
                                ""
                              };
                  string   newReg   =aryReg[0];
                  string   strOutput=strHtml;
                  for(int   i   =   0;i<aryReg.Length;i++){
                      Regex   regex   =   new   Regex(aryReg[i],RegexOptions.IgnoreCase);
                      strOutput   =   regex.Replace(strOutput,aryRep[i]);
                  }
                  strOutput.Replace("<","");
                  strOutput.Replace(">","");
                  strOutput.Replace("\r\n","");
                  return   strOutput;
              }


        public string DelHTML(string Htmlstring)//将HTML去除
        {
            #region
            //删除脚本
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"-->", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<!--.*", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //Htmlstring =System.Text.RegularExpressions. Regex.Replace(Htmlstring,@"<A>.*</A>","");
            //Htmlstring =System.Text.RegularExpressions. Regex.Replace(Htmlstring,@"<[a-zA-Z]*=\.[a-zA-Z]*\?[a-zA-Z]+=\d&\w=%[a-zA-Z]*|[A-Z0-9]","");
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(amp|#38);", "&", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(lt|#60);", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(gt|#62);", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"&#(\d+);", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            //Htmlstring=HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            #endregion
            return Htmlstring;
        }
    }
