using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace 银行监控平安
{
    public partial class CMyXls : Component
    {
        public CMyXls()
        {
            InitializeComponent();
        }

        public CMyXls(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// 开始导出
        /// </summary>
        /// <param name="ht_where">要处理的SQL语句</param>
        /// <param name="HideColumns">要隐藏的列名</param>
        public void BeginExport(DataSet ds, string filename)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                // 在此添加代码,选择的路径为 folderBrowserDialog1.SelectedPath
                string pathselect = folderBrowserDialog1.SelectedPath;              
                MyXlsClass.goxls(ds, filename + ".xls", pathselect);
                MessageBox.Show("文件导出成功！");
            }
            else
            {
                ;
            }
        }

    }
}
