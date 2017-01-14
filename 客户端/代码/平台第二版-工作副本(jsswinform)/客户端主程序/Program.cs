using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;

namespace 客户端主程序
{
    /// <summary>
    /// 用于线程的委托。显示回调。用于显示线程的处理结果，传入线程类中并在线程类中调用
    /// </summary>
    /// <param name="OutPutHT">需要返回的数据集合</param>
    public delegate void delegateForThread(Hashtable OutPutHT);
    /// <summary>
    /// 用于线程的委托。显示回调。用于显示线程的处理结果，在主线程中配合Invoke使用
    /// </summary>
    /// <param name="OutPutHT">需要返回的数据集合</param>
    public delegate void delegateForThreadShow(Hashtable temp);

    

    static class Program
    {


        /// <summary>
        /// 是否开启淡出
        /// </summary>
        public static bool DC_open;
        /// <summary>
        /// 淡出效果定时器间隔
        /// </summary>
        public static int DC_Interval;
        /// <summary>
        /// 淡出效果单步增量
        /// </summary>
        public static double DC_step;

        /// <summary>
        /// 开启工具箱哪种模式，服务商模式或用户模式
        /// </summary>
        public static string TRY;

        public static Formjhjxjszhzc fms;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //首先加载配置文件
            DataControl.XMLConfig.ReadConfig();
            //分析淡出特效参数
            if (DataControl.XMLConfig.GetConfig_NoENC("基本配置", "DC").ToString().Split(',')[0] == "1")
            {
                DC_open = true;
            }
            else
            {
                DC_open = false;
            }
            DC_Interval = System.Convert.ToInt32(DataControl.XMLConfig.GetConfig_NoENC("基本配置", "DC").ToString().Split(',')[1]);
            DC_step = System.Convert.ToDouble(DataControl.XMLConfig.GetConfig_NoENC("基本配置", "DC").ToString().Split(',')[2]);
            TRY = DataControl.XMLConfig.GetConfig_NoENC("基本配置", "TRY").ToString();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new FormMainPublic());
            
        }
    }
}
