using FMOP.DB;
using Hesion.Brick.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Key;

public partial class Web_JHJX_New2013_JHJX_UserDongJie_jhjx_UserDJ : System.Web.UI.Page
{
    string StrSavePath = "dongjiepz";
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if(!IsPostBack)
        {
            DisGrid("");
        }
    }    
    //设置初始默认值检索
    private Hashtable SetV()
    {
        /*---shiyan 2013-12-17 进行数据获取优化。---*/

        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = "10"; //必须设置,每页的数据量。必须是数字。不能是0。
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " ( select a.Number,b.J_BUYJSBH 买家角色编号, a.DLYX 交易方账号,a.JYFBH 交易方编号,a.JSZHLX 账户类型,b.I_JYFMC 交易方名称,b.I_ZCLB 注册类别,'分公司审核时间'=(select FGSSHSJ from AAA_MJMJJYZHYJJRZHGLB where DLYX =c.DLYX and SFSCGLJJR='是' and SFYX='是'),a.FWZXSHSJ 服务中心审核时间,'平台管理机构'=(case a.JSZHLX when '经纪人交易账户' then b.I_PTGLJG else (select I_PTGLJG from AAA_DLZHXXB where J_JJRJSBH=c.GLJJRBH) end),b.I_LXRXM 联系人姓名,b.I_LXRSJH 联系人手机号 from AAA_JYZHZSB a left join AAA_DLZHXXB b on a.DLYX =b.B_DLYX left join AAA_MJMJJYZHYJJRZHGLB c on a.DLYX =c.DLYX and c.SFDQMRJJR='是' where a.FWZXSHZT='建议冻结' and a.JYGLBSHZT='尚未处理' ) as tab ";
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件
        //ht_where["search_mainid"] = " Number ";  //所检索表的主键
        //ht_where["search_paixu"] = " asc ";  //排序方式
        //ht_where["search_paixuZD"] = " 服务中心审核时间";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_PTZHZLSH/JHJX_JYDJ_info.aspx";
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.  
        ht_where["serach_Row_str"] = " Number,DLYX as 交易方账号,JYFBH as 交易方编号,JSZHLX as 账户类型,'' as 交易方名称,'' as 注册类别,'' as 分公司审核时间, FWZXSHSJ as 服务中心审核时间,PTGLJG as 平台管理机构,'' as 联系人姓名,'' as 联系人手机号 ";
        ht_where["search_tbname"] = "  AAA_JYZHZSB ";
        ht_where["search_str_where"] = " FWZXSHZT='建议冻结' and JYGLBSHZT='尚未处理' ";  //检索条件
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " FWZXSHSJ ";  //用于排序的字段
        ht_where["returnlastpage_open"] = "New2013/JHJX_PTZHZLSH/JHJX_JYDJ_info.aspx";
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
            MessageBox.Show(this, "查询超时，请重试或者修改查询条件！");
        }     

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            /*---shiyan增加 2013-12-17 进行数据获取优化。---*/
            DataSet ds_info = new DataSet();
            object obj_fgschecktime = new object();
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                //获取登陆账号信息表中的相关信息
                ds_info = DbHelperSQL.Query("select I_JYFMC 交易方名称,I_ZCLB 注册类别,I_LXRXM 联系人姓名,I_LXRSJH 联系人手机号 from AAA_DLZHXXB where B_DLYX='" + NewDS.Tables[0].Rows[i]["交易方账号"].ToString() + "'");
              if (ds_info != null && ds_info.Tables[0].Rows.Count > 0)
              {
                  NewDS.Tables[0].Rows[i]["交易方名称"] = ds_info.Tables[0].Rows[0]["交易方名称"].ToString();
                  NewDS.Tables[0].Rows[i]["注册类别"] = ds_info.Tables[0].Rows[0]["注册类别"].ToString();
                  NewDS.Tables[0].Rows[i]["联系人姓名"] = ds_info.Tables[0].Rows[0]["联系人姓名"].ToString();
                  NewDS.Tables[0].Rows[i]["联系人手机号"] = ds_info.Tables[0].Rows[0]["联系人手机号"].ToString();
              }

                //获取分公司审核时间
              obj_fgschecktime = DbHelperSQL.GetSingle("select FGSSHSJ from AAA_MJMJJYZHYJJRZHGLB where DLYX ='" + NewDS.Tables[0].Rows[i]["交易方账号"].ToString() + "' and SFSCGLJJR='是' and SFYX='是' ");
              if (obj_fgschecktime != null && obj_fgschecktime.ToString() != "")
              {
                  NewDS.Tables[0].Rows[i]["分公司审核时间"] = obj_fgschecktime.ToString();
              }
            }
            /*---shiyan增加结束---*/

            Repeater1.DataSource = NewDS.Tables[0].DefaultView;
            ts.Visible = false;
        }
        else
        {
           // DataSet ds = DbHelperSQL.Query("select Number,'买家角色编号'='', '交易方账号'='','交易方编号'='','账户类型'='','交易方名称'='','注册类别'='','分公司审核时间'='','服务中心审核时间'='','平台管理机构'='','联系人姓名'='','联系人手机号'='' from AAA_JYZHZSB    where 1!=1");
            //Repeater1.DataSource = ds.Tables[0].DefaultView;
            Repeater1.DataSource = null;
            ts.Visible = true;

        }
        Repeater1.DataBind();
    }
    public void DisGrid(string sql_where)
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        //string sql_where = " and 交易方账号 ='" + txtjyfzh.Text.Trim() + "' and 交易方名称 ='" + txtjyfmc.Text.Trim() + "'";
        if (!string.IsNullOrEmpty(sql_where))
        {
            HTwhere["search_str_where"] += sql_where;
        }
       
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }
    protected void btn_check_Click(object sender, EventArgs e)
    {

        string Strsql = "select B_DLYX as '交易方账号', B_JSZHLX as '交易帐户类型',I_ZCLB as '注册类别',I_JYFMC as '交易方名称',I_LXRXM as  '联系人姓名',I_LXRSJH as '联系人手机号',I_JYFLXDH as '交易方联系电话',(I_SSQYS+I_SSQYSHI+I_SSQYQ) as 所属区域,  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = AAA_DLZHXXB.B_DLYX and SFDQMRJJR='是') as '关联经纪人',I_PTGLJG as '平台管理机构',B_SFDJ as '是否冻结' from dbo.AAA_DLZHXXB where 1=1 ";
        if (this.txtjyfzh.Text.Trim() == "" && this.txtjyfmc.Text.Trim() == "")
        {
            MessageBox.Show(this, "请输入您要查找的用户交易方账号或者交易方名称！");
            this.divinfo.Visible = false;
        }
        else
        {
            if (this.txtjyfzh.Text.Trim() != "")
            {
                Strsql += " and B_DLYX='" + this.txtjyfzh.Text.Trim() + "'";
            }
            if (this.txtjyfmc.Text.Trim() != "")
            {
                Strsql += " and I_JYFMC='" + this.txtjyfmc.Text.Trim() + "'";
            }
        }        
        DataSet dataSet = DbHelperSQL.Query(Strsql);
        if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
        {
            if (dataSet.Tables[0].Rows.Count != 1)
            {
                this.divinfo.Visible = false;
                MessageBox.Show(this, "查找到多天用户信息，请核实用户是否有重复账号！");
            }
            else
            {
                if (dataSet.Tables[0].Rows[0]["是否冻结"].ToString().Trim().Equals("否"))
                {
                    DataRow dr = dataSet.Tables[0].Rows[0];
                    labzhzh.Text = dr["交易方账号"].ToString().Trim();
                    labzhlx.Text = dr["交易帐户类型"].ToString();
                    this.labzclb.Text = dr["注册类别"].ToString();
                    this.labmc.Text = dr["交易方名称"].ToString().Trim();
                    lablxr.Text = dr["联系人姓名"].ToString();
                    this.lablxdh.Text = dr["联系人手机号"].ToString();
                    labjyflxdh.Text = dr["交易方联系电话"].ToString().Trim();
                    labzcd.Text = dr["所属区域"].ToString();
                    this.labgljjr.Text = dr["关联经纪人"].ToString();
                    this.labssfgs.Text = dr["平台管理机构"].ToString();
                    this.divinfo.Visible = true;
                    btnSave.Visible = true;
                    Button1.Visible = false;
                    btnBYDJ.Visible = false;
                }
                else
                {
                    FMOP.Common.MessageBox.Show(this, "帐户已被冻结！");
                    this.divinfo.Visible = false;
                    return;
                }
            }
        }
        else
        {
            this.divinfo.Visible = false;
            MessageBox.Show(this, "帐号不存在！");

        }
    }
    protected void btnyl_Click(object sender, EventArgs e)
    {
        
       // = this.FileUpload1.PostedFile.FileName;
        string urlstr = this.hidfilepath.Value;
        if (!urlstr.Equals(""))
        {
            //if (System.IO.File.Exists("C:\\Program Files\\Internet Explorer\\iexplore.exe"))
            //{
            //    System.Diagnostics.Process.Start("C:\\Program Files\\Internet Explorer\\iexplore.exe", "-new " + urlstr);
            //}
            //else
            //{
            //    System.Diagnostics.Process.Start(urlstr);
            //}
            //this.FileUpload1.PostedFile.FileName = this.hidfilepath.Value；

        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "没有附件可浏览！");
        }
       
    }
    protected void Btnupload_Click(object sender, EventArgs e)
    {
        if (this.FileUpload1.PostedFile.FileName != "")
        {
            
           // DataTable dt = ViewState["fujian"] as DataTable;
            this.Lablsit.Text = "";

            string StrRealPath = Server.MapPath(StrSavePath);
            if (!Directory.Exists(StrRealPath))
            {
                Directory.CreateDirectory(StrRealPath);
            }
            if (this.FileUpload1.PostedFile.ContentLength > 30000000)
            {
                FMOP.Common.MessageBox.Show(Page, "文件太大，无法上传！");
                return;
            }

            string strFilename = this.FileUpload1.PostedFile.FileName;


            string fileshortName = strFilename.Substring(strFilename.LastIndexOf('\\') + 1);


            if (fileshortName.Split('[').Length > 1 || fileshortName.Split(']').Length > 1)
            {
                FMOP.Common.MessageBox.Show(Page, "文件名称中不允许包含‘]’或者‘[’字符,假若确实需要，请使用中文‘】’或者‘【’代替！");
                //FMOP.Common.MessageBox.Show(Page, "文件名称中");
                return;
            }
            if (fileshortName.Split('%').Length > 1)
            {
                FMOP.Common.MessageBox.Show(Page, "文件名称中不允许包含‘%’字符！");
                return;
            }
            this.Lablsit.Text = fileshortName;
            /*上传附件*/

            string CilentFileName = FileUpload1.FileName;
            string ExName = strFilename.Substring(strFilename.LastIndexOf('.')).ToLower();
            string Filename = User.Identity.Name + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + fileshortName;

            this.Lablsit.ToolTip = Filename;

            string path = keyEncryption.encMe("/Web/JHJX/New2013/JHJX_UserDongJie/dongjiepz/" + Filename, "mimamima");
            Lablsit.Text = "<a href = '../../../picView/picView.aspx?path=" + path + "&jiami=y'  target='_blank'>" + fileshortName + "</a>";
            //this.Lablsit.Text = "<a href = 'dongjiepz/" + Filename + "'  target='_blank'>" + fileshortName + "</a>";
            this.FileUpload1.PostedFile.SaveAs(StrRealPath + "\\" + Filename);

            FMOP.Common.MessageBox.Show(Page, "上传成功！");
            this.fujian.Visible = true;
        }
        else
        {
            
            FMOP.Common.MessageBox.Show(Page, "请选择需要上传的文件！");
        }


    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //string Strisdongje = "是";
        bool havea = false;
        bool haveb = false;
        string djgnx = "|";
        string djyyx = "|";
        foreach (ListItem lt in CheckB_djgn.Items)
        {
            if (lt.Selected)
            {
                havea = true;
                djgnx += lt.Text + "|";
            }
        }

        if (!havea)
        {
            FMOP.Common.MessageBox.Show(Page, "请选择需要冻结的用户功能！");
            return;
        }

        foreach (ListItem lt in this.CheckB_djyy.Items)
        {
            if (lt.Selected)
            {
                haveb = true;
                djyyx += lt.Text + "|";
            }
        }

        if (!haveb)
        {
            FMOP.Common.MessageBox.Show(Page, "请选择冻结原因！");
            return;
        }

        if (Lablsit.Text.Trim().Equals(""))
        {
            FMOP.Common.MessageBox.Show(Page, "请上传冻结凭证！");
            return;
        }

        if (this.txtBZ.Text.Trim() == "")
        {
            FMOP.Common.MessageBox.Show(Page, "请填写冻结原因说明！");
            return;
        }

        try
        {
            ArrayList al = new ArrayList();

            string strzlyhb = " update dbo.AAA_DLZHXXB set B_SFDJ = '是' ,DJGNX = '" + djgnx + "' WHERE B_DLYX='" + labzhzh.Text + "'  ";
            al.Add(strzlyhb);
            string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_JhjxUserdongjieremark", ""); //生成主键
            string Strdjremark = "  insert into AAA_JhjxUserdongjieremark (Number, DLYX,JSZHLX,DJSGLJJR,PTGLJG,DJGN,DJYY,DJPZ,YYSM,CreateUser,CreateTime) values ('" + Number + "','" + labzhzh.Text + "','" + labzhlx.Text + "','" + labgljjr.Text + "','" + labssfgs.Text + "','" + djgnx + "','" + djyyx + "','" + this.Lablsit.ToolTip + "','" + this.txtBZ.Text + "','" + User.Identity.Name.ToString() + "',getdate()) ";
            al.Add(Strdjremark);
            DbHelperSQL.ExecSqlTran(al);

            MessageBox.ShowAndRedirect(this, "冻结操作成功！", "jhjx_UserDJ.aspx");
        }
        catch (Exception ex)
        {
            MessageBox.ShowAlertAndBack(this, "冻结操作失败！原因：" + ex.ToString());
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("jhjx_UserDJ.aspx");
    }
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if(e.CommandArgument=="ck")//查看详情
        {
            Response.Redirect("../JHJX_PTZHZLSH/JHJX_JYDJ_info.aspx?number=" + e.CommandName.Trim() + "");
        }
        else if (e.CommandArgument == "tydj")//同意冻结
        {
            string[] str = e.CommandName.Split('&');
            string DLYX = str[1].ToString();
            string Number = str[0].ToString();
            DJ(DLYX);
            spanJYZHZSBNuber.InnerText = Number;//交易账户终审表Number
            btnSave.Visible = false;
            Button1.Visible = true;
            btnBYDJ.Visible = false;
            CheckB_djgn.Enabled = true;
            CheckB_djyy.Enabled = true;
        }
        else if (e.CommandArgument == "bydj")//不予冻结
        {
            string[] str = e.CommandName.Split('&');
            string DLYX = str[1].ToString();
            string Number = str[0].ToString();
            DJ(DLYX);
            spanJYZHZSBNuber.InnerText = Number;//交易账户终审表Number
            btnSave.Visible = false;
            Button1.Visible = false;
            btnBYDJ.Visible=true;
            CheckB_djgn.Enabled = false;
            CheckB_djyy.Enabled = false;
        }
    }

    protected void DJ(string DLYX)
    {
        string Strsql = "select B_DLYX as '交易方账号', B_JSZHLX as '交易帐户类型',I_ZCLB as '注册类别',I_JYFMC as '交易方名称',I_LXRXM as  '联系人姓名',I_LXRSJH as '联系人手机号',I_JYFLXDH as '交易方联系电话',(I_SSQYS+I_SSQYSHI+I_SSQYQ) as 所属区域,  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = AAA_DLZHXXB.B_DLYX and SFDQMRJJR='是') as '关联经纪人',I_PTGLJG as '平台管理机构',B_SFDJ as '是否冻结',DJGN as 冻结功能,DJSGLJJR as 冻结时关联经纪人,DJGN as 记录表冻结功能,DJYY as 冻结原因,DJPZ as 冻结凭证,YYSM as 原因说明  from dbo.AAA_DLZHXXB left join (select top 1 * from AAA_JhjxUserdongjieremark where DLYX ='" + DLYX.Trim() + "'  order by CreateTime DESC )  as tabdjyx on tabdjyx .DLYX=AAA_DLZHXXB.B_DLYX where B_DLYX='" + DLYX.Trim() + "' ";
        
        DataSet ds_ZHXX = DbHelperSQL.Query(Strsql);      

        if (ds_ZHXX != null && ds_ZHXX.Tables[0].Rows.Count > 0)
        {
            if (ds_ZHXX.Tables[0].Rows.Count != 1)
            {
                MessageBox.Show(this, "查找到多个同登陆账号的信息，请先核实是否存在重复！");
                divinfo.Visible = false;
            }
            else
            {
                #region//赋值之前先清空
                foreach (ListItem djgn in CheckB_djgn.Items)
                {
                    djgn.Selected = false;
                }
                foreach (ListItem djyy in CheckB_djyy.Items)
                {
                    djyy.Selected = false;
                }
                txtBZ.Text = "";
                Lablsit.ToolTip = "";
                Lablsit.Text = "";
                #endregion
                divinfo.Visible = true ;
                DataRow dr = ds_ZHXX.Tables[0].Rows[0];
                labzhzh.Text = dr["交易方账号"].ToString().Trim();
                ViewState["交易方登录邮箱"] = dr["交易方账号"].ToString().Trim();
                labzhlx.Text = dr["交易帐户类型"].ToString();
                ViewState["交易方账户类别"] = dr["交易帐户类型"].ToString().Trim();
                labzclb.Text = dr["注册类别"].ToString();
                labmc.Text = dr["交易方名称"].ToString().Trim();
                lablxr.Text = dr["联系人姓名"].ToString();
                lablxdh.Text = dr["联系人手机号"].ToString();
                labjyflxdh.Text = dr["交易方联系电话"].ToString().Trim();
                labzcd.Text = dr["所属区域"].ToString();
                labgljjr.Text = dr["关联经纪人"].ToString();
                ViewState["当前关联经纪人"] = dr["关联经纪人"].ToString();
                labssfgs.Text = dr["平台管理机构"].ToString();

                if (dr["是否冻结"].ToString() == "是")
                {
                    fujian.Visible = true;

                    string[] arr_djgn = dr["冻结功能"].ToString().Split('|');
                    string[] arr_djyx = dr["冻结原因"].ToString().Split('|');

                    foreach (string djgn in arr_djgn)
                    {
                        if (CheckB_djgn.Items.Contains(new ListItem(djgn)))
                        {
                            CheckB_djgn.Items.FindByValue(djgn).Selected = true;
                        }
                    }
                    foreach (string djyy in arr_djyx)
                    {
                        if (CheckB_djyy.Items.Contains(new ListItem(djyy)))
                        {
                            CheckB_djyy.Items.FindByValue(djyy).Selected = true;
                        }
                    }

                    if (dr["冻结凭证"].ToString().Trim() != "")
                    {
                        Lablsit.ToolTip = dr["冻结凭证"].ToString();
                        string path = keyEncryption.encMe("/Web/JHJX/New2013/JHJX_UserDongJie/dongjiepz/" + dr["冻结凭证"].ToString(), "mimamima");
                        Lablsit.Text = "<a href = '../../../picView/picView.aspx?path=" + path + "&jiami=y'  target='_blank'>" + dr["冻结凭证"].ToString() + "</a>";
                       // Lablsit.Text = "<a href = 'dongjiepz/" + dr["冻结凭证"].ToString() + "'  target='_blank'>" + dr["冻结凭证"].ToString() + "</a>";
                    }

                    txtBZ.Text = dr["原因说明"].ToString();   
                }
                else
                {
                    fujian.Visible = false;                    
                }

                      
            }
        }
        else
        {
            MessageBox.Show(this, "该帐号不存在，请核实！");
            divinfo.Visible = false;
        }
    
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DJAndBYDJ("同意冻结");
    }
    protected void btnBYDJ_Click(object sender, EventArgs e)
    {
        DJAndBYDJ("不予冻结");
        
    }

    protected void DJAndBYDJ(string CZ)
    {
        bool havea = false;
        bool haveb = false;
        string djgnx = "|";
        string djyyx = "|";
        foreach (ListItem lt in CheckB_djgn.Items)
        {
            if (lt.Selected)
            {
                havea = true;
                djgnx += lt.Text + "|";
            }
        }
        if (CZ == "同意冻结")
        {
            if (!havea)
            {
                FMOP.Common.MessageBox.Show(Page, "请选择需要冻结的用户功能！");
                return;
            }
        }
        foreach (ListItem lt in this.CheckB_djyy.Items)
        {
            if (lt.Selected)
            {
                haveb = true;
                djyyx += lt.Text + "|";
            }
        }
        if (CZ == "同意冻结")
        {
            if (!haveb)
            {
                FMOP.Common.MessageBox.Show(Page, "请选择冻结原因！");
                return;
            }
        }

        if (Lablsit.Text.Trim().Equals(""))
        {
            FMOP.Common.MessageBox.Show(Page, "请上传凭证！");
            return;
        }

        if (this.txtBZ.Text.Trim() == "")
        {
            FMOP.Common.MessageBox.Show(Page, "请填写原因说明！");
            return;
        }

        try
        {
            Hesion.Brick.Core.User us_ls = Hesion.Brick.Core.Users.GetUserByNumber(User.Identity.Name);
            
            if (txtBZ.Text.Contains("'"))
            {
                txtBZ.Text = txtBZ.Text.Replace("'", "‘");
            }
            ArrayList al = new ArrayList();

            if(CZ=="同意冻结")
            {
                string strzlyhb = " update dbo.AAA_DLZHXXB set B_SFDJ = '是' ,DJGNX = '" + djgnx + "' WHERE B_DLYX='" + labzhzh.Text + "'  ";
                al.Add(strzlyhb);

                string strUpdateZSB = "update AAA_JYZHZSB set JYGLBSHZT='同意冻结',JYGLBSHSJ=getdate(),JYGLBSHYJ='" + this.txtBZ.Text + "',JYGLBSHR='" + us_ls.Name + "',JYGLBSCPZ='" + this.Lablsit.ToolTip + "' where Number='" + spanJYZHZSBNuber.InnerText.Trim() + "'";
                al.Add(strUpdateZSB);
            
                string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_JhjxUserdongjieremark", ""); //生成主键
                string Strdjremark = "  insert into AAA_JhjxUserdongjieremark (Number, DLYX,JSZHLX,DJSGLJJR,PTGLJG,DJGN,DJYY,DJPZ,YYSM,ZSBGLZ,CreateUser,CreateTime) values ('" + Number + "','" + labzhzh.Text + "','" + labzhlx.Text + "','" + labgljjr.Text + "','" + labssfgs.Text + "','" + djgnx + "','" + djyyx + "','" + this.Lablsit.ToolTip + "','" + this.txtBZ.Text + "','" + spanJYZHZSBNuber.InnerText.Trim() + "','" + us_ls.Name + "',getdate()) ";
                al.Add(Strdjremark);


                //自身开户申请  [x1]被平台终审被平台终审交易管理部同意冻结	-0.5	减项	平台终审时      
               //此项作废只扣自己的 关联交易方 关联交易方[x1]，被平台终审交易管理部同意冻结 -1	减项	平台终审时
                jhjx_JYFXYMX jhjxJYFXMMX = new jhjx_JYFXYMX();
                if( ViewState["交易方账户类别"].ToString()=="经纪人交易账户")
                {
                  string[] strArray1=jhjxJYFXMMX.ZSKHSQ_BPTJYGLBTYDJ(ViewState["交易方登录邮箱"].ToString(), "经纪人", us_ls.Name.ToString());
                  al.AddRange(strArray1);
                  //string[] strArray4 = jhjxJYFXMMX.GLJYF_BPTJYGLBTYDJ(ViewState["交易方登录邮箱"].ToString(), ViewState["交易方登录邮箱"].ToString(), "经纪人", us_ls.Name.ToString());
                  //al.AddRange(strArray4);
                }
                else if( ViewState["交易方账户类别"].ToString()=="买家卖家交易账户")
                {
                  string[] strArray2=jhjxJYFXMMX.ZSKHSQ_BPTJYGLBTYDJ(ViewState["交易方登录邮箱"].ToString(), "卖家", us_ls.Name.ToString());
                  al.AddRange(strArray2);
                   string[] strArry3=jhjxJYFXMMX.GLJYF_BPTJYGLBTYDJ(ViewState["当前关联经纪人"].ToString(), ViewState["交易方登录邮箱"].ToString(), "经纪人", us_ls.Name.ToString());
                   al.AddRange(strArry3);
                
                }



               
               


            }
            else if (CZ == "不予冻结")
            {
                string strUpdateZSB = "update AAA_JYZHZSB set JYGLBSHZT='不予冻结',JYGLBSHSJ=getdate(),JYGLBSHYJ='" + this.txtBZ.Text + "',JYGLBSHR='" + us_ls.Name + "',JYGLBSCPZ='" + this.Lablsit.ToolTip + "' where Number='" + spanJYZHZSBNuber.InnerText.Trim() + "'";
                al.Add(strUpdateZSB);
            
            }
            
            DbHelperSQL.ExecSqlTran(al);

            MessageBox.ShowAndRedirect(this, CZ+"操作成功！", "jhjx_UserDJ.aspx");
        }
        catch (Exception ex)
        {
            MessageBox.ShowAlertAndBack(this, "冻结操作失败！原因：" + ex.ToString());
        }
    }
}