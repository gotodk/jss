using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Key;
using Hesion.Brick.Core;
using System.Data.SqlClient;
using Hesion.Brick.Core.WorkFlow;
using Galaxy.ClassLib.DataBaseFactory;
using System.Web.Services;
using System.Threading;

/// <summary>
/// 用户激活休眠账户
/// </summary>
public class ActManage
{
    public ActManage()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public DataSet RunAccountActive(DataSet dsreturn, DataTable dt)
    {
        Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }


            
        string KeyNumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
        DataTable dtemp = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='1304000049'").Tables[0];
        string xm = dtemp.Rows[0]["XM"].ToString().Trim();
        string xz = dtemp.Rows[0]["XZ"].ToString().Trim();
        string zy =dtemp.Rows[0]["ZY"].ToString().Trim();
        string sjlx = dtemp.Rows[0]["SJLX"].ToString().Trim();
        string yslx = dtemp.Rows[0]["YSLX"].ToString().Trim();
        
        //获得结果
        if (dt != null && dt.Rows.Count > 0)
        {
            ClassMoney2013 cm = new ClassMoney2013();
            string dlyx = dt.Rows[0]["dlyx"].ToString().Trim();

            //判断是否已在其他电脑上激活
            DataSet objsf = DbHelperSQL.Query("select  B_SFXM,B_JSZHLX from AAA_DLZHXXB WHERE B_DLYX='" + dlyx + "'");
            if (objsf != null && objsf.Tables[0].Rows.Count > 0 && objsf.Tables[0].Rows[0]["B_SFXM"].ToString() == "否")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "cf";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "检测到帐户已在其他设备上激活，不再扣减激活费用！";
                return dsreturn;
 
            }

            string bankinfo = dt.Rows[0]["bankinfo"].ToString().Trim();
            string jszhlx = dt.Rows[0]["jszhlx"].ToString().Trim();
            string jsbh = dt.Rows[0]["jsbh"].ToString().Trim();

            double zhye = cm.GetMoneyT(dlyx, bankinfo);

            if (zhye < 100)
            {
                double ce = 100 - zhye;
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的账户可用资金余额为"+zhye.ToString("0.00")+"元，差额"+ce.ToString("0.00")+"元，休眠账户无法激活，请您增加账户资金！";
            }
            else
            {
                if (cm.FreezeMoneyT(dlyx, "100", "-"))
                {
                    //写入账款流水明细表
                    string strinsert = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],[CheckState],[CreateUser],[CreateTime]) VALUES ('"+KeyNumber+"','"+dlyx+"','"+jszhlx+"','" + jsbh + "','AAA_DLZHXXB','',getdate(),'"+yslx+"',100,'"+xm+"','"+xz+"','"+zy+"','接口编号','"+sjlx+"',0,'" + dlyx + "',GETDATE())";
                    //更新登录账号信息表
                    string text = DbHelperSQL.GetSingle("select B_XMJCJL from AAA_DLZHXXB where B_DLYX='" + dlyx + "'") == null ? "" : DbHelperSQL.GetSingle("select B_XMJCJL from AAA_DLZHXXB where B_DLYX='" + dlyx + "'").ToString();
                    string textsm = text + DateTime.Now.ToString() + "用户激活休眠账户 ";
                    string strupdate = "UPDATE AAA_DLZHXXB SET B_SFXM='否',B_JCXMSJ=GETDATE(),B_XMJCJL='"+textsm+"' WHERE B_DLYX='" + dlyx + "'";
                    List<string> list = new List<string>();
                    list.Add(strinsert);
                    list.Add(strupdate);

                    #region//2014.01.02 新增功能--根据登录邮箱判断是否是经纪人交易账户--如果是将默认经纪人是次经纪人的 交易方账户的关联记录，除“是否当前默认”记录以外的，是否首次关联为“否”、审核状态为“审核中”或“驳回”、是否当前默认为“否”的所有记录的“是否有效”字段改为“否”。
                    if (objsf != null && objsf.Tables[0].Rows.Count > 0 && objsf.Tables[0].Rows[0]["B_JSZHLX"].ToString() == "经纪人交易账户")
                    {
                        string strUpdate = "update AAA_MJMJJYZHYJJRZHGLB set SFYX='否' where SFSCGLJJR='否' and SFDQMRJJR='否' and (JJRSHZT='审核中' or JJRSHZT='驳回') and DLYX in (select DLYX from AAA_MJMJJYZHYJJRZHGLB where SFDQMRJJR='是' and JJRSHZT='审核通过' and GLJJRDLZH='" + dlyx + "')";
                        list.Add(strUpdate);
                    }
                    #endregion

                    if (DbHelperSQL.ExecuteSqlTran(list) > 0)
                    {
                        //获得结果
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的账户已经成功激活！";

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号激活出现异常，请联系客服人员！";

                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号激活未成功，请稍后再试！";
                }

            }
        }
        else 
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号激活未成功，请稍后再试！";
        }
    
        return dsreturn;

    }
}