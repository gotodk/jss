using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.DB;
using System.Data.SqlClient;

public partial class Web_LeftContentTree : System.Web.UI.Page
{
    private string dept = "";
    private string station = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "select BM,GWMC from HR_Employees where Number='" + User.Identity.Name + "'";
        SqlDataReader sdr = DbHelperSQL.ExecuteReader(sql);
        while (sdr.Read())
        {
            dept = sdr["BM"].ToString();
            station = sdr["GWMC"].ToString();
        }
        sdr.Close();


        string mode = Request.QueryString["mode"];
        switch (mode)
        {
            case "getTree":
                Response.Write(getTree());
                Response.End();
                break;
            //case "getAdd":
            //    Response.Write(getAdd());
            //    break;
            //case "saveAdd":
            //    Response.Write(saveAdd());
            //    break;
            //case "getEdit":
            //    Response.Write(getEdit());
            //    break;
            //case "saveEdit":
            //    Response.Write(saveEdit());
            //    break;
            //case "addSub":
            //    Response.Write(addSub());
            //    break;
            //case "del":
            //    Response.Write(del());
            //    break;
            //case "move":
            //    Response.Write(move());
            //    break;
            //case "returnEdit":
            //    Response.Write(returnEdit());
            //    break;
        }
    }


    //另一种菜单

    public string getTree()
    {
        string fatherID = Request.QueryString["fatherID"];
        if (!IsNumber(fatherID))
        {
            return "禁止提交非法的数据！";
        }

        string StrSql = "SELECT * FROM system_ModuleGroup where parentid =" + fatherID + " and (show is null or show <> '隐藏') and id in (select LMID from LMQXB " +
" where ([TYPE]='1' and [Role]='" + station + "') " +
" or([TYPE]='2' and [Role]='" + dept + "') " +
" or ([TYPE]='3' and [Role]='" + User.Identity.Name + "') " +
" or([role]='全公司')) order by px,id ASC";

        try
        {
            DataSet Dr = DbHelperSQL.Query(StrSql);
            string TopStr = "<a id='add" + fatherID + "'></a><table border='0' cellpadding='0' cellspacing='0' style='width: 100%; height: 10px; background-color:#ffffff;'>";
            string BottomStr = "</table>";
            string MiddleStr = "";

            //遍历目录
            for (int i = 0; i < Dr.Tables[0].Rows.Count; i++)
            {
                string bgst = "";
                if (i % 2 == 0)
                {
                    bgst = "background:#daedf3;";
                }
                else
                {
                    bgst = "background:#EDF7FA;";
                }


                MiddleStr = MiddleStr + "<tr ><td align='left' valign='middle' style='cursor:hand;width:500px;height:22px;" + bgst + "' colspan='2'  onmouseout=\"this.style.border='0px solid #86A4E1';\"     onmouseover=\"this.style.border='1px solid #86A4E1';\"  onclick=getdata('LeftContentTree.aspx?mode=getTree&fatherID=" + Dr.Tables[0].Rows[i]["id"].ToString() + "','sub" + Dr.Tables[0].Rows[i]["id"].ToString() + "');ShowSub(" + Dr.Tables[0].Rows[i]["id"].ToString() + ");><span id='addSub" + Dr.Tables[0].Rows[i]["id"].ToString() + "'>";
                string StrSql2 = "SELECT * FROM system_ModuleGroup where parentid =" + Dr.Tables[0].Rows[i]["id"].ToString() + " order by px,id ASC";
                try
                {
                    DataSet Dr2 = DbHelperSQL.Query(StrSql2);
                    //string kg_edit = "onmousemove=if(g_CatchDiv&&g_objNO!='" + Dr["id"].ToString() + "'){if(confirm('你确定将〖'+g_objName+'〗移动为〖" + moveJS(Dr["Name"].ToString()) + "〗的子级吗？'))postMove(g_objNO," + Dr["id"].ToString() + ")} oncontextmenu=showMenu(" + Dr["ID"].ToString() + "," + fatherID + ");";
                    string kg_edit = "";

                    if (Dr.Tables[0] != null && Dr.Tables[0].Rows != null && Dr.Tables[0].Rows.Count > 0)
                    {

                        MiddleStr = MiddleStr + "<img alt='' id='AddImg" + Dr.Tables[0].Rows[i]["id"].ToString() + "' src='imgx/addImg.gif'  /><a id='move" + Dr.Tables[0].Rows[i]["id"].ToString() + "' ><img src='imgx/zk.gif' style='display:none;' /><span id='edit" + Dr.Tables[0].Rows[i]["id"].ToString() + "' style='font-size:12px; color:' >" + moveHtml(Dr.Tables[0].Rows[i]["title"].ToString()) + "</span></a>";
                    }
                    else
                    {
                        MiddleStr = MiddleStr + "<a id='move" + Dr.Tables[0].Rows[i]["id"].ToString() + "' ><img src='imgx/zk.gif' /><span id='edit" + Dr.Tables[0].Rows[i]["id"].ToString() + "' style='font-size:12px; color:Black;' >" + moveHtml(Dr.Tables[0].Rows[i]["title"].ToString()) + "</span></a>";
                    }

                }
                catch (Exception ex)
                {
                    return "数据库连接失败1！" + ex.ToString();
                }
                MiddleStr = MiddleStr + "</span></td></tr><tr><td align='left' valign='middle' style='width:13px;'></td><td align='left' valign='middle' ><div style='display:none;' id='sub" + Dr.Tables[0].Rows[i]["id"].ToString() + "'></div></td></tr>";
            }

            string StrSql3 = "select distinct a.ModuleName,a.ModuleType,b.title,b.SmallClassId,b.px ";
            StrSql3 = StrSql3 + "from system_auth a,system_modules b ";
            StrSql3 = StrSql3 + "where (a.ModuleName = b.name and ((type=1 and role='" + station + "') ";
            StrSql3 = StrSql3 + "or (type=2 and role='" + dept + "') ";
            StrSql3 = StrSql3 + "or (type=3 and role='" + User.Identity.Name + "') ";
            StrSql3 = StrSql3 + "or (role='全公司') ";
            StrSql3 = StrSql3 + "or ((select count(modulename) from absolutenessModule where modulename = b.name)>0)) )";
            StrSql3 = StrSql3 + "and (b.SmallClassId = " + fatherID + ") ";
            StrSql3 = StrSql3 + "order by  b.px asc,b.title asc";
            DataSet Dr3 = DbHelperSQL.Query(StrSql3);
            //遍历菜单
            for (int i = 0; i < Dr3.Tables[0].Rows.Count; i++)
            {

                string bgst = "";
                if (i % 2 == 0)
                {
                    bgst = "background:#FFFFCC;";
                }
                else
                {
                    bgst = "background:#FFFFDF;";
                }
                string Url = "";
                switch (Dr3.Tables[0].Rows[i]["ModuleType"].ToString())
                {
                    case "1":
                        Url = "WorkFlow_Add.aspx";
                        break;
                    case "2":
                        Url = "TextReport.aspx";
                        break;
                    case "3":
                        Url = "ChartReport.aspx";
                        break;
                    case "4":
                        Url = "CustormModule.aspx";
                        break;
                }
                MiddleStr = MiddleStr + "<tr ><td align='left' valign='middle' name='dddddddd' style='cursor:hand;width:500px;height:22px;" + bgst + "' colspan='2'   onmouseout=\"if(this.style.border!='red 1px solid'){this.style.border='0px solid #6699CC';}\"     onmouseover=\"if(this.style.border!='red 1px solid'){this.style.border='1px solid #6699CC';}\"    onclick=\"for(var i=0; i < document.getElementsByTagName('td').length; i++){if(document.getElementsByTagName('td')[i].getAttribute('name')=='dddddddd'){document.getElementsByTagName('td')[i].style.border='0px solid #B0FFB0';}}this.style.border='1px solid red';OnDataClick('"+moveJS(Dr3.Tables[0].Rows[i]["ModuleName"].ToString())+"','"+ moveHtml(Dr3.Tables[0].Rows[i]["title"].ToString())+"');\"><span  Module='Module' id='addSub" + Dr3.Tables[0].Rows[i]["ModuleName"].ToString() + "'>";
                try
                {
                    string kg_edit = "";


                    MiddleStr = MiddleStr + "<a id='move" + Dr3.Tables[0].Rows[i]["ModuleName"].ToString() + "'  ><img src='imgx/zk.gif' /><span id='edit" + Dr3.Tables[0].Rows[i]["ModuleName"].ToString() + "' style='font-size:12px; color:Black;' >" + moveHtml(Dr3.Tables[0].Rows[i]["title"].ToString()) + "</span></a>";

                }
                catch (Exception ex)
                {
                    return "数据库连接失败2！" + ex.ToString();
                }
                MiddleStr = MiddleStr + "</span></td></tr><tr><td align='left' valign='middle' style='width:13px;'></td><td align='left' valign='middle' ><div style='display:none;' id='sub" + Dr3.Tables[0].Rows[i]["ModuleName"].ToString() + "'></div></td></tr>";
            }


            return TopStr + MiddleStr + BottomStr;
        }
        catch (Exception ex)
        {
            return "数据库连接失败3！" + ex.ToString();
        }
    }


    //**************************
    //**  判断是否是数字组合  **
    //**************************
    public bool IsNumber(string strDate)
    {
        if (strDate == null)
        {
            return false;
        }
        if (strDate.Equals(string.Empty))
        {
            return false;
        }

        Regex numRegex = new Regex(@"0*[0-9][0-9]*$");
        return numRegex.IsMatch(strDate);
    }

    //******************
    //**  过滤JS代码  **
    //******************
    public string moveJS(string strString)
    {
        string str;
        str = strString;
        if (str != null)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace(((char)34).ToString(), "\\\"");
            str = str.Replace(((char)39).ToString(), "\\'");
            str = str.Replace(((char)13).ToString(), "\\n");
            str = str.Replace(((char)10).ToString(), "\\r");
            str = str.Replace("'", "&#39;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace(((char)9).ToString(), "&nbsp;");
            str = str.Replace("<", "&lt");
            str = str.Replace(">", "&gt");
        }
        return str;
    }

    //********************
    //**  过滤HTML代码  **
    //********************
    public string moveHtml(string strString)
    {
        string str;
        str = strString;
        if (str != null)
        {
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt");
            str = str.Replace(">", "&gt");
            str = str.Replace("|", "");
            str = str.Replace(((char)9).ToString(), "&nbsp;");
            str = str.Replace(((char)34).ToString(), "&quot;");
            str = str.Replace(((char)39).ToString(), "&#39;");
            str = str.Replace(((char)13).ToString() + ((char)10).ToString(), "<br>");
        }
        return str;
    }

    //******************
    //**  过滤'"符号  **
    //******************
    public string MoveBidStr(string strString)
    {
        string str;
        str = strString;
        if (str != null)
        {
            str = str.Replace("'", "''");
            str = str.Replace(((char)39).ToString(), ((char)39 + (char)39).ToString());
        }
        return str;
    }
}