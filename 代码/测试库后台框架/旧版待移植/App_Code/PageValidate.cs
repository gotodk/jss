using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Hesion.Brick.Core;
using System.Text;


namespace FMOP.Tools //可以修改成实际项目的命名空间名称
{
	/// <summary>
	/// 页面数据校验类
	/// 李天平
	/// 2004.8
    /// 2007.12 gzl add
	/// </summary>
	public class PageValidate
	{
		private static Regex RegNumber = new Regex("^[0-9]+$");
		private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
		private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
		private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
		private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
		private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

		public PageValidate()
		{
		}


		#region 数字字符串检查		
		
		/// <summary>
		/// 检查Request查询字符串的键值，是否是数字，最大长度限制
		/// </summary>
		/// <param name="req">Request</param>
		/// <param name="inputKey">Request的键值</param>
		/// <param name="maxLen">最大长度</param>
		/// <returns>返回Request查询字符串</returns>
		public static string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
		{
			string retVal = string.Empty;
			if(inputKey != null && inputKey != string.Empty)
			{
				retVal = req.QueryString[inputKey];
				if(null == retVal)
					retVal = req.Form[inputKey];
				if(null != retVal)
				{
					retVal = SqlText(retVal, maxLen);
					if(!IsNumber(retVal))
						retVal = string.Empty;
				}
			}
			if(retVal == null)
				retVal = string.Empty;
			return retVal;
		}		
		/// <summary>
		/// 是否数字字符串
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsNumber(string inputData)
		{
			Match m = RegNumber.Match(inputData);
			return m.Success;
		}		
		/// <summary>
		/// 是否数字字符串 可带正负号
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsNumberSign(string inputData)
		{
			Match m = RegNumberSign.Match(inputData);
			return m.Success;
		}		
		/// <summary>
		/// 是否是浮点数
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsDecimal(string inputData)
		{
			Match m = RegDecimal.Match(inputData);
			return m.Success;
		}		
		/// <summary>
		/// 是否是浮点数 可带正负号
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsDecimalSign(string inputData)
		{
			Match m = RegDecimalSign.Match(inputData);
			return m.Success;
		}		

		#endregion

		#region 中文检测

		/// <summary>
		/// 检测是否有中文字符
		/// </summary>
		/// <param name="inputData"></param>
		/// <returns></returns>
		public static bool IsHasCHZN(string inputData)
		{
			Match m = RegCHZN.Match(inputData);
			return m.Success;
		}	

		#endregion

		#region 邮件地址
		/// <summary>
		/// 是否是浮点数 可带正负号
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsEmail(string inputData)
		{
			Match m = RegEmail.Match(inputData);
			return m.Success;
		}		

		#endregion

		#region 其他

		/// <summary>
		/// 检查字符串最大长度，返回指定长度的串
		/// </summary>
		/// <param name="sqlInput">输入字符串</param>
		/// <param name="maxLength">最大长度</param>
		/// <returns></returns>			
		public static string SqlText(string sqlInput, int maxLength)
		{			
			if(sqlInput != null && sqlInput != string.Empty)
			{
				sqlInput = sqlInput.Trim();							
				if(sqlInput.Length > maxLength)//按最大长度截取字符串
					sqlInput = sqlInput.Substring(0, maxLength);
			}
			return sqlInput;
		}

		
		/// <summary>
		/// 字符串编码
		/// </summary>
		/// <param name="inputData"></param>
		/// <returns></returns>
		public static string HtmlEncode(string inputData)
		{
			return HttpUtility.HtmlEncode(inputData);
		}
		/// <summary>
		/// 设置Label显示Encode的字符串
		/// </summary>
		/// <param name="lbl"></param>
		/// <param name="txtInput"></param>
		public static void SetLabel(Label lbl, string txtInput)
		{
			lbl.Text = HtmlEncode(txtInput);
		}
		public static void SetLabel(Label lbl, object inputObj)
		{
			SetLabel(lbl, inputObj.ToString());
		}		

		#endregion

		#region 检测特殊字符
		/// <summary>
		/// 检测特殊字符
		/// </summary>
		/// <param name="inputString"></param>
		/// <param name="maxLength">字符串长</param>
		/// <returns></returns>
		public static string InputText(string inputString, int maxLength)
		{
			StringBuilder builder1 = new StringBuilder();
			if ((inputString != null) && (inputString != string.Empty))
			{
				inputString = inputString.Trim();
				if (inputString.Length > maxLength)
				{
					inputString = inputString.Substring(0, maxLength);
				}
				for (int num1 = 0; num1 < inputString.Length; num1++)
				{
					switch (inputString[num1])
					{
						case '<':
							builder1.Append("&lt;");
							break;

						case '>':
							builder1.Append("&gt;");
							break;

						case '"':
							builder1.Append("&quot;");
							break;

						default:
							builder1.Append(inputString[num1]);
							break;
					}
				}
				builder1.Replace("'", " ");
			}
			return builder1.ToString();
		}
		#endregion


        #region 检验字符串是否含 ‘.’点，并转换
        public static string fieldDot(string arg)
        {
            if (arg.Equals(string.Empty))
            {
                return "";
            }
            else
            {
                int start = arg.IndexOf(".");
                if (start == -1)
                {
                    return arg;
                }
                else
                {
                    return arg.Substring(start + 1);
                }
            }
        }
        #endregion


        /// <summary>
        /// 查询table输入是否合法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool selectValidate(string type, string str)
        {
            try
            {
                if (type.Equals("System.String"))
                {
                }
                else if (type.Equals("System.DateTime"))
                {
                    DateTime.Parse(str);  
                }
                else
                {
                    decimal.Parse(str);
                }
                return true;

            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return false;
            }
        }


        /// <summary>
        /// where条件
        /// </summary>
        /// <param name="mark"></param>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string selectValue(string mark,string str,string type)
        {
            StringBuilder sb = new StringBuilder();
            if (mark.Equals("%%"))
            {
                sb.Append(" like ");
                sb.Append("'%" + str + "%' ");
            }
            else if (mark.Equals("a%"))
            {
                sb.Append(" like ");
                sb.Append("'" + str + "%' ");
            }
            else if (mark.Equals("%a"))
            {
                sb.Append(" like ");
                sb.Append("'%" + str + "' ");
            }
            else
            {
                if (type.Equals("System.String") || type.Equals("System.DateTime"))
                {
                    sb.Append(mark);
                    sb.Append(" '" + str + "' ");
                }
                else
                {
                    sb.Append(mark);
                    sb.Append(" " + str + " ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 组合新的查询语句
        /// </summary>
        /// <param name="selectSQL"></param>
        /// <param name="wherestr"></param>
        /// <returns></returns>
        public static string getSelectSQL(string selectSQL,string wherestr)
        {
            string beforesql, aftersql,sqlfull;
            int end = selectSQL.IndexOf("group"), end2 = selectSQL.IndexOf("order");
            if (end != -1)
            {
                beforesql = selectSQL.Substring(0, end);
                aftersql = selectSQL.Substring(end);
                sqlfull = beforesql + wherestr + aftersql;
            }
            else if (end2 != -1)
            {
                beforesql = selectSQL.Substring(0, end2);
                aftersql = selectSQL.Substring(end2);
                sqlfull = beforesql + wherestr + aftersql;
            }
            else
            {
                sqlfull= selectSQL + wherestr;
            }
            return sqlfull;
        }

		public static int stringToInt(string vars)
		{
			int i = 0;
			try
			{
				i = int.Parse(vars);
			}
			catch (Exception e)
			{
                Log.Error(e.Message);
				return 0;
			}
			return i;
		}


		///   <summary>   
		///   过滤输出字符串   
		///   </summary>   
		///   <param   name="inputstring">要过滤的字符串</param>   
		///   <returns>过滤后的字符串</returns>   
		public static string output(object inputstring)
		{
			if (inputstring == null)
				return string.Empty;

			StringBuilder strbuilder = new StringBuilder();
			string str1 = inputstring.ToString();
			strbuilder.Insert(0, str1);
			//strbuilder.replace(((char)32).tostring(), "&nbsp;");
			//strbuilder.replace(((char)9).tostring(), "&nbsp;");
			//strbuilder.replace(((char)34).tostring(), "&quot;");
			//strbuilder.replace(((char)39).tostring(), "&#39;");
			//strbuilder.replace(((char)13).tostring(), "   ");
			//strbuilder.replace(((char)10).tostring() + ((char)10).tostring(), "</p><p>");
			strbuilder.Replace(((char)10).ToString(), "<br />");

			return strbuilder.ToString();
		}


    }
}
