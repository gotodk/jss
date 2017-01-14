using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace 客户端主程序
{
    public static class Program
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


        /// <summary>
        /// 所有商品大盘数据(仅当前获得的，可能是所有收藏的商品)
        /// </summary>
        public static DataSet AllData;

        /// <summary>
        /// 这是登陆时，所有的商品。完整的。软件不关，一直不会变
        /// </summary>
        public static DataSet ZhenAllData = null;
        /// <summary>
        /// 当前大盘开始索引
        /// </summary>
        public static Int32 nowIndex = 0;

    }
}
