using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.NewDataControl;
using System.Threading;

namespace 客户端主程序.SubForm.NewCenterForm.MuBan
{
    public partial class UCFMPTYDD : UserControl
    {
        string strZBDB_Number;
        public UCFMPTYDD()
        {
            InitializeComponent();
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            //获取参数
            strZBDB_Number = this.Tag.ToString();
            SRT_GetYDD_Run();
        }

        #region//开启线程获取，界面数据并且绑定界面

        //开启一个线程提交数据
        private void SRT_GetYDD_Run()
        {
            Hashtable hashTable = new Hashtable();
            hashTable["中标定标_Number"] = strZBDB_Number;
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTable, new delegateForThread(SRT_YDDData));
            Thread trd = new Thread(new ThreadStart(OTD.Get_YDDData));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_YDDData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetYDDData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetYDDData_Invoke(Hashtable OutPutHT)
        {
            //重新开放提交区域,并滚动条强制置顶
            //返回值后的处理
            DataSet dsreturn = (DataSet)OutPutHT["返回值"];

            string zt = dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showstr = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
            //显示执行结果
            switch (zt)
            {
                case "ok":
                    BindData(dsreturn);
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showstr);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }
            this.MinimumSize = new Size(100, 100);
        }

        #endregion

        //处理非线程创建的控件
        //处理非线程创建的控件
        private void BindData(DataSet dsreturn)
        {

            DataTable dataTable = (DataTable)dsreturn.Tables["预订单"];
            //预订单号
            this.labYDDH.Text = dataTable.Columns["预订单号"].ColumnName.ToString() + "：" + dataTable.Rows[0]["预订单号"].ToString();
            //发布时间
            this.labFBSJ.Text = dataTable.Columns["下单时间"].ColumnName.ToString() + "：" + dataTable.Rows[0]["下单时间"].ToString();
            dataTable.Columns.Remove("预订单号");
            dataTable.Columns.Remove("下单时间");

            if (dataTable != null && dataTable.Rows.Count >= 1)
            {
                int lienum = TLPshow.ColumnCount;
                int hangnum = TLPshow.RowCount;
                int shujunum = 3;

                for (int h = 0; h < hangnum; h++)
                {
                    for (int l = 0; l < lienum; l++)
                    {
                        if (h == 0)
                        {
                            this.labMJMC.Text = dataTable.Columns["买方名称"].ColumnName + "：" + dataTable.Rows[0]["买方名称"].ToString();
                        }
                        else if (h == 1)
                        {
                            this.labNMRSPMC.Text = dataTable.Columns["拟买入商品名称"].ColumnName + "：" + dataTable.Rows[0]["拟买入商品名称"].ToString();

                            this.labGG.Text = dataTable.Columns["规格"].ColumnName + "：" + dataTable.Rows[0]["规格"].ToString();
                        }
                        else if (h == 2)
                        {
                            Label ll = new Label();
                            ll.AutoSize = true;
                            ll.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            ll.BackColor = Color.White;
                            ll.ForeColor = Color.Black;
                            ll.Margin = new System.Windows.Forms.Padding(0);
                            ll.Text = dataTable.Columns[shujunum].ColumnName;
                            ll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                            ll.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
                            ll.Name = "myshow_" + shujunum.ToString();
                            TLPshow.Controls.Add(ll, l, h);
                            shujunum++;
                        }
                        else if (h == 3)
                        {
                            Label ll = new Label();
                            ll.AutoSize = true;
                            ll.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            ll.BackColor = Color.White;
                            ll.ForeColor = Color.Black;
                            ll.Margin = new System.Windows.Forms.Padding(0);
                            ll.Text = dataTable.Rows[0][shujunum - lienum + l].ToString();
                            ll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                            ll.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
                            ll.Name = "myshow_" + shujunum + "_" + l.ToString();
                            TLPshow.Controls.Add(ll, l, h);
                        }
                        else
                        {
                         
                            string str = dataTable.Rows[0]["收货区域"].ToString().Replace('|', '、').ToString();
                            str = str.Remove(str.Length - 1);
                            str = str.Substring(1);
                            str = dataTable.Columns["收货区域"].ColumnName + "：" + str;
                            this.labGHQY.Text = str;
                        }

                    }
                }
            }



        }
    }
}
