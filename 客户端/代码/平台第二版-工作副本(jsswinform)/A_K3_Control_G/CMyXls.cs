using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace 客户端主程序.SubForm.NewCenterForm
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
        //public void BeginRunFrom_ht_where(string sql, string[] HideColumns)
        //{
        //    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        // 在此添加代码,选择的路径为 folderBrowserDialog1.SelectedPath
        //        string pathselect = folderBrowserDialog1.SelectedPath;
        //        CMyXlsForm MXF = new CMyXlsForm(sql, HideColumns, pathselect);
        //        MXF.ShowDialog();
        //    }
        //    else
        //    {
        //        ;
        //    }
        //}
        public void BeginRunFrom_ht_where(Hashtable ht_tiaojian)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                // 在此添加代码,选择的路径为 folderBrowserDialog1.SelectedPath
                string pathselect = folderBrowserDialog1.SelectedPath;
                CMyXlsForm MXF = new CMyXlsForm(ht_tiaojian, pathselect);
                MXF.ShowDialog();
            }
            else
            {
                ;
            }
        }

    }
}
