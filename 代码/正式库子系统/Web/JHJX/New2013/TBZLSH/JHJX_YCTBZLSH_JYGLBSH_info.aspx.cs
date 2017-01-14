using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using System.ComponentModel;
using System.Collections;

public partial class Web_JHJX_New2013_TBZLSH_JHJX_YCTBZLSH_JYGLBSH_info : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Params["Number"] != null && Request.Params["Number"].ToString() != "")//获取参数
            {

                ViewState["TBD_Number"] = Request.Params["Number"].ToString();
                BindData();
            }

        }
    }

    /// <summary>
    /// 绑定界面的基础数据信息
    /// </summary>
    public void BindData()
    {
        string strSql = "select SP.SPBH '商品编号',SP.SPMC '商品名称',SP.GG '执行标准',SP.JJDW '计价单位',TBD.Number '投标单编号',TBD.PTSDZDJJPL '平台设定最大经济批量',SP.SPMS '商品描述',DL.B_DLYX '交易方账号',DL.I_ZCLB '注册类别',DL.I_JYFMC '交易方名称',DL.I_SFZH '身份证号',DL.I_SFZSMJ '身份证扫描件',DL.I_SFZFMSMJ '身份证反面扫描件',DL.I_LXRXM '联系人姓名',DL.I_LXRSJH '联系人手机号',DL.I_JYFLXDH '交易方联系电话',DL.I_YYZZZCH '营业执照注册号',DL.I_YYZZSMJ '营业执照扫描件',DL.I_ZZJGDMZDM '组织机构代码证代码',DL.I_ZZJGDMZSMJ '组织机构代码证扫描件',DL.I_SWDJZSH '税务登记证税号',DL.I_SWDJZSMJ '税务登记证扫描件',DL.I_FDDBRSFZH '法定代表人身份证号',DL.I_FDDBRSFZSMJ '法定代表人身份证扫描件',DL.I_FDDBRSFZFMSMJ '法定代表人身份证反面扫描件',DL.I_YLYJK '预留印鉴卡',TBD.ZLBZYZM '质量标准与证明',TBD.CPJCBG '产品检测报告',TBD.PGZFZRFLCNS '品管总负责人法律承诺书',TBD.FDDBRCNS '法定代表人承诺书',TBD.SHFWGDYCN '售后服务规定与承诺',TBD.CPSJSQS '产品送检授权书',TBD.SLZM '税率证明',TBD.ZZ01 '特殊资质',TBSH.YCTBZZ '异常投标资质',TBSH.FWZXSHR '服务中心审核人',TBSH.FWZXSHSJ '服务中心审核时间' ,TBSH.FWZXSHYJ '服务中心审核意见',TBSH.JYGLBSHYJ '交易管理部审核意见' from dbo.AAA_TBZLSHB as TBSH left join  dbo.AAA_TBD as TBD on  TBSH.TBDH=TBD.Number left join dbo.AAA_PTSPXXB as SP on TBD.SPBH=SP.SPBH left join dbo.AAA_DLZHXXB as DL on TBD.DLYX=DL.B_DLYX  where TBSH.Number='" + ViewState["TBD_Number"].ToString().Trim() + "'";

        DataSet ds = DbHelperSQL.Query(strSql);

        if (ds.Tables[0].Rows.Count > 0)
        {

            #region//商品基本信息
            spanSPBH.InnerText = ds.Tables[0].Rows[0]["商品编号"].ToString();
            spanSPMC.InnerText = ds.Tables[0].Rows[0]["商品名称"].ToString();
            spanZXBZ.InnerText = ds.Tables[0].Rows[0]["执行标准"].ToString();
            spanJJDW.InnerText = ds.Tables[0].Rows[0]["计价单位"].ToString();
            spanPTSDZDJJPL.InnerText = ds.Tables[0].Rows[0]["平台设定最大经济批量"].ToString();
            spanSPMS.InnerText = ds.Tables[0].Rows[0]["商品描述"].ToString();
            #endregion

            #region//交易方基本信息
            spanJYFZH.InnerText = ds.Tables[0].Rows[0]["交易方账号"].ToString();
            ViewState["DLYX"] = ds.Tables[0].Rows[0]["交易方账号"].ToString();//卖家登录邮箱   
            ViewState["TBDBH"] = ds.Tables[0].Rows[0]["投标单编号"].ToString();//投标单编号
            spanJYFMC.InnerText = ds.Tables[0].Rows[0]["交易方名称"].ToString();
            spanLXRXM.InnerText = ds.Tables[0].Rows[0]["联系人姓名"].ToString();
            spanLXRSJH.InnerText = ds.Tables[0].Rows[0]["联系人手机号"].ToString();
            spanJYFLXDH.InnerText = ds.Tables[0].Rows[0]["交易方联系电话"].ToString();
            #endregion

            #region//交易方资质
            if (ds.Tables[0].Rows[0]["注册类别"].ToString() == "单位")
            {
                this.trYYZZ.Visible = true;
                this.trZZJGDMZ.Visible = true;
                this.trSWDJZ.Visible = true;
                this.trFDDBRSFZ.Visible = true;
                this.trYLYJK.Visible = true;
                this.trSFZ.Visible = false;
            }
            else
            {
                this.trYYZZ.Visible = false;
                this.trZZJGDMZ.Visible = false;
                this.trSWDJZ.Visible = false;
                this.trFDDBRSFZ.Visible = false;
                this.trYLYJK.Visible = false;
                this.trSFZ.Visible = true;
            }



            this.spanYYZZZCH.InnerText = ds.Tables[0].Rows[0]["营业执照注册号"].ToString().Replace(@"\", "/");
            this.linkYYZZ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["营业执照扫描件"].ToString().Replace(@"\", "/");
            this.spanZZJGDMZDM.InnerText = ds.Tables[0].Rows[0]["组织机构代码证代码"].ToString().Replace(@"\", "/");
            this.linkZZJGDMZ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["组织机构代码证扫描件"].ToString().Replace(@"\", "/");
            this.spanSWDJZSH.InnerText = ds.Tables[0].Rows[0]["税务登记证税号"].ToString().Replace(@"\", "/");
            this.linkSWDJZ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["税务登记证扫描件"].ToString().Replace(@"\", "/");
            this.spanFDDBRSFZ.InnerText = ds.Tables[0].Rows[0]["法定代表人身份证号"].ToString().Replace(@"\", "/");
            this.linkFDDBRSFZSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["法定代表人身份证扫描件"].ToString().Replace(@"\", "/");
            this.linkFDDBRSFZSMJ_FM.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["法定代表人身份证反面扫描件"].ToString().Replace(@"\", "/");
            this.linkYLYJKSMJ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["预留印鉴卡"].ToString().Replace(@"\", "/");

            this.spanSFZH.InnerText = ds.Tables[0].Rows[0]["身份证号"].ToString().Replace(@"\", "/");
            this.linkSFZ.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["身份证扫描件"].ToString().Replace(@"\", "/");
            this.linkSFZ_FM.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["身份证反面扫描件"].ToString().Replace(@"\", "/");

            #endregion


            #region//投标资质
          
            //特殊资质
            //资质格式如下：|资质xxx*1.png|资质bbb*2.png|
            string strTSZZ = "";
            string[] strArray = ds.Tables[0].Rows[0]["特殊资质"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            //异常投标资质
            string[] strYCTBZZ = ds.Tables[0].Rows[0]["异常投标资质"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("资质名称", typeof(System.String));
            dataTable.Columns.Add("资质路径", typeof(System.String));
            dataTable.Columns.Add("是否异常资质", typeof(System.String));

            dataTable.Rows.Add(new object[] { ds.Tables[0].Columns["质量标准与证明"].ColumnName.ToString(), "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["质量标准与证明"].ToString().Replace(@"\", "/"), "否" });
            dataTable.Rows.Add(new object[] { ds.Tables[0].Columns["产品检测报告"].ColumnName.ToString(), "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["产品检测报告"].ToString().Replace(@"\", "/"), "否" });
            dataTable.Rows.Add(new object[] { ds.Tables[0].Columns["品管总负责人法律承诺书"].ColumnName.ToString(), "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["品管总负责人法律承诺书"].ToString().Replace(@"\", "/"), "否" });
            dataTable.Rows.Add(new object[] { ds.Tables[0].Columns["法定代表人承诺书"].ColumnName.ToString(), "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["法定代表人承诺书"].ToString().Replace(@"\", "/"), "否" });
            dataTable.Rows.Add(new object[] { ds.Tables[0].Columns["售后服务规定与承诺"].ColumnName.ToString(), "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["售后服务规定与承诺"].ToString().Replace(@"\", "/"), "否" });
            dataTable.Rows.Add(new object[] { ds.Tables[0].Columns["产品送检授权书"].ColumnName.ToString(), "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["产品送检授权书"].ToString().Replace(@"\", "/"), "否" });
            //dataTable.Rows.Add(new object[] { ds.Tables[0].Columns["税率证明"].ColumnName.ToString(), "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["税率证明"].ToString().Replace(@"\", "/"), "否" });



            if (strArray.Length > 0)//说明存在特殊资质
            {
              
                for (int i = 0; i < strArray.Length; i++)
                {
                    string[] strSingle = strArray[i].Split('*');
                    string strTSZZMC = strSingle[0].ToString();//特殊资质名称
                    string strTSZZLJ = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + strSingle[1].ToString().Replace(@"\", "/");//特殊资质路径
                    dataTable.Rows.Add(new object[] { strTSZZMC, strTSZZLJ,"否" });
                }
            }
           
            //匹配数据
            if (strYCTBZZ.Length > 0)//存在异常投标资质
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < strYCTBZZ.Length; j++)
                    {
                        if (dataTable.Rows[i]["资质名称"].ToString() == strYCTBZZ[j].ToString())
                        {
                            dataTable.Rows[i]["是否异常资质"] = "是";
                            break;
                        }
                    }
                }
            }
            ViewState["TBZZ"] = dataTable;
           
            #endregion

            #region//审核意见
            this.lbl_FWZX_SHR.Text = ds.Tables[0].Rows[0]["服务中心审核人"].ToString();
            this.lbl_FWZX_SHSJ.Text = ds.Tables[0].Rows[0]["服务中心审核时间"].ToString();
            this.txtSHYJ.Text = ds.Tables[0].Rows[0]["服务中心审核意见"].ToString();
            this.txtJYGLB.Text = ds.Tables[0].Rows[0]["交易管理部审核意见"].ToString();
            #endregion
        }
    }



    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("JHJX_YCTBZLSH_JYGLBSH.aspx");
    }

    /// <summary>
    /// 审核通过
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPass_Click(object sender, EventArgs e)
    {
        this.btnPass.Enabled = false;
        this.btnReject.Enabled = false;
        if (!string.IsNullOrEmpty(this.txtJYGLB.Text.Trim()))
        {
            if (this.txtJYGLB.Text.Trim().Contains("'"))
            {
                this.txtJYGLB.Text = this.txtJYGLB.Text.Trim().Replace("'", "‘");
            }
            if (this.txtJYGLB.Text.Trim().Length > 1000)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('您输入的审核意见字符过长，请限制在1000个字符以内！');</script>");
                this.btnPass.Enabled = true;
                this.btnReject.Enabled = true;
                return;
            }
            string strYCTPZZ = "";
            object obj = Request["typeZZ"];
            Hesion.Brick.Core.User us_ls = Hesion.Brick.Core.Users.GetUserByNumber(User.Identity.Name);
            object objName = us_ls.Name.ToString();
            DbHelperSQL.ExecuteSql("update AAA_TBZLSHB set JYGLBSHZT='审核通过',JYGLBZHYCSHR='" + objName.ToString() + "',JYGLBZHYCSHSJ='" + DateTime.Now.ToString() + "',JYGLBSHYJ='" + this.txtJYGLB.Text.Trim() + "',YCTBZZ='',JYGLBSHWTGHSFXG='初始值' where Number='" + ViewState["TBD_Number"].ToString() + "'");
            string url = "New2013/TBZLSH/JHJX_YCTBZLSH_JYGLBSH.aspx";
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核信息提交成功！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
        }

        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核意见不能为空，请填写审核意见！');</script>");
            this.btnPass.Enabled = true;
            this.btnReject.Enabled = true;
        }
    }
    /// <summary>
    /// 审核未通过
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReject_Click(object sender, EventArgs e)
    {
        this.btnPass.Enabled = false;
        this.btnReject.Enabled = false;
        string strYCTPZZ = "";
        object obj = Request["typeZZ"];
        if (obj == null)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('请选择异常投标资质！');</script>");
            this.btnPass.Enabled = true;
            this.btnReject.Enabled = true;
            return;
        }
        else
        {
            strYCTPZZ = "|" + obj.ToString().Replace(',', '|') + "|";
        }
        if (!string.IsNullOrEmpty(this.txtJYGLB.Text.Trim()))
        {
            

            if (this.txtJYGLB.Text.Trim().Contains("'"))
            {
                this.txtJYGLB.Text = this.txtJYGLB.Text.Trim().Replace("'", "‘");
            }
            if (this.txtJYGLB.Text.Trim().Length > 1000)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('您输入的审核意见字符过长，请限制在1000个字符以内！');</script>");
                this.btnPass.Enabled = true;
                this.btnReject.Enabled = true;
                return;
            }
           
          
            Hesion.Brick.Core.User us_ls = Hesion.Brick.Core.Users.GetUserByNumber(User.Identity.Name);
            object objName = us_ls.Name.ToString();
            ArrayList arrayList = new ArrayList();
            string strSql = "update AAA_TBZLSHB set JYGLBSHZT='审核未通过',JYGLBZHYCSHR='" + objName.ToString() + "',JYGLBZHYCSHSJ='" + DateTime.Now.ToString() + "',JYGLBSHYJ='" + this.txtJYGLB.Text.Trim() + "',JYGLBSHWTGHSFXG='否',YCTBZZ='" + strYCTPZZ + "' where Number='" + ViewState["TBD_Number"].ToString() + "'";
            arrayList.Add(strSql);
            jhjx_JYFXYMX jhjxJYfXYMX = new jhjx_JYFXYMX();
         string[] strSqlArray=jhjxJYfXYMX.ZSZHMC_YCTBDJYGLBSHWTG(ViewState["DLYX"].ToString(), ViewState["TBDBH"].ToString(), "卖家", objName.ToString());
         arrayList.AddRange(strSqlArray);
         DbHelperSQL.ExecuteSqlTran(arrayList);
            //DbHelperSQL.ExecuteSql("update AAA_TBZLSHB set JYGLBSHZT='审核未通过',JYGLBZHYCSHR='" + objName.ToString() + "',JYGLBZHYCSHSJ='" + DateTime.Now.ToString() + "',JYGLBSHYJ='" + this.txtJYGLB.Text.Trim() + "',JYGLBSHWTGHSFXG='否',YCTBZZ='" + strYCTPZZ + "' where Number='" + ViewState["TBD_Number"].ToString() + "'");
            string url = "New2013/TBZLSH/JHJX_YCTBZLSH_JYGLBSH.aspx";
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核信息提交成功！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
        }

        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核意见不能为空，请填写审核意见！');</script>");
            this.btnPass.Enabled = true;
            this.btnReject.Enabled = true;
        }
    }
    /// <summary>
    /// 返回列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click1(object sender, EventArgs e)
    {
        Response.Redirect("JHJX_YCTBZLSH_JYGLBSH.aspx");
    }
}