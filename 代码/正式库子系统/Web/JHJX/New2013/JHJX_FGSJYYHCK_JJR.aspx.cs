using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using System.Collections;

public partial class Web_JHJX_New2013_JHJX_FGSJYYHCK_JJR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail1.initdefault();
          
            DisGrid();
            UCCityList1.initdefault();

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
                if (NewDS.Tables[0].Rows[i]["是否休眠"].ToString() == "是")
                {
                    str += "休眠、";
                }
                if (NewDS.Tables[0].Rows[i]["是否暂停新用户审核"].ToString() == "是")
                {
                    str += "暂停新用户审核、";
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
            rptDLRSH.DataSource = null;
            this.tdEmpty.Visible = true;
        }
        rptDLRSH.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {
        

        //Hashtable ht_where = new Hashtable();
        //ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " (select top 100 percent  a.Number,B_DLYX '交易方账号',I_JYFMC '交易方名称',convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,I_ZCLB '注册类型',I_SSQYS+I_SSQYSHI+I_SSQYQ '所属区域',I_PTGLJG '平台管理机构',I_LXRXM '联系人姓名',I_LXRSJH '联系电话',FGSSHZT '初审记录',b.J_JJRZTXYHSHSJ '暂停用户审核时间',b.B_SFDJ '是否冻结',b.B_SFXM '是否休眠',b.J_JJRSFZTXYHSH '是否暂停新用户审核','' '账户状态'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.GLJJRBH=b.J_JJRJSBH  where I_YWGLBMFL like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and  I_PTGLJG like '%" + UCFWJGDetail1.Value[1].ToString() + "%' and  a.JSZHLX='经纪人交易账户'  and I_JYFMC like '%" + this.txtJYFMC.Text.Trim() + "%' and a.FGSSHZT like '%" + this.ddLCSJL.SelectedValue.ToString() + "%' and a.SFYX='是' order by a.CreateTime desc) as tab ";  //检索的表
        //ht_where["search_mainid"] = " Number ";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        //ht_where["search_paixu"] = " DESC ";  //排序方式
        //ht_where["search_paixuZD"] = " CreateTime ";  //用于排序的字段
        
        /*---shiyan 2013-12-16 进行数据获取优化。---*/
        Hashtable ht_where = new Hashtable();
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.        
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " a.Number,B_DLYX '交易方账号',I_JYFMC '交易方名称',convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',a.CreateTime,I_ZCLB '注册类型',I_SSQYS as 所属区域省,I_SSQYSHI as 所属区域市,I_SSQYQ as '所属区域区',I_PTGLJG '平台管理机构',I_LXRXM '联系人姓名',I_LXRSJH '联系电话',FGSSHZT '初审记录',b.J_JJRZTXYHSHSJ '暂停用户审核时间',b.B_SFDJ '是否冻结',b.B_SFXM '是否休眠',b.J_JJRSFZTXYHSH '是否暂停新用户审核','' '账户状态' ";
        ht_where["search_tbname"] = " AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.GLJJRBH=b.J_JJRJSBH ";  //检索的表
        ht_where["search_mainid"] = " a.Number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 and a.JSZHLX='经纪人交易账户' and a.SFYX='是' ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " a.CreateTime ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "New2013/JHJX_FGSJYZHSH_JJR_DetailsInfo.aspx";
        return ht_where;
    }


    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        /*---shiyan增加 2013-12-16 进行数据获取优化。---*/
        if(UCFWJGDetail1 .Value [0].ToString ()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString() + "' ";
        }
        if(UCFWJGDetail1 .Value [1].ToString ()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and  I_PTGLJG='" + UCFWJGDetail1.Value[1].ToString() + "' ";  
        }
        if(txtJYFMC .Text .Trim ()!="")
        {
             HTwhere["search_str_where"] = HTwhere["search_str_where"] +" and I_JYFMC like '%" + this.txtJYFMC.Text.Trim() + "%' ";
        }
        if(ddLCSJL .SelectedValue.ToString ()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and a.FGSSHZT='" + this.ddLCSJL.SelectedValue.ToString() + "' ";
        }

        string[] strQY = UCCityList1.SelectedItem;
        string strSF = "";
        string strDS = "";
        string strQX = "";
        if (strQY[0].ToString() != "请选择省份" && strQY[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and I_SSQYS='" + strQY[0].ToString() + "' ";
        }
        if (strQY[1].ToString() != "请选择城市" && strQY[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and I_SSQYSHI='" + strQY[1].ToString() + "' ";
        }

        if (strQY[2].ToString() != "请选择区县" && strQY[2].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and I_SSQYQ='" + strQY[2].ToString() + "' ";
        }
        /*---shiyan增加结束---*/
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    /// <summary>
    /// 查看经纪人资料详情
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptDLRSH_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "linkZLXQ"://资料详情
                object objJSBH = DbHelperSQL.GetSingle("select GLJJRBH from AAA_MJMJJYZHYJJRZHGLB where Number='" + e.CommandArgument.ToString() + "'");
                string strUrl = "JHJX_FGSJYZHSH_JJR_DetailsInfo.aspx?JSBH=" + objJSBH.ToString();
                Response.Redirect(strUrl);
                break;
        }
    }
}