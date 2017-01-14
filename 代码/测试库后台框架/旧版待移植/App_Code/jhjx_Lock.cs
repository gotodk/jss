using System;
using System.Collections.Generic;
using System.Web;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System.Threading;

/// <summary>
///jhjx_Lock 的摘要说明
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
        //判断自己的帐户操作是否被锁定。若被锁定，进入30秒等待，超时后提示系统忙，然后解除锁定。。。若未被锁定，将锁定信息写入数据库。
        string re = "0";
        object obj = DbHelperSQL.GetSingle("EXEC AAA_LOCKmessage_CheckLock '" + khbh + "'");
        if (obj != null)
        {
            if (obj.ToString() == "用户未锁定，目前已执行锁定")
            {
                re = "1";
                return re;
            }
            if (obj.ToString() == "用户早已被锁定")
            {
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(3000);
                    object objr = DbHelperSQL.GetSingle("EXEC AAA_LOCKmessage_CheckLock '" + khbh + "'");
                    if (objr != null)
                    {
                        if (objr.ToString() == "用户未锁定，目前已执行锁定")
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
                //强制解锁
                DbHelperSQL.GetSingle("EXEC AAA_LOCKmessage_UnLock '" + khbh + "'");
                re = "2";
                return re;

            }
            re = "0";
            return re;
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
        DbHelperSQL.GetSingle("EXEC AAA_LOCKmessage_UnLock '" + khbh + "'");
        return "ok";
    }

}