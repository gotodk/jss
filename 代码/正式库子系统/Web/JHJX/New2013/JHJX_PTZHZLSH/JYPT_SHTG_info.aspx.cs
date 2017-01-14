using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_JHJX_PTZHZLSH_JYPT_SHTG_info : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["Number"] != null && Request.QueryString["Number"].ToString() != "")
            {
                ViewState["Number"] = Request.QueryString["Number"].ToString();
                BindDataFace();
            }

        }
    }
    /// <summary>
    /// 绑定界面数据信息
    /// </summary>
    public void BindDataFace()
    {

        DataTable dataTable = DbHelperSQL.Query("select DL.B_DLYX '交易方账号',DL.I_ZQZJZH '交易方编号',DL.I_JYFMC '交易方名称',DL.B_JSZHLX '交易账户类型',DL.I_ZCLB '注册类别',DL.I_SFZH '身份证号',DL.I_SFZSMJ '身份证扫描件',DL.I_SFZFMSMJ '身份证反面扫描件',DL.I_YYZZZCH '营业执照注册号',DL.I_YYZZSMJ '营业执照扫描件',DL.I_ZZJGDMZDM '组织机构代码证代码',DL.I_ZZJGDMZSMJ '组织机构代码证扫描件',DL.I_SWDJZSH '税务登记证税号',DL.I_SWDJZSMJ '税务登记证扫描件',DL.I_YBNSRZGZSMJ '一般纳税人资格证扫描件',DL.I_KHXKZH '开户许可证号',DL.I_KHXKZSMJ '开户许可证扫描件',DL.I_YLYJK '预留印鉴卡',DL.I_FDDBRXM '法定代表人姓名',DL.I_FDDBRSFZH '法定代表人身份证号',DL.I_FDDBRSFZSMJ '法定代表人身份证扫描件',DL.I_FDDBRSFZFMSMJ '法定代表人身份证反面扫描件',DL.I_FDDBRSQS '法定代表人授权书',DL.I_JYFLXDH '交易方联系电话',DL.I_SSQYS+DL.I_SSQYSHI+DL.I_SSQYQ '所属区域',DL.I_XXDZ '详细地址',DL.I_LXRXM '联系人姓名',DL.I_LXRSJH '联系人手机号',DL.I_KHYH '开户银行',DL.I_YHZH '银行账号',(select I_PTGLJG from AAA_DLZHXXB where J_JJRJSBH=GL.GLJJRBH) '平台管理机构',(select J_JJRZGZSBH from AAA_DLZHXXB where J_JJRJSBH=GL.GLJJRBH) '当前关联经纪人资格证书编号',(select I_JYFMC from AAA_DLZHXXB where J_JJRJSBH=GL.GLJJRBH) '当前关联经纪人名称',(select I_JYFLXDH from AAA_DLZHXXB where J_JJRJSBH=GL.GLJJRBH) '当前关联经纪人联系电话',GL.GLJJRBH '关联经纪人角色编号',GL.GLJJRDLZH '经纪人审核人',GL.JJRSHSJ '经纪人审核时间',GL.JJRSHYJ '经纪人审核意见',GL.FGSSHR '分公司审核人',GL.FGSSHSJ '分公司审核时间',GL.FGSSHYJ '分公司审核意见',ZS.FWZXSHR '服务中心审核人',ZS.FWZXSHSJ '服务中心审核时间',isnull(ZS.FWZXSHYJ,'') '服务中心审核意见' from AAA_DLZHXXB as DL inner join AAA_MJMJJYZHYJJRZHGLB as GL on DL.B_DLYX=GL.DLYX left join AAA_JYZHZSB as ZS on DL.I_ZQZJZH=ZS.JYFBH where  GL.SFYX='是' and GL.SFSCGLJJR='是' and GL.FGSSHZT='审核通过'  and ZS.Number='" + ViewState["Number"] + "'").Tables[0];

        if (dataTable.Rows.Count > 0)
        {

            //交易方编号
            ViewState["JYFBH"] = dataTable.Rows[0]["交易方编号"].ToString();
            ViewState["GLJJRJSBH"] = dataTable.Rows[0]["关联经纪人角色编号"].ToString();
            ViewState["PTGLJG"] = dataTable.Rows[0]["平台管理机构"].ToString();

            #region//显示数据信息
            this.lbl_JYFZH.Text = dataTable.Rows[0]["交易方账号"].ToString();
            this.lbl_JYFMC.Text = dataTable.Rows[0]["交易方名称"].ToString();
            this.lbl_JYZHLX.Text = dataTable.Rows[0]["交易账户类型"].ToString();
            this.lbl_ZCLB.Text = dataTable.Rows[0]["注册类别"].ToString();
            this.lbl_SFZH.Text = dataTable.Rows[0]["身份证号"].ToString();
            this.linkSFZSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["身份证扫描件"].ToString().Replace(@"\", "/");
            this.linkSFZSMJ_FM.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["身份证反面扫描件"].ToString().Replace(@"\", "/");

            this.lbl_YYZZZCH.Text = dataTable.Rows[0]["营业执照注册号"].ToString();
            this.linkYYZZSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["营业执照扫描件"].ToString().Replace(@"\", "/");
            this.lbl_ZZJGDMZDM.Text = dataTable.Rows[0]["组织机构代码证代码"].ToString();
            this.linkZZJGDMZSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["组织机构代码证扫描件"].ToString().Replace(@"\", "/");
            this.lbl_SWDJZSH.Text = dataTable.Rows[0]["税务登记证税号"].ToString();
            this.linkSWDJZSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["税务登记证扫描件"].ToString().Replace(@"\", "/");
            this.linkYBNSRZGZMSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["一般纳税人资格证扫描件"].ToString().Replace(@"\", "/");
            this.linkFDDBRSQSSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["法定代表人授权书"].ToString().Replace(@"\", "/");
            this.lbl_KHXKZH.Text = dataTable.Rows[0]["开户许可证号"].ToString();
            this.linkKHXKZSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["开户许可证扫描件"].ToString().Replace(@"\", "/");
            this.linkYLYJK.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["预留印鉴卡"].ToString().Replace(@"\", "/");
            this.lbl_FDDBRXM.Text = dataTable.Rows[0]["法定代表人姓名"].ToString();
            this.lbl_FDDBRSHZH.Text = dataTable.Rows[0]["法定代表人身份证号"].ToString();
            this.linkFDDBRSFZSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["法定代表人身份证扫描件"].ToString().Replace(@"\", "/");
            this.linkFDDBRSFZSMJ_FM.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["法定代表人身份证反面扫描件"].ToString().Replace(@"\", "/");
            this.lbl_JYFLXDH.Text = dataTable.Rows[0]["交易方联系电话"].ToString();
            this.lbl_SSQY.Text = dataTable.Rows[0]["所属区域"].ToString();
            this.lblXXDZ.Text = dataTable.Rows[0]["详细地址"].ToString();
            this.lbl_LXRXM.Text = dataTable.Rows[0]["联系人姓名"].ToString();
            this.lbl_LXRSJH.Text = dataTable.Rows[0]["联系人手机号"].ToString();
            this.lbl_KHYH.Text = dataTable.Rows[0]["开户银行"].ToString();
            this.lbl_YHZH.Text = dataTable.Rows[0]["银行账号"].ToString();
            this.lbl_PTGLJG.Text = dataTable.Rows[0]["平台管理机构"].ToString();
            this.lbl_GLJJRZGZSBH.Text = dataTable.Rows[0]["当前关联经纪人资格证书编号"].ToString();
            this.lbl_GLJJRMC.Text = dataTable.Rows[0]["当前关联经纪人名称"].ToString();
            this.lbl_GLJJRLXDH.Text = dataTable.Rows[0]["当前关联经纪人联系电话"].ToString();


            //经纪人审核
            this.lbl_JJR_SHR.Text = dataTable.Rows[0]["经纪人审核人"].ToString();
            this.lbl_JJR_SHSJ.Text = dataTable.Rows[0]["经纪人审核时间"].ToString();
            this.txt_JJR_SHYJ.Text = dataTable.Rows[0]["经纪人审核意见"].ToString();
            //分公司审核
            this.lbl_FGS_SHR.Text = dataTable.Rows[0]["分公司审核人"].ToString();
            this.lbl_FGS_SHSJ.Text = dataTable.Rows[0]["分公司审核时间"].ToString();
            this.txt_FGS_SHYJ.Text = dataTable.Rows[0]["分公司审核意见"].ToString();
            //服务中心审核
            this.lbl_FWZXSH_SHR.Text = dataTable.Rows[0]["服务中心审核人"].ToString();
            this.lbl_FWZX_SHSJ.Text = dataTable.Rows[0]["服务中心审核时间"].ToString();
            this.txt_FWZX_SHYJ.Text = dataTable.Rows[0]["服务中心审核意见"].ToString();

            #endregion





            #region//决定显示哪种界面
            if (dataTable.Rows[0]["注册类别"].ToString() == "单位")
            {
                this.trJYFZH_MC.Visible = true;
                this.trJYZHLX_LB.Visible = true;
                this.trJYFSFZ.Visible = false;
                this.trJYFYYZZ.Visible = true;
                this.trZZJGDMZ.Visible = true;
                this.trSWDJZ.Visible = true;
                this.trYBNSRZGZ.Visible = true;
                this.trKHXKZ.Visible = true;
                this.trYLYJK.Visible = true;
                this.trFDDBRSFZ.Visible = true;

                this.trJYFLXDH.Visible = true;
                this.trXXDZ.Visible = true;
                this.trLXRXM.Visible = true;
                this.trKHYH.Visible = true;



            }
            else if (dataTable.Rows[0]["注册类别"].ToString() == "自然人")
            {
                this.trJYFZH_MC.Visible = true;
                this.trJYZHLX_LB.Visible = true;
                this.trJYFSFZ.Visible = true;
                this.trJYFYYZZ.Visible = false;
                this.trZZJGDMZ.Visible = false;
                this.trSWDJZ.Visible = false;
                this.trYBNSRZGZ.Visible = false;
                this.trKHXKZ.Visible = false;
                this.trYLYJK.Visible = false;
                this.trFDDBRSFZ.Visible = false;

                this.trJYFLXDH.Visible = true;
                this.trXXDZ.Visible = true;
                this.trLXRXM.Visible = true;
                this.trKHYH.Visible = true;
            }

            if (dataTable.Rows[0]["交易账户类型"].ToString() == "经纪人交易账户")
            {
                this.trPTGLJG.Visible = true;
                this.trGLJJR.Visible = false;
                this.trGLJJRDH.Visible = false;
                this.tabJJRSH.Visible = false;
            }
            else //经纪人交易账户
            {
                this.trPTGLJG.Visible = false;
                this.trGLJJR.Visible = true;
                this.trGLJJRDH.Visible = true;
                this.tabJJRSH.Visible = true;
            }
            #endregion
        }
    }

    /// <summary>
    /// 返回列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("JYPT_SHTG.aspx");
    }
    /// <summary>
    /// 撤销原审核并建议冻结
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPass_Click(object sender, EventArgs e)
    {
        this.btnPass.Enabled = false;
        if (!string.IsNullOrEmpty(this.txt_FWZXX_SHYJ.Text.Trim()))
        {
            if (this.txt_FWZXX_SHYJ.Text.Trim().Contains("'"))
            {
                this.txt_FWZXX_SHYJ.Text = this.txt_FWZXX_SHYJ.Text.Trim().Replace("'", "‘");
            }
            if (this.txt_FWZXX_SHYJ.Text.Trim().Length > 1000)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('您输入的审核意见字符过长，请限制在1000个字符以内！');</script>");
                this.btnPass.Enabled = true;
                return;
            }
            Hesion.Brick.Core.User us_ls = Hesion.Brick.Core.Users.GetUserByNumber(User.Identity.Name);
            object objName = us_ls.Name.ToString();
            string url = "New2013/JHJX_PTZHZLSH/JYPT_SHTG.aspx";
            DbHelperSQL.ExecuteSql("update AAA_JYZHZSB set FWZXSHZT='建议冻结',FWZXXSHR='" + objName.ToString() + "',FWZXXSHSJ='" + DateTime.Now.ToString() + "',FWZXXSHYJ='" + this.txt_FWZXX_SHYJ.Text.Trim() + "',JYGLBSHZT='尚未处理' where JYFBH='" + ViewState["JYFBH"].ToString() + "'");
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核信息提交成功！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
        }

        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核意见不能为空，请填写审核意见！');</script>");
            this.btnPass.Enabled = true;

        }
    }
}