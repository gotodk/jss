using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using 客户端主程序.Support;
using System.Collections;

namespace 客户端主程序.SubForm.NewCenterForm
{
    public partial class UCshangchuan : UserControl
    {
        public UCshangchuan()
        {
            InitializeComponent();
        }

        ImageShow IS;
        bool[] SHOWB = null;
        /// <summary>
        /// 获取或设置三个按钮哪个显示(顺序：上传，查看，删除)
        /// </summary>
        [Description("获取或设置三个按钮哪个显示"), Category("Appearance")]
        public bool[] showB
        {
            get
            {
                return SHOWB;
            }
            set
            {
                SHOWB = value;
            }
        }


        /// <summary>
        /// 获取或设置已上传的文件
        /// </summary>
        [Description("获取或设置已上传的文件"), Category("Appearance")]
        public ListView UpItem
        {
            get
            {
                return listView1;
            }
            set
            {
                listView1 = value;
            }
        }

        /// <summary>
        /// 获取或设置文件选择对话框
        /// </summary>
        [Description("获取或设置文件选择对话框"), Category("Appearance")]
        public OpenFileDialog UpSet
        {
            get
            {
                return openFileDialog1;
            }
            set
            {
                openFileDialog1 = value;
            }
        }

        string jsbh = "xx";
        /// <summary>
        /// 获取或设置角色编号(暂时没用)
        /// </summary>
        [Description("获取或设置角色编号"), Category("Appearance")]
        public string JSBH
        {
            get
            {
                return jsbh;
            }
            set
            {
                jsbh = value;
            }
        }
        string _manageData = "";
        [Description("对资料的处理设置（①为空不作处理②加水印加字样）")]
        public string ManageData
        {
            get
            {
                return _manageData;
            }
            set {
                _manageData= value;
            }
        
        }


        private void B_SC_Click(object sender, EventArgs e)
        {

            ((Control)sender).Focus();
            //开启上传
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fName = openFileDialog1.FileNames;

                //若选择选择对话框允许选择多个，则还要检验不能超过5个
                //多不是对话框选择的，是直接指定数组，则还要检验不能重名

                //开始上传,传入文件名数组，传入对应的上传结果控件，传入回调，传入角色编号
                FormSC FSC = new FormSC(fName, listView1, new delegateForSC(UpLoadSucceed), jsbh, ManageData);
                FSC.ShowDialog();

            }
        }

        private void B_SCCK_Click(object sender, EventArgs e)
        {
            //有地址
            if (listView1.Items.Count > 0 && listView1.Items[0].SubItems[1].Text.Trim() != "")
            {
                string url = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/JHJXPT/SaveDir/" + listView1.Items[0].SubItems[1].Text.Trim().Replace(@"\", "/");
                //StringOP.OpenUrl(url);

                Hashtable htT = new Hashtable();
                htT["类型"] = "网址";
                htT["地址"] = url;
                if (IS == null || IS.IsDisposed == true)
                {
                    IS = new ImageShow(htT);
                }
                IS.Show();
                IS.Activate();
            }
        }

        private void B_SCSC_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        /// <summary>
        /// 若全部上传完成，在主窗体进行数据处理，根据情况编写，没有处理也要带着这个方法
        /// </summary>
        /// <param name="LV">上传结果集合</param>
        private void UpLoadSucceed(ListView LV)
        {
            //这里的LV,实际上是打开上传时指定的那个隐藏控件listView1，所以这个方法在多个上传按钮时，只需要写一次就行
            //MessageBox.Show("回调测试" + LV.Name);
        }

        private void UCshangchuan_Load(object sender, EventArgs e)
        {
            //默认让三个按钮都不显示
            B_SC.Visible = false;
            B_SCCK.Visible = false;
            B_SCSC.Visible = false;
        }

        private void timer_SCCK_Tick(object sender, EventArgs e)
        {

            if (SHOWB != null)
            {
                B_SC.Visible = SHOWB[0];
                B_SCCK.Visible = SHOWB[1];
                B_SCSC.Visible = SHOWB[2];
                //调整位置，若第一个上传按钮隐藏
                if (!B_SC.Visible)
                {
                    B_SCCK.Location = B_SC.Location;
                }
            }
            else
            {
                if (listView1.Items.Count > 0)
                {
                    B_SC.Visible = true;
                    B_SCCK.Visible = true;
                    B_SCSC.Visible = true;
                }
                else
                {
                    B_SC.Visible = true;
                    B_SCCK.Visible = false;
                    B_SCSC.Visible = false;
                }
            }
        }
    }
}
