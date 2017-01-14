using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using RTXServerApi;
using RTXClient;
using RTXCAPILib;
using FMOP.DB;
using System.Data.SqlClient;
using FMOP.Module;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;


public partial class tq : System.Web.UI.Page
{

    /// <summary>
    /// 把数据文件，放到哈希表中
    /// </summary>
    /// <returns></returns>
    private NoSortHashTable citytxtTdt()
    {


        NoSortHashTable htcitymain = new NoSortHashTable();
        string path = Server.MapPath("citytxt.txt");
        try
        {
            Encoding ascii = Encoding.Default;
            using (StreamReader sr = new StreamReader(path, ascii))
            {
                string now_sheng = "";
                while (sr.Peek() >= 0)
                {
                    string strtemp = sr.ReadLine().Trim();
                    if (strtemp!="")
                    {
                        //写入主哈希表
                        if (strtemp.StartsWith("["))
                        {
                            now_sheng = strtemp.Replace("[","");
                            NoSortHashTable htcity = new NoSortHashTable();
                            htcitymain.Add(now_sheng, htcity);
                        }
                        else //写入子哈希表
                        {
                            string number = strtemp.Split('=')[0];
                            string city = strtemp.Split('=')[1];
                            ((NoSortHashTable)(htcitymain[now_sheng])).Add(city, number);
                        }
                    }
                   
                }
            }
            return htcitymain;
        }
        catch (Exception ex)
        {
            return null;
        }



        
    }


    /// <summary>
    /// 通过星期，返回中文星期
    /// </summary>
    /// <param name="dw"></param>
    /// <returns></returns>
    public string Week(DayOfWeek dw)
    {
        string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        string week = weekdays[Convert.ToInt32(dw)];
        return week;
    }

    /// <summary>
    /// 通过星期，返回特殊样式
    /// </summary>
    /// <param name="dw"></param>
    /// <returns></returns>
    public string get_yangshi(DayOfWeek dw)
    {
        if (Week(dw) == "星期六")
        {
            return "style='font-weight: normal; color:#009944'";
        }
        if (Week(dw) == "星期日")
        {
            return "style='font-weight: normal; color:#cc0000'";
        }
        return "style='font-weight: normal; color:#000000'";
    }

    /// <summary>
    /// 返回图片地址，通过白天和晚上的不同列名称进行判断，只用于转换晚上
    /// </summary>
    /// <param name="lieming"></param>
    /// <param name="ds"></param>
    private string getimg(string d_lieming, string n_lieming, DataSet ds)
    {
        string d_bz = ds.Tables[0].Rows[0][d_lieming].ToString();//白天
        string n_bz = ds.Tables[0].Rows[0][n_lieming].ToString();//晚上
        if (n_bz.Trim() == "99" || n_bz.Trim() == "")
        {
            n_bz = d_bz;
        }
        string dizhi = "tianqi/main/a" + n_bz + ".gif";
        return dizhi;
    }

    /// <summary>
    /// 返回温度的样式
    /// </summary>
    /// <param name="lieming"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    private string getwendu(string lieming, DataSet ds)
    {
        string wendu = "";
        string wenduold = ds.Tables[0].Rows[0][lieming].ToString();
        try
        {
            wendu = "<span style='color:#4899d3'>" + wenduold.Split('~')[0] + "</span>～" + "<span style='color:#cc0000'>" + wenduold.Split('~')[1] + "</span>";
        }
        catch
        {
            wendu = wenduold;
        }

        return wendu;
    }

    /// <summary>
    /// 通过城市编号展示天气预报
    /// </summary>
    /// <param name="citystr"></param>
    private void showtq(string citystr)
    {
        DataSet dstq = GetNowTQ(citystr);
        if (dstq != null && dstq.Tables[0].Rows.Count > 0)
        {
            //展示天气信息
            titlemain.Text = dstq.Tables[0].Rows[0]["完整日期"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + dstq.Tables[0].Rows[0]["星期"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + dstq.Tables[0].Rows[0]["城市中文名"].ToString();

            top_tu.ImageUrl = "tianqi/main/a" + dstq.Tables[0].Rows[0]["第一天白天天气图标"].ToString() + ".gif";

            top_tq.Text = dstq.Tables[0].Rows[0]["第一天全天天气"].ToString() + "<br />" + dstq.Tables[0].Rows[0]["第一天摄氏度"].ToString();

            top_other.Text = "24小时穿衣指数：" + dstq.Tables[0].Rows[0]["第一天的穿衣指数"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;48小时穿衣指数：" + dstq.Tables[0].Rows[0]["48小时穿衣指数"].ToString() + "<br />24小时紫外线指数：" + dstq.Tables[0].Rows[0]["第一天紫外线指数"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;48小时紫外线指数：" + dstq.Tables[0].Rows[0]["48小时紫外线指数"].ToString() + "<br />洗车指数：" + dstq.Tables[0].Rows[0]["第一天洗车指数"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;舒适度指数：" + dstq.Tables[0].Rows[0]["第一天舒适度指数"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;晨练指数：" + dstq.Tables[0].Rows[0]["第一天晨练指数"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;<br />晾晒指数：" + dstq.Tables[0].Rows[0]["第一天晾晒指数"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;过敏指数：" + dstq.Tables[0].Rows[0]["第一天过敏气象指数"].ToString();

            DateTime dtnow = Convert.ToDateTime(dstq.Tables[0].Rows[0]["完整日期"].ToString());

            riqi1.Text = dtnow.Month + "月" + dtnow.Day + "日(<span style='font-weight: bolder; color:#000000' >今天</span>)";
            riqi2.Text = dtnow.AddDays(1).Month + "月" + dtnow.AddDays(1).Day + "日(<span " + get_yangshi(dtnow.AddDays(1).DayOfWeek) + " >" + Week(dtnow.AddDays(1).DayOfWeek) + "</span>)";
            riqi3.Text = dtnow.AddDays(2).Month + "月" + dtnow.AddDays(2).Day + "日(<span " + get_yangshi(dtnow.AddDays(2).DayOfWeek) + " >" + Week(dtnow.AddDays(2).DayOfWeek) + "</span>)";
            riqi4.Text = dtnow.AddDays(3).Month + "月" + dtnow.AddDays(3).Day + "日(<span " + get_yangshi(dtnow.AddDays(3).DayOfWeek) + " >" + Week(dtnow.AddDays(3).DayOfWeek) + "</span>)";
            riqi5.Text = dtnow.AddDays(4).Month + "月" + dtnow.AddDays(4).Day + "日(<span " + get_yangshi(dtnow.AddDays(4).DayOfWeek) + " >" + Week(dtnow.AddDays(4).DayOfWeek) + "</span>)";
            riqi6.Text = dtnow.AddDays(5).Month + "月" + dtnow.AddDays(5).Day + "日(<span " + get_yangshi(dtnow.AddDays(5).DayOfWeek) + " >" + Week(dtnow.AddDays(5).DayOfWeek) + "</span>)";


            Image1.ImageUrl = "tianqi/main/a" + dstq.Tables[0].Rows[0]["第一天白天天气图标"].ToString() + ".gif";
            Image2.ImageUrl = getimg("第一天白天天气图标", "第一天夜晚天气图标", dstq);
            Image3.ImageUrl = "tianqi/main/a" + dstq.Tables[0].Rows[0]["第二天白天天气图标"].ToString() + ".gif";
            Image4.ImageUrl = getimg("第二天白天天气图标", "第二天夜晚天气图标", dstq);
            Image5.ImageUrl = "tianqi/main/a" + dstq.Tables[0].Rows[0]["第三天白天天气图标"].ToString() + ".gif";
            Image6.ImageUrl = getimg("第三天白天天气图标", "第三天夜晚天气图标", dstq);
            Image7.ImageUrl = "tianqi/main/a" + dstq.Tables[0].Rows[0]["第四天白天天气图标"].ToString() + ".gif";
            Image8.ImageUrl = getimg("第四天白天天气图标", "第四天夜晚天气图标", dstq);
            Image9.ImageUrl = "tianqi/main/a" + dstq.Tables[0].Rows[0]["第五天白天天气图标"].ToString() + ".gif";
            Image10.ImageUrl = getimg("第五天白天天气图标", "第五天夜晚天气图标", dstq);
            Image11.ImageUrl = "tianqi/main/a" + dstq.Tables[0].Rows[0]["第六天白天天气图标"].ToString() + ".gif";
            Image12.ImageUrl = getimg("第六天白天天气图标", "第六天夜晚天气图标", dstq);

            Label1.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第一天白天天气标题"].ToString() + "</span>";
            Label2.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第二天白天天气标题"].ToString() + "</span>";
            Label3.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第三天白天天气标题"].ToString() + "</span>";
            Label4.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第四天白天天气标题"].ToString() + "</span>";
            Label5.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第五天白天天气标题"].ToString() + "</span>";
            Label6.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第六天白天天气标题"].ToString() + "</span>";

            yj1.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第一天夜晚天气标题"].ToString() + "</span>";
            yj2.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第二天夜晚天气标题"].ToString() + "</span>";
            yj3.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第三天夜晚天气标题"].ToString() + "</span>";
            yj4.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第四天夜晚天气标题"].ToString() + "</span>";
            yj5.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第五天夜晚天气标题"].ToString() + "</span>";
            yj6.Text = "<span style='bolder; color:#000000'>" + dstq.Tables[0].Rows[0]["第六天夜晚天气标题"].ToString() + "</span>";

            wd1.Text = getwendu("第一天摄氏度", dstq);
            wd2.Text = getwendu("第二天摄氏度", dstq);
            wd3.Text = getwendu("第三天摄氏度", dstq);
            wd4.Text = getwendu("第四天摄氏度", dstq);
            wd5.Text = getwendu("第五天摄氏度", dstq);
            wd6.Text = getwendu("第六天摄氏度", dstq);

            fx1.Text = dstq.Tables[0].Rows[0]["第一天的风力风向"].ToString();
            fx2.Text = dstq.Tables[0].Rows[0]["第二天的风力风向"].ToString();
            fx3.Text = dstq.Tables[0].Rows[0]["第三天的风力风向"].ToString();
            fx4.Text = dstq.Tables[0].Rows[0]["第四天的风力风向"].ToString();
            fx5.Text = dstq.Tables[0].Rows[0]["第五天的风力风向"].ToString();
            fx6.Text = dstq.Tables[0].Rows[0]["第六天的风力风向"].ToString();
        }
    }


    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
     
                //默认值
                string now_city = "山东";
                string now_shi = "济南";

                try
                {
                    //获取默认城市
                    HttpCookie cookie_city = Request.Cookies["morencity1"];
                    if (cookie_city != null && cookie_city.Value.Trim() != "")//有默认值
                    {
                        now_city = cookie_city.Value.Split('|')[0];
                        now_shi = cookie_city.Value.Split('|')[1];
                    }
                    else//无默认值
                    {
                        //获取当前城市
                        string SelectSqlForSSBSC = "select tqyb from System_City_0 where [Name] = (select top 1 bm from hr_employees where number='" + User.Identity.Name + "')";
                        string nowcity = now_city;
                        object ob = DbHelperSQL.GetSingle(SelectSqlForSSBSC);
                        if (ob != null)
                        {
                            now_city = ob.ToString();
                            now_shi = "";
                        }
                    }
                }
                catch(Exception exxx){}


                //绑定省份
                NoSortHashTable ht_sheng = citytxtTdt();
                foreach (string str in ht_sheng.Keys)
                {
                    DropDownList1.Items.Add(str);
                }
                //设置默认值
                DropDownList1.SelectedValue = now_city;
                //绑定城市
                NoSortHashTable ht_shi = (NoSortHashTable)(ht_sheng[DropDownList1.SelectedValue]);

                foreach (string str1 in ht_shi.Keys)
                {
                    DropDownList2.Items.Add(str1);
                }
                //设置默认值
                ListItem li = new ListItem(now_shi);
                if (DropDownList2.Items.Contains(li))
                {
                    DropDownList2.SelectedValue = now_shi;
                }
                else
                {
                    DropDownList2.SelectedIndex = 0;
                }
                //显示天气
                showtq(ht_shi[DropDownList2.SelectedValue].ToString());
            }
            catch(Exception eex)
            {
                ;
            }
        }

        //读取近三天的考勤记录，以确定是否进行提醒
        DateTime time = DateTime.Now;
        //string sql = "select top 5 ID, SJKSSJ,SJJSSJ,convert(char(4), JSSJ)+ '-' +substring( JSSJ,5,2)+ '-' +substring( JSSJ,7,2) dt from KQCQ where (convert(smalldatetime ,convert(char(4), JSSJ)+ '-' +substring( JSSJ,5,2)+ '-' " +
        //  " +substring( JSSJ,7,2)+' ' +substring( JSSJ,9,2)+': ' +substring( JSSJ,11,2)+': ' +substring( JSSJ,13,2)) BETWEEN '"
        //  + time.AddDays(-3).ToShortDateString() + "' AND '" + time.ToShortDateString() + "')";

        //从考勤统计里查询是否是正常的工作日
        string sql = "select ID,convert(char(4), JSSJ)+ '-' +substring( JSSJ,5,2)+ '-' +substring( JSSJ,7,2) dt from KQCQ where (convert(smalldatetime ,convert(char(4), JSSJ)+ '-' +substring( JSSJ,5,2)+ '-' " +
           " +substring( JSSJ,7,2)+' ' +substring( JSSJ,9,2)+': ' +substring( JSSJ,11,2)+': ' +substring( JSSJ,13,2)) BETWEEN '"
           + time.AddDays(-3).ToShortDateString() + "' AND '" + time.ToShortDateString() + "')"
           + " and EMPNo='" + User.Identity.Name + "' and convert(char(8), JSSJ) not in (select HOLDATE from HOLIDAY)";
        SqlDataReader reader = DbHelperSQL.GetKQ(sql);
        while (reader.Read())
        {
            //从考勤记录表里查询统计每天的上下班时间
            string str = "select substring(max(sktime),9,2)a,substring(min(sktime),9,2)i,max(convert(char(4), sktime)+ '-' +substring( sktime,5,2)+ '-'" +
                " +substring( sktime,7,2)+' ' +substring( sktime,9,2)+': ' +substring( sktime,11,2))mx,min(convert(char(4), sktime)+ '-' +substring( sktime,5,2)+ '-' " +
                " +substring( sktime,7,2)+' ' +substring( sktime,9,2)+': ' +substring( sktime,11,2)) ni from YSSKKQ_xxx  where empno='" + User.Identity.Name + "' and  " +
                " convert(smalldatetime ,convert(char(4), sktime)+ '-' +substring( sktime,5,2)+ '-' " +
                " +substring( sktime,7,2)+' ' +substring( sktime,9,2)+': ' +substring( sktime,11,2)+': ' +substring( sktime,13,2))>='" + reader["dt"].ToString() + "' " +
                " and convert(smalldatetime ,convert(char(4), sktime)+ '-' +substring( sktime,5,2)+ '-'  +substring( sktime,7,2)+' ' +substring( sktime,9,2)+': ' " +
                " +substring( sktime,11,2)+': ' +substring( sktime,13,2))<'" + Convert.ToDateTime(reader["dt"].ToString()).AddDays(1).ToString()
                + "'";
            SqlDataReader r = DbHelperSQL.GetKQ(str);
            if (r.Read())
            {
                //mx为下班时间，ni为上班时间,为空说明当天未打卡，两者相等说明只打了一次卡，只是上班或者下班打卡
                if ((r["mx"] == DBNull.Value) || (r["ni"] == DBNull.Value) || (r["mx"].ToString() == r["ni"].ToString()) || (Convert.ToInt32(r["i"]) > 12) || ((Convert.ToInt32(r["a"]) < 12)))
                {
                    //该记录是否已经写到提醒表里
                    string strsql = "select Id from User_Warnings where Module_Url like '%Id=" + reader["ID"].ToString() + "'";
                    int Rcount = DbHelperSQL.QueryInt(strsql);
                    if (Rcount == 0)
                    {
                        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule("SJHZZGX");
                        //wf.authentication.InsertWarnings("有新的个人信息自助更新，工号为" + User.Identity.Name + "，请及时查看处理。", "WorkFlow_View.aspx?module=HR_Employees_modify", "1", User.Identity.Name.Trim(), touser);
                        wf.authentication.InsertWarnings(reader["dt"].ToString() + "打卡异常(仅提醒前三天)", "HR/Kaoqinguanli/KQDetail.aspx?Id=" + reader["ID"].ToString() + "", "1", "系统", User.Identity.Name.Trim());
                    }
                    r.Close();
                    continue;
                }

            }
            else
            {
                r.Close();
                continue;
            }
            r.Close();
            //string content = "";
            //if (reader["SJKSSJ"] == DBNull.Value || reader["SJJSSJ"] == DBNull.Value)
            //{
            //    content =reader["dt"].ToString()+"打卡异常";
            //}

        }
        reader.Close();

        //galaxy添加，用于自动登录邮箱。.......................................................................................
        //验证是否添加过邮箱账号与工号的对应关系
        //没有添加，跳转到添加页面

        //已经添加过，自动登录邮箱
        string userName = User.Identity.Name;
        string upsql = "select * from YXGHYSB where YGGH='" + userName + "'";
        DataSet ds = DbHelperSQL.Query(upsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["emaillogin"] = "yes";
            string YX_str = ds.Tables[0].Rows[0]["FMYXZHBBHHZ"].ToString();
            Session["mail_url"] = "http://192.168.0.4:8080/default_fm.asp?uname=" + YX_str + "&pword=1";
            //Response.Write("<script>window.parent.frames.f2.location.href = \"http://192.168.0.4:8080/default_fm.asp?uname=" + YX_str + "&pword=1\";</script>");
            //Response.Write("<script>window.open (\"http://192.168.0.4:8080/default_fm.asp\", \"newwindow\", \"height=0, width=0, top=0, left=0, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no\");</script>");
            //Response.Write("http://192.168.0.4:8080/default_fm.asp?uname=" + YX_str + "&pword=1");
            //Response.Write("<script>window.parent.frames.f2.location.href = \"autologinemail.html?un=" + YX_str + "&pw=1\";</script>");

        }
        else
        {
            Session["emaillogin"] = null;
        }



        //Response.Write("<script>window.parent.hideIframe0.location = \"www.google.cn\";</script>");
        //........................................................................................................................

        //Response.Write("<script><javascript:window.open('../RTXntegration/hidertxlogin.aspx?Sessionkey=" + GetSessionKey() + "')></script>");
        //GetSessionKey();

        #region RTX登陆

        /*2009.07.09  王永辉添加 RTX登陆*/

        //try
        //{
        //    if (Session["daililogin"] != null && Session["daililogin"].ToString() != "")
        //    {
        //        RTXSevera obiRtxSever = new RTXSevera();

        //        /*这里需要一个业务操作平台账号跟RTX账号的对应暂时没有*/
        //        if (GetRtxPass() != "")
        //        {
        //            string UserID = GetRtxPass();
        //            string severIP = ConfigurationManager.AppSettings["RtxseverIP"].ToString();
        //            if (!Page.IsStartupScriptRegistered("RTXClientLogin"))
        //            {
        //                Page.RegisterStartupScript("RTXClientLogin", obiRtxSever.GenerateClientLoginScript(UserID, severIP));
        //            }
        //        }
        //        else
        //        {
        //            FMOP.Common.MessageBox.Show(Page, "您的RTX未能登陆，可能是您还未分配RTX账号！");
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //     FMOP.Common.MessageBox.Show(Page, ex+"可能是您还未分配RTX账号！");
        //}

        #endregion
    }


    //private string GetRtxPass()
    //{
    //    string StrRtx = "";
    //    string StrSql = "select RTXZH from YXGHYSB where YGGH = '" + Session["daililogin"].ToString().Trim()+"'";
    //    DataSet ds = DbHelperSQL.Query(StrSql);
    //    if (ds.Tables[0].Rows.Count >0)
    //    {
    //        StrRtx = ds.Tables[0].Rows[0]["RTXZH"].ToString();
    //    }

    //    return StrRtx.Trim();
    //}





    /// <summary>
    /// 根据城市代码获取天气，来源为中国天气网
    /// </summary>
    /// <param name="cityid"></param>
    /// <returns></returns>
    public DataSet GetNowTQ(string cityid)
    {
        try
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.GetEncoding("utf-8");
            //设置缓存
            System.Net.Cache.RequestCachePolicy rcp = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.CacheIfAvailable);
            client.CachePolicy = rcp;
            //设置每小时才更新一次缓存
            string spstr = System.DateTime.Now.Year + "_" + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + "_" + System.DateTime.Now.Hour;
            string reply = client.DownloadString("http://m.weather.com.cn/data/" + cityid + ".html?spstr=" + spstr);
            DataSet ds = GetDataSetformJSON(reply);

            Hashtable htDZB = new Hashtable();
            htDZB["city"] = "城市中文名";
            htDZB["city_en"] = "城市英文名";
            htDZB["date_y"] = "完整日期";
            htDZB["date"] = "date作用未知";
            htDZB["week"] = "星期";
            htDZB["fchh"] = "fchh作用未知";
            htDZB["cityid"] = "城市编号";

            htDZB["temp1"] = "第一天摄氏度";
            htDZB["temp2"] = "第二天摄氏度";
            htDZB["temp3"] = "第三天摄氏度";
            htDZB["temp4"] = "第四天摄氏度";
            htDZB["temp5"] = "第五天摄氏度";
            htDZB["temp6"] = "第六天摄氏度";

            htDZB["tempF1"] = "第一天华氏度";
            htDZB["tempF2"] = "第二天华氏度";
            htDZB["tempF3"] = "第三天华氏度";
            htDZB["tempF4"] = "第四天华氏度";
            htDZB["tempF5"] = "第五天华氏度";
            htDZB["tempF6"] = "第六天华氏度";

            htDZB["weather1"] = "第一天全天天气";
            htDZB["weather2"] = "第二天全天天气";
            htDZB["weather3"] = "第三天全天天气";
            htDZB["weather4"] = "第四天全天天气";
            htDZB["weather5"] = "第五天全天天气";
            htDZB["weather6"] = "第六天全天天气";

            htDZB["img1"] = "第一天白天天气图标";
            htDZB["img2"] = "第一天夜晚天气图标";
            htDZB["img3"] = "第二天白天天气图标";
            htDZB["img4"] = "第二天夜晚天气图标";
            htDZB["img5"] = "第三天白天天气图标";
            htDZB["img6"] = "第三天夜晚天气图标";
            htDZB["img7"] = "第四天白天天气图标";
            htDZB["img8"] = "第四天夜晚天气图标";
            htDZB["img9"] = "第五天白天天气图标";
            htDZB["img10"] = "第五天夜晚天气图标";
            htDZB["img11"] = "第六天白天天气图标";
            htDZB["img12"] = "第六天夜晚天气图标";
            htDZB["img_single"] = "第一天单独图标";

            htDZB["img_title1"] = "第一天白天天气标题";
            htDZB["img_title2"] = "第一天夜晚天气标题";
            htDZB["img_title3"] = "第二天白天天气标题";
            htDZB["img_title4"] = "第二天夜晚天气标题";
            htDZB["img_title5"] = "第三天白天天气标题";
            htDZB["img_title6"] = "第三天夜晚天气标题";
            htDZB["img_title7"] = "第四天白天天气标题";
            htDZB["img_title8"] = "第四天夜晚天气标题";
            htDZB["img_title9"] = "第五天白天天气标题";
            htDZB["img_title10"] = "第五天夜晚天气标题";
            htDZB["img_title11"] = "第六天白天天气标题";
            htDZB["img_title12"] = "第六天夜晚天气标题";
            htDZB["img_title_single"] = "第一天单独标题";

            htDZB["wind1"] = "第一天的风力风向";
            htDZB["wind2"] = "第二天的风力风向";
            htDZB["wind3"] = "第三天的风力风向";
            htDZB["wind4"] = "第四天的风力风向";
            htDZB["wind5"] = "第五天的风力风向";
            htDZB["wind6"] = "第六天的风力风向";

            htDZB["fx1"] = "第一天风向";
            htDZB["fx2"] = "第二天风向";

            htDZB["fl1"] = "第一天风力";
            htDZB["fl2"] = "第二天风力";
            htDZB["fl3"] = "第三天风力";
            htDZB["fl4"] = "第四天风力";
            htDZB["fl5"] = "第五天风力";
            htDZB["fl6"] = "第六天风力";

            htDZB["index"] = "第一天的穿衣指数";
            htDZB["index_d"] = "第一天穿衣指数描述";
            htDZB["index48"] = "48小时穿衣指数";
            htDZB["index48_d"] = "48小时穿衣指数描述";
            htDZB["index_uv"] = "第一天紫外线指数";
            htDZB["index48_uv"] = "48小时紫外线指数";
            htDZB["index_xc"] = "第一天洗车指数";
            htDZB["index_tr"] = "index_tr未知";
            htDZB["index_co"] = "第一天舒适度指数";

            htDZB["st1"] = "st1作用未知";
            htDZB["st2"] = "st2作用未知";
            htDZB["st3"] = "st3作用未知";
            htDZB["st4"] = "st4作用未知";
            htDZB["st5"] = "st5作用未知";
            htDZB["st6"] = "st6作用未知";

            htDZB["index_cl"] = "第一天晨练指数";
            htDZB["index_ls"] = "第一天晾晒指数";
            htDZB["index_ag"] = "第一天过敏气象指数";

            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                string thisname = ds.Tables[0].Columns[i].ColumnName;
                if (htDZB.ContainsKey(thisname))
                {
                    ds.Tables[0].Columns[i].ColumnName = htDZB[thisname].ToString();
                }
            }
            return ds;
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            return null;
        }

    }

    /// <summary>
    /// 通过json格式数据，返回数据集文档
    /// </summary>
    /// <param name="jsonstr"></param>
    /// <returns></returns>
    public DataSet GetDataSetformJSON(string jsonstr)
    {
        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonstr);
        DataSet ds = new DataSet();
        XmlNodeReader reader = new XmlNodeReader(doc);
        ds.ReadXml(reader);
        return ds;
    }


    /// <summary>
    /// 通过json格式数据，返回xml文档
    /// </summary>
    /// <param name="jsonstr"></param>
    /// <returns></returns>
    public string GetJSONformXML(XmlDocument xmldoc)
    {
        string json = JsonConvert.SerializeXmlNode(xmldoc.DocumentElement);
        return json;
    }


    protected void LB_moren_Click(object sender, System.EventArgs e)
    {
        string xuanze = DropDownList1.SelectedItem.Value +"|"+ DropDownList2.SelectedItem.Text;
        Response.Cookies["morencity1"].Value = xuanze;
        Response.Cookies["morencity1"].Expires = DateTime.MaxValue;
    }



    protected void DropDownList1_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        DropDownList2.Items.Clear();
        //绑定城市
        NoSortHashTable ht_sheng = citytxtTdt();
        NoSortHashTable ht_shi = (NoSortHashTable)(ht_sheng[DropDownList1.SelectedValue]);
        foreach (string str1 in ht_shi.Keys)
        {
            DropDownList2.Items.Add(str1);
        }
        showtq(ht_shi[DropDownList2.SelectedValue].ToString());
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        NoSortHashTable ht_sheng = citytxtTdt();
        NoSortHashTable ht_shi = (NoSortHashTable)(ht_sheng[DropDownList1.SelectedValue]);
        showtq(ht_shi[DropDownList2.SelectedValue].ToString());
    }
}
public class NoSortHashTable : Hashtable
{
    private ArrayList list = new ArrayList();
    public override void Add(object key, object value)
    {
        base.Add(key, value);
        list.Add(key);
    }
    public override void Clear()
    {
        base.Clear();
        list.Clear();
    }
    public override void Remove(object key)
    {
        base.Remove(key);
        list.Remove(key);
    }
    public override ICollection Keys
    {
        get
        {
            return list;
        }
    }
}