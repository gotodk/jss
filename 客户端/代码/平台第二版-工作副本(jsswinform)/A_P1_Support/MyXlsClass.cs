using System;
using org.in2bits.MyXls;
using System.Collections;
using System.Data;

namespace 客户端主程序.Support
{
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
        public static void goxls(DataSet dsxls, string filename, string sheetsname, string title, int rowwidth, string lujing)
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
            xf_col.PatternColor = Colors.Default30;
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
            xf_col.Font.ColorIndex = 1;


            int rowCount = dsxls.Tables[0].Rows.Count;
            int colCount = dsxls.Tables[0].Columns.Count;
            int rowMin = 1, colMin = 1;

            int rowbegin = 0;
            if (title.Trim() != "")
            {
                MergeArea meaA = new MergeArea(1, 1, 1, dsxls.Tables[0].Columns.Count);
                sheet.AddMergeArea(meaA);//填加合并单元格 
                Cell cell_title = cells.Add(1, 1, title);
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
            try
            {
                doc.Save(lujing, true);
            }
            catch (Exception ex)
            {
                ;
            }
            //doc.Send();
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
        public static void goxls_2(DataSet dsxls, string filename, string sheetsname, string title1, string title2, int rowwidth, string lujing)
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
            xf_col.Font.Height = 11 * 20;
            xf_col.Font.ColorIndex = 1;


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
            doc.Save(lujing, true);
            //doc.Send();
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
        public static void goxls_3(DataSet dsxls, string filename, string sheetsname, ArrayList title, string title2, int rowwidth, string lujing)
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
            doc.Save(lujing, true);
            //doc.Send();
            //Response.Flush();
            //Response.End();
        }

    }
}
