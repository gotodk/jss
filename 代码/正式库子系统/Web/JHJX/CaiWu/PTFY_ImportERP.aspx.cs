using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;
using System.Collections;

public partial class Web_JHJX_PTFY_ImportERP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
          

            BindData();         

        }
    }
    private void BindData()
    {

        int day_total = 999;//每天可处理的最大数量
        int page_size = 50;//每次可处理的最大数据量
        spanDCLSJL.InnerText = "0";
        spanYCLSJL.InnerText = "0";
        spanKCLSJL.InnerText = day_total.ToString();       

        //获取所有待处理的数据信息
        //string sql_total = "select ROW_NUMBER() OVER (ORDER BY createtime) AS 序号,*,(case when isnull(ptgljg,'')='' then '失败' when isnull(I_ZQZJZH,'')='' then '失败' when isnull(dyjskmbh,'')='' then '失败' else '成功' end ) as sjyz from (select a.*,b.B_DLYX,B_JSZHLX,I_JYFMC,I_XXDZ,I_LXRXM,I_LXRSJH,I_ZQZJZH,I_ZCLB,(case when B_JSZHLX='经纪人交易账户' then I_PTGLJG else (select I_PTGLJG from AAA_MJMJJYZHYJJRZHGLB as aa left join AAA_DLZHXXB as bb on aa.GLJJRBH =bb.J_JJRJSBH where aa.SFDQMRJJR ='是' and aa.DLYX =a.dlyx) end) as ptgljg,c.dyjskmbh as dyjskmbh,d.khbh as erpkhbh from AAA_ZKLSMXB as a left join AAA_DLZHXXB as b on a.dlyx=b.B_DLYX left join AAA_JSKMDZB as c on b.I_KHYH=c.yhmc left join AAA_PTKHYZERPSJB as d on a.dlyx=d.dlyx where  a.number not in (select lydh from AAA_PTFYYZERPSJB where sjly='账款流水明细表' ) and xm in ('技术服务费','账户管理费','违规罚款')) as tab order by createtime";//2013-10-11因修改交易方平台管理机构获取方式作废。

        string sql_total = "select ROW_NUMBER() OVER (ORDER BY createtime) AS 序号,*,(case when isnull(ptgljg,'')='' then '失败' when isnull(I_ZQZJZH,'')='' then '失败' when isnull(dyjskmbh,'')='' then '失败' else '成功' end ) as sjyz from (select a.*,b.B_DLYX,B_JSZHLX,I_JYFMC,I_XXDZ,I_LXRXM,I_LXRSJH,I_ZQZJZH,I_ZCLB,I_PTGLJG as ptgljg,c.dyjskmbh as dyjskmbh,d.khbh as erpkhbh from AAA_ZKLSMXB as a left join AAA_DLZHXXB as b on a.dlyx=b.B_DLYX left join AAA_JSKMDZB as c on b.I_KHYH=c.yhmc left join AAA_PTKHYZERPSJB as d on a.dlyx=d.dlyx where  a.number not in (select lydh from AAA_PTFYYZERPSJB where sjly='账款流水明细表' ) and xm in ('技术服务费','账户管理费','违规罚款')) as tab order by createtime";
        
        DataSet ds_total = DbHelperSQL.Query(sql_total);    

        spanDCLSJL.InnerText = ds_total.Tables[0].Rows.Count.ToString();//待处理数据量

        //获取今天已经处理的数据信息
        string sql_ycl = "select * from AAA_PTFYYZERPSJB where convert(varchar(10),createtime,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
        DataSet ds_ycl = DbHelperSQL.Query(sql_ycl);

        spanYCLSJL.InnerText = ds_ycl.Tables[0].Rows.Count.ToString();//已处理数据量

        int kclsjl=day_total - ds_ycl.Tables[0].Rows.Count;//还可处理数据量
        spanKCLSJL.InnerText = kclsjl.ToString();//还可处理数据量

        DataTable dt_topN = new DataTable();
        if (ds_total != null && ds_total.Tables[0].Rows.Count > 0)
        {
            if (kclsjl > page_size)
            {
                dt_topN = DtSelectTop(page_size, ds_total.Tables[0]);
            }
            else if (kclsjl > 0)
            {
                dt_topN = DtSelectTop(kclsjl, ds_total.Tables[0]);
            }
            else
            {
                lblk.InnerText = "今日转入ERP数据已满" + day_total + "条，当前还剩余"+spanDCLSJL .InnerText.ToString ()+"条数据，请明天处理！";
                   
            }
            IsConvert.Value = "1";        //可以显示按钮   
        }
        else
        {         
            IsConvert.Value = "0";//没有数据需要显示
        }
        if (dt_topN != null && dt_topN.Rows.Count > 0)
        {
            rpt.DataSource = dt_topN.DefaultView;
            rpt.DataBind();
            if (dt_topN.Select("sjyz='失败'").Length > 0)
            {
                IsConvert.Value = "3";//数据验证没有成功
            }

            //给导出用的数据表赋值
            DataTable dtPrint = dt_topN.Copy();
            rptPrint.DataSource = dtPrint.DefaultView;
            rptPrint.DataBind();
        }
        else
        {
            tdEmpty.Visible = true;
        }
       
        ViewState["dt_topN"] = dt_topN;
    }

    //获取dataset中的top N条数据
    public  DataTable DtSelectTop(int TopItem, DataTable oDT)
    {
        if (oDT.Rows.Count < TopItem) return oDT;

        DataTable NewTable = oDT.Clone();
        DataRow[] rows = oDT.Select("1=1");
        for (int i = 0; i < TopItem; i++)
        {
            NewTable.ImportRow((DataRow)rows[i]);
        }
        return NewTable;
    }  


    //验证客户信息是否在ERP中存在，不存在的导入ERP客户档案中
    protected void btnValidate_Click(object sender, EventArgs e)
    {
        ArrayList al_ERPInsert = new ArrayList();
        ArrayList al_FMOPInsert = new ArrayList();      

        DataTable dt = (DataTable)ViewState["dt_topN"];
        if (dt.Select("isnull(sjyz,'')=''").Length > 0)
        {
            MessageBox.ShowAlertAndBack(this, "数据列表中有字段为空的数据，请核对后再进行操作！");
        }
        if (dt != null && dt.Rows.Count > 0)
        {
            //获取本批数据中不同的客户
            DataTable dt_khxx = dt.DefaultView.ToTable(true, new string[] { "B_DLYX", "B_JSZHLX", "I_JYFMC", "I_XXDZ", "I_LXRXM", "I_LXRSJH", "I_ZQZJZH", "ptgljg", "I_ZCLB", "erpkhbh", });

            string no_gljg = "";//用于判断该部门是否在ERP中存在
            if (dt_khxx.Select("isnull(erpkhbh,'')=''").Length > 0)
            {
                foreach (DataRow dr in dt_khxx.Rows)
                {
                    if (dr["erpkhbh"].ToString().Trim() == "")
                    {
                        //导入ERP客户档案中
                        //获取平台管理机构对应的编号
                        string StrErpSql = " select ME001 from  CMSME where ME002 like '%" + dr["ptgljg"].ToString().Trim() + "%'";
                        DataSet ds = GetErpData.GetDataSet(StrErpSql, "公司总部");
                        string gljgbh = "";
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            gljgbh = ds.Tables[0].Rows[0]["ME001"].ToString();
                        }
                        else
                        {
                            no_gljg += "/" + dr["ptgljg"].ToString() + "/";
                        }

                        string qd = dr["I_ZCLB"].ToString() == "自然人" ? "111" : "110";//渠道
                        string dwjc = dr["I_JYFMC"].ToString().Trim().Length <= 9 ? dr["I_JYFMC"].ToString() : dr["I_JYFMC"].ToString().Trim().Substring(0, 9);//单位简称,ERP最大char(20)
                        string dwqc = dr["I_JYFMC"].ToString().Trim().Length <= 35 ? dr["I_JYFMC"].ToString() : dr["I_JYFMC"].ToString().Trim().Substring(0, 35);//单位全称，ERP最大char(72)
                        string lxrsjh = dr["I_LXRSJH"].ToString().Trim().Length <= 14 ? dr["I_LXRSJH"].ToString() : dr["I_JYFMC"].ToString().Trim().Substring(0, 14);//联系电话，ERP最大char(20)
                        string xxdz = dr["I_XXDZ"].ToString().Trim().Length <= 35 ? dr["I_XXDZ"].ToString().Trim() : dr["I_XXDZ"].ToString().Trim().Substring(0, 35);//地址，ERP最大char(72)

                        string sql_erp = "insert into COPMA( COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG,MA001,MA002,MA003,MA005,MA006,MA007,MA009,MA014,MA015,MA016,MA017,MA023,MA024,MA027,MA035,MA037,MA038,MA039,MA041,MA042,MA048,MA064,MA066,MA075,MA084,MA085,MA087,MA089,MA101,MA032,MA091,MA092,MA093,MA094,MA088) values ( 'fm','DS','','" + DateTime.Now.ToString("yyyymmddhhmmss").Trim() + "','1','" + dr["I_ZQZJZH"].ToString().Trim() + "','" + dwjc + "','" + dwqc + "','" + dr["I_LXRXM"].ToString().Trim() + "','" + lxrsjh + "','" + lxrsjh + "','" + dr["B_DLYX"].ToString().Trim() + "','RMB','" + gljgbh + "','fm','" + qd + "','" + xxdz + "','" + dr["I_LXRXM"].ToString().Trim() + "，" + lxrsjh + "','" + xxdz + "','2','A','1','1','2','1','1','" + dr["I_LXRXM"].ToString().Trim() + "，" + lxrsjh + "','N','142','N','fm','2','3','0.1700','y','1.0000','1.0000','1.0000','1.0000','2')";
                        al_ERPInsert.Add(sql_erp);

                        WorkFlowModule WFM = new WorkFlowModule("AAA_PTKHYZERPSJB");
                        string KeyNumber = WFM.numberFormat.GetNextNumber();
                        string sql_fmop = "INSERT INTO [AAA_PTKHYZERPSJB]([Number],[PTGLJG] ,[DLYX],[KHBH],[DWMC],[JYZHLX],[ZCLB],[CheckState],[CreateUser],[CreateTime]) VALUES ('" + KeyNumber + "','" + dr["ptgljg"].ToString() + "' ,'" + dr["B_DLYX"].ToString() + "','" + dr["I_ZQZJZH"].ToString() + "','" + dr["I_JYFMC"].ToString() + "','" + dr["B_JSZHLX"].ToString() + "','" + dr["I_ZCLB"].ToString() + "' ,1,'" + User.Identity.Name.ToString() + "','" + DateTime.Now.ToString() + "')";
                        al_FMOPInsert.Add(sql_fmop);
                    }
                }


                if (no_gljg.Trim() != "")
                {
                    MessageBox.ShowAlertAndBack(this, "客户信息验证失败，平台管理机构" + no_gljg + "在ERP中不存在！");
                }
                if (al_ERPInsert.Count > 0 && al_FMOPInsert.Count > 0 && al_ERPInsert.Count == al_FMOPInsert.Count)
                {
                    bool success = GetErpData.ExecuteSqlTran(al_ERPInsert, al_FMOPInsert, "公司总部");

                    if (success == true)
                    {
                        MessageBox.Show(this, "客户信息验证成功，所有客户信息均已在ERP中存在！\\n可以进行“转入ERP收款单”操作了！");
                        BindData();
                    }
                    else
                    {
                        MessageBox.ShowAlertAndBack(this, "客户信息验证失败！");
                    }
                }
            }
            else
            {
                MessageBox.ShowAlertAndBack(this, "客户信息验证成功，所有客户都已在ERP中存在！\\n可以进行“转入ERP收款单”操作了！");
            }
        }
    }
    //转入ERP收款单的操作
    protected void btnConvert_Click(object sender, EventArgs e)
    {
        bool canpass = true;//客户信息是否存在
        DataTable dt = (DataTable)ViewState["dt_topN"];
        if (dt == null || dt.Rows.Count <= 0)
        {
            MessageBox.ShowAlertAndBack(this, "没有需要转入ERP收款的数据！");
        }
        if (dt.Select("sjyz='失败'").Length > 0)
        {
            MessageBox.ShowAlertAndBack(this, "列表中存在不满足导入条件的数据，请核对修改后重试！");
        }
        if (dt.Select("isnull(erpkhbh,'')=''").Length > 0)
        {
            MessageBox.ShowAlertAndBack(this, "列表中部分客户在ERP中不存在，请先进行“验证客户信息”操作！");
        }
        ArrayList al_erp = new ArrayList();
        ArrayList al_fmop = new ArrayList();
        foreach (DataRow dr in dt.Rows)
        {
            string sql_khxx = "select MA001 as 客户编号,MA016 as 收款业务员,MA015 as 部门,MA014 as 币种,MA041 as 结算方式,MA047 as 账款科目,MA002 as 客户简称 from COPMA where MA001='" + dr["I_ZQZJZH"].ToString().Trim() + "'";
            DataTable dtA = GetErpData.GetDataSet(sql_khxx, "公司总部").Tables[0];//客户在ERP中的信息
            if (dtA == null || dtA.Rows.Count <= 0)
            {
                canpass = false;
            }
            string bz=dr["I_JYFMC"].ToString()+dr["XM"].ToString();

            string SqlA = "INSERT INTO ACRTK(COMPANY,CREATOR,USR_GROUP,CREATE_DATE,MODIFIER,MODI_DATE,FLAG,";
            SqlA = SqlA + "TK001,TK002,TK003,TK004,TK005,TK006,TK007,TK008,TK009,TK010,";
            SqlA = SqlA + "TK011,TK012,TK013,TK014,TK015,TK016,TK017,TK018,TK019,TK020,";
            SqlA = SqlA + "TK021,TK022,TK023,TK024,TK025,TK026,TK027,TK028,TK029,TK030,";
            SqlA = SqlA + "TK031,TK032,TK033,TK034,TK035,TK036,TK037,TK038,TK039,TK040,";
            SqlA = SqlA + "TK041,TK042,UDF01) VALUES ";
            SqlA = SqlA + "('fm','DS','','" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + "','','','1',";
            SqlA = SqlA + "'633',dbo.getTableID('633','ACRTK'),'" + System.DateTime.Now.ToString("yyyyMMdd") + "','" + dr["I_ZQZJZH"].ToString().Trim() + "','" + dtA.Rows[0]["收款业务员"].ToString() + "','" + dtA.Rows[0]["部门"].ToString() + "','" + dtA.Rows[0]["币种"].ToString() + "','1.0000000','"+bz+"','" + dtA.Rows[0]["结算方式"].ToString() + "',";
            SqlA = SqlA + "'','','" + dr["dyjskmbh"].ToString() + "','','','112205','','','0','N',";
            SqlA = SqlA + "'N','" + System.DateTime.Now.ToString("yyyyMMdd") + "','','N','','','0','N','','1',";
            SqlA = SqlA + "'N'," + dr["JE"].ToString() + "," + dr["JE"].ToString() + ",'0.00','0.00','0.00','0.00','0.00','0.0000000','',";
            SqlA = SqlA + "'0.00','','" + dr["number"].ToString() + "')";
            al_erp.Add(SqlA);

            WorkFlowModule WFM = new WorkFlowModule("AAA_PTFYYZERPSJB");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            string sqlB = "INSERT INTO [AAA_PTFYYZERPSJB] ([Number],[PTGLJG] ,[DLYX],[KHBH],[DWMC],[KXLX],[KXJE],[SJLY],[LYDH],[CheckState],[CreateUser],[CreateTime])VALUES('" + KeyNumber + "','" + dr["ptgljg"].ToString() + "','" + dr["B_DLYX"].ToString() + "','" + dr["I_ZQZJZH"].ToString() + "','" + dr["I_JYFMC"].ToString() + "','" + dr["XM"].ToString() + "'," + dr["JE"].ToString() + ",'账款流水明细表','" + dr["number"].ToString() + "',1,'" + User.Identity.Name.ToString() + "','" + DateTime.Now.ToString() + "')";
            al_fmop.Add(sqlB);

        }
        if (canpass == false)
        { 
            MessageBox .ShowAlertAndBack(this,"有客户信息在ERP中不存在，无法执行操作！");
        }
        if (al_erp.Count > 0 && al_fmop.Count > 0 && al_erp.Count == al_fmop.Count)
        {
            bool success = GetErpData.ExecuteSqlTran(al_erp, al_fmop, "公司总部");

            if (success == true)
            {
                MessageBox.Show(this, "数据已成功导入ERP收款单，请进入ERP进行审核！");
                BindData();
            }
            else
            {
                MessageBox.ShowAlertAndBack(this, "数据转入ERP收款单操作失败！");
            }
        }

    }
    //刷新页面数据
    protected void btnShuaXin_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        ToExcel(divDisplay);
    }
    protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblDWMC = (Label)e.Item.FindControl("lblDWMC");      
        if (lblDWMC.Text.Length > 10)
            lblDWMC.Text = lblDWMC.Text.Substring(0, 10) + "...";

        Label lblPTGLJG = (Label)e.Item.FindControl("lblPTGLJG");
        if (lblPTGLJG.Text.Length > 6)
        {
            lblPTGLJG.Text = lblPTGLJG.Text.Substring(0, 6) + "...";
        }
    }

    /// <summary>
    /// 将数据导出到excel
    /// </summary>
    /// <param name="ctl"></param>
    public void ToExcel(System.Web.UI.Control ctl)
    {
        //   HttpContext.Current.Response.Charset   ="GB2312";   
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=ConvertToERP.xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   
        ctl.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        ctl.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
    }
}