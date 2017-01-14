using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.IO;
using FMOP.DB;

namespace Hesion.Brick.Core.WorkFlow{
	/// <summary>
	/// 单号生成类
	/// </summary>
	public class NumberFormat{	

		/// <summary>
		/// 类型(auto:自动生成;manual:手动填写)
		/// </summary>
		private string Type;

		/// <summary>
		/// 单号前缀
		/// </summary>
		private string Head;

		/// <summary>
		/// 日期格式(yymm:0801;yyyymm:200801;none:空)2008年1月
		/// </summary>
		private string DateFormat;

		/// <summary>
		/// 序号长度
		/// </summary>
		private int SeqLength;

		/// <summary>
		/// 工作流模块基本属性
		/// </summary>
		private Property property;
        /// <summary>
        /// NumberFormatNode节点
        /// </summary>
        private XmlNode NumberFormatNode;

		/// <summary>
		/// 通过工作流模块基本属性和xml节点构造本类
		/// </summary>
		/// <param name="pro"></param>
		/// <param name="Node"></param>
		public NumberFormat(Property pro, XmlNode Node){
            if (pro != null && Node != null)
            {
                property = pro;
                NumberFormatNode = Node;
            }
		}
	
		/// <summary>
		/// 获取下一张单据的单号
		/// </summary>
		/// <returns></returns>
		public string GetNextNumber(){
            string nextNumber = null;
            if (property != null && NumberFormatNode != null)
            {
                //取当前单号数据
                int LastYear = property.LastYear;
                int LastMonth = property.LastMonth;
                int Sequence = property.Sequence;
                //取得当前系统时间的年份
                string Nowyear = System.DateTime.Now.ToString("yyyy");//当前年份
                string ModuleName = property.ModuleName;
                //取配置信息中生成单号的信息
                if (NumberFormatNode != null)
                {
                    Type = NumberFormatNode["type"].InnerXml;
                    Head = NumberFormatNode["head"].InnerXml;
                    DateFormat = NumberFormatNode["dateFormat"].InnerXml;
                    SeqLength = int.Parse(NumberFormatNode["seqLength"].InnerXml);
                    if (Type == "auto")
                    {
                        string Numbertype = null;
                        switch (DateFormat)
                        {
                            case "yymm":
                                Numbertype = System.DateTime.Now.ToString("yy");//取得系统年份
                                nextNumber = RtNumber(Nowyear, LastYear, LastMonth, Head, Sequence, SeqLength, ModuleName, Numbertype);
                                break;
                            case "yyyy":
                                Numbertype = Nowyear;
                                nextNumber = RtNumber(Nowyear, LastYear, LastMonth, Head, Sequence, SeqLength, ModuleName, Numbertype);
                                break;
                            case "none":
                                //取得数据库中的序号进行加1
                                int new_sequence = Sequence + 1;
                                //更新数据库
                                UpdateSequence(new_sequence, ModuleName);
                                //生成指定位数sequence字符串
                                string sequenceNumber = PatchZero(new_sequence.ToString(), SeqLength);
                                //指定格式的单号
                                nextNumber = Head + sequenceNumber;
                                break;
                            default:
                                break;
                        }
                    }
                    return nextNumber;
                }
            }
            return nextNumber;
        }






        /// <summary>
        /// 获取下一张单据的单号(前置符+两位年+两位月+两位日+六位顺序号)
        /// </summary>
        /// <param name="beginStr">前置符号</param>
        /// <returns></returns>
        public string GetNextNumberZZ(string beginStr)
        {
            string nextNumber = null;
            if (property != null && NumberFormatNode != null)
            {
                //取当前单号数据
                int LastYear = property.LastYear;
                int LastMonth = property.LastMonth;
                int Sequence = property.Sequence;
                //取得当前系统时间的年份
                string Nowyear = System.DateTime.Now.ToString("yyyy");//当前年份
                string ModuleName = property.ModuleName;
                //取配置信息中生成单号的信息
                if (NumberFormatNode != null)
                {
                    Type = NumberFormatNode["type"].InnerXml;
                    Head = NumberFormatNode["head"].InnerXml;
                    DateFormat = NumberFormatNode["dateFormat"].InnerXml;
                    SeqLength = int.Parse(NumberFormatNode["seqLength"].InnerXml);
                    if (Type == "auto")
                    {
                        string Numbertype = null;
                        Numbertype = System.DateTime.Now.ToString("yy");//取得系统年份
                        nextNumber = RtNumberZZ(Nowyear, LastYear, LastMonth, beginStr, Sequence, SeqLength, ModuleName, Numbertype);
                    }
                    return nextNumber;
                }
            }
            return nextNumber;
        }




        /// <summary>
        /// 获取导入ERP的交易方编号(前置符+两位年+两位月+五位顺序号)
        /// </summary>
        /// <param name="beginStr">前置符号</param>
        /// <returns></returns>
        public string GetNextNumberZZ_JYFBH(string beginStr)
        {
            string nextNumber = null;
            if (property != null && NumberFormatNode != null)
            {
                //取当前单号数据
                int LastYear = property.LastYear;
                int LastMonth = property.LastMonth;
                int Sequence = property.Sequence;
                //取得当前系统时间的年份
                string Nowyear = System.DateTime.Now.ToString("yyyy");//当前年份
                string ModuleName = property.ModuleName;
                //取配置信息中生成单号的信息
                if (NumberFormatNode != null)
                {
                    Type = NumberFormatNode["type"].InnerXml;
                    Head = NumberFormatNode["head"].InnerXml;
                    DateFormat = NumberFormatNode["dateFormat"].InnerXml;
                    SeqLength = 5;
                    if (Type == "auto")
                    {
                        string Numbertype = null;
                        Numbertype = System.DateTime.Now.ToString("yy");//取得系统年份
                        nextNumber = RtNumber(Nowyear, LastYear, LastMonth, beginStr, Sequence, SeqLength, ModuleName, Numbertype);
                    }
                    return nextNumber;
                }
            }
            return nextNumber;
        }




        #region
        /// <summary>
        /// 根据格式返回指定单号
        /// </summary>
        /// <param name="Nowyear">当前年份</param>
        /// <param name="_lastyear">数据库中的年份</param>
        /// <param name="_lastmonth">数据库中的月份</param>
        /// <param name="Head">配置信息中生成单号号头</param>
        /// <param name="_sequence">数据库中单号的序号</param>
        /// <param name="seqLength">配置信息中序号的长度</param>
        /// <param name="ModuleName">模块名</param>
        /// <param name="Numbertype">声称单号的格式</param>
        /// <returns>Number</returns>
        private static string RtNumber(string Nowyear, int _lastyear, int _lastmonth, string Head, int _sequence, int seqLength, string ModuleName, string Numbertype)
        {
            //取得当前系统时间的月份
            string Number = null;
            string Nowmonth = System.DateTime.Now.ToString("MM");
            int NoNowyear = int.Parse(Nowyear);
            int NoNowmonth = int.Parse(Nowmonth);
            if (NoNowyear == _lastyear)
            {
                if (NoNowmonth == _lastmonth)
                {
                    //取得数据库中的序号进行加1
                    int new_sequence = _sequence + 1;
                    //更新数据库
                    Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                    //生成指定位数sequence字符串
                    string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                    //指定格式的单号
                    Number = Head + Numbertype + Nowmonth + sequenceNumber;
                }
                else
                {
                    //数据库序列为1
                    int new_sequence = 1;
                    //更新数据库
                    Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                    //生成指定位数sequence字符串
                    string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                    Number = Head + Numbertype + Nowmonth + sequenceNumber;
                }
            }
            else if (NoNowyear > _lastyear)
            {
                //数据库序列为1
                int new_sequence = 1;
                //更新数据库
                Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                //生成指定位数sequence字符串
                string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                Number = Head + Numbertype + Nowmonth + sequenceNumber;
            }
            return Number;
        }




        private static string RtNumberZZ(string Nowyear, int _lastyear, int _lastmonth, string Head, int _sequence, int seqLength, string ModuleName, string Numbertype)
        {
            //取得当前系统时间的月份
            string Number = null;
            string Nowmonth = System.DateTime.Now.ToString("MM");
            string Nowday = System.DateTime.Now.ToString("dd");
            int NoNowyear = int.Parse(Nowyear);
            int NoNowmonth = int.Parse(Nowmonth);
            if (NoNowyear == _lastyear)
            {
                if (NoNowmonth == _lastmonth)
                {
                    //取得数据库中的序号进行加1
                    int new_sequence = _sequence + 1;
                    //更新数据库
                    Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                    //生成指定位数sequence字符串
                    string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                    //指定格式的单号
                    Number = Head + Numbertype + Nowmonth + Nowday + sequenceNumber;
                }
                else
                {
                    //数据库序列为1
                    int new_sequence = 1;
                    //更新数据库
                    Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                    //生成指定位数sequence字符串
                    string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                    Number = Head + Numbertype + Nowmonth + Nowday + sequenceNumber;
                }
            }
            else if (NoNowyear > _lastyear)
            {
                //数据库序列为1
                int new_sequence = 1;
                //更新数据库
                Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                //生成指定位数sequence字符串
                string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                Number = Head + Numbertype + Nowmonth + Nowday + sequenceNumber;
            }
            return Number;
        }

        #endregion

        #region
        /// <summary>
        /// 补0操作
        /// </summary>
        /// <param name="_sequence">要补的数</param>
        /// <param name="seqLength">要补的长度</param>
        /// <returns>SequenceNumber</returns>
        public static string PatchZero(string _sequence, int seqLength)
        {
            string SequenceNumber = null;
            //判断_sequence的长度是否符合配置文件要求的长度，不符合长度用0补齐
            for (int i = _sequence.Length; i < seqLength; i++)
            {
                _sequence = "0" + _sequence;

            }
            SequenceNumber = _sequence;
            return SequenceNumber;
        }
        #endregion

        #region
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="LastYear">年份</param>
        /// <param name="LastMonth">月份</param>
        /// <param name="Sequence">序号</param>
        /// <param name="ModuleName">模块名</param>
        private static void Update(int LastYear, int LastMonth, int Sequence, string ModuleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update system_Modules set ");
            strSql.Append("LastYear=@LastYear,");
            strSql.Append("LastMonth=@LastMonth,");
            strSql.Append("Sequence=@Sequence");
            strSql.Append(" where name=@name ");

            SqlParameter[] parameters = {
					new SqlParameter("@LastYear", SqlDbType.SmallInt,2),
					new SqlParameter("@LastMonth", SqlDbType.SmallInt,2),
					new SqlParameter("@Sequence", SqlDbType.Int,4),
                    new SqlParameter ("@name",SqlDbType.VarChar,100)};
            parameters[0].Value = LastYear;
            parameters[1].Value = LastMonth;
            parameters[2].Value = Sequence;
            parameters[3].Value = ModuleName;
            //执行更新
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        #endregion

        #region
        /// <summary>
        /// 更新序列号
        /// </summary>
        /// <param name="Sequence">序列号</param>
        /// <param name="ModuleName">模块名</param>
        private static void UpdateSequence(int Sequence, string ModuleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update system_Modules set ");
            strSql.Append("Sequence=@Sequence ");
            strSql.Append("where name=@name ");

            SqlParameter[] parameters = {
					new SqlParameter("@Sequence", SqlDbType.Int,4),
                    new SqlParameter ("@name",SqlDbType.VarChar,100)};
            parameters[0].Value = Sequence;
            parameters[1].Value = ModuleName;
            //执行更新
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        #endregion
    }
}