using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using System.Data;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// fmwscenter 的摘要说明
/// </summary>
[WebService(Namespace = "http://ipc.ipc.com/", Description = "V1.00->接口聚合中心服务提供的相关服务处理。")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class fmwscenter : System.Web.Services.WebService {

    public fmwscenter () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }


    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(Description = "测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
        return "ok";
    }


    /// <summary>
    /// 获取接口调用关系的详细信息。用于缓存至调用发起方。
    /// </summary>
    /// <param name="GX_shibie">调用方识别标记</param>
    /// <returns></returns>
    [WebMethod(Description = "获取接口调用关系的详细信息。用于缓存至调用发起方。传入调用方识别标记")]
    public DataSet GetGX_from_GX_shibie(string GX_shibie)
    {
        //数据库连接初始化
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@GX_shibie"] = GX_shibie;
        string sql="select GX.GX_guid as 关系唯一标示,GX.GX_shibie as 调用方识别标记,GX.GX_savelog as 是否开启日志,FF.FF_yewuname as 业务名称,FF.FF_CorE as 操作特点,GX.GX_type as 调用方式,JK.JK_host as 接口域名,JK.JK_path as 接口地址,FF.FF_name as 方法名,FF.FF_retype as 返回值类型,FF.FF_canshu as 参数类型  from AAA_ipcGX as GX left join AAA_ipcJK as JK on GX.GX_JK_guid=JK.JK_guid  left join AAA_ipcFF as FF on GX.GX_FF_guid = FF.FF_guid and FF.FF_JK_guid = JK.JK_guid where JK.JK_open = 1 and FF.FF_open = 1 and GX.GX_open = 1 and GX.GX_shibie = @GX_shibie";
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "关系数据", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsGXlist = (DataSet)(return_ht["return_ds"]);
        }
        else
        {
            //读取数据库出错。。。
            dsGXlist = null;
        }

        return dsGXlist;
    }

    /// <summary>
    /// 将调用日志写入表。okgo:唯一编号。  这样的返回代表成功了。其他一概失败。
    /// </summary>
    /// <param name="dtgx">关系数据集</param>
    /// <param name="objCS_str">执行远程方法所用的参数被特殊序列化后的字符串数组</param>
    /// <returns></returns>
    [WebMethod(Description = "将调用日志写入表。okgo:唯一编号。  这样的返回代表成功了。其他一概失败。")]
    public string SaveCallLog(DataTable dtgx, string[] objCS_str)
    {
        try
        {
            //数据库连接初始化(实际上这里应该是用内存数据库，以及内存文件系统。 加快速度。暂不实现)
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");           
            Hashtable input = new Hashtable();
            input["@LOG_guid"] = Guid.NewGuid().ToString();
            input["@LOG_GX_guid"] = dtgx.Rows[0]["关系唯一标示"].ToString();
            input["@LOG_JK_host"] = dtgx.Rows[0]["接口域名"].ToString();
            input["@LOG_JK_path"] = dtgx.Rows[0]["接口地址"].ToString();
            input["@LOG_FF_yewuname"] = dtgx.Rows[0]["业务名称"].ToString();
            input["@LOG_FF_name"] = dtgx.Rows[0]["方法名"].ToString();
            input["@LOG_FF_retype"] = dtgx.Rows[0]["返回值类型"].ToString();
            input["@LOG_FF_canshu"] = dtgx.Rows[0]["参数类型"].ToString();
            input["@LOG_GX_type"] = dtgx.Rows[0]["调用方式"].ToString();
            input["@LOG_FF_CorE"] = dtgx.Rows[0]["操作特点"].ToString();
            input["@LOG_zt"] = 9; //默认是不确定
            Hashtable return_ht = I_DBL.RunParam_SQL("INSERT INTO  AAA_ipcLOG(LOG_guid,LOG_GX_guid,LOG_JK_host,LOG_JK_path,LOG_FF_yewuname,LOG_FF_name,LOG_FF_retype,LOG_FF_canshu,LOG_GX_type,LOG_FF_CorE,LOG_zt) VALUES (@LOG_guid,@LOG_GX_guid,@LOG_JK_host,@LOG_JK_path,@LOG_FF_yewuname,@LOG_FF_name,@LOG_FF_retype,@LOG_FF_canshu,@LOG_GX_type,@LOG_FF_CorE,@LOG_zt) ", input);
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                //必须把具体参数序列化，记录到磁盘上。

                //FileStream stream = new FileStream(Context.Request.MapPath("/objCS_" + dtgx.Rows[0]["调用方式"].ToString() + "/") + "\\" + input["@LOG_guid"].ToString() + ".txt", FileMode.Create);
                //XmlSerializer serializer = new XmlSerializer(objCS.GetType());
                //serializer.Serialize(stream, objCS);
                //stream.Close();

                string[] typestr = objCS_str[0].Split('|');
                string mainfile_str = "";
                for (int i = 0; i < typestr.Length; i++)
                {
                    string thisfilename = input["@LOG_guid"].ToString() + "_" + i + "_" + typestr[i] + ".txt";
                    if (i == typestr.Length - 1)
                    {
                        mainfile_str = mainfile_str + thisfilename;
                    }
                    else
                    {
                        mainfile_str = mainfile_str + thisfilename + "|";
                    }
                    File.WriteAllText(Context.Request.MapPath("/objCS_" + dtgx.Rows[0]["调用方式"].ToString() + "/") + "\\" + thisfilename, objCS_str[i + 1]);
                }
                File.WriteAllText(Context.Request.MapPath("/objCS_" + dtgx.Rows[0]["调用方式"].ToString() + "/") + "\\" + input["@LOG_guid"].ToString() + ".txt", mainfile_str);


                return "okgo:"+input["@LOG_guid"].ToString();
            }
            else
            {
                //数据库出错。。。
                return "errstop";
            }
        }
        catch (Exception ex)
        {
            //File.WriteAllText("d:\aaa.txt",ex.ToString() + ex.InnerException.Message);
            return "errstop";
        }
         
    }


    /// <summary>
    /// 将调用的结果更新。 传入参数顺序是： 日志唯一标示 、首次执行开始时间 、执行消耗时间、当前状态
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "将调用的结果更新。 传入参数顺序是： 日志唯一标示 、首次执行开始时间 、执行消耗时间、当前状态")]
    public string SaveCallLog_end(string LOG_guid, string LOG_begintime, string LOG_alltime, string LOG_zt, string LOG_trytext1, string LOG_xingneng)
    {
        try
        {
            //数据库连接初始化(实际上这里应该是用内存数据库，以及内存文件系统。 加快速度。暂不实现)
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");          
            Hashtable input = new Hashtable();
            input["@LOG_guid"] = LOG_guid; //日志唯一标示
            input["@LOG_begintime"] = LOG_begintime;//首次执行开始时间
            input["@LOG_alltime"] = LOG_alltime;//执行消耗时间
            input["@LOG_zt"] = LOG_zt;//当前状态
            input["@LOG_trytext1"] = LOG_trytext1;//首次执行调试信息
            input["@LOG_xingneng"] = LOG_xingneng;//性能调试信息
            Hashtable return_ht = I_DBL.RunParam_SQL("update  AAA_ipcLOG set LOG_begintime=@LOG_begintime,LOG_alltime=@LOG_alltime,LOG_zt=@LOG_zt,LOG_trytext1=@LOG_trytext1,LOG_xingneng=@LOG_xingneng where LOG_guid=@LOG_guid ", input);
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                return "ok";
            }
            else
            {
                return return_ht["return_errmsg"].ToString();
            }
        }
        catch (Exception exx)
        {
            return exx.ToString();
        }        
    }








    /// <summary>
    /// (新版)将调用日志写入表。okgo:唯一编号。  这样的返回代表成功了。其他一概失败。
    /// </summary>
    /// <param name="dtone"></param>
    /// <param name="IPCurl"></param>
    /// <param name="dtbegin"></param>
    /// <param name="jg"></param>
    /// <param name="catchstring"></param>
    /// <returns></returns>
    [WebMethod(Description = "将调用日志写入表。okgo:唯一编号。  这样的返回代表成功了。其他一概失败。")]
    public string SaveCallLogNEW(DataTable dtgx, string IPCurl, string dtbegin, string jg, string catchstring)
    {
        try
        {
            //数据库连接初始化(实际上这里应该是用内存数据库。 使用频次不高，暂不实现)
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable input = new Hashtable();
            input["@LOG_guid"] = Guid.NewGuid().ToString();
            input["@LOG_GX_guid"] = dtgx.Rows[0]["关系唯一标示"].ToString();
            input["@LOG_JK_host"] = dtgx.Rows[0]["接口域名"].ToString();
            input["@LOG_JK_path"] = dtgx.Rows[0]["接口地址"].ToString();
            input["@LOG_FF_yewuname"] = dtgx.Rows[0]["业务名称"].ToString();
            input["@LOG_FF_name"] = dtgx.Rows[0]["方法名"].ToString();
            input["@LOG_FF_retype"] = dtgx.Rows[0]["返回值类型"].ToString();
            input["@LOG_FF_canshu"] = dtgx.Rows[0]["参数类型"].ToString();
            input["@LOG_GX_type"] = dtgx.Rows[0]["调用方式"].ToString();
            input["@LOG_FF_CorE"] = dtgx.Rows[0]["操作特点"].ToString();

            int LOG_zt = (catchstring == "okle") ? 1 : 0;
            input["@LOG_zt"] = LOG_zt; //默认是不确定

            input["@LOG_begintime"] = dtbegin;//首次执行开始时间
            input["@LOG_alltime"] = jg;//执行消耗时间
            input["@LOG_trytext1"] = catchstring;//首次执行调试信息

            Hashtable return_ht = I_DBL.RunParam_SQL("INSERT INTO  AAA_ipcLOG(LOG_guid,LOG_GX_guid,LOG_JK_host,LOG_JK_path,LOG_FF_yewuname,LOG_FF_name,LOG_FF_retype,LOG_FF_canshu,LOG_GX_type,LOG_FF_CorE,LOG_zt,LOG_begintime,LOG_alltime,LOG_trytext1) VALUES (@LOG_guid,@LOG_GX_guid,@LOG_JK_host,@LOG_JK_path,@LOG_FF_yewuname,@LOG_FF_name,@LOG_FF_retype,@LOG_FF_canshu,@LOG_GX_type,@LOG_FF_CorE,@LOG_zt,@LOG_begintime,@LOG_alltime,@LOG_trytext1) ", input);
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                return "okgo:" + input["@LOG_guid"].ToString();
            }
            else
            {
                //数据库出错。。。
                return "errstop";
            }
        }
        catch (Exception ex)
        {
            return "errstop";
        }

    }






    /// <summary>
    /// 根据传入的方法中文名获取方法的日志类型、方法英文名、接口名称、接口地址信息。
    /// </summary>
    /// <param name="methodname"></param>
    /// <returns></returns>
   // [WebMethod(Description = "获取方法当前的日志类型等基本信息")]
    private DataSet GetFFinfo(string methodname,I_Dblink I_DBL)
    {
        try
        {
            //数据库连接初始化
           // I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet dsGXlist = new DataSet();
            Hashtable input = new Hashtable();
            input["@FF_yewuname"] = methodname; //日志唯一标示 

            string sql_select = "select FF_name,FF_guid,FF_log,JK_guid,JK_host,JK_path from AAA_ipcFF as FF left join AAA_ipcJK as JK on FF.FF_JK_guid=JK_guid where FF.FF_yewuname=@FF_yewuname";

            DataSet dsreturn = new DataSet();
            Hashtable return_ht = I_DBL.RunParam_SQL(sql_select, "日志类型", input);
            if ((bool)return_ht["return_float"])
            {
                return (DataSet)return_ht["return_ds"];
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 将运行日志写入数据库IPCkf的表中
    /// </summary>
    /// <param name="logparams">参数数组：方法业务名、日志类型、日志内容、方法运行主机名</param>
    /// <param name="dsAttrs">日志扩展内容，包含两个字段：扩展字段名、扩展字段内容</param>
    /// <returns></returns>

    [WebMethod(Description = "将运行日志写入数据库")]
    public string SaveWorkLog(string[] logparams, DataSet dsAttrs)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        //判断改业务当前的日志类型
        string FF_yewuname = logparams[0].ToString();
        string logtype = logparams[1].ToString();
        string logtext = logparams[2].ToString();
        string FF_hostname = logparams[3].ToString();
        DataSet ds_FFinfo = GetFFinfo(FF_yewuname, I_DBL);
        if (ds_FFinfo != null && ds_FFinfo.Tables[0].Rows.Count > 0)
        {
            if (ds_FFinfo.Tables[0].Rows[0]["FF_log"].ToString().IndexOf(logtype) < 0)
            { //没有开启此日志类型
                return "err:此日志类型未开启";
            }

            //数据库连接初始化
            ArrayList al = new ArrayList();
            Hashtable input = new Hashtable();
            input["@Log_guid"] = Guid.NewGuid().ToString(); //日志唯一标示
            input["@Log_addtime"] = DateTime.Now.ToString();//写入时间
            input["@Log_Text"] = logtext;//日志内容
            input["@Log_type"] = logtype;//日志类型
            input["@FF_guid"] = ds_FFinfo.Tables[0].Rows[0]["FF_guid"].ToString();
            input["@FF_yewuname"] = FF_yewuname;
            input["@FF_name"] = ds_FFinfo.Tables[0].Rows[0]["FF_name"].ToString();
            input["@JK_guid"] = ds_FFinfo.Tables[0].Rows[0]["JK_guid"].ToString();
            input["@JK_path"] = ds_FFinfo.Tables[0].Rows[0]["JK_path"].ToString();
            input["@JK_host"] = ds_FFinfo.Tables[0].Rows[0]["JK_host"].ToString();
            input["@FF_hostname"] = FF_hostname;
            input["@Log_AttrsCount"] = 0;
            if (dsAttrs != null &&dsAttrs.Tables.Count >0&&dsAttrs.Tables[0].Rows.Count > 0 && dsAttrs.Tables[0].Columns.Contains("Name") == true && dsAttrs.Tables[0].Columns.Contains("Value") == true)
            {
                input["@Log_AttrsCount"] = dsAttrs.Tables[0].Rows.Count.ToString();
            }

            //写日志主表数据
            string sql_insert = "INSERT INTO AAA_LogMsg(Log_guid,Log_Type,Log_Text,FF_guid,FF_yewuname,FF_name,JK_guid,JK_path,JK_host,Log_addtime,Log_AttrsCount,FF_hostname) VALUES (@Log_guid,@Log_Type,@Log_Text,@FF_guid,@FF_yewuname,@FF_name,@JK_guid,@JK_path,@JK_host,@Log_addtime,@Log_AttrsCount,@FF_hostname)";
            al.Add(sql_insert);

            //写日志从表数据
            Hashtable input_attrs = new Hashtable();
            if (dsAttrs != null&&dsAttrs .Tables .Count >0&&dsAttrs.Tables[0].Rows.Count > 0 && dsAttrs.Tables[0].Columns.Contains("Name") == true && dsAttrs.Tables[0].Columns.Contains("Value") == true)
            {
                for (int i = 0; i < dsAttrs.Tables[0].Rows.Count; i++)
                {
                    input["@Attrs_guid" + i] = Guid.NewGuid().ToString();
                    input["AttrsName" + i] = dsAttrs.Tables[0].Rows[i]["Name"].ToString();
                    input["AttrsValue" + i] = dsAttrs.Tables[0].Rows[i]["Value"].ToString();

                    string sql = "INSERT INTO AAA_LogMsgAttrs(Attrs_guid,Log_guid,AttrsName,AttrsValue) values (@Attrs_guid" + i + ",@Log_guid,@AttrsName" + i + ",@AttrsValue" + i + ")";
                    al.Add(sql);
                }
            }
            Hashtable return_htattrs = I_DBL.RunParam_SQL(al, input);
            if ((bool)return_htattrs["return_float"])
            {
                return "ok";
            }
            else
            {
                return "err:写数据库错误";
            }
        }
        else
        {
            return "err:";
        }
    }  
    
}
