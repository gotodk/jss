using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMDBHelperClass;
using System.Collections;
using FMipcClass;

public partial class pingtaiservices_moban_FMPTJJRZHKTXY : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string oldStr = "";
            string newStr = "";
            if (Request.Params["Action"] != null && Request.Params["Action"].ToString() == "KT_ZQ")//账户开通之前查看协议
            {
                oldStr = BuildHtm(HttpContext.Current.Server.MapPath("/pingtaiservices/moban/FMPTJJRZHKTXY.htm"));
                newStr = oldStr.Replace("[YF]", "").Replace("[YYYY-MM-dd]", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                Response.Write(newStr);
                Response.End();
            }
            else if (Request.Params["Action"] != null && Request.Params["Action"].ToString() == "KT_ZH")
            {
                if (Request.Params["Number"] != null && Request.Params["Number"].ToString() != "")
                {
                   object[] re =  IPC.Call("获取交易方名称及资料提交时间", new object[] { Request.Params["Number"].ToString() });
                   if (re[0].ToString() == "ok")
                   {
                       //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                       if (re[1] != null && ((DataSet)re[1]).Tables.Count > 1 && ((DataSet)re[1]).Tables["用户信息"].Rows.Count > 0)
                       {
                           DataTable dataTable = ((DataSet)re[1]).Tables[1];
                           oldStr = BuildHtm(HttpContext.Current.Server.MapPath("/pingtaiservices/moban/FMPTJJRZHKTXY.htm"));
                           newStr = oldStr.Replace("[YF]", dataTable.Rows[0]["交易方名称"].ToString()).Replace("[YYYY-MM-dd]", dataTable.Rows[0]["资料提交时间"].ToString() + "&nbsp;&nbsp;");
                           Response.Write(newStr);
                           Response.End();
                       }
                       else
                       {
                           Response.Write("获取数据失败");
                           Response.End();
                       }
                   }
                   else
                   {
                       Response.Write((string)re[1]);
                       Response.End();
                   }

                   
                    //DataTable dataTable = DbHelperSQL.Query("select I_JYFMC '交易方名称',convert(varchar(10),I_ZLTJSJ,120) '资料提交时间' from dbo.AAA_DLZHXXB where Number='" + Request.Params["Number"].ToString() + "' ").Tables[0];
                  //oldStr = BuildHtm(HttpContext.Current.Server.MapPath("FMPTJJRZHKTXY.htm"));
                  //newStr = oldStr.Replace("[YF]", dataTable.Rows[0]["交易方名称"].ToString()).Replace("[YYYY-MM-dd]",  dataTable.Rows[0]["资料提交时间"].ToString()+"&nbsp;&nbsp;" );
                  //Response.Write(newStr);
                  //Response.End();

                
                }

            }



        }
    }


    /// <summary>
    /// 根据模板读取数据库内容，无需创建其他列表，直接创建html
    /// </summary>
    /// <param name="strTmplPath">网页模板文件的路径</param>
    public string BuildHtm(string strTmplPath)
    {
        //取模板文件的内容
        System.Text.Encoding code = System.Text.Encoding.GetEncoding("gb2312");
        StreamReader sr = null;
        string str = "";
        try
        {
            sr = new StreamReader(strTmplPath, code);
            str = sr.ReadToEnd(); // 读取文件
            sr.Close();
        }
        catch (Exception exp)
        {
            return exp.ToString();
            sr.Close();
        }
        //string htmlfilename = this.GetFileSaveName(strEnName);//通过英文名获取保存后的文件名
        //替换变量标签

        //string strNew=str.Replace("[$nameChs$]", strChsName);
        //string str = str;
        //strNew = strNew.Replace("[$areaContect$]", "新的文本");//替换左侧页面导航
        return str;
    }
}