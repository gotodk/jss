using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Data.SqlClient;
using FMOP.DB;
using FMOP.XHelp;
using Hesion.Brick.Core;

public partial class WorkFlow_Delete : System.Web.UI.Page
{
    private string module;
    private string number;

    private void Page_Load(object sender, System.EventArgs e)
    {
        if (Request["module"] != null && Request["module"].ToString() != string.Empty)
        {
            module = Request["module"].ToString();
            if (Request["number"] != null && Request["number"].ToString() != string.Empty)
            {
                number = Request["number"].ToString();
				int xmlp = xmlParse();
				if (xmlp == 10)
				{
					Response.Clear();
					Response.Write("<script language=javascript>window.alert('您没有删除权限！');window.close();</script>");
					Response.End();
					return;
				}
				else if (xmlp != 1)
				{
					Response.Clear();
					Response.Write("<script language=javascript>window.alert('删除失败！');window.close();</script>");
					Response.End();
					return;
				}
				else
				{
					Response.Clear();
					Response.Write("<script language=javascript>window.close();</script>");
					Response.End();
					return;
				}
            }
            else
            {
                Response.Clear();
                Response.Write("<script language=javascript>window.alert('没有获取到单号参数！');window.close();</script>");
                Response.End();
                return;
            }
        }
        else
        {
            Response.Clear();
            Response.Write("<script language=javascript>window.alert('没有获取到模块参数！');window.close();</script>");
            Response.End();
            return;
        }

    }

    /// <summary>
    /// 读取//WorkFlowModule/Data/Data-Field节点属性
    /// </summary>
    /// <returns>0:数据库没有xml文档记录</returns>
	protected int xmlParse()
	{
		if (!getPepo())
		{
			return 10;	
		}
		string sp_delete = FMOP.XParse.xmlParse.XmlGetNodeInnerXml(module, "//WorkFlowModule/SP_OnDelete");
		int bl = 0;
		if (sp_delete != null&& sp_delete != string.Empty)
		{
			SqlParameter[] sqlp = new SqlParameter[1];
			sqlp[0] = new SqlParameter("Number", SqlDbType.VarChar,5000);
			sqlp[0].Value = number;
			int aa;
			bl = FMOP.DB.DbHelperSQL.RunProcedure(sp_delete, sqlp, out aa);
		}
		if (bl >= 0)
		{
			XmlNodeList xlist = FMOP.XParse.xmlParse.XmlGetNode(module, "//WorkFlowModule/Data/Data-Field");
			if (xlist != null)
			{
				ArrayList al = new ArrayList();
				foreach (XmlNode item in xlist)
				{
					#region 子表
					if (XMLHelper.GetSingleString(item, "type", "").Equals("SubTable"))
					{
						string deleteSql = "delete from " + XMLHelper.GetSingleString(item, "name", "") + " where parentNumber='" + number + "'";
						al.Add(deleteSql);
					}
					#endregion


				}
				#region 上传文件
				string upsql = "delete from fileup where number ='" + number + "'";
				al.Add(upsql);
				#endregion

				#region 主表
				string sqlD = "delete from " + module + " where number = '" + number + "'";
				al.Add(sqlD);
				#endregion

				Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
                if (wf.warning != null)
                {
                    wf.warning.CreateWarningToRole(number, 3, User.Identity.Name);
                }
				//FMOP.WN.Warning.CreateWarning(module, number, 3, User.Identity.Name);


				DbHelperSQL.ExecuteSqlTran(al);

                string dailiren = "无";
                if (Session["daililogin"] != null)
                {
                    dailiren = Session["daililogin"].ToString();
                }
                FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_Delete.aspx", number, "", dailiren);

				return 1;
			}
			else
			{
				return 0;
			}
		}
		else
		{
			return 0;
		}
	}


	/// <summary>
	/// 判断当前用户是否是系统管理员，只有系统管理员有删除权限。
	/// </summary>
	/// <returns></returns>
	protected bool getPepo()
	{
		string sqlstr = "select count(1) from hr_employees where gwmc='系统管理员' and number='" + User.Identity.Name + "'";
		int ret = Convert.ToInt32(DbHelperSQL.GetSingle(sqlstr));
		if (ret == 0)
			return false;
		return true;
	}
}
