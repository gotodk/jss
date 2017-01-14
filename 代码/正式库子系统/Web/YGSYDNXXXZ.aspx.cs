using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using FMOP.Common;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_YGSYDNXXXZ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindBM();//绑定总部所有部门
            if (Request.QueryString["Number"] != null && Request.QueryString["Number"] != "")
            {
                divXZ.Visible = false;
                divXGBTN.Visible = true;
                divTJ.Visible = false;
                ViewState["Number"] = Request.QueryString["Number"].ToString();
                BindXX();//绑定要修改的信息
            }
            else
            {
                divXZ.Visible = true;
                divXGBTN.Visible = false;
                divTJ.Visible = true;
                ViewState["Number"] = "";
            }
        }        
    }
    //绑定总部所有部门
    protected void BindBM()
    {
        string strSQL = "select distinct DeptName from dbo.HR_Dept where Superior='公司总部'";//查询总部所有部门
        DataSet dsBM = DbHelperSQL.Query(strSQL);
        drpSSBM.DataSource = null;
        drpSSBM.Items.Clear();
        drpSSBM.DataSource = dsBM;
        drpSSBM.DataTextField = "DeptName";
        drpSSBM.DataValueField = "DeptName";        
        drpSSBM.DataBind();
        drpSSBM.Items.Add(new ListItem("库存区", "库存区"));
        drpSSBM.Items.Add(new ListItem("报废区", "报废区"));
	drpSSBM.Items.Add(new ListItem("财务中心(百仕加)", "财务中心(百仕加)"));
    }
    //绑定要修改的信息
    protected void BindXX()
    {
        string strSQL = "select * from YGSYDNXXB where Number='" + ViewState["Number"].ToString() + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        if(ds.Tables[0].Rows.Count>0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
            {
                drpSSBM.SelectedValue = ds.Tables[0].Rows[i]["SSBM"].ToString();                
                txtSYR.Text = ds.Tables[0].Rows[i]["SYR"].ToString();
                string[] ip = ds.Tables[0].Rows[i]["IPDZ"].ToString().Split('.');
                txtIP1.Value = ip[0].ToString();
                txtIP2.Value = ip[1].ToString();
                txtIP3.Value = ip[2].ToString();
                txtIP4.Value = ip[3].ToString();
                string[] mac = ds.Tables[0].Rows[i]["MACDZ"].ToString().Split('-');
                txtMAC1.Value = mac[0].ToString();
                txtMAC2.Value = mac[1].ToString();
                txtMAC3.Value = mac[2].ToString();
                txtPZ.Text = ds.Tables[0].Rows[i]["PZXX"].ToString();
                //内网
                string nw = ds.Tables[0].Rows[i]["NW"].ToString();
                if (nw == "是")
                {
                    rdnws.Checked = true;
                    rdnwf.Checked = false;
                    if (ds.Tables[0].Rows[i]["NWSYQX"].ToString() != "长期")
                    {
                        drpNW.SelectedValue = "短期";
                        divNW.Visible = true;
                        string[] nwtime = ds.Tables[0].Rows[i]["NWSYQX"].ToString().Split('~');
                        txtNWStart.Text = nwtime[0].ToString();
                        txtNWEnd.Text = nwtime[1].ToString();
                    }
                    else if (ds.Tables[0].Rows[i]["NWSYQX"].ToString() == "长期")
                    {
                        drpNW.SelectedValue = "长期";
                    }
                }
                else
                {
                    rdnws.Checked = false;
                    rdnwf.Checked = true;
                }
                //外网
                string ww = ds.Tables[0].Rows[i]["WW"].ToString();
                if (ww == "是")
                {
                    rdwws.Checked = true;
                    rdwwf.Checked = false;
                    if (ds.Tables[0].Rows[i]["WWSYQX"].ToString() != "长期")
                    {
                        drpWW.SelectedValue = "短期";
                        divWW.Visible = true;
                        string[] wwtime = ds.Tables[0].Rows[i]["WWSYQX"].ToString().Split('~');
                        txtWWStart.Text = wwtime[0].ToString();
                        txtWWEnd.Text = wwtime[1].ToString();
                    }
                    else if (ds.Tables[0].Rows[i]["WWSYQX"].ToString() == "长期")
                    {
                        drpWW.SelectedValue = "长期";
                    }
                }
                else
                {
                    rdwws.Checked = false;
                    rdwwf.Checked = true;
                }
                //USB
                string usb = ds.Tables[0].Rows[i]["USB"].ToString();
                if (usb == "是")
                {
                    rdusbf.Checked = false;
                    rdusbs.Checked = true;
                    if (ds.Tables[0].Rows[i]["USBSYQX"].ToString() != "长期")
                    {
                        drpUSB.SelectedValue = "短期";
                        divUSB.Visible = true;
                        string[] usbtime = ds.Tables[0].Rows[i]["USBSYQX"].ToString().Split('~');
                        txtUSBStart.Text = usbtime[0].ToString();
                        txtUSBEnd.Text = usbtime[1].ToString();
                    }
                    else if (ds.Tables[0].Rows[i]["USBSYQX"].ToString() == "长期")
                    {
                        drpUSB.SelectedValue = "长期";
                    }
                }
                else
                {
                    rdusbs.Checked = false;
                    rdusbf.Checked = true;
                }
                //光驱
                string gq = ds.Tables[0].Rows[i]["GQ"].ToString();
                switch (gq)
                {
                    case "是":
                        rdgqf.Checked = false;
                        rdgqs.Checked = true;
                        switch (ds.Tables[0].Rows[i]["GQSYQX"].ToString())
                        {
                            case "长期":
                                drpGQ.SelectedValue = "长期";
                                break;
                            default:
                                drpGQ.SelectedValue = "短期";
                                divGQ.Visible = true;
                                string[] gqtime = ds.Tables[0].Rows[i]["GQSYQX"].ToString().Split('~');
                                txtGQStart.Text = gqtime[0].ToString();
                                txtGQEnd.Text = gqtime[1].ToString();
                                break;
                        }
                        break;
                    default:
                        rdgqf.Checked = true;
                        rdgqs.Checked = false;
                        break;
                
                }
                //管理员
                string gly = ds.Tables[0].Rows[i]["DNGLY"].ToString();
                switch (gly)
                {
                    case "是":
                        rdglyf.Checked = false;
                        rdglys.Checked = true;
                        switch (ds.Tables[0].Rows[i]["DNGLYSYQX"].ToString())
                        {
                            case "长期":
                                drpGLY.SelectedValue = "长期";
                                break;
                            default:
                                drpGLY.SelectedValue = "短期";
                                divGLY.Visible = true;
                                string[] glytime = ds.Tables[0].Rows[i]["DNGLYSYQX"].ToString().Split('~');
                                txtGLYStart.Text = glytime[0].ToString();
                                txtGLYEnd.Text = glytime[1].ToString();
                                break;
                        }
                        break;
                    default:
                        rdglyf.Checked = true;
                        rdglys.Checked = false;
                        break;

                }

                txtBZ.Text = ds.Tables[0].Rows[i]["BZ"].ToString();
            }
        }

    }
    //内网使用期限
    protected void drpNW_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpNW.SelectedValue == "短期")
        {
            divNW.Visible = true;
        }
        else
        {
            divNW.Visible = false;
        }
    }
    //外网使用期限
    protected void drpWW_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpWW.SelectedValue == "短期")
        {
            divWW.Visible = true;
        }
        else
        {
            divWW.Visible = false;
        }
    }
    //USB使用期限
    protected void drpUSB_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpUSB.SelectedValue == "短期")
        {
            divUSB.Visible = true;
        }
        else
        {
            divUSB.Visible = false;
        }
    }
    //光驱使用期限
    protected void drpGQ_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpGQ.SelectedValue == "短期")
        {
            divGQ.Visible = true;
        }
        else
        {
            divGQ.Visible = false;
        }
    }
    //电脑管理员使用期限
    protected void drpGLY_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpGLY.SelectedValue == "短期")
        {
            divGLY.Visible = true;
        }
        else
        {
            divGLY.Visible = false;
        }
    }
    //新增按钮
    protected void btnTJ_Click(object sender, EventArgs e)
    {
        string time = "";
        //生成YGSYDNXXB员工使用电脑信息表的number
        WorkFlowModule WFPKNumber = new WorkFlowModule("YGSYDNXXB");
        string YGXXBNumber = WFPKNumber.numberFormat.GetNextNumber();

        string bm = drpSSBM.SelectedValue.ToString();//所属部门
        string syr = txtSYR.Text.Trim();//使用人
        string ip = txtIP1.Value.Trim() + "." + txtIP2.Value.Trim() + "." + txtIP3.Value.Trim() + "." + txtIP4.Value.Trim();//IP地址
        string mac = txtMAC1.Value.Trim() + "-" + txtMAC2.Value.Trim() + "-" + txtMAC3.Value.Trim();//MAC地址
        string pz=txtPZ.Text.Trim();//配置
        string bz = txtBZ.Text.Trim();//备注
        string nw = "";//内网
        string nwtime = "";//内网使用期限
        if (rdnwf.Checked == true)
        {
            nw = rdnwf.Value.ToString();
        }
        else
        {
            nw = rdnws.Value.ToString();
            if(drpNW.SelectedValue.ToString()=="短期")//如果使用期限是短期，则必须填写使用时间
            {
                if (txtNWStart.Text.Trim() == "" || txtNWEnd.Text.Trim() == "")
                {
                    time += "请将内网使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtNWStart.Text.Trim()) > Convert.ToDateTime(txtNWEnd.Text.Trim()))
                    {
                        time += "内网的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        nwtime = txtNWStart.Text.Trim() + "~" + txtNWEnd.Text.Trim();
                    }                    
                }
            }
            else if (drpNW.SelectedValue.ToString() == "长期")
            {
                nwtime = drpNW.SelectedValue.ToString();
            }
            else
            {
                time += "请选择内网使用期限！";
            }
        }
        string ww = "";//外网
        string wwtime="";//外网使用期限
        if (rdwwf.Checked == true)
        {
            ww = rdwwf.Value.ToString();
        }
        else
        {
            ww = rdwws.Value.ToString();
            if (drpWW.SelectedValue.ToString() == "短期")//如果使用期限是短期，则必须填写使用时间
            {
                if (txtWWStart.Text.Trim() == "" || txtWWEnd.Text.Trim() == "")
                {
                    time += "请将外网使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtWWStart.Text.Trim()) > Convert.ToDateTime(txtWWEnd.Text.Trim()))
                    {
                        time += "外网的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        wwtime = txtWWStart.Text.Trim() + "~" + txtWWEnd.Text.Trim();
                    }                     
                }
            }
            else if (drpWW.SelectedValue.ToString() == "长期")
            {
                wwtime = drpWW.SelectedValue.ToString();
            }
            else
            {
                time += "请选择外网使用期限！";
            }
        }
        string usb = "";//USB
        string usbtime = "";//USB使用期限
        if (rdusbf.Checked == true)
        {
            usb = rdusbf.Value.ToString();
        }
        else
        {
            usb = rdusbs.Value.ToString();
            if (drpUSB.SelectedValue == "短期")
            {
                if (txtUSBStart.Text.Trim() == "" || txtUSBEnd.Text.Trim() == "")
                {
                    time += "请将USB使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtUSBStart.Text.Trim()) > Convert.ToDateTime(txtUSBEnd.Text.Trim()))
                    {
                        time += "USB的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        usbtime = txtUSBStart.Text.Trim() + "~" + txtUSBEnd.Text.Trim();
                    }                     
                }
            }
            else if (drpUSB.SelectedValue == "长期")
            {
                usbtime = drpUSB.SelectedValue;
            }
            else
            {
                time += "请选择USB使用期限！";
            }
        }
        string gq = "";//光驱
        string gqtime = "";//光驱使用期限
        if(rdgqf.Checked==true)
        {
            gq = rdgqf.Value.ToString();
        }
        else
        {
            gq = rdgqs.Value.ToString();
            if (drpGQ.SelectedValue.ToString() == "短期")
            {
                if (txtGQStart.Text.Trim() == "" || txtGQEnd.Text.Trim() == "")
                {
                    time += "请将光驱使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtGQStart.Text.Trim()) > Convert.ToDateTime(txtGQStart.Text.Trim()))
                    {
                        time += "光驱的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        gqtime = txtGQStart.Text.Trim() + "~" + txtGQEnd.Text.Trim();
                    } 
                    
                }
            }
            else if (drpGQ.SelectedValue.ToString() == "长期")
            {
                gqtime = drpGQ.SelectedValue.ToString();
            }
            else
            {
                time += "请选择光驱使用期限！";
            }
        }
        string gly = "";//管理员
        string glytime = "";//管理员使用期限
        if(rdglyf.Checked==true)
        {
            gly = rdglyf.Value.ToString();
        }
        else
        {
            gly = rdglys.Value.ToString();
            if (drpGLY.SelectedValue.ToString() == "短期")
            {
                if (txtGLYStart.Text.Trim() == "" || txtGLYEnd.Text.Trim() == "")
                {
                    time += "请将管理员使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtGLYStart.Text.Trim()) > Convert.ToDateTime(txtGLYEnd.Text.Trim()))
                    {
                        time += "管理员的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        glytime = txtGLYStart.Text.Trim() + "~" + txtGLYEnd.Text.Trim();
                    }                     
                }
            }
            else if (drpGLY.SelectedValue.ToString() == "长期")
            {
                glytime = drpGLY.SelectedValue.ToString();
            }
            else
            {
                time += "请选择管理员使用期限！";
            }
        }

        //如果有一个使用期限为短期且时间没有填写，则跳出此方法
        if (time.Trim() != "")
        {
            MessageBox.Show(this,time);
            return;
        }
        else
        {
            string strSQLInsert = "INSERT INTO [dbo].[YGSYDNXXB]([Number],[SSBM] ,[SYR] ,[IPDZ],[MACDZ],[PZXX],[NW],[NWSYQX],[WW],[WWSYQX]"+
                                  ",[USB],[USBSYQX],[GQ],[GQSYQX],[DNGLY],[DNGLYSYQX],[BZ],[CheckState]"+
                                  ",[CreateUser],[CreateTime] ,[CheckLimitTime])VALUES('" + YGXXBNumber + "','"+drpSSBM.SelectedValue.Trim()+"'"+
                                  " ,'"+syr.ToString()+"','"+ip.ToString()+"','"+mac.ToString()+"' ,'"+pz.ToString()+"','"+nw.ToString()+"'"+
                                  ",'"+nwtime.ToString()+"','"+ww.ToString()+"','"+wwtime.ToString()+"','"+usb.ToString()+"','"+usbtime.ToString()+"'"+
                                  ",'"+gq.ToString()+"','"+gqtime.ToString()+"','"+gly.ToString()+"','"+glytime.ToString()+"','"+bz.ToString()+"'"+
                                  ",1,'" + User.Identity.Name.ToString() + "',getdate(),getdate())";
            int i = DbHelperSQL.ExecuteSql(strSQLInsert);
            if (i > 0)
            {
                MessageBox.Show(this, "信息新增成功！");                
                BindBM();//绑定总部所有部门
                txtSYR.Text = "";
                txtIP1.Value = "";
                txtIP2.Value = "";
                txtIP3.Value = "";
                txtIP4.Value = "";
                txtMAC1.Value = "";
                txtMAC2.Value = "";
                txtMAC3.Value = "";
                txtPZ.Text = "";
                rdnwf.Checked = true;
                drpNW.SelectedValue = "请选择";
                rdwwf.Checked = true;
                drpWW.SelectedValue = "请选择";
                rdusbf.Checked = true;
                drpUSB.SelectedValue = "请选择";
                rdgqf.Checked = true;
                drpGQ.SelectedValue = "请选择";
                rdglyf.Checked = true;
                drpGLY.SelectedValue = "请选择";
                txtBZ.Text = "";
                divNW.Visible = false;
                txtNWEnd.Text = "";
                txtNWStart.Text = "";
                divWW.Visible = false;
                txtWWEnd.Text = "";
                txtWWStart.Text = "";
                divUSB.Visible = false;
                txtUSBEnd.Text = "";
                txtUSBStart.Text = "";
                divGQ.Visible = false;
                txtGQEnd.Text = "";
                txtGQStart.Text = "";
                divGLY.Visible = false;
                txtGLYEnd.Text = "";
                txtGLYStart.Text = "";
            }
            else 
            {
                MessageBox.Show(this, "信息新增失败！");
            }
        }

    }
    //修改保存按钮
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string time = "";//用于记录没有填完整的字段
        string bm = drpSSBM.SelectedValue.ToString();//所属部门
        string syr = txtSYR.Text.Trim();//使用人
        string ip = txtIP1.Value.Trim() + "." + txtIP2.Value.Trim() + "." + txtIP3.Value.Trim() + "." + txtIP4.Value.Trim();//IP地址
        string mac = txtMAC1.Value.Trim() + "-" + txtMAC2.Value.Trim() + "-" + txtMAC3.Value.Trim();//MAC地址
        string pz = txtPZ.Text.Trim();//配置
        string bz = txtBZ.Text.Trim();//备注
        string nw = "";//内网
        string nwtime = "";//内网使用期限
        if (rdnwf.Checked == true)
        {
            nw = rdnwf.Value.ToString();
        }
        else
        {
            nw = rdnws.Value.ToString();
            if (drpNW.SelectedValue.ToString() == "短期")//如果使用期限是短期，则必须填写使用时间
            {
                if (txtNWStart.Text.Trim() == "" || txtNWEnd.Text.Trim() == "")
                {
                    time += "请将内网使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtNWStart.Text.Trim()) > Convert.ToDateTime(txtNWEnd.Text.Trim()))
                    {
                        time += "内网的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        nwtime = txtNWStart.Text.Trim() + "~" + txtNWEnd.Text.Trim();
                    } 
                }
            }
            else if (drpNW.SelectedValue.ToString() == "长期")
            {
                nwtime = drpNW.SelectedValue.ToString();
            }
            else
            {
                time += "请选择内网使用期限！";
            }
        }
        string ww = "";//外网
        string wwtime = "";//外网使用期限
        if (rdwwf.Checked == true)
        {
            ww = rdwwf.Value.ToString();
        }
        else
        {
            ww = rdwws.Value.ToString();
            if (drpWW.SelectedValue.ToString() == "短期")//如果使用期限是短期，则必须填写使用时间
            {
                if (txtWWStart.Text.Trim() == "" || txtWWEnd.Text.Trim() == "")
                {
                    time += "请将外网使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtWWStart.Text.Trim()) > Convert.ToDateTime(txtWWEnd.Text.Trim()))
                    {
                        time += "外网的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        wwtime = txtWWStart.Text.Trim() + "~" + txtWWEnd.Text.Trim();
                    }  
                }
            }
            else if (drpWW.SelectedValue.ToString() == "长期")
            {
                wwtime = drpWW.SelectedValue.ToString();
            }
            else
            {
                time += "请选择外网使用期限！";
            }
        }
        string usb = "";//USB
        string usbtime = "";//USB使用期限
        if (rdusbf.Checked == true)
        {
            usb = rdusbf.Value.ToString();
        }
        else
        {
            usb = rdusbs.Value.ToString();
            if (drpUSB.SelectedValue == "短期")
            {
                if (txtUSBStart.Text.Trim() == "" || txtUSBEnd.Text.Trim() == "")
                {
                    time += "请将USB使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtUSBStart.Text.Trim()) > Convert.ToDateTime(txtUSBEnd.Text.Trim()))
                    {
                        time += "USB的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        usbtime = txtUSBStart.Text.Trim() + "~" + txtUSBEnd.Text.Trim();
                    } 
                }
            }
            else if (drpUSB.SelectedValue == "长期")
            {
                usbtime = drpUSB.SelectedValue;
            }
            else
            {
                time += "请选择USB使用期限！";
            }
        }
        string gq = "";//光驱
        string gqtime = "";//光驱使用期限
        if (rdgqf.Checked == true)
        {
            gq = rdgqf.Value.ToString();
        }
        else
        {
            gq = rdgqs.Value.ToString();
            if (drpGQ.SelectedValue.ToString() == "短期")
            {
                if (txtGQStart.Text.Trim() == "" || txtGQEnd.Text.Trim() == "")
                {
                    time += "请将光驱使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtGQStart.Text.Trim()) > Convert.ToDateTime(txtGQStart.Text.Trim()))
                    {
                        time += "光驱的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        gqtime = txtGQStart.Text.Trim() + "~" + txtGQEnd.Text.Trim();
                    } 
                }
            }
            else if (drpGQ.SelectedValue.ToString() == "长期")
            {
                gqtime = drpGQ.SelectedValue.ToString();
            }
            else
            {
                time += "请选择光驱使用期限！";
            }
        }
        string gly = "";//管理员
        string glytime = "";//管理员使用期限
        if (rdglyf.Checked == true)
        {
            gly = rdglyf.Value.ToString();
        }
        else
        {
            gly = rdglys.Value.ToString();
            if (drpGLY.SelectedValue.ToString() == "短期")
            {
                if (txtGLYStart.Text.Trim() == "" || txtGLYEnd.Text.Trim() == "")
                {
                    time += "请将管理员使用期限填写完整！";
                }
                else
                {
                    if (Convert.ToDateTime(txtGLYStart.Text.Trim()) > Convert.ToDateTime(txtGLYEnd.Text.Trim()))
                    {
                        time += "管理员的起始时间不能大于终止时间！";
                    }
                    else
                    {
                        glytime = txtGLYStart.Text.Trim() + "~" + txtGLYEnd.Text.Trim();
                    }  
                }
            }
            else if (drpGLY.SelectedValue.ToString() == "长期")
            {
                glytime = drpGLY.SelectedValue.ToString();
            }
            else
            {
                time += "请选择管理员使用期限！";
            }
        }

        //如果有一个使用期限为短期且时间没有填写，则跳出此方法
        if (time.Trim() != "")
        {
            MessageBox.Show(this, time);
            return;
        }
        else
        {
            string strSQLInsert = "UPDATE [dbo].[YGSYDNXXB] SET [SSBM] = '"+drpSSBM.SelectedValue.Trim()+"',[SYR] = '"+syr+"',[IPDZ] = '"+ip
                +"',[MACDZ] = '"+mac+"',[PZXX] = '"+pz+"',[NW] = '"+nw+"',[NWSYQX] = '"+nwtime+"',[WW] = '"+ww+"',[WWSYQX] = '"+wwtime+"' ,[USB] = '"+usb
                +"',[USBSYQX] = '"+usbtime+"',[GQ] = '"+gq+"',[GQSYQX] = '"+gqtime+"' ,[DNGLY] = '"+gly+"',[DNGLYSYQX] = '"+glytime+"',[BZ] = '"+bz
                + "',[ZHYCGXR] = '" + User.Identity.Name.ToString() + "',[ZHYCGXSJ] = getdate() WHERE Number='"+ViewState["Number"].ToString()+"'";
            int i = DbHelperSQL.ExecuteSql(strSQLInsert);
            if (i > 0)
            {
                MessageBox.Show(this, "信息修改成功！");                
            }
            else
            {
                MessageBox.Show(this, "信息修改失败！");
            }
        }
    }
    //返回按钮
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("YGSYDNXXCK.aspx");
        Response.End();
    }
}