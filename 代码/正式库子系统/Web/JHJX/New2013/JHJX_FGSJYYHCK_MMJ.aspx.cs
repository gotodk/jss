using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using System.Collections;

public partial class Web_JHJX_New2013_JHJX_FGSJYYHCK_MMJ : System.Web.UI.Page
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


            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                string str = "";
                if (NewDS.Tables[0].Rows[i]["是否冻结"].ToString() == "是")
                {
                    str += "冻结、";
                }
                else
                {
                    str += "";
                }
                if (NewDS.Tables[0].Rows[i]["是否休眠"].ToString() == "是")
                {
                    str += "休眠、";
                }
                else
                {
                    str += "";
                }

                if (NewDS.Tables[0].Rows[i]["是否暂停用户新业务"].ToString() == "是")
                {
                    str += "暂停新业务、";
                }
                else
                {
                    str += "";
                }
                if (!string.IsNullOrEmpty(str))
                {
                    int index = str.LastIndexOf('、');
                    str = str.Substring(0, index);
                    NewDS.Tables[0].Rows[i]["账户状态"] = str.ToString();
                }
                else
                {
                    NewDS.Tables[0].Rows[i]["账户状态"] = "正常";
                }


            }

            rptDLRSH.DataSource = NewDS.Tables[0].DefaultView;
            this.tdEmpty.Visible = false;
        }
        else
        {
            //rptDLRSH.DataSource = DbHelperSQL.Query("select a.Number,b.B_DLYX '交易方账号',b.I_JYFMC '交易方名称',c.J_JJRZGZSBH '经纪人资格证书编号',c.I_JYFMC '经纪人名称',convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,b.I_ZCLB '注册类型',b.I_SSQYS+b.I_SSQYSHI+b.I_SSQYQ '所属区域',c.I_PTGLJG '平台管理机构',b.I_LXRXM '联系人姓名',b.I_LXRSJH '联系电话',a.JJRSHSJ '初审时间',a.JJRSHZT '初审记录',a.FGSSHZT '复审记录',a.ZTXYWSJ '暂停新业务时间',b.B_SFDJ '是否冻结',b.B_SFXM '是否休眠',a.SFZTYHXYW '是否暂停用户新业务','' '账户状态'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX left join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where 1!=1");
            rptDLRSH.DataSource = null;
            this.tdEmpty.Visible = true;
        }
        rptDLRSH.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
       
        Hashtable ht_where = new Hashtable();
        //ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " (select top 100 percent  a.Number,b.B_DLYX '交易方账号',b.I_JYFMC '交易方名称',c.J_JJRZGZSBH '经纪人资格证书编号',c.I_JYFMC '经纪人名称',convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,b.I_ZCLB '注册类型',b.I_SSQYS+b.I_SSQYSHI+b.I_SSQYQ '所属区域',c.I_PTGLJG '平台管理机构',b.I_LXRXM '联系人姓名',b.I_LXRSJH '联系电话',a.JJRSHSJ '初审时间',a.JJRSHZT '初审记录',a.FGSSHZT '复审记录',a.ZTXYWSJ '暂停新业务时间',b.B_SFDJ '是否冻结',b.B_SFXM '是否休眠',a.SFZTYHXYW '是否暂停用户新业务','' '账户状态'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX left join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where  c.I_YWGLBMFL like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and c.I_PTGLJG like '%" + UCFWJGDetail1.Value[1].ToString() + "%'  and a.JSZHLX='买家卖家交易账户'   and a.FGSSHZT like '%" + this.ddlFSJL.SelectedValue.ToString() + "%' and b.I_JYFMC like '%" + this.txtJYFMC.Text.ToString().Trim() + "%' and a.SFYX='是' and a.SFDQMRJJR='是' and  c.I_JYFMC like '%" + this.txtJJRMC.Text.ToString().Trim() + "%'  order by a.CreateTime desc ) as tab ";  //检索的表
        //ht_where["search_mainid"] = " Number ";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        //ht_where["search_paixu"] = " DESC ";  //排序方式
        //ht_where["search_paixuZD"] = " CreateTime ";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_FGSJYZHSH_MMJ_DetailsInfo.aspx";
        
        /*---shiyan 2013-12-17 进行数据获取优化。---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.        
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " a.Number,b.B_DLYX '交易方账号',b.I_JYFMC '交易方名称',c.J_JJRZGZSBH '经纪人资格证书编号',c.I_JYFMC '经纪人名称',convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,b.I_ZCLB '注册类型',b.I_SSQYS+b.I_SSQYSHI+b.I_SSQYQ '所属区域',c.I_PTGLJG '平台管理机构',b.I_LXRXM '联系人姓名',b.I_LXRSJH '联系电话',a.JJRSHSJ '初审时间',a.JJRSHZT '初审记录',a.FGSSHZT '复审记录',a.ZTXYWSJ '暂停新业务时间',b.B_SFDJ '是否冻结',b.B_SFXM '是否休眠',a.SFZTYHXYW '是否暂停用户新业务','' '账户状态' ";
        ht_where["search_tbname"] = " AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX left join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH ";  //检索的表
        ht_where["search_mainid"] = " a.Number ";  //所检索表的主键
        ht_where["search_str_where"] = " a.JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.SFDQMRJJR='是' ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " a.CreateTime ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "New2013/JHJX_FGSJYZHSH_MMJ_DetailsInfo.aspx";
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
        /*---shiyan增加 2013-12-17 数据获取优化---*/
        //平台管理机构条件
        if(UCFWJGDetail1 .Value [0].ToString ()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and c.I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString() + "' ";
        }
        if(UCFWJGDetail1 .Value [1].ToString ()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and  c.I_PTGLJG='" + UCFWJGDetail1.Value[1].ToString() + "' ";  
        }
        //交易方名称
        if(txtJYFMC .Text .Trim ()!="")
        {
             HTwhere["search_str_where"] = HTwhere["search_str_where"] +" and b.I_JYFMC like '%" + this.txtJYFMC.Text.Trim() + "%' ";
        }
        //经纪人名称
        if(txtJJRMC .Text .Trim ()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and c.I_JYFMC like '%" + this.txtJJRMC.Text.Trim() + "%' ";
        } 
        //复审记录
        if(ddlFSJL .SelectedValue.ToString ()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.FGSSHZT='" + this.ddlFSJL.SelectedValue.ToString() + "' ";
        }
        //经纪人账号
        if (txtJJRZH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and c.B_DLYX like '%" + this.txtJJRZH.Text.Trim() + "%' ";
        }
        //交易方账号
        if (txtJYFZH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.B_DLYX like '%" + this.txtJYFZH.Text.Trim() + "%' ";
        }

        HTwhere["search_str_where"] += " and b.I_SSQYS+b.I_SSQYSHI+b.I_SSQYQ like '%" + strSF + strDS + strQX + "%'";
        /*---shiyan增加结束---*/
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    /// <summary>
    /// 处理资料详情的操作
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptDLRSH_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "linkJXSH"://进行审核
                object objJSBH = DbHelperSQL.GetSingle("select b.J_SELJSBH from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  a.Number='" + e.CommandArgument.ToString() + "'");
                string strUrl = "JHJX_FGSJYZHSH_MMJ_DetailsInfo.aspx?JSBH=" + objJSBH.ToString();
                Response.Redirect(strUrl);
                break;
        }
    }
}