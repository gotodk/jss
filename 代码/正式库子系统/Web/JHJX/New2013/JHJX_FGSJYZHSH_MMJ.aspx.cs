using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using System.Collections;

public partial class Web_JHJX_New2013_JHJX_FGSJYZHSH_MMJ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail1.initdefault();
            UCCityList1.initdefault();
      
            DisGrid();


        }
    }
  
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        //输出调试错误
        // Response.Write(ERRinfo);
        //Response.End();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器超时，请稍后重试！',function(){ window.location.href=window.location.href;});</script>");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {

            rptDLRSH.DataSource = NewDS.Tables[0].DefaultView;
            this.tdEmpty.Visible = false;
        }
        else
        {
            rptDLRSH.DataSource = DbHelperSQL.Query("select a.Number,b.B_DLYX '交易方账号',b.I_JYFMC '交易方名称',c.J_JJRZGZSBH '经纪人资格证书编号',c.I_JYFMC '经纪人名称',convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,b.I_ZCLB '注册类型',b.I_SSQYS+b.I_SSQYSHI+b.I_SSQYQ '所属区域',c.I_PTGLJG '平台管理机构',b.I_LXRXM '联系人姓名',b.I_LXRSJH '联系电话',a.JJRSHSJ '初审时间',a.JJRSHZT '初审记录',a.FGSSHZT '复审记录','' '驳回后是否修改'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX left join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where 1!=1");
            this.tdEmpty.Visible = true;
        }
        rptDLRSH.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select top 100 percent a.Number,b.B_DLYX '交易方账号',b.I_JYFMC '交易方名称',c.J_JJRZGZSBH '经纪人资格证书编号',c.I_JYFMC '经纪人名称',convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,b.I_ZCLB '注册类型',b.I_SSQYS+b.I_SSQYSHI+b.I_SSQYQ '所属区域',c.I_PTGLJG '平台管理机构',b.I_LXRXM '联系人姓名',b.I_LXRSJH '联系电话',a.JJRSHSJ '初审时间',a.JJRSHZT '初审记录',a.FGSSHZT '复审记录',(case a.FGSSHZT when '审核中' then '--' when '驳回' then isnull(a.SFXG,'--') else '' end ) '驳回后是否修改'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX left join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where c.I_YWGLBMFL like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and  c.I_PTGLJG like '%" + UCFWJGDetail1.Value[1].ToString() + "%'  and a.JSZHLX='买家卖家交易账户' and a.JJRSHZT='审核通过' and  a.FGSSHZT in('审核中','驳回')  and a.FGSSHZT like '%" + this.ddlFSJL.SelectedValue.ToString() + "%' and b.I_JYFMC like '%" + this.txtJYFMC.Text.ToString().Trim() + "%' and a.SFYX='是' and  c.I_JYFMC like '%" + this.txtJJRMC.Text.ToString().Trim() + "%'  order by a.CreateTime desc ) as tab ";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = " CreateTime asc,初审时间 ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "New2013/JHJX_FGSJYZHSH_MMJ_Details.aspx";
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        string[] strQY = UCCityList1.SelectedItem;
        string strSF = "";
        string strDS = "";
        string strQX = "";
        if (strQY[0].ToString() == "请选择省份")
        {
            strSF = "";
        }
        else
        {
            strSF = strQY[0].ToString();
        }

        if (strQY[1].ToString() == "请选择城市")
        {
            strDS = "";
        }
        else
        {
            strDS = strQY[1].ToString();
        }

        if (strQY[2].ToString() == "请选择区县")
        {
            strQX = "";
        }
        else
        {
            strQX = strQY[2].ToString();
        }

        HTwhere["search_str_where"] += " and 所属区域 like '%" + strSF + strDS + strQX + "%' and 驳回后是否修改 like '%" + this.ddlSFXG.SelectedValue.ToString() + "%'  and 交易方账号 like '%" + txtJYFZH.Text.Trim() + "%' ";
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    /// <summary>
    /// 处理审核、驳回操作
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptDLRSH_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "linkJXSH"://进行审核

                Label lblb = (Label)e.Item.FindControl("lblb");
                Label lbjg = (Label)e.Item.FindControl("lbjg");

                string str = "select a.Number,B_DLYX '交易方账号',I_JYFMC '交易方名称',a.GLJJRBH '关联经纪人编号',b.J_SELJSBH '卖家角色编号', convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,I_ZCLB '注册类型',I_SSQYS+I_SSQYSHI+I_SSQYQ '所属区域',I_PTGLJG '平台管理机构',I_LXRXM '联系人姓名',I_LXRSJH '联系电话',FGSSHZT '初审记录','' '驳回后是否修改'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX  where a.Number='" + e.CommandArgument.ToString().Trim() + "'";

                DataSet ds = DbHelperSQL.Query(str);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    string lb = ds.Tables[0].Rows[0]["注册类型"].ToString().Trim();

                    string jg = ds.Tables[0].Rows[0]["平台管理机构"].ToString().Trim();

                    string bh = ds.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();

                    if (jg != lbjg.Text.Trim())
                    {
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前交易账户已更换所属服务机构！');</script>");
                        DisGrid();
                        return;
                    }
                    if (lb != lblb.Text.Trim())
                    {
                        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('当前交易账户已更换注册类别，请重新审核！');</script>");
                        DisGrid();
                       
                        return;

                    }

                    else
                    {                       
                        string strUrl = "JHJX_FGSJYZHSH_MMJ_Details.aspx?JSBH=" + bh;
                        Response.Redirect(strUrl);
                    }
 
                }
                //object objJSBH = DbHelperSQL.GetSingle("select b.J_SELJSBH from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  a.Number='" + e.CommandArgument.ToString() + "'");
                //string strUrl = "JHJX_FGSJYZHSH_MMJ_Details.aspx?JSBH=" + objJSBH.ToString();
                //Response.Redirect(strUrl);
                break;
        }
    }
}