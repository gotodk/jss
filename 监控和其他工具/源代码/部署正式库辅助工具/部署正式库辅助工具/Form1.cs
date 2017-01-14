﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;

namespace 部署正式库辅助工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重绘窗体的状态
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref   Message m)
        {
            try
            {

                base.WndProc(ref   m);
            }
            catch (Exception ex)
            {
                ;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            //删除原来的记录
            string pSavedPath1 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\svnlog.xml";
            if (File.Exists(pSavedPath1))
            {
                FileInfo fi = new FileInfo(pSavedPath1);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(pSavedPath1);
            }


            //执行命令获得历史记录
            string username = textBox1.Text;
            string password = textBox2.Text;
            string svnurl = textBox3.Text;
            string xmlsavepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\svnlog.xml";
            //string cmd = @"svn log ""https://192.168.0.26:8443/svn/PT2013/"" -v --xml --username yuhaibin --password yuhaibin  >> c:\b.txt";

            string cmd = "svn log \"" + svnurl + "\" -v -g   --xml --username " + username + " --password " + password + "  >> " + xmlsavepath + "";

            button1.Text = "读取提交历史记录>>正在读取……";
            button1.Enabled = false;
            dataGridView1.DataSource = null;



            //开启线程进行服务器验证
            Hashtable InPutHT = new Hashtable();
            InPutHT["cmd"] = cmd;
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TrayMsg);
            TH th = new TH(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(th.Run));
            trd.IsBackground = true;
            trd.Start();

        }


        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_TrayMsg(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_TrayMsg_Invoke), new Hashtable[] { OutPutHT });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }





        private void button2_Click(object sender, EventArgs e)
        {
            button2.Text = "对比表>>正在读取……";
            button2.Enabled = false;
            dataGridView2.DataSource = null;

            //开启线程进行服务器验证
            Hashtable InPutHT = new Hashtable();
            InPutHT["连接串1"] = textBox6.Text;
            InPutHT["连接串2"] = textBox5.Text;
            InPutHT["对比开头"] = textBox4.Text;
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TrayMsg);
            TH th = new TH(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(th.Run_DBduibi));
            trd.IsBackground = true;
            trd.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Text = "对比存储过程、视图、函数>>正在读取……";
            button4.Enabled = false;
            dataGridView2.DataSource = null;

            //开启线程进行服务器验证
            Hashtable InPutHT = new Hashtable();
            InPutHT["连接串1"] = textBox6.Text;
            InPutHT["连接串2"] = textBox5.Text;
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TrayMsg);
            TH th = new TH(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(th.Run_DBduibi_SPSP));
            trd.IsBackground = true;
            trd.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "对比字段>>正在读取……";
            button3.Enabled = false;
            dataGridView2.DataSource = null;

            //开启线程进行服务器验证
            Hashtable InPutHT = new Hashtable();
            InPutHT["连接串1"] = textBox6.Text;
            InPutHT["连接串2"] = textBox5.Text;
            InPutHT["对比开头"] = textBox4.Text;
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TrayMsg);
            TH th = new TH(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(th.Run_DBduibi_ziduan));
            trd.IsBackground = true;
            trd.Start();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] MyFiles;
                int i;
                // 将文件赋给一个数组。 
                MyFiles = (string[])(e.Data.GetData(DataFormats.FileDrop));     // 循环处理数组并将文件添加到列表中。 
                for (i = 0; i <= MyFiles.Length - 1; i++)
                {
                    listBox1.Items.Add(MyFiles[i]);
                }
            }
        }

        private void listBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void listBox2_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] MyFiles;
                int i;
                // 将文件赋给一个数组。 
                MyFiles = (string[])(e.Data.GetData(DataFormats.FileDrop));     // 循环处理数组并将文件添加到列表中。 
                for (i = 0; i <= MyFiles.Length - 1; i++)
                {
                    listBox2.Items.Add(MyFiles[i]);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Text = "对比文件>>正在读取……";
            button5.Enabled = false;
            listBox3.DataSource = null;

            string[] f1 = new string[listBox1.Items.Count];
            listBox1.Items.CopyTo(f1, 0);
            string[] f2 = new string[listBox2.Items.Count];
            listBox2.Items.CopyTo(f2, 0);
            //开启线程进行服务器验证
            Hashtable InPutHT = new Hashtable();
            InPutHT["files1"] = f1;
            InPutHT["files2"] = f2;
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TrayMsg);
            TH th = new TH(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(th.Run_files));
            trd.IsBackground = true;
            trd.Start();


        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.EndScroll)
            {
                int aaa = e.NewValue;
                if (aaa < hScrollBar1.Minimum)
                {
                    aaa = hScrollBar1.Minimum;
                    hScrollBar1.Value = aaa;
                }
                if (aaa > hScrollBar1.Maximum)
                {
                    aaa = hScrollBar1.Maximum;
                    hScrollBar1.Value = aaa;
                }

                dataGridView3.HorizontalScrollingOffset = aaa;
                if (dataGridView3.HorizontalScrollingOffset != aaa)
                {
                    int djlie_K = 0;
                    int zongliekuan = 0;
                    for (int i = 0; i < dataGridView3.ColumnCount; i++)
                    {

                        if (dataGridView3.Columns[i].Frozen)
                        {
                            djlie_K = djlie_K + dataGridView3.Columns[i].Width;
                        }
                        if (dataGridView3.Columns[i].Visible)
                        {
                            zongliekuan = zongliekuan + dataGridView3.Columns[i].Width;
                        }
                    }


                    //计算当前被隐藏的宽度
                    int 显示出来的宽度 = this.Width - djlie_K;
                    int 没显示出来的宽度 = zongliekuan - 显示出来的宽度;
                    double dd = zongliekuan / this.Width;
                    double cc = dataGridView3.Width / dataGridView3.HorizontalScrollingOffset;
                    MessageBox.Show(dataGridView3.HorizontalScrollingOffset.ToString() + "|" + (zongliekuan - this.Width).ToString());
                }
                //dataGridView3.FirstDisplayedScrollingColumnIndex = aaa;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //生成一个临时数据
            DataTable tblDatas = new DataTable("Datas");
            DataColumn dc = null;

            //赋值给dc，是便于对每一个datacolumn的操作
            dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
            dc.AutoIncrement = true;//自动增加
            dc.AutoIncrementSeed = 1;//起始为1
            dc.AutoIncrementStep = 1;//步长为1
            dc.AllowDBNull = false;//

            dc = tblDatas.Columns.Add("aaaaaaaa", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("bbbbbb", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("cccccc", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("dddddd", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("eeeeeee", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("ffffff", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("gggggg", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("hhhhh", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("iiiiii", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("jjjjj", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("kkkkkkk", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("llllll", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("mmmmmm", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("nnnnnn", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("oooooo", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("ppppppp", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("qqqqqq", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("rrrrr", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("sssss", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("ttttt", Type.GetType("System.String"));
            tblDatas.Rows.Add(new string[] { null, "aaaaaaaa", "sdfsdf", "234234", "345345", "45654", "22", "34", "234234", "234", "tg", "54654", "g5454", "54g5", "n6jn", "7j76j76", "67j76j67", "76j67j", "67j67j", "56j56j56", "43534" });



            //绑定临时数据
            dataGridView3.DataSource = tblDatas.DefaultView;

            //把前四列搞成冻结列
            for (int i = 0; i < dataGridView3.Columns.Count; i++)
            {
                if (i < 4)
                {
                    dataGridView3.Columns[i].Frozen = true;
                }
                else
                {
                    dataGridView3.Columns[i].Frozen = false;
                }
            }




            //模拟平台上代码，找到第一个不冻结的列的索引，并设置FirstDisplayedScrollingColumnIndex。如冻结了4列，那这里计算出来的值就是5
            int djl_index = 0;//第一个不冻结的列(冻结列只能加到最前，不然出错)
            for (int i = 0; i < dataGridView3.ColumnCount; i++)
            {

                if (dataGridView3.Columns[i].Frozen)
                {
                    djl_index++;
                }

            }
            hScrollBar1.Minimum = djl_index;
            hScrollBar1.Value = djl_index;
            //最大值就是最小值 + 剩余没有被冻结的列数。
            hScrollBar1.Maximum = 2000;

            dataGridView3.FirstDisplayedScrollingColumnIndex = djl_index;
        }

        private void dataGridView3_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void dataGridView3_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Formtempaaa F = new Formtempaaa();
            F.Show();
            //
            //int x = 3; int y = -4; int z = 5;
            //int aaa = x++ - y + (++z);
            //MessageBox.Show(aaa.ToString());
            //string logo = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\tupianshuiyin.png";
            //string oldtu = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\loginbg.jpg";

            ////缩放图标并添加水印(由于是全屏水印，因此图片是根据水印大小自动缩放的)
            //WaterImageManage WIM = new WaterImageManage();
            //Image newtu =  WIM.OnlyPingTai(logo, oldtu);
 
            ////保存图片看看效果
            //newtu.Save(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\生成预览.jpg");
            //newtu.Dispose();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Text = "开始对比>>正在对比……";
            button8.Enabled = false;
            button9.Enabled = false;
            DGVlist.Rows.Clear();
            //开启线程进行服务器验证
            Hashtable InPutHT = new Hashtable();
            InPutHT["测试库目录"] = tb_ceshi_path.Text;
            InPutHT["正式库目录"] = tb_zhengshi_path.Text;
            InPutHT["要排除的目录"] = rtb_paichu.Lines;
            InPutHT["是否同时执行拷贝"] = "否";
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TrayMsg);
            TH th = new TH(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(th.fileBeginduibi));
            trd.IsBackground = true;
            trd.Start();
        }


        //处理非线程创建的控件
        private void ShowThreadResult_TrayMsg_Invoke(Hashtable OutPutHT)
        {


            if (OutPutHT.ContainsKey("执行标记") && OutPutHT["执行标记"].ToString() == "绑定")
            {
                button1.Text = "读取提交历史记录";
                button1.Enabled = true;
                DataTable aaa = (DataTable)(OutPutHT["测试列表数据表"]);
                dataGridView1.DataSource = aaa;

                this.Text = "";
            }

            if (OutPutHT.ContainsKey("执行标记") && OutPutHT["执行标记"].ToString() == "数据表对比完成")
            {
                button2.Text = "对比表";
                button2.Enabled = true;
                button4.Text = "对比存储过程、视图、函数";
                button4.Enabled = true;
                button3.Text = "对比字段";
                button3.Enabled = true;

                DataTable aaa = (DataTable)(OutPutHT["对比结果"]);
                dataGridView2.DataSource = aaa;

                this.Text = "";
            }
            if (OutPutHT.ContainsKey("执行标记") && OutPutHT["执行标记"].ToString() == "进度显示")
            {
                this.Text = OutPutHT["进度显示"].ToString();
            }
            if (OutPutHT.ContainsKey("执行标记") && OutPutHT["执行标记"].ToString() == "文件对比")
            {
                button5.Text = "对比文件";
                button5.Enabled = true;
                listBox3.DataSource = (ArrayList)(OutPutHT["对比结果"]);
            }


            if (OutPutHT.ContainsKey("执行标记") && OutPutHT["执行标记"].ToString() == "文件对比完成")
            {

                button8.Text = "开始对比";
                button9.Text = "开始自动拷贝";
                button8.Enabled = true;
                button9.Enabled = true;
                this.Text = "";
                MessageBox.Show("处理完成。");
            }
            if (OutPutHT.ContainsKey("执行标记") && OutPutHT["执行标记"].ToString() == "完成一个")
            {
 
                string[] re = (string[])OutPutHT["执行结果"];
                DGVlist.Rows.Add(re);
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("此操作具备很大危险性，会直接覆盖文件，不可恢复，一定仔细检查再确认！\n\r\n\r是否确认执行？", "危险", MessageBoxButtons.YesNo);
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            if (tb_ceshi_path.Text.IndexOf("192.168.0.26") < 0 || tb_zhengshi_path.Text.IndexOf("192.168.0.7") < 0)
            {
                DialogResult dr2 = MessageBox.Show("正式库和测试库目录，有可能设置是错误的，请再次仔细检查！\n\r\n\r是否强制执行？", "危险", MessageBoxButtons.YesNo);
                if (dr2 != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            button9.Text = "开始自动拷贝>>正在拷贝……";
            button9.Enabled = false;
            button8.Enabled = false;
            DGVlist.Rows.Clear();
            //开启线程进行服务器验证
            Hashtable InPutHT = new Hashtable();
            InPutHT["测试库目录"] = tb_ceshi_path.Text;
            InPutHT["正式库目录"] = tb_zhengshi_path.Text;
            InPutHT["要排除的目录"] = rtb_paichu.Lines;
            InPutHT["是否同时执行拷贝"] = "是";
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_TrayMsg);
            TH th = new TH(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(th.fileBeginduibi));
            trd.IsBackground = true;
            trd.Start();
        }



    }
}
