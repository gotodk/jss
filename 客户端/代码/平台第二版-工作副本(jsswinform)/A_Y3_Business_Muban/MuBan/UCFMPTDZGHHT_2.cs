using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.NewDataControl;
using System.Threading;

namespace 客户端主程序.SubForm.NewCenterForm.MuBan
{
    public partial class UCFMPTDZGHHT_2 : UserControl
    {
        Hashtable hashTable;



        public UCFMPTDZGHHT_2()
        {
         
            InitializeComponent();
         
            flowLayoutPanel1.Enabled = false;
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            Hashtable hashTabl = (Hashtable)this.Tag;
            hashTable = new Hashtable();
            hashTable["中标定标_Number"] = hashTabl["要传递的参数1"].ToString();
            SRT_GetMMJData_Run();
          
        }


        #region//开启线程获取，界面数据并且绑定界面

        //开启一个线程提交数据
        private void SRT_GetMMJData_Run()
        {
            RunThreadClassKTJYZH OTD = new RunThreadClassKTJYZH(hashTable, new delegateForThread(SRT_GetMMJData));
            Thread trd = new Thread(new ThreadStart(OTD.GetDZGHHTXX));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据),调用异步委托
        private void SRT_GetMMJData(Hashtable OutPutHT)
        {
            try { Invoke(new delegateForThreadShow(SRT_GetMMJData_Invoke), new Hashtable[] { OutPutHT }); }
            catch (Exception ex) { Support.StringOP.WriteLog("委托回调错误：" + ex.ToString()); }
        }
        //处理非线程创建的控件
        private void SRT_GetMMJData_Invoke(Hashtable OutPutHT)
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
                    flowLayoutPanel1.Enabled = true;
                    BindDataFace(dsreturn);
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

        /// <summary>
        /// 绑定界面数据信息
        /// </summary>
        /// <param name="dataSet"></param>
        public void BindDataFace(DataSet dataSet)
        {
            DataTable dataTableTBDZZ = dataSet.Tables["投标单资质"];
            DataTable dataTableTHYFHXX = dataSet.Tables["提货单发货单"];
            DataTable dataTableQTZZ = dataSet.Tables["其他资质"];
            //投标单资质
            this.lab_ZLBZYZM.Tag = dataTableTBDZZ.Rows[0]["质量标准与证明"].ToString();
            this.lab_CPJCBG.Tag = dataTableTBDZZ.Rows[0]["产品检测报告"].ToString();
            this.lab_PGFZRFLCNS.Tag = dataTableTBDZZ.Rows[0]["品管总负责人法律承诺书"].ToString();
            this.lab_FDDBRCNS.Tag = dataTableTBDZZ.Rows[0]["法定代表人承诺书"].ToString();
            this.lab_SHFWGDYCN.Tag = dataTableTBDZZ.Rows[0]["售后服务规定与承诺"].ToString();
            this.lab_CPSJSQS.Tag = dataTableTBDZZ.Rows[0]["产品送检授权书"].ToString();
            this.lab_SLZM.Tag = dataTableTBDZZ.Rows[0]["税率证明"].ToString();
            string[] strArray = dataTableTBDZZ.Rows[0]["资质01"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("特殊资质名称", typeof(System.String));
            dataTable.Columns.Add("特殊资质路径", typeof(System.String));
            if (strArray.Length > 0)//说明存在特殊资质
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    string[] strSingle = strArray[i].Split('*');
                    string strTSZZMC = strSingle[0].ToString();//特殊资质名称
                    string strTSZZLJ = strSingle[1].ToString();//特殊资质路径
                    dataTable.Rows.Add(new object[] { strTSZZMC, strTSZZLJ });
                }
                for (int j = 0; j < 10 - strArray.Length; j++)//为了凑够10个资质
                {
                    dataTable.Rows.Add(new object[] { "", "" });
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    dataTable.Rows.Add(new object[] { "", "" });
                }
            }
            this.lab_ZZ01.Tag = dataTable.Rows[0]["特殊资质路径"].ToString();
            this.lab_ZZ02.Tag = dataTable.Rows[1]["特殊资质路径"].ToString();
            this.lab_ZZ03.Tag = dataTable.Rows[2]["特殊资质路径"].ToString();
            this.lab_ZZ04.Tag = dataTable.Rows[3]["特殊资质路径"].ToString();
            this.lab_ZZ05.Tag = dataTable.Rows[4]["特殊资质路径"].ToString();
            this.lab_ZZ06.Tag = dataTable.Rows[5]["特殊资质路径"].ToString();
            this.lab_ZZ07.Tag = dataTable.Rows[6]["特殊资质路径"].ToString();
            this.lab_ZZ08.Tag = dataTable.Rows[7]["特殊资质路径"].ToString();
            this.lab_ZZ09.Tag = dataTable.Rows[8]["特殊资质路径"].ToString();
            this.lab_ZZ10.Tag = dataTable.Rows[9]["特殊资质路径"].ToString();
            if (dataTableTHYFHXX.Rows.Count > 0)
            {
                //提货单与发货单信息
                this.lab_YTHSL.Text = "已提货数量：" + dataTableTHYFHXX.Rows[0]["已提货数量"].ToString();
                this.lab_YTHJE.Text = "已提货金额：" + dataTableTHYFHXX.Rows[0]["已提货金额"].ToString();
                this.lab_YFHSL.Text = "已发货数量：" + dataTableTHYFHXX.Rows[0]["已发货数量"].ToString();
                this.lab_WYYSHSL.Text = "无异议收货数量： " + dataTableTHYFHXX.Rows[0]["无异议收货数量"].ToString();
                this.dataGridView2.AutoGenerateColumns = false;
                this.dataGridView2.DataSource = dataTableTHYFHXX.DefaultView;
            }

            if (dataTable.Rows[0]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ01.Visible = false;
            }
            else
            {
                this.panel_ZZ01.Visible = true;
                this.lab_ZZ01.Text = dataTable.Rows[0]["特殊资质名称"].ToString();
            }
            if (dataTable.Rows[1]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ02.Visible = false;
            }
            else
            {
                this.panel_ZZ02.Visible = true;
                this.lab_ZZ02.Text = dataTable.Rows[1]["特殊资质名称"].ToString();
            }
            if (dataTable.Rows[2]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ03.Visible = false;
            }
            else
            {
                this.panel_ZZ03.Visible = true;
                this.lab_ZZ03.Text = dataTable.Rows[2]["特殊资质名称"].ToString();
            }
            if (dataTable.Rows[3]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ04.Visible = false;
            }
            else
            {
                this.panel_ZZ04.Visible = true;
                this.lab_ZZ04.Text = dataTable.Rows[3]["特殊资质名称"].ToString();
            }
            if (dataTable.Rows[4]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ05.Visible = false;
            }
            else
            {
                this.panel_ZZ05.Visible = true;
                this.lab_ZZ05.Text = dataTable.Rows[4]["特殊资质名称"].ToString();
            }
            if (dataTable.Rows[5]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ06.Visible = false;
            }
            else
            {
                this.panel_ZZ06.Visible = true;
                this.lab_ZZ06.Text = dataTable.Rows[5]["特殊资质名称"].ToString();
            }
            if (dataTable.Rows[6]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ07.Visible = false;
            }
            else
            {
                this.panel_ZZ07.Visible = true;
                this.lab_ZZ07.Text = dataTable.Rows[6]["特殊资质名称"].ToString();
            }
            if (dataTable.Rows[7]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ08.Visible = false;
            }
            else
            {
                this.panel_ZZ08.Visible = true;
                this.lab_ZZ08.Text = dataTable.Rows[7]["特殊资质名称"].ToString();
            }
            if (dataTable.Rows[8]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ09.Visible = false;
            }
            else
            {
                this.panel_ZZ09.Visible = true;
                this.lab_ZZ09.Text = dataTable.Rows[8]["特殊资质名称"].ToString();
            }
            if (dataTable.Rows[9]["特殊资质路径"].ToString() == "")
            {
                this.panel_ZZ10.Visible = false;
            }
            else
            {
                this.panel_ZZ10.Visible = true;
                this.lab_ZZ10.Text = dataTable.Rows[9]["特殊资质名称"].ToString();
            }

            if (dataTableQTZZ.Rows.Count > 0)
            {
                this.linkBZH.Tag = dataTableQTZZ.Rows[0]["保函编号"].ToString();
                if (dataTableQTZZ.Rows[0]["履约争议证明文件路径"].ToString() != "")
                {
                    this.linkLVZMWJ.Tag = dataTableQTZZ.Rows[0]["履约争议证明文件路径"].ToString().Replace(@"\", "/");
                    this.linkLVZMWJ.Visible = true;
                }
                else
                {
                    this.linkLVZMWJ.Visible = false;
                }

            }
            else
            {
                this.panelQTZZ.Visible = false;
                this.panelBZH.Visible = false;
                this.linkLVZMWJ.Visible = false;


            }




        }

        ImageShow ISIS;
        /// <summary>
        /// 资质查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lab_Buyer_YYZZ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
            LinkLabel linkLable = (LinkLabel)sender;
            string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + linkLable.Tag.ToString().Replace(@"\", "/").Replace(@"\", "/");
            //StringOP.OpenUrl(url);

            Hashtable htT = new Hashtable();
            htT["类型"] = "网址";
            htT["地址"] = url;
            if (ISIS == null || ISIS.IsDisposed == true)
            {
                ISIS = new ImageShow(htT);
            }
            ISIS.Show();
            ISIS.Activate();
          

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView2.Columns[e.ColumnIndex].Name;
            if (columnName == "提货单编号" && e.RowIndex > -1)
            {
                string cs = dataGridView2.Rows[e.RowIndex].Cells["提货单编号"].Value.ToString();
                FormDZGHHT dy = new FormDZGHHT("查看并打印提货单", cs, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.THDDY" });
                dy.ShowDialog();
            }
            if (columnName == "发货单编号" && e.RowIndex > -1)
            {
                string cs = dataGridView2.Rows[e.RowIndex].Cells["发货单编号"].Value.ToString();
                FormDZGHHT dy = new FormDZGHHT("查看并打印发货单", cs, new string[] { "Y5_Business_HWSF.dll)!客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF.FHDDY" });
                dy.ShowDialog();
            }
        }
        /// <summary>
        /// 查看保证函
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkBZH_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel linkLable = (LinkLabel)sender;

            int pagenumber = 1;//预估页数，要大于可能的最大页数
            object[] CSarr = new object[pagenumber];
            string[] MKarr = new string[pagenumber];
            for (int i = 0; i < pagenumber; i++)
            {
                Hashtable htcs = new Hashtable();
                htcs["纯数字索引"] = (i + 1).ToString(); //这个参数必须存在。
                htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/BZH.aspx?moban=BZH.htm&Number=" + linkLable.Tag.ToString();
                htcs["要传递的参数1"] = "参数1";
                CSarr[i] = htcs; //模拟参数
                MKarr[i] = "Y3_Business_Muban.dll)!客户端主程序.SubForm.NewCenterForm.PrintUC.UCbzh";
            }
            FormDZGHHT dy = new FormDZGHHT("保证函", CSarr, MKarr);
            dy.Show();
        }

       







    }
}
