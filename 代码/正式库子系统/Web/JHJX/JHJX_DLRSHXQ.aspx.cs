using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System.Collections;
using System.Configuration;
using ShareLiu.DXS;

public partial class Web_JHJX_JHJX_DLRSHXQ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["JSBH"] = "";
          
            if (Request["JSBH"] != null)
            {
                ViewState["JSBH"] = Request["JSBH"].ToString();
            }
            if (Request["ck"] != null)
            {
                btnPass.Visible = false;
                btnReject.Visible = false;
            }
            BindOperationData(ViewState["JSBH"].ToString());
        }
    
    }

    //订单产品列表绑定，同时显示未发货量和当前办事处可用库存
    protected void BindOperationData(string strJSBH)
    {
        //DataSet dsCPLB = DbHelperSQL.Query("select id,XGLB as cplb,XGXH as cpxh,JG as jg,BHSL as sl,0 as yjsl,0 as wshsl from FGS_YZBBHDDJLB_XGXX where parentNumber='" + num + "' order by XGLB");
        string strYX = DbHelperSQL.GetSingle("select ZCLX from ZZ_DLRJBZLXXB where JSBH='" + strJSBH + "'").ToString();
        if (strYX == "个人注册")  //显示个人注册的信息
        {
            this.trGSMC.Visible = false;
            this.trGSDH.Visible = false;
            this.trGSDZ.Visible = false;
            this.trYYZZ.Visible = false;
            this.trZZJGDMZ.Visible = false;
            this.trSWDJZ.Visible = false;
            this.trKHXKZ.Visible = false;
            this.trKHXKZ.Visible = false;
            string strSql = "select ZZ_DLRJBZLXXB.DLYX,ZZ_DLRJBZLXXB.YHM,ZZ_YHJSXXB.KTSHXX,ZZ_YHJSXXB.FGSKTSHXX,ZZ_DLRJBZLXXB.JSBH,LXRXM,SJH,SFYZSJH,SJHYZM,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,ZCLX,SFZSMJ,GSMC,GSDH,GSDZ,YYZZSMJ,ZZJGDMZSMJ,SWDJZSMJ,KHXKZSMJ,FDDBRQZDCNSSMJ,YXKPLX,YXKPXX from ZZ_DLRJBZLXXB inner join ZZ_YHJSXXB on ZZ_DLRJBZLXXB.JSBH=ZZ_YHJSXXB.JSBH   where ZZ_DLRJBZLXXB.JSBH='" + strJSBH + "'";
         
            DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
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
                //txtQZCNS.Text = dataTable.Rows[0]["FDDBRQZDCNSSMJ"].ToString();
                if (IsCompanyHeaderPerson() == "分公司人员")//加载分公司审核信息
                {
                    txtSHYJ.Text = dataTable.Rows[0]["FGSKTSHXX"].ToString();
                }
                else //加载总部审核信息
                {
                    txtSHYJ.Text = dataTable.Rows[0]["KTSHXX"].ToString();
                }
            }
        }
        else if (strYX=="企业注册") //显示企业注册的信息
        {
            trSFZSMJ.Visible = false;
            this.trSFZH.Visible = false;
            this.trGSMC.Visible = true;
            this.trGSDH.Visible = true;
            this.txtGSDZ.Visible = true;
            this.txtYYZZ.Visible = true;
            this.trZZJGDMZ.Visible = true;
            this.txtSWDJZ.Visible = true;
            this.txtKHXKZ.Visible = true;
          
            string strSql = "select ZZ_DLRJBZLXXB.DLYX,ZZ_DLRJBZLXXB.YHM,ZZ_YHJSXXB.KTSHXX,ZZ_YHJSXXB.FGSKTSHXX,ZZ_DLRJBZLXXB.JSBH,LXRXM,SJH,SFYZSJH,SJHYZM,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,ZCLX,SFZSMJ,GSMC,GSDH,GSDZ,YYZZSMJ,ZZJGDMZSMJ,SWDJZSMJ,KHXKZSMJ,FDDBRQZDCNSSMJ,YXKPLX,YXKPXX from ZZ_DLRJBZLXXB inner join ZZ_YHJSXXB on ZZ_DLRJBZLXXB.JSBH=ZZ_YHJSXXB.JSBH   where ZZ_DLRJBZLXXB.JSBH='" + strJSBH + "'";
            DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
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
            //txtQZCNS.Text = dataTable.Rows[0]["FDDBRQZDCNSSMJ"].ToString();

            txtGSMC.Text = dataTable.Rows[0]["GSMC"].ToString();
            txtGSDH.Text = dataTable.Rows[0]["GSDH"].ToString();
            txtGSDZ.Text = dataTable.Rows[0]["GSDZ"].ToString();
            txtYYZZ.Text = dataTable.Rows[0]["YYZZSMJ"].ToString();
            txtZZJGDMZ.Text = dataTable.Rows[0]["ZZJGDMZSMJ"].ToString();
            txtSWDJZ.Text = dataTable.Rows[0]["SWDJZSMJ"].ToString();
            txtKHXKZ.Text = dataTable.Rows[0]["KHXKZSMJ"].ToString();
            if (IsCompanyHeaderPerson() == "分公司人员")//加载分公司审核信息
            {
                txtSHYJ.Text = dataTable.Rows[0]["FGSKTSHXX"].ToString();
            }
            else //加载总部审核信息
            {
                txtSHYJ.Text = dataTable.Rows[0]["KTSHXX"].ToString();
            }
        }
        BingLinkData();
    }
    //绑定link标签上的数据
    protected void BingLinkData()
    {

        this.linkSFZSMJ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtSFZSMJ.Text.Trim().Replace(@"\", "/");
        //this.linkQZCNS.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtQZCNS.Text.Trim().Replace(@"\", "/");
        this.linkYYZZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtYYZZ.Text.Trim().Replace(@"\", "/");
        this.linkZZJGDMZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtZZJGDMZ.Text.Trim().Replace(@"\", "/");
        this.linkSWDJZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtSWDJZ.Text.Trim().Replace(@"\", "/");
        this.linkKHXKZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString() + "/JHJXPT/SaveDir/" + this.txtKHXKZ.Text.Trim().Replace(@"\", "/");

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



        if (this.txtSHYJ.Text.Trim().Contains("'"))
        {
            this.txtSHYJ.Text = this.txtSHYJ.Text.Trim().Replace("'", "‘");
        }


        string strSql = "update ZZ_YHJSXXB set FGSKTSHZT='审核通过',FGSKTSHRBH='" + User.Identity.Name + "',FGSKTSHTGSJ =GETDATE(),FGSKTSHXX='" + txtSHYJ.Text + "' where JSBH='" + ViewState["JSBH"].ToString() + "' ";

        IfPassInsertTableData(strSql);
        VerifySuccessSendEmail();
        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核信息提交成功，同时向该用户邮箱和用户手机分别发送一封提醒邮件和一条提醒短信！');</script>");

       

    }
    #region //审核失败成功后发送的电子邮件
    /// <summary>
    /// 审核成功后发送的邮件
    /// </summary>
    /// <param name="hashTable"></param>
    protected string VerifySuccessSendEmail()
    {
        try
        {
            //发送电子邮件
            Hashtable buyerHashTable = new Hashtable();
            buyerHashTable["登录邮箱"] = txtZCYX.Text.ToString().Trim();
            buyerHashTable["用户名"] = txtYHM.Text.ToString().Trim();
            buyerHashTable["结算账户"] = "经纪人账户和买家结算账户";
            buyerHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");
            buyerHashTable["手机号"] = txtSJH.Text.ToString();
            ClassEmail CE = new ClassEmail();
            string strSuject = "中国商品批发交易平台-资料审核";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/Web/JHJX/JSZHVerifySuccessEmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[yhm]]"] = buyerHashTable["用户名"];
            ht["[[jszh]]"] = buyerHashTable["结算账户"];
            ht["[[DateTime]]"] = buyerHashTable["发送时间"];
            CE.SendSPEmail(buyerHashTable["登录邮箱"].ToString(), strSuject, modHtmlFileName, ht);

            string StrMess = "您的资料已经通过审核，可正常使用" + buyerHashTable["结算账户"].ToString() + "。[中国商品批发交易平台]";
            object[] ob = { buyerHashTable["手机号"].ToString(), StrMess };
            object return_ob = WebServiceHelper.InvokeWebService("http://192.168.0.10:8183/MessageService.asmx", "SendMessage", ob);
            //发送短信
            //string teest = (dlist.Items[i].FindControl("labGonghai") as Label).Text.Trim();
            //sendPhone.SendToP(teest, StrMess);
            //Response.Write((dlist.Items[i].FindControl("labGonghai") as Label).Text.Trim());
          
         
            return "发送成功";
        }
        catch (Exception exec)
        {
            return "发送失败";
        
        }
    }

    /// <summary>
    /// 审核失败后，发送的邮件
    /// </summary>
    /// <param name="hashTable"></param>
    protected string VerifyFailedSendEmail()
    {
        try
        {
            //发送电子邮件
            Hashtable buyerHashTable = new Hashtable();
            buyerHashTable["登录邮箱"] = txtZCYX.Text.ToString().Trim();
            buyerHashTable["用户名"] = txtYHM.Text.ToString().Trim();
            buyerHashTable["驳回缘由"] = txtSHYJ.Text.ToString();
            buyerHashTable["发送时间"] = DateTime.Now.ToString("yyyy-MM-dd");

            ClassEmail CE = new ClassEmail();
            string strSuject = "中国商品批发交易平台-资料审核";
            string modHtmlFileName = HttpContext.Current.Server.MapPath("~/Web/JHJX/JSZHVerifyFailedEmailTemplate.htm");
            Hashtable ht = new Hashtable();
            ht["[[yhm]]"] = buyerHashTable["用户名"];
            ht["[[reason]]"] = buyerHashTable["驳回缘由"];
            ht["[[DateTime]]"] = buyerHashTable["发送时间"];
            CE.SendSPEmail(buyerHashTable["登录邮箱"].ToString(), strSuject, modHtmlFileName, ht);

            string StrMess = "[" + buyerHashTable["用户名"].ToString() + "]的资料分公司审核失败，请修改后重新提交。[中国商品批发交易平台]";
            object[] ob = { buyerHashTable["手机号"].ToString(), StrMess };
            object return_ob = WebServiceHelper.InvokeWebService("http://192.168.0.10:8183/MessageService.asmx", "SendMessage", ob);
            //发送短信
            return "邮件发送成功";
        }
        catch (Exception exec)
        {
            return "邮件发送失败";

        }
    
    }
    #endregion

    /// <summary>
    ///当总部或者分公司是最后一个审核通过的时候，调用次方法
    /// </summary>
    protected void IfPassInsertTableData(string strSqlSql)
    {
        //查询代理人详细信息
        DataTable dataTable = DbHelperSQL.Query("select DLR.DLYX,DLR.YHM,JS.JSZHLX,DLR.JSBH,JS.JSLX,DLR.GSMC,DLR.LXRXM,DLR.SSSF,DLR.SSDS,DLR.SSQX,DLR.SSFGS,DLR.SFZH,DLR.SFZSMJ,DLR.XXDZ,DLR.YZBM,DLR.SJH from  ZZ_DLRJBZLXXB as DLR inner join ZZ_YHJSXXB as JS on DLR.JSBH=JS.JSBH where DLR.JSBH='" + ViewState["JSBH"].ToString() + "'").Tables[0];

        //WorkFlowModule WFPKNumber = new WorkFlowModule("ZZ_SHDZXXB");
        //string JFXXBNumber = WFPKNumber.numberFormat.GetNextNumber();//生成主键
        string JFXXBNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_SHDZXXB", "");
        //根据角色信息表生成角色编号 
        string strBuyerRole = jhjx_PublicClass.GetNextNumberZZ("ZZ_YHJSXXB", "C");

        //向地址信息表中插入，经纪人收获地址信息
        string sqlSql1 = " insert ZZ_SHDZXXB(Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX,SHDWMC,SHRXM,SZSF,SZDS,SZQX,XXDZ,YZBM,LXDH,SFMRDZ,CheckState,CreateUser,CreateTime,CheckLimitTime) values('" + JFXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','" + dataTable.Rows[0]["JSZHLX"].ToString() + "','" + strBuyerRole + "','买家','" + dataTable.Rows[0]["GSMC"].ToString() + "','" + dataTable.Rows[0]["LXRXM"].ToString() + "','" + dataTable.Rows[0]["SSSF"].ToString() + "','" + dataTable.Rows[0]["SSDS"].ToString() + "','" + dataTable.Rows[0]["SSQX"].ToString() + "','" + dataTable.Rows[0]["XXDZ"].ToString() + "','" + dataTable.Rows[0]["YZBM"].ToString() + "','" + dataTable.Rows[0]["SJH"].ToString() + "','是','1','" + User.Identity.Name + "',GETDATE(),GETDATE())";

        //向买家基本信息表中插入数据
        //WorkFlowModule BUYJBZLXXBNumber = new WorkFlowModule("ZZ_BUYJBZLXXB");
        //string strBUYJBZLXXBNumber = BUYJBZLXXBNumber.numberFormat.GetNextNumber();//生成主键
        string strBUYJBZLXXBNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_BUYJBZLXXB", "");
        string sqlMJJBJSXXB = "insert ZZ_BUYJBZLXXB(Number,DLYX,YHM,JSZHLX,JSBH,LXRXM,SJH,SFYZSJH,SSSF,SSDS,SSQX,SSFGS,XXDZ,YZBM,SFZH,SFZSMJ,CheckState,CreateUser,CreateTime,CheckLimitTime) values('" + strBUYJBZLXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','经纪人账户','" + strBuyerRole + "','" + dataTable.Rows[0]["LXRXM"].ToString() + "','" + dataTable.Rows[0]["SJH"].ToString() + "','否','" + dataTable.Rows[0]["SSSF"].ToString() + "','" + dataTable.Rows[0]["SSDS"].ToString() + "','" + dataTable.Rows[0]["SSQX"].ToString() + "','" + dataTable.Rows[0]["SSFGS"].ToString() + "','" + dataTable.Rows[0]["XXDZ"].ToString() + "','" + dataTable.Rows[0]["YZBM"].ToString() + "','" + dataTable.Rows[0]["SFZH"].ToString() + "','" + dataTable.Rows[0]["SFZSMJ"].ToString() + "','1','" + User.Identity.Name + "',GETDATE(),GETDATE())";
        //WorkFlowModule YHJSXXBNumber = new WorkFlowModule("ZZ_YHJSXXB");
        //string strYHJSXXBNumber = YHJSXXBNumber.numberFormat.GetNextNumber();//生成主键
        string strYHJSXXBNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_YHJSXXB", "");
        //向角色信息表中插入数据
        string strJSXXB = "insert ZZ_YHJSXXB(Number,DLYX,YHM,JSZHLX,JSLX,JSBH,SQKTSJ,KTSHZT,FGSKTSHZT,KTSHRLX,KTSHRBH,FGSKTSHRBH,KTSHTGSJ,FGSKTSHTGSJ,KTSHXX,FGSKTSHXX,DLRZTJSXYH,CheckState,CreateUser,CreateTime,CheckLimitTime)  values('" + strYHJSXXBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','经纪人账户','买家','" + strBuyerRole + "',GETDATE(),'审核通过','审核通过','经纪人','" + dataTable.Rows[0]["JSBH"].ToString() + "','"+User.Identity.Name+"',GETDATE(),'','','','否','1','" + User.Identity.Name + "',GETDATE(),GETDATE())";
        //WorkFlowModule MMJYDLRZHGLBNumber = new WorkFlowModule("ZZ_MMJYDLRZHGLB");
        //string strMMJYDLRZHGLBNumber = MMJYDLRZHGLBNumber.numberFormat.GetNextNumber();//生成主键
        string strMMJYDLRZHGLBNumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_MMJYDLRZHGLB", "");
        //向关联表中插入数据
        string strGLB = "insert ZZ_MMJYDLRZHGLB(Number,JSDLZH,JSYHM,JSZHLX,JSBH,JSLX,DLRDLZH,DLRBH,DLRYHM,SFMRDLR,SQGLSJ,DLRSHZT,DLRSHSJ,DLRSHYJ,GLZT,SFZTXYW,CheckState,CreateUser,CreateTime,CheckLimitTime,FGSSHZT,FGSSHSJ,FGSKTSHRBH,FGSSHYJ)  values('" + strMMJYDLRZHGLBNumber + "','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','经纪人账户','" + strBuyerRole + "','买家','" + dataTable.Rows[0]["DLYX"].ToString() + "','" + dataTable.Rows[0]["JSBH"].ToString() + "','" + dataTable.Rows[0]["YHM"].ToString() + "','是',GETDATE(),'已审核',GETDATE(),'','有效','否','1','" + User.Identity.Name + "',GETDATE(),GETDATE(),'已审核',GETDATE(),'" + User.Identity.Name + "','" + this.txtSHYJ.Text.ToString().Trim() + "')";

        ArrayList arrayListSql = new ArrayList();
        arrayListSql.Add(strSqlSql);
        arrayListSql.Add(sqlSql1);
        arrayListSql.Add(sqlMJJBJSXXB);
        arrayListSql.Add(strJSXXB);
        arrayListSql.Add(strGLB);
        //执行多条Sql语句

        DbHelperSQL.ExecuteSqlTran(arrayListSql);
    }

    /// <summary>
    /// 判断当前用户是总户人员还是分公司人员
    /// </summary>
    /// <returns></returns>
    public string IsCompanyHeaderPerson()
    { 
        
     string QX = "SELECT  BM from HR_EMPLOYEES WHERE NUMBER ='" + User.Identity.Name + "'";//判断权限
        //当前用户为分公司人员
     if ((DbHelperSQL.GetSingle(QX).ToString().IndexOf("办事处") > 0) && User.Identity.Name != "admin")
     {
         return "分公司人员";
     }
     else
     {
         return "总部人员";
     }
    }
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

            string strSql = "update ZZ_YHJSXXB set FGSKTSHZT='驳回',FGSKTSHRBH='" + User.Identity.Name + "',FGSKTSHTGSJ ='" + DateTime.Now.ToString() + "',FGSKTSHXX='" + txtSHYJ.Text + "' where JSBH='" + ViewState["JSBH"].ToString() + "' ";
            if (DbHelperSQL.ExecuteSql(strSql) > 0)
            {
                VerifyFailedSendEmail();
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
        Response.Redirect("JHJX_SHDLR.aspx");
    }
}