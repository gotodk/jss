using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 更新检查线程类
    /// </summary>
    class RunThreadClassUp
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public RunThreadClassUp(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun()
        {
            Thread.Sleep(500);

                //检查
                FormUp FU = (FormUp)InPutHT["更新窗体实例"];
                string cstr = FU.CheckUpdate();

                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["检查更新结果"] = cstr;//
                OutPutHT["更新窗体实例"] = FU;//
                DForThread(OutPutHT);


        }

        /// <summary>
        /// 开始下载更新
        /// </summary>
        public void BeginDownLoad()
        {
            Thread.Sleep(500);

            Hashtable htUpdateFile = (Hashtable)InPutHT["更新列表"];
            string updateUrl = InPutHT["更新地址"].ToString();
            string tempUpdatePath = InPutHT["临时目录"].ToString();
            for (int i = 0; i < htUpdateFile.Count; i++)
            {
                string[] fileArray = (string[])htUpdateFile[i];
                string UpdateFile = fileArray[0].Trim();
                string updateFileUrl = updateUrl + fileArray[0].Trim().Replace("\\", "/");


                try
                {
  

                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["正在下载"] = i;//
                    DForThread(OutPutHT);


                    string tempPath = tempUpdatePath + UpdateFile;
                    CookieContainer CC = new CookieContainer();
                    AutoUpdate.ThreadDownLoad TDL = new AutoUpdate.ThreadDownLoad(updateFileUrl, 60000, Path.GetDirectoryName(tempPath) + "\\", true, false, CC);
                    TDL.BeginDown();


                    Hashtable OutPutHT2 = new Hashtable();
                    OutPutHT2["下载完成"] = i;//
                    DForThread(OutPutHT2);

                }
                catch (Exception exxxx)
                {
                    Support.StringOP.WriteLog("下载更新文件错误：" + exxxx.ToString());
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["下载失败"] = exxxx.ToString();//
                    DForThread(OutPutHT);
                }

            }
            Hashtable OutPutHT3 = new Hashtable();
            OutPutHT3["全部下载完成"] = "全部下载完成";
            DForThread(OutPutHT3);
        }



    }
}
