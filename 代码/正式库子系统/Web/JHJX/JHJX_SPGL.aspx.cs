using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_JHJX_SPGL : System.Web.UI.Page
{
    //用于所属分类下拉框条件
    protected string strwhere ="" ;

    //用于无限极下拉框数据源
    DataView dv;
    //层次分隔符
    const string STR_TREENODE = "┆┄";
    //顶级父节点
    const int INT_TOPID = 0;
    //分类ID,与一下几个字段同数据库中的字段名称相同，以方便取值
    const string SortID = "SortID";
    //父分类ID
    const string SortParentID = "SortParentID";
    //分类名称
    const string SortName = "SortName";
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
           
           
            //用于下拉框ddlSSFL数据源
            dv = DbHelperSQL.Query("select SortID,SortName,SortParentID from AAA_tbMenuSPFL").Tables[0].DefaultView;
            dv.Sort = SortParentID;
            string schar = STR_TREENODE;
            if (dv.Table.Rows.Count > 0)
            {
                RecursBind(INT_TOPID, schar);
            }
         
            DisGrid();
        }

    }

    /// <summary>
    /// 递归绑定DropDownList
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="schar"></param> 
    protected void RecursBind(int pid, string schar)
    {
        DataRowView[] rows = dv.FindRows(pid);

        if (pid != 0)
        {
            schar += STR_TREENODE;
        }

        foreach (DataRowView row in rows)
        {
            this.ddlssfl.Items.Add(new ListItem(schar + row[SortName].ToString(), row[SortID].ToString()));

            RecursBind(Convert.ToInt32(row[SortID]), schar);

        }


    }
    protected void FindCSorts(string str )
    {
        
        DataTable dtc = DbHelperSQL.Query("SELECT SortID FROM AAA_tbMenuSPFL WHERE SortParentID="+str ).Tables[0];
        if (dtc != null && dtc.Rows.Count > 0)
        {
            for (int i = 0; i < dtc.Rows.Count; i++)
            {
                strwhere += "," + dtc.Rows[i]["SortID"].ToString();
                FindCSorts(dtc.Rows[i]["SortID"].ToString());
            }

        }
        else 
        {
            strwhere += "," + str;
        }

    }


    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            tdEmpty.Visible = false;
            DataTable dt = NewDS.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sortid = dt.Rows[i]["SSFLBH"].ToString();
                dt.Rows[i]["EJFL"] = GetCsort(sortid);
                dt.Rows[i]["YJFL"] = DbHelperSQL.GetSingle("select  SortName from  AAA_tbMenuSPFL where SortID=(select  SortParentID from  AAA_tbMenuSPFL where SortName='" + dt.Rows[i]["EJFL"].ToString() + "')") == null ? dt.Rows[i]["EJFL"].ToString() : DbHelperSQL.GetSingle("select  SortName from  AAA_tbMenuSPFL where SortID=(select  SortParentID from  AAA_tbMenuSPFL where SortName='" + dt.Rows[i]["EJFL"].ToString() + "')").ToString();

            }

            rptSPXX.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rptSPXX.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " *, '' AS YJFL,''AS EJFL ";
        ht_where["search_tbname"] = " AAA_PTSPXXB ";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " FBRQ ";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        //获取in条件
        FindCSorts(ddlssfl.SelectedValue.Trim());
        if (strwhere != "")
        {
            strwhere = strwhere.Remove(0, 1); 
        }
        HTwhere["search_str_where"] = HTwhere["search_str_where"] + "  and SFYX like '%" + ddlsfyx.SelectedValue.ToString() + "%' and SPMC like '%" + txtspmc.Text.Trim() + "%' and GG like '%" + txtspgg.Text.Trim() + "%'  and SSFLBH in ("+strwhere+") " ;
        

        //if (txtYHM.Text.Trim() != "")
        //{
        //    HTwhere["search_str_where"] = HTwhere["search_str_where"] + "and YHM like '%" + txtYHM.Text.Trim() + "%'";
        //}
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }
   
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void ddrFGSMC_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void rptSPXX_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lbspms = (Label)e.Item.FindControl("lbspms");
        Label lbzzmc = (Label)e.Item.FindControl("lbzzmc");
        Label lbgg = (Label)e.Item.FindControl("lbgg");
        Label lbspmc = (Label)e.Item.FindControl("lbspmc");

        if (lbspms.Text.Length > 10)
            lbspms.Text = lbspms.Text.Substring(0, 10) + "...";
        if (lbzzmc.Text.Length > 10)
            lbzzmc.Text = lbzzmc.Text.Substring(0, 10) + "...";
        if (lbgg.Text.Length > 10)
            lbgg.Text = lbgg.Text.Substring(0, 10) + "...";
        if (lbspmc.Text.Length > 10)
            lbspmc.Text = lbspmc.Text.Substring(0, 10) + "...";
    }

    /// <summary>
    /// 获取二级分类:一级分类--次级分类--三级分类--...--商品分类--商品名,以后可通过次级分类得到一级分类
    /// </summary>
    /// <param name="sortid">商品分类</param>
    /// <returns>次级分类</returns>
    protected string GetCsort(string sortid)
    { 
        string pid=  DbHelperSQL.GetSingle("select  SortParentID from  AAA_tbMenuSPFL where SortID="+sortid)==null?"":DbHelperSQL.GetSingle("select  SortParentID from  AAA_tbMenuSPFL where SortID="+sortid).ToString();
        string pidpid = DbHelperSQL.GetSingle("select  SortParentID from  AAA_tbMenuSPFL where SortID=" + pid) == null ? "" : DbHelperSQL.GetSingle("select  SortParentID from  AAA_tbMenuSPFL where SortID=" + pid).ToString();
        
        {
            if (pidpid == "0" || pidpid =="")
            {
                return DbHelperSQL.GetSingle("select  SortName from  AAA_tbMenuSPFL where SortID=" + sortid)==null?"":DbHelperSQL.GetSingle("select  SortName from  AAA_tbMenuSPFL where SortID=" + sortid).ToString();
            }
            else
            {
                return GetCsort(pid);
            }
        }
      
    }

    protected void rptSPXX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        LinkButton lb = (LinkButton)e.Item.FindControl("lsz");
        if (e.CommandName == "linkbj")
        {
           // Response.Redirect("JHJX_DLRSHXQ.aspx?JSBH=" + e.CommandArgument.ToString().Trim() + "&ck=OK");
            Response.Redirect("JHJX_SPBJ.aspx?ID=" + e.CommandArgument.ToString().Trim() );
        }
        //设置与无效
        if (e.CommandName == "linksz")
        {
            string sfyx = DbHelperSQL.GetSingle(" select SFYX from AAA_PTSPXXB where Number='" + e.CommandArgument.ToString().Trim() + "'").ToString();
            if (sfyx == "是")
            {
                DbHelperSQL.ExecuteSql("UPDATE [AAA_PTSPXXB]SET ZFRQ=GETDATE(),SFYX='否'  WHERE Number='" + e.CommandArgument.ToString().Trim() + "'");
                lb.Text = "设为有效";
            }
            else if (sfyx == "否")
            {
                DbHelperSQL.ExecuteSql("UPDATE [AAA_PTSPXXB]SET FBRQ=GETDATE(),SFYX='是' ,ZFRQ=NULL  WHERE Number='" + e.CommandArgument.ToString().Trim() + "'");
                lb.Text = "设为无效";
 
            }
           
        }

    }

   
}