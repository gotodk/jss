using System;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace 客户端主程序
{
    /// <summary>
    /// 使应用程序只能运行一个实例。
    /// </summary>
    public class AppSingleton
    {
        static Mutex m_Mutex;

        //public static void Run()
        //{
        //    if (IsFirstInstance())
        //    {
        //        Application.ApplicationExit += new EventHandler(OnExit);
        //        //Application.Run();
        //    }

        //}
        //public static void Run(ApplicationContext context)
        //{
        //    if (IsFirstInstance())
        //    {
        //        Application.ApplicationExit += new EventHandler(OnExit);
        //        //Application.Run(context);
        //    }
        //}

        /// <summary>
        /// 检查登陆状态。
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="dlyx"></param>
        /// <returns>返回真代表可以登陆，返回否代表不能登陆</returns>
        public static bool CheckLoginState(FormMainPublic mainForm,string dlyx)
        {
   
                if (IsFirstInstance(dlyx))
                {
                    Application.ApplicationExit -= new EventHandler(OnExit);
                    Application.ApplicationExit -= new EventHandler(OnExit);
                    Application.ApplicationExit += new EventHandler(OnExit);

                    //Application.Run(mainForm);
                    return true;

                }
                else
                {
                    ArrayList Almsg3 = new ArrayList();
                    Almsg3.Add("");
                    Almsg3.Add("您的帐户已在本机登录，无需重复登录！");
                    FormAlertMessage FRSE3 = new FormAlertMessage("仅确定", "其他", "中国商品批发交易平台", Almsg3);
                    FRSE3.ShowDialog();

                    return false;

                }

        }
        static bool IsFirstInstance(string dlyx)
        {

            m_Mutex = new Mutex(false, "平台互斥pt.fm8844.com|" + dlyx.Trim());
            bool owned = false;
            try
            {
                owned = m_Mutex.WaitOne(TimeSpan.Zero, false);
            }
            catch (Exception ex)
            {
                return true;
            }
            return owned;
        }
        public static void OnExit(object sender, EventArgs args)
        {
            m_Mutex.ReleaseMutex();
            m_Mutex.Close();
        }
    }
}
