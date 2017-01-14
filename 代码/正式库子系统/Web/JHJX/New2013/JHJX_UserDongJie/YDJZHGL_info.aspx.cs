using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;
using System.IO;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
using Key;

public partial class Web_JHJX_New2013_JHJX_UserDongJie_YDJZHGL_info : System.Web.UI.Page
{
    string StrSavePath = "dongjiepz";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
 
            if (Request["dlyx"] != null && Request["dlyx"].ToString() != "")
            {
                labzhzh.Text = Request["dlyx"].ToString();
            }
            DisGrid();
        }
    }

    private void DisGrid()
    {
        string Strsql = "select B_DLYX as '交易方账号', B_JSZHLX as '交易帐户类型',I_ZCLB as '注册类别',I_JYFMC as '交易方名称',I_LXRXM as  '联系人姓名',I_LXRSJH as '联系人手机号',I_JYFLXDH as '交易方联系电话',(I_SSQYS+I_SSQYSHI+I_SSQYQ) as 所属区域,  b.关联经纪人,I_PTGLJG as '平台管理机构',B_SFDJ as '是否冻结',DJGN as 冻结功能,DJSGLJJR as 冻结时关联经纪人,DJGN as 记录表冻结功能,DJYY as 冻结原因,DJPZ as 冻结凭证,YYSM as 原因说明  from dbo.AAA_DLZHXXB left join (select DLYX, GLJJRDLZH as 关联经纪人 from  AAA_MJMJJYZHYJJRZHGLB where SFDQMRJJR='是') as b on b.DLYX=AAA_DLZHXXB.B_DLYX left join (select top 1 * from AAA_JhjxUserdongjieremark where DLYX ='" + labzhzh.Text.Trim() + "'  order by CreateTime DESC )  as tabdjyx on tabdjyx .DLYX=AAA_DLZHXXB.B_DLYX where B_DLYX='" + labzhzh.Text.Trim() + "' ";
        
        DataSet ds_ZHXX = DbHelperSQL.Query(Strsql);      

        if (ds_ZHXX != null && ds_ZHXX.Tables[0].Rows.Count > 0)
        {
            if (ds_ZHXX.Tables[0].Rows.Count != 1)
            {
                MessageBox.ShowAndRedirect(this, "查找到多个同登陆账号的信息，请先核实是否存在重复！", "YDJZHGL.aspx");
            }
            else
            {
                DataRow dr = ds_ZHXX.Tables[0].Rows[0];
                labzhzh.Text = dr["交易方账号"].ToString().Trim();
                labzhlx.Text = dr["交易帐户类型"].ToString();
                labzclb.Text = dr["注册类别"].ToString();
                labmc.Text = dr["交易方名称"].ToString().Trim();
                lablxr.Text = dr["联系人姓名"].ToString();
                lablxdh.Text = dr["联系人手机号"].ToString();
                labjyflxdh.Text = dr["交易方联系电话"].ToString().Trim();
                labzcd.Text = dr["所属区域"].ToString();
                labgljjr.Text = dr["关联经纪人"].ToString();
                labssfgs.Text = dr["平台管理机构"].ToString();

                string[] arr_djgn = dr["冻结功能"].ToString().Split ('|');
                string[] arr_djyx = dr["冻结原因"].ToString().Split ('|');

                foreach (string djgn in arr_djgn)
                {
                    if(CheckB_djgn .Items.Contains(new ListItem (djgn)))
                    {
                       CheckB_djgn .Items .FindByValue(djgn ).Selected =true;
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
                }

                txtBZ.Text = dr["原因说明"].ToString();              
            }
        }
        else
        {
            MessageBox.ShowAndRedirect(this, "该帐号不存在，请核实！", "YDJZHGL.aspx");
        }
    }
  //确认修改操作
    protected void btnQRXG_Click(object sender, EventArgs e)
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
            MessageBox.Show(this, "请选择需要冻结的用户功能！");
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
            MessageBox.Show(this, "请选择冻结原因！");
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
            MessageBox.ShowAndRedirect(this, "修改操作成功，将为您返回列表页面！", "YDJZHGL.aspx");
        }
        catch(Exception ex)
        {
            MessageBox.ShowAlertAndBack(this, "修改操作失败！原因：" + ex.ToString());
        }        
    }
    //返回列表操作
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("YDJZHGL.aspx");
    }
    //解冻操作
    protected void btnJieDong_Click(object sender, EventArgs e)
    {
        try
        {
            ArrayList al = new ArrayList();
            string strzlyhb = " update dbo.AAA_DLZHXXB set B_SFDJ = '否' ,DJGNX = '' WHERE B_DLYX='" + labzhzh.Text + "'  ";
            al.Add(strzlyhb);
            string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_JhjxUserdongjieremark", ""); //生成主键
            string Strdjremark = "  insert into AAA_JhjxUserdongjieremark (Number, DLYX,JSZHLX,DJSGLJJR,PTGLJG,DJGN,DJYY,DJPZ,YYSM,CreateUser,CreateTime) values ('" + Number + "','" + labzhzh.Text + "','" + labzhlx.Text + "','" + labgljjr.Text + "','" + labssfgs.Text + "','解冻','','" + this.Lablsit.ToolTip + "','" + this.txtBZ.Text + "','" + User.Identity.Name.ToString() + "',getdate()) ";
            al.Add(Strdjremark);

            #region//2014.01.02 新增功能--根据登录邮箱判断是否是经纪人交易账户--如果是将默认经纪人是次经纪人的 交易方账户的关联记录，除“是否当前默认”记录以外的，是否首次关联为“否”、审核状态为“审核中”或“驳回”、是否当前默认为“否”的所有记录的“是否有效”字段改为“否”。
            string strJJRJYZHLX = "select B_JSZHLX from AAA_DLZHXXB  where B_DLYX='" + labzhzh.Text + "'";
            object JYZHLX = DbHelperSQL.GetSingle(strJJRJYZHLX);
            if (JYZHLX != null && JYZHLX.ToString() == "经纪人交易账户")
            {
                string strUpdate = "update AAA_MJMJJYZHYJJRZHGLB set SFYX='否' where SFSCGLJJR='否' and SFDQMRJJR='否' and (JJRSHZT='审核中' or JJRSHZT='驳回') and DLYX in (select DLYX from AAA_MJMJJYZHYJJRZHGLB where SFDQMRJJR='是' and JJRSHZT='审核通过' and GLJJRDLZH='" + labzhzh.Text + "')";
                al.Add(strUpdate);
            }
            #endregion

            DbHelperSQL.ExecSqlTran(al);
            MessageBox.ShowAndRedirect(this, "解冻成功，将返回列表页面！", "YDJZHGL.aspx");
        }
        catch (Exception ex)
        {
            MessageBox.ShowAlertAndBack(this, "解冻失败！原因：" + ex.ToString());
        }
    }
      

//文件上传操作
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
            this.Lablsit.Text = "<a href = 'dongjiepz/" + Filename + "'  target='_blank'>" + fileshortName + "</a>";
            this.FileUpload1.PostedFile.SaveAs(StrRealPath + "\\" + Filename);

            FMOP.Common.MessageBox.Show(Page, "上传成功！");
            this.fujian.Visible = true;
        }
        else
        {

            FMOP.Common.MessageBox.Show(Page, "请选择需要上传的文件！");
        }
    }
}
