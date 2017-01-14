using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using System.Collections;
using FMOP.DB;
using System.Data;
using System.Configuration;

public partial class Web_JHJX_JHJX_MMJSHXQ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["JSBH"] = "";
            ViewState["JJRBH"] = "";
            ViewState["DLYX"] = "";
            if (Request["JSBH"] != null)
            {
                ViewState["JSBH"] = Request["JSBH"].ToString();
                ViewState["JSLX"] = Server.UrlDecode(Request["JSLX"].ToString());
            }
            if (Request["JJRBH"] != null)
            {
                ViewState["JJRBH"] = Request["JJRBH"].ToString();                
            }
            if (Request["DLYX"] != null)
            {
                ViewState["DLYX"] = Request["DLYX"].ToString();
            }
            BindOperationData(ViewState["JSBH"].ToString(), ViewState["JSLX"].ToString(), ViewState["JJRBH"].ToString().Trim());
        }

    }

    //订单产品列表绑定，同时显示未发货量和当前办事处可用库存
    protected void BindOperationData(string strJSBH,string strJSLX,string jjrbh)
    {
        //DataSet dsCPLB = DbHelperSQL.Query("select id,XGLB as cplb,XGXH as cpxh,JG as jg,BHSL as sl,0 as yjsl,0 as wshsl from FGS_YZBBHDDJLB_XGXX where parentNumber='" + num + "' order by XGLB");
       // string strYX = DbHelperSQL.GetSingle("select ZCLX from ZZ_DLRJBZLXXB where JSBH='" + strJSBH + "'").ToString();
        if (strJSLX.Contains("买家"))  //买家基本信息
        {
            //签字承诺书
            this.trQZCNS.Visible= false;
            ////公司名称
            //this.trGSMC.Visible= false;
            ////公司电话
            //this.trGSDH.Visible = false;
            ////公司地址
            //this.trGSDZ.Visible = false;
            ////营业执照
            //this.trYYZZ.Visible = false;
            ////组织结构代码证
            //this.trZZJGDMZ.Visible= false;
            ////税务登记证
            //this.trSWDJZ.Visible = false;
            ////开户许可证
            //this.trKHXKZ.Visible = false;
            //string strSql = "select ZZ_DLRJBZLXXB.DLYX,ZZ_DLRJBZLXXB.YHM,ZZ_YHJSXXB.KTSHXX,ZZ_YHJSXXB.FGSKTSHXX,ZZ_DLRJBZLXXB.JSBH,LXRXM,SJH,SFYZSJH,SJHYZM,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,ZCLX,SFZSMJ,GSMC,GSDH,GSDZ,YYZZSMJ,ZZJGDMZSMJ,SWDJZSMJ,KHXKZSMJ,FDDBRQZDCNSSMJ,YXKPLX,YXKPXX from ZZ_DLRJBZLXXB inner join ZZ_YHJSXXB on ZZ_DLRJBZLXXB.JSBH=ZZ_YHJSXXB.JSBH   where ZZ_DLRJBZLXXB.JSBH='" + strJSBH + "'";
            //string strSql = "select JS.JSLX,JS.JSBH,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SFZH,MJ.SJH,MJ.SSSF,MJ.SSDS,MJ.SSQX,MJ.XXDZ,MJ.YZBM,MJ.SFZSMJ,JS.KTSHXX,JS.FGSKTSHXX,MJ.GSMC,MJ.GSDH,MJ.GSDZ,MJ.YYZZSMJ,MJ.ZZJGDMZSMJ,MJ.SWDJZSMJ,MJ.KHXKZSMJ,MJ.QTZZWJSMJ from ZZ_BUYJBZLXXB as MJ  left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH where MJ.JSBH='" + strJSBH + "'";
            string strSql = "select JS.JSLX,JS.JSBH,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SFZH,MJ.SJH,MJ.SSSF,MJ.SSDS,MJ.SSQX,MJ.XXDZ,MJ.YZBM,MJ.SFZSMJ,JS.KTSHXX,JS.FGSKTSHXX,MJ.GSMC,MJ.GSDH,MJ.GSDZ,MJ.YYZZSMJ,MJ.ZZJGDMZSMJ,MJ.SWDJZSMJ,MJ.KHXKZSMJ,MJ.QTZZWJSMJ,MMJ.DLRSHYJ,MMJ.FGSSHYJ from ZZ_BUYJBZLXXB as MJ  left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MMJ on MMJ.JSBH=MJ.JSBH and MMJ.DLRBH=JS.KTSHRBH where MJ.JSBH='" + strJSBH + "' and MMJ.DLRDLZH='" + jjrbh + "'";

            DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                txtZCJS.Text = dataTable.Rows[0]["JSLX"].ToString();
                txtYHM.Text = dataTable.Rows[0]["YHM"].ToString();
                txtZCYX.Text = dataTable.Rows[0]["DLYX"].ToString();
                txtLXRXM.Text = dataTable.Rows[0]["LXRXM"].ToString();
                txtSFZH.Text = dataTable.Rows[0]["SFZH"].ToString();
                txtSJH.Text = dataTable.Rows[0]["SJH"].ToString();
                //省市
                txtSZSF.Text = dataTable.Rows[0]["SSSF"].ToString();
                txtSZDS.Text = dataTable.Rows[0]["SSDS"].ToString();
                txtSSQX.Text = dataTable.Rows[0]["SSQX"].ToString();

                txtYXDZ.Text = dataTable.Rows[0]["XXDZ"].ToString();
                txtYZBM.Text = dataTable.Rows[0]["YZBM"].ToString();
                txtSFZSMJ.Text = dataTable.Rows[0]["SFZSMJ"].ToString();

                txtGSMC.Text = dataTable.Rows[0]["GSMC"].ToString();
                txtGSDH.Text = dataTable.Rows[0]["GSDH"].ToString();
                txtGSDZ.Text = dataTable.Rows[0]["GSDZ"].ToString();
                txtYYZZ.Text = dataTable.Rows[0]["YYZZSMJ"].ToString();
                txtZZJGDMZ.Text = dataTable.Rows[0]["ZZJGDMZSMJ"].ToString();
                txtSWDJZ.Text = dataTable.Rows[0]["SWDJZSMJ"].ToString();
                txtKHXKZ.Text = dataTable.Rows[0]["KHXKZSMJ"].ToString();
                txtQTZZ.Text = dataTable.Rows[0]["QTZZWJSMJ"].ToString();
                txtJJRSHYJ.Text = dataTable.Rows[0]["DLRSHYJ"].ToString();
                txtSHYJ.Text = dataTable.Rows[0]["FGSSHYJ"].ToString();
                //if (IsCompanyHeaderPerson() == "分公司人员")//加载分公司审核信息
                //{
                //    txtSHYJ.Text = dataTable.Rows[0]["FGSKTSHXX"].ToString();
                //}
            }
        }
        else if (strJSLX.Contains("卖家")) //显示企业注册的信息
        {

            //身份证号 
            this.trSFZH.Visible = false;
            //身份证扫描件
            this.trSFZSMJ.Visible = false;
            //签字承诺书
            this.trQZCNS.Visible = false;
            //string strSql = "select JS.JSLX,JS.JSBH,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SFZH,MJ.SJH,MJ.SSSF,MJ.SSDS,MJ.SSQX,MJ.XXDZ,MJ.YZBM,MJ.FDDBRQZDCNSSMJ,MJ.GSMC,MJ.GSDH,MJ.GSDZ,MJ.YYZZSMJ,MJ.ZZJGDMZSMJ,MJ.SWDJZSMJ,MJ.KHXKZSMJ,JS.FGSKTSHXX,JS.KTSHXX,MJ.QTZZWJSMJ from ZZ_SELJBZLXXB as MJ  left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH where MJ.JSBH='" + strJSBH + "'";
            string strSql = "select JS.JSLX,JS.JSBH,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SFZH,MJ.SJH,MJ.SSSF,MJ.SSDS,MJ.SSQX,MJ.XXDZ,MJ.YZBM,MJ.FDDBRQZDCNSSMJ,MJ.GSMC,MJ.GSDH,MJ.GSDZ,MJ.YYZZSMJ,MJ.ZZJGDMZSMJ,MJ.SWDJZSMJ,MJ.KHXKZSMJ,JS.FGSKTSHXX,JS.KTSHXX,MJ.QTZZWJSMJ,MMJ.DLRSHYJ,MMJ.FGSSHYJ from ZZ_SELJBZLXXB as MJ  left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH  left join ZZ_MMJYDLRZHGLB as MMJ on MMJ.JSBH=MJ.JSBH and MMJ.DLRBH=JS.KTSHRBH where MJ.JSBH='" + strJSBH + "' and MMJ.DLRDLZH='" + jjrbh + "'";
            DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
            txtZCJS.Text = dataTable.Rows[0]["JSLX"].ToString();
            txtYHM.Text = dataTable.Rows[0]["YHM"].ToString();
            txtZCYX.Text = dataTable.Rows[0]["DLYX"].ToString();
            txtLXRXM.Text = dataTable.Rows[0]["LXRXM"].ToString();
           // txtSFZH.Text = dataTable.Rows[0]["SFZH"].ToString();
            txtSJH.Text = dataTable.Rows[0]["SJH"].ToString();
            //省市
            txtSZSF.Text = dataTable.Rows[0]["SSSF"].ToString();
            txtSZDS.Text = dataTable.Rows[0]["SSDS"].ToString();
            txtSSQX.Text = dataTable.Rows[0]["SSQX"].ToString();

            txtYXDZ.Text = dataTable.Rows[0]["XXDZ"].ToString();
            txtYZBM.Text = dataTable.Rows[0]["YZBM"].ToString();
            //txtSFZSMJ.Text = dataTable.Rows[0]["SFZSMJ"].ToString();
            txtQZCNS.Text = dataTable.Rows[0]["FDDBRQZDCNSSMJ"].ToString();

            txtGSMC.Text = dataTable.Rows[0]["GSMC"].ToString();
            txtGSDH.Text = dataTable.Rows[0]["GSDH"].ToString();
            txtGSDZ.Text = dataTable.Rows[0]["GSDZ"].ToString();
            txtYYZZ.Text = dataTable.Rows[0]["YYZZSMJ"].ToString();
            txtZZJGDMZ.Text = dataTable.Rows[0]["ZZJGDMZSMJ"].ToString();
            txtSWDJZ.Text = dataTable.Rows[0]["SWDJZSMJ"].ToString();
            txtKHXKZ.Text = dataTable.Rows[0]["KHXKZSMJ"].ToString();
            txtQTZZ.Text = dataTable.Rows[0]["QTZZWJSMJ"].ToString();
            txtJJRSHYJ.Text = dataTable.Rows[0]["DLRSHYJ"].ToString();
            txtSHYJ.Text = dataTable.Rows[0]["FGSSHYJ"].ToString();
            //if (IsCompanyHeaderPerson() == "分公司人员")//加载分公司审核信息
            //{
            //    txtSHYJ.Text = dataTable.Rows[0]["FGSKTSHXX"].ToString();
            //}
            //else //加载总部审核信息
            //{
            //    txtSHYJ.Text = dataTable.Rows[0]["KTSHXX"].ToString();
            //}
        }
        BingLinkData();
    }
    //绑定link标签上的数据
    protected void BingLinkData()
    {
        if (this.txtSFZSMJ.Text.Trim() == "")
        {
            this.linkSFZSMJ.HRef = "";
        }
        else
        {
            this.linkSFZSMJ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtSFZSMJ.Text.Trim().Replace(@"\", "/");
        }
        if (this.txtQZCNS.Text.Trim() == "")
        {
            this.linkQZCNS.HRef = "";
        }
        else
        {
            this.linkQZCNS.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtQZCNS.Text.Trim().Replace(@"\", "/");
        }
        if (this.txtYYZZ.Text.Trim() == "")
        {
            this.linkYYZZ.HRef = "";
        }
        else
        {
            this.linkYYZZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtYYZZ.Text.Trim().Replace(@"\", "/");
        }
        if (this.txtZZJGDMZ.Text.Trim() == "")
        {
            this.linkZZJGDMZ.HRef = "";
        }
        else
        {
            this.linkZZJGDMZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtZZJGDMZ.Text.Trim().Replace(@"\", "/");
        }
        if (this.txtSWDJZ.Text.Trim() == "")
        {
            this.linkSWDJZ.HRef = "";
        }
        else
        {
            this.linkSWDJZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtSWDJZ.Text.Trim().Replace(@"\", "/");
        }
        if (this.txtKHXKZ.Text.Trim() == "")
        {
            this.linkKHXKZ.HRef = "";
        }
        else
        {
            this.linkKHXKZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtKHXKZ.Text.Trim().Replace(@"\", "/");
        }        
        if (this.txtQTZZ.Text.Trim() == "")
        {
            this.linkQTZZ.HRef = "";
        }
        else
        {
            this.linkQTZZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtQTZZ.Text.Trim().Replace(@"\", "/");
        }
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
        //当前用户为分公司人员

            if (this.txtSHYJ.Text.Trim().Contains("'"))
            {
                this.txtSHYJ.Text = this.txtSHYJ.Text.Trim().Replace("'", "‘");
            }

                ArrayList arrayList = new ArrayList();
                string strSqlJSB1 = "";
                string strSqlGLB2 = "";
                string strMJGLB = "";
                string strSql3 = "";
                if (this.txtZCJS.Text.ToString() == "买家")
                {

                    DataTable dataTable = DbHelperSQL.Query("select Number,DLYX,YHM,JSZHLX,JSBH,LXRXM,SJH,SSSF,SSDS,SSQX,XXDZ,YZBM,SFZH,GSMC from ZZ_BUYJBZLXXB where DLYX='" + txtZCYX.Text.ToString().Trim() + "'").Tables[0];
                    DataTable dataTableInfo = DbHelperSQL.Query("select DLRBH as '经纪人角色编号',JSDLZH as '买家登录账号' from ZZ_MMJYDLRZHGLB where JSDLZH='" + this.txtZCYX.Text.ToString().Trim() + "' and JSLX='买家'").Tables[0];
                    if (GetSHZT())
                    {
                        strSqlJSB1 = "update ZZ_YHJSXXB set FGSKTSHZT='审核通过',FGSKTSHRBH='" + User.Identity.Name + "',FGSKTSHTGSJ =GETDATE(),FGSKTSHXX='" + this.txtSHYJ.Text.ToString().Trim() + "' where JSBH='" + ViewState["JSBH"].ToString().Trim() + "' ";
                    }
                    strSqlGLB2 = "update ZZ_MMJYDLRZHGLB set FGSSHZT ='已审核',FGSSHSJ=GETDATE(),FGSKTSHRBH='" + User.Identity.Name + "',FGSSHYJ='" + this.txtSHYJ.Text.ToString().Trim() + "' where JSBH='" + ViewState["JSBH"].ToString().Trim() + "' and DLRDLZH='" + ViewState["JJRBH"].ToString().Trim() + "'";

                    //WorkFlowModule WFPKNumber = new WorkFlowModule("ZZ_SHDZXXB");
                    //string JFXXBNumber = WFPKNumber.numberFormat.GetNextNumber();//生成主键
                    string JFXXBNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_SHDZXXB", ""); //生成主键
                    if (GetSHZT())
                    {
                        strSql3 = "insert ZZ_SHDZXXB(Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX,SHDWMC,SHRXM,SZSF,SZDS,SZQX,XXDZ,YZBM,LXDH,SFMRDZ,CheckState,CreateUser,CreateTime,CheckLimitTime) values('" + JFXXBNumber.ToString() + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','" + dataTable.Rows[0]["JSZHLX"].ToString() + "','" + dataTable.Rows[0]["JSBH"].ToString() + "','买家','" + dataTable.Rows[0]["GSMC"].ToString() + "','" + dataTable.Rows[0]["LXRXM"].ToString() + "','" + dataTable.Rows[0]["SSSF"].ToString() + "','" + dataTable.Rows[0]["SSDS"].ToString() + "','" + dataTable.Rows[0]["SSQX"].ToString() + "','" + dataTable.Rows[0]["XXDZ"].ToString() + "','" + dataTable.Rows[0]["YZBM"].ToString() + "','" + dataTable.Rows[0]["SJH"].ToString() + "','是','1','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),GETDATE())";
                    }
                    arrayList.Add(strSqlJSB1);
                    arrayList.Add(strSqlGLB2);
                    arrayList.Add(strSql3);
                    //发送电子邮件
                    Hashtable buyerHashTable = new Hashtable();
                    buyerHashTable["登录邮箱"] = txtZCYX.Text.ToString().Trim();
                    buyerHashTable["用户名"] = txtYHM.Text.ToString().Trim();
                    buyerHashTable["结算账户"] = "买家结算账户";
                    buyerHashTable["手机号"] = this.txtSJH.Text.ToString();
                    buyerHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");
                    SendVerifyPassEmailPhone(buyerHashTable);
                    if (DbHelperSQL.ExecSqlTran(arrayList))
                    {
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核信息提交成功！');</script>");
                    }
                    else
                    {
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器异常，请联系管理员！');</script>");
                    }

                }
                else if (this.txtZCJS.Text.ToString() == "卖家")
                {
                  
                    //发送电子邮件
                    Hashtable sellerHashTable = new Hashtable();
                    sellerHashTable["登录邮箱"] = txtZCYX.Text.ToString().Trim();
                    sellerHashTable["用户名"] = txtYHM.Text.ToString().Trim();
                    sellerHashTable["结算账户"] = "卖家结算账户和买家结算账户";
                    sellerHashTable["手机号"] = this.txtSJH.Text.ToString();
                    sellerHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");
                    SendVerifyPassEmailPhone(sellerHashTable);

                    string[] strSql =VerifySellerDataPass();
                    if (strSql[0] == "ok")
                    {
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核信息提交成功！');</script>");
                    }
                    else
                    {
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器异常，请联系管理员！');</script>");
                    }
                }

    }
    #region //审核失败成功后发送的电子邮件

    /// <summary>
    /// 审核成功后，发送邮件  发送短信
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public string SendVerifyPassEmailPhone(Hashtable hashTable)
    {
        string[] strArray = new string[2];

        try
        {

            //发送电子邮件
            ClassEmail CE = new ClassEmail();
            string strSuject = "中国商品批发交易平台-资料审核";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/Web/JHJX/JSZHVerifySuccessEmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[yhm]]"] = hashTable["用户名"];
            ht["[[jszh]]"] = hashTable["结算账户"];
            ht["[[DateTime]]"] = hashTable["发送时间"];
            CE.SendSPEmail(hashTable["登录邮箱"].ToString(), strSuject, modHtmlFileName, ht);

            //发送短信
            string strMess = "您的资料已经通过审核，可正常使用" + hashTable["结算账户"].ToString() + "。[中国商品批发交易平台]";
            object[] ob = { hashTable["手机号"].ToString(), strMess };
            object return_ob = WebServiceHelper.InvokeWebService("http://192.168.0.10:8183/MessageService.asmx", "SendMessage", ob);
            return "发送成功";
        }
        catch (Exception ex)
        {
            return "发送失败";
        }
    }


    /// <summary>
    /// 审核失败后发送电子邮件
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public string SendVerifyRejectEmailPhone(Hashtable hashTable)
    {
        string[] strArray = new string[2];

        try
        {
            ClassEmail CE = new ClassEmail();
            string strSuject = "中国商品批发交易平台-资料审核";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/Web/JHJX/JSZHVerifyFailedEmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[yhm]]"] = hashTable["用户名"];
            ht["[[reason]]"] = hashTable["驳回缘由"];
            ht["[[DateTime]]"] = hashTable["发送时间"];
            CE.SendSPEmail(hashTable["登录邮箱"].ToString(), strSuject, modHtmlFileName, ht);

            //发送短信
            string strMess = "[" + hashTable["用户名"].ToString() + "]的资料分公司审核失败，请修改后重新提交。[中国商品批发交易平台]";
            object[] ob = { hashTable["手机号"].ToString(), strMess };
            object return_ob = WebServiceHelper.InvokeWebService("http://192.168.0.10:8183/MessageService.asmx", "SendMessage", ob);

            return "发送成功";
        }
        catch (Exception ex)
        {
            return "发送失败";
        }
    }
   
   

    #endregion




    /// <summary>
    /// 审核卖家基本信息  审核通过
    /// </summary>
    /// <param name="dataTableInfo"></param>
    /// <returns></returns>
    public  string[] VerifySellerDataPass()
    {
        DataTable dataTableInfo = DbHelperSQL.Query("select DLRBH as '经纪人角色编号',JSDLZH as '卖家登录账号' from ZZ_MMJYDLRZHGLB where JSDLZH='" + this.txtZCYX.Text.ToString().Trim() + "' and JSLX='卖家'").Tables[0];
        string[] result = new string[2];
        //获取角色编号的相关信息       
        Hashtable jjrUserInfo = GetUserInfo(dataTableInfo.Rows[0]["经纪人角色编号"].ToString());
        //查询卖家基本信息资料表
        DataTable dataTable = DbHelperSQL.Query("select  Number,DLYX,YHM,JSBH,LXRXM,SJH,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,GSMC,GSDH,GSDZ,YYZZSMJ,ZZJGDMZSMJ,SWDJZSMJ,KHXKZSMJ,FDDBRQZDCNSSMJ,QTZZWJSMJ,YXKPLX,YXKPXX from  ZZ_SELJBZLXXB where DLYX='" + dataTableInfo.Rows[0]["卖家登录账号"].ToString() + "' ").Tables[0];

        //一：更新买卖家与经纪人账户关联表


        string strSql1 = "update ZZ_MMJYDLRZHGLB set FGSSHZT ='已审核',FGSSHSJ=GETDATE(),FGSKTSHRBH='" + User.Identity.Name + "',FGSSHYJ='" + this.txtSHYJ.Text.ToString().Trim() + "' where DLRSHZT='已审核' and FGSSHZT ='待审核' and JSDLZH='" + ViewState["DLYX"].ToString().Trim() + "'  and DLRDLZH='" + ViewState["JJRBH"].ToString().Trim() + "'";
        string strSql2 = "";
        string strSql3 = "";
        string sqlMJJBJSXXB = "";
        string strJSXXB = "";
        string strGLB = "";
        /*
        //二.1：查询角色信息表里面是否已经存在  此角色信息
        string strSqll = "select COUNT(*) from ZZ_YHJSXXB  where  and JSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'  and JSLX='卖家'  ";
        int intCount = (int)DbHelperSQL.GetSingle(strSqll);
       
        
        if (intCount == 0) //角色表里面不存在及，本次操作是第一次审核操作
        {
            //二.2：向用户角色信息表插入数据
            WorkFlowModule YHJSXXBNumber = new WorkFlowModule("ZZ_YHJSXXB");
            string strYHJSXXBNumber = YHJSXXBNumber.numberFormat.GetNextNumber();//生成主键
            strSql2 = "insert into ZZ_YHJSXXB(Number,DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,KTSHRLX,FGSKTSHXX,FGSKTSHRBH,FGSKTSHTGSJ,FGSKTSHZT,KTSHRBH,KTSHTGSJ,KTSHXX,DLRZTJSXYH,CheckState,CreateTime,CheckLimitTime) values('" + YHJSXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','卖家账户','" + dataTable.Rows[0]["JSLX"].ToString() + "','" + dataTable.Rows[0]["JSBH"].ToString() + "',GETDATE(),'审核通过','经纪人','','','','','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),'" + dataTableInfo.Rows[0]["经纪人审核意见"].ToString() + "','否','1',GETDATE(),GETDATE())";
        }
        else if (intCount > 0)
        {
         * */
        //二.3：向角色信息表更新数据
        if (GetSHZT())
        {
            strSql2 = "update ZZ_YHJSXXB set FGSKTSHZT='审核通过',FGSKTSHRBH='" + User.Identity.Name + "',FGSKTSHTGSJ =GETDATE(),FGSKTSHXX='" + this.txtSHYJ.Text.ToString().Trim() + "' where JSBH='" + ViewState["JSBH"].ToString().Trim() + "' ";
        
        //}

         //生成买家角色编号
         string strBuyerRole = jhjx_PublicClass.GetNextNumberZZ("ZZ_YHJSXXB", "C");

        //三：向收获地址信息表中插入数据
        WorkFlowModule WFPKNumber = new WorkFlowModule("ZZ_SHDZXXB");
        string JFXXBNumber = WFPKNumber.numberFormat.GetNextNumberZZ("");//生成主键
        strSql3 = "insert ZZ_SHDZXXB(Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX,SHDWMC,SHRXM,SZSF,SZDS,SZQX,XXDZ,YZBM,LXDH,SFMRDZ,CheckState,CreateUser,CreateTime,CheckLimitTime) values('" + JFXXBNumber.ToString() + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','卖家账户','" + strBuyerRole + "','买家','" + dataTable.Rows[0]["GSMC"].ToString() + "','" + dataTable.Rows[0]["LXRXM"].ToString() + "','" + dataTable.Rows[0]["SSSF"].ToString() + "','" + dataTable.Rows[0]["SSDS"].ToString() + "','" + dataTable.Rows[0]["SSQX"].ToString() + "','" + dataTable.Rows[0]["XXDZ"].ToString() + "','" + dataTable.Rows[0]["YZBM"].ToString() + "','" + dataTable.Rows[0]["SJH"].ToString() + "','是','1','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),GETDATE())";
        //四：买家基本信息表里面插入一条数据
        //WorkFlowModule BUYJBZLXXBNumber = new WorkFlowModule("ZZ_BUYJBZLXXB");
        //string strBUYJBZLXXBNumber = BUYJBZLXXBNumber.numberFormat.GetNextNumber();//生成主键
        string strBUYJBZLXXBNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_BUYJBZLXXB", "");//生成主键
        sqlMJJBJSXXB = "insert ZZ_BUYJBZLXXB(Number,DLYX,YHM,JSZHLX,JSBH,LXRXM,SJH,SFYZSJH,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,SFZSMJ,CheckState,CreateUser,CreateTime,CheckLimitTime) values('" + strBUYJBZLXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','卖家账户','" + strBuyerRole + "','" + dataTable.Rows[0]["LXRXM"].ToString() + "','" + dataTable.Rows[0]["SJH"].ToString() + "','否','" + dataTable.Rows[0]["SSSF"].ToString() + "','" + dataTable.Rows[0]["SSDS"].ToString() + "','" + dataTable.Rows[0]["SSQX"].ToString() + "','" + dataTable.Rows[0]["SSFGS"].ToString() + "','" + dataTable.Rows[0]["XXDZ"].ToString() + "','" + dataTable.Rows[0]["YZBM"].ToString() + "','" + dataTable.Rows[0]["SFZH"].ToString() + "','','1','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),GETDATE())";
        //五：向基本角色信息表里面插入一条数据
        //WorkFlowModule buyerJSNumber = new WorkFlowModule("ZZ_YHJSXXB");
        //string strbuyerJSNumber = buyerJSNumber.numberFormat.GetNextNumber();//生成主键
        string strbuyerJSNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_YHJSXXB", "");//生成主键
        //向角色信息表中插入数据
        strJSXXB = "insert ZZ_YHJSXXB(Number,DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,FGSKTSHZT,KTSHRLX,KTSHRBH,FGSKTSHRBH,KTSHTGSJ,FGSKTSHTGSJ,KTSHXX,FGSKTSHXX,DLRZTJSXYH,CheckState,CreateUser,CreateTime,CheckLimitTime)  values('" + strbuyerJSNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','卖家账户','买家','" + strBuyerRole + "',GETDATE(),'审核通过','审核通过','经纪人','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "','" + User.Identity.Name + "',GETDATE(),GETDATE(),'','','否','1','" + User.Identity.Name + "',GETDATE(),GETDATE())";
        //六：向买、卖家与经纪人关联表中插入一条数据
        //WorkFlowModule MMJYDLRZHGLBNumber = new WorkFlowModule("ZZ_MMJYDLRZHGLB");
        //string strMMJYDLRZHGLBNumber = MMJYDLRZHGLBNumber.numberFormat.GetNextNumber();//生成主键
        string strMMJYDLRZHGLBNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_MMJYDLRZHGLB", "");//生成主键
        strGLB = "insert ZZ_MMJYDLRZHGLB(Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX,DLRDLZH,DLRBH,DLRYHM,SFMRDLR,SQGLSJ,DLRSHZT,DLRSHSJ,DLRSHYJ,GLZT,SFZTXYW,CheckState,CreateUser,CreateTime,CheckLimitTime,FGSSHZT,FGSSHSJ,FGSKTSHRBH,FGSSHYJ)  values('" + strMMJYDLRZHGLBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','卖家账户','" + strBuyerRole + "','买家','" + jjrUserInfo["登录邮箱"].ToString() + "','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "','" + jjrUserInfo["用户名"].ToString() + "','是',GETDATE(),'已审核',GETDATE(),'','有效','否','1','" + dataTableInfo.Rows[0]["经纪人角色编号"].ToString() + "',GETDATE(),GETDATE(),'已审核',GETDATE(),'" + User.Identity.Name + "','" + this.txtSHYJ.Text.ToString().Trim() + "')";
        }
        ArrayList arrayList = new ArrayList();
        arrayList.Add(strSql1);
        arrayList.Add(strSql2);
        arrayList.Add(strSql3);
        arrayList.Add(sqlMJJBJSXXB);
        arrayList.Add(strJSXXB);
        arrayList.Add(strGLB);
        bool isSuccess = DbHelperSQL.ExecSqlTran(arrayList);
        if (isSuccess)
        {
            result[0] = "ok";
            result[1] = "审核信息保存成功！";
        }
        else
        {
            result[0] = "失败";
            result[1] = "审核信息保存失败！";
        }
        return result;
    }


    /// <summary>
    /// 通过角色编号获取经纪人的相关信息
    /// </summary>
    /// <param name="bh">角色编号</param>
    /// <returns>带有信息的哈希表</returns>
    public static Hashtable GetUserInfo(string bh)
    {
        Hashtable HTUserInfo = new Hashtable();
        HTUserInfo["登录邮箱"] = "";
        HTUserInfo["用户名"] = "";
        HTUserInfo["结算账户类型"] = "";
        HTUserInfo["角色类型"] = "";
        HTUserInfo["是否审核通过"] = "";
        //获取单个数据
        DataSet ds_p1 = DbHelperSQL.Query("select top 1 * from ZZ_YHJSXXB where JSBH = '" + bh + "' order by CreateTime DESC");
        if (ds_p1 != null && ds_p1.Tables[0].Rows.Count > 0)
        {
            HTUserInfo["登录邮箱"] = ds_p1.Tables[0].Rows[0]["DLYX"].ToString();
            HTUserInfo["用户名"] = ds_p1.Tables[0].Rows[0]["YHM"].ToString();
            HTUserInfo["结算账户类型"] = ds_p1.Tables[0].Rows[0]["JSZHLX"].ToString();
            HTUserInfo["角色类型"] = ds_p1.Tables[0].Rows[0]["JSLX"].ToString();
            HTUserInfo["是否审核通过"] = ds_p1.Tables[0].Rows[0]["KTSHZT"].ToString();
        }

        return HTUserInfo;
    }









   

    ///// <summary>
    ///// 判断当前用户是总户人员还是分公司人员
    ///// </summary>
    ///// <returns></returns>
    //public string IsCompanyHeaderPerson()
    //{

    //    string QX = "SELECT  BM from HR_EMPLOYEES WHERE NUMBER ='" + User.Identity.Name + "'";//判断权限
    //    //当前用户为分公司人员
    //    if ((DbHelperSQL.GetSingle(QX).ToString().IndexOf("办事处") > 0) && User.Identity.Name != "admin")
    //    {
    //        return "分公司人员";
    //    }
    //    else
    //    {
    //        return "总部人员";
    //    }
    //}

    /// <summary>
    /// 审核驳回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReject_Click(object sender, EventArgs e)
    {
        
        this.btnReject.Enabled = false;
        this.btnPass.Enabled = false;
        if (!string.IsNullOrEmpty(this.txtSHYJ.Text.Trim()))
        {
            if (this.txtSHYJ.Text.Trim().Contains("'"))
            {
                this.txtSHYJ.Text = this.txtSHYJ.Text.Trim().Replace("'", "‘");
            }
       

                ArrayList arrayList = new ArrayList();
                string strSqlJSB = "";
                string strSqlGLB = "";
                if (this.txtZCJS.Text.ToString() == "买家")
                {                    
                    if (GetSHZT())
                    {
                        strSqlJSB = "update ZZ_YHJSXXB set KTSHZT='驳回',FGSKTSHZT='驳回',FGSKTSHRBH='" + User.Identity.Name + "',FGSKTSHTGSJ =GETDATE(),FGSKTSHXX='" + txtSHYJ.Text.ToString().Trim() + "' where JSBH='" + ViewState["JSBH"].ToString() + "' ";
                    }
                    strSqlGLB = "update ZZ_MMJYDLRZHGLB set DLRSHZT='驳回', FGSSHZT ='驳回',FGSSHSJ=GETDATE(),FGSKTSHRBH='" + User.Identity.Name + "',FGSSHYJ='" + txtSHYJ.Text.ToString().Trim() + "' where JSBH='" + ViewState["JSBH"].ToString() + "' and DLRBH='" + ViewState["JJRBH"].ToString().Trim() + "'";
                    arrayList.Add(strSqlJSB);
                    arrayList.Add(strSqlGLB);

                    Hashtable rejectHashTable = new Hashtable();
                    rejectHashTable["登录邮箱"] = this.txtZCYX.Text.ToString().Trim();
                    rejectHashTable["用户名"] = this.txtYHM.Text.ToString().Trim();
                    rejectHashTable["手机号"] = this.txtSJH.Text.ToString();
                    rejectHashTable["驳回缘由"] = txtSHYJ.Text.ToString().Trim();
                    rejectHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");
                    SendVerifyRejectEmailPhone(rejectHashTable);
                }
                else if (this.txtZCJS.Text.ToString() == "卖家")
                {
                    if (GetSHZT())
                    {
                        strSqlJSB = "update ZZ_YHJSXXB set KTSHZT='驳回',FGSKTSHZT='驳回',FGSKTSHRBH='" + User.Identity.Name + "',FGSKTSHTGSJ =GETDATE(),FGSKTSHXX='" + txtSHYJ.Text.ToString().Trim() + "' where JSBH='" + ViewState["JSBH"].ToString() + "' ";
                    }
                    strSqlGLB = "update ZZ_MMJYDLRZHGLB set FGSSHZT ='驳回',DLRSHZT='驳回',FGSSHSJ=GETDATE(),FGSKTSHRBH='" + User.Identity.Name + "',FGSSHYJ='" + txtSHYJ.Text.ToString().Trim() + "' where JSBH='" + ViewState["JSBH"].ToString() + "'  and DLRBH='" + ViewState["JJRBH"].ToString().Trim() + "'";
                    arrayList.Add(strSqlJSB);
                    arrayList.Add(strSqlGLB);
                    Hashtable rejectHashTable = new Hashtable();
                    rejectHashTable["登录邮箱"] = this.txtZCYX.Text.ToString().Trim();
                    rejectHashTable["用户名"] = this.txtYHM.Text.ToString().Trim();
                    rejectHashTable["手机号"] = this.txtSJH.Text.ToString();
                    rejectHashTable["驳回缘由"] = txtSHYJ.Text.ToString().Trim();
                    rejectHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");
                    SendVerifyRejectEmailPhone(rejectHashTable);
                }
                if (DbHelperSQL.ExecSqlTran(arrayList))
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核信息保存成功，同时向该用户邮箱和用户手机分别发送一封提醒邮件和一条提醒短信！');</script>");
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器异常，请联系管理员！');</script>");
                }
           
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核意见不能为空，请填写审核意见！');</script>");
            this.btnReject.Enabled = true;
            this.btnPass.Enabled = true;
        }
    }

    /// <summary>
    /// 返回列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("JHJX_SHMMJ.aspx");
    }

    /// <summary>
    /// 根据用户角色信息表来区分用户是第一次开通结算账户，还是新添加经纪人
    /// </summary>
    /// <returns>false:新添加经纪人;true:第一次开通结算账户</returns>
    protected bool GetSHZT()
    {
        string strSQL = "select * from ZZ_YHJSXXB where JSBH='" + ViewState["JSBH"].ToString().Trim() + "'";
        DataSet KTSHZT = DbHelperSQL.Query(strSQL);
        if (KTSHZT.Tables[0].Rows[0]["FGSKTSHZT"].ToString() == "审核通过" && KTSHZT.Tables[0].Rows[0]["KTSHZT"].ToString() == "审核通过")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}