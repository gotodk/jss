using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace 客户端主程序.DataControl
{
    class XMLConfig
    {
        /// <summary>
        /// 本地配置的完整数据集
        /// </summary>
        public static DataSet DSConfig;
        static string filaname;
        static XMLConfig()
        {
            DSConfig = new DataSet();
            filaname = "Config.xml";
        }

        /// <summary>
        /// 生成配置文件，仅用于开发测试和标记表结构。
        /// </summary>
        public static void initConfig()
        {
            DataSet dsmain = new DataSet();
            DataTable tblDatas = new DataTable("基本配置");
            ///数据库连接字符串
            tblDatas.Columns.Add("MSS", Type.GetType("System.String"));
            //IP获取的支持网址
            tblDatas.Columns.Add("IPURL", Type.GetType("System.String"));
            //是否调试模式
            tblDatas.Columns.Add("TRY", Type.GetType("System.String"));
            //公司官方网站
            tblDatas.Columns.Add("WWWSITE", Type.GetType("System.String"));
            //淡出特效参数
            tblDatas.Columns.Add("DC", Type.GetType("System.String"));
            //配置信息
            tblDatas.Rows.Add(new object[] {  "", "", "否", "http://www.fm8844.com","1,80,0.1" });


            DataTable tblDatas_user = new DataTable("用户本地配置");
            ///用户账号
            tblDatas_user.Columns.Add("UserEmail", Type.GetType("System.String"));
            ///用户密码
            tblDatas_user.Columns.Add("UserPassWord", Type.GetType("System.String"));
            ///是否保存密码
            tblDatas_user.Columns.Add("IsSavePassWord", Type.GetType("System.String"));
            //是否自动登录
            tblDatas_user.Columns.Add("IsAutoLogin", Type.GetType("System.String"));
            //是否开启声音
            tblDatas_user.Columns.Add("IsAudio", Type.GetType("System.String"));
            //是否弹出公告提示
            tblDatas_user.Columns.Add("IsT", Type.GetType("System.String")); 


            //可允许多个用户配置
            tblDatas_user.Rows.Add(new object[] { "ceshi@ceshi.com", "1", "否", "否", "是", "是" });

            //将基本配置添加到数据集中
            dsmain.Tables.Add(tblDatas);
            //将用户本地配置添加到数据集中
            dsmain.Tables.Add(tblDatas_user);
            //写入XML
            try
            {
                for (int i = 0; i < dsmain.Tables[0].Rows.Count; i++)
                {
                    dsmain.Tables[0].Rows[i]["MSS"] = 客户端主程序.Support.StringOP.encMe(dsmain.Tables[0].Rows[i]["MSS"].ToString(), "mimamima");
                }
                for (int i = 0; i < dsmain.Tables[1].Rows.Count; i++)
                {
                    dsmain.Tables[1].Rows[i]["UserEmail"] = 客户端主程序.Support.StringOP.encMe(dsmain.Tables[1].Rows[i]["UserEmail"].ToString(), "mimamima");
                    dsmain.Tables[1].Rows[i]["UserPassWord"] = 客户端主程序.Support.StringOP.encMe(dsmain.Tables[1].Rows[i]["UserPassWord"].ToString(), "mimamima");
                }
                dsmain.WriteXml(filaname);
            }
            catch { }

        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public static void SaveConfig()
        {
            try
            {
                for (int i = 0; i < DSConfig.Tables[0].Rows.Count; i++)
                {
                    DSConfig.Tables[0].Rows[i]["MSS"] = 客户端主程序.Support.StringOP.encMe(DSConfig.Tables[0].Rows[i]["MSS"].ToString(), "mimamima");
                }
                for (int i = 0; i < DSConfig.Tables[1].Rows.Count; i++)
                {
                    DSConfig.Tables[1].Rows[i]["UserEmail"] = 客户端主程序.Support.StringOP.encMe(DSConfig.Tables[1].Rows[i]["UserEmail"].ToString(), "mimamima");
                    DSConfig.Tables[1].Rows[i]["UserPassWord"] = 客户端主程序.Support.StringOP.encMe(DSConfig.Tables[1].Rows[i]["UserPassWord"].ToString(), "mimamima");
                }
                DSConfig.WriteXml(filaname);
            }
            catch { }

        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        public static void ReadConfig()
        {
            DSConfig.Clear();
            DSConfig.ReadXml(filaname);

            try
            {
                for (int i = 0; i < DSConfig.Tables[0].Rows.Count; i++)
                {
                    DSConfig.Tables[0].Rows[i]["MSS"] = 客户端主程序.Support.StringOP.uncMe(DSConfig.Tables[0].Rows[i]["MSS"].ToString(), "mimamima");
                }
                for (int i = 0; i < DSConfig.Tables[1].Rows.Count; i++)
                {
                    DSConfig.Tables[1].Rows[i]["UserEmail"] = 客户端主程序.Support.StringOP.uncMe(DSConfig.Tables[1].Rows[i]["UserEmail"].ToString(), "mimamima");
                    DSConfig.Tables[1].Rows[i]["UserPassWord"] = 客户端主程序.Support.StringOP.uncMe(DSConfig.Tables[1].Rows[i]["UserPassWord"].ToString(), "mimamima");
                }
            }
            catch { }
        }
    }
}
