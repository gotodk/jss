using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace 客户端主程序.DataControl
{
    /// <summary>
    /// 配置文件处理
    /// </summary>
    class XMLConfig
    {
        /// <summary>
        /// 本地配置的完整数据集(原始加密数据，仅用于保存)
        /// </summary>
        public static DataSet DSConfig_ENC;

        /// <summary>
        /// 本地配置的完整数据集(解密后数据，仅用于读取)
        /// </summary>
        public static DataSet DSConfig_NoENC;

        static string filaname = "Config.xml";
        static XMLConfig()
        {
            DSConfig_ENC = new DataSet();
            DSConfig_NoENC = new DataSet();
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

            //配置信息
            tblDatas.Rows.Add(new object[] { "测试库", "", "否", "http://www.fm8844.com" });


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
        /// 保存配置文件，保存后立刻读取，保存的为加密后数据集
        /// </summary>
        public static void SaveConfig()
        {
            try
            {
                DSConfig_ENC.WriteXml(filaname);
                ReadConfig();
            }
            catch { }

        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        public static void ReadConfig()
        {
            DSConfig_ENC.Clear();
            DSConfig_NoENC.Clear();
            DSConfig_ENC.ReadXml(filaname);
            DSConfig_NoENC = DSConfig_ENC.Copy();
            //开始解密数据
            try
            {
                for (int i = 0; i < DSConfig_NoENC.Tables[0].Rows.Count; i++)
                {
                    DSConfig_NoENC.Tables[0].Rows[i]["MSS"] = DSConfig_ENC.Tables[0].Rows[i]["MSS"];
                }
                for (int i = 0; i < DSConfig_NoENC.Tables[1].Rows.Count; i++)
                {
                    DSConfig_NoENC.Tables[1].Rows[i]["UserEmail"] = 客户端主程序.Support.StringOP.uncMe(DSConfig_ENC.Tables[1].Rows[i]["UserEmail"].ToString(), "mimamima");
                    DSConfig_NoENC.Tables[1].Rows[i]["UserPassWord"] = 客户端主程序.Support.StringOP.uncMe(DSConfig_ENC.Tables[1].Rows[i]["UserPassWord"].ToString(), "mimamima");
                }
            }
            catch { }
        }

        /// <summary>
        /// 设置加密配置文件某个数据，特殊字段自动进行加密(未更新前并不真正保存配置)
        /// </summary>
        /// <param name="tablename">配置表名</param>
        /// <param name="columnname">配置列名</param>
        /// <param name="valuestring">要设置的新值</param>
        public static void SetConfig_ENC(string tablename, string columnname,string valuestring)
        {
            if (columnname == "UserEmail" || columnname == "UserPassWord")
            {
                valuestring = 客户端主程序.Support.StringOP.encMe(valuestring, "mimamima");
            }
            DataControl.XMLConfig.DSConfig_ENC.Tables[tablename].Rows[0][columnname] = valuestring;
        }

        /// <summary>
        /// 得到解密后配置文件某个数据
        /// </summary>
        /// <param name="tablename">配置表名</param>
        /// <param name="columnname">配置列名</param>
        /// <returns></returns>
        public static string GetConfig_NoENC(string tablename, string columnname)
        {
            return DataControl.XMLConfig.DSConfig_NoENC.Tables[tablename].Rows[0][columnname].ToString();
        }
    }
}
