using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.NewDataControl;
using System.Threading;
using 客户端主程序.Support;
using 客户端主程序.DataControl;
using System.Collections;

namespace 客户端主程序.SubForm.NewCenterForm.SUBUC.HWSF
{
    public partial class FHDDY : UserControl
    {
        public FHDDY()
        {
            InitializeComponent();
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            //获取参数
            object CS = this.Tag;            
            lblDH.Text = CS.ToString();//发货单号
            lblFHDH.Text = CS.ToString();//发货单号
            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            //panelUCedit.Enabled = false;
            //resetloadLocation(btnDY, PBload);
            Hashtable ht = new Hashtable();
            ht["FHDBH"] = CS.ToString().TrimStart('F');
            delegateForThread dft = new delegateForThread(ShowThreadResult_GetFPXX);
            NewDataControl.FHDCK RCthd = new NewDataControl.FHDCK(ht, dft);
            Thread trd = new Thread(new ThreadStart(RCthd.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_GetFPXX(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_GetFPXX_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_GetFPXX_Invoke(Hashtable returnHT)
        {
            this.MinimumSize = new Size(100, 100);
            //panelUCedit.Enabled = true;
            //PBload.Visible = false;
            //显示执行结果
            DataSet returnds = (DataSet)returnHT["返回值"];
            if (returnds == null || returnds.Tables["返回值单条"].Rows.Count < 1)
            {
                ArrayList Almsg = new ArrayList();
                Almsg.Add("");
                Almsg.Add("系统繁忙！请稍后重试...");
                FormAlertMessage FRSE = new FormAlertMessage("仅确定", "其他", "提示", Almsg);
                FRSE.ShowDialog();
                return;
            }
            else
            {
                switch (returnds.Tables["返回值单条"].Rows[0]["执行结果"].ToString())
                {
                    case "err":
                        ArrayList Almsg3 = new ArrayList();
                        Almsg3.Add("");
                        Almsg3.Add(returnds.Tables["返回值单条"].Rows[0]["提示文本"].ToString());
                        FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "提示", Almsg3);
                        DialogResult dr = FRSE3.ShowDialog();
                        break;
                    case "ok":
                      
                        if (returnds != null && returnds.Tables["主表"].Rows.Count > 0)
                        {
                            DateTime time = Convert.ToDateTime(returnds.Tables["主表"].Rows[0]["发货单下达时间"].ToString());
                            string thdtime = time.Year.ToString() + "年" + time.Month.ToString().PadLeft(2, '0') + "月" + time.Day.ToString().PadLeft(2, '0') + "日";

                            lblTHDTime.Text = thdtime;
                            lblNW.Text = "根据" + returnds.Tables["主表"].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》，及贵方T" + returnds.Tables["主表"].Rows[0]["发货单编号"].ToString() + "提货单，我方已按要求将货物发出。贵方验收时以此发货单为准。";
                            lblBuyMC.Text = returnds.Tables["主表"].Rows[0]["买家名称"].ToString() + "：";                            
                            lblLJTHCS.Text = returnds.Tables["主表"].Rows[0]["此前累计发货次数"].ToString();
                            lblBCFHL.Text = returnds.Tables["主表"].Rows[0]["本次发货数量"].ToString();
                            lblLJFHL.Text = returnds.Tables["主表"].Rows[0]["此前累计发货数量"].ToString();
                            lblSHRXM.Text = returnds.Tables["主表"].Rows[0]["收货人姓名"].ToString();
                            lblSHRDH.Text = returnds.Tables["主表"].Rows[0]["收货人联系电话"].ToString();
                            lblSHDZ1.Text = returnds.Tables["主表"].Rows[0]["收货地址"].ToString();
                            lblFHRXM.Text = returnds.Tables["主表"].Rows[0]["发货人姓名"].ToString();
                            lblFHRDH.Text = returnds.Tables["主表"].Rows[0]["发货人联系电话"].ToString();
                            lblFPBH.Text = returnds.Tables["主表"].Rows[0]["发票编号"].ToString();
                            lblFPSFSHTX.Text = returnds.Tables["主表"].Rows[0]["发票是否随货同行"].ToString();
                            lblBZ1.Text = "1、本发货单作为" + returnds.Tables["主表"].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》的附件，与" + returnds.Tables["主表"].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》具有同等法律效力。";
                            lblBZ2.Text = "2、本次发货由" + returnds.Tables["主表"].Rows[0]["卖家名称"].ToString() + "自主安排物流并承担相关费用及责任，运费已包含运险费。";
                            if (returnds.Tables["主表"].Rows[0]["特殊要求"].ToString() == "是")
                            {
                                //lblYSTSYQ.Text = returnds.Tables["主表"].Rows[0]["特殊要求"].ToString() + "(" + returnds.Tables["主表"].Rows[0]["特殊要求说明"].ToString() + ")";
                                //UCtxtYSTSYQ.Text = returnds.Tables["主表"].Rows[0]["特殊要求"].ToString() + "(" + returnds.Tables["主表"].Rows[0]["特殊要求说明"].ToString() + ")";
                                UCtxtYSTSYQ.Text = returnds.Tables["主表"].Rows[0]["特殊要求说明"].ToString() ;
                            }
                            else
                            {
                                //lblYSTSYQ.Text = returnds.Tables["主表"].Rows[0]["特殊要求"].ToString();
                                //UCtxtYSTSYQ.Text = returnds.Tables["主表"].Rows[0]["特殊要求"].ToString() + "(" + returnds.Tables["主表"].Rows[0]["特殊要求说明"].ToString() + ")";
                                UCtxtYSTSYQ.Text = returnds.Tables["主表"].Rows[0]["特殊要求"].ToString();
                            }
                            lblBFBZ.Text = returnds.Tables["主表"].Rows[0]["补发备注"].ToString();
                           
                            dataGridView1.AutoGenerateColumns = false;
                            dataGridView1.DataSource = returnds.Tables["主表"].DefaultView;

                            //取消列表的自动排序
                            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                            {
                                if (i == 0)
                                {
                                    //让第一列留出一点空白
                                    DataGridViewCellStyle dgvcs = new DataGridViewCellStyle();
                                    dgvcs.Padding = new Padding(10, 0, 0, 0);
                                    this.dataGridView1.Columns[i].HeaderCell.Style = dgvcs;
                                    this.dataGridView1.Columns[i].DefaultCellStyle = dgvcs;
                                }
                                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                            }

                        }
                        else
                        {
                            ArrayList Almsg4 = new ArrayList();
                            Almsg4.Add("");
                            Almsg4.Add("未查询到发货单信息！");
                            FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "其他", "提示", Almsg4);
                            FRSE4.ShowDialog();
                            //this.Close();
                        }
                        break;
                    default:
                        ArrayList Almsg7 = new ArrayList();
                        Almsg7.Add("");
                        Almsg7.Add("系统繁忙！请稍后重试...");
                        FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "其他", "提示", Almsg7);
                        FRSE7.ShowDialog();
                        break;
                }
            }

        }
        
    }
}
