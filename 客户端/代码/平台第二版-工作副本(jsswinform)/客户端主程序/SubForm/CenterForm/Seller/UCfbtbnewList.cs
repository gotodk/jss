using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;
using System.Threading;
using 客户端主程序.DataControl;

namespace 客户端主程序.SubForm.CenterForm.Seller
{
    public partial class UCfbtbnewList : UserControl
    {
        private FBtbnew FB;


        /// <summary>
        /// 获取或设置列表
        /// </summary>
        [Description("获取或设置列表"), Category("Appearance")]
        public DataTable tblist
        {
            get
            {
                return (DataTable)dataGridView1.DataSource;
            }
            set
            {
                dataGridView1.DataSource = value;
            }
        }

        public UCfbtbnewList(FBtbnew FBtemp)
        {
            InitializeComponent();

            //让列表不自动绑定
            dataGridView1.AutoGenerateColumns = false;

            FB = FBtemp;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ;
        }

        private void UCfbtbnewList_Load(object sender, EventArgs e)
        {
            
        }

        //继续购物
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (FB != null)
            {
                FB.ShowOrHideList(false,false);
                FB.addUCfbtbnew();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                //修改
                //获取选中值
                int rowindex = e.RowIndex;
                string selectname = dataGridView1.Rows[rowindex].Cells["kjmc"].Value.ToString();

                FB.ShowOrHideList(false, false);
                FB.showOrRemoveOneUC(selectname, true, false);
            }
            if (e.ColumnIndex == 1 && e.RowIndex > -1)
            {
                //删除
                //获取选中值
                int rowindex = e.RowIndex;
                string selectname = dataGridView1.Rows[rowindex].Cells["kjmc"].Value.ToString();
                FB.showOrRemoveOneUC(selectname, false,true);
                FB.ShowOrHideList(true, true);

            }
        }

        /// <summary>
        /// 重设进度条位置，放到按钮旁边
        /// </summary>
        /// <param name="BB">参考提交按钮</param>
        /// <param name="PB">要移动的进度条</param>
        private void resetloadLocation(BasicButton BB, PictureBox PB)
        {
            PB.Location = new Point(BB.Location.X + BB.Width + 30, BB.Location.Y);
            PB.Visible = true;
        }


        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="err"></param>
        private void showAlertY(string err)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add(err);
            FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "错误", "投标信息提交", Almsg3);
            FRSE3.ShowDialog();
        }

        private void ResetB_Click(object sender, EventArgs e)
        {
            ArrayList Almsg3 = new ArrayList();
            Almsg3.Add("");
            Almsg3.Add("您确定要提交投标信息吗？");
            FormAlertMessage FRSE3 = new FormAlertMessage("确定取消", "问号", "xxx", Almsg3);
            DialogResult dr = FRSE3.ShowDialog();
            if (dr != DialogResult.Yes)
            {
                return;
            }

            string JSNM = PublicDS.PublisDsUser.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();
            //开始提交数据
            DataSet InPutdsCS = ((FBtbnew)(this.Parent.Parent)).GetTJ();
            if (InPutdsCS == null || InPutdsCS.Tables["输入列表"].Rows.Count < 1)
            {
                showAlertY("请至少选择一种商品发布投标信息！");
                return;
            }

            //禁用提交区域并开启进度
            dataGridView1.Enabled = false;
            linkLabel1.Enabled = false;
            ResetB.Enabled = false;
            resetloadLocation(ResetB, PBload);


            

            //开启线程提交数据
            delegateForThread dft = new delegateForThread(ShowThreadResult_save);
            DataControl.RunThreadClassFBTHXXSave RTC = new DataControl.RunThreadClassFBTHXXSave(InPutdsCS, dft);
            Thread trd = new Thread(new ThreadStart(RTC.BeginRun));
            trd.IsBackground = true;
            trd.Start();
        }
        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_save(Hashtable returnHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_save_Invoke), new Hashtable[] { returnHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_save_Invoke(Hashtable returnHT)
        {
            //重新开放提交区域
            dataGridView1.Enabled = true;
            linkLabel1.Enabled = true;
            PBload.Visible = false;
            ResetB.Enabled = true;

            //显示执行结果
            DataSet returnds = (DataSet)returnHT["执行结果"];
            string jieguo = returnds.Tables["返回值单条"].Rows[0]["执行结果"].ToString();
            string showmsg = returnds.Tables["返回值单条"].Rows[0]["错误提示"].ToString();
            switch (jieguo)
            {
                case "ok":

                    //给出表单提交成功的提示
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add(showmsg);
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "叹号", "处理成功", Almsg3);
                    FRSE3.ShowDialog();

                    FB.reset();
                    break;
                case "系统繁忙":
                    ArrayList Almsg7 = new ArrayList();
                    Almsg7.Add("");
                    Almsg7.Add("抱歉，系统繁忙，请稍后再试……");
                    FormAlertMessage FRSE7 = new FormAlertMessage("仅确定", "错误", "系统忙", Almsg7);
                    FRSE7.ShowDialog();
                    break;
                case "err多条提示":
                    ArrayList Almsg8 = new ArrayList();
                    Almsg8.Add("");
                    Almsg8.Add(showmsg);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    for (int p = 0; p < returnds.Tables["返回值多条"].Rows.Count; p++)
                    {
                        Almsg8.Add(returnds.Tables["返回值多条"].Rows[p]["多条错误"].ToString());
                    }
                    FormAlertMessage FRSE8 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg8);
                    FRSE8.ShowDialog();
                    break;
                default:
                    ArrayList Almsg4 = new ArrayList();
                    Almsg4.Add("");
                    Almsg4.Add(showmsg);// 这里是提交失败的提示，可能是程序出错，可能是条件不足等
                    FormAlertMessage FRSE4 = new FormAlertMessage("仅确定", "错误", "提交xxx失败", Almsg4);
                    FRSE4.ShowDialog();

                    break;
            }

        }
    }
}
