using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using FMDBHelperClass;
using FMipcClass;


public partial class mywork_JYPT_SHJYJL_SPSC : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpager1.OnNeedLoadData += new pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData);//分类列表的分页
        if (!IsPostBack)
        {
            if (Request["spbh"] != null && Request["spbh"].ToString() != "")
            {
                hidSPBH.Value = Request["spbh"].ToString();
            }
            if (Request["dlyx"] != null && Request["dlyx"].ToString() != "")
            {
                hidDLYX.Value = Request["dlyx"].ToString();
            }

            //获取登陆者的用户名
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable input = new Hashtable();
            input["@B_DLYX"] = hidDLYX.Value;
            string sql_sel = "select B_YHM from AAA_DLZHXXB where B_DLYX=@B_DLYX";
            Hashtable return_YHM = I_DBL.RunParam_SQL(sql_sel, "用户名", input);
            if ((bool)return_YHM["return_float"])
            {
                DataSet dsyhm = (DataSet)return_YHM["return_ds"];
                hidYHM.Value = dsyhm.Tables[0].Rows[0]["B_YHM"].ToString();
            }
            else
            {               
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S1:系统异常！');</script>");
                return;
            }

            setURL();
            SetContent();
            RepeaterBind();
        }
    }
    private void setURL()
    {
        aSPSC.HRef = "JYPT_SHJYJL_SPSC.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
        aSPMS.HRef = "JYPT_SHJYJL_SPMS.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
        aJYBZ.HRef = "JYPT_SHJYJL_YSBZ.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
        aZLBZ.HRef = "JYPT_SHJYJL_ZLBZ.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
    }
    /// <summary>
    /// 根据商品编号和登陆邮箱查找当前的建议俗称信息，并确定页面上应该显示的内容。
    /// </summary>
    private void SetContent()
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        
        Hashtable input = new Hashtable();
        input["@DLYX"] = hidDLYX.Value;
        input["@spbh"] = hidSPBH.Value.ToString();

        //平台现用商品俗称
        string sql_sel = "select SPMC from AAA_PTSPXXB where spbh=@spbh";
        Hashtable result_XYMC = I_DBL.RunParam_SQL(sql_sel, "俗称", input);
        if ((bool)result_XYMC["return_float"])
        {
            DataSet dsxymc = (DataSet)result_XYMC["return_ds"];
            if (dsxymc != null && dsxymc.Tables[0].Rows.Count > 0)
            {
                spanXYMC.InnerText = dsxymc.Tables[0].Rows[0]["SPMC"].ToString();
            }
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S11:系统异常！');</script>");
            return;
        }

        //判断是否已经提交过建议             
        string sql_jy = "select * from AAA_SPXXJYB where JYRDLYX=@DLYX and spbh=@spbh and jysx='商品俗称'";
        Hashtable result_jy = I_DBL.RunParam_SQL(sql_jy, "提交", input);
        DataSet ds_jy = new DataSet();
        if ((bool)result_jy["return_float"])
        {
            ds_jy = (DataSet)result_jy["return_ds"];
        }
        else
        {           
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S2:系统异常！');</script>");
            return;
        }
       
        if (ds_jy != null && ds_jy.Tables[0].Rows.Count > 0)
        {
            trTJ.Visible = false;
            trCK.Visible = true;
            lblCK.Text = "您已提交了新的商品俗称建议，内容为：" + ds_jy.Tables[0].Rows[0]["JYNR"].ToString();
            hidCan.Value = "false";
        }
        else
        {
            //没有提交过建议的话，判断是否已经支持过某个建议。
            string sql_zc = "select a.*,b.* from AAA_SPXXJYB_ZCXQ as a left join AAA_SPXXJYB as b on a.parentnumber=b.number where ZCRDLYX=@DLYX and spbh=@spbh and JYSX='商品俗称'";
           
            DataSet ds_zc = new DataSet();
            Hashtable result_zc = I_DBL.RunParam_SQL(sql_zc, "支持", input);
            if ((bool)result_zc["return_float"])
            {
                ds_zc = (DataSet)result_zc["return_ds"];
            }
            else
            {              
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S3:系统异常！');</script>");
                return;
            }
            if (ds_zc != null && ds_zc.Tables[0].Rows.Count > 0)
            {
                trTJ.Visible = false;
                trCK.Visible = true;
                lblCK.Text = "您已经对用户 <span style=\"color :Red\">" + ds_zc.Tables[0].Rows[0]["JYRYHM"].ToString() + "</span> 提出的建议俗称表示了支持。";
                hidCan.Value = "false";
            }
            else
            {
                trTJ.Visible = true;
                trCK.Visible = false;
                hidCan.Visible = true;
            }
        }
    }

    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        rpt.DataSource = null;

        if (NewDS != null && NewDS.Tables[0].Rows .Count > 0)
        {
            tempty.Visible = false;
            rpt.DataSource = NewDS;
        }       
        //控制在没有数据时显示表头和表脚
        else
        {
            tempty.Visible = true;            
        }
        rpt.DataBind();
    }
   
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 7 ";
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select a.number,a.spbh,a.jysx as 建议事项,a.jynr as 建议俗称,a.jyryhm as 建议者,a.JYRDLYX as 建议者邮箱,a.zccs+1 as 支持次数,参与总数,CONVERT(varchar(10), ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,参与总数)*100.00,2))+'%' as 支持率, ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,参与总数)*100.00,2) as zclsort from AAA_SPXXJYB as a left join (select spbh,jysx,SUM(ZCCS+1) as 参与总数 from AAA_SPXXJYB group by SPBH,jysx) as b on a.spbh=b.spbh and a.jysx=b.jysx where a.jysx='商品俗称') as tab ";
        ht_where["search_mainid"] = " number ";
        ht_where["search_str_where"] = " 1=1 ";
        ht_where["search_paixu"] = " DESC ";   
        ht_where["search_paixuZD"] = " zclsort DESC, spbh";
            
        return ht_where;
    }

    protected void RepeaterBind()
    {
        Hashtable HTwhere = SetV();
        HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and spbh='" + hidSPBH.Value + "'";
        commonpager1.HTwhere = HTwhere;
        commonpager1.GetFYdataAndRaiseEvent();
    }

    protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblJYSC = (Label)e.Item.FindControl("lbljysc");
        if (lblJYSC.Text.Length > 26)
            lblJYSC.Text = lblJYSC.Text.Substring(0, 26) + "...";

        Label lblJYZ = (Label)e.Item.FindControl("lblJYZ");
        if (lblJYZ.Text.Length >10)
        {
            lblJYZ.Text = lblJYZ.Text.Substring(0,10)+"...";
        }

        LinkButton likb = (LinkButton)e.Item.FindControl("btnZhiChi");
        if (hidCan.Value == "false")
        {
            likb.Enabled = false;
            likb.ToolTip = lblCK.Text;
        }

    }
    protected void btnTJ_Click(object sender, EventArgs e)
    {
        //确定同样的商品描述是不是已经有人提交过了
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable input = new Hashtable();
        input["@jynr"] = txtSPXMC.Text.Trim();
        input["@spbh"] = hidSPBH.Value.ToString();
        string sql_jy = "select * from AAA_SPXXJYB where jynr=@jynr and spbh=@spbh and jysx='商品俗称'";

        Hashtable result_jy = I_DBL.RunParam_SQL(sql_jy, "俗称", input);
        DataSet ds_jy = new DataSet();
        if ((bool)result_jy["return_float"])
        {
            ds_jy = (DataSet)result_jy["return_ds"];
        }
        else
        {
            //ds_jy = null;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S4:系统异常！');</script>");
            return;
        }
        if (ds_jy != null && ds_jy.Tables[0].Rows.Count > 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('此建议已存在，您可以支持此建议或者提交新的建议！');</script>");
            return;
        }
        if (txtSPXMC.Text.Trim().Length > 30)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('您的建议已经超过30个字，请修改后重新提交！');</script>");
            return;
        }
        if (hidSPBH.Value != "" && hidDLYX.Value != "")
        {
            object[] re = IPC.Call("获取主键", new object[] { "AAA_SPXXJYB", "" });
            string KeyNumber = "";
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                KeyNumber = (string)(re[1]);
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                string reFF = re[1].ToString();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('S5:系统异常！');</script>");
                return;
            }

            input["@KeyNumber"] = KeyNumber;
            input["@DLYX"] = hidDLYX.Value.ToString();
            input["@YHM"] = hidYHM.Value.ToString();
            input["@CreateTime"] = DateTime.Now.ToString();

            string sql_insert = "INSERT INTO [AAA_SPXXJYB]([Number],[JYRDLYX],[JYRYHM],[SPBH],[JYSX],[JYNR],[ZCCS],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES(@KeyNumber,@DLYX,@YHM,@spbh,'商品俗称', @jynr,0,1,'admin',@CreateTime,@CreateTime)";

            Hashtable result = I_DBL.RunParam_SQL(sql_insert, input);
            if (((bool)result["return_float"]) == true && result["return_other"].ToString() == "1")
            {
                string url = "JYPT_SHJYJL_SPSC.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('建议提交成功！');window.location.href='" + url + "';</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('建议提交失败！');</script>");
                return;
            }
        }
    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "ZhiChi")
        {
            string[] cdarg = e.CommandArgument.ToString().Split(',');
            string parentnumber = cdarg[0].ToString();
            string jyzdlyx = cdarg[1].ToString();
            
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable input = new Hashtable();
            input["@parentNumber"] = parentnumber;
            input["@DLYX"] = hidDLYX.Value.ToString();
            input["@YHM"] = hidYHM.Value.ToString();
            input["@ZCSJ"] = DateTime.Now.ToString();
            input["@spbh"] = hidSPBH.Value.ToString();

            ArrayList al = new ArrayList();
            string sql_insert = "INSERT INTO [AAA_SPXXJYB_ZCXQ](parentNumber,ZCRDLYX,ZCRYHM,ZCSJ) VALUES(@parentNumber,@DLYX,@YHM,@ZCSJ)";
            al.Add(sql_insert);
            string sql_update = "UPDATE AAA_SPXXJYB set ZCCS=ZCCS+1 where number=@parentNumber and  SPBH=@spbh and jysx='商品俗称'";
            al.Add(sql_update);

            Hashtable result = I_DBL.RunParam_SQL(al, input);

            if (((bool)result["return_float"]) == true && result["return_other"].ToString() == al.Count.ToString())
            {
                //发布的建议被第五个人支持，发布的人加5分,异步处理
                AddJF(I_DBL, parentnumber,jyzdlyx);
                string url = "JYPT_SHJYJL_SPSC.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('支持成功！');window.location.href='" + url + "';</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script>alert('支持失败！');</script>");
                return;
            }

        }
    }
    //发布的建议第5个支持的，就加5分
    private void AddJF(I_Dblink I_DBL, string parentnumber, string jyzdlyx)
    {
        //判断是否需要增加积分2013-10-25
        Hashtable input = new Hashtable();
        input["@parentNumber"] = parentnumber;
        string sql_zcsl = "select * from AAA_SPXXJYB_ZCXQ where parentnumber=@parentNumber";
        DataSet ds_zcsl = new DataSet();
        Hashtable result_zcsl = I_DBL.RunParam_SQL(sql_zcsl, "支持数量", input);
        if ((bool)result_zcsl["return_float"])
        {
            ds_zcsl = (DataSet)result_zcsl["return_ds"];
        }
        else
        {
            ds_zcsl = null;
        }
        if (ds_zcsl != null && ds_zcsl.Tables[0].Rows.Count == 5)
        {//如果支持数量已经有5个了，则调用增加信用积分的方法
            DataTable dt = new DataTable();
            dt.TableName = "参数";
            dt.Columns.Add("登录邮箱");
            dt.Columns.Add("备注");
            dt.Rows.Add(new string[] { jyzdlyx, "商品俗称" + parentnumber });
            object[] run_xyjf = IPC.Call("信用等级积分处理", new object[] { "大盘经验交流", dt });
        }
    }
}