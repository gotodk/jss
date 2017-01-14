using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
namespace 主控服务器
{
    /// <summary>
    /// 用于线程的委托。显示回调。用于显示线程的处理结果
    /// </summary>
    /// <param name="OutPutHT">需要返回的数据集合</param>
    public delegate void delegateForThread(Hashtable OutPutHT);

    /// <summary>
    /// 用于调用异步委托,处理非线程创建的空间,跟delegateForThread配合使用
    /// </summary>
    /// <param name="temp">需要返回的数据集合</param>
    public delegate void delegateForShow(Hashtable temp);

    /// <summary>
    /// 用于线程的委托。让线程封送线程内的方法供主窗体调用
    /// </summary>
    /// <param name="temp">需要返回的数据集合</param>
    public delegate void delegateForMainSend(string temp);


    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormWindowsServicesTry());
        }
    }
}
