using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Telerik.WebControls;
using System.Data.SqlClient;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using FMOP.DB;
using System.Text;

public partial class Web_Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql_a = "select number,KHMC,XSQD,SSHY,SSXSQY from KHGL_New where number not like '%DY%'";
        DataSet dsd = DbHelperSQL.Query(sql_a);

        for(int i = 0; i < dsd.Tables[0].Rows.Count;i++)
        { //CheckState  CreateUser
            WorkFlowModule WFM = new WorkFlowModule("KHGL_KHMMGL");
            string KeyNumber = WFM.numberFormat.GetNextNumber();

            string KHBH = dsd.Tables[0].Rows[i]["number"].ToString();
            string KHMC = dsd.Tables[0].Rows[i]["KHMC"].ToString();
            string XSQD = dsd.Tables[0].Rows[i]["XSQD"].ToString();
            string SSHY = dsd.Tables[0].Rows[i]["SSHY"].ToString();
            string SSXSQY = dsd.Tables[0].Rows[i]["SSXSQY"].ToString();
            DbHelperSQL.Query("insert KHGL_KHMMGL (number,KHBH,KHMM,KHMC,XSQD,SSHY,SSBSC,CheckState,CreateUser) values ('" + KeyNumber + "','" + KHBH + "','" + generatepassword(6) + "','" + KHMC + "','" + XSQD + "','" + SSHY + "','" + SSXSQY + "',1,'admin')");
        }

        
    }


//// <summary> 
    /// 生成随机字母与数字 
    /// </summary> 
    ///  <param name="Length">生成长度</param> 
    ///  <returns></returns> 
        public static string generatepassword(int Length) 
        { 
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' }; 
            string result = ""; 
            int n = Pattern.Length; 
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks)); 
            for (int i = 0; i < Length; i++) 
            { 
                int rnd = random.Next(0,n);
                result += Pattern[rnd]; 
            } 
            return result; 
        } 
}
