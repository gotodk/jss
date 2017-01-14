using FMOP.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_JHJX_New2013_JYGLBJK_jhjx_qstxchaxun : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            DisGrid();
        }
    }

    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 15 ";
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select  b.Z_HTBH  as '合同编号' ,('F'+a.Number) as '发货单号', (F_QMJQSQRCZSJ) as '提醒签收时间',b.Y_YSYDDDLYX as '买家账号', ( select  I_JYFMC from AAA_DLZHXXB where B_DLYX=Y_YSYDDDLYX) as '买家名称',(select  I_LXRXM from AAA_DLZHXXB where B_DLYX=b.Y_YSYDDDLYX) as '买家联系人', (select  I_LXRSJH from AAA_DLZHXXB where B_DLYX=b.Y_YSYDDDLYX )  as '买家联系方式',isnull( b.T_YSTBDDLYX,'无') as '卖家账号',(select  I_JYFMC from AAA_DLZHXXB where B_DLYX=b.T_YSTBDDLYX) as '卖家名称', (select  I_LXRXM from AAA_DLZHXXB where B_DLYX=b.T_YSTBDDLYX) as '卖家联系人', (select  I_LXRSJH from AAA_DLZHXXB where B_DLYX=b.T_YSTBDDLYX )  as '卖家联系方式' , b.Z_SPBH as '商品编号',b.Z_SPMC as '商品名称', a.T_THSL as '发货数量', cast(isnull(b.Z_ZBJG,0)* isnull(a.T_THSL,0) as numeric(18,2)) as '发货金额',(case  when F_DQZT= '未生成发货单' or F_DQZT= '已生成发货单' or F_DQZT= '已录入发货信息'  then '未签收' when F_DQZT= '无异议收货' then '正常签收' when F_DQZT ='部分收货' or F_DQZT ='部分收货' or F_DQZT ='已录入补发备注' or F_DQZT ='补发货物无异议收货' or F_DQZT ='有异议收货' or F_DQZT ='有异议收货后无异议收货' or F_DQZT ='卖家主动退货' or F_DQZT ='请重新发货' or F_DQZT ='撤销' then '异常签收' when F_DQZT='默认无异议收货' then '默认签收' else '数据异常' end ) as '签收状态' , (select  case  when COUNT(Number) = 0 then '否' else '是' end  from AAA_DZGHTHJKTZCLB where LYSJLX = '发货单'  and JKLX = '卖家提醒买家签收' and SFCL = '是' and LYDH =('F'+a.Number) ) as '是否已处理', (case F_QMJQSWLQSD when '' then '无' else F_QMJQSWLQSD end )  as '卖家反馈签收单'  from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where  F_QMJQSQRCZSJ is not null ) as tab ";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " 发货单号 ";  //所检索表的主键
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = " 提醒签收时间 ";  //用于排序的字段
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

                if (!dr["卖家反馈签收单"].ToString().Equals("") && !dr["卖家反馈签收单"].ToString().Equals("无"))
                    {
                        string url1 = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dr["卖家反馈签收单"].ToString().Replace(@"\", "/");
                        //string str = "<a href = '" + dr["异常操作附件"].ToString() + "'  target='_blank'>" + "(物流单)影印件" + "</a>";
                        dr["卖家反馈签收单"] = "<a href = '" + url1 + "'  target='_blank'>" + "卖家反馈签收单" + "</a>";
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
        if (!ddl_qszt.SelectedValue.ToString().Trim().Equals("全部"))
        {
            HTwhere["search_str_where"] += " and 签收状态='" + ddl_qszt.SelectedValue.ToString().Trim() + "'";
        }
        if (!this.txtBeginTime.Text.Equals(""))
        {
            HTwhere["search_str_where"] += " and 提醒签收时间 >='" + this.txtBeginTime.Text.ToString().Trim() + "'";
        }
        if (!this.txtEndTime.Text.Equals(""))
        {
            HTwhere["search_str_where"] += " and 提醒签收时间 <='" + this.txtEndTime.Text.ToString().Trim() + " 23:59:59.999'";
        }
        if (!this.txt_jyfmc.Text.Equals(""))
        {
            HTwhere["search_str_where"] += " and (买家名称 like '%" + this.txt_jyfmc.Text.ToString().Trim() + "%' or 卖家名称 like '%" + this.txt_jyfmc.Text.ToString().Trim() + "%')";
        }

        if (!this.txt_fhdh.Text.Equals(""))
        {
            HTwhere["search_str_where"] += " and 发货单号 ='" + this.txt_fhdh.Text.Trim() + "'";
        }

        if (!this.ddl_sfycl.SelectedValue.ToString().Trim().Equals("全部"))
        {
            HTwhere["search_str_where"] += " and 是否已处理='" + ddl_sfycl.SelectedValue.ToString().Trim() + "'";
        }


        this.hidwhereis.Value = HTwhere["search_tbname"].ToString();
        this.hidwhere.Value = HTwhere["search_str_where"].ToString();

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
        if (this.Repeater1 != null && this.Repeater1.Items.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(" select '" + this.hidID.Value + "' as '收益时间',*  from ");
            stringBuilder.Append(" select * from  " + this.hidwhereis.Value + " where " + this.hidwhere.Value.ToString() + " order by 提醒签收时间 asc ");
            // stringBuilder.Append(" where " + hidwhereis.Value.ToString());
            DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString());

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "卖家提醒买家签收", "卖家提醒买家签收", 15);
        }
        else
        {
            FMOP.Common.MessageBox.Show(this, "列表中没有数据可导出！");
        }
    }
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "linkbj")
        {
            if (!e.CommandArgument.ToString().Trim().Equals(""))
            {
                string strget = "select  case  when COUNT(Number) = 0 then '否' else '是' end  from AAA_DZGHTHJKTZCLB where LYSJLX = '发货单'  and JKLX = '卖家提醒买家签收' and SFCL = '是' and LYDH ='" + e.CommandArgument.ToString().Trim() + "'";
                object obj = DbHelperSQL.GetSingle(strget);
                if (obj.ToString().Trim().Equals("否"))
                {
                    string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_DZGHTHJKTZCLB", "");
                    string Sql = " insert into AAA_DZGHTHJKTZCLB ( Number, JKLX,SFCL,LYSJLX,LYDH,CreateTime,CreateUser) values ( '" + Number + "','卖家提醒买家签收','是','发货单','"+e.CommandArgument.ToString().Trim()+"',getdate(),'"+User.Identity.Name.ToString()+"' ) ";
                   if(DbHelperSQL.ExecuteSql(Sql)>0)
                   {
                       FMOP.Common.MessageBox.Show(this, "数据操作成功！");
                       DisGrid();
                   }
                }
                else
                {
                    FMOP.Common.MessageBox.Show(this, "数据已经处理过请不要重复操作！");
                }
            }
            
        }
    }
}