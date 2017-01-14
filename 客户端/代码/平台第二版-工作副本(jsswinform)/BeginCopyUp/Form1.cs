using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace BeginCopyUp
{
    public partial class Form1 : Form
    {
        string[] argc;
        
        public Form1(string[] argc_temp)
        {
            InitializeComponent();
            argc = argc_temp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //设置任务栏图标
            Icon ic = new Icon("skin/ji.ico");
            this.Icon = ic;

            //停止所有主进程
            //以传入的主进程名称为依据，结束进程
            try
            {
                System.Diagnostics.Process[] killprocess = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(argc[0]));
                foreach (System.Diagnostics.Process p in killprocess)
                {
                    for (int i = 0; i < p.Threads.Count; i++)
                    {
                        p.Threads[i].Dispose();
                    }
                    p.Kill();

                }
            }
            catch {

            }


            timer1.Enabled = true;
            
        }

        //复制文件;
        public void CopyFile(string sourcePath, string objPath)
        {
            string updatelist = "";
            if (!Directory.Exists(objPath))
            {
                Directory.CreateDirectory(objPath);
            }
            string[] files = Directory.GetFiles(sourcePath);
            for (int i = 0; i < files.Length; i++)
            {
                
                if (!files[i].Contains("UpdateList.xml"))
                {
                    string[] childfile = files[i].Split('\\');
                    File.Copy(files[i], objPath + @"\" + childfile[childfile.Length - 1], true);
                }
                else
                {
                    updatelist =files[i];
                }
               
            }
            string[] dirs = Directory.GetDirectories(sourcePath);
            for (int i = 0; i < dirs.Length; i++)
            {
                string[] childdir = dirs[i].Split('\\');
                CopyFile(dirs[i], objPath + @"\" + childdir[childdir.Length - 1]);
            }
            if(updatelist!="")
            {
                File.Copy(updatelist, objPath + @"\" + "UpdateList.xml", true);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //第一个参数为主进程名称,第二个参数为拷贝临时目录路径
            try
            {



                //拷贝更新
                string tempUpdatePath = argc[1];

                CopyFile(tempUpdatePath, Directory.GetCurrentDirectory());
                System.IO.Directory.Delete(tempUpdatePath, true);


                //启动主进程
                string mainAppExe = argc[0];
                System.Diagnostics.Process.Start(mainAppExe);
                Application.Exit();

            }
            catch (Exception ex)
            {
                MessageBox.Show("更新失败："+ex.Message.ToString());
            }


            timer1.Enabled = false;
        }
    }
}
