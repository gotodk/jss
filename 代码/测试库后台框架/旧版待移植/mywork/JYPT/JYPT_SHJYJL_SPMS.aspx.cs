using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;
using System.Collections;

public partial class mywork_JYPT_JYPT_SHJYJL_SPMS : System.Web.UI.Page
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
            object objYHM = DbHelperSQL.GetSingle("select B_YHM from AAA_DLZHXXB where B_DLYX='" + hidDLYX.Value + "'");
            if (objYHM != null && objYHM.ToString() != "")
            {
                hidYHM.Value = objYHM.ToString();
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
        //平台现用名称
        object objXYMC = DbHelperSQL.GetSingle("select SPMS from AAA_PTSPXXB where spbh='" + hidSPBH.Value + "'");
        if (objXYMC != null && objXYMC.ToString() != "")
        {
            spanXYNR.InnerText = objXYMC.ToString();
        }

        //确定是否提交过建议
        string sql_jy = "select * from AAA_SPXXJYB where JYRDLYX='" + hidDLYX.Value + "' and spbh='"+hidSPBH .Value +"' and jysx='商品描述'";
        DataSet ds_jy = DbHelperSQL.Query(sql_jy);
        if (ds_jy != null && ds_jy.Tables[0].Rows.Count > 0)
        {
            trTJ.Visible = false;
            trCK.Visible = true;
            lblCK.Text = "您已经提交了新的商品描述建议，内容为：" + ds_jy.Tables[0].Rows[0]["JYNR"].ToString();
            hidCan.Value = "false";
        }
        else
        {//没有提交过建议的时候，再确定是否支持过某个建议
            string sql_zc = "select a.*,b.* from AAA_SPXXJYB_ZCXQ as a left join AAA_SPXXJYB as b on a.parentnumber=b.number where ZCRDLYX='" + hidDLYX.Value + "' and spbh='"+hidSPBH.Value+"' and jysx='商品描述'";
            DataSet ds_zc = DbHelperSQL.Query(sql_zc);
            if (ds_zc != null && ds_zc.Tables[0].Rows.Count > 0)
            {
                trTJ.Visible = false;
                trCK.Visible = true;
                lblCK.Text = "您已经对用户 <span style=\"color :Red\">" + ds_zc.Tables[0].Rows[0]["JYRYHM"].ToString() + "</span> 提出的商品描述表示了支持。";
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

        if (NewDS != null && NewDS.Tables.Count > 0)
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
        ht_where["page_size"] = " 5 ";
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select a.number,a.spbh,a.jynr as 建议内容,a.jyryhm as 建议者,a.zccs+1 as 支持次数,参与总数,CONVERT(varchar(10), ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,参与总数)*100.00,2))+'%' as 支持率, ROUND(CONVERT(float,(a.zccs+1))/CONVERT(float,参与总数)*100.00,2) as zclsort from AAA_SPXXJYB as a left join (select spbh,jysx,SUM(ZCCS+1) as 参与总数 from AAA_SPXXJYB where jysx='商品描述' group by SPBH,jysx) as b on a.spbh=b.spbh and a.jysx=b.jysx where a.jysx='商品描述') as tab ";
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
        Label lblJYSC = (Label)e.Item.FindControl("lbljynr");
        if (lblJYSC.Text.Length > 20)
            lblJYSC.Text = lblJYSC.Text.Substring(0, 20) + "...";

        Label lblJYZ = (Label)e.Item.FindControl("lblJYZ");
        if (lblJYZ.Text.Length > 10)
        {
            lblJYZ.Text = lblJYZ.Text.Substring(0, 10) + "...";
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
        //确定同样的建议是不是已经有人提交过了
        DataSet ds_jy = DbHelperSQL.Query("select * from AAA_SPXXJYB where jynr='" + txtSPXMC.Text.Trim() + "' and spbh='"+hidSPBH+"' and jysx='商品描述'");
        if (ds_jy != null && ds_jy.Tables[0].Rows.Count > 0)
        {
            MessageBox.ShowAlertAndBack(this, "此建议已存在，您可以支持此建议或者提交新的建议！");
        }
        if (txtSPXMC.Text.Trim().Length > 300)
        {
            MessageBox.ShowAlertAndBack(this, "您的建议已经超过300个字，请修改后重新提交！");
        }

        //提交新建议
        if (hidSPBH.Value != "" && hidDLYX.Value != "")
        {
            //获取新插入数据的number
            WorkFlowModule WFM = new WorkFlowModule("AAA_SPXXJYB");
            string KeyNumber = WFM.numberFormat.GetNextNumber();

            string sql_insert = "INSERT INTO [AAA_SPXXJYB]([Number],[JYRDLYX],[JYRYHM],[SPBH],[JYSX],[JYNR],[ZCCS],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES('" + KeyNumber + "','" + hidDLYX.Value + "','" + hidYHM.Value + "','" + hidSPBH.Value + "','商品描述', '" + txtSPXMC.Text.Trim() + "',0,1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
            try
            {
                DbHelperSQL.ExecuteSql(sql_insert);
                MessageBox.ShowAndRedirect(this, "建议提交成功！", "JYPT_SHJYJL_SPMS.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value);
            }
            catch
            {
                MessageBox.ShowAlertAndBack(this, "建议提交失败！");
            }
        }       
    }

    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //支持操作
        if (e.CommandName == "ZhiChi")
        {
            string parentnumber = e.CommandArgument.ToString();
            ArrayList al = new ArrayList();
            string sql_insert = "INSERT INTO [AAA_SPXXJYB_ZCXQ](parentNumber,ZCRDLYX,ZCRYHM,ZCSJ) VALUES('" + parentnumber + "','" + hidDLYX.Value + "','" + hidYHM.Value + "','" + DateTime.Now.ToString() + "')";
            al.Add(sql_insert);
            string sql_update = "UPDATE AAA_SPXXJYB set ZCCS=ZCCS+1 where Number='"+parentnumber+"' and jysx='商品描述' and spbh='"+hidSPBH .Value +"'";
            al.Add(sql_update);

            //增加积分2013-10-25,将获取的sql语句增加到原来的al中
            string[] resql = AddJF(parentnumber);
            if (resql.Length > 0)
            {
                for (int i = 0; i < resql.Length; i++)
                {
                    al.Add(resql[i]);
                }
            }
            try
            {
                DbHelperSQL.ExecuteSqlTran(al);
                MessageBox.ShowAndRedirect(this, "支持成功！", "JYPT_SHJYJL_SPMS.aspx?spbh=" + hidSPBH.Value + "&dlyx=" + hidDLYX.Value);
            }
            catch
            {
                MessageBox.ShowAlertAndBack(this, "支持失败！");
            }

        }
    }

    //发布的建议第5个支持的，就加5分
    private string[] AddJF(string parentnumber)
    {
        string[] sql = new string[]{};
        //判断是否需要增加积分2013-10-25
        DataSet ds_zcsl = DbHelperSQL.Query("select * from AAA_SPXXJYB_ZCXQ where parentnumber='" + parentnumber + "'");

        if (ds_zcsl != null && ds_zcsl.Tables[0].Rows.Count == 4)
        {
            //获取此条建议提供者的登录邮箱
            DataSet ds_tgz = DbHelperSQL.Query("select JYRDLYX,B_JSZHLX from AAA_SPXXJYB as a left join AAA_DLZHXXB as b on a.JYRDLYX=B_DLYX where a.number='" + parentnumber + "' and jysx='商品描述' and spbh='" + hidSPBH.Value + "'");
            string tgzyx = "";
            string zhlx = "";
            if (ds_tgz != null && ds_tgz.Tables[0].Rows.Count > 0)
            {
                tgzyx = ds_tgz.Tables[0].Rows[0]["JYRDLYX"].ToString();
                zhlx = ds_tgz.Tables[0].Rows[0]["B_JSZHLX"].ToString();
            }

            if (tgzyx != "" && zhlx == "买家卖家交易账户")
            {
                jhjx_JYFXYMX createsql = new jhjx_JYFXYMX();
                sql = createsql.JYJL(tgzyx);
            }
        }
        return sql;
    }
}