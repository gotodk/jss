using System.Text.RegularExpressions;
using System.Collections;


namespace GOCE.f_AnalyzeClass
{
    /// <summary>
    /// �ַ���������
    /// </summary>
    class HTMLAnalyzeClass
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


    }
}
