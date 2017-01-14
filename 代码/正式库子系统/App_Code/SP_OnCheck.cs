using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.DB;
using FMOP.XParse;
using System.Data.SqlClient;
using System.Xml;
using Mailer.Components;
using System.Net.Mail;
using System.IO;
using System.Text;


namespace FMOP.SP_OnChecks
{
    /// <summary>
    /// SP_OnCheck 的摘要说明
    /// </summary>
    public class SP_OnCheck
    {
        public SP_OnCheck()
        {

        }
        /// <summary>
        /// 执行审批后的存储过程
        /// </summary>
        /// <param name="modeName">模块名称</param>
        /// <param name="KeyNumber">单号</param>
        public static void do_oncheck(string modeName, string KeyNumber)
        {
            try
            {
                string sp_Name = "";
                string sqlCmd = "SELECT Configuration.query('/WorkFlowModule') from system_Modules where name ='" + modeName + "'";
                string strXml = DbHelperSQL.GetSingle(sqlCmd).ToString();
                XmlNode resultNode = xmlParse.XmlFirstNode(strXml, "//SP_OnCheck");
                if (resultNode != null)
                {
                    sp_Name = xmlParse.XmlFirstNode(strXml, "//SP_OnCheck").InnerXml;
                }
                if (sp_Name != "")
                {
                    SqlParameter[] spParameter = new SqlParameter[1];
                    spParameter[0] = new SqlParameter("@Number", SqlDbType.VarChar, 20, "Number");
                    spParameter[0].Value = KeyNumber;
                    //spParameter[0].Direction = ParameterDirection.Input;
                    int aa;
                    DbHelperSQL.RunProcedure(sp_Name,spParameter); 
                    
                    //spInfo = MakeObj(sp_Name, spParameter, CommandType.StoredProcedure);
				}
				#region 单独模块
				if (modeName.Equals("OnLineOrder"))
				{
					string sql = "select dzyj1 from OnLineOrder where number='" + KeyNumber + "'";
					object ob = DbHelperSQL.GetSingle(sql);
					if (ob == null)
						return;
					string email = ob.ToString();

					SMTP sm = new SMTP();
					sm.Send(email, "你在富美网站的订单已经审核", setbody(KeyNumber), MailPriority.High, true);
					
				}

				#endregion 单独模块


				//return spInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		/// <summary>
		/// 订单内容
		/// </summary>
		/// <returns></returns>
		private static string setbody(string number)
		{
			StringBuilder body = new StringBuilder();
			try
			{
				//读取HTML模板，即发送的页面
				string strPath = System.Web.HttpContext.Current.Server.MapPath("~/Web/orderchechmail.htm");
				//读取文件，“System.Text.Encoding.Default”可以解决中文乱码问题
				StreamReader sr = new StreamReader(strPath, System.Text.Encoding.Default);

				body.Append(sr.ReadToEnd());
				//关闭文件流
				sr.Close();
			}
			catch (Exception e)
			{ }
			#region 替换指定内容，通常为需要变动的内容


			string xm="";
			string yaoqiu = "";
			string tablerow="";
			string strsql = "select shrxm,fkfs,jsfs,hj,case when checkstate=0 then '正在审批中' when checkstate=1 then '审批通过' when checkstate=2 then '审批驳回' end as checkstate,createtime,PSSDZYSX from OnLineOrder where number='"+number+"'";
			SqlDataReader sdr = DbHelperSQL.ExecuteReader(strsql);
			if (sdr.Read())
			{
				xm = sdr["shrxm"].ToString();
				yaoqiu = sdr["PSSDZYSX"].ToString();
				tablerow = "<tr class=\"grid_border\"><td class=\"grid_border\" style=\"color:#3366CC;\">" + number + "</td>" +
															"<td class=\"grid_border\">"+xm+"</td>" +
															"<td class=\"grid_border\">" + sdr["fkfs"].ToString() + "</td>" +
															"<td class=\"grid_border\">" + sdr["jsfs"].ToString() + "</td>" +
															"<td class=\"grid_border\"><span style=\"color:#e54c8d\">￥" + sdr["hj"].ToString() + "</span></td>" +
															"<td class=\"grid_border\"><span style=\"color:#e54c8d\">" + sdr["checkstate"].ToString() + "</span></td>" +
															"<td class=\"grid_border\"><span>" + sdr["createtime"].ToString() + "</span></td></tr>";
			}
			sdr.Close();
			//单位
			body = body.Replace("{dept}", string.Empty);
			//名称
			body = body.Replace("{username}", xm);
			//订单号
			body = body.Replace("{number}", number);
			//订单内容
			body = body.Replace("{tablerows}", tablerow);
			//配送要求
			body = body.Replace("{peisong}", yaoqiu);
			//积分
			body = body.Replace("{jifen}", "0");

			#endregion 替换指定内容，通常为需要变动的内容

			return body.ToString().Trim();

		}
    }
}