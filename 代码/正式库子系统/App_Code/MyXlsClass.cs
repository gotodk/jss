using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using org.in2bits.MyXls;
using System.Collections;

/// <summary>
///MyXlsClass 的摘要说明
/// </summary>
public class MyXlsClass
{
	public MyXlsClass()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 利用MyXls控件的一个方法，导出到电子表格
    /// </summary>
    /// <param name="dsxls">数据集，列明应该是中文滴</param>
    /// <param name="filename">导出文件名</param>
    /// <param name="sheetsname">导出的sheet名</param>
    /// <param name="title">大标题，如果留空，则不显示大标题</param>
    /// <param name="rowwidth">每个列的宽度</param>
    public void goxls(DataSet dsxls, string filename, string sheetsname, string title, int rowwidth)
    {
        org.in2bits.MyXls.XlsDocument doc = new XlsDocument();
        doc.FileName = filename;
      
        Worksheet sheet = doc.Workbook.Worksheets.Add(sheetsname);
        Cells cells = sheet.Cells;
        ColumnInfo colInfo = new ColumnInfo(doc, sheet);
        colInfo.ColumnIndexStart = 0;
        colInfo.ColumnIndexEnd = (ushort)(dsxls.Tables[0].Columns.Count - 1);
        colInfo.Width = (ushort)(rowwidth * 256);
        sheet.AddColumnInfo(colInfo);


        XF xf_col = doc.NewXF();
        xf_col.HorizontalAlignment = HorizontalAlignments.Centered;
        xf_col.VerticalAlignment = VerticalAlignments.Centered;
        //xf_col.Pattern = 1;
        //xf_col.PatternColor = Colors.Default30;
        xf_col.UseBorder = true;
        xf_col.TopLineStyle = 1;
        xf_col.TopLineColor = Colors.Black;
        xf_col.BottomLineStyle = 1;
        xf_col.BottomLineColor = Colors.Black;
        xf_col.LeftLineStyle = 1;
        xf_col.LeftLineColor = Colors.Black;
        xf_col.RightLineStyle = 1;
        xf_col.RightLineColor = Colors.Black;
        xf_col.Font.Bold = true;
        xf_col.Font.Height = 11 * 20;
        xf_col.Font.ColorIndex =0;


        int rowCount = dsxls.Tables[0].Rows.Count;
        int colCount = dsxls.Tables[0].Columns.Count;
        int rowMin = 1, colMin = 1;

        int rowbegin = 0;
        if (title.Trim() != "")
        {
            MergeArea meaA = new MergeArea(1, 1, 1, dsxls.Tables[0].Columns.Count);
            sheet.AddMergeArea(meaA);//填加合并单元格 
            Cell cell_title = cells.Add(1, 1, title);
            cell_title.Font.Weight = FontWeight.Bold;
            cell_title.HorizontalAlignment = HorizontalAlignments.Centered;
            cell_title.Font.Height = 15 * 20;
            rowbegin = 1;
        }



        for (int row = rowbegin; row < rowCount + rowbegin + 1; row++)
        {
            if (row == rowbegin)
            {
                for (int col = 1; col <= colCount; col++)
                {
                    Cell cell = cells.Add(rowMin + row, colMin + col - 1, dsxls.Tables[0].Columns[col - 1].ColumnName, xf_col);
                    cell.Font.Weight = FontWeight.Bold;
                 
                    cell.HorizontalAlignment = HorizontalAlignments.Centered;
                }
            }
            else
            {
                for (int col = 1; col <= colCount; col++)
                {
                    Cell cell = cells.Add(rowMin + row, colMin + col - 1, dsxls.Tables[0].Rows[row - 1 - rowbegin][col - 1].ToString());
                    cell.HorizontalAlignment = HorizontalAlignments.Centered;
                }
            }
        }

        doc.Send();
     
    }


    /// <summary>
    /// 利用MyXls控件的一个方法，导出到电子表格
    /// </summary>
    /// <param name="dsxls">数据集，列明应该是中文滴</param>
    /// <param name="filename">导出文件名</param>
    /// <param name="sheetsname">导出的sheet名</param>
    /// <param name="title1">大标题1</param>
    /// <param name="title1">大标题2</param>
    /// <param name="rowwidth">每个列的宽度</param>
    public void goxls_2(DataSet dsxls, string filename, string sheetsname, string title1, string title2, int rowwidth)
    {
        org.in2bits.MyXls.XlsDocument doc = new XlsDocument();
        doc.FileName = filename;
        Worksheet sheet = doc.Workbook.Worksheets.Add(sheetsname);
        Cells cells = sheet.Cells;
        ColumnInfo colInfo = new ColumnInfo(doc, sheet);
        colInfo.ColumnIndexStart = 0;
        colInfo.ColumnIndexEnd = (ushort)(dsxls.Tables[0].Columns.Count - 1);
        colInfo.Width = (ushort)(rowwidth * 256);
        sheet.AddColumnInfo(colInfo);


        XF xf_col = doc.NewXF();
        xf_col.HorizontalAlignment = HorizontalAlignments.Centered;
        xf_col.VerticalAlignment = VerticalAlignments.Centered;
        xf_col.Pattern = 1;
        xf_col.PatternColor = Colors.White;
        xf_col.UseBorder = true;
        xf_col.TopLineStyle = 1;
        xf_col.TopLineColor = Colors.Black;
        xf_col.BottomLineStyle = 1;
        xf_col.BottomLineColor = Colors.Black;
        xf_col.LeftLineStyle = 1;
        xf_col.LeftLineColor = Colors.Black;
        xf_col.RightLineStyle = 1;
        xf_col.RightLineColor = Colors.Black;
        xf_col.Font.Bold = true;
        xf_col.Font.Height = 11 * 20;
        xf_col.Font.ColorIndex = 256;


        int rowCount = dsxls.Tables[0].Rows.Count;
        int colCount = dsxls.Tables[0].Columns.Count;
        int rowMin = 1, colMin = 1;

        int rowbegin = 0;
        if (title1.Trim() != "" && title2.Trim() != "")
        {
            MergeArea meaA1 = new MergeArea(1, 1, 1, dsxls.Tables[0].Columns.Count);
            sheet.AddMergeArea(meaA1);//填加合并单元格 
            Cell cell_title1 = cells.Add(1, 1, title1);
            cell_title1.Font.Height = 15 * 20;
            cell_title1.HorizontalAlignment = HorizontalAlignments.Centered;

            MergeArea meaA2 = new MergeArea(2, 2, 1, dsxls.Tables[0].Columns.Count);
            sheet.AddMergeArea(meaA2);//填加合并单元格 
            Cell cell_title2 = cells.Add(2, 1, title2);
            cell_title2.Font.Height = 11 * 20;


            rowbegin = 2;
        }



        for (int row = rowbegin; row < rowCount + rowbegin + 1; row++)
        {
            if (row == rowbegin)
            {
                for (int col = 1; col <= colCount; col++)
                {
                    Cell cell = cells.Add(rowMin + row, colMin + col - 1, dsxls.Tables[0].Columns[col - 1].ColumnName, xf_col);
                    cell.Font.Height = 11 * 20;
                }
            }
            else
            {
                for (int col = 1; col <= colCount; col++)
                {
                    Cell cell = cells.Add(rowMin + row, colMin + col - 1, dsxls.Tables[0].Rows[row - 1 - rowbegin][col - 1].ToString());
                }
            }
        }

        doc.Send();
        //Response.Flush();
        //Response.End();
    }

    /// <summary>
    /// 利用MyXls控件的一个方法，导出到电子表格
    /// </summary>
    /// <param name="dsxls">数据集，列明应该是中文滴</param>
    /// <param name="filename">导出文件名</param>
    /// <param name="sheetsname">导出的sheet名</param>
    /// <param name="title1">大标题1</param>
    /// <param name="title1">大标题2</param>
    /// <param name="rowwidth">每个列的宽度</param>
    /// <param name="array">列表底部合计数据</param>
    public void goxls_2(DataSet dsxls, string filename, string sheetsname, string title1, string title2, int rowwidth,ArrayList array)
    {
        org.in2bits.MyXls.XlsDocument doc = new XlsDocument();
        doc.FileName = filename;
        Worksheet sheet = doc.Workbook.Worksheets.Add(sheetsname);
        Cells cells = sheet.Cells;
        ColumnInfo colInfo = new ColumnInfo(doc, sheet);
        colInfo.ColumnIndexStart = 0;
        colInfo.ColumnIndexEnd = (ushort)(dsxls.Tables[0].Columns.Count - 1);
        colInfo.Width = (ushort)(rowwidth * 256);
        sheet.AddColumnInfo(colInfo);


        XF xf_col = doc.NewXF();
        xf_col.HorizontalAlignment = HorizontalAlignments.Centered;
        xf_col.VerticalAlignment = VerticalAlignments.Centered;
        //xf_col.Pattern = 1;
        //xf_col.PatternColor = Colors.Grey;
        xf_col.UseBorder = true;
        xf_col.TopLineStyle = 1;
        xf_col.TopLineColor = Colors.Black;
        xf_col.BottomLineStyle = 1;
        xf_col.BottomLineColor = Colors.Black;
        xf_col.LeftLineStyle = 1;
        xf_col.LeftLineColor = Colors.Black;
        xf_col.RightLineStyle = 1;
        xf_col.RightLineColor = Colors.Black;
        xf_col.Font.Bold = true;
        xf_col.Font.Height = 11 * 20;
        xf_col.Font.ColorIndex = 0;


        int rowCount = dsxls.Tables[0].Rows.Count;
        int colCount = dsxls.Tables[0].Columns.Count;
        int rowMin = 1, colMin = 1;

        int rowbegin = 0;
        if (title1.Trim() != "" && title2.Trim() != "")
        {
            MergeArea meaA1 = new MergeArea(1, 1, 1, dsxls.Tables[0].Columns.Count);
            sheet.AddMergeArea(meaA1);//填加合并单元格 
            Cell cell_title1 = cells.Add(1, 1, title1);
            cell_title1.Font.Height = 15 * 20;
            cell_title1.Font.Weight = FontWeight.Bold;
            cell_title1.HorizontalAlignment = HorizontalAlignments.Centered;

            MergeArea meaA2 = new MergeArea(2, 2, 1, dsxls.Tables[0].Columns.Count);
            sheet.AddMergeArea(meaA2);//填加合并单元格 
            Cell cell_title2 = cells.Add(2, 1, title2);
            rowbegin = 2;
        }



        for (int row = rowbegin; row < rowCount + rowbegin + 1; row++)
        {
            if (row == rowbegin)
            {
                for (int col = 1; col <= colCount; col++)
                {
                    Cell cell = cells.Add(rowMin + row, colMin + col - 1, dsxls.Tables[0].Columns[col - 1].ColumnName, xf_col);
                    cell.Font.Height = 11 * 20;
                }
            }
            else
            {
                for (int col = 1; col <= colCount; col++)
                {
                    Cell cell = cells.Add(rowMin + row, colMin + col - 1, dsxls.Tables[0].Rows[row - 1 - rowbegin][col - 1].ToString());
                    cell.HorizontalAlignment = HorizontalAlignments.Centered;
                    cell.VerticalAlignment = VerticalAlignments.Centered;
                }
            }
        }
        rowbegin = rowCount + rowbegin + 2;
        if (array.Count > 0)//合并底部数据
        {

            MergeArea meaA1 = new MergeArea(rowbegin, rowbegin, 1, dsxls.Tables[0].Columns.Count - array.Count+1);
            sheet.AddMergeArea(meaA1);//填加合并单元格 
            for (int i = 0; i < array.Count; i++)
            {

                Cell cell_foot = i == 0 ? cells.Add(rowbegin, colMin + i, array[i]) : cells.Add(rowbegin, colMin + i + 2, array[i]);
                 cell_foot.Font.Height = 11 * 20;
                cell_foot.Font.Weight = FontWeight.Bold;
                cell_foot.HorizontalAlignment = i == 0?HorizontalAlignments.Right: HorizontalAlignments.Centered;
                    cell_foot.VerticalAlignment = VerticalAlignments.Centered;
            }
        }
        doc.Send();
        //Response.Flush();
        //Response.End();
    }

    /// <summary>
    /// 利用MyXls控件的一个方法，导出到电子表格
    /// </summary>
    /// <param name="dsxls">数据集，列明应该是中文滴</param>
    /// <param name="filename">导出文件名</param>
    /// <param name="sheetsname">导出的sheet名</param>
    /// <param name="title">大标题列表，根据dt中表格的顺序添加</param>
    /// <param name="title1">大标题2</param>
    /// <param name="rowwidth">每个列的宽度</param>   
        public void goxls_3(DataSet dsxls, string filename, string sheetsname, ArrayList title, string title2, int rowwidth)
    {
        org.in2bits.MyXls.XlsDocument doc = new XlsDocument();
        doc.FileName = filename;
        Worksheet sheet = doc.Workbook.Worksheets.Add(sheetsname);
        Cells cells = sheet.Cells;
        ColumnInfo colInfo = new ColumnInfo(doc, sheet);
        colInfo.ColumnIndexStart = 0;
        colInfo.ColumnIndexEnd = (ushort)(dsxls.Tables[0].Columns.Count - 1);
        colInfo.Width = (ushort)(rowwidth * 256);
        sheet.AddColumnInfo(colInfo);


        XF xf_col = doc.NewXF();
        xf_col.HorizontalAlignment = HorizontalAlignments.Centered;
        xf_col.VerticalAlignment = VerticalAlignments.Centered;
        xf_col.Pattern = 1;
        xf_col.PatternColor = Colors.Grey;
        xf_col.UseBorder = true;
        xf_col.TopLineStyle = 1;
        xf_col.TopLineColor = Colors.Black;
        xf_col.BottomLineStyle = 1;
        xf_col.BottomLineColor = Colors.Black;
        xf_col.LeftLineStyle = 1;
        xf_col.LeftLineColor = Colors.Black;
        xf_col.RightLineStyle = 1;
        xf_col.RightLineColor = Colors.Black;
        xf_col.Font.Bold = true;
        xf_col.Font.Height = 10 * 20;
        xf_col.Font.ColorIndex = 1;

        int totalrow = 0;
        for (int i = 0; i < dsxls.Tables.Count; i++)
        {
            int rowCount = dsxls.Tables[i].Rows.Count;
            int colCount = dsxls.Tables[i].Columns.Count;
            totalrow = totalrow + dsxls.Tables[i].Rows.Count;
            int rowMin = i > 0 ? totalrow - rowCount + (i * 3) + 1 : 1;
            int colMin = 1;
            int rowtitle = 0;

            string title1 = title[i].ToString();
            if (title1.Trim() != "" && title2.Trim() != "")
            {
                MergeArea meaA1 = new MergeArea(rowMin, rowMin, 1, dsxls.Tables[i].Columns.Count);
                sheet.AddMergeArea(meaA1);//填加合并单元格 
                Cell cell_title1 = cells.Add(rowMin, 1, title[i]);
                cell_title1.Font.Height = 12 * 18;
                cell_title1.HorizontalAlignment = HorizontalAlignments.Centered;
                cell_title1.Font.Bold = true;

                MergeArea meaA2 = new MergeArea(rowMin + 1, rowMin + 1, 1, dsxls.Tables[i].Columns.Count);
                sheet.AddMergeArea(meaA2);//填加合并单元格 
                Cell cell_title2 = cells.Add(rowMin + 1, 1, title2);
                cell_title2.Font.Height = 10 * 20;

                rowtitle = 2;
            }
            for (int row = rowtitle; row < rowCount + rowtitle + 1; row++)
            {
                if (row == rowtitle)
                {
                    for (int col = 1; col <= colCount; col++)
                    {
                        Cell cell = cells.Add(rowMin + row, colMin + col - 1, dsxls.Tables[i].Columns[col - 1].ColumnName, xf_col);
                    }
                }
                else
                {
                    for (int col = 1; col <= colCount; col++)
                    {
                        Cell cell = cells.Add(rowMin + row, colMin + col - 1, dsxls.Tables[i].Rows[row - 1 - rowtitle][col - 1].ToString());
                    }
                }
            }

        }

        doc.Send();
    
    }

    /// <summary>
    /// 利用MyXls控件的一个方法，导出到电子表格
    /// </summary>
    /// <param name="dsDD">订单信息</param>
    /// <param name="dsFWSXX">数据集，服务商信息</param>
    /// <param name="fwsColumns">服务商信息显示几列</param>
    /// <param name="dsSJLB">数据集，数据列表</param>
    /// <param name="dsFPXX">数据集，开票信息</param>
    /// <param name="fpColumns">开票信息显示几列</param>
    /// <param name="filename">导出文件名</param>
    /// <param name="sheetsname">导出的sheet名</param>
    /// <param name="title">大标题</param>
    /// <param name="titleFWSXX">数据集服务商信息上的标题</param>
    /// <param name="titleSJLB">数据集数据列表上的标题</param>
    /// <param name="titleFPXX">数据集开票信息上的标题</param>
    /// <param name="rowwidth">每个列的宽度</param>
    public void goxls_4(DataSet dsDD, DataSet dsFWSXX,int fwsColumns, DataSet dsSJLB, DataSet dsFPXX,int fpColumns, string filename, string sheetsname,string title, string titleFWSXX, string titleSJLB, string titleFPXX, int rowwidth)
    {
        #region//取数据列最多的DataSet，将这个列数作为EXCEL大标题合并单元格的列数
        int tableColumnsCount = 0;
        if (dsFWSXX.Tables[0].Columns.Count > dsSJLB.Tables[0].Columns.Count)
        {
            tableColumnsCount = dsFWSXX.Tables[0].Columns.Count;
        }
        else
        {
            tableColumnsCount = dsSJLB.Tables[0].Columns.Count;
        }
        if (tableColumnsCount <= dsFPXX.Tables[0].Columns.Count)
        {
            tableColumnsCount = dsFPXX.Tables[0].Columns.Count;
        }
        org.in2bits.MyXls.XlsDocument doc = new XlsDocument();
        doc.FileName = filename;
        Worksheet sheet = doc.Workbook.Worksheets.Add(sheetsname);
        Cells cells = sheet.Cells;
        ColumnInfo colInfo = new ColumnInfo(doc, sheet);
        colInfo.ColumnIndexStart = 0;
        colInfo.ColumnIndexEnd = (ushort)(tableColumnsCount - 1);
        colInfo.Width = (ushort)(rowwidth * 256);
        sheet.AddColumnInfo(colInfo);
        #endregion

        #region//设置单元格的样式
        XF xf_col = doc.NewXF();
        xf_col.HorizontalAlignment = HorizontalAlignments.Centered;
        xf_col.VerticalAlignment = VerticalAlignments.Centered;
        //xf_col.Pattern = 1;
        //xf_col.PatternColor = Colors.Default30;
        xf_col.UseBorder = true;
        xf_col.TopLineStyle = 1;
        xf_col.TopLineColor = Colors.Black;
        xf_col.BottomLineStyle = 1;
        xf_col.BottomLineColor = Colors.Black;
        xf_col.LeftLineStyle = 1;
        xf_col.LeftLineColor = Colors.Black;
        xf_col.RightLineStyle = 1;
        xf_col.RightLineColor = Colors.Black;
        xf_col.Font.Bold = false;
        xf_col.Font.Height = 11 * 20;
        xf_col.Font.ColorIndex = 0;
        #endregion
          
        int rowbegin = 0;
        #region//设置文件标题
        if (title.Trim() != "")
        {
            rowbegin++;
            MergeArea meaA = new MergeArea(rowbegin, rowbegin, 1, tableColumnsCount);
            sheet.AddMergeArea(meaA);//填加合并单元格 
            Cell cell_title = cells.Add(rowbegin, 1, title);
            cell_title.Font.Weight = FontWeight.Bold;
            cell_title.HorizontalAlignment = HorizontalAlignments.Centered;//字体水平位置 
            cell_title.Font.Height = 15 * 20;

        }
        #endregion

        #region//设置订单号、时间
        if (dsDD != null && dsDD.Tables[0].Rows.Count>0)
        {
            rowbegin++;
            for (int i = 0; i < dsDD.Tables[0].Rows.Count;i++ )
            {
                string Message = dsDD.Tables[0].Columns[0].ColumnName.ToString() + ":" + dsDD.Tables[0].Rows[i]["订单号"].ToString()
                    +"  "+ dsDD.Tables[0].Columns[1].ColumnName.ToString() + ":" + dsDD.Tables[0].Rows[i]["订单时间"].ToString();
                MergeArea meaA1 = new MergeArea(rowbegin, rowbegin, 1, tableColumnsCount);
                sheet.AddMergeArea(meaA1);//填加合并单元格 
                Cell cell_title = cells.Add(rowbegin, 1, Message);
                cell_title.Font.Weight = FontWeight.Bold;
                cell_title.HorizontalAlignment = HorizontalAlignments.Centered;//字体水平位置         
                cell_title.Font.Height = 12 * 20;
                
            }
        }
        #endregion

        #region //服务商信息上的标题
        if (titleFWSXX.Trim() != "")
        {
            rowbegin++;
            MergeArea meaA1 = new MergeArea(rowbegin, rowbegin, 1, tableColumnsCount);
            sheet.AddMergeArea(meaA1);//填加合并单元格 
            Cell cell_title = cells.Add(rowbegin, 1, titleFWSXX);
            cell_title.Font.Weight = FontWeight.Bold;
            cell_title.HorizontalAlignment = HorizontalAlignments.Left;//字体水平位置         
            cell_title.Font.Height = 12 * 20;

        }
        #endregion

        #region//绑定服务商信息
        if (dsFWSXX.Tables[0].Rows.Count>0)
        {
            rowbegin++;
            int rows=dsFWSXX.Tables[0].Rows.Count;
            int columns=dsFWSXX.Tables[0].Columns.Count;            
            int col = 0;
            for (int column = 1; column <= columns; column++)
            {
                col++;
                Cell cell = cells.Add(rowbegin, col, dsFWSXX.Tables[0].Columns[column - 1].ColumnName, xf_col);
                cell.HorizontalAlignment = HorizontalAlignments.Centered;
                col++;
                cell = cells.Add(rowbegin, col, dsFWSXX.Tables[0].Rows[0][column - 1].ToString(), xf_col);
                cell.HorizontalAlignment = HorizontalAlignments.Centered;
                if (col >= fwsColumns * 2)
                {
                    col = 0;
                    rowbegin++;
                }
                
            }
        }
        #endregion

        #region //数据集数据列表上的标题
        if (titleSJLB.Trim() != "")
        {
            rowbegin++;
            MergeArea meaA1 = new MergeArea(rowbegin, rowbegin, 1, tableColumnsCount);
            sheet.AddMergeArea(meaA1);//填加合并单元格 
            Cell cell_title = cells.Add(rowbegin, 1, titleSJLB);
            cell_title.Font.Weight = FontWeight.Bold;
            cell_title.HorizontalAlignment = HorizontalAlignments.Left;//字体水平位置         
            cell_title.Font.Height = 12 * 20;

        }
        #endregion

        #region//绑定数据列表数据集
        if (dsSJLB.Tables[0].Rows.Count>0)
        {
            rowbegin++;
            int rows = dsSJLB.Tables[0].Rows.Count;
            int columns = dsSJLB.Tables[0].Columns.Count;        
            #region
            for (int row = rowbegin; row < rows + rowbegin + 1; row++)
            {
                if (row == rowbegin)
                {
                    for (int column = 1; column <= columns; column++)
                    {
                        Cell cell = cells.Add(row, column, dsSJLB.Tables[0].Columns[column - 1].ColumnName, xf_col);
                        cell.HorizontalAlignment = HorizontalAlignments.Centered;
                    }
                }
                else
                {
                    for (int column = 1; column <= columns; column++)
                    {
                        Cell cell = cells.Add(row, column, dsSJLB.Tables[0].Rows[row - rowbegin - 1][column - 1].ToString(), xf_col);
                        cell.HorizontalAlignment = HorizontalAlignments.Centered;
                    }
                }
            }
            rowbegin += rows+1;
            #endregion
        }
        #endregion

        #region //开票信息上的标题
        if (titleFPXX.Trim() != "")
        {
            rowbegin++;
            MergeArea meaA1 = new MergeArea(rowbegin, rowbegin, 1, tableColumnsCount);
            sheet.AddMergeArea(meaA1);//填加合并单元格 
            Cell cell_title = cells.Add(rowbegin, 1, titleFPXX);
            cell_title.Font.Weight = FontWeight.Bold;
            cell_title.HorizontalAlignment = HorizontalAlignments.Left;//字体水平位置         
            cell_title.Font.Height = 12 * 20;

        }
        #endregion

        #region//绑定开票信息数据集
        if (dsFPXX.Tables[0].Rows.Count > 0)
        {
            rowbegin++;
            int rows = dsFPXX.Tables[0].Rows.Count;
            int columns = dsFPXX.Tables[0].Columns.Count;
            int col = 0;
            for (int column = 1; column <= columns; column++)
            {
                col++;
                Cell cell = cells.Add(rowbegin, col, dsFPXX.Tables[0].Columns[column - 1].ColumnName, xf_col);                
                cell.HorizontalAlignment = HorizontalAlignments.Centered;
                col++;
                cell = cells.Add(rowbegin, col, dsFPXX.Tables[0].Rows[0][column - 1].ToString(), xf_col);                
                cell.HorizontalAlignment = HorizontalAlignments.Centered;
                if (col >= fpColumns * 2)
                {
                    col = 0;
                    rowbegin++;
                }

            }
        }
        #endregion

        doc.Send();
        #region
        //int Firsthbdyg = tableColumnsCount / fwsColumns;//将合并的单元格分成两部分，这是平均分的第一部分，剩下的是第二部分
        //int Lasthbdyg = tableColumnsCount - Firsthbdyg;//第二部分
        //int first = Firsthbdyg / 2;
        //int middle = Firsthbdyg - Firsthbdyg / 2;
        //int last = Lasthbdyg - Firsthbdyg / 2;
        //col++;
        //MergeArea meaA2 = new MergeArea(rowbegin, rowbegin, col, first);
        //sheet.AddMergeArea(meaA2);//填加合并单元格 
        //Cell cell_title = cells.Add(rowbegin, col, dsFWSXX.Tables[0].Columns[column - 1].ColumnName, xf_col);
        //col++;
        //if (col == fwsColumns * 2)
        //{
        //    MergeArea meaA3 = new MergeArea(rowbegin, rowbegin, col, last);
        //    sheet.AddMergeArea(meaA3);//填加合并单元格 
        //    cell_title = cells.Add(rowbegin, col, dsFWSXX.Tables[0].Rows[0][column - 1].ToString(), xf_col);
        //}
        //else
        //{
        //    MergeArea meaA4 = new MergeArea(rowbegin, rowbegin, col, middle);
        //    sheet.AddMergeArea(meaA4);//填加合并单元格 
        //    cell_title = cells.Add(rowbegin, col, dsFWSXX.Tables[0].Rows[0][column - 1].ToString(), xf_col);
        //}
        #endregion
    }
}