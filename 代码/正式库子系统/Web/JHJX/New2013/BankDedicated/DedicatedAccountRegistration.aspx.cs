using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Key;

public partial class Web_JHJX_New2013_BankDedicated_DedicatedAccountRegistration : System.Web.UI.Page
{
    string url = "http://192.168.0.26:777/pingtaiservices/ws2013.asmx";
    string StrSavePath = "ZhuCe\\jjr_bank";
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] args=new string[2];

        object result = WebServiceHelper.InvokeWebService("http://192.168.0.26:777/pingtaiservices/ws2013.asmx", "GetPublisDsData", null);
        DataSet ds = (DataSet)result;
        //GridView1.DataSource = ds;
        //GridView1.DataBind();

        if(!IsPostBack)
        {
            this.UCCityList1.initdefault();
        }
        string[] city = UCCityList1.SelectedItem;
        txtMM.Attributes.Add("value", txtMM.Text);
        txtQRMM.Attributes.Add("value", txtQRMM.Text); 
    }

    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="file">上传控件id</param>
    /// <param name="filename">lable 记录上传的图片的路径</param>
    /// <param name="btnCK">查看按钮</param>
    /// <param name="btnDel">删除按钮</param>
    /// <param name="message">提示文字</param>
    protected void SC(FileUpload file,Label filename,Button btnCK,Button btnDel,string message)
    {
        if (file.PostedFile.FileName != "")
        {
            filename.Text = "";

            #region//判断是否存在jjr_bank文件夹，如果没有则新建
            string StrRealPath = Server.MapPath(StrSavePath);
            if (!Directory.Exists(StrRealPath))
            {
                Directory.CreateDirectory(StrRealPath);
            }
            #endregion

            if (file.PostedFile.ContentLength > 30000000)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('文件太大，无法上传！');</script>");
                return;
            }

            string strFilename = file.PostedFile.FileName;

            string fileshortName = strFilename.Substring(strFilename.LastIndexOf('\\') + 1);

            if (fileshortName.Split('[').Length > 1 || fileshortName.Split(']').Length > 1)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('文件名称中不允许包含‘]’或者‘[’字符,假若确实需要，请使用中文‘】’或者‘【’代替！');</script>");
                return;
            }
            if (fileshortName.Split('%').Length > 1)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('文件名称中不允许包含‘%’字符！');</script>");
                return;
            }

            string fileleixing = System.IO.Path.GetExtension(file.FileName).ToString().ToLower(); //获取文件的扩展名
            if (fileleixing != ".jpg" && fileleixing != ".jpeg" && fileleixing != ".bmp" && fileleixing != ".gif" && fileleixing != ".psd" && fileleixing != ".png" && fileleixing != ".pcx" && fileleixing != ".dxf" && fileleixing != ".cdr")
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('只能上传图片文件！');</script>");
                return;
            }
            double fileLenth = file.PostedFile.ContentLength / 1024.0 / 1024.0;
            if (fileLenth>1)
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('上传图片最大为1M，请重新选择图片！');</script>");
                return;
            }            

            /*上传附件*/

            string Filename =  User.Identity.Name + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + fileleixing;

            filename.ToolTip = "jjr_bank\\" + Filename;

            string path = keyEncryption.encMe("/Web/JHJX/New2013/BankDedicated/ZhuCe/jjr_bank/" + Filename, "mimamima");
            //lblYYZZ.Text = "<a href = '../../../picView/picView.aspx?path=" + path + "&jiami=y'  target='_blank'>" + fileshortName + "</a>";
            filename.Text = path;
            file.PostedFile.SaveAs(StrRealPath + "\\" + Filename);

            this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('上传成功！');</script>");
            btnCK.Visible = true;
            btnDel.Visible = true;
        }
        else
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('" + message + "');</script>");
        }
    }

    #region//营业执照
    protected void btnYYZZUpload_Click(object sender, EventArgs e)
    {
        SC(FULoadYYZZ, lblYYZZ, btnYYZZCK, btnYYZZDelete, "请选择需要上传的营业执照扫描件！");        
    }
    protected void btnYYZZCK_Click(object sender, EventArgs e)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >var a = window.location.href; var b = a.substring(0,a.lastIndexOf('/JHJX')); var url = b + '/picView/picView.aspx?path=" + lblYYZZ.Text + "&jiami=y';  window.open(url);</script>");

    }
    protected void btnYYZZDelete_Click(object sender, EventArgs e)
    {
        this.lblYYZZ.ToolTip = "";
        lblYYZZ.Text = "";
        btnYYZZCK.Visible = false;
        btnYYZZDelete.Visible = false;
    }
    #endregion

    #region//组织机构代码证
    protected void btnZZJGDMZUpload_Click(object sender, EventArgs e)
    {
        SC(FULoadZZJGDMZ, lblZZJGDMZ, btnZZJGDMZCK, btnZZJGDMZDelete, "请选择需要上传的组织机构代码证扫描件！");
    }    
    protected void btnZZJGDMZCK_Click(object sender, EventArgs e)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >var a = window.location.href; var b = a.substring(0,a.lastIndexOf('/JHJX')); var url = b + '/picView/picView.aspx?path=" + lblZZJGDMZ.Text + "&jiami=y';  window.open(url);</script>");
    }
    protected void btnZZJGDMZDelete_Click(object sender, EventArgs e)
    {
        this.lblZZJGDMZ.ToolTip = "";
        lblZZJGDMZ.Text = "";
        btnZZJGDMZCK.Visible = false;
        btnZZJGDMZDelete.Visible = false;
    }
    #endregion

    #region//税务登记证
    protected void btnSWDJZUpload_Click(object sender, EventArgs e)
    {
        SC(FULoadSWDJZ, lblSWDJZ, btnSWDJCK, btnSWDJDelete, "请选择需要上传的税务登记证扫描件！");
    }
    protected void btnSWDJCK_Click(object sender, EventArgs e)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >var a = window.location.href; var b = a.substring(0,a.lastIndexOf('/JHJX')); var url = b + '/picView/picView.aspx?path=" + lblSWDJZ.Text + "&jiami=y';  window.open(url);</script>");
    }
    protected void btnSWDJDelete_Click(object sender, EventArgs e)
    {
        this.lblSWDJZ.ToolTip = "";
        lblSWDJZ.Text = "";
        btnSWDJCK.Visible = false;
        btnSWDJDelete.Visible = false;
    }
    #endregion

    #region// 开户许可证
    protected void btnKHXKZUpload_Click(object sender, EventArgs e)
    {
        SC(FULoadKHXKZ, lblKHXKZ, btnKHXKZCK, btnKHXKZDelete, "请选择需要上传的开户许可证扫描件！");
    }
    protected void btnKHXKZCK_Click(object sender, EventArgs e)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >var a = window.location.href; var b = a.substring(0,a.lastIndexOf('/JHJX')); var url = b + '/picView/picView.aspx?path=" + lblKHXKZ.Text + "&jiami=y';  window.open(url);</script>");
    }
    protected void btnKHXKZDelete_Click(object sender, EventArgs e)
    {
        this.lblKHXKZ.ToolTip = "";
        lblKHXKZ.Text = "";
        btnKHXKZCK.Visible = false;
        btnKHXKZDelete.Visible = false;
    }
    #endregion

    #region// 预留印签卡
    protected void btnYLYQKUpload_Click(object sender, EventArgs e)
    {
        SC(FULoadYLYQK, lblYLYQK, btnYLYQKCK, btnYLYQKDelte, "请选择需要上传的预留印签卡扫描件！");
    }
    protected void btnYLYQKCK_Click(object sender, EventArgs e)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >var a = window.location.href; var b = a.substring(0,a.lastIndexOf('/JHJX')); var url = b + '/picView/picView.aspx?path=" + lblYLYQK.Text + "&jiami=y';  window.open(url);</script>");
    }
    protected void btnYLYQKDelte_Click(object sender, EventArgs e)
    {
        this.lblYLYQK.ToolTip = "";
        lblYLYQK.Text = "";
        btnYLYQKCK.Visible = false;
        btnYLYQKDelte.Visible = false;
    }
    #endregion

    #region//身份证正面扫描件
    protected void btnSFZZMUpload_Click(object sender, EventArgs e)
    {
        SC(FULoadSFZZM, lblSFZZM, btnSFZZMCK, btnSFZZMDelete, "请选择需要上传的身份证正面扫描件！");
    }
    protected void btnSFZZMCK_Click(object sender, EventArgs e)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >var a = window.location.href; var b = a.substring(0,a.lastIndexOf('/JHJX')); var url = b + '/picView/picView.aspx?path=" + lblSFZZM.Text + "&jiami=y';  window.open(url);</script>");
    }
    protected void btnSFZZMDelete_Click(object sender, EventArgs e)
    {
        this.lblFDRSQS.ToolTip = "";
        lblFDRSQS.Text = "";
        btnSFZZMCK.Visible = false;
        btnSFZZMDelete.Visible = false;
    }
    #endregion

    #region//身份证反面扫描件
    protected void btnSFZFMUpload_Click(object sender, EventArgs e)
    {
        SC(FULoadSFZFM, lblSFZFM, btnSFZFMCK, btnSFZFMDelete, "请选择需要上传的法定代表人身份证反面扫描件！");
    }
    protected void btnSFZFMCK_Click(object sender, EventArgs e)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >var a = window.location.href; var b = a.substring(0,a.lastIndexOf('/JHJX')); var url = b + '/picView/picView.aspx?path=" + lblSFZFM.Text + "&jiami=y';  window.open(url);</script>");
    }
    protected void btnSFZFMDelete_Click(object sender, EventArgs e)
    {
        this.lblSFZFM.ToolTip = "";
        lblSFZFM.Text = "";
        btnSFZFMCK.Visible = false;
        btnSFZFMDelete.Visible = false;
    }
    #endregion

    #region//法定人代表人授权书
    protected void btnFDRDBSQSUpload_Click(object sender, EventArgs e)
    {
        SC(FULoadFDRDBSQS, lblFDRSQS, btnFDRDBSQSCK, btnFDRDBSQSDelete, "请选择需要上传的法定代表人授权书扫描件！");
    }
    protected void btnFDRDBSQSCK_Click(object sender, EventArgs e)
    {
        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >var a = window.location.href; var b = a.substring(0,a.lastIndexOf('/JHJX')); var url = b + '/picView/picView.aspx?path=" + lblFDRSQS.Text + "&jiami=y';  window.open(url);</script>");
    }
    protected void btnFDRDBSQSDelete_Click(object sender, EventArgs e)
    {
        this.lblFDRSQS.ToolTip = "";
        lblFDRSQS.Text = "";
        btnFDRDBSQSCK.Visible = false;
        btnFDRDBSQSDelete.Visible = false;
    }
    #endregion


    protected void btnPass_Click(object sender, EventArgs e)
    {
        try
        {
            #region//验证所属区域
            if (UCCityList1.SelectedItem[0].ToString().Contains("请选择") || UCCityList1.SelectedItem[1].ToString().Contains("请选择") || UCCityList1.SelectedItem[2].ToString().Contains("请选择"))
            {
                lblSSQYYZ.Text = "请填写完整的省市区信息！";
                return;
            }
            else
            {
                lblSSQYYZ.Text = "";
            }
            #endregion

            #region//验证身份证号是否已经被注册
            if (!JudgeSFZH("法定代表人身份证号", txtSFZH.Text.Trim()))
            {
                lblSFZZMYZ.Text = "此身份证号已经被注册！";
                return;
            }
            else
            {
                lblSFZZMYZ.Text = "";
            }
            #endregion

            #region//验证邮箱、用户名
            Hashtable ht = new Hashtable();
            ht["dlyx"] = this.txtYX.Text;//邮箱
            ht["yhm"] = this.txtUserName.Text;//用户名
            ht["pwd"] = this.txtMM.Text;//密码
            ht["yzm"] = "123456";//Support.StringOP.GetRandomNumber(10, true);//验证码
            object result = WebServiceHelper.InvokeWebService(url, "UserRegister", new object[] { ht["dlyx"].ToString(), ht["yhm"].ToString().Trim(), ht["pwd"].ToString().Trim(), ht["yzm"].ToString().Trim() });
            DataSet ds = (DataSet)result;
            if (ds.Tables[0].Rows[0]["RESULT"].ToString() == "SUCCESS")
            {
                string pk = ds.Tables[0].Rows[0]["pk"].ToString();//主键
                string dlyx = ds.Tables[0].Rows[0]["DLYX"].ToString();//邮箱
                string yhm = ds.Tables[0].Rows[0]["YHM"].ToString();//用户名
                string dlmm = ds.Tables[0].Rows[0]["DLMM"].ToString();//登录密码
                string yzm = ds.Tables[0].Rows[0]["YZM"].ToString();//验证码

                #region//进行更新 标志验证邮箱的一些的字段流程
                result = WebServiceHelper.InvokeWebService(url, "AccountID", new object[] { dlyx, yzm });
                DataSet dsYZEmail = (DataSet)result;
                if (dsYZEmail.Tables[0].Rows[0]["RESULT"].ToString() == "SUCCESS")
                {
                    #region//进行注册交易账户流程- 准备注册交易账户参数

                    string smj = "DBL.png";//非必须上传扫描件

                    Hashtable hashTableInfor = new Hashtable();
                    hashTableInfor["经纪人单位交易账户类别"] = "经纪人交易账户";
                    hashTableInfor["经纪人单位交易注册类别"] = "单位";
                    hashTableInfor["经纪人单位交易方名称"] = txtJJFMC.Text.Trim();
                    hashTableInfor["经纪人单位营业执照注册号"] = txtYYZZZCH.Text.Trim();
                    hashTableInfor["经纪人单位营业执照扫描件"] = lblYYZZ.ToolTip.Trim();
                    hashTableInfor["经纪人单位组织机构代码证代码证"] = txtZZJGDMZDM.Text.Trim();
                    hashTableInfor["经纪人单位组织机构代码证代码证扫描件"] = lblZZJGDMZ.ToolTip.Trim();
                    hashTableInfor["经纪人单位税务登记证税号"] = txtSWDJZSH.Text.Trim();
                    hashTableInfor["经纪人单位税务登记证扫描件"] = lblSWDJZ.ToolTip.Trim();
                    hashTableInfor["经纪人单位一般纳税人资格证扫描件"] = smj;
                    hashTableInfor["经纪人单位开户许可证号"] = txtKHXKZ.Text.Trim();
                    hashTableInfor["经纪人单位开户许扫描件"] = lblKHXKZ.ToolTip.Trim();
                    hashTableInfor["经纪人单位预留印鉴卡扫描件"] = lblYLYQK.ToolTip.Trim();
                    hashTableInfor["经纪人单位法定代表人姓名"] = txtFDDBRXM.Text.Trim();
                    hashTableInfor["经纪人单位法定代表人身份证号"] = txtSFZH.Text.Trim();
                    hashTableInfor["经纪人单位法定代表人身份证扫描件"] = lblSFZZM.ToolTip.Trim();
                    hashTableInfor["经纪人单位法定代表人身份证反面扫描件"] = lblSFZFM.ToolTip.Trim();
                    hashTableInfor["经纪人单位法定代表人授权书"] = lblFDRSQS.ToolTip.Trim() == "" ? smj : lblFDRSQS.ToolTip.Trim();
                    hashTableInfor["经纪人单位交易方联系电话"] = txtJYFLXDH.Text.Trim();
                    hashTableInfor["经纪人单位所属省份"] = UCCityList1.SelectedItem[0].Trim();
                    hashTableInfor["经纪人单位所属地市"] = UCCityList1.SelectedItem[1].Trim();
                    hashTableInfor["经纪人单位所属区县"] = UCCityList1.SelectedItem[2].Trim();
                    hashTableInfor["经纪人单位详细地址"] = txtXXDZ.Text.Trim();
                    hashTableInfor["经纪人单位联系人姓名"] = txtLXRXM.Text.Trim();
                    hashTableInfor["经纪人单位联系人手机号"] = txtLXRSJH.Text.Trim();
                    hashTableInfor["经纪人单位开户银行"] = drpBank.SelectedValue.Trim();
                    hashTableInfor["经纪人单位银行账号"] = "";
                    hashTableInfor["经纪人单位平台管理机构"] = "平台总部";
                    hashTableInfor["经纪人单位登录邮箱"] = dlyx;
                    hashTableInfor["经纪人单位用户名"] = yhm;
                    hashTableInfor["经纪人单位证券资金密码"] = txtJYZJMM.Text.Trim();
                    hashTableInfor["经纪人单位院系编号"] = "";
                    hashTableInfor["经纪人单位经纪人分类"] = rdbtnJJRLX.SelectedValue.Trim();
                    hashTableInfor["经纪人单位业务管理部门分类"] = "平台总部";
                    #endregion

                    DataTable dataTable = StringOP.GetDataTableFormHashtable(hashTableInfor);
                    result = WebServiceHelper.InvokeWebService(url, "SubmitJJRDW", new object[] { dataTable });
                    DataSet dsJJZH = (DataSet)result;
                    if (dsJJZH.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok")
                    {
                        string wblsh = dsJJZH.Tables["用户信息"].Rows[0]["Number"].ToString();//登录表number
                        string jyfmc = dsJJZH.Tables["用户信息"].Rows[0]["交易方名称"].ToString();
                        string zzjgdmz = dsJJZH.Tables["用户信息"].Rows[0]["组织机构代码证代码"].ToString();
                        string gljjrjsbh = dsJJZH.Tables["关联信息"].Rows[0]["关联经纪人编号"].ToString();
                        string zqzjzh = "";//证券资金账号
                        string jyzhlx = dsJJZH.Tables["用户信息"].Rows[0]["结算账户类型"].ToString();
                        string ptgljg = dsJJZH.Tables["用户信息"].Rows[0]["平台管理机构"].ToString();

                        #region//进行分公司审核及终审流程 
                        if (ConfigurationManager.ConnectionStrings["onlineJHJX_YHJK"].ToString().Trim() == "开启银行接口")
                        {
                            #region//开启银行接口调用的方法

                            //生成营业部编码
                            ErpNumber erpNumber = new ErpNumber();

                            //向银行发送指令代码
                            DataTable dt = BankServicePF.N26701_Deal_Init();
                            dt.Rows[0]["ExSerial"] = wblsh;//外部流水号
                            dt.Rows[0]["sBranchId"] = "0000";//营业部编码
                            dt.Rows[0]["Name"] = jyfmc;//客户姓名
                            string strCustProperty = "1";//客户性质
                            string strCardType = "8";//客户证件类型    
                            string strCard = zzjgdmz;//客户证件号
                            string strerpNumber = erpNumber.GetZQZJNumber("单位");//证券资金账号
                            zqzjzh=strerpNumber;
                            dt.Rows[0]["OccurBala"] = "0";//客户姓名

                            dt.Rows[0]["sCustAcct"] = strerpNumber;
                            dt.Rows[0]["sCustProperty"] = strCustProperty;
                            dt.Rows[0]["CardType"] = strCardType;
                            dt.Rows[0]["Card"] = strCard;
                            dt.Rows[0]["Date"] = DateTime.Now.ToString("yyyyMMdd");
                            DataSet dsPF = BankServicePF.N26701_Deal_Push(dt);
                            DataTable dtPFResult = dsPF.Tables["result"];
                            if (dtPFResult.Rows.Count <= 0)
                            {
                                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('银行接口调用失败！');</script>");
                                this.btnPass.Enabled = false;
                                this.btnReject.Enabled = false;
                                return;

                            }

                            if (dtPFResult.Rows[0]["ErrorNo"].ToString() == "0000")//银行成功返回信息
                            {
                                #region //本地代码
                                DateTime dateTimeStart = DateTime.Now;
                                DateTime dateTimeEnd = dateTimeStart.AddYears(2);

                                string strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='是', J_JJRZGZSYXQKSSJ='" + dateTimeStart.ToString() + "', J_JJRZGZSYXQJSSJ='" + dateTimeEnd.ToString() + "',I_DSFCGZT='未开通',I_ZQZJZH='" + strerpNumber + "',I_GLYH='" + jyfmc.Trim() + "',I_GLYHGZRYGH='无' where B_JSZHLX='经纪人交易账户'   and  J_JJRJSBH='" + gljjrjsbh + "' ";

                                string strSHYJ = "";
                                string strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set FGSSHZT='审核通过', FGSSHR='系统自动',FGSSHSJ='" + DateTime.Now.ToString() + "',FGSSHYJ='" + strSHYJ + "'    where JSZHLX='经纪人交易账户' and  GLJJRBH='" + gljjrjsbh + "' and SFYX='是'  ";

                                //交易账户终审
                                string strJYZHZSB = jhjx_PublicClass.GetNextNumberZZ("AAA_JYZHZSB", ""); //生成主键
                                string strSQL3= "insert AAA_JYZHZSB(Number,DLYX,JYFBH,JSZHLX,GLJJRJSBH,PTGLJG,FWZXSHZT,FWZXSHYJ,FWZXSHSJ,FWZXSHR,JYGLBSHZT) values('" + strJYZHZSB + "','" + dlyx.Trim() + "','" + zqzjzh + "','" + jyzhlx.Trim() + "','" + gljjrjsbh + "','" + ptgljg + "','审核通过','','" + DateTime.Now.ToString() + "','系统自动','尚未处理')";

                                ArrayList arrayList = new ArrayList();
                                arrayList.Add(strSql1);
                                arrayList.Add(strSql2);//发送提醒  尚未做
                                arrayList.Add(strSQL3);
                                if (DbHelperSQL.ExecSqlTran(arrayList))
                                {
                                    Hashtable htJHJX = new Hashtable();
                                    htJHJX["type"] = "集合集合经销平台";
                                    htJHJX["提醒对象登陆邮箱"] = dlyx;
                                    htJHJX["提醒对象用户名"] = jyfmc;
                                    htJHJX["提醒对象结算账户类型"] = "经纪人交易账户";
                                    htJHJX["提醒对象角色编号"] = gljjrjsbh;
                                    htJHJX["提醒对象角色类型"] = "经纪人";
                                    htJHJX["提醒内容文本"] = "您" + strerpNumber + "交易方编号的交易账户已开通，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。";
                                    htJHJX["创建人"] = "系统自动";
                                    List<Hashtable> list = new List<Hashtable>();
                                    list.Add(htJHJX);
                                    JHJX_SendRemindInfor.Sendmes(list);
                                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('专用账户开通成功！', function () {var a = window.location.href; var b = a.substring(0, a.lastIndexOf('/JHJX')); var url = b + '/JHJX/New2013/BankDedicated/DedicatedAccountRegistration.aspx'; window.location = url;  });</script>");
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
                                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('" + dtPFResult.Rows[0]["ErrorInfo"].ToString() + "');</script>");

                            }
                            #endregion
                        }
                        else if (ConfigurationManager.ConnectionStrings["onlineJHJX_YHJK"].ToString().Trim() == "关闭银行接口")
                        {
                            #region//关闭银行接口调用的方法

                            // //生成营业部编码
                            ErpNumber erpNumber = new ErpNumber();
                            string strerpNumber = "";//证券资金账号
                            strerpNumber = erpNumber.GetZQZJNumber("单位");
                            zqzjzh = strerpNumber;
                            #region //本地代码
                            DateTime dateTimeStart = DateTime.Now;
                            DateTime dateTimeEnd = dateTimeStart.AddYears(2);

                           // string strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='是', J_JJRZGZSYXQKSSJ='" + dateTimeStart.ToString() + "', J_JJRZGZSYXQJSSJ='" + dateTimeEnd.ToString() + "',I_DSFCGZT='未开通',I_ZQZJZH='" + strerpNumber + "',I_GLYH='" + jyfmc.Trim() + "',I_GLYHGZRYGH='无' where B_JSZHLX='经纪人交易账户'   and  J_JJRJSBH='" + gljjrjsbh + "' "; 模拟运行期间暂时去掉 2014.08.26 
                            string strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='是', J_JJRZGZSYXQKSSJ='" + dateTimeStart.ToString() + "', J_JJRZGZSYXQJSSJ='" + dateTimeEnd.ToString() + "',I_DSFCGZT='开通',I_ZQZJZH='" + strerpNumber + "',I_GLYH='" + jyfmc.Trim() + "',I_GLYHGZRYGH='无' where B_JSZHLX='经纪人交易账户',I_YHZH='558212166545464546', I_JYFMC='测试开通' where B_JSZHLX='经纪人交易账户'    and  J_JJRJSBH='" + gljjrjsbh + "' "; //模拟运行期间专用 wyh 2014.08.26 

                            object objName = "系统自动";
                            string strSHYJ = "";
                            string strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set FGSSHZT='审核通过', FGSSHR='" + objName.ToString().Trim() + "',FGSSHSJ='" + DateTime.Now.ToString() + "',FGSSHYJ='" + strSHYJ + "'    where JSZHLX='经纪人交易账户' and  GLJJRBH='" + gljjrjsbh + "' and SFYX='是'  ";

                            //交易账户终审
                            string strJYZHZSB = jhjx_PublicClass.GetNextNumberZZ("AAA_JYZHZSB", ""); //生成主键
                            string strSQL3 = "insert AAA_JYZHZSB(Number,DLYX,JYFBH,JSZHLX,GLJJRJSBH,PTGLJG,FWZXSHZT,FWZXSHYJ,FWZXSHSJ,FWZXSHR,JYGLBSHZT) values('" + strJYZHZSB + "','" + dlyx.Trim() + "','" + zqzjzh + "','" + jyzhlx.Trim() + "','" + gljjrjsbh + "','" + ptgljg + "','审核通过','','" + DateTime.Now.ToString() + "','系统自动','尚未处理')";

                            ArrayList arrayList = new ArrayList();
                            arrayList.Add(strSql1);
                            arrayList.Add(strSql2);//发送提醒  尚未做
                            arrayList.Add(strSQL3);
                            if (DbHelperSQL.ExecSqlTran(arrayList))
                            {
                                Hashtable htJHJX = new Hashtable();
                                htJHJX["type"] = "集合集合经销平台";
                                htJHJX["提醒对象登陆邮箱"] = dlyx;
                                htJHJX["提醒对象用户名"] = jyfmc;
                                htJHJX["提醒对象结算账户类型"] = "经纪人交易账户";
                                htJHJX["提醒对象角色编号"] = gljjrjsbh;
                                htJHJX["提醒对象角色类型"] = "经纪人";
                                htJHJX["提醒内容文本"] = "您" + strerpNumber + "交易方编号的交易账户已开通，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。";
                                htJHJX["创建人"] = objName;
                                List<Hashtable> list = new List<Hashtable>();
                                list.Add(htJHJX);
                                JHJX_SendRemindInfor.Sendmes(list);
                                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('专用账户开通成功！', function () {var a = window.location.href; var b = a.substring(0, a.lastIndexOf('/JHJX')); var url = b + '/JHJX/New2013/BankDedicated/DedicatedAccountRegistration.aspx'; window.location = url;  });</script>");
                            }
                            else
                            {
                                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器异常，请联系管理员！');</script>");
                            }
                            #endregion
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('" + dsJJZH.Tables["返回值单条"].Rows[0]["提示文本"].ToString() + "');</script>");
                    }
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('" + dsYZEmail.Tables[0].Rows[0]["REASON"].ToString() + "');</script>");
                    return;
                }
                #endregion


            }
            else
            {
                if (ds.Tables[0].Rows[0]["REASON"].ToString() == "已存在该帐户")
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('很抱歉，该邮箱已被注册，您可以尝试其他邮箱！');</script>");
                    return;
                }
                else if (ds.Tables[0].Rows[0]["REASON"].ToString() == "用户名重复")
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('很抱歉，该用户名已被注册，您可以尝试其他用户名');</script>");
                    return;
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('服务器繁忙，请稍后再试！');</script>");
                    return;
                }


            }
            #endregion
        }
        catch(Exception ex)
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Dialog.alert('" + ex.Message.ToString() + "');</script>");
            return;
        }
    }
    /// <summary>
    /// 判断身份证号是否已经被注册
    /// </summary>
    /// <param name="strZHLX"></param>
    /// <param name="strSqlSFZH"></param>
    /// <returns></returns>
    private bool JudgeSFZH(string strZHLX, string strSqlSFZH)
    {
        string[] args = { strZHLX, strSqlSFZH };
        object result = WebServiceHelper.InvokeWebService(url, "JudgeSFZHXX", args);
        DataSet dsreturn = (DataSet)result;
        DataTable dt = dsreturn.Tables["返回值单条"];
        if (dt != null && dt.Rows.Count > 0)
        {
            switch (dt.Rows[0]["执行结果"].ToString())
            {
                case "ok":
                    return true;
                case "err":
                    return false;
                default:
                    return false;
            }
        }
        return false;
    }

}