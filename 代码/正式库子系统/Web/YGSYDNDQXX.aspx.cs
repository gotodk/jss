using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using System.Collections;
using System.Data;
using FMOP.Common;

public partial class Web_YGSYDNDQXX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpager1.OnNeedLoadData += new Web_pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if(!IsPostBack)
        {
            DisGrid();
        }
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select * from(select * from(select * from(select * from(select * from(select Number as NWNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(NWSYQX),charindex('~',reverse(NWSYQX))-1)),106)) as nwd from YGSYDNXXB where NWSYQX<>'长期' and NWSYQX!='') as a full outer join(select Number as WWNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(WWSYQX),charindex('~',reverse(WWSYQX))-1)),106)) as wwd from YGSYDNXXB where WWSYQX <>'长期' and WWSYQX!='') as b on a.NWNumber = b.WWNumber) as c full outer join(select Number as USBNumber,datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(USBSYQX),charindex('~',reverse(USBSYQX))-1)),106)) as usbd from YGSYDNXXB where USBSYQX <>'长期' and USBSYQX!='') as d on c.NWNumber=d.USBNumber or c.WWNumber=d.USBNumber) as e full outer join(select Number as GQNumber,datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(GQSYQX),charindex('~',reverse(GQSYQX))-1)),106)) as gqd from YGSYDNXXB where GQSYQX <>'长期' and GQSYQX!='') as f on e.NWNumber=f.GQNumber or e.WWNumber=f.GQNumber or e.USBNumber=f.GQNumber) as g full outer join(select Number as GLYNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(DNGLYSYQX),charindex('~',reverse(DNGLYSYQX))-1)),106)) as glyd from YGSYDNXXB where DNGLYSYQX <>'长期' and DNGLYSYQX!='') as h on g.NWNumber = h.GLYNumber or g.WWNumber=h.GLYNumber or g.USBNumber=h.GLYNumber or g.GQNumber=h.GLYNumber ) as tabA left join (select Number, SYR,CreateTime,NW,WW,USB,GQ,DNGLY from YGSYDNXXB) as tabB on tabA.NWNumber=tabB.Number or tabA.WWNumber = tabB.Number or tabA.USBNumber=tabB.Number or tabA.GQNumber=tabB.Number or tabA.GLYNumber=tabB.Number where nwd like '-%' or nwd='0' or wwd like '-%' or wwd='0' or usbd like'-%' or usbd='0' or gqd like'-%' or gqd ='0' or glyd like '-%' or glyd = '0') as ta ";
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
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                string nw = NewDS.Tables[0].Rows[i]["nwd"].ToString();
                string ww = NewDS.Tables[0].Rows[i]["wwd"].ToString();
                string usb = NewDS.Tables[0].Rows[i]["usbd"].ToString();
                string gq = NewDS.Tables[0].Rows[i]["gqd"].ToString();
                string gly = NewDS.Tables[0].Rows[i]["glyd"].ToString();
                switch (nw)
                {
                    case "":
                    case "NULL":
                        NewDS.Tables[0].Rows[i]["NW"] = "";
                        break;
                    default:
                        switch (nw.Substring(0,1))
                        {
                            case "-":
                            case "0":
                                NewDS.Tables[0].Rows[i]["NW"] = "√";
                                break;
                            default:
                                NewDS.Tables[0].Rows[i]["NW"] = "";
                                break;
                        }
                        break;
                }
                switch (ww)
                {
                    case "":
                    case "NULL":
                        NewDS.Tables[0].Rows[i]["WW"] = "";
                        break;
                    default:
                        switch (ww.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                NewDS.Tables[0].Rows[i]["WW"] = "√";
                                break;
                            default:
                                NewDS.Tables[0].Rows[i]["WW"] = "";
                                break;
                        }
                        break;
                }
                switch (usb)
                {
                    case "":
                    case "NULL":
                        NewDS.Tables[0].Rows[i]["USB"] = "";
                        break;
                    default:
                        switch (usb.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                NewDS.Tables[0].Rows[i]["USB"] = "√";
                                break;
                            default:
                                NewDS.Tables[0].Rows[i]["USB"] = "";
                                break;
                        }
                        break;
                }
                switch (gq)
                {
                    case "":
                    case "NULL":
                        NewDS.Tables[0].Rows[i]["GQ"] = "";
                        break;
                    default:
                        switch (gq.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                NewDS.Tables[0].Rows[i]["GQ"] = "√";
                                break;
                            default:
                                NewDS.Tables[0].Rows[i]["GQ"] = "";
                                break;
                        }
                        break;
                }
                switch (gly)
                {
                    case "":
                    case "NULL":
                        NewDS.Tables[0].Rows[i]["DNGLY"] = "";
                        break;
                    default:
                        switch (gly.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                NewDS.Tables[0].Rows[i]["DNGLY"] = "√";
                                break;
                            default:
                                NewDS.Tables[0].Rows[i]["DNGLY"] = "";
                                break;
                        }
                        break;
                }                
            }
            RadGrid1.DataSource = NewDS.Tables[0].DefaultView;            
        }
        else
        {
            RadGrid1.DataSource = DbHelperSQL.Query("select * from YGSYDNXXB where 1!=1");
        }
        RadGrid1.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        string number = txtXH.Text.Trim();
        string syr = txtSYR.Text.Trim();//使用人
        HTwhere["search_str_where"] += " and SYR like '%" + syr + "%' and Number like '%" + number + "%'";
        commonpager1.HTwhere = HTwhere;
        commonpager1.GetFYdataAndRaiseEvent();
    }
    //查询按钮
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    //更新层的取消按钮
    protected void btnQX_Click(object sender, EventArgs e)
    {
        drpZT.SelectedIndex = 0;
        drpNWTime.SelectedIndex = 0;
        divUpdate.Visible = false;
        divNW.Visible = false;
        txtNWStart.Text = "";
        txtNWEnd.Text = "";
        lblXH.InnerText = "";
        txtDQTime.Text = "";
    }
    //更新层的确定按钮
    protected void btnOK_Click(object sender, EventArgs e)
    {
        string QX = lblDQTime.InnerText.ToString();//内网、外网、USB、光驱、管理员
        string ZT = drpZT.SelectedValue.ToString();//状态的选择：开通或关闭
        string SYTime = drpNWTime.SelectedValue.ToString();//使用时间：长期或短期
        switch (QX)
        {
            case "内网":
                switch (ZT)
                {
                    case "开通":
                        switch (SYTime)
                        {
                            case "短期":
                                DQ("NW", "NWSYQX");
                                break;
                            case "长期":
                                CQ("NW", "NWSYQX");
                                break;
                            case "请选择":
                                MessageBox.Show(this, "请选择开通时间!");
                                break;
                            default:
                                break;
                        }
                        break;
                    case "关闭":
                        Close("NW", "NWSYQX");
                        break;
                    default:
                        break;
                }
                break;
            case "外网":
                switch (ZT)
                {
                    case "开通":
                        switch (SYTime)
                        {
                            case "短期":
                                DQ("WW", "WWSYQX");
                                break;
                            case "长期":
                                CQ("WW", "WWSYQX");
                                break;
                            case "请选择":
                                MessageBox.Show(this, "请选择开通时间!");
                                break;
                            default:
                                break;
                        }
                        break;
                    case "关闭":
                        Close("WW", "WWSYQX");
                        break;
                    default:
                        break;
                }
                break;
            case "USB":
                switch (ZT)
                {
                    case "开通":
                        switch (SYTime)
                        {
                            case "短期":
                                DQ("USB", "USBSYQX");
                                break;
                            case "长期":
                                CQ("USB", "USBSYQX");
                                break;
                            case "请选择":
                                MessageBox.Show(this, "请选择开通时间!");
                                break;
                            default:
                                break;
                        }
                        break;
                    case "关闭":
                        Close("USB", "USBSYQX");
                        break;
                    default:
                        break;
                }
                break;
            case "光驱":
                switch (ZT)
                {
                    case "开通":
                        switch (SYTime)
                        {
                            case "短期":
                                DQ("GQ", "GQSYQX");
                                break;
                            case "长期":
                                CQ("GQ", "GQSYQX");
                                break;
                            case "请选择":
                                MessageBox.Show(this, "请选择开通时间!");
                                break;
                            default:
                                break;
                        }
                        break;
                    case "关闭":
                        Close("GQ", "GQSYQX");
                        break;
                    default:
                        break;
                }
                break;
            case "管理员":
                switch (ZT)
                {
                    case "开通":
                        switch (SYTime)
                        {
                            case "短期":
                                DQ("DNGLY", "DNGLYSYQX");
                                break;
                            case "长期":
                                CQ("DNGLY", "DNGLYSYQX");
                                break;
                            case "请选择":
                                MessageBox.Show(this, "请选择开通时间!");
                                break;
                            default:
                                break;
                        }
                        break;
                    case "关闭":
                        Close("DNGLY", "DNGLYSYQX");
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

    }
    //更新层的时间选框
    protected void drpNWTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpNWTime.SelectedValue.ToString() == "短期")
        {
            divNW.Visible = true;
        }
        else
        {
            divNW.Visible = false;
        }
    }
    //数据绑定点击内网、外网、USB、光驱、管理员的事件
    protected void RadGrid1_ItemCommand(object source, Telerik.WebControls.GridCommandEventArgs e)
    {
        #region//单击开通权限，弹出修改使用期限的窗口
        lblXH.InnerText = e.CommandName.ToString();
        if (e.CommandArgument == "nw")
        {
            divUpdate.Visible = true;
            lblDQTime.InnerText = "内网";
            string strSQL = "select NWSYQX from YGSYDNXXB where Number='"+e.CommandName+"'";
            object nw = DbHelperSQL.GetSingle(strSQL);
            if (nw.ToString() == "")
            {
                txtDQTime.Text = "未开通";
            }
            else
            {
                txtDQTime.Text = nw.ToString();
            }            
        }
        else if (e.CommandArgument == "ww")
        {
            divUpdate.Visible = true;
            lblDQTime.InnerText = "外网";
            string strSQL = "select WWSYQX from YGSYDNXXB where Number='" + e.CommandName + "'";
            object ww = DbHelperSQL.GetSingle(strSQL);
            if (ww.ToString() == "")
            {
                txtDQTime.Text = "未开通";
            }
            else
            {
                txtDQTime.Text = ww.ToString();
            }    
        }
        else if (e.CommandArgument == "usb")
        {
            divUpdate.Visible = true;
            lblDQTime.InnerText = "USB";
            string strSQL = "select USBSYQX from YGSYDNXXB where Number='" + e.CommandName + "'";
            object usb = DbHelperSQL.GetSingle(strSQL);
            if (usb.ToString() == "")
            {
                txtDQTime.Text = "未开通";
            }
            else
            {
                txtDQTime.Text = usb.ToString();
            }    
        }
        else if (e.CommandArgument == "gq")
        {
            divUpdate.Visible = true;
            lblDQTime.InnerText = "光驱";
            string strSQL = "select GQSYQX from YGSYDNXXB where Number='" + e.CommandName + "'";
            object gq = DbHelperSQL.GetSingle(strSQL);
            if (gq.ToString() == "")
            {
                txtDQTime.Text = "未开通";
            }
            else
            {
                txtDQTime.Text = gq.ToString();
            }  
        }
        else if (e.CommandArgument == "gly")
        {
            divUpdate.Visible = true;
            lblDQTime.InnerText = "管理员";
            string strSQL = "select DNGLYSYQX from YGSYDNXXB where Number='" + e.CommandName + "'";
            object gly = DbHelperSQL.GetSingle(strSQL);
            if (gly.ToString() == "")
            {
                txtDQTime.Text = "未开通";
            }
            else
            {
                txtDQTime.Text = gly.ToString();
            }
        }
        #endregion
        #region//数据列表的关闭
        else if (e.CommandArgument == "gb")
        {
            string strSQL = "select * from(select * from(select * from(select * from(select * from(select Number as NWNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(NWSYQX),charindex('~',reverse(NWSYQX))-1)),106)) as nwd from YGSYDNXXB where NWSYQX<>'长期' and NWSYQX!='') as a full outer join(select Number as WWNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(WWSYQX),charindex('~',reverse(WWSYQX))-1)),106)) as wwd from YGSYDNXXB where WWSYQX <>'长期' and WWSYQX!='') as b on a.NWNumber = b.WWNumber) as c full outer join(select Number as USBNumber,datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(USBSYQX),charindex('~',reverse(USBSYQX))-1)),106)) as usbd from YGSYDNXXB where USBSYQX <>'长期' and USBSYQX!='') as d on c.NWNumber=d.USBNumber or c.WWNumber=d.USBNumber) as e full outer join(select Number as GQNumber,datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(GQSYQX),charindex('~',reverse(GQSYQX))-1)),106)) as gqd from YGSYDNXXB where GQSYQX <>'长期' and GQSYQX!='') as f on e.NWNumber=f.GQNumber or e.WWNumber=f.GQNumber or e.USBNumber=f.GQNumber) as g full outer join(select Number as GLYNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(DNGLYSYQX),charindex('~',reverse(DNGLYSYQX))-1)),106)) as glyd from YGSYDNXXB where DNGLYSYQX <>'长期' and DNGLYSYQX!='') as h on g.NWNumber = h.GLYNumber or g.WWNumber=h.GLYNumber or g.USBNumber=h.GLYNumber or g.GQNumber=h.GLYNumber ) as tabA left join (select Number, SYR,CreateTime,NW,WW,USB,GQ,DNGLY from YGSYDNXXB) as tabB on tabA.NWNumber=tabB.Number or tabA.WWNumber = tabB.Number or tabA.USBNumber=tabB.Number or tabA.GQNumber=tabB.Number or tabA.GLYNumber=tabB.Number where (nwd like '-%' or nwd='0' or wwd like '-%' or wwd='0' or usbd like'-%' or usbd='0' or gqd like'-%' or gqd ='0' or glyd like '-%' or glyd = '0') and Number='"+e.CommandName+"' ";
            DataSet ds = DbHelperSQL.Query(strSQL);
            if(ds.Tables[0].Rows.Count>0)
            {
                string strSQLUpadate = "UPDATE [dbo].[YGSYDNXXB] SET ";
                string nw = ds.Tables[0].Rows[0]["nwd"].ToString();
                string ww = ds.Tables[0].Rows[0]["wwd"].ToString();
                string usb = ds.Tables[0].Rows[0]["usbd"].ToString();
                string gq = ds.Tables[0].Rows[0]["gqd"].ToString();
                string gly = ds.Tables[0].Rows[0]["glyd"].ToString();
                #region//设置更新的字段
                switch (nw)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch(nw.Substring(0,1))
                        {
                            case "-":
                            case"0":
                                strSQLUpadate += "[NW]='否',[NWSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                switch (ww)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch (ww.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                strSQLUpadate += "[WW]='否',[WWSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                switch (usb)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch (usb.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                strSQLUpadate += "[USB]='否',[USBSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                switch (gq)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch (gq.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                strSQLUpadate += "[GQ]='否',[GQSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                switch (gly)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch (gly.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                strSQLUpadate += "[DNGLY]='否',[DNGLYSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                #endregion
                strSQLUpadate += "[ZHYCGXR] = '" + User.Identity.Name.ToString() + "',[ZHYCGXSJ] = GETDATE() where Number ='"+e.CommandName+"'";
                int i = DbHelperSQL.ExecuteSql(strSQLUpadate);
                if (i > 0)
                {
                    MessageBox.Show(this, "更新成功！");
                    DisGrid();
                }
                else
                {
                    MessageBox.Show(this,"更新失败！");
                }
            }

        }
        #endregion
        #region//数据列表的删除
        else if(e.CommandArgument=="del")
        {
            string strSQL = "DELETE FROM [dbo].[YGSYDNXXB] WHERE Number='"+e.CommandName+"'";
            int i = DbHelperSQL.ExecuteSql(strSQL);
            if (i > 0)
            {
                MessageBox.Show(this, "删除成功!");
                DisGrid();//重新绑定数据
            }
            else
            {
                MessageBox.Show(this,"删除失败！");
            }
        }
        #endregion

    }
    //短期
    protected void DQ(string QXZD,string TimeZD)
    {
        
        string Number = lblXH.InnerText.ToString();    
        string strSQL = "";
        if (txtNWStart.Text.ToString() == "" || txtNWEnd.Text.ToString() == "")
        {
            MessageBox.Show(this, "请将使用时间段填写完整!");
        }
        else if (Convert.ToDateTime(txtNWStart.Text.ToString()) > Convert.ToDateTime(txtNWEnd.Text.ToString()))
        {
            MessageBox.Show(this, "起始时间不能大于结束时间!");
            txtNWStart.Text = "";
            txtNWEnd.Text = "";
        }
        else
        {
            strSQL = "UPDATE [dbo].[YGSYDNXXB] SET " + QXZD + " = '是' ," + TimeZD + " = '" +
                txtNWStart.Text.ToString() + "~" + txtNWEnd.Text.ToString() + "' ,[ZHYCGXR] = '" +
                User.Identity.Name + "' ,[ZHYCGXSJ] = GETDATE() WHERE [Number] = '" + Number + "'";
            int i = DbHelperSQL.ExecuteSql(strSQL);
            if (i > 0)
            {
                MessageBox.Show(this, "更新成功!");
                divUpdate.Visible = false;
                DisGrid();
            }
            else
            {
                MessageBox.Show(this, "更新失败!");
            }
        }
    }
    //长期
    protected void CQ(string QXZD, string TimeZD)
    {
        string Number = lblXH.InnerText.ToString();
        string strSQL = "";
        strSQL = "UPDATE [dbo].[YGSYDNXXB] SET " + QXZD + " = '是' ," + TimeZD + " = '长期' ,[ZHYCGXR] = '" + User.Identity.Name
                                    + "' ,[ZHYCGXSJ] = GETDATE() WHERE [Number] = '" + Number + "'";
        int j = DbHelperSQL.ExecuteSql(strSQL);
        if (j > 0)
        {
            MessageBox.Show(this, "更新成功!");
            divUpdate.Visible = false;
            DisGrid();
        }
        else
        {
            MessageBox.Show(this, "更新失败!");
        }
    }
    //关闭单个人的到期的权限
    protected void Close(string QXZD, string TimeZD)
    {
        string Number = lblXH.InnerText.ToString();
        string strSQL = "";
        strSQL = "UPDATE [dbo].[YGSYDNXXB] SET " + QXZD + " = '否' ," + TimeZD + " = '' ,[ZHYCGXR] = '" + User.Identity.Name
                                + "' ,[ZHYCGXSJ] = GETDATE() WHERE [Number] = '" + Number + "'";
        int m = DbHelperSQL.ExecuteSql(strSQL);
        if (m > 0)
        {
            MessageBox.Show(this, "更新成功!");
            divUpdate.Visible = false;
            DisGrid();
        }
        else
        {
            MessageBox.Show(this, "更新失败!");
        }
    }
    //全部关闭到期的权限
    protected void btnCloseALL_Click(object sender, EventArgs e)
    {
        string number = txtXH.Text.Trim();
        string syr = txtSYR.Text.Trim();//使用人        
        string strSQL = "select * from(select * from(select * from(select * from(select * from(select Number as NWNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(NWSYQX),charindex('~',reverse(NWSYQX))-1)),106)) as nwd from YGSYDNXXB where NWSYQX<>'长期' and NWSYQX!='') as a full outer join(select Number as WWNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(WWSYQX),charindex('~',reverse(WWSYQX))-1)),106)) as wwd from YGSYDNXXB where WWSYQX <>'长期' and WWSYQX!='') as b on a.NWNumber = b.WWNumber) as c full outer join(select Number as USBNumber,datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(USBSYQX),charindex('~',reverse(USBSYQX))-1)),106)) as usbd from YGSYDNXXB where USBSYQX <>'长期' and USBSYQX!='') as d on c.NWNumber=d.USBNumber or c.WWNumber=d.USBNumber) as e full outer join(select Number as GQNumber,datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(GQSYQX),charindex('~',reverse(GQSYQX))-1)),106)) as gqd from YGSYDNXXB where GQSYQX <>'长期' and GQSYQX!='') as f on e.NWNumber=f.GQNumber or e.WWNumber=f.GQNumber or e.USBNumber=f.GQNumber) as g full outer join(select Number as GLYNumber, datediff(D,GETDATE(),convert(varchar(100),reverse(left(reverse(DNGLYSYQX),charindex('~',reverse(DNGLYSYQX))-1)),106)) as glyd from YGSYDNXXB where DNGLYSYQX <>'长期' and DNGLYSYQX!='') as h on g.NWNumber = h.GLYNumber or g.WWNumber=h.GLYNumber or g.USBNumber=h.GLYNumber or g.GQNumber=h.GLYNumber ) as tabA left join (select Number, SYR,CreateTime,NW,WW,USB,GQ,DNGLY from YGSYDNXXB) as tabB on tabA.NWNumber=tabB.Number or tabA.WWNumber = tabB.Number or tabA.USBNumber=tabB.Number or tabA.GQNumber=tabB.Number or tabA.GLYNumber=tabB.Number where (nwd like '-%' or nwd='0' or wwd like '-%' or wwd='0' or usbd like'-%' or usbd='0' or gqd like'-%' or gqd ='0' or glyd like '-%' or glyd = '0') and SYR like '%" + syr + "%' and Number like '%" + number + "%' ";
        DataSet ds = DbHelperSQL.Query(strSQL);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ArrayList list = new ArrayList();
            for (int j = 0; j < ds.Tables[0].Rows.Count;j++ )
            {
                string strSQLUpadate = "UPDATE [dbo].[YGSYDNXXB] SET ";
                string nw = ds.Tables[0].Rows[j]["nwd"].ToString();
                string ww = ds.Tables[0].Rows[j]["wwd"].ToString();
                string usb = ds.Tables[0].Rows[j]["usbd"].ToString();
                string gq = ds.Tables[0].Rows[j]["gqd"].ToString();
                string gly = ds.Tables[0].Rows[j]["glyd"].ToString();
                #region//设置更新的字段
                switch (nw)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch (nw.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                strSQLUpadate += "[NW]='否',[NWSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                switch (ww)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch (ww.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                strSQLUpadate += "[WW]='否',[WWSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                switch (usb)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch (usb.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                strSQLUpadate += "[USB]='否',[USBSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                switch (gq)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch (gq.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                strSQLUpadate += "[GQ]='否',[GQSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                switch (gly)
                {
                    case "":
                    case "NULL":
                        break;
                    default:
                        switch (gly.Substring(0, 1))
                        {
                            case "-":
                            case "0":
                                strSQLUpadate += "[DNGLY]='否',[DNGLYSYQX] = '',";
                                break;
                            default:
                                break;
                        }
                        break;
                }
                #endregion
                strSQLUpadate += "[ZHYCGXR] = '" + User.Identity.Name.ToString() + "',[ZHYCGXSJ] = GETDATE() where Number='" + 
                    ds.Tables[0].Rows[j]["Number"].ToString() +"'";
                list.Add(strSQLUpadate);
            }

            bool l = DbHelperSQL.ExecSqlTran(list);
            if (l)
            {
                MessageBox.Show(this, "更新成功！");
                DisGrid();
            }
            else
            {
                MessageBox.Show(this, "更新失败！");
            }
        }
    }
    //返回信息查看页面
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("YGSYDNXXCK.aspx");
    }
}
