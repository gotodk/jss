using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Web.Script.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace 客户端主程序
{
    static class Program
    {
        /// <summary>
        /// 是否开启淡出
        /// </summary>
        public static bool DC_open = true;
        /// <summary>
        /// 淡出效果定时器间隔
        /// </summary>
        public static int DC_Interval = 40;
        /// <summary>
        /// 淡出效果单步增量
        /// </summary>
        public static double DC_step = 0.1;

        /// <summary>
        /// 开启工具箱哪种模式，服务商模式或用户模式
        /// </summary>
        public static string TRY;

        public static FormKTJYZH fms;

        /// <summary>
        /// 所有商品大盘数据(仅当前获得的，可能是所有收藏的商品)
        /// </summary>
        public static DataSet AllData;

        /// <summary>
        /// 大盘锁
        /// </summary>
        public static object locker_for_AllData = new object();//添加一个对象作为锁

        /// <summary>
        /// 这是登陆时，所有的商品。完整。软件不关，一直不会变
        /// </summary>
        public static DataSet ZhenAllData = null;
        /// <summary>
        /// 当前大盘开始索引
        /// </summary>
        public static Int32 nowIndex = 0;

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
            //Application.Run(new 客户端主程序.HTMLshow("查看合同详情","http://capi.fm8844.com/pingtaiservices/mobanrun/FMPTDZGHHT.aspx?Number=N130801000294",null));
            Application.Run(new FormMainPublic());



            //////Application.Run(new 客户端主程序.SubForm.NewCenterForm.PDFshowS("查看合同详情","http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/mobanrun/getpdfHT.aspx?Number=N130801000294&g=" + Guid.NewGuid().ToString()));
        }


    }
}
