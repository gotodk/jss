/********************************************************************************************
//文件名：jhjx_Lock.cs
//
//创建者：wyh   移植  从第一版集合经销中心移植
//
//创建日期：2014.04.08
//
//功能描述：用户并发控制功能
 * ******************************************************************************************/

using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using FMDBHelperClass;
using System.Data;
using System.Collections;

/// <summary>
///用户并发控制功能
/// </summary>
public class jhjx_Lock
{
	public jhjx_Lock()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}


    /// <summary>
    /// 用户锁定，针对一个用户进行的业务锁定，调用此锁的地方，只能同时处理此用户一次业务。只有返回1，才能继续进行业务处理，否则取消业务处理
    /// </summary>
    /// <param name="khbh">客户编号</param>
    /// <returns>只有返回1，才能继续进行业务处理，否则取消业务处理，提示系统忙。0代表发生意外。2代表强制解锁了，也必须取消业务处理。</returns>
    public static string LockBegin_ByUser(string khbh)
    {
        
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@KHBH"] = khbh; 
        Hashtable return_ht = I_DBL.RunProc_CMD("AAA_LOCKmessage_CheckLock", "锁", input);
        string re = "0";
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsGXlist.Tables[0].Rows[0];
                if (dr[0].ToString().Trim().Equals("用户未锁定，目前已执行锁定"))
                {
                    re = "1";
                    return re;
                }
                else
                    if (dr[0].ToString().Trim().Equals("用户早已被锁定"))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Thread.Sleep(3000);
                            DataSet dslocked = new DataSet();

                            Hashtable return_htlockde = I_DBL.RunProc_CMD("AAA_LOCKmessage_CheckLock", "锁", input);
                            if ((bool)(return_htlockde["return_float"]))
                            {
                                dslocked = (DataSet)(return_htlockde["return_ds"]);
                                if (dslocked != null && dslocked.Tables[0].Rows.Count > 0)
                                {
                                    DataRow drlocked = dslocked.Tables[0].Rows[0];
                                    if (drlocked[0].ToString().Trim().Equals("用户未锁定，目前已执行锁定"))
                                    {
                                        re = "1";
                                        return re;
                                    }
                                }
                                else
                                {
                                    re = "0";
                                    return re;
                                }
                            }
                            else
                            {
                                re = "0";
                                return re;
                            }
                        }

                        //强制解锁
                        Hashtable return_htunlock = I_DBL.RunProc_CMD("AAA_LOCKmessage_UnLock", "锁", input);
                        if ((bool)(return_htunlock["return_htunlock"]))
                        {

                            re = "2";
                            return re;
                        }
                        else
                        {
                            re = "0";
                            return re;
                        }
                       
                    }
                    else
                    {
                        re = "0";
                        return re;
                    }
            }
            else
            {
                re = "0";
                return re;
            }
        }
        else
        {
            re = "0";
            return re;
        }
      
    }

    /// <summary>
    /// 解除用户锁定,与LockBegin_ByUser配对
    /// </summary>
    /// <param name="khbh">客户编号</param>
    /// <returns>这个返回值没什么用</returns>
    public static string LockEnd_ByUser(string khbh)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        input["@KHBH"] = khbh; //跟sql语句中对应的参数。
        Hashtable return_ht = I_DBL.RunProc_CMD("AAA_LOCKmessage_UnLock","锁", input);

        return "ok";

    }

}