using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.IO;

namespace FMipcClass
{
    static public class HelpConfig
    {
        /// <summary>
        /// 回收应用程序池
        /// </summary>
        /// <returns>ok/err:xxxx</returns>
        static public string huishou()
        {
            //应用程序池命名，当找不到该应用程序池时，系统会报找不到指定路径
            string AppPoolName = ConfigurationManager.ConnectionStrings["ThisAppPoolName"].ToString();
            //string AppPoolName = "一组独立服务";    //应用程序池命名，当找不到该应用程序池时，系统会报找不到指定路径
            string method = "Recycle";            //启动命令， 停止：“Stop” ； 回收：“Recycle”
            try
            {
                DirectoryEntry appPool = new DirectoryEntry("IIS://127.0.0.1/W3SVC/AppPools");
                DirectoryEntry findPool = appPool.Children.Find(AppPoolName, "IIsApplicationPool");
                findPool.Invoke(method, null);
                appPool.CommitChanges();
                appPool.Close();
                return "ok";
            }
            catch (Exception ex)
            {
                return "err:" + ex.ToString() ;
            }

            
        }

        /// <summary>
        /// 获取最新关系到本地
        /// </summary>
        /// <returns>ok/err:xxxx</returns>
        static public string huoquguanxi()
        {
            object re = null;
            try
            {
                string IPCurl = ConfigurationManager.ConnectionStrings["IPCurl"].ToString(); //聚合中心地址
                string GX_shibie = ConfigurationManager.ConnectionStrings["ThisAppPoolName"].ToString();//用进程池名称作为标识
                FMWScenter wsd = new FMWScenter(IPCurl + "?wsdl");
                re = wsd.ExecuteQuery("GetGX_from_GX_shibie", new object[] { GX_shibie });

                if (re == null)
                {
                    return "err:";
                }
                else
                {
                    string dizhi = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "//ForIPC//IPClist.config";
                    ((DataSet)re).WriteXml(dizhi,XmlWriteMode.WriteSchema);
                    return "ok";
                }

            }
            catch (Exception ex)
            {
                return "err:" + ex.ToString();
            }


        }

        /// <summary>
        /// 重新生成代理类 
        /// </summary>
        /// <returns>ok/err:xxxx</returns>
        static public string shengchengdaili()
        {
            DataSet dsipc = new DataSet();
            try
            {
                string dizhi = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/ForIPC/IPClist.config";
                dsipc.ReadXml(dizhi,XmlReadMode.ReadSchema);
            }
            catch(Exception ex)
            {
                return "err:" + ex.ToString();
            }
            if (dsipc != null && dsipc.Tables.Count > 0 && dsipc.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dsipc.Tables[0].DefaultView.ToTable(true, new string[] { "接口域名", "接口地址" });
                string[] urls = new string[dt.Rows.Count+1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    urls[i] = "http://" + dt.Rows[i]["接口域名"].ToString() + "/" + dt.Rows[i]["接口地址"].ToString() + "?wsdl";
                }
            
                urls[dt.Rows.Count] = ConfigurationManager.ConnectionStrings["IPCurl"].ToString() + "?wsdl"; //聚合中心地址
                //正确的话，会返回"ok"
                FMWScenter wsd = new FMWScenter();
                string rere = wsd.GetNewWSDL(urls);
                if (rere == "ok")
                {
                    return "ok";
                }
                else
                {
                    return "err:"+rere;
                }
            }
            else
            {
                return "err:一个对应关系都没有？不可能吧？";
            }
        
        }

    }
}
