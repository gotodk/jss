using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pingtaiservices_moban_cs1 : System.Web.UI.Page
{
 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["moban"] == null || Request["pagenumber"] == null || Request["htbh"] == null)
        {
            Response.Write("系统错误");
            Response.End();
            return;
        }
        string moban = Request["moban"].ToString();//模拟模板名称
        string htbh = Request["htbh"].ToString();//模拟合同编号
        string oldstr = BuildHtm(HttpContext.Current.Server.MapPath(moban));
        string newstr = "";
        //开始替换模板
        newstr = oldstr.Replace("[$电子购货合同编号$]", htbh);

        Response.Write(newstr);
        Response.End();
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