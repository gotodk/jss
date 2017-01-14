using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Xml;
using Aspose.Words;
using Aspose.Words.Saving;
using System.Drawing;
using System.Net;
using System.Text;

public partial class Web_JHJX_New2013_JHJX_FGSJYZHSH_JJR_Details : System.Web.UI.Page
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
            ViewState["Number"] = "";
            if (Request["JSBH"] != null)
            {
                ViewState["JSBH"] = Request["JSBH"].ToString();
             
            }
            BindData();
        
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
        string strSql = "select b.Number,B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',(case FGSSHZT when '审核中' then '--' else isnull(CONVERT(varchar(10), a.ZHXGSJ,120),'--') end) '驳回修改时间'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.GLJJRBH=b.J_JJRJSBH   where b.B_JSZHLX='经纪人交易账户'  and a.SFYX='是' and b.J_JJRJSBH='" + ViewState["JSBH"].ToString() + "'";

   DataTable dataTable=DbHelperSQL.Query(strSql).Tables[0];
   if (dataTable.Rows.Count > 0)
   {

       ViewState["DLYX"] = dataTable.Rows[0]["B_DLYX"].ToString();
       ViewState["YHM"] = dataTable.Rows[0]["B_YHM"].ToString();

       ViewState["Number"] = dataTable.Rows[0]["Number"].ToString();


       if(dataTable.Rows[0]["B_JSZHLX"].ToString()=="经纪人交易账户")
       {
       this.radJJR.Checked=true;
       this.radMMJ.Checked = false;
       }
       else
       {
           this.radJJR.Checked = false;
       this.radMMJ.Checked=true;
       }

       if (dataTable.Rows[0]["I_ZCLB"].ToString()=="单位")
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
       this.UCCityList1.SelectedItem = new string[] { dataTable.Rows[0]["I_SSQYS"].ToString(), dataTable.Rows[0]["I_SSQYSHI"].ToString(), dataTable.Rows[0]["I_SSQYQ"].ToString() };
       this.UCCityList1.EnabledItem = new bool[] {false,false,false };
       this.txtXXDZ.Text = dataTable.Rows[0]["I_XXDZ"].ToString();   
       this.txtLXRXM.Text = dataTable.Rows[0]["I_LXRXM"].ToString();   

       this.txtLXRSJH.Text = dataTable.Rows[0]["I_LXRSJH"].ToString();
       //this.txtKHYH.Text = dataTable.Rows[0]["I_KHYH"].ToString();
       this.txtYHZH.Text = dataTable.Rows[0]["I_YHZH"].ToString();

       this.ddlKHYH.Items.Add(dataTable.Rows[0]["I_KHYH"].ToString());
       this.ddlKHYH.Enabled = false;

       this.ddlPTGLJG.Items.Add(dataTable.Rows[0]["I_PTGLJG"].ToString());
       this.ddlPTGLJG.Enabled = false;

       this.txtZLTJSJ.Text = dataTable.Rows[0]["资料提交时间"].ToString();

     
       this.txtCS_SHYJ.Text = dataTable.Rows[0]["FGSSHYJ"].ToString();
       this.hidSHYJ.Value = dataTable.Rows[0]["FGSSHYJ"].ToString();
       this.txtBHXGSJ.Text = dataTable.Rows[0]["驳回修改时间"].ToString();


       //决定要显示的页面
       if (dataTable.Rows[0]["I_ZCLB"].ToString() == "单位")
       {
           this.trYYZZZCH.Visible = true;
           this.trSFZH.Visible = false;
           this.trZZJGDMZ.Visible = true;

           this.trSWDJZSH.Visible = true;
           //this.trYBNSRZGZMSMJ.Visible = true;
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

        if (this.txtCS_SHYJ.Text.Trim().Contains("'"))
        {
            this.txtCS_SHYJ.Text = this.txtCS_SHYJ.Text.Trim().Replace("'", "‘");
        }
        if (this.txtCS_SHYJ.Text.Trim().Length > 1000)
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


        if (IsJJRGHGLJG())
        {
            string url = "New2013/JHJX_FGSJYZHSH_JJR.aspx";
            //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前交易账户已更换所属服务机构！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前交易账户已更换业务管理部门，无需审核！', function () {  __doPostBack('btnBack','');   });</script>");
            return;
        }
        else if (IsGHZCLB())
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('当前交易账户已更换注册类别，请重新审核！', function () {  window.location.href=window.location.href;  });</script>");
            return; 
        }
        else if (IsFGSPassJJR())
        {
            string url = "New2013/JHJX_FGSJYZHSH_JJR.aspx";
            //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前经纪人交易账户已经被审核通过，不能再进行审核操作！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前经纪人交易账户已经被审核通过，不能再进行审核操作！', function () {   __doPostBack('btnBack','');  });</script>");
            return;
            
            }
        else
        {
            if (ConfigurationManager.ConnectionStrings["onlineJHJX_YHJK"].ToString().Trim()=="开启银行接口")
            {
                #region//开启银行接口调用的方法

                // //生成营业部编码
                ErpNumber erpNumber = new ErpNumber();
                #region
                /*
                //////向银行发送指令代码
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
                  * */
              
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

                //调用webservcie 
              
                
               // CallPT2013Services.ws2013 WsQianYue = new CallPT2013Services.ws2013();
                //DataTable dtResult = WsQianYue.DsSendQianYue(dspar).Tables["返回值单条"]; 
                TestBank.bank bank = new TestBank.bank();   // wyh　２０１４．０８．２６　ａｄｄ
                DataTable dtResult = bank.DsSendQianYue(dspar).Tables["返回值单条"];

                #endregion dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "OK";

                if (dtResult.Rows.Count <= 0)
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('银行接口调用失败！');</script>");
                    this.btnPass.Enabled = false;
                    this.btnReject.Enabled = false;
                    return;

                }

                if (dtResult.Rows[0]["执行结果"].ToString() == "ok")//银行成功返回信息
                {
                    #region //本地代码
                    DateTime dateTimeStart = DateTime.Now;
                    DateTime dateTimeEnd = dateTimeStart.AddYears(2);

                    string strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='是', J_JJRZGZSYXQKSSJ='" + dateTimeStart.ToString() + "', J_JJRZGZSYXQJSSJ='" + dateTimeEnd.ToString() + "',I_DSFCGZT='未开通',I_ZQZJZH='" + strerpNumber + "' where B_JSZHLX='经纪人交易账户'   and  J_JJRJSBH='" + ViewState["JSBH"].ToString() + "' ";
                    object objName = GetEmployee_Name();
                    string strSHYJ = "";
                    if (this.txtCS_SHYJ.Text.Trim().ToString().Equals(this.hidSHYJ.Value.Trim().ToString()))
                    {
                        strSHYJ = "审核通过";
                    }
                    else
                    {
                        strSHYJ = this.txtCS_SHYJ.Text.ToString();
                    }
                    string strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set FGSSHZT='审核通过', FGSSHR='" + objName.ToString().Trim() + "',FGSSHSJ='" + DateTime.Now.ToString() + "',FGSSHYJ='" + strSHYJ + "'    where JSZHLX='经纪人交易账户' and  GLJJRBH='" + ViewState["JSBH"].ToString() + "' and SFYX='是'  ";
                  

                    ArrayList arrayList = new ArrayList();
                    arrayList.Add(strSql1);
                    arrayList.Add(strSql2);//发送提醒  尚未做
                    //arrayList.Add(strSql3);
                    if (DbHelperSQL.ExecSqlTran(arrayList))
                    {
                        Hashtable ht = new Hashtable();
                        ht["type"] = "集合集合经销平台";
                        ht["提醒对象登陆邮箱"] = ViewState["DLYX"].ToString();
                        ht["提醒对象用户名"] = ViewState["JYFMC"].ToString();
                        ht["提醒对象结算账户类型"] = "经纪人交易账户";
                        ht["提醒对象角色编号"] = ViewState["JSBH"].ToString();
                        ht["提醒对象角色类型"] = "经纪人";
                        //ht["提醒内容文本"] = "尊敬的" + ViewState["JYFMC"].ToString() + "，您开通交易账户的申请已审核通过，可以进行相关业务操作！";
                        ht["提醒内容文本"] = "您" + strerpNumber + "交易方编号的交易账户已开通，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。";
                        ht["创建人"] = objName;
                        List<Hashtable> list = new List<Hashtable>();
                        list.Add(ht);
                        JHJX_SendRemindInfor.Sendmes(list);
                        string url = "New2013/JHJX_FGSJYZHSH_JJR.aspx";
                        //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核信息提交成功，同时向该用户发送审核通过提醒！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核信息提交成功，同时向该用户发送审核通过提醒！', function () {   __doPostBack('btnBack','');  });</script>");
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
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('" + dtResult.Rows[0]["提示文本"].ToString() + "');</script>");
                    this.btnPass.Enabled = false;
                    this.btnReject.Enabled = false;

                }
                #endregion
            }
            else if (ConfigurationManager.ConnectionStrings["onlineJHJX_YHJK"].ToString().Trim() == "关闭银行接口")
            {
                #region//关闭银行接口调用的方法

                // //生成营业部编码
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
                    #region //本地代码
                    DateTime dateTimeStart = DateTime.Now;
                    DateTime dateTimeEnd = dateTimeStart.AddYears(2);

                   // string strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='是', J_JJRZGZSYXQKSSJ='" + dateTimeStart.ToString() + "', J_JJRZGZSYXQJSSJ='" + dateTimeEnd.ToString() + "',I_DSFCGZT='未开通',I_ZQZJZH='" + strerpNumber + "' where B_JSZHLX='经纪人交易账户'   and  J_JJRJSBH='" + ViewState["JSBH"].ToString() + "' "; 模拟运行期间暂时去掉 wyh 2014.08.26 
                    string strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='是', J_JJRZGZSYXQKSSJ='" + dateTimeStart.ToString() + "', J_JJRZGZSYXQJSSJ='" + dateTimeEnd.ToString() + "',I_DSFCGZT='开通',I_ZQZJZH='" + strerpNumber + "',I_YHZH='558212166545464546', I_JYFMC='测试开通'+I_JYFMC where B_JSZHLX='经纪人交易账户'   and  J_JJRJSBH='" + ViewState["JSBH"].ToString() + "' ";
                    object objName = GetEmployee_Name();
                    string strSHYJ = "";
                    if (this.txtCS_SHYJ.Text.Trim().ToString().Equals(this.hidSHYJ.Value.Trim().ToString()))
                    {
                        strSHYJ = "审核通过";
                    }
                    else
                    {
                        strSHYJ = this.txtCS_SHYJ.Text.ToString();
                    }
                    string strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set FGSSHZT='审核通过', FGSSHR='" + objName.ToString().Trim() + "',FGSSHSJ='" + DateTime.Now.ToString() + "',FGSSHYJ='" + strSHYJ + "'    where JSZHLX='经纪人交易账户' and  GLJJRBH='" + ViewState["JSBH"].ToString() + "' and SFYX='是'  ";
                 

                    ArrayList arrayList = new ArrayList();
                    arrayList.Add(strSql1);
                    arrayList.Add(strSql2);//发送提醒  尚未做
               
                    if (DbHelperSQL.ExecSqlTran(arrayList))
                    {
                        Hashtable ht = new Hashtable();
                        ht["type"] = "集合集合经销平台";
                        ht["提醒对象登陆邮箱"] = ViewState["DLYX"].ToString();
                        ht["提醒对象用户名"] = ViewState["JYFMC"].ToString();
                        ht["提醒对象结算账户类型"] = "经纪人交易账户";
                        ht["提醒对象角色编号"] = ViewState["JSBH"].ToString();
                        ht["提醒对象角色类型"] = "经纪人";
                        //ht["提醒内容文本"] = "尊敬的" + ViewState["JYFMC"].ToString() + "，您开通交易账户的申请已审核通过，可以进行相关业务操作！";
                        ht["提醒内容文本"] = "您" + strerpNumber + "交易方编号的交易账户已开通，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。";
                        ht["创建人"] = objName;
                        List<Hashtable> list = new List<Hashtable>();
                        list.Add(ht);
                        JHJX_SendRemindInfor.Sendmes(list);
                        string url = "New2013/JHJX_FGSJYZHSH_JJR.aspx";
                        //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核信息提交成功，同时向该用户发送审核通过提醒！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('审核信息提交成功，同时向该用户发送审核通过提醒！', function () {  __doPostBack('btnBack','');   });</script>");
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
    /// 预指定签约
    /// </summary>
    /// <returns></returns>
    public void Contract()
    {
        ErpNumber erpNumber = new ErpNumber();
        string invokeState = string.Empty;
        IBank IB = new IBank(ViewState["DLYX"].ToString(), "签约");
        DataSet ds = initReturnDataSet();
        ds.Tables[0].Rows[0]["外部流水号"] = ViewState["Number"].ToString();//外部流水号
        ds.Tables[0].Rows[0]["营业部编码"] = "0000";//营业部编码
        ds.Tables[0].Rows[0]["客户姓名"] = this.txtJYFMC.Text.Trim().ToString();//客户姓名
        string strCustProperty = "";//客户性质
        string strCardType = "";//客户证件类型    
        string strCard = "";//客户证件号
        string strerpNumber = "";//证券资金账号
        ds.Tables[0].Rows[0]["证券资金余额"] = "0";//证券资金余额
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
        ds.Tables[0].Rows[0]["证券资金账号"] = strerpNumber;
        ds.Tables[0].Rows[0]["客户性质"] = strCustProperty;
        ds.Tables[0].Rows[0]["客户证件类型"] = strCardType;
        ds.Tables[0].Rows[0]["客户证件号"] = strCard;
        ds.Tables[0].Rows[0]["交易日期"] = DateTime.Now.ToString("yyyyMMdd");
        DataSet dsResult = IB.Invoke(ds, ref invokeState);
    }


    /// <summary>
    /// 初始化返回值数据集,客户存管签约确认
    /// </summary>
    /// <returns></returns>
    public DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "浦发银行-客户存管签约";
        auto2.Columns.Add("外部流水号");
        auto2.Columns.Add("营业部编码");
        auto2.Columns.Add("客户姓名");
        auto2.Columns.Add("客户性质");
        auto2.Columns.Add("客户证件类型");
        auto2.Columns.Add("客户证件号");
        auto2.Columns.Add("证券资金账号");
        auto2.Columns.Add("证券资金余额");
        auto2.Columns.Add("交易日期");
        ds.Tables.Add(auto2);
        return ds;
    }




    /// <summary>
    /// 生成经纪人资格证书编号
    /// </summary>
    protected string CreateJJRZGZSBH()
    {

        DataTable dt_UserInfo = DbHelperSQL.Query("select J_JJRZGZSBH '经纪人资格证书编号',I_JYFMC '交易方名称',JJRZGZS '经纪人资格证书',* from AAA_DLZHXXB where B_DLYX='" + ViewState["DLYX"].ToString() + "'").Tables[0];//获取用户信息，
        string strJJRZGZSBH = dt_UserInfo.Rows[0]["经纪人资格证书编号"].ToString();//经纪人资格证书编号
        string strJYFMC = dt_UserInfo.Rows[0]["交易方名称"].ToString();//交易方名称
        string strJJRZGZS = dt_UserInfo.Rows[0]["交易方名称"].ToString();//经纪人资格证书
        //如果已有经纪人资格证书
        //if (!String.IsNullOrEmpty(strJJRZGZS))
        //{
         if(true)
         {

            DateTime ZSYXQ_QS = DateTime.Now; //得到当前时间
            string year_QS = ZSYXQ_QS.Year.ToString();//年份
            string month_QS = ZSYXQ_QS.Month.ToString();//月份
            string day_QS = ZSYXQ_QS.Day.ToString();//日期
            DateTime ZSYXQ_ZZ = ZSYXQ_QS.AddYears(2);//有限期截止时间，在当前有效期在延后两年
            string year_ZZ = ZSYXQ_ZZ.Year.ToString();//年份
            string month_ZZ = ZSYXQ_ZZ.Month.ToString();//月份
            string day_ZZ = ZSYXQ_ZZ.Day.ToString();//日期


            string ResourcePath = Server.MapPath("JJRZGZS_Path/JJRZGZS_Initial.xml");//经纪人资格证书模板服务器路径
            string FileName = Guid.NewGuid().ToString();
            string Paths = Server.MapPath("JJRZGZS_NewPath/") + FileName + ".xml";//拷贝后的文件目录、
            string NewPaths = Server.MapPath("JJRZGZS_NewPath/COPY/") + FileName + ".xml";//保存后的目录
            string SavingPath = "~/Web/JHJX/New2013/JJRZGZS_Temp/" + FileName + ".png";//存入路径
            if (File.Exists(ResourcePath))
            {
                File.Copy(ResourcePath, Paths, true);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Paths);//载入此XML
                XmlNodeList xnl = xmlDoc.GetElementsByTagName("w:t");//office xml word的Tag
                foreach (XmlNode xn in xnl)
                {
                    if (xn.InnerXml == "PT_BH")
                    {
                        xn.InnerText = strJJRZGZSBH;
                    }
                    if (xn.InnerXml == "JYFMC")
                    {
                        xn.InnerXml = strJYFMC;
                    }
                    if (xn.InnerXml == "YYYY_QS")
                    {
                        xn.InnerXml = year_QS;
                    }
                    if (xn.InnerXml == "MM_QS")
                    {
                        xn.InnerXml = month_QS;
                    }
                    if (xn.InnerXml == "dd_QS")
                    {
                        xn.InnerXml = day_QS;
                    }
                    if (xn.InnerXml == "YYYY_ZZ")
                    {
                        xn.InnerXml = year_ZZ;
                    }
                    if (xn.InnerXml == "MM_ZZ")
                    {
                        xn.InnerXml = month_ZZ;
                    }
                    if (xn.InnerXml == "dd_ZZ")
                    {
                        xn.InnerXml = day_ZZ;
                    }
                    if (xn.InnerXml == "YYYY_PT")
                    {
                        xn.InnerXml = year_QS;
                    }
                    if (xn.InnerXml == "MM_PT")
                    {
                        xn.InnerXml = month_QS;
                    }
                    if (xn.InnerXml == "dd_PT")
                    {
                        xn.InnerXml = day_QS;
                    }
                }
                xmlDoc.Save(NewPaths);
                if (File.Exists(NewPaths))
                {
                    Document doc = new Document(NewPaths);
                    ImageSaveOptions iso = new ImageSaveOptions(SaveFormat.Png);//另存为PNG格式
                    iso.Resolution = 256;
                   
                    doc.Save(Server.MapPath(SavingPath), iso);
                }
                File.Delete(NewPaths);
                File.Delete(Paths);
            }
            return SavingPath;
        }
        else
        {

            return null;
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
        if (!string.IsNullOrEmpty(this.txtCS_SHYJ.Text.Trim()))
        {
            if (this.txtCS_SHYJ.Text.Trim().Contains("'"))
            {
                this.txtCS_SHYJ.Text = this.txtCS_SHYJ.Text.Trim().Replace("'", "‘");
            }
            if (this.txtCS_SHYJ.Text.Trim().Length > 1000)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('您输入的审核意见字符过长，请限制在1000个字符以内！');</script>");
                this.btnReject.Enabled = true;
                this.btnPass.Enabled = true;
                return;
            }
            if (IsJJRGHGLJG())
            {
                string url = "New2013/JHJX_FGSJYZHSH_JJR.aspx";
                //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前交易账户已更换所属服务机构！', function () { var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前交易账户已更换所属服务机构！', function () {  __doPostBack('btnBack','');    });</script>");
                return;
            }
            else if (IsGHZCLB())
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('当前交易账户已更换注册类别，请重新审核！', function () {  window.location.href=window.location.href;  });</script>");
                return;
            }
            else if (IsFGSPassJJR())
            {
                string url = "New2013/JHJX_FGSJYZHSH_JJR.aspx";
                //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前经纪人交易账户已经被审核通过，不能再进行驳回操作！', function () {  var a = window.location.href; var b = a.substring(0, a.indexOf('New2013/'));  var newurl = b+'" + url + "'; window.location.href=newurl;   });</script>");
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前经纪人交易账户已经被审核通过，不能再进行驳回操作！', function () {   __doPostBack('btnBack','');   });</script>");
                return;

            }
            else
            {

                ArrayList arrayList = new ArrayList();
                string strSqlJSB = "update AAA_DLZHXXB set S_SFYBFGSSHTG='否' where B_JSZHLX='经纪人交易账户' and  J_JJRJSBH='" + ViewState["JSBH"].ToString() + "' ";
                object objName = GetEmployee_Name();

                string strSqlGLB = "update AAA_MJMJJYZHYJJRZHGLB set FGSSHZT='驳回',SFXG='否', FGSSHR='" + objName.ToString() + "',FGSSHSJ='" + DateTime.Now.ToString() + "',FGSSHYJ='" + this.txtCS_SHYJ.Text.ToString().Trim() + "' where DLYX='" + ViewState["DLYX"].ToString() + "' and JSZHLX='经纪人交易账户' and  GLJJRBH='" + ViewState["JSBH"].ToString() + "' and SFYX='是'   ";
                arrayList.Add(strSqlGLB);
                //自身开户申请    [x1]被分公司审核时驳回	-0.5	减项	分公司审核时
                  Hesion.Brick.Core.User us_ls = Hesion.Brick.Core.Users.GetUserByNumber(User.Identity.Name);
                 object objNameValue = us_ls.Name.ToString();
                jhjx_JYFXYMX jhjxJYFXYMX = new jhjx_JYFXYMX();
                 string[] strJYFXYMX=jhjxJYFXYMX.ZSKHSQ_BFGSSHSBH(ViewState["DLYX"].ToString(), "经纪人", objNameValue.ToString());
                arrayList.AddRange(strJYFXYMX);
                //关联交易方	关联交易方[x1]，被分公司审核时驳回	-1	减项	分公司审核时      李朋波
                //string[] strGLJJF = jhjxJYFXYMX.GLJYF_BFGSSHSBH(ViewState["DLYX"].ToString(), ViewState["DLYX"].ToString(), "经纪人", objNameValue.ToString());
                //arrayList.AddRange(strGLJJF);
                if (DbHelperSQL.ExecSqlTran(arrayList))
                {


                    Hashtable ht = new Hashtable();
                    ht["type"] = "集合集合经销平台";
                    ht["提醒对象登陆邮箱"] = ViewState["DLYX"].ToString();
                    ht["提醒对象用户名"] = ViewState["JYFMC"].ToString();
                    ht["提醒对象结算账户类型"] = "经纪人交易账户";
                    ht["提醒对象角色编号"] = ViewState["JSBH"].ToString();
                    ht["提醒对象角色类型"] = "经纪人";
                    //ht["提醒内容文本"] = " 尊敬的" + ViewState["JYFMC"].ToString() + "，您提交的开通交易账户的资料未能通过平台管理机构的审核，请与您的平台管理机构联系！";
                    ht["提醒内容文本"] = " 您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请。 ";
                    ht["创建人"] = objName;
                    List<Hashtable> list = new List<Hashtable>();
                    list.Add(ht);
                    JHJX_SendRemindInfor.Sendmes(list);

                    string url = "New2013/JHJX_FGSJYZHSH_JJR.aspx";
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核信息保存成功，同时向该用户发送审核驳回提醒！', function () {   __doPostBack('btnBack','');   });</script>");
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('服务器异常，请联系管理员！');</script>");
                }
            }

        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核意见不能为空，请填写审核意见！');</script>");
            this.btnReject.Enabled = true;
            this.btnPass.Enabled = true;
        }
    }


    /// <summary>
    /// 判断当前经纪人是否更换平台管理机构
    /// </summary>
    /// <returns></returns>
    protected bool IsJJRGHGLJG()
    {
        string strIsExists = "select  a.Number,B_DLYX '交易方账号',I_JYFMC '交易方名称',convert(varchar(10),a.CreateTime,120) '资料提交时间',a.CreateTime,I_ZCLB '注册类型',I_SSQYS+I_SSQYSHI+I_SSQYQ '所属区域',I_PTGLJG '平台管理机构',I_LXRXM '联系人姓名',I_LXRSJH '联系电话',FGSSHZT '初审记录'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.GLJJRBH=b.J_JJRJSBH  where   I_PTGLJG = '" + this.ddlPTGLJG.SelectedItem.ToString() + "' and  a.JSZHLX='经纪人交易账户' and b.J_JJRJSBH='" + ViewState["JSBH"].ToString()+ "' and a.SFYX='是'  ";

      DataTable dataTable=DbHelperSQL.Query(strIsExists).Tables[0];
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
          return false;
      }
      else
      {
          return true;
      }
    }
    /// <summary>
    /// 判断当前交易账户是否更换注册类型
    /// </summary>
    /// <returns>false 未更换注册类别，true已更换注册类别</returns>
    protected bool IsGHZCLB()
    {
        string strIsExists = "select  a.Number,B_DLYX '交易方账号',I_JYFMC '交易方名称',convert(varchar(10),a.CreateTime,120) '资料提交时间',a.CreateTime,I_ZCLB '注册类型',I_SSQYS+I_SSQYSHI+I_SSQYQ '所属区域',I_PTGLJG '平台管理机构',I_LXRXM '联系人姓名',I_LXRSJH '联系电话',FGSSHZT '初审记录'  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.GLJJRBH=b.J_JJRJSBH  where   I_PTGLJG = '" + this.ddlPTGLJG.SelectedItem.ToString() + "' and  a.JSZHLX='经纪人交易账户' and b.J_JJRJSBH='" + ViewState["JSBH"].ToString() + "' and a.SFYX='是'  ";
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
        DataTable dataTable = DbHelperSQL.Query(strIsExists).Tables[0];
        if (dataTable.Rows[0]["注册类型"].ToString() == strZCLB)
        {
            return false;
        }
        else
        {
            return true;
        }
       
    }

    /// <summary>
    /// 判断当前经纪人是否已经被分公司审核通过
    /// </summary>
    /// <param name="strMMJDLYX"></param>
    /// <param name="strGLJJRBH"></param>
    /// <returns></returns>
    public bool IsFGSPassJJR()
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
        Response.Redirect("JHJX_FGSJYZHSH_JJR.aspx");
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