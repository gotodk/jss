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
	/// ����������
	/// </summary>
	public class NumberFormat{	

		/// <summary>
		/// ����(auto:�Զ�����;manual:�ֶ���д)
		/// </summary>
		private string Type;

		/// <summary>
		/// ����ǰ׺
		/// </summary>
		private string Head;

		/// <summary>
		/// ���ڸ�ʽ(yymm:0801;yyyymm:200801;none:��)2008��1��
		/// </summary>
		private string DateFormat;

		/// <summary>
		/// ��ų���
		/// </summary>
		private int SeqLength;

		/// <summary>
		/// ������ģ���������
		/// </summary>
		private Property property;
        /// <summary>
        /// NumberFormatNode�ڵ�
        /// </summary>
        private XmlNode NumberFormatNode;

		/// <summary>
		/// ͨ��������ģ��������Ժ�xml�ڵ㹹�챾��
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
		/// ��ȡ��һ�ŵ��ݵĵ���
		/// </summary>
		/// <returns></returns>
		public string GetNextNumber(){
            string nextNumber = null;
            if (property != null && NumberFormatNode != null)
            {
                //ȡ��ǰ��������
                int LastYear = property.LastYear;
                int LastMonth = property.LastMonth;
                int Sequence = property.Sequence;
                //ȡ�õ�ǰϵͳʱ������
                string Nowyear = System.DateTime.Now.ToString("yyyy");//��ǰ���
                string ModuleName = property.ModuleName;
                //ȡ������Ϣ�����ɵ��ŵ���Ϣ
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
                                Numbertype = System.DateTime.Now.ToString("yy");//ȡ��ϵͳ���
                                nextNumber = RtNumber(Nowyear, LastYear, LastMonth, Head, Sequence, SeqLength, ModuleName, Numbertype);
                                break;
                            case "yyyy":
                                Numbertype = Nowyear;
                                nextNumber = RtNumber(Nowyear, LastYear, LastMonth, Head, Sequence, SeqLength, ModuleName, Numbertype);
                                break;
                            case "none":
                                //ȡ�����ݿ��е���Ž��м�1
                                int new_sequence = Sequence + 1;
                                //�������ݿ�
                                UpdateSequence(new_sequence, ModuleName);
                                //����ָ��λ��sequence�ַ���
                                string sequenceNumber = PatchZero(new_sequence.ToString(), SeqLength);
                                //ָ����ʽ�ĵ���
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
        /// ��ȡ��һ�ŵ��ݵĵ���(ǰ�÷�+��λ��+��λ��+��λ��+��λ˳���)
        /// </summary>
        /// <param name="beginStr">ǰ�÷���</param>
        /// <returns></returns>
        public string GetNextNumberZZ(string beginStr)
        {
            string nextNumber = null;
            if (property != null && NumberFormatNode != null)
            {
                //ȡ��ǰ��������
                int LastYear = property.LastYear;
                int LastMonth = property.LastMonth;
                int Sequence = property.Sequence;
                //ȡ�õ�ǰϵͳʱ������
                string Nowyear = System.DateTime.Now.ToString("yyyy");//��ǰ���
                string ModuleName = property.ModuleName;
                //ȡ������Ϣ�����ɵ��ŵ���Ϣ
                if (NumberFormatNode != null)
                {
                    Type = NumberFormatNode["type"].InnerXml;
                    Head = NumberFormatNode["head"].InnerXml;
                    DateFormat = NumberFormatNode["dateFormat"].InnerXml;
                    SeqLength = int.Parse(NumberFormatNode["seqLength"].InnerXml);
                    if (Type == "auto")
                    {
                        string Numbertype = null;
                        Numbertype = System.DateTime.Now.ToString("yy");//ȡ��ϵͳ���
                        nextNumber = RtNumberZZ(Nowyear, LastYear, LastMonth, beginStr, Sequence, SeqLength, ModuleName, Numbertype);
                    }
                    return nextNumber;
                }
            }
            return nextNumber;
        }




        /// <summary>
        /// ��ȡ����ERP�Ľ��׷����(ǰ�÷�+��λ��+��λ��+��λ˳���)
        /// </summary>
        /// <param name="beginStr">ǰ�÷���</param>
        /// <returns></returns>
        public string GetNextNumberZZ_JYFBH(string beginStr)
        {
            string nextNumber = null;
            if (property != null && NumberFormatNode != null)
            {
                //ȡ��ǰ��������
                int LastYear = property.LastYear;
                int LastMonth = property.LastMonth;
                int Sequence = property.Sequence;
                //ȡ�õ�ǰϵͳʱ������
                string Nowyear = System.DateTime.Now.ToString("yyyy");//��ǰ���
                string ModuleName = property.ModuleName;
                //ȡ������Ϣ�����ɵ��ŵ���Ϣ
                if (NumberFormatNode != null)
                {
                    Type = NumberFormatNode["type"].InnerXml;
                    Head = NumberFormatNode["head"].InnerXml;
                    DateFormat = NumberFormatNode["dateFormat"].InnerXml;
                    SeqLength = 5;
                    if (Type == "auto")
                    {
                        string Numbertype = null;
                        Numbertype = System.DateTime.Now.ToString("yy");//ȡ��ϵͳ���
                        nextNumber = RtNumber(Nowyear, LastYear, LastMonth, beginStr, Sequence, SeqLength, ModuleName, Numbertype);
                    }
                    return nextNumber;
                }
            }
            return nextNumber;
        }




        #region
        /// <summary>
        /// ���ݸ�ʽ����ָ������
        /// </summary>
        /// <param name="Nowyear">��ǰ���</param>
        /// <param name="_lastyear">���ݿ��е����</param>
        /// <param name="_lastmonth">���ݿ��е��·�</param>
        /// <param name="Head">������Ϣ�����ɵ��ź�ͷ</param>
        /// <param name="_sequence">���ݿ��е��ŵ����</param>
        /// <param name="seqLength">������Ϣ����ŵĳ���</param>
        /// <param name="ModuleName">ģ����</param>
        /// <param name="Numbertype">���Ƶ��ŵĸ�ʽ</param>
        /// <returns>Number</returns>
        private static string RtNumber(string Nowyear, int _lastyear, int _lastmonth, string Head, int _sequence, int seqLength, string ModuleName, string Numbertype)
        {
            //ȡ�õ�ǰϵͳʱ����·�
            string Number = null;
            string Nowmonth = System.DateTime.Now.ToString("MM");
            int NoNowyear = int.Parse(Nowyear);
            int NoNowmonth = int.Parse(Nowmonth);
            if (NoNowyear == _lastyear)
            {
                if (NoNowmonth == _lastmonth)
                {
                    //ȡ�����ݿ��е���Ž��м�1
                    int new_sequence = _sequence + 1;
                    //�������ݿ�
                    Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                    //����ָ��λ��sequence�ַ���
                    string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                    //ָ����ʽ�ĵ���
                    Number = Head + Numbertype + Nowmonth + sequenceNumber;
                }
                else
                {
                    //���ݿ�����Ϊ1
                    int new_sequence = 1;
                    //�������ݿ�
                    Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                    //����ָ��λ��sequence�ַ���
                    string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                    Number = Head + Numbertype + Nowmonth + sequenceNumber;
                }
            }
            else if (NoNowyear > _lastyear)
            {
                //���ݿ�����Ϊ1
                int new_sequence = 1;
                //�������ݿ�
                Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                //����ָ��λ��sequence�ַ���
                string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                Number = Head + Numbertype + Nowmonth + sequenceNumber;
            }
            return Number;
        }




        private static string RtNumberZZ(string Nowyear, int _lastyear, int _lastmonth, string Head, int _sequence, int seqLength, string ModuleName, string Numbertype)
        {
            //ȡ�õ�ǰϵͳʱ����·�
            string Number = null;
            string Nowmonth = System.DateTime.Now.ToString("MM");
            string Nowday = System.DateTime.Now.ToString("dd");
            int NoNowyear = int.Parse(Nowyear);
            int NoNowmonth = int.Parse(Nowmonth);
            if (NoNowyear == _lastyear)
            {
                if (NoNowmonth == _lastmonth)
                {
                    //ȡ�����ݿ��е���Ž��м�1
                    int new_sequence = _sequence + 1;
                    //�������ݿ�
                    Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                    //����ָ��λ��sequence�ַ���
                    string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                    //ָ����ʽ�ĵ���
                    Number = Head + Numbertype + Nowmonth + Nowday + sequenceNumber;
                }
                else
                {
                    //���ݿ�����Ϊ1
                    int new_sequence = 1;
                    //�������ݿ�
                    Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                    //����ָ��λ��sequence�ַ���
                    string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                    Number = Head + Numbertype + Nowmonth + Nowday + sequenceNumber;
                }
            }
            else if (NoNowyear > _lastyear)
            {
                //���ݿ�����Ϊ1
                int new_sequence = 1;
                //�������ݿ�
                Update(NoNowyear, NoNowmonth, new_sequence, ModuleName);
                //����ָ��λ��sequence�ַ���
                string sequenceNumber = PatchZero(new_sequence.ToString(), seqLength);
                Number = Head + Numbertype + Nowmonth + Nowday + sequenceNumber;
            }
            return Number;
        }

        #endregion

        #region
        /// <summary>
        /// ��0����
        /// </summary>
        /// <param name="_sequence">Ҫ������</param>
        /// <param name="seqLength">Ҫ���ĳ���</param>
        /// <returns>SequenceNumber</returns>
        public static string PatchZero(string _sequence, int seqLength)
        {
            string SequenceNumber = null;
            //�ж�_sequence�ĳ����Ƿ���������ļ�Ҫ��ĳ��ȣ������ϳ�����0����
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
        /// ��������
        /// </summary>
        /// <param name="LastYear">���</param>
        /// <param name="LastMonth">�·�</param>
        /// <param name="Sequence">���</param>
        /// <param name="ModuleName">ģ����</param>
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
            //ִ�и���
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        #endregion

        #region
        /// <summary>
        /// �������к�
        /// </summary>
        /// <param name="Sequence">���к�</param>
        /// <param name="ModuleName">ģ����</param>
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
            //ִ�и���
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        #endregion
    }
}