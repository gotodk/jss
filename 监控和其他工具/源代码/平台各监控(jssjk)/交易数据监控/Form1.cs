using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace 交易数据监控
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //监控有顺序的区别，注意！！
            //红色监控，正常每隔几秒就跑一次的
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "中标监控(三个月一年)", "只处理三个月一年的", "红色", "查看", "中标监控_x0028_三个月一年_x0029_", "1", "单独运行" });
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "即时交易监控", "只处理即时交易", "红色", "查看", "即时交易监控", "2", "单独运行" });//serverZBJK_JSJY
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "重新生成大盘监控", "包括大盘和大盘底部统计", "红色", "查看", "重新生成大盘监控", "3", "单独运行" });//CreatNewDapanList_Group


            //监控有顺序的区别，注意！！
            //蓝色监控，正常在凌晨刚过十分钟左右跑
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "预订单过期", "必须跑在其他蓝色监控之前", "蓝色", "查看", "预订单过期", "0", "顺序运行" });//serverGQJK
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "延迟发货监控", "只处理三个月一年的", "蓝色", "查看", "延迟发货监控", "0", "顺序运行" });//serverRunFHYC
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "延迟发票监控", "只处理三个月一年的", "蓝色", "查看", "延迟发票监控", "0", "顺序运行" });
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "即时延迟发货", "只处理即时的", "蓝色", "查看", "即时延迟发货", "0", "顺序运行" });//serverRunFHYC_onlyJSJY
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "计算履约保证金不足的合同", "违规废标处理", "蓝色", "查看", "计算履约保证金不足的合同", "0", "顺序运行" });//serverRunLYBZJbuzu

            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "提醒后自动签收", "自动默认签收", "蓝色", "查看", "提醒后自动签收", "0", "顺序运行" });//serverRunZDQS
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "计算经纪人收益扣税", "对个人用户自动扣税", "蓝色", "查看", "计算经纪人收益扣税", "0", "顺序运行" });//ServerMYJJRKS

            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "合同期满处理", "处理到期的合同", "蓝色", "查看", "执行合同到期处理监控", "0", "顺序运行" });//ServerHTQMJK
            DSmain.Tables["监控列表"].Rows.Add(new object[] { false, "超期未缴纳履约保证金监控", "无", "蓝色", "查看", "执行超期未缴纳履约保证金监控", "0", "顺序运行" });//serverWJNLYBZJJK

            dataGridView1.DataSource = DSmain.Tables["监控列表"].DefaultView;


            toolStripComboBox3_Click(null, null);
            this.tabControl1.SelectedIndex = 1;
            this.toolStripComboBox4.Items.Add("请选择日志文件");
            if (DSmain.Tables["监控列表"].Rows.Count>0)
            {
                for (int i = 0; i < DSmain.Tables["监控列表"].Rows.Count; i++)
                {
                    this.toolStripComboBox4.Items.Add(DSmain.Tables["监控列表"].Rows[i][1].ToString()+".log");
                }
            }


            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //处理列点击
            int lie = e.ColumnIndex;
            int hang = e.RowIndex;
            if (hang < 0)
            {
                return;
            }
            if (lie == 0) //选中
            {
                 //string aaa= dataGridView1.Rows[hang].Cells[lie].Value.ToString();
               
                    bool oldV = (bool)(dataGridView1.Rows[hang].Cells[lie].Value);
                    DSmain.Tables["监控列表"].Rows[hang]["选中"] = !oldV;
               
            }
            if (lie==4) //看日志，预留
            {
                this.tabControl1.SelectedIndex = 0;
                for (int i = 0; i < this.toolStripComboBox4.Items.Count; i++)
                {
                    if (this.toolStripComboBox4.Items[i].ToString().Replace(".log","").Equals(dataGridView1.Rows[hang].Cells["监控名称"].Value.ToString()))
                    {
                        this.toolStripComboBox4.SelectedIndex = i;
                        break;
                    }
                }
                toolStripComboBox4_SelectedIndexChanged(null,null);
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //双击单独运行一个监控
            int lie = e.ColumnIndex;
            int hang = e.RowIndex;
            if (hang < 0)
            {
                return;
            }
            DataSet dsone = DSmain.Copy();
            int allnum = DSmain.Tables["监控列表"].Rows.Count;
            for (int i = 0; i < allnum; i++)
            {
                dsone.Tables["监控列表"].Rows[i]["选中"] = false;
            }
            dsone.Tables["监控列表"].Rows[hang]["选中"] = true;
            //实例化委托,并参数传递给线程执行类开始执行线程
            Hashtable InPutHT = new Hashtable();
            InPutHT["监控列表集合"] = dsone;
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_Log);
            ClassThreadRun RTCBM = new ClassThreadRun(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCBM.BeginRun_now));
            trd.IsBackground = true;
            trd.Start();

            groupBox1.Text = "监控正在运行中……";
            groupBox1.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //实例化委托,并参数传递给线程执行类开始执行线程
            Hashtable InPutHT = new Hashtable();
            InPutHT["监控列表集合"] = DSmain.Copy();
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_Log);
            ClassThreadRun RTCBM = new ClassThreadRun(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCBM.BeginRun_now));
            trd.IsBackground = true;
            trd.Start();

            groupBox1.Text = "监控正在运行中……";
            groupBox1.Enabled = false;
      
        }

                //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_Log(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_Log_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("委托回调错误：" + ex.ToString(),"委托回调错误日志");
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_Log_Invoke(Hashtable OutPutHT)
        {


            string name = OutPutHT["执行任务名称"].ToString();
            if (name == "本次执行完成")
            {
                groupBox1.Text = "操作";
                groupBox1.Enabled = true;
            }

            string begintime = OutPutHT["开始时间"].ToString();
            string endtime = OutPutHT["结束时间"].ToString();
            string restr = OutPutHT["执行结果"].ToString();
            string otherstr = OutPutHT["其他描述"].ToString();
            richTextBox2.AppendText(">>>>执行任务名称:" + name + ",开始时间:" + begintime + ",结束时间:" + endtime + ",执行结果:" + restr + ",其他描述:" + otherstr + "\r\n");
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet dsPrivous = DSmain.Copy();
            //实例化委托,并参数传递给线程执行类开始执行线程
             //跑顺序执行的。
            Hashtable InPutHT = new Hashtable();
            InPutHT["监控列表集合"] =GetData(dsPrivous.Tables[0].Select("运行方式='顺序运行'"),dsPrivous);
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_Log);
            ClassThreadRun RTCBM = new ClassThreadRun(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCBM.BeginRun));
            trd.IsBackground = true;
            trd.Start();
   
            //拆开独立线程跑独立执行的
            DataSet dsDDZX = GetData(dsPrivous.Tables[0].Select("运行方式='单独运行'"), dsPrivous);
            for(int i=0;i<dsDDZX.Tables[0].Rows.Count;i++)
            {
             //实例化委托,并参数传递给线程执行类开始执行线程
                Hashtable InPutHTDDZX = new Hashtable();
                InPutHTDDZX["监控列表集合"] = GetData(dsDDZX.Tables[0].Select("线程编号='" + dsDDZX.Tables[0].Rows[i]["线程编号"].ToString() + "'"), dsDDZX);
                delegateForThread tempDFTDDZX = new delegateForThread(ShowThreadResult_Log);
                ClassThreadRun RTCBMDDZX = new ClassThreadRun(InPutHTDDZX, tempDFTDDZX);
                Thread trdDDZX = new Thread(new ThreadStart(RTCBMDDZX.BeginRun));
                trdDDZX.IsBackground = true;
                trdDDZX.Start();
            }
         
 
            groupBox1.Text = "监控正在运行中……";
            groupBox1.Enabled = false;
        }

        /// <summary>
        ///获得筛选后的数据信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="dataPrivous"></param>
        /// <returns></returns>
        public DataSet GetData(DataRow[] dataRow,DataSet dataPrivous)
        {
            //DataRow[] dataRowClone = dataRow;
            DataSet dataSet = new DataSet();
            DataTable dataTable = dataPrivous.Tables[0].Clone();
            dataTable.TableName = dataPrivous.Tables[0].TableName.ToString();
            foreach (DataRow dataRowSingle in dataRow)
            {
                dataTable.Rows.Add(dataRowSingle.ItemArray);
            }
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }





        #region//RichTextBox处理的文本文档的内容
        private int _selectIndex;
        private int _selectLength;
        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Cut();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Copy();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Paste();
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectedText = "";
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectAll();
        }
     
        private void SetTest(string str)
        {
            this.toolStripTextBox1.Text = str;
        }    //Search();

        private void SearchTest()
        {

            Search();

        }





        private void 工具栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem mm = (ToolStripMenuItem)sender;
            if (mm.Checked == true)
            {
                this.toolStrip1.Visible = true;
            }
            else
            {

                this.toolStrip1.Visible = false;
            }

        }


        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.自动换行ToolStripMenuItem.Checked == true)
            {
                this.richTextBox1.WordWrap = true;

            }
            else
            {
                this.richTextBox1.WordWrap = false;
            }

        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)//查找
        {
            FindIndex(0);
        }


        private int FindIndex(int startIndex)//查找方法
        {

            this.richTextBox1.Select(_selectIndex, _selectLength);//选中找到的文本
            this.richTextBox1.SelectionBackColor = this.richTextBox1.BackColor;//改变选中文本的颜色为系统颜色
            int index = this.richTextBox1.Find(this.toolStripTextBox1.Text, startIndex, RichTextBoxFinds.None);//得到符合文本的索引
            if (index != -1)
            {
                this.richTextBox1.Select(index, this.toolStripTextBox1.Text.Length);
                _selectIndex = index;//保存本次得到的文本
                _selectLength = this.toolStripTextBox1.Text.Length;//

                this.richTextBox1.SelectionBackColor = Color.Yellow;
            }

            return index;
        }

        private void Search()
        {
            int index = FindIndex(this.richTextBox1.SelectionStart + this.richTextBox1.SelectionLength);
            if (index == -1)
            {
                FindIndex(0);
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)//下一个
        {
            Search();

        }
        private void toolStripComboBox1_Click(object sender, EventArgs e)//设置字体名称
        {

            this.richTextBox1.Font = new Font(this.toolStripComboBox1.SelectedItem.ToString(), this.richTextBox1.Font.Size);

            

        }

        private void toolStripComboBox2_Click(object sender, EventArgs e)//设置字体大小
        {
            this.richTextBox1.Font = new Font(this.richTextBox1.Font.Name, float.Parse(this.toolStripComboBox2.SelectedItem.ToString()));
         
        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "副文本(*.txt)|*.txt";
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fs))
                {
                    this.richTextBox1.Rtf = sr.ReadToEnd();
                }
            }


        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "副文本(*.txt)|*.txt";
            sfd.InitialDirectory = "C:\\";
            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
                {
                    sw.Write(this.richTextBox1.Rtf);
                    sw.Flush();
                }
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = this.richTextBox1.Font;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.richTextBox1.Font = fd.Font;

            }

        }

        private void 颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                //this.richTextBox1.BackColor = cd.Color;
                this.richTextBox1.ForeColor = cd.Color;
            }
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (this.richTextBox1.Text != "")
            //{
            //    保存ToolStripMenuItem_Click(sender, e);
            //}
            if (this.richTextBox1.Text.Length == 0)
                return;
            DialogResult r = MessageBox.Show("是否保存?", "保存", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
            {
                Save();
                this.richTextBox1.Clear();
            }

        }
         





        #endregion

        private void toolStripComboBox3_Click(object sender, EventArgs e)
        {
            this.toolStripComboBox3.Items.Clear();
            //加载日志文件夹的列表
            string[] strArray = StringOP.GetDirtoryInfor("文件夹列表");

            if (strArray.Length > 0)
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = strArray[i].Substring(strArray[i].ToString().LastIndexOf("\\")+1);
                
                }
                    this.toolStripComboBox3.Items.AddRange(strArray);
               
            }
            else
            {
                this.toolStripComboBox3.Items.Add("没有日志信息！");
            }
            this.toolStripComboBox3.SelectedIndex = 0;
        }

        //获取文件列表
        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.toolStripComboBox4.Items.Clear();
            //this.toolStripComboBox4.Text = "";
            //if (this.toolStripComboBox3.SelectedItem.ToString().Trim().Equals("没有日志信息！"))
            //{
            //    this.toolStripComboBox4.Items.Add("没有日志信息！");
            //    return;
            //}


            ////加载日志文件夹的列表
            //string[] strArray = StringOP.GetDirtoryFiles(this.toolStripComboBox3.SelectedItem.ToString());

            //if (strArray.Length > 0)
            //{
            //    for (int i = 0; i < strArray.Length; i++)
            //    {
            //        strArray[i] = strArray[i].Substring(strArray[i].ToString().LastIndexOf("\\") + 1);

            //    }
            //    this.toolStripComboBox4.Items.AddRange(strArray);
            //}
            //else
            //{
            //    this.toolStripComboBox4.Items.Add("没有日志信息！");
            //}
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.toolStripComboBox3.SelectedItem != null && this.toolStripComboBox4.SelectedItem != null)
            {
              if(File.Exists(StringOP.GetFillAllName(this.toolStripComboBox3.SelectedItem.ToString(), this.toolStripComboBox4.SelectedItem.ToString())))
              {
               FileStream fs = new FileStream(StringOP.GetFillAllName(this.toolStripComboBox3.SelectedItem.ToString(), this.toolStripComboBox4.SelectedItem.ToString()), FileMode.Open, FileAccess.Read);
                
                using (StreamReader sr = new StreamReader(fs))
                {
                    this.richTextBox1.Text = sr.ReadToEnd();
                }
              }
             else
              {
                this.richTextBox1.Text ="没有对应的日志信息！";
              }


               
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
            else
            {
                dataGridView1.Cursor = Cursors.Default;
            }
      
        }

      



    }
}
