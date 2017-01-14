using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using FMOP.DB;
using System.Configuration;
using System.Data;
using System.Net;
using System.Text;

public partial class Web_JHJX_New2013_JHJX_FGSJYZHSH_MMJ_Details : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.UCCityList1.initdefault();
            ViewState["JSBH"] = "";
            ViewState["DLYX"] = "";
            ViewState["YHM"] = "";
            ViewState["JYFMC"] = "";
            ViewState["GLJJRBH"] = "";
            ViewState["Number"] = "";
            if (Request["JSBH"] != null)
            {
                ViewState["JSBH"] = Request["JSBH"].ToString();
                BindData();
              
               
            }

        }
    }


    /// <summary>
    /// 绑定界面数据信息
    /// </summary>
    /// <param name="strJSBH"></param>
    /// <param name="strJSLX"></param>
    /// <param name="jjrbh"></param>
    protected void BindData()
    {
        string strSql = "select b.Number,B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',(case FGSSHZT when '审核中' then '--' when '驳回' then isnull(convert(varchar(10),a.ZHXGSJ,120),'--') else '' end) '最后修改时间'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + ViewState["JSBH"].ToString() + "' and a.SFYX='是'";

        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
        if (dataTable.Rows.Count > 0)
        {

            ViewState["DLYX"] = dataTable.Rows[0]["B_DLYX"].ToString();
            ViewState["YHM"] = dataTable.Rows[0]["B_YHM"].ToString();

            ViewState["Number"] = dataTable.Rows[0]["Number"].ToString();
            if (dataTable.Rows[0]["B_JSZHLX"].ToString() == "经纪人交易账户")
            {
                this.radJJR.Checked = true;
                this.radMMJ.Checked = false;
                this.trPTGLJG.Visible = true;
            }
            else
            {
                this.trPTGLJG.Visible = false;
                this.radJJR.Checked = false;
                this.radMMJ.Checked = true;
            }

            if (dataTable.Rows[0]["I_ZCLB"].ToString() == "单位")
            {
                this.radDW.Checked = true;
                this.radZRR.Checked = false;
            }
            else
            {
                this.radDW.Checked = false;
                this.radZRR.Checked = true;
            }

            this.txtJYFMC.Text = dataTable.Rows[0]["I_JYFMC"].ToString();

            ViewState["JYFMC"] = dataTable.Rows[0]["I_JYFMC"].ToString();

            this.txtYYZZZCH.Text = dataTable.Rows[0]["I_YYZZZCH"].ToString();
            this.linkYYZZ.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_YYZZSMJ"].ToString().Replace(@"\", "/");

            this.txtSFZH.Text = dataTable.Rows[0]["I_SFZH"].ToString();
            this.linkSFZH.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_SFZSMJ"].ToString().Replace(@"\", "/");
            this.linkSFZH_FM.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_SFZFMSMJ"].ToString().Replace(@"\", "/");

            this.txtZZJGDMZ.Text = dataTable.Rows[0]["I_ZZJGDMZDM"].ToString();
            this.linkZZJGDMZ.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_ZZJGDMZSMJ"].ToString().Replace(@"\", "/");


            this.txtSWDJZ.Text = dataTable.Rows[0]["I_SWDJZSH"].ToString();
            this.linkSWDJZ.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_SWDJZSMJ"].ToString().Replace(@"\", "/");


            this.linkYBNSRZGZMSMJ.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_YBNSRZGZSMJ"].ToString().Replace(@"\", "/");

            this.txtKHXKZH.Text = dataTable.Rows[0]["I_KHXKZH"].ToString();
            this.linkKHXKZH.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_KHXKZSMJ"].ToString().Replace(@"\", "/");

            this.linkYLYJK.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_YLYJK"].ToString().Replace(@"\", "/");

            this.txtFDDBRXM.Text = dataTable.Rows[0]["I_FDDBRXM"].ToString();

            this.txtFDDBRSHZHJSMJ.Text = dataTable.Rows[0]["I_FDDBRSFZH"].ToString();
            this.linkFDDBRSHZHJSM.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_FDDBRSFZSMJ"].ToString().Replace(@"\", "/");
            this.linkFDDBRSHZHJSM_FM.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_FDDBRSFZFMSMJ"].ToString().Replace(@"\", "/");

            this.linkFDDBRSQS.HRef = "../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dataTable.Rows[0]["I_FDDBRSQS"].ToString().Replace(@"\", "/");

            this.txtJYFLXDH.Text = dataTable.Rows[0]["I_JYFLXDH"].ToString();
            //所属区域
            //所属区域
            this.UCCityList1.SelectedItem = new string[] { dataTable.Rows[0]["I_SSQYS"].ToString(), dataTable.Rows[0]["I_SSQYSHI"].ToString(), dataTable.Rows[0]["I_SSQYQ"].ToString() };
            this.UCCityList1.EnabledItem = new bool[] { false, false, false };

            this.txtXXDZ.Text = dataTable.Rows[0]["I_XXDZ"].ToString();
            this.txtLXRXM.Text = dataTable.Rows[0]["I_LXRXM"].ToString();

            this.txtLXRSJH.Text = dataTable.Rows[0]["I_LXRSJH"].ToString();
            //this.txtKHYH.Text = dataTable.Rows[0]["I_KHYH"].ToString();
            this.txtYHZH.Text = dataTable.Rows[0]["I_YHZH"].ToString();

            this.ddlKHYH.Items.Add(dataTable.Rows[0]["I_KHYH"].ToString());
            this.ddlKHYH.Enabled = false;

            this.ddlPTGLJG.Items.Add(dataTable.Rows[0]["I_PTGLJG"].ToString());
            this.ddlPTGLJG.Enabled = false;

             ViewState["GLJJRBH"] = dataTable.Rows[0]["GLJJRBH"].ToString();


         DataTable dataTableTwo=DbHelperSQL.Query("select J_JJRZGZSBH,JJRZGZS,I_JYFMC,I_JYFLXDH from AAA_DLZHXXB  where J_JJRJSBH='" + ViewState["GLJJRBH"].ToString().Trim()+ "'").Tables[0];
         if (dataTableTwo != null && dataTableTwo.Rows.Count > 0)
         {
             this.txtGLJJRZGZSBH.Text = dataTableTwo.Rows[0]["J_JJRZGZSBH"].ToString();
             this.txtGLJJRMC.Text = dataTableTwo.Rows[0]["I_JYFMC"].ToString();
             this.txtGLJJRLXDH.Text = dataTableTwo.Rows[0]["I_JYFLXDH"].ToString();   
         }



            this.txtZLTJSJ.Text = dataTable.Rows[0]["资料提交时间"].ToString();

            this.txtFS_YJ.Text=dataTable.Rows[0]["FGSSHYJ"].ToString();
            this.hidSHYJ.Value = dataTable.Rows[0]["FGSSHYJ"].ToString();

            this.txtBHXGSJ.Text = dataTable.Rows[0]["最后修改时间"].ToString();

            //决定要显示的页面
            if (dataTable.Rows[0]["I_ZCLB"].ToString() == "单位")
            {
                this.trYYZZZCH.Visible = true;
                this.trSFZH.Visible = false;
                this.trZZJGDMZ.Visible = true;
                
                this.trSWDJZSH.Visible = true;
              //  this.trYBNSRZGZMSMJ.Visible = true;
                this.trKHXKZH.Visible = true;

                this.trYLYJK.Visible = true;

                this.trFDDBRXM.Visible = true;
                this.trFDDBRSHZHJSMJ.Visible = true;
                this.trFDDBRSQS.Visible = true;

            }
            else
            {
                this.trYYZZZCH.Visible = false;
                this.trSFZH.Visible = true;
                this.trZZJGDMZ.Visible = false;

                this.trSWDJZSH.Visible = false;
                this.trYBNSRZGZMSMJ.Visible = false;
                this.trKHXKZH.Visible = false;

                this.trYLYJK.Visible = false;

                this.trFDDBRXM.Visible = false;
                this.trFDDBRSHZHJSMJ.Visible = false;
                this.trFDDBRSQS.Visible = false;

            }




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
        //1、查询关联表中 买卖家账户的关联的默认经纪人
        //2、比较当前经纪人和当前买卖家账户的默认经纪人是不是同一个人 
            //2.1、如果不同 （更换了经纪人）
                //②更新关联表
                //③更新原来关联表原记录的 默认经纪人为“否”，当前关联的经纪人记录的 默认经纪人为“是”
            //2.2、如果相同  （首次注册关联的经纪人）
                //①更新登录表
                 //②更新关联表
        if (this.txtFS_YJ.Text.Trim().Contains("'"))
        {
            this.txtFS_YJ.Text = this.txtFS_YJ.Text.Trim().Replace("'", "‘");
        }
        if (this.txtFS_YJ.Text.Trim().Length > 1000)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('您输入的审核意见字符过长，请限制在1000个字符以内！');</script>");
            this.btnReject.Enabled = true;
            this.btnPass.Enabled = true;
            return;
        }
        if (this.hidSHYJ.Value.Trim().Contains("'"))
        {
            this.hidSHYJ.Value = this.hidSHYJ.Value.Trim().Replace("'", "‘");
        }

        if (IsGHZCLB())
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('当前交易账户已更换注册类别，请重新审核！', function () {  window.location.href=window.location.href;  });</script>");
            return;
        }
        if (IsFGSPassMMJ())
        {
            string url = "New2013/JHJX_FGSJYZHSH_MMJ.aspx";
            //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前买卖卖家交易账户已经被审核通过，不能再进行审核操作！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前交易方交易账户已经被审核通过，不能再进行审核操作！', function () {  __doPostBack('btnBack','');  });</script>");
            return;
        
        }

        if (ConfigurationManager.ConnectionStrings["onlineJHJX_YHJK"].ToString().Trim() == "开启银行接口")
        {
            #region//开启银行接口的调用方法

            //生成营业部编码
            ErpNumber erpNumber = new ErpNumber();
            #region 早期版本 
            /*
            ////向银行发送指令代码
            DataTable dt = BankServicePF.N26701_Deal_Init();
            dt.Rows[0]["ExSerial"] = ViewState["Number"].ToString();//外部流水号
            dt.Rows[0]["sBranchId"] = "0000";//营业部编码
            dt.Rows[0]["Name"] = this.txtJYFMC.Text.Trim().ToString();//客户姓名
            string strCustProperty = "";//客户性质
            string strCardType = "";//客户证件类型    
            string strCard = "";//客户证件号
            string strerpNumber = "";//证券资金账号
            dt.Rows[0]["OccurBala"] = "0";//客户姓名
            if (this.radZRR.Checked == true)//个人
            {
                strCustProperty = "0";
                strCardType = "1";//身份证
                strCard = this.txtSFZH.Text.Trim().ToString();
                strerpNumber = erpNumber.GetZQZJNumber("个人");
            }
            if (this.radDW.Checked == true)//机构
            {
                strCustProperty = "1";
                strCardType = "8";//组织机构代码
                strCard = this.txtZZJGDMZ.Text.Trim().ToString();
                strerpNumber = erpNumber.GetZQZJNumber("单位");
            }
            dt.Rows[0]["sCustAcct"] = strerpNumber;
            dt.Rows[0]["sCustProperty"] = strCustProperty;
            dt.Rows[0]["CardType"] = strCardType;
            dt.Rows[0]["Card"] = strCard;
            dt.Rows[0]["Date"] = DateTime.Now.ToString("yyyyMMdd");
            DataSet ds = BankServicePF.N26701_Deal_Push(dt);
            DataTable dtResult = ds.Tables["result"];
             *  */
            #endregion


            #region  wyh 2014.04.15 add 更换新的接口调用方式

            //初始化参数
            DataSet dspar = DsParsJieGou();
            dspar.Tables[0].Rows[0]["用户邮箱"] = ViewState["DLYX"].ToString();
            dspar.Tables[0].Rows[0]["客户姓名"] = this.txtJYFMC.Text.Trim().ToString();
            dspar.Tables[0].Rows[0]["业务流水号"] = ViewState["Number"].ToString();

            string strCard = "";//客户证件号
            string strerpNumber = "";//证券资金账号
            string Strzclb = "";

            if (this.radZRR.Checked == true)//个人
            {
                strCard = this.txtSFZH.Text.Trim().ToString();
                strerpNumber = erpNumber.GetZQZJNumber("个人");
                Strzclb = "radZRR"; //控件ID来区分注册类别
            }
            if (this.radDW.Checked == true)//机构
            {
                strCard = this.txtZZJGDMZ.Text.Trim().ToString();
                strerpNumber = erpNumber.GetZQZJNumber("单位");
                Strzclb = "radDW";
            }

            dspar.Tables[0].Rows[0]["客户证件号"] = strCard;
            dspar.Tables[0].Rows[0]["证券资金账号"] = strerpNumber;//ViewState["DLYX"].ToString();
            dspar.Tables[0].Rows[0]["注册类别"] = Strzclb;

            //调用webservcie 
          
            //CallPT2013Services.ws2013 WsQianYue = new CallPT2013Services.ws2013();
          //  DataTable dtResult = null; //WsQianYue.DsSendQianYue(dspar).Tables["返回值单条"];

            TestBank.bank bank = new TestBank.bank();   // wyh　２０１４．０８．２６　ａｄｄ
            DataTable dtResult = bank.DsSendQianYue(dspar).Tables["返回值单条"];

            #endregion dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "OK";



            if (dtResult == null || dtResult.Rows.Count <= 0)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('银行接口调用失败！');</script>");
                this.btnPass.Enabled = false;
                this.btnReject.Enabled = false;
                return;
            }
            if (dtResult.Rows[0]["执行结果"] == "ok")//(dtResult.Rows[0]["ErrorNo"].ToString() == "0000")//银行成功返回信息
            {
                #region//本地代码
                string strSqlMRJJR = "select a.GLJJRBH '关联经纪人编号' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + ViewState["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是'";

                object objJSBH = DbHelperSQL.GetSingle(strSqlMRJJR);
                object objName = GetEmployee_Name();
                string strSql1 = "";
                string strSql2 = "";
                string strSql3 = "";

                string strSHYJ = "";
                if (this.txtFS_YJ.Text.Trim().ToString().Equals(this.hidSHYJ.Value.Trim().ToString()))
                {
                    strSHYJ = "审核通过";
                }
                else
                {
                    strSHYJ = this.txtFS_YJ.Text.ToString();
                }
                ArrayList arrayList = new ArrayList();
              
                    strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='是',I_DSFCGZT='未开通',I_ZQZJZH='" + strerpNumber + "' where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH='" + ViewState["JSBH"].ToString() + "'";
                    strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set FGSSHZT='审核通过', FGSSHR='" + objName.ToString().Trim() + "',FGSSHSJ='" + DateTime.Now.ToString() + "',FGSSHYJ='" + strSHYJ + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + ViewState["DLYX"].ToString().Trim() + "' and  GLJJRBH='" + ViewState["GLJJRBH"].ToString() + "' and SFYX='是' ";
                    arrayList.Add(strSql1);
                    arrayList.Add(strSql2);
                

              

                if (DbHelperSQL.ExecSqlTran(arrayList))
                {
                    Hashtable ht = new Hashtable();
                    ht["type"] = "集合集合经销平台";
                    ht["提醒对象登陆邮箱"] = ViewState["DLYX"].ToString();
                    ht["提醒对象用户名"] = ViewState["JYFMC"].ToString();
                    ht["提醒对象结算账户类型"] = "买家卖家交易账户";
                    ht["提醒对象角色编号"] = ViewState["JSBH"].ToString();
                    ht["提醒对象角色类型"] = "卖家";
                    // ht["提醒内容文本"] = "尊敬的" + ViewState["JYFMC"].ToString() + "，您开通交易账户的申请已审核通过，可以进行相关业务操作！";
                    ht["提醒内容文本"] = "您" + strerpNumber + "交易方编号的交易账户已开通，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。";
                    ht["创建人"] = objName;
                    List<Hashtable> list = new List<Hashtable>();
                    list.Add(ht);
                    JHJX_SendRemindInfor.Sendmes(list);
                    string url = "New2013/JHJX_FGSJYZHSH_MMJ.aspx";
                    //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核信息提交成功，同时向该用户发送审核通过提醒！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核信息提交成功，同时向该用户发送审核通过提醒！', function () {   __doPostBack('btnBack',''); });</script>");
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器异常，请联系管理员！');</script>");
                    this.btnPass.Enabled = false;
                    this.btnReject.Enabled = false;
                }

                #endregion
            }
            else
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('" + dtResult.Rows[0]["ErrorInfo"].ToString() + "');</script>");
                this.btnPass.Enabled = false;
                this.btnReject.Enabled = false;

            }
            #endregion
        }
        else if (ConfigurationManager.ConnectionStrings["onlineJHJX_YHJK"].ToString().Trim() == "关闭银行接口")
        {
            #region//关闭银行接口的调用方法

            //生成营业部编码
            ErpNumber erpNumber = new ErpNumber();
            string strerpNumber = "";//证券资金账号
         
            if (this.radZRR.Checked == true)//个人
            {
                strerpNumber = erpNumber.GetZQZJNumber("个人");
            }
            if (this.radDW.Checked == true)//机构
            {
                strerpNumber = erpNumber.GetZQZJNumber("单位");
            }
                #region//本地代码
                string strSqlMRJJR = "select a.GLJJRBH '关联经纪人编号' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + ViewState["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是'";

                object objJSBH = DbHelperSQL.GetSingle(strSqlMRJJR);
                object objName = GetEmployee_Name();
                string strSql1 = "";
                string strSql2 = "";
                string strSql3 = "";

                string strSHYJ = "";
                if (this.txtFS_YJ.Text.Trim().ToString().Equals(this.hidSHYJ.Value.Trim().ToString()))
                {
                    strSHYJ = "审核通过";
                }
                else
                {
                    strSHYJ = this.txtFS_YJ.Text.ToString();
                }
                ArrayList arrayList = new ArrayList();
           
                   // strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='是',I_DSFCGZT='未开通',I_ZQZJZH='" + strerpNumber + "' where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH='" + ViewState["JSBH"].ToString() + "'";模拟运行期间 暂时删除 wyh 2014.08.26 
                strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='是',I_DSFCGZT='开通',I_ZQZJZH='" + strerpNumber + "'  ,I_YHZH='558212166545464546', I_JYFMC='测试开通'  where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH='" + ViewState["JSBH"].ToString() + "'"; //模拟运行期间专用 ｗｙｈ　２０１４．０８．２６　ａｄｄ

                    strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set FGSSHZT='审核通过', FGSSHR='" + objName.ToString().Trim() + "',FGSSHSJ='" + DateTime.Now.ToString() + "',FGSSHYJ='" + strSHYJ + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + ViewState["DLYX"].ToString().Trim() + "' and  GLJJRBH='" + ViewState["GLJJRBH"].ToString() + "' and SFYX='是' ";
                    arrayList.Add(strSql1);
                    arrayList.Add(strSql2);
                 

               


                if (DbHelperSQL.ExecSqlTran(arrayList))
                {
                    Hashtable ht = new Hashtable();
                    ht["type"] = "集合集合经销平台";
                    ht["提醒对象登陆邮箱"] = ViewState["DLYX"].ToString();
                    ht["提醒对象用户名"] = ViewState["JYFMC"].ToString();
                    ht["提醒对象结算账户类型"] = "买家卖家交易账户";
                    ht["提醒对象角色编号"] = ViewState["JSBH"].ToString();
                    ht["提醒对象角色类型"] = "卖家";
                    // ht["提醒内容文本"] = "尊敬的" + ViewState["JYFMC"].ToString() + "，您开通交易账户的申请已审核通过，可以进行相关业务操作！";
                    ht["提醒内容文本"] = "您" + strerpNumber + "交易方编号的交易账户已开通，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。";
                    ht["创建人"] = objName;
                    List<Hashtable> list = new List<Hashtable>();
                    list.Add(ht);
                    JHJX_SendRemindInfor.Sendmes(list);
                    string url = "New2013/JHJX_FGSJYZHSH_MMJ.aspx";
                    //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核信息提交成功，同时向该用户发送审核通过提醒！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核信息提交成功，同时向该用户发送审核通过提醒！', function () {  __doPostBack('btnBack','');  });</script>");
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器异常，请联系管理员！');</script>");
                    this.btnPass.Enabled = false;
                    this.btnReject.Enabled = false;
                }

                #endregion
            #endregion
        }

        

    }
    /// <summary>
    /// 驳回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReject_Click(object sender, EventArgs e)
    {
        this.btnReject.Enabled = false;
        this.btnPass.Enabled = false;
        if (!string.IsNullOrEmpty(this.txtFS_YJ.Text.Trim()))
        {
            if (this.txtFS_YJ.Text.Trim().Contains("'"))
            {
                this.txtFS_YJ.Text = this.txtFS_YJ.Text.Trim().Replace("'", "‘");
            }
            if (this.txtFS_YJ.Text.Trim().Length>1000)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('您输入的审核意见字符过长，请限制在1000个字符以内！');</script>");
                this.btnReject.Enabled = true;
                this.btnPass.Enabled = true;
                return;
            }

            if (IsGHZCLB())
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('当前交易账户已更换注册类别，请重新审核！', function () {  window.location.href=window.location.href;  });</script>");
                return;
            }
            if (IsFGSPassMMJ())
            {
                string url = "New2013/JHJX_FGSJYZHSH_MMJ.aspx";
                //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前买卖卖家交易账户已经被审核通过，不能再进行驳回操作！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前交易方交易账户已经被审核通过，不能再进行驳回操作！', function () {  __doPostBack('btnBack','');   });</script>");
                return;

            }


                string strSqlMRJJR = "select a.GLJJRBH '关联经纪人编号' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + ViewState["JSBH"].ToString() + "' and a.SFDQMRJJR='是'";

                object objJSBH = DbHelperSQL.GetSingle(strSqlMRJJR);
                object objName = GetEmployee_Name();
                string strSql1 = "";
                string strSql2 = "";
                ArrayList arrayList = new ArrayList();
                //if (ViewState["GLJJRBH"].ToString().Equals(objJSBH.ToString()))//第一次的经纪人
                //{
                    strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='否' where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH='" + ViewState["JSBH"].ToString() + "'";
                    strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set FGSSHZT='驳回',SFXG='否', FGSSHR='" + objName.ToString().Trim() + "',FGSSHSJ='" + DateTime.Now.ToString() + "',FGSSHYJ='" + this.txtFS_YJ.Text.Trim() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + ViewState["DLYX"].ToString().Trim() + "' and  GLJJRBH='" + ViewState["GLJJRBH"].ToString() + "'  and SFYX='是'";
                    arrayList.Add(strSql1);
                    arrayList.Add(strSql2); 
                //}
                //else //更换的经纪人
                //{
                //    strSql1 = "update AAA_MJMJJYZHYJJRZHGLB set FGSSHZT='驳回',SFXG='否', FGSSHR='" + objName.ToString().Trim() + "',FGSSHSJ='" + DateTime.Now.ToString() + "',FGSSHYJ='" + this.txtFS_YJ.Text.Trim() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + ViewState["DLYX"].ToString().Trim() + "' and  GLJJRBH='" + ViewState["GLJJRBH"].ToString() + "'  ";
                //    arrayList.Add(strSql1);
                //}

                //自身开户申请	[x1]被分公司审核时驳回	-0.5	减项	分公司审核时
                Hesion.Brick.Core.User us_ls = Hesion.Brick.Core.Users.GetUserByNumber(User.Identity.Name);
                object objNameValue = us_ls.Name.ToString();
                jhjx_JYFXYMX jhjxJYFXYMX = new jhjx_JYFXYMX();
                string[] strJYFXYMX = jhjxJYFXYMX.ZSKHSQ_BFGSSHSBH(ViewState["DLYX"].ToString(),"卖家", objNameValue.ToString());
                arrayList.AddRange(strJYFXYMX);
             object jjrDLYX= DbHelperSQL.GetSingle("select GLJJRDLZH from   AAA_MJMJJYZHYJJRZHGLB where DLYX='" + ViewState["DLYX"].ToString() + "' and SFDQMRJJR='是'");//得到关联经纪人的登录邮箱
             string[] strBFGSSHSBH = jhjxJYFXYMX.GLJYF_BFGSSHSBH(jjrDLYX.ToString(), ViewState["DLYX"].ToString(), "经纪人", objNameValue.ToString());
             arrayList.AddRange(strBFGSSHSBH);










                if (DbHelperSQL.ExecSqlTran(arrayList))
                {
                    Hashtable ht = new Hashtable();
                    ht["type"] = "集合集合经销平台";
                    ht["提醒对象登陆邮箱"] = ViewState["DLYX"].ToString();
                    ht["提醒对象用户名"] = ViewState["JYFMC"].ToString();
                    ht["提醒对象结算账户类型"] = "买家卖家交易账户";
                    ht["提醒对象角色编号"] = ViewState["JSBH"].ToString();
                    ht["提醒对象角色类型"] = "卖家";
                  //  ht["提醒内容文本"] = " 尊敬的" + ViewState["JYFMC"].ToString() + "，您提交的开通交易账户的资料未能通过平台管理机构的审核，请与您的平台管理机构联系！";
                    ht["提醒内容文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请。 ";
                    ht["创建人"] = objName;
                    List<Hashtable> list = new List<Hashtable>();
                    list.Add(ht);
                    JHJX_SendRemindInfor.Sendmes(list);





                    string url = "New2013/JHJX_FGSJYZHSH_MMJ_Details.aspx?SPURL=JHJX_FGSJYZHSH_MMJ.aspx";   
                    //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核信息保存成功，同时向该用户发送审核驳回提醒！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核信息保存成功，同时向该用户发送审核驳回提醒！', function () {  __doPostBack('btnBack','');   });</script>");
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('服务器异常，请联系管理员！');</script>");
                }

         
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('复审意见不能为空，请填写复审意见！');</script>");
            this.btnReject.Enabled = true;
            this.btnPass.Enabled = true;
        }
    }

    //根据登录账号得到登录用户名
    protected string GetEmployee_Name()
    {
        /*
        string url = ConfigurationManager.ConnectionStrings["GetBM"].ToString().Trim() + "/Web/GetBM/Handler.ashx?number=" + User.Identity.Name + "&method=GetEmployee_Name";
        WebClient httpclient = new WebClient();
        //byte[] bytes = httpclient.DownloadData(url);
        httpclient.Encoding = Encoding.UTF8;
        string Employee_Name = httpclient.DownloadString(url);
        return Employee_Name;
         */
        string sql_Employee_Name = "select Employee_Name from hr_employees where number='" + User.Identity.Name + "'";
        return DbHelperSQL.GetSingle(sql_Employee_Name).ToString();
    }

    /// <summary>
    /// 判断买卖家交易账户是否更换了注册类别
    /// </summary>
    /// <returns>false未更换注册类别，true已更换注册类别</returns>
    protected bool IsGHZCLB()
    {
        string strSql = "select B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),a.CreateTime,120) '资料提交时间'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + ViewState["JSBH"].ToString() + "' and a.SFYX='是'";
         //得到当前的注册类别
        string strZCLB = "";
        if (this.radDW.Checked == true)
        {
            strZCLB = "单位";
        }
        else
        {
            strZCLB = "自然人";
        }
     DataTable dataTable=DbHelperSQL.Query(strSql).Tables[0];
     if (dataTable.Rows[0]["I_ZCLB"].ToString() == strZCLB)
     {
         return false;
     }
     else
     {
         return true;
     }
    }


    /// <summary>
    /// 判断当前买卖家是否已经被分公司审核通过
    /// </summary>
    /// <param name="strMMJDLYX"></param>
    /// <param name="strGLJJRBH"></param>
    /// <returns></returns>
    public bool IsFGSPassMMJ()
    {
        string strSql = "select * from dbo.AAA_DLZHXXB where B_DLYX='" + ViewState["DLYX"].ToString() + "'  and S_SFYBFGSSHTG='是'";
        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }




    /// <summary>
    /// 返回列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("JHJX_FGSJYZHSH_MMJ.aspx");
    }

    /// <summary>
    /// 调用银行签约接口时的参数结构 wyh 2014.04.15 add
    /// </summary>
    /// <returns>参数结构体</returns>
    public DataSet DsParsJieGou()
    {
        DataSet ds = new DataSet();
        ds.Clear();
        DataTable auto2 = new DataTable();
        auto2.Columns.Add("用户邮箱");
        auto2.Columns.Add("业务流水号");
        auto2.Columns.Add("客户姓名");
        auto2.Columns.Add("证券资金账号");
        auto2.Columns.Add("注册类别");
        auto2.Columns.Add("客户证件号");
        auto2.Rows.Add(new string[] { "", "", "", "", "", "" });
        ds.Tables.Add(auto2);
        return ds;
    }

}