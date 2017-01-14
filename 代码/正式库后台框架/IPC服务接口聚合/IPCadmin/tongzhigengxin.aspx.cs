using FMipcClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMDBHelperClass;
using System.Data;
using System.Collections;
using System.Configuration;

public partial class IPCadmin_tongzhigengxin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Button)wuccaidan1.FindControl("Button3")).ForeColor = System.Drawing.Color.Red;
            beginshow();
        }
        

        //显示配置下载链接
        string[] biaoshi = new string[] { "测试版客户端","模拟版客户端", "交易数据监控", "各银行监控统一标记", "旧版待移植", "Server客户端专用中心", "Server成交处理中心", "Server成交履约中心", "Server辅助业务中心", "Server交易指令中心", "Server开放平台中心", "Server数据分析中心", "Server系统控制中心", "Server银行合作中心", "Server用户控制中心" };

        string htmlx = "";
        htmlx = htmlx + "<strong>直接通过识别标记取得关系：</strong>";
        htmlx = htmlx + "<br/>";

        for (int i = 0; i < biaoshi.Count(); i++)
        {
            //System.Web.HttpUtility.UrlEncode(".net技术", System.Text.Encoding.GetEncoding("GB2312"));

            string huanhang = "";
            if (i == 5 || i == 10)
            {
                huanhang = "<br/>";
            }
            string n = HttpUtility.UrlEncode(biaoshi[i],System.Text.Encoding.UTF8);
            htmlx = htmlx + "<a href='xz.aspx?yewuming=" + n + "' target='_blank'>" + biaoshi[i] + "</a> |" + huanhang;
        }

        xiazaiguanxi.InnerHtml = htmlx;

    }
    //绑定数据列表
    private void beginshow()
    {
        string sql = " select distinct  IP.IP_address as IP地址,IP.IP_JK_host as 接口域名,JK.JK_port as 接口备用端口,IP.IP_open as IP地址是否有效,(CASE IP.IP_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as IP地址状态,IP.IP_shuoming as 备注,IP.IP_addtime as IP添加时间,IP.IP_edittime as IP最后修改时间 from  AAA_ipcIP as IP left join (select distinct JK_host,JK_port from AAA_ipcJK where JK_open=1) as JK on JK.JK_host=IP.IP_JK_host";
        if (TextBox2.Text.Trim() != "")
        {
            sql = sql + " " + TextBox2.Text.Trim();
        }
        if (TextBox1.Text.Trim() != "")
        {
            sql = sql + " " + TextBox1.Text.Trim();
        }

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsIPlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "IP负载", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsIPlist = (DataSet)(return_ht["return_ds"]);
            if (dsIPlist.Tables["IP负载"].Rows.Count < 1)
            {
                showmsg.InnerHtml = showmsg.InnerHtml + "结果:没有负载IP需要通知。";
                GridView1.DataSource = null;
                GridView1.DataBind();
                return;
            }
            else
            {
                GridView1.DataSource = dsIPlist.Tables["IP负载"].DefaultView;
                GridView1.DataBind();
            }
        }
        else
        {
            //读取数据库出错。。。
            showmsg.InnerHtml = showmsg.InnerHtml + "结果:获得通知列表失败。" + return_ht["return_errmsg"].ToString();
            return;
            //Response.Write(return_ht["return_errmsg"].ToString());
        }
    }
    //对选中的项目执行通知更新
    protected void Button1_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            CheckBox ckb = (CheckBox)GridView1.Rows[i].FindControl("IP_chkItem");
            if (ckb.Checked)
            {
                showmsg.InnerHtml = showmsg.InnerHtml + "====" + GridView1.Rows[i].Cells[1].Text.Trim() + "[" + GridView1.Rows[i].Cells[2].Text.ToString() + "][" + GridView1.Rows[i].Cells[3].Text.ToString() + "]===<br />";
                //找到IPC登记的有效接口，进行批量通知。
                object re = null;
                try
                {
                    string IPCurl = "http://" + GridView1.Rows[i].Cells[2].Text.ToString() + ":" + GridView1.Rows[i].Cells[3].Text.ToString() + "/OrderFormIPC.asmx";
                    FMWScenter wsd = new FMWScenter(IPCurl + "?wsdl");
                    re = wsd.ExecuteQuery("ReSetThisWebServices", new object[] { "" });

                    if (re == null)
                    {
                        showmsg.InnerHtml = showmsg.InnerHtml + "结果:发送通知出错" + "<br />";
                    }
                    else
                    {
                        showmsg.InnerHtml = showmsg.InnerHtml + "结果:" + re.ToString() + "<br />";
                    }
                }
                catch (Exception ex)
                {
                    showmsg.InnerHtml = showmsg.InnerHtml + i + "结果:" + ex.ToString() + "<br />";
                }
                showmsg.InnerHtml = showmsg.InnerHtml + "==============================================<br />";
            }
        }
        if (showmsg.InnerHtml.ToString().Trim() == "")
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('未选中任何项！');</script>");
            return;
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //如果当前行尾数据行
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //添加鼠标在当前行时的background-color属性
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#fbed90'){this.style.backgroundColor='#aadef6';}");
            //鼠标离开当前行后
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#fbed90'){this.style.backgroundColor='#ffffff';}");
            e.Row.Attributes.Add("onclick", "if(window.oldtr!=null){window.oldtr.runtimeStyle.cssText= ' ';}this.runtimeStyle.cssText= 'background-color:#e6c5fc ';window.oldtr=this");
            e.Row.Attributes.Add("ondblclick", "if(this.style.backgroundColor=='#aadef6' || this.style.backgroundColor=='#ffffff'){this.style.backgroundColor='#fbed90';}else{this.style.backgroundColor='#ffffff';}");
        }
    }
    //全选
    protected void IP_chkAll_CheckedChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            ((CheckBox)GridView1.Rows[i].FindControl("IP_chkItem")).Checked =
                ((CheckBox)this.GridView1.HeaderRow.FindControl("IP_chkAll")).Checked;
        }
    }

    //刷新
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("tongzhigengxin.aspx");
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        beginshow();
    }

  
     
}