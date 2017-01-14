using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using Key;

public partial class Web_JHJX_JHJX_JYFWGKFTJ : System.Web.UI.Page
{
    string StrSavePath = "docs";
    public static string Filename = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetItems();

        }
    }

    protected void SetItems()
    {
        DataSet ds = DbHelperSQL.Query("select SXNR from AAA_WGSXB");


        ddlwgsx.DataSource = ds;
        ddlwgsx.DataTextField = "SXNR";
        ddlwgsx.DataValueField = "SXNR";
        ddlwgsx.DataBind();
        ddlwgsx.Items.Insert(0, new ListItem("请选择违规事项", ""));
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        //先初始化
        Initial();
        if (txtjyfzh.Text.Trim() == "" && txtjyfmc.Text.Trim()=="")
        {
            MessageBox.Show(this, "请输入您要查找的交易方账号或者交易方名称！");
           
            return;

        }
        DataSet ds = DbHelperSQL.Query(GetSql(txtjyfzh.Text.Trim(),txtjyfmc.Text.Trim()));

        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
           
            MessageBox.Show(this,"查询不到您输入的账户，请重新进行输入！");
        }
        else if (ds != null && ds.Tables[0].Rows.Count > 1)
        {
            
            MessageBox.Show(this, "查询到多个账户，请确认交易方账号和名称是否属于同一账户！");
        }
        else if(ds!=null && ds.Tables[0].Rows.Count==1)
        {
            lbjyfzh.Text =ds.Tables[0].Rows[0]["交易方账号"]==null?"": ds.Tables[0].Rows[0]["交易方账号"].ToString().Trim();
            lbjyfmc.Text= ds.Tables[0].Rows[0]["交易方名称"]==null?"": ds.Tables[0].Rows[0]["交易方名称"].ToString().Trim();
            lbjyzhlx.Text=ds.Tables[0].Rows[0]["交易账户类型"]==null?"": ds.Tables[0].Rows[0]["交易账户类型"].ToString().Trim();
            lbzclb.Text =ds.Tables[0].Rows[0]["注册类别"]==null?"":ds.Tables[0].Rows[0]["注册类别"].ToString().Trim();
            lblxrxm.Text =ds.Tables[0].Rows[0]["联系人姓名"]==null?"": ds.Tables[0].Rows[0]["联系人姓名"].ToString().Trim();
            lblxrsjh.Text =ds.Tables[0].Rows[0]["联系人手机号"]==null?"": ds.Tables[0].Rows[0]["联系人手机号"].ToString().Trim();
            lbssqy.Text =ds.Tables[0].Rows[0]["所属区域"]==null?"": ds.Tables[0].Rows[0]["所属区域"].ToString().Trim();
            lbgljjrzh.Text = ds.Tables[0].Rows[0]["关联经纪人账号"]==null?"": ds.Tables[0].Rows[0]["关联经纪人账号"].ToString().Trim();
            lbssfgs.Text =ds.Tables[0].Rows[0]["所属分公司"]==null?"": ds.Tables[0].Rows[0]["所属分公司"].ToString().Trim();
            tabf.Visible = true;
            tabn.Visible = true;
            tab2.Visible = true;
            tab3.Visible = true;

        }
        if (lbjyzhlx.Text.Trim() == "买家卖家交易账户")
        {
            ddlzhlb.Items.Add(new ListItem("请选择",""));
            ddlzhlb.Items.Add(new ListItem("卖方", "卖方"));
            ddlzhlb.Items.Add(new ListItem("买方", "买方"));
            tdjjrkf.Visible = true;
            txtjjrkfje.Visible = true;
        }
        if (lbjyzhlx.Text.Trim() == "经纪人交易账户")
        {
            ddlzhlb.Items.Add(new ListItem("经纪人", "经纪人"));
            txtjjrkfje.Visible = false;
            tdjjrkf.Visible = false;
        }


    }
    protected string GetSql(string jyfzh,string jyfmc )
    {
        return "select * from (select B_DLYX as 交易方账号,I_JYFMC as 交易方名称,B_JSZHLX as 交易账户类型,I_ZCLB as 注册类别, I_LXRXM as 联系人姓名,I_LXRSJH as 联系人手机号,isnull(I_SSQYS,'')+isnull(I_SSQYSHI,'')+isnull(I_SSQYQ,'') as 所属区域,(select GLJJRDLZH from AAA_MJMJJYZHYJJRZHGLB where DLYX=B_DLYX and SFDQMRJJR='是'and JJRSHZT='审核通过' and SFYX='是') as 关联经纪人账号 from AAA_DLZHXXB )a left join (select I_PTGLJG as 所属分公司, B_DLYX as 经纪人邮箱  from  AAA_DLZHXXB)b on a.关联经纪人账号=b.经纪人邮箱 where a.交易方账号='" + jyfzh + "' or a.交易方名称='" + jyfmc + "'";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
      
        
        //验证条件是否满足
        if (lbjyfzh.Text.Trim() == "")
        {
            MessageBox.Show(this,"请查询出要扣罚的交易账号！");
        }
        else if (lbjyzhlx.Text.Trim() == "" )
        {
            MessageBox.Show(this, "交易方尚未开通结算账户，不能进行处理！"); 
        }
        else if (ddlwgsx.SelectedValue.Trim() == "")
        {
            MessageBox.Show(this, "请选择违规事项！");  
        }
        else if(Filename=="")
        {
            MessageBox.Show(this, "请上传相应的凭证！");  
        }
        else if (txtjyfkfje.Text.Trim() == ""  )
        {
            MessageBox.Show(this, "请填写交易方扣罚金额！");  
 
        }
        else if (Convert.ToDouble(txtjyfkfje.Text.Trim()) == 0)
        {
            MessageBox.Show(this, "交易方扣罚金额不能为零！");  
        }
        else if (txtqkjs.Text.Trim() == "")
        {
            MessageBox.Show(this, "请填写情况简述！");

        }
        else if(ddlzhlb.SelectedValue.Trim()=="请选择")
        {
            MessageBox.Show(this, "请选择扣罚账户类别！");
        }
        else
        {
           object jyfobj= DbHelperSQL.GetSingle("select  B_ZHDQKYYE from  AAA_DLZHXXB  where B_DLYX='"+lbjyfzh.Text.Trim()+"'");
           if (Convert.ToDouble(txtjyfkfje.Text.Trim()) > Convert.ToDouble(jyfobj))
           {
               MessageBox.Show(this,"当前交易账户余额为"+jyfobj.ToString()+"，扣罚金额已超出！");
               return;
           }

           if (txtjjrkfje.Text.Trim() != "")
           {
               object jjrobj = DbHelperSQL.GetSingle("select  B_ZHDQKYYE from  AAA_DLZHXXB  where B_DLYX='" + lbgljjrzh.Text.Trim() + "'");
               if (Convert.ToDouble(txtjjrkfje.Text.Trim()) > Convert.ToDouble(jjrobj))
               {
                   MessageBox.Show(this, "当前关联经纪人余额为" + jjrobj.ToString() + "，扣罚金额已超出！");
                   return;
               }
 
           }
           List<string> list = new List<string>();

           WorkFlowModule WFM = new WorkFlowModule("AAA_WGKFB");
           string key = WFM.numberFormat.GetNextNumberZZ("");
            
           //如果不扣罚经纪人
           if (txtjjrkfje.Text.Trim() == "")
            {
                txtjjrkfje.Text = "0";
            }
           //写入违规扣罚表
           string str1 = "insert into [AAA_WGKFB]( [Number],[DLYX],[JSZHLX],[GLJJRYX],[GLJJRPTGLJG],[WGSX],[KFPZ],[KFJE],[JJRKFJE],[QKJS],[CheckState],[CreateUser],[CreateTime]) values('" + key + "','" + lbjyfzh.Text.Trim() + "','" + lbjyzhlx.Text.Trim() + "','" + lbgljjrzh.Text.Trim() + "','" + lbssfgs.Text.Trim() + "','" + ddlwgsx.SelectedValue.Trim() + "','" + Filename + "'," + txtjyfkfje.Text.Trim() + "," + txtjjrkfje.Text.Trim() + ",'" + txtqkjs.Text.Trim() + "',1,'" + User.Identity.Name + "',getdate())";
           list.Add(str1 );

            //写入资金流水明细
           string str2 = SqlInsertText("1304000041", lbjyfzh.Text.Trim(), "AAA_WGKFB", txtjyfkfje.Text.Trim(), key, key, ddlwgsx.SelectedValue.Trim(), ddlzhlb.SelectedValue.Trim());
           list.Add(str2);
            
            //更新登录账号表余额
           string str3 = "update AAA_DLZHXXB set B_ZHDQKYYE=B_ZHDQKYYE-"+txtjyfkfje.Text.Trim()+"  where B_DLYX='"+lbjyfzh.Text.Trim()+"'";
           list.Add(str3);

            //如果存在经纪人扣罚
           if (txtjjrkfje.Text.Trim() != "" && Convert.ToDouble(txtjjrkfje.Text.Trim())!=0)
           {
               string str4 = SqlInsertText("1304000041", lbgljjrzh.Text.Trim(), "AAA_WGKFB", txtjjrkfje.Text.Trim(), key, key, ddlwgsx.SelectedValue.Trim(), ddlzhlb.SelectedValue.Trim());
               list.Add(str4);

               //更新登录账号表余额
               string str5 = "update AAA_DLZHXXB set B_ZHDQKYYE=B_ZHDQKYYE-" + txtjjrkfje.Text.Trim() + "  where B_DLYX='" + lbgljjrzh.Text.Trim() + "'";
               list.Add(str5);
 
           }

            //更新数据发送提醒

           if (DbHelperSQL.ExecuteSqlTran(list) > 0)
           {
               List<Hashtable> ls = new List<Hashtable>();
               //交易账户提醒
               DataSet ds = DbHelperSQL.Query("SELECT B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,J_BUYJSBH AS 买家角色编号 FROM AAA_DLZHXXB WHERE B_DLYX='" + lbjyfzh.Text.Trim() + "'");

               Hashtable ht = new Hashtable();
               ht["type"] = "集合集合经销平台";
               ht["提醒对象登陆邮箱"] = lbjyfzh.Text.Trim();
               ht["提醒对象用户名"] = ds.Tables[0].Rows[0]["用户名"].ToString().Trim();
               ht["提醒对象结算账户类型"] = ds.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
               ht["提醒对象角色编号"] = ds.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
               ht["提醒对象角色类型"] = ddlzhlb.SelectedValue.Trim();
               ht["提醒内容文本"] = "依据平台相关规定，现认定您存在“" + ddlwgsx.SelectedValue.Trim() + "”的违规行为，平台将对您作出“"+double.Parse(txtjyfkfje.Text.Trim()).ToString("#0.00")+"元”的处罚。如有异议，请即致电平台服务人员！";
               ht["创建人"] = User.Identity.Name;
             
               ls.Add(ht);

               //经纪人提醒
               if (txtjjrkfje.Text.Trim() != "" && Convert.ToDouble(txtjjrkfje.Text.Trim())!=0)
               {
                   DataSet db = DbHelperSQL.Query("SELECT B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,J_BUYJSBH AS 买家角色编号 FROM AAA_DLZHXXB WHERE B_DLYX='" + lbgljjrzh.Text.Trim() + "'");

                   Hashtable jjrht = new Hashtable();
                   jjrht["type"] = "集合集合经销平台";
                   jjrht["提醒对象登陆邮箱"] = lbgljjrzh.Text.Trim();
                   jjrht["提醒对象用户名"] = db.Tables[0].Rows[0]["用户名"].ToString().Trim();
                   jjrht["提醒对象结算账户类型"] = db.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
                   jjrht["提醒对象角色编号"] = db.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
                   jjrht["提醒对象角色类型"] = "买家";
                   jjrht["提醒内容文本"] = "依据平台相关规定，现认定您存在“" + ddlwgsx.SelectedValue.Trim() + "”的违规行为，平台将对您作出“" +double.Parse(txtjjrkfje.Text.Trim()).ToString("#0.00") + "元”的处罚。如有异议，请即致电平台服务人员！";
                   jjrht["创建人"] = User.Identity.Name;

                   ls.Add(jjrht);
 
               }               

               JHJX_SendRemindInfor.Sendmes(ls);

               MessageBox.Show(this, "数据提交成功！");

               MessageBox.ShowAndRedirect(this, "数据更新成功", "JHJX_JYFWGKFTJ.aspx");
              

           }
           else
           {
               MessageBox.Show(this, "数据提交失败");
           }

 
        }

    }
    /// <summary>
    /// 获取写入账款流水明细表的SQL语句
    /// </summary>
    /// <param name="num">对照表主键编号</param>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="mname">模块名</param>
    /// <param name="je">金额</param>
    /// <param name="lydh">来源单号</param>
    /// <param name="replacex1">替代字符串</param>
    /// <param name="replacex2">替代字符串</param>
    /// <returns></returns>
    protected string SqlInsertText(string num, string dlyx, string mname, string je, string lydh, string replacex1, string replacex2, string zhxz)
    {
        WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
        string key = WFM.numberFormat.GetNextNumberZZ("");

        DataTable dtemp1 = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='" + num + "'").Tables[0];
        string xm = dtemp1.Rows[0]["XM"].ToString().Trim();
        string xz = dtemp1.Rows[0]["XZ"].ToString().Trim();
        string zy = dtemp1.Rows[0]["ZY"].ToString().Trim().Replace("[x1]", replacex1).Replace("[x2]", replacex2);
        string sjlx = dtemp1.Rows[0]["SJLX"].ToString().Trim();
        string yslx = dtemp1.Rows[0]["YSLX"].ToString().Trim();

        DataTable dtemp2 = DbHelperSQL.Query("select B_JSZHLX, J_SELJSBH,J_BUYJSBH,J_JJRJSBH from AAA_DLZHXXB  where B_DLYX='" + dlyx + "'").Tables[0];
        string jszhlx = dtemp2.Rows[0]["B_JSZHLX"].ToString().Trim();

        string jsbh = "";
        if (zhxz == "买方" && dtemp2.Rows[0]["J_BUYJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_BUYJSBH"].ToString().Trim();
        }
        else if (zhxz == "卖方" && dtemp2.Rows[0]["J_SELJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_SELJSBH"].ToString().Trim();
        }
        else if (zhxz == "经纪人" && dtemp2.Rows[0]["J_JJRJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_JJRJSBH"].ToString().Trim();
        }


        string str = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + dlyx + "','" + jszhlx + "','" + jsbh + "','" + mname + "','" + lydh + "',getdate(),'" + yslx + "'," + je + ",'" + xm + "','" + xz + "','" + zy + "','接口编号','" + sjlx + "',0,'" + User.Identity.Name.ToString() + "',GETDATE())";

        dtemp1.Dispose();
        dtemp2.Dispose();

        return str;

    }
   

    protected void btnCancle_Click(object sender, EventArgs e)
    {
        if (Filename != "")
        {
            string Path = Server.MapPath("docs\\" + Filename);
            File.Delete(Path);
            Filename = "";
            this.Lablsit.Text = "";

        }
      
        Response.Redirect("JHJX_JYFWGKFTJ.aspx");
    }
    protected void BtnSC_Click(object sender, EventArgs e)
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
            Filename = User.Identity.Name + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + fileshortName;

            this.Lablsit.ToolTip = Filename;

            string path = keyEncryption.encMe("/Web/JHJX/New2013/JHJX_JYKF/docs/" + Filename, "mimamima");
            Lablsit.Text = "<a href = '../../../picView/picView.aspx?path=" + path + "&jiami=y'  target='_blank'>" + fileshortName + "</a>";
           // this.Lablsit.Text = "<a href = 'docs/" + Filename + "'  target='_blank'>" + fileshortName + "</a>";
            this.FileUpload1.PostedFile.SaveAs(StrRealPath + "\\" + Filename);

            FMOP.Common.MessageBox.Show(Page, "上传成功！");
            //this.fujian.Visible = true;
            trpz.Visible = true;
            this.Lablsit.Visible = true;


            #region 上传到其他服务器的语句，备用
            ////上传接收处理文件路径
            //string url = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"] + "/JHJXPT/shangchuan.aspx";

            //int max = 0;

            //    //生成新文件名
            //    string nfilename = Guid.NewGuid().ToString();

            //    //生成拓展名
            //    string extstr = Path.GetExtension(FileUpload1.FileName.ToString()).ToLower();

            //    max = max + Upload_Request(url, FileUpload1.PostedFile.FileName.ToString(), nfilename + extstr);

            #endregion
          
        }
        else
        {

            FMOP.Common.MessageBox.Show(Page, "请选择需要上传的文件！");
        }
    }



    // <summary> 
    /// 将本地文件上传到指定的服务器(HttpWebRequest方法) 
    /// </summary> 
    /// <param name="address">文件上传到的服务器</param> 
    /// <param name="fileNamePath">要上传的本地文件（全路径）</param> 
    /// <param name="saveName">文件上传后的名称</param> 
    /// <returns>成功返回1，失败返回0</returns> 
    private int Upload_Request(string address, string fileNamePath, string saveName)
    {
        int returnValue = 0;

        // 要上传的文件 
        FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
        BinaryReader r = new BinaryReader(fs);

        //时间戳 
        string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
        byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");

        //请求头部信息 
        StringBuilder sb = new StringBuilder();
        sb.Append("--");
        sb.Append(strBoundary);
        sb.Append("\r\n");
        sb.Append("Content-Disposition: form-data; name=\"");
        sb.Append("file");
        sb.Append("\"; filename=\"");
        sb.Append(saveName);
        sb.Append("\"");
        sb.Append("\r\n");
        sb.Append("Content-Type: ");
        sb.Append("application/octet-stream");
        sb.Append("\r\n");
        sb.Append("\r\n");
        string strPostHeader = sb.ToString();
        byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);

        // 根据uri创建HttpWebRequest对象 
        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
        httpReq.Method = "POST";

        //对发送的数据不使用缓存 
        httpReq.AllowWriteStreamBuffering = false;

        //设置获得响应的超时时间（300秒） 
        httpReq.Timeout = 300000;
        httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
        long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
        long fileLength = fs.Length;
        httpReq.ContentLength = length;
        try
        {

            //每次上传4k 
            int bufferLength = 4096;
            byte[] buffer = new byte[bufferLength];

            //已上传的字节数 
            long offset = 0;

            //开始上传时间 
            DateTime startTime = DateTime.Now;
            int size = r.Read(buffer, 0, bufferLength);
            Stream postStream = httpReq.GetRequestStream();

            //发送请求头部消息 
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            while (size > 0)
            {
                postStream.Write(buffer, 0, size);
                offset += size;
                TimeSpan span = DateTime.Now - startTime;
                double second = span.TotalSeconds;
                size = r.Read(buffer, 0, bufferLength);
            }
            //添加尾部的时间戳 
            postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            postStream.Close();

            //获取服务器端的响应 
            WebResponse webRespon = httpReq.GetResponse();
            Stream s = webRespon.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            //读取服务器端返回的消息 
            String sReturnString = sr.ReadLine();
            s.Close();
            sr.Close();
            string bz = sReturnString.Split('*')[0];
            if (bz == "Success")
            {
                returnValue = 1;
            }
            else if (bz == "Error")
            {
                returnValue = 0;
            }

        }
        catch (Exception ex)
        {
            string aaaa = ex.ToString();
            returnValue = 0;
        }
        finally
        {
            fs.Close();
            r.Close();
        }
        return returnValue;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected void Initial()
    {
        lbjyfzh.Text = "";
        lbjyfmc.Text = "";
        Filename = "";
        lbgljjrzh.Text = "";
        lbjyzhlx.Text = "";
        lbzclb.Text = "";
        lblxrsjh.Text = "";
        lblxrxm.Text = "";
        lbssfgs.Text = "";
        lbssqy.Text = "";
        ddlwgsx.SelectedIndex = 0;
        ddlzhlb.Items.Clear();
        txtjjrkfje.Text = "";
        txtjyfkfje.Text = "";
        txtqkjs.Text = "";
        ddlzhlb.Items.Clear();
        tab2.Visible = false;
        tab3.Visible = false;
        tabf.Visible = false;
        tabn.Visible = false;

        this.Lablsit.Text = "";
        trpz.Visible = false;

    }


    protected void BtnQC_Click(object sender, EventArgs e)
    {
        if (Filename != "")
        {
            string Path = Server.MapPath("docs\\" + Filename);
            File.Delete(Path);
            Filename = "";
            this.Lablsit.Text = "";

            MessageBox.Show(this, "删除成功!");
            trpz.Visible = false;
        }
        else
        {
            MessageBox.Show(this, "并未找到上传的文件！"); 
        }
     

    }
}