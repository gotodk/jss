using FMipcClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pingtaiservices_moban_BZCNS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Params["Number"] != null && Request.Params["Number"].ToString() != "" && Request.Params["DLYX"] != null && Request.Params["DLYX"].ToString() != "")
            {
                ////当前买卖是一家
                //string strMMSYJ = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',T_MJCNSQDSJ '卖家承诺书签订时间',isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',Y_MJCNSQDSJ '买家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号' from  dbo.AAA_ZBDBXXB where Number='" + Request.Params["Number"].ToString().Trim() + "' and T_YSTBDDLYX='" + Request.Params["DLYX"].ToString().Trim() + "' and Y_YSYDDDLYX='" + Request.Params["DLYX"].ToString().Trim() + "'";
                //DataTable dataTableMMSYJ = DbHelperSQL.Query(strMMSYJ).Tables[0];
                ////当前是卖家
                //string strDQSSeller = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',T_MJCNSQDSJ '卖家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号' from  dbo.AAA_ZBDBXXB where Number='" + Request.Params["Number"].ToString().Trim() + "' and T_YSTBDDLYX='" + Request.Params["DLYX"].ToString().Trim() + "' ";
                //DataTable dataTableDQSSeller = DbHelperSQL.Query(strDQSSeller).Tables[0];
                ////当前是买家
                //string strDQSBuyer = "select Number,isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',Y_MJCNSQDSJ '买家承诺书签订时间',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号'  from  dbo.AAA_ZBDBXXB where Number='" + Request.Params["Number"].ToString().Trim() + "' and Y_YSYDDDLYX='" + Request.Params["DLYX"].ToString().Trim() + "' ";
                //DataTable dataTableDQSBuyer = DbHelperSQL.Query(strDQSBuyer).Tables[0];

                DataTable dataTableMMSYJ = null;
                DataTable dataTableDQSSeller = null;
                DataTable dataTableDQSBuyer = null;

               object[] re =  IPC.Call("保密承诺书获取中标定标表的信息", new object[] { Request.Params["Number"].ToString().Trim(), Request.Params["DLYX"].ToString().Trim() });
               if (re[0].ToString() == "ok")
               {
                   //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。                   
                   if (re[1] != null && ((DataSet)re[1]).Tables.Count > 1 && ((DataSet)re[1]).Tables["返回值单条"].Rows[0]["执行结果"] == "ok")
                   {
                       dataTableMMSYJ = ((DataSet)re[1]).Tables["买卖一家"];
                       dataTableDQSSeller = ((DataSet)re[1]).Tables["卖家"];
                       dataTableDQSBuyer = ((DataSet)re[1]).Tables["买家"];
                   }
               }
               else
               {
                   //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                   string reFF = re[1].ToString();
               }

                string oldStr = "";
                string newStr = "";
                string strJYFMC = "";

                string strSFQDCNS = "";//是否签订承诺书
                DateTime dateQDSJ=DateTime.Now;//承诺书签订时间

              
                if (dataTableMMSYJ != null && dataTableMMSYJ.Rows.Count > 0)  //当前买卖是一家
                {
                    strSFQDCNS = dataTableMMSYJ.Rows[0]["卖家是否签订承诺书"].ToString();
                    object obj = dataTableMMSYJ.Rows[0]["卖家承诺书签订时间"];
                    if (obj.ToString()=="")
                    {
                        dateQDSJ = DateTime.Now;
                    }
                    else
                    {
                        dateQDSJ = Convert.ToDateTime(dataTableMMSYJ.Rows[0]["卖家承诺书签订时间"].ToString());
                    }
                }
                else if (dataTableDQSSeller != null && dataTableDQSSeller.Rows.Count>0)  //当前是卖家
                {
                  strSFQDCNS = dataTableDQSSeller.Rows[0]["卖家是否签订承诺书"].ToString();
                  object obj = dataTableDQSSeller.Rows[0]["卖家承诺书签订时间"];
                  if (obj.ToString()=="")
                    {
                        dateQDSJ = DateTime.Now;
                    }
                    else
                    {
                        dateQDSJ = Convert.ToDateTime(dataTableDQSSeller.Rows[0]["卖家承诺书签订时间"].ToString());
                    }
                }
                else if (dataTableDQSBuyer != null && dataTableDQSBuyer.Rows.Count > 0)//当前是买家
                {
                    strSFQDCNS = dataTableDQSBuyer.Rows[0]["买家是否签订承诺书"].ToString();
                    object obj = dataTableDQSBuyer.Rows[0]["买家承诺书签订时间"];
                    if (obj.ToString()=="")
                    {
                        dateQDSJ = DateTime.Now;
                    }
                    else
                    {
                        dateQDSJ = Convert.ToDateTime(dataTableDQSBuyer.Rows[0]["买家承诺书签订时间"].ToString());
                    }
                
                }

                //strJYFMC = DbHelperSQL.Query("select isnull(I_JYFMC,'') '交易方名称'  from AAA_DLZHXXB where B_DLYX='" + Request.Params["DLYX"].ToString().Trim() + "'").Tables[0].Rows[0]["交易方名称"].ToString();//交易方名称

                re = IPC.Call("用户基本信息", new object[] { Request.Params["DLYX"].ToString().Trim() });
                if (re[0].ToString() == "ok")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。                   
                    if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count>0)
                    {
                        strJYFMC = ((DataSet)re[1]).Tables[0].Rows[0]["交易方名称"].ToString();
                    }
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                    string reFF = re[1].ToString();
                }

                oldStr = BuildHtm(HttpContext.Current.Server.MapPath("/pingtaiservices/moban/BZCNS.htm"));
                newStr = oldStr.Replace("CNF", strJYFMC).Replace("YYYY", dateQDSJ.Year.ToString()).Replace("MM", dateQDSJ.Month.ToString()).Replace("dd", dateQDSJ.Day.ToString());
                Response.Write(newStr);
                Response.End();
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