using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Data;

public partial class Web_YGSYDNXXCK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpager1.OnNeedLoadData += new Web_pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if(!IsPostBack)
        {
            DisGrid();
            BindBM();//绑定总部所有部门
            YGSYDNDQ();//判断员工使用电脑是否有到期的
        }        
    }
    //绑定总部所有部门
    protected void BindBM()
    {
        string strSQL = "select distinct DeptName from dbo.HR_Dept where Superior='公司总部'";//查询总部所有部门
        DataSet dsBM = DbHelperSQL.Query(strSQL);
        //搜索处所绑定的部门
        drpSSBM.DataSource = null;
        drpSSBM.Items.Clear();
        drpSSBM.DataSource = dsBM;
        drpSSBM.DataTextField = "DeptName";
        drpSSBM.DataValueField = "DeptName";
        drpSSBM.DataBind();
        drpSSBM.Items.Insert(0,new ListItem("所有部门",""));
        drpSSBM.Items.Add(new ListItem("库存区", "库存区"));
        drpSSBM.Items.Add(new ListItem("报废区", "报废区"));
        drpSSBM.Items.Add(new ListItem("财务中心(百仕加)", "财务中心(百仕加)"));
        //资产转移所绑定的部门
        drpZYH.DataSource = null;
        drpZYH.Items.Clear();
        drpZYH.DataSource = dsBM;
        drpZYH.DataTextField = "DeptName";
        drpZYH.DataValueField = "DeptName";
        drpZYH.DataBind();
        drpZYH.Items.Add(new ListItem("库存区", "库存区"));
        drpZYH.Items.Add(new ListItem("报废区", "报废区"));
        drpZYH.Items.Add(new ListItem("财务中心(百仕加)", "财务中心(百仕加)"));
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " YGSYDNXXB";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = "CreateTime";  //用于排序的字段
        return ht_where;
    }
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.Show(this, "查询超时，请重试或者修改查询条件！");
        }

        RadGrid1.DataSource = null;

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            for (int i = 0; i < NewDS.Tables[0].Rows.Count;i++ )
            {
                NewDS.Tables[0].Rows[i]["NW"] = NewDS.Tables[0].Rows[i]["NW"].ToString() == "是" ? "√" : "×";
                NewDS.Tables[0].Rows[i]["WW"] = NewDS.Tables[0].Rows[i]["WW"].ToString() == "是" ? "√" : "×";
                NewDS.Tables[0].Rows[i]["USB"] = NewDS.Tables[0].Rows[i]["USB"].ToString() == "是" ? "√" : "×";
                NewDS.Tables[0].Rows[i]["GQ"] = NewDS.Tables[0].Rows[i]["GQ"].ToString() == "是" ? "√" : "×";
                NewDS.Tables[0].Rows[i]["DNGLY"] = NewDS.Tables[0].Rows[i]["DNGLY"].ToString() == "是" ? "√" : "×";
            }
            RadGrid1.DataSource = NewDS.Tables[0].DefaultView;
            //GV_DC.DataSource = NewDS.Tables[0].DefaultView;
        }
        else
        {
            RadGrid1.DataSource = DbHelperSQL.Query("select * from YGSYDNXXB where 1!=1");
            //GV_DC.DataSource = DbHelperSQL.Query("select * from YGSYDNXXB where 1!=1");
        }
        RadGrid1.DataBind();        
        //GV_DC.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        string bm = drpSSBM.SelectedValue.Trim();//所属部门
        string syr = txtSYR.Text.Trim();//使用人
        string ip = txtIP1.Text.Trim() + "%.%" + txtIP2.Text.Trim() + "%.%" + txtIP3.Text.Trim() + "%.%" + txtIP4.Text.Trim(); ;//IP地址
        string nw = drpNW.SelectedValue.Trim();//内网
        string ww = drpWW.SelectedValue.Trim();//外网
        HTwhere["search_str_where"] += "and SSBM like '%" + bm + "%' and SYR like '%" + syr + "%' and IPDZ like '%" + ip + "%' and NW like '%"+nw
            +"%' and WW like '%"+ww+"%'";

        string strSQL = "select * from YGSYDNXXB where SSBM like '%" + bm + "%' and SYR like '%" + syr + "%' and IPDZ like '%" + ip + "%' and NW like '%" + nw + "%' and WW like '%" + ww + "%'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i]["NW"] = ds.Tables[0].Rows[i]["NW"].ToString() == "是" ? "√" : "×";
                ds.Tables[0].Rows[i]["WW"] = ds.Tables[0].Rows[i]["WW"].ToString() == "是" ? "√" : "×";
                ds.Tables[0].Rows[i]["USB"] = ds.Tables[0].Rows[i]["USB"].ToString() == "是" ? "√" : "×";
                ds.Tables[0].Rows[i]["GQ"] = ds.Tables[0].Rows[i]["GQ"].ToString() == "是" ? "√" : "×";
                ds.Tables[0].Rows[i]["DNGLY"] = ds.Tables[0].Rows[i]["DNGLY"].ToString() == "是" ? "√" : "×";
            }
            GV_DC.DataSource = ds.Tables[0].DefaultView;
        }
        else
        {
            GV_DC.DataSource = null;
        }
        GV_DC.DataBind();
        commonpager1.HTwhere = HTwhere;
        commonpager1.GetFYdataAndRaiseEvent();
    }
    //查询按钮
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    //修改、资产转移、资产转移历史记录、删除
    protected void RadGrid1_ItemCommand(object source, Telerik.WebControls.GridCommandEventArgs e)
    {
        if (e.CommandArgument == "xg")//修改
        {
            string strURL = "YGSYDNXXXZ.aspx?Number="+e.CommandName.ToString();
            Response.Redirect(strURL);
            Response.End();
        }
        else if (e.CommandArgument == "zczy")//资产转移
        {
            divZCZY.Visible = true;
            lblZCZY.Text = e.CommandName.ToString();
            BindBM();//绑定总部所有部门
            string strSQL = "select SSBM from YGSYDNXXB where Number='"+e.CommandName.ToString()+"'";
            object bm = DbHelperSQL.GetSingle(strSQL);
            txtZYQ.Text = bm.ToString();
        
        }
        else if (e.CommandArgument == "ck")//资产转移查看
        {
            divZCZYCK.Visible = true;
            lblFXH.Text = e.CommandName.ToString();//使用人的序号
            BindZCZYCK(e.CommandName.ToString());//绑定资产转移历史记录
        }
        else if (e.CommandArgument == "del")//删除
        {
            string strSQLDel = "DELETE FROM [dbo].[YGSYDNXXB] WHERE Number ='" + e.CommandName.ToString() + "'";
            int i = DbHelperSQL.ExecuteSql(strSQLDel);
            if (i > 0)
            {
                MessageBox.Show(this, "删除成功！");
                DisGrid();
            }
            else
            {
                MessageBox.Show(this, "删除失败！");
            }
        }
        else if (e.CommandArgument == "ckxq")
        {
            divCKXQ.Visible = true;
            lblSYXQ.Text = e.CommandName.ToString();
            string strSQLXQ = "select * from YGSYDNXXB where Number='" + e.CommandName.ToString() + "'";
            DataSet ds = DbHelperSQL.Query(strSQLXQ);
            if(ds.Tables[0].Rows.Count>0)
            {
                for(int i=0; i<ds.Tables[0].Rows.Count; i++)
                {
                    lblssbm.Text = ds.Tables[0].Rows[i]["SSBM"].ToString();
                    lblXQsyr.Text = ds.Tables[0].Rows[i]["SYR"].ToString();
                    lblIP.Text = ds.Tables[0].Rows[i]["IPDZ"].ToString();
                    lblMAC.Text = ds.Tables[0].Rows[i]["MACDZ"].ToString();
                    lblPZ.Text = ds.Tables[0].Rows[i]["PZXX"].ToString();
                    lblNW.Text = ds.Tables[0].Rows[i]["NW"].ToString() == "是" ? "√" : "×";
                    lblNWTime.Text = ds.Tables[0].Rows[i]["NWSYQX"].ToString();
                    lblWW.Text = ds.Tables[0].Rows[i]["WW"].ToString() == "是" ? "√" : "×";
                    lblWWTime.Text = ds.Tables[0].Rows[i]["WWSYQX"].ToString();
                    lblUSB.Text = ds.Tables[0].Rows[i]["USB"].ToString() == "是" ? "√" : "×";
                    lblUSBTime.Text = ds.Tables[0].Rows[i]["USBSYQX"].ToString();
                    lblGQ.Text = ds.Tables[0].Rows[i]["GQ"].ToString() == "是" ? "√" : "×";
                    lblGQTime.Text = ds.Tables[0].Rows[i]["GQSYQX"].ToString();
                    lblGLY.Text = ds.Tables[0].Rows[i]["DNGLY"].ToString() == "是" ? "√" : "×";
                    lblGLYTime.Text = ds.Tables[0].Rows[i]["DNGLYSYQX"].ToString();
                    lblCreateUser.Text = ds.Tables[0].Rows[i]["CreateUser"].ToString();
                    lblCreateTime.Text = ds.Tables[0].Rows[i]["CreateTime"].ToString();
                    lblLastUpdateUser.Text = ds.Tables[0].Rows[i]["ZHYCGXR"].ToString();
                    lblLastUpdateTime.Text = ds.Tables[0].Rows[i]["ZHYCGXSJ"].ToString();
                    lblLastZYTime.Text = ds.Tables[0].Rows[i]["ZHYCZCZYSJ"].ToString();
                    lblBZ.Text = ds.Tables[0].Rows[i]["BZ"].ToString();
                    lbltxsj.Text = ds.Tables[0].Rows[i]["CreateTime"].ToString();
                    lblXQxh.Text = ds.Tables[0].Rows[i]["Number"].ToString();
                }
            }

        }
        else if (e.CommandArgument == "nw")//内网
        {
            divSYQX.Visible = true;
            lblXH.Text = e.CommandName.ToString();
            lblSYQX.Text = "内网";
            string strSQLSYQX = "select NWSYQX from YGSYDNXXB where Number='"+e.CommandName.ToString()+"'";
            object syqx = DbHelperSQL.GetSingle(strSQLSYQX);
            if (syqx.ToString() == "")
            {
                txtSYQX.Text = "未开通";
            }
            else
            {
                txtSYQX.Text = syqx.ToString();
            }            
        }
        else if (e.CommandArgument == "ww")//外网
        {
            divSYQX.Visible = true;
            lblXH.Text = e.CommandName.ToString();
            lblSYQX.Text = "外网";
            string strSQLSYQX = "select WWSYQX from YGSYDNXXB where Number='" + e.CommandName.ToString() + "'";
            object syqx = DbHelperSQL.GetSingle(strSQLSYQX);
            if (syqx.ToString() == "")
            {
                txtSYQX.Text = "未开通";
            }
            else
            {
                txtSYQX.Text = syqx.ToString();
            }            
        }
        else if (e.CommandArgument == "usb")//usb
        {
            divSYQX.Visible = true;
            lblXH.Text = e.CommandName.ToString();
            lblSYQX.Text = "USB";
            string strSQLSYQX = "select USBSYQX from YGSYDNXXB where Number='" + e.CommandName.ToString() + "'";
            object syqx = DbHelperSQL.GetSingle(strSQLSYQX);
            if (syqx.ToString() == "")
            {
                txtSYQX.Text = "未开通";
            }
            else
            {
                txtSYQX.Text = syqx.ToString();
            }            
        }
        else if (e.CommandArgument == "gq")//光驱
        {
            divSYQX.Visible = true;
            lblXH.Text = e.CommandName.ToString();
            lblSYQX.Text = "光驱";
            string strSQLSYQX = "select GQSYQX from YGSYDNXXB where Number='" + e.CommandName.ToString() + "'";
            object syqx = DbHelperSQL.GetSingle(strSQLSYQX);
            if (syqx.ToString() == "")
            {
                txtSYQX.Text = "未开通";
            }
            else
            {
                txtSYQX.Text = syqx.ToString();
            }            
        }
        else if (e.CommandArgument == "gly")//管理员
        {
            divSYQX.Visible = true;
            lblXH.Text = e.CommandName.ToString();
            lblSYQX.Text = "管理员";
            string strSQLSYQX = "select DNGLYSYQX from YGSYDNXXB where Number='" + e.CommandName.ToString() + "'";
            object syqx = DbHelperSQL.GetSingle(strSQLSYQX);
            if (syqx.ToString() == "")
            {
                txtSYQX.Text = "未开通";
            }
            else
            {
                txtSYQX.Text = syqx.ToString();
            }  
        }
    }
    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.Charset = "";
        string filename = "YGSYDNXX" + System.DateTime.Now.ToString("_yyyyMMddHHmm");
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" +

        System.Web.HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8) + ".xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   
        ctl.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        ctl.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
    }
    // 导出Exel时需重载此方法
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    //导出按钮
    protected void btnExport_Click(object sender, EventArgs e)
    {
        #region//设定导出的字段
        //填写时间
        if (chkCreateTime.Checked == false)
        {
            int i =  int.Parse(chkCreateTime.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //序号
        if (chkNumber.Checked == false)
        {
            int i = int.Parse(chkNumber.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //所属部门
        if (chkSSBM.Checked == false)
        {
            int i = int.Parse(chkSSBM.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //使用人
        if (chkSYR.Checked == false)
        {
            int i = int.Parse(chkSYR.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //IP地址
        if (chkIP.Checked == false)
        {
            int i = int.Parse(chkIP.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //MAC地址
        if (chkMAC.Checked == false)
        {
            int i = int.Parse(chkMAC.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //配置信息
        if (chkPZ.Checked == false)
        {
            int i = int.Parse(chkPZ.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //创建人
        if (chkCreateUser.Checked == false)
        {
            int i = int.Parse(chkCreateUser.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //内网
        if (chkNW.Checked == false)
        {
            int i = int.Parse(chkNW.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //内网使用期限
        if (chkNWTime.Checked == false)
        {
            int i = int.Parse(chkNWTime.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //外网
        if (chkWW.Checked == false)
        {
            int i = int.Parse(chkWW.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //外网使用期限
        if (chkWWTime.Checked == false)
        {
            int i = int.Parse(chkWWTime.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //USB
        if (chkUSB.Checked == false)
        {
            int i = int.Parse(chkUSB.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //USB使用期限
        if (chkUSBTime.Checked == false)
        {
            int i = int.Parse(chkUSBTime.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //光驱
        if (chkGQ.Checked == false)
        {
            int i = int.Parse(chkGQ.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //光驱使用期限
        if (chkGQTime.Checked == false)
        {
            int i = int.Parse(chkGQTime.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //管理员
        if (chkGLY.Checked == false)
        {
            int i = int.Parse(chkGLY.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //管理员使用期限
        if (chkGLYTime.Checked == false)
        {
            int i = int.Parse(chkGLYTime.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //最后一次更新人
        if (chkLastUpateUser.Checked == false)
        {
            int i = int.Parse(chkLastUpateUser.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //最后一次更新时间
        if (chkLastUpateTime.Checked == false)
        {
            int i = int.Parse(chkLastUpateTime.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //最后一次资产转移时间
        if (chkLastZCZYTime.Checked == false)
        {
            int i = int.Parse(chkLastZCZYTime.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        //备注
        if (chkBZ.Checked == false)
        {
            int i = int.Parse(chkBZ.Value.ToString());
            GV_DC.Columns[i].Visible = false;
        }
        #endregion
        DisGrid();
        ToExcel(GV_DC);
    }
    //资产转移上的确定按钮
    protected void OKButton_Click(object sender, EventArgs e)
    {
        ArrayList arylist = new ArrayList();
        string strSQL = "UPDATE [dbo].[YGSYDNXXB] SET [SSBM] = '"+drpZYH.SelectedValue.ToString()+"',[ZHYCGXR] = '"+User.Identity.Name.ToString()
            + "',[ZHYCGXSJ] = GETDATE(),[ZHYCZCZYSJ] = GETDATE() WHERE [Number] = '"+ lblZCZY.Text.Trim()+"'";
        arylist.Add(strSQL);
        string strSQLInsert = "INSERT INTO [dbo].[YGSYDNXXB_ZCZYJLB]([parentNumber] ,[ZCZYQSSBM] ,[ZCZYHSSBM],[CZR] ,[ZCZYSJ]) VALUES('"+lblZCZY.Text.Trim()
            + "','" + txtZYQ.Text.Trim() + "','" + drpZYH.SelectedValue.Trim() + "','" + User.Identity.Name.ToString() + "',GETDATE())";
        arylist.Add(strSQLInsert);
        bool bl = DbHelperSQL.ExecSqlTran(arylist);
        if (bl)
        {
            MessageBox.Show(this, "资产转移成功！");
            divZCZY.Visible = false;
            DisGrid();
        }
        else
        {
            MessageBox.Show(this, "资产转移失败！");
        }
    }
    //资产转移上的取消按钮
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divZCZY.Visible = false;
    }
    //资产转移查看上的取消按钮
    protected void btnGB_Click1(object sender, EventArgs e)
    {
        divZCZYCK.Visible = false;
    }
    //资产转移查看历史记录的上一页、下一页
    protected void RadGrid2_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
        BindZCZYCK(lblFXH.Text.ToString());//绑定资产转移历史记录
    }
    //绑定资产转移历史记录
    protected void BindZCZYCK(string parentNumber)
    {
        string strSQL = "select * from dbo.YGSYDNXXB_ZCZYJLB where parentNumber='" + parentNumber.ToString() + "' order by ZCZYSJ desc";
        DataSet ds = DbHelperSQL.Query(strSQL);
        if (ds.Tables[0].Rows.Count > 0)
        {
            RadGrid2.DataSource = ds.Tables[0].DefaultView;
        }
        else
        {
            RadGrid2.DataSource = DbHelperSQL.Query("select * from dbo.YGSYDNXXB_ZCZYJLB where 1!=1");
        }
        RadGrid2.DataBind();
    }
    //使用期限上的取消按钮
    protected void btnSYQX_Click(object sender, EventArgs e)
    {
        divSYQX.Visible = false;
    }
    //查看详情上的取消按钮
    protected void btnXQQX_Click(object sender, EventArgs e)
    {
        divCKXQ.Visible = false;
    }
    //选择导出字段按钮
    protected void btnDCZD_Click(object sender, EventArgs e)
    {
        divDCZD.Visible = true;
    }
    //选择导出字段div中的取消按钮
    protected void btnQX_Click(object sender, EventArgs e)
    {
        #region//设定导出的字段
        //填写时间
        chkCreateTime.Checked = false;
        //序号
        chkNumber.Checked = false;
        //所属部门
        chkSSBM.Checked = false;
        //使用人
        chkSYR.Checked = false;
        //IP地址
        chkIP.Checked = false;
        //MAC地址
        chkMAC.Checked = false;
        //配置信息
        chkPZ.Checked = false;
        //创建人
        chkCreateUser.Checked = false;
        //内网
        chkNW.Checked = false;
        //内网使用期限
        chkNWTime.Checked = false;
        //外网
        chkWW.Checked = false;
        //外网使用期限
        chkWWTime.Checked = false;
        //USB
        chkUSB.Checked =false;
        //USB使用期限
        chkUSBTime.Checked = false;
        //光驱
        chkGQ.Checked = false;
        //光驱使用期限
        chkGQTime.Checked = false;
        //管理员
        chkGLY.Checked = false;
        //管理员使用期限
        chkGLYTime.Checked = false;
        //最后一次更新人
        chkLastUpateUser.Checked = false;
        //最后一次更新时间
        chkLastUpateTime.Checked = false;
        //最后一次资产转移时间
        chkLastZCZYTime.Checked = false;
        //备注
        chkBZ.Checked = false;
        //全选
        chkALL.Checked = false;
        #endregion
        divDCZD.Visible = false;
    }
    //到期提醒的取消按钮
    protected void btnDQCancle_Click(object sender, EventArgs e)
    {
        divDQ.Visible = false;
    }
    //判断员工使用电脑是否有到期的
    protected void YGSYDNDQ()
    {
        string strSQL = "select count(*) from(select * from(select * from(select * from(select * from(select Number as NWNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(NWSYQX),charindex('~',reverse(NWSYQX))-1)),106)) as nwd from YGSYDNXXB where NWSYQX<>'长期' and NWSYQX!='') as a full outer join(select Number as WWNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(WWSYQX),charindex('~',reverse(WWSYQX))-1)),106)) as wwd from YGSYDNXXB where WWSYQX <>'长期' and WWSYQX!='') as b on a.NWNumber = b.WWNumber) as c full outer join(select Number as USBNumber,datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(USBSYQX),charindex('~',reverse(USBSYQX))-1)),106)) as usbd from YGSYDNXXB where USBSYQX <>'长期' and USBSYQX!='') as d on c.NWNumber=d.USBNumber or c.WWNumber=d.USBNumber) as e full outer join(select Number as GQNumber,datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(GQSYQX),charindex('~',reverse(GQSYQX))-1)),106)) as gqd from YGSYDNXXB where GQSYQX <>'长期' and GQSYQX!='') as f on e.NWNumber=f.GQNumber or e.WWNumber=f.GQNumber or e.USBNumber=f.GQNumber) as g full outer join(select Number as GLYNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(DNGLYSYQX),charindex('~',reverse(DNGLYSYQX))-1)),106)) as glyd from YGSYDNXXB where DNGLYSYQX <>'长期' and DNGLYSYQX!='') as h on g.NWNumber = h.GLYNumber or g.WWNumber=h.GLYNumber or g.USBNumber=h.GLYNumber or g.GQNumber=h.GLYNumber ) as tabA left join (select Number, SYR,CreateTime,NW,WW,USB,GQ,DNGLY from YGSYDNXXB) as tabB on tabA.NWNumber=tabB.Number or tabA.WWNumber = tabB.Number or tabA.USBNumber=tabB.Number or tabA.GQNumber=tabB.Number or tabA.GLYNumber=tabB.Number where nwd like '-%' or nwd='0' or wwd like '-%' or wwd='0' or usbd like'-%' or usbd='0' or gqd like'-%' or gqd ='0' or glyd like '-%' or glyd = '0'";
        object DQ = DbHelperSQL.GetSingle(strSQL);
        if (DQ.ToString() != "0")
        {
            divDQ.Visible = true;
        }
        else
        {
            divDQ.Visible = false;
        }
    }
    //员工使用电脑是否有到期的处理按钮
    protected void btnDQCL_Click(object sender, EventArgs e)
    {
        Response.Redirect("YGSYDNDQXX.aspx");
    }
}