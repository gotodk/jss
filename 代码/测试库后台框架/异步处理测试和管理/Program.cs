using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 异步处理测试和管理
{
    static class Program
    {
        /// <summary>
        /// 移植到其他需要防止重开的程序上。 值改这个就行。其他代码都照抄就行。
        /// </summary>
        public static string RunAPP = "异步监控";
        public static bool UseArgs = false;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);






            RedisClient RC;
            if (args.Length < 1)
            {
                //只能同时开一个的验证。 不只是同一个机器，只要是使用了相同的OnlyOne标志，都只能开一个。
                try
                {
                    RC = FMDBHelperClass.RedisClass.GetRedisClient("OnlyOpenOneCheckRedis");
                    //RC.Set("str:queIPC:" + Program.RunAPP, System.Text.Encoding.UTF8.GetBytes("CanRun"));
                    byte[] s = RC.Get("str:OnlyOpenOneCheck:" + RunAPP);
                    if (s == null || System.Text.Encoding.UTF8.GetString(s) != "CanRun")
                    {
                        //没有找到标志或标志不是0，就不能开。
                        MessageBox.Show("可能已有“" + RunAPP + "”正在运行，不允许运行！！！！！！！\n\n若谨慎确定真的只有自己在运行，\n可使用命令行添加任意参数启动本程序，来重置运行状态。");
                        System.Environment.Exit(System.Environment.ExitCode);
                    }
                }
                catch
                {
                    MessageBox.Show("无法连接Redis，不允许运行！！");
                    System.Environment.Exit(System.Environment.ExitCode);
                }

                //移除可运行状态
                RC = FMDBHelperClass.RedisClass.GetRedisClient("OnlyOpenOneCheckRedis");
                RC.Set("str:OnlyOpenOneCheck:" + Program.RunAPP, System.Text.Encoding.UTF8.GetBytes("nonono"));

            }
            else
            {
                UseArgs = true;
                //重建可运行状态
                RC = FMDBHelperClass.RedisClass.GetRedisClient("OnlyOpenOneCheckRedis");
                RC.Set("str:OnlyOpenOneCheck:" + Program.RunAPP, System.Text.Encoding.UTF8.GetBytes("CanRun"));
                MessageBox.Show("运行状态已重置！！");
                System.Environment.Exit(System.Environment.ExitCode);
            }


            



           
            

            Application.Run(new Form1());
        }
    }
}
