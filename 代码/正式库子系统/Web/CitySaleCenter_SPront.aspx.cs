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
using System.Data.SqlClient;

public partial class Web_CitySaleCenter_SPront : System.Web.UI.Page
{
     string NewYHSQDH = "" ;
     string NewCsxsgs = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bindDDL();
            BindRagGrid();
        }
    }

    /// <summary>
    /// 绑定城市销售中心内容
    /// </summary>
    private void bindDDL()
    {
        try
        {
            string StrSql = "select Number, name from system_city";
            DataSet ds = DbHelperSQL.Query(StrSql);

            this.DDlSaleCity.DataSource = ds;

            this.DDlSaleCity.DataTextField = "name";
            this.DDlSaleCity.DataValueField = "name";
            this.DDlSaleCity.DataBind();
            ListItem lis = new ListItem("--请选择城市--","-100");
            this.DDlSaleCity.Items.Insert(0,lis);
        }
        catch (Exception e)
        {
            Hesion.Brick.Core.Log.Error(e.Message.ToString());
            FMOP.Common.MessageBox.Show(Page, "程序出现如下错误：" + e.Message.ToString());
        }

    }


   /// <summary>
    /// 绑定审核通过的要货申请单
   /// </summary>
   /// <param name="YHSQDH">要货申请单号</param>
   /// <param name="XSGS">城市销售公司</param>
    private void BindRagGrid()
    {

        try
        {
            string StrSql = "" ;
            if (NewYHSQDH == "" && NewCsxsgs == "-100")
            {
                StrSql = "select * from CitySaleCenter_SApp where CheckState = 1";

            }
            else
            {
                if (NewYHSQDH == "" && NewCsxsgs != "-100" && NewCsxsgs != null)
                {
                    StrSql = "select * from CitySaleCenter_SApp where CheckState = 1 and  CSXSGS like '%" + NewCsxsgs + "%'";
                }
                if (NewCsxsgs == "-100" && NewYHSQDH != "" && NewYHSQDH != null)
                {
                    StrSql = "select * from CitySaleCenter_SApp where CheckState = 1 and  YHSQDH like '%" + NewYHSQDH + "%'";
                }
                if (NewCsxsgs != "-100" && NewYHSQDH != "" && NewYHSQDH != null && NewCsxsgs != null)
                {
                    StrSql = "select * from CitySaleCenter_SApp where CheckState = 1 and  YHSQDH like '%" + NewYHSQDH + "%' and  CSXSGS like '%" + NewCsxsgs + "%'";
                }
            }
            DataSet ds = DbHelperSQL.Query(StrSql);
            this.RadGrid1.DataSource = ds;
            this.RadGrid1.DataBind();

        }
        catch (Exception e)
        {
            Hesion.Brick.Core.Log.Error(e.Message.ToString());
            FMOP.Common.MessageBox.Show(Page, "程序出现如下错误：" + e.Message.ToString());
        }
       
    }
    protected void EditButton_Click(object sender, EventArgs e)
    {
         NewYHSQDH = this.TXTYHSQDH.Text.ToString();
         NewCsxsgs = this.DDlSaleCity.SelectedValue.ToString();
         BindRagGrid();
    }
    protected void RadGrid1_NeedDataSource(object source, Telerik.WebControls.GridNeedDataSourceEventArgs e)
    {
        
    }
    protected void RadGrid1_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
        BindRagGrid();
    }
}
