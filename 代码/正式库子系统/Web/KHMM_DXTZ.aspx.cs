using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.IO;
using FMOP.DB;
using ShareLiu;
using ShareLiu.SwClasses;
using ShareLiu.DXS;
public partial class Web_KHMM_DXTZ : System.Web.UI.Page
{
    
    ArrayList al = null;
    SendDX sendPhone = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        string str_Number ="select * from KHGL_KHMMGL where Number ='" + Request["number"].ToString()+"'";
        DataSet ds = FMOP.DB.DbHelperSQL.Query(str_Number);
        string khbh = "";
        string khmm = "";
        string dxnr = "";
        string khsjhm = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            khbh = ds.Tables[0].Rows[0]["KHBH"].ToString();
            khmm = ds.Tables[0].Rows[0]["KHMM"].ToString();
        }
        string StrSql="select * from KHGL_New where Number='"+khbh+"'";
        DataSet ds_sj = FMOP.DB.DbHelperSQL.Query(StrSql);
        if (ds_sj.Tables[0].Rows.Count > 0)
        {
            khsjhm = ds_sj.Tables[0].Rows[0]["LXSJ"].ToString();
            if (khsjhm != "" && khsjhm != null)
            {
                this.sjhm.Text=khsjhm;
            }
        }
        
        dxnr= "即日起，您可通过富美官网产品与品质服务模块查询发货详情；";
        dxnr += "登录编号"+khbh+"，";
        dxnr += "密码"+khmm+"，";
        dxnr += "详情可咨询客服热线。富美科技。";
        this.txtDXNR.Text = dxnr;

    }
    protected void Btnsave_Click(object sender, EventArgs e)
    {
        SaveAttributes();

        
    }
    private void SaveAttributes()
    {
        try
        {
            string khsj = this.sjhm.Text.ToString();
            if (khsj == "" || khsj == null)
            {
                //MessageBox.Show(this, "客户手机号码为空，不能发送短信！");
                FMOP.Common.MessageBox.Show(Page, "客户手机号码为空，不能发送短信！");
                return;
            }
            else
            {



                //***********************************短信测试*********************************************************************************************

                DateTime dt = DateTime.Now;
                User usinfo = Users.GetUserByNumber(User.Identity.Name.Trim());
                string StrMess = this.txtDXNR.Text.ToString();
                sendPhone = new SendDX();
                sendPhone.SendToM(khsj, StrMess);

                //*******************************************************************************************************************************************


                ArrayList al = new ArrayList();
                string Sql = "update KHGL_KHMMGL set SFTZ='已通知',FSKHSJHM='" + khsj + "',FSR='" + User.Identity.Name.Trim() + "',FSSJ= '" + DateTime.Now.ToString() + "' where Number ='" + Request["number"].ToString() + "'";

                al.Add(Sql);

                DbHelperSQL.ExecuteSqlTran(al);
                Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('发送完成！');window.top.frames['rightFrame'].my_closediag();window.top.frames['rightFrame'].location.reload();</script>");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            FMOP.Common.MessageBox.Show(Page, "系统出错，请及时联系管理员！");
            return;
        }

    }
    protected void BtnQX_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.top.frames['rightFrame'].my_closediag();</script>");
        Response.End();
    }
}
