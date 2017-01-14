using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace 银行监控平安
{
    public delegate void delegateForThread(Hashtable OutPutHT);

    public delegate void delegateForThreadShow(Hashtable temp);

    static class Program
    {        
        public static Hashtable state1005 = new Hashtable(); //用于清算郭程程1005接口执行情况的记录       
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //state1005初始化
            state1005.Add("状态", "开始");//执行状态
            state1005.Add("文件名", "");//存放银行生成文件名
            state1005.Add("类型", "");//银行接口FuncFlag

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormCenter());
        }
    }
}
