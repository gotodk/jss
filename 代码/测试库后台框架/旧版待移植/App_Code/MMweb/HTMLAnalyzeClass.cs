using System.Text.RegularExpressions;
using System.Collections;


    /// <summary>
    /// �ַ���������
    /// </summary>
    public class HTMLAnalyzeClass
    {

        public HTMLAnalyzeClass()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            ;
        }


        /// <summary>
        /// ��ȡ��������ʽ��ƥ����ַ�����̬����
        /// �����ȡ���ֻ��һ��ƥ�䣬��������ֻ��һ��ֵ
        /// �����ȡ����ж�������������ж��ֵ
        /// ���ҵ�ƥ�����һ�����ý����Ѿ��ҵ����ַ���֮��ʼ����
        /// </summary>
        /// <param name="inputString">��Ҫ��ȡ���ַ���</param>
        /// <param name="begin_str">��ʼ��ȡ�ж���־</param>
        /// <param name="over_str">ֹͣ��ȡ�ж���־</param>
        /// <param name="baohan">����ֵ�Ƿ�����ж���־(0Ϊ������1Ϊ������)</param>
        /// <param name="mustAa">�Ƿ����ִ�Сд(trueΪ���֣�falseΪ������)</param>
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
        /// ����������ʽ�����ַ�������������ʽƥ����ַ��������滻
        /// </summary>
        /// <param name="str">��Ҫ���˵��ַ���</param>
        /// <param name="RegexStr">��Ҫ���˵�ƥ�������ʽ,����"��"���ָ������Ҫ���˹���</param>
        /// <param name="TH">��Ҫ�滻���ַ���</param>
        /// <returns>���˽��</returns>
        public string RegexGL(string str, string RegexStr, string TH)
        {
            string[] tempRegexARR = RegexStr.Split('��');
            string[] tempTHARR = TH.Split('��');
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
        /// �������ű�����������ţ��Ա����ݿ���ʶ��
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
        /// �������ţ����ڻ�С�ڷ��ŵȽ���ɷ�HTML���Ե��ַ�����Encode��ԣ�

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
        /// ��ȡָ�����ȵ��ַ���(�ӵ�һλ��ʼ,֧�ֺ���)
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
                    //������,��"..."���ű���
                    return temp + "...";
                }
            }
            return "";
        }


        /// <summary>
        /// �����ַ����е�html��ǩ��
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
            html = regex1.Replace(html, ""); //����<script></script>���
            html = regex2.Replace(html, " href=$1"); //����href=javascript: (<A>) ����
            html = regex3.Replace(html, " _disabledevent="); //���������ؼ���on...�¼�
            html = regex4.Replace(html, ""); //����iframe
            html = regex5.Replace(html, ""); //����frameset
            html = regex6.Replace(html, " "); //����style�����expression
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


        public string DelHTML(string Htmlstring)//��HTMLȥ��
        {
            #region
            //ɾ���ű�
            Htmlstring = System.Text.RegularExpressions.Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //ɾ��HTML
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
