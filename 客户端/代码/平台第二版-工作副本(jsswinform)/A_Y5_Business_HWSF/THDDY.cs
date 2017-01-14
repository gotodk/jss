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
    public partial class THDDY : UserControl
    {
        public THDDY()
        {
            InitializeComponent();
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {
            //获取参数
            object CS = this.Tag;
            //label1.Text = CS.ToString();
            lblDH.Text = CS.ToString();//提货单号
            lblTHDBH.Text = CS.ToString();//提货单号
            //开启线程，通过合同编号和买家角色编号，获取一些数据。
            //panelUCedit.Enabled = false;
            //resetloadLocation(btnDY, PBload);
            Hashtable ht = new Hashtable();
            ht["THDBH"] = CS.ToString().TrimStart('T');
            delegateForThread dft = new delegateForThread(ShowThreadResult_GetFPXX);
            NewDataControl.THDCK RCthd = new NewDataControl.THDCK(ht, dft);
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
            if (returnds == null || returnds.Tables[0].Rows.Count < 1)
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
                            DateTime time = Convert.ToDateTime(returnds.Tables["主表"].Rows[0]["提货单下达时间"].ToString());
                            string thdtime = time.Year.ToString() + "年" + time.Month.ToString().PadLeft(2, '0') + "月" + time.Day.ToString().PadLeft(2, '0') + "日";

                            lblTHDTime.Text = thdtime;
                            lblHTBH.Text = returnds.Tables["主表"].Rows[0]["电子购货合同编号"].ToString();
                            lblSelMC.Text = returnds.Tables["主表"].Rows[0]["卖家名称"].ToString()+"：";
                            DateTime ZCFHR = Convert.ToDateTime(returnds.Tables["主表"].Rows[0]["最迟发货日"].ToString());
                            string zcfhrtime = ZCFHR.Year.ToString() + "年" + ZCFHR.Month.ToString().PadLeft(2, '0') + "月" + ZCFHR.Day.ToString().PadLeft(2, '0') + "日";
                            lblZCFHR.Text = zcfhrtime + "前（最迟发货日）";
                            lblTHCS.Text = returnds.Tables["主表"].Rows[0]["此前累计提货次数"].ToString();
                            lblBCTHSL.Text = returnds.Tables["主表"].Rows[0]["本次提货数量"].ToString();
                            lblLJTHSL.Text = returnds.Tables["主表"].Rows[0]["此前累计提货数量"].ToString();
                            lblDBSL.Text = returnds.Tables["主表"].Rows[0]["定标数量"].ToString();
                            lblSYKTHSL.Text = returnds.Tables["主表"].Rows[0]["剩余可提货数量"].ToString();
                            lblBCTHJE.Text = returnds.Tables["主表"].Rows[0]["本次提货金额"].ToString();
                            lblLJTHJE.Text = returnds.Tables["主表"].Rows[0]["此前累计提货金额"].ToString();
                            lblDBJE.Text = returnds.Tables["主表"].Rows[0]["定标金额"].ToString();
                            lblSYKTHJE.Text = returnds.Tables["主表"].Rows[0]["剩余可提货金额"].ToString();
                            lblFPLX.Text = returnds.Tables["主表"].Rows[0]["发票类型"].ToString();
                            switch (returnds.Tables["主表"].Rows[0]["发票类型"].ToString())
                            {
                                case "增值税普通发票":
                                    label29.Visible = false;
                                    panelSH.Visible = false;//开票税号、银行账号
                                    panelYH.Visible = false;//开户银行
                                    panelDZ.Visible = false;//单位地址
                                    break;
                                case "增值税专用发票":
                                    label29.Visible = true;
                                    panelSH.Visible = true;//开票税号、银行账号
                                    panelYH.Visible = true;//开户银行
                                    panelDZ.Visible = true;//单位地址
                                    lblDWDZ.Text = returnds.Tables["主表"].Rows[0]["单位地址"].ToString();
                                    lblKHYH.Text = returnds.Tables["主表"].Rows[0]["开户银行"].ToString();
                                    lblYHZH.Text = returnds.Tables["主表"].Rows[0]["银行账号"].ToString();
                                    break;
                                default:
                                    break;
                            }
                            lblFPTT.Text = returnds.Tables["主表"].Rows[0]["发票抬头"].ToString();
                            lblFPSH.Text = returnds.Tables["主表"].Rows[0]["税号"].ToString();
                            lblSHR.Text = returnds.Tables["主表"].Rows[0]["收货人"].ToString();
                            lblSHLXFS.Text = returnds.Tables["主表"].Rows[0]["收货联系方式"].ToString();
                            lblSHDZ.Text = returnds.Tables["主表"].Rows[0]["收货地址"].ToString();
                            lblBZ1.Text = "1、本提货单作为" + returnds.Tables["主表"].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》的附件，与" + returnds.Tables["主表"].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》具有同等法律效力。";
                            //是否自动绑定列
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
                            Almsg4.Add("未查询到提货单信息！");
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
        ///// <summary>
        ///// 重设进度条位置，放到按钮旁边
        ///// </summary>
        ///// <param name="BB">参考提交按钮</param>
        ///// <param name="PB">要移动的进度条</param>
        //private void resetloadLocation(BasicButton BB, PictureBox PB)
        //{
        //    PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y + 23);
        //    PB.Visible = true;
        //}
    }
}
