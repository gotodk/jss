﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FMBANKpufa;
using System.Collections;
using System.IO;
using System.Threading;
namespace 银行接口协议测试
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
        }

        //查看加密前的包体
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtthis = (DataTable)(dataGridViewSet.DataSource);
            if (dtthis == null)
            { return; }
            RTBlog.SelectionColor = Color.White;
            RTBlog.AppendText("未加密的包:=========================" + Environment.NewLine);
            //转化查看
            FMBANKpufa.ClassSend CS = new ClassSend();
            Hashtable reHT = CS.Get_ByteAndString_From_DataTable(dtthis, false, "");
            byte[] bb = (byte[])(reHT["字节流"]);
            string ss = reHT["视觉检测字符串"].ToString();
            RTBlog.SelectionColor = Color.LightPink;
            RTBlog.AppendText(ss + Environment.NewLine);
            RTBlog.SelectionColor = Color.White;
            RTBlog.AppendText("============================" + Environment.NewLine);
        }


        //查看加密后的包体
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //发送数据包
        private void button3_Click(object sender, EventArgs e)
        {
            //清理日志区域
            dataGridViewShow.DataSource = null;
            Lreturnbaotou.Text = "";
            DataTable dtthis = (DataTable)(dataGridViewSet.DataSource);
            if (dtthis == null)
            { return; }


            
                //实例化委托,并参数传递给线程执行类开始执行线程
                Hashtable HTbao = new Hashtable();
                HTbao["待发送包体"] = dtthis;
                HTbao["IP地址"] = CBIP.Text;
                HTbao["端口"] = TBDK.Text;
                delegateForThread tempDFT = new delegateForThread(ShowThreadResultFFFFF);
                银行接口协议测试.ClassAcceptorMsg RCT = new 银行接口协议测试.ClassAcceptorMsg(HTbao, tempDFT);
                Thread trd = new Thread(new ThreadStart(RCT.SendTest));
                trd.IsBackground = true;
                trd.Start();





            
            //...
      
        }



        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResultFFFFF(Hashtable OutPutHT)
        {
            try
            {

                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResultFFFFF_Invoke), new Hashtable[] { OutPutHT });

            }
            catch (Exception ex)
            {
                RTBlog.SelectionColor = Color.White;
                RTBlog.AppendText("委托回调错误:=========================" + Environment.NewLine);
                RTBlog.SelectionColor = Color.Red;
                RTBlog.AppendText(ex.ToString() + Environment.NewLine);
                RTBlog.SelectionColor = Color.White;
                RTBlog.AppendText("============================" + Environment.NewLine);
            }
        }

        //处理非线程创建的控件
        private void ShowThreadResultFFFFF_Invoke(Hashtable OutPutHT)
        {

            RTBlog.AppendText("============================" + Environment.NewLine);
            RTBlog.SelectionColor = Color.LightYellow;
            RTBlog.AppendText(OutPutHT["返回的日志"].ToString() + Environment.NewLine);
            RTBlog.SelectionColor = Color.White;
            RTBlog.AppendText(OutPutHT["发包函数执行阶段"].ToString() + Environment.NewLine);
            //解包
            FMBANKpufa.ClassSend CS = new ClassSend();
            Hashtable reHT2 = CS.Get_BaotouAndDataTableAndString_From_Byte((byte[])(OutPutHT["返回的数据包"]), false, "");
            string show1 = reHT2["包头视觉检测字符串"].ToString();
            string show2 = reHT2["包体视觉检测字符串"].ToString();
            //显示解开的包
            RTBlog.SelectionColor = Color.LightPink;
            RTBlog.AppendText("返回的包头包体视觉检测：" + Environment.NewLine + show1 + show2 + Environment.NewLine);
            RTBlog.SelectionColor = Color.White;
            RTBlog.AppendText(Environment.NewLine + "============================" + Environment.NewLine);
            Lreturnbaotou.Text = "包头：" + show1;
            DataTable dtshow = (DataTable)reHT2["包体数据集"];
            dataGridViewShow.DataSource = dtshow;
        }


        private void FormTest_Load(object sender, EventArgs e)
        {
            //给richTextBox添加右键菜单
            RTBlog.MouseUp += new System.Windows.Forms.MouseEventHandler(rtb_MouseUp);

            //清理下拉框
            CBgongneng.Items.Clear();
            CBgongneng.Items.Add("");
            //从目录加载xml配置对照文件，便于测试
            string savepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\浦发\";
            List<FileInfo> FI = GetAllFilesInDirectory(savepath);
            foreach (FileInfo file in FI)
            {
                CBgongneng.Items.Add(file);
            }
             
        }

        private void CBgongneng_SelectedIndexChanged(object sender, EventArgs e)
        {
            //选择配置后，加载到测试区域
            if (CBgongneng.SelectedItem.ToString() == "")
            {
                dataGridViewSet.DataSource = null;
                return;
            }
            FileInfo FI = (FileInfo)(CBgongneng.SelectedItem);
            DataTable dtthis = new DataTable();
            dtthis.ReadXml(FI.FullName);

            dataGridViewSet.DataSource = dtthis;
            for (int i = 0; i < dataGridViewSet.Columns.Count; i++)
            {
                dataGridViewSet.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewSet.Columns[i].ToolTipText = dtthis.Columns[i].Caption;
            }

        }

        /// <summary>
        /// asciiCode转字符串
        /// </summary>
        /// <param name="asciiCode"></param>
        /// <returns></returns>
        private string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                return "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormXML FX = new FormXML();
            FX.Show();
        }


        /// <summary>
        /// 返回指定目录下的所有文件信息
        /// </summary>
        /// <param name="strDirectory"></param>
        /// <returns></returns>
        private List<FileInfo> GetAllFilesInDirectory(string strDirectory)
        {
            List<FileInfo> listFiles = new List<FileInfo>(); //保存所有的文件信息  
            DirectoryInfo directory = new DirectoryInfo(strDirectory);
            DirectoryInfo[] directoryArray = directory.GetDirectories();
            FileInfo[] fileInfoArray = directory.GetFiles();
            if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);
            foreach (DirectoryInfo _directoryInfo in directoryArray)
            {
                DirectoryInfo directoryA = new DirectoryInfo(_directoryInfo.FullName);
                DirectoryInfo[] directoryArrayA = directoryA.GetDirectories();
                FileInfo[] fileInfoArrayA = directoryA.GetFiles();
                if (fileInfoArrayA.Length > 0) listFiles.AddRange(fileInfoArrayA);
                GetAllFilesInDirectory(_directoryInfo.FullName);//递归遍历  
            }
            return listFiles;
        }




        #region richTextBox右键菜单

        private void copy_Click(object sender, EventArgs e)//复制
        {
            RichTextBox thisTRB = (RichTextBox)(contextMenuStrip1.Tag);
            thisTRB.Copy();
        }

        private void undo_Click(object sender, EventArgs e)//取消
        {
            RichTextBox thisTRB = (RichTextBox)(contextMenuStrip1.Tag);
            if (thisTRB.CanUndo)
            {
                thisTRB.Undo();
            }
        }

        private void selectall_Click(object sender, EventArgs e)//全选
        {
            RichTextBox thisTRB = (RichTextBox)(contextMenuStrip1.Tag);
            thisTRB.SelectAll();
        }

        private void delete_Click(object sender, EventArgs e)//清除
        {
            RichTextBox thisTRB = (RichTextBox)(contextMenuStrip1.Tag);
            thisTRB.Clear();
        }

        private void paste_Click(object sender, EventArgs e)//粘贴
        {
            RichTextBox thisTRB = (RichTextBox)(contextMenuStrip1.Tag);
            if ((thisTRB.SelectionLength > 0) && (MessageBox.Show("是否覆盖选中的文本?", "覆盖", MessageBoxButtons.YesNo) == DialogResult.No))
                thisTRB.SelectionStart = thisTRB.SelectionStart + thisTRB.SelectionLength;
            thisTRB.Paste();
        }

        private void clip_Click(object sender, EventArgs e)//剪切
        {
            RichTextBox thisTRB = (RichTextBox)(contextMenuStrip1.Tag);
            thisTRB.Cut();
        }

        private void redo_Click(object sender, EventArgs e)//重做
        {
            RichTextBox thisTRB = (RichTextBox)(contextMenuStrip1.Tag);
            if (thisTRB.CanRedo)
                thisTRB.Redo();
        }

        private void rtb_MouseUp(object sender, MouseEventArgs e)//控制右键菜单的显示
        {
            RichTextBox thisTRB = (RichTextBox)(sender);
            contextMenuStrip1.Tag = thisTRB;
            if (e.Button == MouseButtons.Right)
            {
                if (thisTRB.CanRedo)//redo
                    copy.Enabled = true;
                else
                    copy.Enabled = false;
                if (thisTRB.CanUndo)//undo
                    undo.Enabled = true;
                else
                    undo.Enabled = false;
                if (thisTRB.SelectionLength > 0)
                {
                    copy.Enabled = true;
                    clip.Enabled = true;
                }
                else
                {
                    copy.Enabled = false;
                    clip.Enabled = false;
                }
                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                    paste.Enabled = true;
                else
                    paste.Enabled = false;
                contextMenuStrip1.Show(thisTRB, new Point(e.X, e.Y));
            }
        }

        #endregion

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            //实例化委托,并参数传递给线程执行类开始执行线程
            delegateForThread tempDFT = new delegateForThread(ShowThreadResultSSSS);
            银行接口协议测试.ClassAcceptorMsg RCT = new 银行接口协议测试.ClassAcceptorMsg(null, tempDFT);
            Thread trd = new Thread(new ThreadStart(RCT.AcceptorTest));
            trd.IsBackground = true;
            trd.Start();
        }

        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResultSSSS(Hashtable OutPutHT)
        {
            try
            {

                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResultSSSS_Invoke), new Hashtable[] { OutPutHT });

            }
            catch (Exception ex)
            {
                RTBlog.SelectionColor = Color.White;
                RTBlog.AppendText("委托回调错误:=========================" + Environment.NewLine);
                RTBlog.SelectionColor = Color.Red;
                RTBlog.AppendText(ex.ToString() + Environment.NewLine);
                RTBlog.SelectionColor = Color.White;
                RTBlog.AppendText("============================" + Environment.NewLine);
            }
        }

        //处理非线程创建的控件
        private void ShowThreadResultSSSS_Invoke(Hashtable OutPutHT)
        {
            RTBlog.SelectionColor = Color.White;
            RTBlog.AppendText("=========================" + Environment.NewLine);
            RTBlog.SelectionColor = Color.Red;
            RTBlog.AppendText(OutPutHT["测试返回值"].ToString() + Environment.NewLine);
            RTBlog.SelectionColor = Color.White;
            RTBlog.AppendText("============================" + Environment.NewLine);
        }

    }
}
