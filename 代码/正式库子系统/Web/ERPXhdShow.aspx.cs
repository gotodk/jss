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
using FMOP.DB;

public partial class Web_ERPXhdShow: System.Web.UI.Page
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
        string startTime, endTime;
        string sqlWhere="";
        if (txtStartTime.Text != "")
        {
         startTime =txtStartTime.Text.Substring(0,4)+txtStartTime.Text.Substring(5,2)+txtStartTime.Text.Substring(8,2);
         sqlWhere=sqlWhere+" and cast(TG003 as int)>='"+startTime.Trim()+"'";
        }
        if(txtEndTime.Text!="")
        {
            endTime=txtEndTime.Text.Substring(0,4)+txtEndTime.Text.Substring(5,2)+txtEndTime.Text.Substring(8,2);
            sqlWhere=sqlWhere+" and cast(TG003 as int)<='"+endTime.Trim()+"'";
        }
        
        endTime = txtEndTime.Text.Trim();
        try
        {

            if (this.hiddanbie.Value.ToString() == "" && this.hiddanhao.Value.ToString() == "" && this.hidshadress.Value.ToString() == "" && this.txtArea.Text.ToString() == "")
            {
                StrSql = " select a.TG001,a.TG002,a.TG003,(replace(a.TG008,'','')+','+a.TG009)AS TGADDRESS,a.TG020,b.MA003,a.TG008,a.TG009 from COPTG as a left join COPMA as b ON a.TG004 = b.MA001 " + " where a.TG023 = 'Y'";
            }
            else
            {
                StrSql = "select a.TG001,a.TG002,a.TG003,(replace(a.TG008,'','')+','+a.TG009)AS TGADDRESS,a.TG020,b.MA003,a.TG008,a.TG009 from COPTG as a left join COPMA as b ON a.TG004 = b.MA001 WHERE a.TG023 = 'Y' and a.TG001 like '%"
                         + this.hiddanbie.Value.ToString() + "%'and a.TG002 LIKE '%" + this.hiddanhao.Value.ToString() + "%' AND a.TG008 LIKE '%" + this.hidshadress.Value.ToString()
                         + "%' and b.MA003 LIKE '%" + this.txtArea.Text.ToString() + "%'";
            }
           StrSql = StrSql + sqlWhere;
        //主窗口子表ds

            string mainStrSql = "select XHDDB,XHDDH from YDXHD";
            DataSet dsSun = DbHelperSQL.Query(mainStrSql);
            string sonSql = " and (";
            if (dsSun.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i <= dsSun.Tables[0].Rows.Count-1; i++)
                {
                    sonSql = sonSql + " (a.TG001<>'" + dsSun.Tables[0].Rows[i]["XHDDB"].ToString() + "' and a.TG002<>'" + dsSun.Tables[0].Rows[i]["XHDDH"].ToString() + "')";

                    if (i < dsSun.Tables[0].Rows.Count-1)
                    {
                        sonSql = sonSql + " or ";
                    }
                    else
                    {
                        sonSql = sonSql + ")";
                    }


                }
            }
            else
            {
                sonSql = sonSql + "1=1)";
            }

           
            StrSql = StrSql + sonSql;
            
            //Response.Write(StrSql);
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
            string danbie = gdi.Cells[4].Text.ToString();
            string danhao = gdi.Cells[5].Text.ToString();
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
            StrSql = "select (replace(TH001,'','')+'-'+ replace(TH002,'',''))AS XIAOHUODANHAO ,TH004,TH005,TH006,TH012,TH013,TH017,b.TG020,TH009,cast(a.TH008 as int) as XHSL,(replace(b.TG008,'','')+','+b.TG009)AS TGADDRESS,c.MA003,TH018 from COPTG as b left join COPTH as a on a.TH001=b.TG001 AND a.TH002 = b.TG002 left join COPMA as c on b.TG004 = c.MA001  where a.TH001 = '" + danbie + "' AND a.TH002 = '" + danhao + "'";
            ds = GetErpData.GetXHD(StrSql);
        
        }

        return ds;
             
    }
    protected void cbAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbAll = sender as CheckBox;
        foreach (Telerik.WebControls.GridDataItem gdi in this.RadGrid1.Items)
        {

            CheckBox cb = gdi.FindControl("checkbox") as CheckBox;


            cb.Checked = cbAll.Checked;
        }
    }




    protected void btnOK_Click(object sender, EventArgs e)
    {      
        foreach (Telerik.WebControls.GridDataItem gdi in this.RadGrid1.Items)
        {
            CheckBox cb = gdi.FindControl("checkbox") as CheckBox;
            
            if (gdi.Cells[4].Text.ToString().Trim() != "" &&gdi.Cells[5].Text.ToString().Trim()!=""&& cb.Checked == true)
            {
                string thisDate = gdi.Cells[6].Text.ToString().Trim();
                //Response.Write("<script>parent.opener.document.getElementById('YDXHDERPXHDDH').value='" + gdi.Cells[4].Text.ToString().Trim() + "-" + gdi.Cells[5].Text.ToString().Trim() + "';</script>");
                Response.Write("<script>parent.opener.document.getElementById('YDXHDXHDDB').value='" + gdi.Cells[4].Text.ToString().Trim() + "';</script>");
                Response.Write("<script>parent.opener.document.getElementById('YDXHDXHDDH').value='" + gdi.Cells[5].Text.ToString().Trim() + "';</script>");
                Response.Write("<script>parent.opener.document.getElementById('YDXHDXHRQ').value='" + thisDate.Substring(0, 4).ToString() + "-" + thisDate.Substring(4, 2).ToString() + "-" + thisDate.Substring(6, 2).ToString() + "';</script>");
                //Response.Write("<script>parent.opener.YDXHDaddTableRow('YDXHDERPXHDDH,YDXHDFHRQ','YDXHD','YDXHDERPXHDDH:TextBox:False:::50.YDXHDFHRQ:DateSelectBox:False:::50');</script>");
                Response.Write("<script>parent.opener.YDXHDaddTableRow('YDXHDXHDDB,YDXHDXHDDH,YDXHDXHRQ','YDXHD','YDXHDXHDDB:TextBox:False:::50.YDXHDXHDDH:TextBox:False:::50.YDXHDXHRQ:DateSelectBox:False:::50');</script>");
                Response.Write("<script>window.close();</script>");
            }

        }
    }
    protected void Btnhide_Click(object sender, EventArgs e)
    {

        Panal1.Visible = !Panal1.Visible;
        if (Panal1.Visible==true)
        {
            (sender as Button).Text = "隐藏主表";
            btnOK.Visible = true;
        }
        else
        {
            (sender as Button).Text = "显示主表";
            btnOK.Visible = false;
        }

    }
}
