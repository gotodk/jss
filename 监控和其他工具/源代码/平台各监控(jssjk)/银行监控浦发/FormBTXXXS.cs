using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using 银行监控浦发后台;

namespace 银行监控浦发
{
    public partial class FormBTXXXS : Form
    {
        /// <summary>
        ///银行文件中独自存在的数据
        /// </summary>
        private DataTable dataTableFromFiel_ADD;

        /// <summary>
        ///来自数据库中的不同数据
        /// </summary>
        private DataTable dataTableDifferFrom_Base;

        /// <summary>
        ///来自银行文件中的不同数据
        /// </summary>
        private DataTable dataTableDifferFrom_File;

        /// <summary>
        ///来自银行文件中缺少的数据
        /// </summary>
        private DataTable dataTableFromFiel_Del;

        public FormBTXXXS()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重载构造
        /// </summary>
        /// <param name="dataTableFromFielADD">银行文件中独自存在的数据</param>
        /// <param name="dataTableDifferFromBase">来自数据库中的不同数据</param>
        /// <param name="dataTableDifferFromFile">来自银行文件中的不同数据</param>
        /// <param name="dataTableFromFielDel">来自银行文件中缺少的数据</param>
        public FormBTXXXS(DataTable dataTableFromFielADD, DataTable dataTableDifferFromBase, DataTable dataTableDifferFromFile, DataTable dataTableFromFielDel)
            : this()
        {

            dataTableFromFiel_ADD = dataTableFromFielADD;
            dataTableDifferFrom_Base = dataTableDifferFromBase;
            dataTableDifferFrom_File = dataTableDifferFromFile;
            dataTableFromFiel_Del = dataTableFromFielDel;



        }

        private void FormBTXXXS_Load(object sender, EventArgs e)
        {
            this.labOnlyYH.Text = "银行有数据我们没有数据：";
            this.dgvOnlyYH.DataSource = dataTableFromFiel_ADD;

            this.labOnlyMine.Text = "我们有数据银行没有数据：";
            this.dgvOnlyMine.DataSource = dataTableFromFiel_Del;

            this.labDifferOurYH.Text = "我们和银行不同的数据：";
            this.dgvDifferOurYH.DataSource = dataTableDifferFrom_Base;

            this.labDifferYHOur.Text = "银行和我们不同的数据：";
            this.dgvDifferYHOur.DataSource = dataTableDifferFrom_File;



            //1、银行有数据我们没有数据
            if (dataTableFromFiel_ADD.Rows.Count > 0)
            {
                StringOP.WriteLog("银行有数据我们没有数据：","");

                this.panOnlyYH.Visible = true;
                this.dgvOnlyYH.Visible = true;
            }
            else
            {
                this.panOnlyYH.Visible = false;
                this.dgvOnlyYH.Visible = false;
            }
            //2、我们有数据银行没有数据
            if (dataTableFromFiel_Del.Rows.Count > 0)
            {
                StringOP.WriteLog("我们有数据银行没有数据：","");
                this.panOnlyMine.Visible = true;
                this.dgvOnlyMine.Visible = true;
            }
            else
            {
                this.panOnlyMine.Visible = false;
                this.dgvOnlyMine.Visible = false;
            }
            //3、我们和银行不同的数据
            if (dataTableDifferFrom_Base.Rows.Count > 0)
            {
                StringOP.WriteLog("我们和银行不同的数据：","");
                this.panDifferOurYH.Visible = true;
                this.dgvDifferOurYH.Visible = true;
            }
            else
            {
                this.panDifferOurYH.Visible = false;
                this.dgvDifferOurYH.Visible = false;
            }
            //4、银行和我们不同的数据
            if (dataTableDifferFrom_File.Rows.Count > 0)
            {
                StringOP.WriteLog("银行和我们不同的数据：","");
                this.panDifferYHOur.Visible = true;
                this.dgvDifferYHOur.Visible = true;
            }
            else
            {
                this.panDifferYHOur.Visible = false;
                this.dgvDifferYHOur.Visible = false;
            }
        }

        /// <summary>
        /// 成功解决差异
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSuccessCY_Click(object sender, EventArgs e)
        {

            //if (MessageBox.Show("确保本次的差异对账信息已经成功解决完毕！") == DialogResult.OK)
            //{
            //    string fileName = "FileYHZZConfig.xml";
            //    DataSet dataSet = new DataSet();
            //    dataSet.ReadXml(fileName);
            //    string saveFilePath = dataSet.Tables["基本配置"].Rows[0]["MSS"].ToString();
            //    string saveFileTime = dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString().Split('|')[0];
            //    string saveFileMsg = dataSet.Tables["基本配置"].Rows[0]["Tag"].ToString().Split('|')[1];
            //    string saveFileBeforName = dataSet.Tables["基本配置"].Rows[0]["FileName"].ToString();
            //    dataSet.Tables["基本配置"].Rows[0]["Tag"] = saveFileTime + "|" + "数据没有差异";
            //    dataSet.WriteXml(fileName);

            //    MessageBox.Show(Convert.ToDateTime(saveFileTime).ToString("yyyy年MM月dd日")+"的差异对账信息已经成功解决！");
            //}
        }


        #region

        /// <summary>
        /// 银行有数据我们没有数据 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOnlyYH_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "|.xls";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = dataTableFromFiel_ADD.Copy();
                dataSet.Tables.Add(dataTable);
                MyXlsClass myXlsClass = new MyXlsClass();
                myXlsClass.goxls(dataSet, saveFileDialog.FileName, "  ", "银行有数据我们没有数据", 20);
            }
        }
        /// <summary>
        /// 我们有数据银行没有数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOnlyMine_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "|.xls";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = dataTableFromFiel_Del.Copy();
                dataSet.Tables.Add(dataTable);
                MyXlsClass myXlsClass = new MyXlsClass();
                myXlsClass.goxls(dataSet, saveFileDialog.FileName, "  ", "我们有数据银行没有数据", 20);
            }
        }

        /// <summary>
        /// 我们和银行不同的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDifferOurYH_Click()
        {
             SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "|.xls";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = dataTableDifferFrom_Base.Copy();
                dataSet.Tables.Add(dataTable);
                MyXlsClass myXlsClass = new MyXlsClass();
                myXlsClass.goxls(dataSet, saveFileDialog.FileName, "  ", "我们和银行不同的数据", 20);
            }
        }
        /// <summary>
        /// 银行和我们不同的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDifferYHOur_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "|.xls";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = dataTableDifferFrom_File.Copy();
                dataSet.Tables.Add(dataTable);
                MyXlsClass myXlsClass = new MyXlsClass();
                myXlsClass.goxls(dataSet, saveFileDialog.FileName, "  ", "银行和我们不同的数据", 20);
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {

        }









    }
}
