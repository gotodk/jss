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

    class PublicVariable
    {
    }
    public class Program
    {
        /// <summary>
        /// 是否开启淡出
        /// </summary>
        public static bool DC_open =true;
        /// <summary>
        /// 淡出效果定时器间隔
        /// </summary>
        public static int DC_Interval = 5;
        /// <summary>
        /// 淡出效果单步增量
        /// </summary>
        public static double DC_step = 0.3;
    }
}
