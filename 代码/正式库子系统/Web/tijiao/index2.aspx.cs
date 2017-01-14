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
using FMOP.DB;
using System.Text;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_tijiao_index2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        if (!Page.IsPostBack)
        {
            ///*初始化页面*/
            drbangding();
            InnitPage();
        }
    }
    //会议室下拉框绑定
    private void drbangding()
    {

        ddl_hydd.DataSource = DbHelperSQL.Query("SELECT distinct  HYSMC FROM ZCBGS_HYSXD");
        ddl_hydd.DataTextField = "HYSMC";
        ddl_hydd.DataValueField = "HYSMC";
        ddl_hydd.DataBind();
        ddl_hydd.Items.Insert(0, "所有");
    }
    private void InnitPage()
    {
        string Str_HY = "";
        if (txtHYStartTime.Text != "" && txtHYEndTime.Text != "")
        {
            try
            {
                if (Convert.ToDateTime(txtHYStartTime.Text.Trim()) > Convert.ToDateTime(txtHYEndTime.Text.Trim()))
                {
                    MessageBox.Show(this, "会议开始时间不能大于会议结束时间");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "时间转换可能有错");
                return;
            }
        }
        Str_HY = "select ID ,HYZT ,HYZBF ,HYZCR,HYDD,convert(varchar(16),HYKSSJ,121) as HYKSSJ,convert(varchar(16),HYJSSJ,121) as HYJSSJ,HYSQRGH,HYSQRXM,SFYX,SQTJSJ,Ischeck,Remark,";
        Str_HY = Str_HY + "(case when datediff(n,getdate(),HYKSSJ)>0 and SFYX='有效' then '尚未召开' when datediff(n,getdate(),HYJSSJ)<0  and SFYX='有效'then '已经结束' when datediff(n,getdate(),HYJSSJ)>0 and datediff(n,getdate(),HYKSSJ)<=0 and SFYX='有效' then '正在召开'when SFYX='撤销' then '已经撤销' end ) as HYDQZT,";
        Str_HY = Str_HY + "(case  when sfyx='有效' and  datediff(n,getdate(),HYKSSJ)>0 then  CAST((datediff(n,getdate(),HYKSSJ)/60.0) as decimal(9,2)) when sfyx='有效' and datediff(n,getdate(),HYKSSJ)<=0 then null end ) as HYDJS ";
        Str_HY = Str_HY + " from HYGL_HYSSQDJ where 1=1 and HYZT like '%" + this.tb_hyzt.Text.Trim() + "%' and HYZBF like '%" + tb_hyzbf.Text.Trim() + "%' and HYZCR like '%" + tb_hyzcr.Text.Trim() + "%' and HYSQRXM like '%" + tb_hysqrxm.Text.Trim() + "%'";
        if (txtHYStartTime.Text != "")
        {
            Str_HY = Str_HY + " and HYKSSJ>='" + txtHYStartTime.Text + "'";
        }
        if (this.txtHYEndTime.Text != "")
        {
            Str_HY = Str_HY + " and HYJSSJ <= '" + txtHYEndTime.Text + "'";
        }
        if (tb_sqtjsj.Text != "")
        {
            Str_HY = Str_HY + " and SQTJSJ='" + tb_sqtjsj.Text + "'";
        }
        if (ddl_hydd.Text == "所有")
        {

        }
        else
        {
            Str_HY = Str_HY + " and HYDD='" + ddl_hydd.Text.Trim() + "'";
        }
        if (ddl_sfyx.Text == "所有")
        {

        }
        else
        {
            Str_HY = Str_HY + " and SFYX='" + ddl_sfyx.Text.Trim() + "'";
        }
        if (ddl_hydqzt.Text == "所有")
        {

        }
        else
        {
            Str_HY = Str_HY + " and SFYX='" + ddl_hydqzt.Text.Trim() + "'";
        }

        Str_HY = Str_HY + "  order by SQTJSJ DESC";
        //Response.Write(sql);

        DataSet ds = DbHelperSQL.Query(Str_HY);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["Ischeck"].ToString() == "1")
            {
                dr["Ischeck"] = "是";
            }
            else
            {
                dr["Ischeck"] = "否";
            }
            if (dr["Remark"].ToString().Length > 6)
            {

                dr["Remark"] = dr["Remark"].ToString().Remove(5) + ".....";
            }
        }
        this.RadGrid1.DataSource = ds.Tables[0];
        this.RadGrid1.DataBind();
    }
    protected void RadGrid1_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        InnitPage();
    }
    protected void lbtnCX_Click(object sender, EventArgs e)
    {
        /*会议撤销*/
        try
        {
            LinkButton lbtn = sender as LinkButton;
            Telerik.WebControls.GridDataItem gdi = lbtn.Parent.Parent as Telerik.WebControls.GridDataItem;
            string ID = gdi.Cells[2].Text.ToString().Trim();
            string TishiText = gdi.Cells[6].Text.ToString().Trim();
            string sfcx = gdi.Cells[11].Text.ToString().Trim();
            string sqr = gdi.Cells[9].Text.ToString().Trim();
            string UserID = User.Identity.Name.ToString();
            if (sfcx == "撤销")
            {
                FMOP.Common.MessageBox.Show(Page, TishiText + "申请已被撤销，请不要重复操作！");
            }
            else
            {
                string Strsql = "update HYGL_HYSSQDJ set SFYX = '撤销' where  ID = " + ID;
                DbHelperSQL.QueryInt(Strsql);
                Authentication auth = new Authentication();
                //auth.InsertWarnings(TishiText + "的申请已被撤销", "CustormModule.aspx?module=HYGL_QGSHYGL", "1", User.Identity.Name.ToString(), "6860696");
                //如果是办公室人员撤销，提醒申请人
                if (UserID != sqr)
                {
                    auth.InsertWarnings(TishiText + "的申请已被撤销", "CustormModule.aspx?module=HYGL_BBMHYGL", "1", User.Identity.Name.ToString(), sqr);
                }
                InnitPage();
                FMOP.Common.MessageBox.Show(Page, TishiText + "申请已被撤销！");
            }
        }
        catch (Exception ex)
        {
            FMOP.Common.MessageBox.Show(Page, "程序出现如下错误：" + ex.Message.ToString());
        }
    }
    protected void btnTJ_Click(object sender, EventArgs e)
    {
        this.Panel1.Visible = !this.Panel1.Visible;
        if (this.Panel1.Visible == true)
        {
            btnTJ.Text = "隐藏查询条件";
        }
        else
        {
            btnTJ.Text = "显示查询条件";
        }
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        InnitPage();
    }



    /// <summary>
    /// 审核按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnCHeck_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lbtn = sender as LinkButton;
            Telerik.WebControls.GridDataItem gdi = lbtn.Parent.Parent as Telerik.WebControls.GridDataItem;
            string ID = gdi.Cells[2].Text.ToString().Trim();
            string strzhuti = gdi.Cells[3].Text.ToString().Trim();
            string strDept = gdi.Cells[4].Text.ToString().Trim();
            string Strdidian = gdi.Cells[6].Text.ToString().Trim();

            string StrSql = " Update HYGL_HYSSQDJ set Ischeck = 1 where id ='" + ID.Trim() + "'";
            int a = DbHelperSQL.ExecuteSql(StrSql);
            if (a > 0)
            {
                FMOP.Common.MessageBox.Show(Page, strzhuti + "会议申请已被审核通过！");
                InnitPage();
            }

        }
        catch
        {
            FMOP.Common.MessageBox.Show(Page,  "系统正在维护请及时与管理员联系！");
        }
    }

    /// <summary>
    /// 备注按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnRemark_Click(object sender, EventArgs e)
    {
        LinkButton lbtn = sender as LinkButton;
        Telerik.WebControls.GridDataItem gdi = lbtn.Parent.Parent as Telerik.WebControls.GridDataItem;
        hidID.Value = gdi.Cells[2].Text.ToString().Trim();
        labsmtitle.Text = gdi.Cells[3].Text.ToString().Trim();
        labsmid.Text = gdi.Cells[4].Text.ToString().Trim();
        string Strdidian = gdi.Cells[6].Text.ToString().Trim();
        divStop.Visible = true;
    }

    /// <summary>
    /// 备注信息保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnStopSave_Click(object sender, EventArgs e)
    {
        try
        {
            string StrSql = " update HYGL_HYSSQDJ set Remark='" + txtStopidear.Text.Trim() + "' where id='" + this.hidID.Value.ToString().Trim() + "'";
            int a = DbHelperSQL.ExecuteSql(StrSql);
            if (a > 0)
            {
                FMOP.Common.MessageBox.Show(Page, labsmtitle.Text + "备注添加成功！");
                divStop.Visible = false;
                InnitPage();
            }
        }
        catch
        {
            FMOP.Common.MessageBox.Show(Page, "系统正在维护请及时与管理员联系！");
            divStop.Visible = false;
        }
    }
    /// <summary>
    /// 取消按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Btnstopcancel_Click(object sender, EventArgs e)
    {
        this.divStop.Visible = false;
        txtStopidear.Text = "";
    }
}
