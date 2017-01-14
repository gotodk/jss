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

public partial class Web_HR_YGXX_Export_LZ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UltraWebGrid1.DataBind();        
    }

    // 导出员工基本信息
    #region 王永辉添加 作用：导出Excel文件
    /// <summary>
    /// 导出按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>   
    protected void Button1_Click(object sender, EventArgs e)
    {
        //this.UltraWebGridExcelExporter1.ExcelStartRow = 1;
        this.UltraWebGridExcelExporter1.DownloadName = "LZRYXX.xls";
        this.UltraWebGridExcelExporter1.Export(this.UltraWebGrid1);
    }

    /// <summary>
    /// 在执行导出操作前触发此事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UltraWebGridExcelExporter1_BeginExport(object sender, Infragistics.WebUI.UltraWebGrid.ExcelExport.BeginExportEventArgs e)
    {
        //// Add a title to the report 为报表添加标题
        //e.CurrentWorksheet.Rows[0].Cells[0].Value = "Active customers (" + DateTime.Now.Year + ")";
        //e.CurrentWorksheet.Rows[0].Cells[0].CellFormat.Font.Color = System.Drawing.Color.Crimson;
        //e.CurrentWorksheet.Rows[0].Cells[0].CellFormat.Font.Height = 400;

        //// Add a caption with some more information
        //e.CurrentWorksheet.Rows[1].Cells[0].Value = "A listing of customers that have purchased products in the first quarter of " + DateTime.Now.Year;

        //// Set UltraWebGridExcelExporter1.ExcelStartRow in the event that
        //// trigger the export to choose which line of the workbook that 
        //// the grid export starts on

    }
    protected void UltraWebGridExcelExporter1_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.ExcelExport.ExcelExportInitializeRowEventArgs e)
    {
        //  添加额外的样式去描述文件中的行
        //if ((string)e.Row.Cells.FromKey("年份").Value == "2009") 
        //{
        //    // 当此行符合要求的条件时显示高亮样式（颜色pink）
        //    e.Row.Style.BackColor = System.Drawing.Color.Pink;
        //}
    }
    #endregion   
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("Employees_List_LZ.aspx");
    }
}
