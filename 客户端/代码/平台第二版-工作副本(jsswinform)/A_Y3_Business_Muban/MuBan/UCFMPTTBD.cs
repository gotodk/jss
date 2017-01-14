using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.NewDataControl;
using System.Threading;

namespace 客户端主程序.SubForm.NewCenterForm.MuBan
{
    public partial class UCFMPTTBD : UserControl
    {
        string strZBDB_Number;
        public UCFMPTTBD()
        {
            InitializeComponent();
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            //获取参数
            strZBDB_Number = this.Tag.ToString();
            SRT_GetTBD_Run();
        }


        #region//开启线程获取，界面数据并且绑定界面

        //开启一个线程提交数据
        private void SRT_GetTBD_Run()
        {
            Hashtable hashTable = new Hashtable();
            hashTable["中标定标_Number"] = strZBDB_Number;
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTable, new delegateForThread(SRT_TBDData));
            Thread trd = new Thread(new ThreadStart(OTD.Get_TBDData));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_TBDData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetTBDData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetTBDData_Invoke(Hashtable OutPutHT)
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
        private void BindData(DataSet dsreturn)
        {

            DataTable dataTable = (DataTable)dsreturn.Tables["投标单"];
            //投标单号
            this.labTBDH.Text =dataTable.Columns["投标单号"].ColumnName.ToString()+"："+ dataTable.Rows[0]["投标单号"].ToString();
            //发布时间
            this.labFBSJ.Text = dataTable.Columns["发布时间"].ColumnName.ToString() + "：" + dataTable.Rows[0]["发布时间"].ToString();
            dataTable.Columns.Remove("投标单号");
            dataTable.Columns.Remove("发布时间");

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
                                this.labMJMC.Text = dataTable.Columns["卖方名称"].ColumnName + "：" + dataTable.Rows[0]["卖方名称"].ToString();
                            }
                            else if (h == 1)
                            {
                                this.labSPMC.Text = dataTable.Columns["拟卖出商品名称"].ColumnName + "：" + dataTable.Rows[0]["拟卖出商品名称"].ToString();

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
                                ll.Name = "myshow_" + shujunum+"_"+l.ToString();
                                TLPshow.Controls.Add(ll, l, h);
                            }
                            else
                            {
                            
                                string str = dataTable.Rows[0]["可供货区域"].ToString().Replace('|', '、').ToString();
                                str = str.Remove(str.Length - 1);
                                str = str.Substring(1);
                                str = dataTable.Columns["可供货区域"].ColumnName + "：" + str;
                                int LblNum = str.Length;   //Label内容长度
                                int RowNum = 50;           //每行显示的字数
                                float FontWidth = labGHQY.Width / labGHQY.Text.Length;    //每个字符的宽度
                                int RowHeight = 15;           //每行的高度
                                int ColNum = (LblNum - (LblNum / RowNum) * RowNum) == 0 ? (LblNum / RowNum) : (LblNum / RowNum) + 1;   //列数
                                labGHQY.AutoSize = false;    //设置AutoSize
                                labGHQY.Height = RowHeight * ColNum;           //设置显示高度
                                this.labGHQY.Text = str;
                             
                            }

                        }
                }
            }



        }

    }


}
