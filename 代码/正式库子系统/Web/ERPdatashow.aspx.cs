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
using Telerik.WebControls;

public partial class Web_ERPdatashow : System.Web.UI.Page
{
    string StrSql = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.RadGridBind();
            //BindDDL();
        }
    }



    protected void RadGridBind()
    {
        try
        {
            //"where TG023 = 'N' "  啊..这里写的比较笨哈哈，大家有好的方法可以修改修改哈.
          
            if (this.hiddanbie.Value.ToString() == "" && this.hiddanhao.Value.ToString() == "" && this.hidshadress.Value.ToString() == "" && this.txtArea.Text.ToString() == "")
            {
                StrSql = " select a.TG001,a.TG002,(replace(a.TG008,'','')+','+a.TG009)AS TGADDRESS,a.TG020,b.MA003 from COPTG as a left join COPMA as b ON a.TG004 = b.MA001 " + " where a.TG023 = 'Y'";
            }
            else
            {
                StrSql = "select a.TG001,a.TG002,(replace(a.TG008,'','')+','+a.TG009)AS TGADDRESS,a.TG020,b.MA003 from COPTG as a left join COPMA as b ON a.TG004 = b.MA001 WHERE a.TG023 = 'Y' and a.TG001 like '%"
                         + this.hiddanbie.Value.ToString() + "%'and a.TG002 LIKE '%" + this.hiddanhao.Value.ToString() + "%' AND a.TG008 LIKE '%" + this.hidshadress.Value.ToString()
                         + "%' and b.MA003 LIKE '%" + this.txtArea.Text.ToString() + "%'";
                //and TG009 LIKE '%" + this.hidshadress.Value.ToString() + "%'";
            }

            //if (StrSql != null && StrSql != "")
            //{
            //    StrSql += " where TG023 = 'N'";
            //}
            DataSet ds = GetErpData.GetXHD(StrSql);

            this.RadGrid1.DataSource = ds;
            this.RadGrid1.DataBind();
        }
        catch (Exception e)
        {
            Hesion.Brick.Core.Log.Error(e.Message.ToString());
            FMOP.Common.MessageBox.Show(Page, "程序出现如下错误：" + e.Message.ToString());
          

        }

    }


    //protected void BindDDL()
    //{
    //    try
    //    {
    //        StrSql = "select DISTINCT TG001 from COPTG ";
    //        DataSet ds = GetErpData.GetXHD(StrSql);

    //        ddldanbie.DataSource = ds;
    //        ddldanbie.DataTextField = "TG001";
    //        ddldanbie.DataValueField = "TG001";
    //        ddldanbie.DataBind();
    //        ListItem lt = new ListItem("--请选择单别--", "0");
    //        ddldanbie.Items.Insert(-1, lt);
           

    //        string StrSql2 = "select DISTINCT TG002 from COPTG ";
    //        DataSet ds2 = GetErpData.GetXHD(StrSql2);

    //        ddldanhao.DataSource = ds2;
    //        ddldanhao.DataTextField = "TG002";
    //        ddldanhao.DataValueField = "TG002";
    //        ddldanhao.DataBind();
    //        ListItem ltt = new ListItem("--请选择单号--","0");
    //        ddldanhao.Items.Insert(-1, ltt);
    //    }
    //    catch (Exception e)
    //    {
    //        Hesion.Brick.Core.Log.Error(e.Message.ToString());
    //        FMOP.Common.MessageBox.Show(Page, "程序出现如下错误：" + e.Message.ToString());
    //    }


    //}

   

    protected void EditButton_Click(object sender, EventArgs e)
    {
        this.hiddanbie.Value = this.txtdanbie.Text.ToString();
        this.hiddanhao.Value = this.txtdanhao.Text.ToString() ;
        this.hidshadress.Value = this.txtshouhuoAdress.Text.ToString();
        RadGridBind();
    }
    protected void RadGrid1_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
        RadGridBind();
       
        
    }
    protected void btnChildTable_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btn = sender as ImageButton;


            //string Name = btn.Parent.GetType().FullName.ToString();
            //string Name1 = btn.Parent.Parent.GetType().FullName.ToString();
      
            Telerik.WebControls.GridDataItem gdi = btn.Parent.Parent as Telerik.WebControls.GridDataItem;


            //取得当前行的单别和单号
            string danbie = gdi.Cells[3].Text.ToString();
            string danhao = gdi.Cells[4].Text.ToString();
            DataSet ds = GetTable(danbie, danhao);
            if (ds != null)
            {
                RadGrid2.DataSource = ds;
                RadGrid2.DataBind();
                this.divChildTable.Visible = true;

            }
         
        }
        catch (Exception ex)
        {
            Hesion.Brick.Core.Log.Error(ex.Message.ToString());
            FMOP.Common.MessageBox.Show(Page, "程序出现如下错误：" + ex.Message.ToString());
        }

    }

    public DataSet GetTable(string danbie, string danhao)
    {
        DataSet ds = null;
        if (danbie != "" && danbie != null && danhao != "" && danhao != "")
        {
            StrSql = "select (replace(TH001,'','')+'-'+ replace(TH002,'',''))AS XIAOHUODANHAO ,TH004,TH005,TH006,TH012,TH013,TH017,b.TG020,TH009,cast(a.TH008 as int) as XHSL,(replace(b.TG008,'','')+','+b.TG009)AS TGADDRESS,c.MA003 from COPTG as b left join COPTH as a on a.TH001=b.TG001 AND a.TH002 = b.TG002 left join COPMA as c on b.TG004 = c.MA001  where a.TH001 = '" + danbie + "' AND a.TH002 = '" + danhao + "'";
            ds = GetErpData.GetXHD(StrSql);
        
        }

        return ds;
             
    }



     
}
