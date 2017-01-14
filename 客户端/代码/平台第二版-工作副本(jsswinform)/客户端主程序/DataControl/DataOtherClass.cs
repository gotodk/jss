using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;

namespace 客户端主程序.DataControl
{
    /// <summary>
    /// 特殊数据处理线程类
    /// </summary>
    class DataOtherClass
    {

        /// <summary>
        /// 手写的绑定数据，用于动态刷新，不用传统绑定，避免用户当前滚动条位置、选中行、表头排序的状态丢失。
        /// </summary>
        /// <param name="dstest">数据集</param>
        /// <param name="dataGridViewMain">DataGridView控件</param>
        public static void LoadDataSet(DataSet dstest, DataGridView dataGridViewMain)
        {

            //保持既定排序和横向滚动条不变
            DataGridViewColumn dgvc = dataGridViewMain.SortedColumn;
            SortOrder so = dataGridViewMain.SortOrder;
            int si1 = dataGridViewMain.FirstDisplayedScrollingColumnIndex;

            //重写数值，新行数小的话，删除多余行。新行数大的话，增加新增行。
            //差额正值说明新数据行多
            int cha = dstest.Tables[0].Rows.Count - dataGridViewMain.Rows.Count;
            if (cha >= 0)
            {
                int oldrowindex = dataGridViewMain.Rows.Count - 1;
                for (int i = 0; i < dataGridViewMain.Rows.Count - 1; i++)
                {
                    for (int c = 0; c < dataGridViewMain.ColumnCount - 1; c++)
                    {
                        dataGridViewMain.Rows[i].Cells[c].Value = dstest.Tables[0].Rows[i][c].ToString();
                    }
                }
                for (int p = 0; p < cha; p++)
                {
                    int index = dataGridViewMain.Rows.Add();
                    for (int c = 0; c < dataGridViewMain.ColumnCount - 1; c++)
                    {
                        dataGridViewMain.Rows[index].Cells[c].Value = dstest.Tables[0].Rows[oldrowindex + p + 1][c].ToString();
                    }
                }

            }
            if (cha < 0)//差额负值说明新数据行少
            {
                int oldrowindex = dataGridViewMain.Rows.Count - 1;
                for (int i = 0; i < dstest.Tables[0].Rows.Count - 1; i++)
                {
                    for (int c = 0; c < dataGridViewMain.ColumnCount - 1; c++)
                    {
                        dataGridViewMain.Rows[i].Cells[c].Value = dstest.Tables[0].Rows[i][c].ToString();
                    }
                }
                for (int p = 0; p < -cha; p++)
                {

                    dataGridViewMain.Rows.RemoveAt(oldrowindex - p - 1);
                }
            }

            //恢复排序和横向滚动条
            ListSortDirection direction;
            if (so == SortOrder.Ascending && dgvc != null)
            {
                direction = ListSortDirection.Ascending;
                dataGridViewMain.Sort(dgvc, direction);
            }
            if (so == SortOrder.Descending && dgvc != null)
            {
                direction = ListSortDirection.Descending;
                dataGridViewMain.Sort(dgvc, direction);
            }
            dataGridViewMain.FirstDisplayedScrollingColumnIndex = si1;
        }












    }
}
