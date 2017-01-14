using FMOP.DB;
using Infragistics.WebUI.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;



public partial class Web_JHJX_New2013_JYGLBJK_jhjx_ycqianshoutj : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
 

           // setchengshi();
           
            Initleastmoney();
            DisGrid();
        }

    }

    private void Initleastmoney()
    {
        string Strget = "select isnull(DEYCFHDJEWY,0.00) from AAA_YJTJWHB ";
        object obj = DbHelperSQL.GetSingle(Strget);
        if(obj != null && Convert.ToDouble(obj.ToString()) !=0.00 )
        {
            txtjjrbh.Text =  Convert.ToDouble( obj.ToString()).ToString("0.00");
        }
    }


    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 15 ";
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select  b.Z_HTBH  as '合同编号' ,b.Z_SPBH as '商品编号',b.Z_SPMC as '商品名称', ('F'+a.Number) as '发货单号',a.T_THSL as '发货数量', cast(isnull(b.Z_ZBJG,0)* isnull(a.T_THSL,0) as numeric(18,2)) as '发货金额',F_DQZT as '异常签收类型' ,(case F_DQZT when '部分收货' then F_BUYBFSHCZSJ  when '有异议收货'  then F_BUYYYYSHCZSJ when '请重新发货' then F_BUYQZXFHCZSJ end) as '签收时间',b.Y_YSYDDDLYX as '买家账号', ( select  I_JYFMC from AAA_DLZHXXB where B_DLYX=Y_YSYDDDLYX) as '买家名称',(select  I_LXRXM from AAA_DLZHXXB where B_DLYX=b.Y_YSYDDDLYX) as '买家联系人', (select  I_LXRSJH from AAA_DLZHXXB where B_DLYX=b.Y_YSYDDDLYX )  as '买家联系方式',isnull( b.T_YSTBDDLYX,'无') as '卖家账号',(select  I_JYFMC from AAA_DLZHXXB where B_DLYX=b.T_YSTBDDLYX) as '卖家名称', (select  I_LXRXM from AAA_DLZHXXB where B_DLYX=b.T_YSTBDDLYX) as '卖家联系人', (select  I_LXRSJH from AAA_DLZHXXB where B_DLYX=b.T_YSTBDDLYX )  as '卖家联系方式' ,(case F_DQZT when '部分收货' then  isnull(F_BUYBFSHSJQSSL,0) when '有异议收货' then isnull(a.T_THSL,0) when '请重新发货' then 0 end ) as '收货数量', (ISNULL(a.T_THSL,0)-(case F_DQZT when '部分收货' then  isnull(F_BUYBFSHSJQSSL,0) when '有异议收货' then isnull(a.T_THSL,0) when '请重新发货' then 0 end )) as '差异数量',( case F_DQZT when '部分收货' then F_BUYBFSHFHDWLDYYJ  when '有异议收货' then  F_BUYYYYSHFHDWLDYYJ  +'★'+F_BUYYYYYSZP  when '请重新发货' then  F_BUYQZXFHYSZP  end) as '异常操作附件'  from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where F_DQZT In ( '部分收货','有异议收货','请重新发货')) as tab ";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " 发货单号 ";  //所检索表的主键
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = " 发货金额 ";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_CSSPZLSH/JHJX_CKXQ.aspx";
        return ht_where;
    }
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        //输出调试错误
        // Response.Write(ERRinfo);
        //Response.End();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            FMOP.Common.MessageBox.Show(Page, "请选择需要上传的文件！");
        }

        Repeater1.DataSource = null;

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            foreach (DataRow dr in NewDS.Tables[0].Rows)
            {
                if (dr["异常签收类型"].ToString().Equals("有异议收货"))
                {
                  
                    string[] strs = dr["异常操作附件"].ToString().Trim().Split('★');
                    if (strs != null && strs.Length > 0)
                    {
                        string url1 = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + strs[0].Trim().Replace(@"\", "/");
                        string Url2 = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + strs[1].Trim().Replace(@"\", "/");
                        dr["异常操作附件"] = "<a href = '" + url1 + "'  target='_blank'>" + "(物流单)影印件" + "</a> <br />" + "<a href = '" + Url2 + "'  target='_blank'>" + "验收照片" + "</a>";
                    }
                }
                else
                {
                    if (!dr["异常签收类型"].ToString().Equals(""))
                    {
                        string url1 = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dr["异常操作附件"].ToString().Replace(@"\", "/");
                        //string str = "<a href = '" + dr["异常操作附件"].ToString() + "'  target='_blank'>" + "(物流单)影印件" + "</a>";
                        dr["异常操作附件"] = "<a href = '" + url1 + "'  target='_blank'>" + "(物流单)影印件" + "</a>";
                    }
                }
            }
            Repeater1.DataSource = NewDS.Tables[0].DefaultView;
            tdEmpty.Visible = false;
        }
        else
        {

            Repeater1.DataSource = NewDS;
            tdEmpty.Visible = true;

        }
        Repeater1.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        if (!ddl_ycqsqk.SelectedValue.ToString().Trim().Equals("全部"))
        {
            HTwhere["search_str_where"] += " and 异常签收类型='" + ddl_ycqsqk.SelectedValue.ToString().Trim()+"'";   
        }
        if (!this.txtBeginTime.Text.Equals(""))
        {
            HTwhere["search_str_where"] += " and 签收时间 >='" + this.txtBeginTime.Text.ToString().Trim() + "'";   
        }
        if (!this.txtEndTime.Text.Equals(""))
        {
            HTwhere["search_str_where"] += " and 签收时间 <='" + this.txtEndTime.Text.ToString().Trim() + " 23:59:59.999'";
        }
        if (!this.txt_jyfmc.Text.Equals(""))
        {
            HTwhere["search_str_where"] += " and (买家名称 like '%" + this.txt_jyfmc.Text.ToString().Trim() + "%' or 卖家名称 like '%" + this.txt_jyfmc.Text.ToString().Trim() + "%')";
        }

        if (!this.txtjjrbh.Text.Equals(""))
        {
            HTwhere["search_str_where"] += " and 发货金额 >=" + (Convert.ToDouble(this.txtjjrbh.Text.ToString().Trim()) * 10000).ToString();
        }
           
      
        this.hidwhereis.Value = HTwhere["search_tbname"].ToString();
        this.hidwhere.Value = HTwhere["search_str_where"].ToString();

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }



    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!this.txtjjrbh.Text.Trim().Equals(""))
        {
            try
            {
                Convert.ToDouble(this.txtjjrbh.Text.Trim());
            }
            catch
            {
                FMOP.Common.MessageBox.Show(Page, "请输入合法的发货单最小金额！");
            }
        }
        DisGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (this.txtjjrbh.Text.Trim() != "")
        {
            try
            {
                string Strsql = "update AAA_YJTJWHB set DEYCFHDJEWY= " + Convert.ToDouble(this.txtjjrbh.Text);
                DbHelperSQL.ExecuteSql(Strsql);
                FMOP.Common.MessageBox.Show(Page, "发货单发货金额最小值设定成功！" );
            }
            catch (Exception ex)
            {
                FMOP.Common.MessageBox.Show(Page, "请输入合法的数字形式！"+ex.ToString());
            }
        }
        else
        {
            FMOP.Common.MessageBox.Show(Page, "请输入要设定的发货金额最小值！");
        }

    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
        if (this.Repeater1 != null && this.Repeater1.Items.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(" select '" + this.hidID.Value + "' as '收益时间',*  from ");
            stringBuilder.Append(" select * from  " + this.hidwhereis.Value + " where " + this.hidwhere.Value.ToString() + " order by 发货金额 desc ");
            // stringBuilder.Append(" where " + hidwhereis.Value.ToString());
            DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString());

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "异常签收统计", "异常签收统计", 15);
        }
        else
        {
            FMOP.Common.MessageBox.Show(this, "列表中没有数据可导出！");
        }
    }
}