using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Com.Seezt.Skins;

namespace 客户端主程序
{
    public partial class FormUp : BasicForm
    {
        //淡出计时器
        System.Windows.Forms.Timer Timer_DC;

        //public FormLoginIn FLI;

        public FormUp()
        {
            InitializeComponent();
            TaskMenu.Show(this);//增加任务栏右键菜单
            //FLI = FLItemp;
        }

        private string updateUrl = string.Empty;
        private string tempUpdatePath = string.Empty;
        AutoUpdate.XmlFiles updaterXmlFiles = null;
        private int availableUpdate = 0;
        bool isRun = false;
        string mainAppExe = "";
        Hashtable htUpdateFile;




        private void DownUpdateFile()
        {
            this.Cursor = Cursors.WaitCursor;
            mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
            lbState.Text = "正在下载更新文件,请稍后...";
            pbDownFile.Value = 0;
            pbDownFile.Maximum = (int)this.lvUpdateList.Items.Count;


            Hashtable InPutHT = new Hashtable();
            InPutHT["更新列表"] = htUpdateFile;
            InPutHT["更新地址"] = updateUrl;
            InPutHT["临时目录"] = tempUpdatePath;
            delegateForThread tempDFT = new delegateForThread(ShowThreadResult_AutoUpDate);
            DataControl.RunThreadClassUp RTCU = new DataControl.RunThreadClassUp(InPutHT, tempDFT);
            Thread trd = new Thread(new ThreadStart(RTCU.BeginDownLoad));
            trd.Name = "下载更新";
            trd.IsBackground = true;
            trd.Start();


            
            
           

        }




        //显示线程处理结果的函数(用于处理线程返回数据)
        private void ShowThreadResult_AutoUpDate(Hashtable OutPutHT)
        {
            try
            {
                //调用异步委托
                Invoke(new delegateForThreadShow(ShowThreadResult_AutoUpDate_Invoke), new Hashtable[] { OutPutHT });
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("委托回调错误：" + ex.ToString());
            }
        }
        //处理非线程创建的控件
        private void ShowThreadResult_AutoUpDate_Invoke(Hashtable OutPutHT)
        {
            if (OutPutHT.ContainsKey("正在下载"))
            {
                lvUpdateList.Items[(int)OutPutHT["正在下载"]].SubItems[2].Text = "正在下载……";
            }
            if (OutPutHT.ContainsKey("下载完成"))
            {
                lvUpdateList.Items[(int)OutPutHT["下载完成"]].SubItems[2].Text = "下载完成！";
                pbDownFile.Value = (int)OutPutHT["下载完成"] + 1;
            }
            if (OutPutHT.ContainsKey("下载失败"))
            {
                MessageBox.Show("更新文件下载失败！", "其他", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(0);
            }
            if (OutPutHT.ContainsKey("全部下载完成"))
            {
                //启动外部程序，拷贝更新，重启软件
                System.Diagnostics.Process.Start("BeginCopyUp.exe", "\"" + mainAppExe + "\" \"" + tempUpdatePath);
            }
            
        }




        //创建目录
        private void CreateDirtory(string path)
        {
            if (!File.Exists(path))
            {
                string[] dirArray = path.Split('\\');
                string temp = string.Empty;
                for (int i = 0; i < dirArray.Length - 1; i++)
                {
                    temp += dirArray[i].Trim() + "\\";
                    if (!Directory.Exists(temp))
                        Directory.CreateDirectory(temp);
                }
            }
        }

        //复制文件;
        public void CopyFile(string sourcePath, string objPath)
        {
            //			char[] split = @"\".ToCharArray();
            if (!Directory.Exists(objPath))
            {
                Directory.CreateDirectory(objPath);
            }
            string[] files = Directory.GetFiles(sourcePath);
            for (int i = 0; i < files.Length; i++)
            {
                string[] childfile = files[i].Split('\\');
                File.Copy(files[i], objPath + @"\" + childfile[childfile.Length - 1], true);
            }
            string[] dirs = Directory.GetDirectories(sourcePath);
            for (int i = 0; i < dirs.Length; i++)
            {
                string[] childdir = dirs[i].Split('\\');
                CopyFile(dirs[i], objPath + @"\" + childdir[childdir.Length - 1]);
            }
        }

        //点击完成复制更新文件到应用程序目录
        private void btnFinish_Click(object sender, System.EventArgs e)
        {

            this.Close();
            this.Dispose();
            try
            {
                CopyFile(tempUpdatePath, Directory.GetCurrentDirectory());
                System.IO.Directory.Delete(tempUpdatePath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "|" + tempUpdatePath + "|" + Directory.GetCurrentDirectory());
            }
            if (true == this.isRun) Process.Start(mainAppExe);
        }

        //重新绘制窗体部分控件属性
        private void InvalidateControl()
        {
            panel1.Visible = false;
            btnNext.Visible = false;
            btnCancel.Visible = false;
            btnFinish.Location = btnCancel.Location;
            btnFinish.Visible = true;
        }
        //判断主应用程序是否正在运行
        private bool IsMainAppRun()
        {
            string mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
            bool isRun = false;
            Process[] allProcess = Process.GetProcesses();
            foreach (Process p in allProcess)
            {

                if (p.ProcessName.ToLower() + ".exe" == mainAppExe.ToLower())
                {
                    isRun = true;
                    //break;
                }
            }
            return isRun;
        }

        /// <summary>
        /// 检查是否存在更新
        /// </summary>
        /// <returns></returns>
        public string CheckUpdate()
        {
            string localXmlFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\UpdateList.xml";
            string serverXmlFile = string.Empty;
            try
            {
                //从本地读取更新配置文件信息
                updaterXmlFiles = new AutoUpdate.XmlFiles(localXmlFile);
            }
            catch
            {
                return "配置文件出错！";
            }
            //获取服务器地址
            updateUrl = updaterXmlFiles.GetNodeValue("//Url");
            AutoUpdate.AppUpdater appUpdater = new AutoUpdate.AppUpdater();
            appUpdater.UpdaterUrl = updateUrl + "UpdateList.xml";

            //与服务器连接,下载更新配置文件
            try
            {
                DateTime dtnow = System.DateTime.Now;
                //tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + string.Format("{0:yyyyMMddHHmmss}", dtnow) + "_" + "\\";
                tempUpdatePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + string.Format("{0:yyyyMMddHHmmss}", dtnow) + "_" + "\\";
                appUpdater.DownAutoUpdateFile(tempUpdatePath);
            }
            catch(Exception ex)
            {
                System.IO.Directory.Delete(tempUpdatePath, true);
                Support.StringOP.WriteLog("下载更新配置文件错误：" + ex.ToString());
                return "与服务器连接失败,操作超时，请检查网络连接! ";
            }

            //获取更新文件列表
            htUpdateFile = new Hashtable();

            serverXmlFile = tempUpdatePath + "UpdateList.xml";
            if (!File.Exists(serverXmlFile))
            {
                //System.IO.Directory.Delete(tempUpdatePath, true);
                return "更新文件列表获取失败！";
            }
            availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);

            if (availableUpdate > 0)
            {
                return "发现更新";
            }
            else
            {
                System.IO.Directory.Delete(tempUpdatePath, true);
                return "无更新";
            }

            //if (availableUpdate > 0)
            //{
            //    for (int i = 0; i < htUpdateFile.Count; i++)
            //    {
            //        string[] fileArray = (string[])htUpdateFile[i];
            //        lvUpdateList.Items.Add(new ListViewItem(fileArray));
            //    }
            //}
            //			else
            //				btnNext.Enabled = false;


        }

        private void FormUp_Load(object sender, EventArgs e)
        {
            //=======================所有窗体都有这个玩意=========
            Init_one_show();
            //====================================================

            //设置任务栏图标
            Icon ic = new Icon(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+ @"\skin\ji.ico");
            this.Icon = ic;
            
            if (availableUpdate > 0)
            {
                for (int i = 0; i < htUpdateFile.Count; i++)
                {
                    string[] fileArray = (string[])htUpdateFile[i];
                    lvUpdateList.Items.Add(new ListViewItem(fileArray));
                }
            }
            //立刻开始更新
            if (availableUpdate > 0)
            {
                DownUpdateFile();
                //MessageBox.Show("测试!", "自动更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //MessageBox.Show("没有可用的更新!", "自动更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        #region 窗体淡出，窗体最小化最大化等特殊控制，所有窗体都有这个玩意

        /// <summary>
        /// 窗体的Load事件中的淡出处理
        /// </summary>
        private void Init_one_show()
        {
            //设置双缓冲
            this.SetStyle(ControlStyles.DoubleBuffer |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
            true);
            this.UpdateStyles();

            //加载淡出计时器
            Timer_DC = new System.Windows.Forms.Timer();
            Timer_DC.Interval = Program.DC_Interval;
            this.Timer_DC.Tick += new System.EventHandler(this.Timer_DC_Tick);
            //淡出效果
            MaxDC();
        }

        /// <summary>
        /// 显示窗体时启动淡出
        /// </summary>
        private void MaxDC()
        {
            this.Opacity = 0;
            Timer_DC.Enabled = true;
        }

        //淡出显示窗体，绕过窗体闪烁问题
        private void Timer_DC_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity + Program.DC_step;
            if (!Program.DC_open)
            {
                this.Opacity = 1;
            }
            if (this.Opacity >= 1)
            {
                Timer_DC.Enabled = false;
            }
        }

        //允许任务栏最小化
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
                return cp;
            }
        }


        private int WM_SYSCOMMAND = 0x112;
        private long SC_MAXIMIZE = 0xF030;
        private long SC_MINIMIZE = 0xF020;
        private long SC_CLOSE = 0xF060;
        private long SC_NORMAL = 0xF120;
        private FormWindowState SF = FormWindowState.Normal;
        /// <summary>
        /// 重绘窗体的状态
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref   Message m)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                SF = this.WindowState;
            }
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt64() == SC_MAXIMIZE)
                {
                    MaxDC();
                    this.WindowState = FormWindowState.Maximized;
                    return;
                }
                if (m.WParam.ToInt64() == SC_MINIMIZE)
                {
                    this.WindowState = FormWindowState.Minimized;
                    return;
                }
                if (m.WParam.ToInt64() == SC_NORMAL)
                {
                    MaxDC();
                    this.WindowState = SF;
                    return;
                }
                if (m.WParam.ToInt64() == SC_CLOSE)
                {
                    this.Close();
                    return;
                }
            }
            base.WndProc(ref   m);
        }

        #endregion

        private void FormUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();  
        }

    }
}
