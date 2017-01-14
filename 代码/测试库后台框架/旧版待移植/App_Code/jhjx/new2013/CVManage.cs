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
/// CVManage 的摘要说明
/// </summary>
public class CVManage
{
    public CVManage()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public DataSet RunFileData(DataSet dsreturn, DataTable dt)
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


        //进行处理....
        if (dt != null && dt.Rows.Count > 0)
        {
            DataTable dtemp = DbHelperSQL.Query("SELECT B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,J_BUYJSBH AS 买家角色编号 FROM AAA_DLZHXXB WHERE B_DLYX='" + dt.Rows[0]["dlyx"].ToString().Trim() + "'").Tables[0];
            string jszhlx = dtemp.Rows[0]["结算账户类型"].ToString().Trim();
            string jsbh = dtemp.Rows[0]["买家角色编号"].ToString().Trim();
            string yhm = dtemp.Rows[0]["用户名"].ToString().Trim();
            string str = "UPDATE AAA_ZBDBXXB SET Q_QPZMSCSJ=GETDATE(),Q_CLYJ='" + dt.Rows[0]["clyj"].ToString().Trim() + "', Q_ZMSCFDLYX='" + dt.Rows[0]["dlyx"].ToString().Trim() + "',Q_ZMSCFJSZHLX ='" + jszhlx + "',Q_ZMSCFJSBH='" + jsbh + "',Q_ZMWJLJ='" + dt.Rows[0]["wjlj"].ToString().Trim() + "',Q_ZFLYZH ='" + dt.Rows[0]["lyzh"].ToString().Trim() + "',Q_ZFMBZH ='" + dt.Rows[0]["mbzh"].ToString().Trim() + "',Q_ZFJE=" + dt.Rows[0]["zfje"].ToString().Trim() + " WHERE Number='" + dt.Rows[0]["num"].ToString().Trim() + "'";
            List<string> list = new List<string>();
            list.Add(str);

            if (DbHelperSQL.ExecuteSqlTran(list) > 0)
            {
               
                //提醒
                string dfyx = dt.Rows[0]["dfyx"].ToString().Trim();
                string dfjslx = dt.Rows[0]["dfjslx"].ToString().Trim();

                DataTable df= DbHelperSQL.Query("SELECT B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,J_BUYJSBH AS 买家角色编号 FROM AAA_DLZHXXB WHERE B_DLYX='" + dfyx + "'").Tables[0];

                Hashtable ht = new Hashtable();
                ht["提醒对象登陆邮箱"] = dfyx;
                ht["提醒对象结算账户类型"] = df.Rows[0]["结算账户类型"].ToString().Trim();
                ht["提醒对象角色编号"] = df.Rows[0]["买家角色编号"].ToString().Trim();
                ht["提醒对象角色类型"] = dfjslx;
                ht["提醒内容文本"] = yhm + "上传了关于编号为" + dt.Rows[0]["htbh"].ToString().Trim() + "的电子购货合同的争议的证明文件，请进行确认。";
                ht["创建人"] = dt.Rows[0]["dlyx"].ToString().Trim();
                ht["type"] = "集合集合经销平台";
                List<Hashtable> ls = new List<Hashtable>();
                ls.Add(ht);
                PublicClass2013.Sendmes(ls);

                //获得结果
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "确认成功！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "tj";
                

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "提交失败，请稍后重试！";

            }
        }
        else
        {

            //获得结果
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "提交失败，请稍后重试！";

            //附加数据表
        }
        return dsreturn;

    }
    //争议文件确认
    public DataSet RunConfirmData(DataSet dsreturn, DataTable dt)
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

        //进行处理....
        if (dt != null && dt.Rows.Count > 0)
        {
            DataTable dtemp = DbHelperSQL.Query("SELECT B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,J_BUYJSBH AS 买家角色编号 FROM AAA_DLZHXXB WHERE B_DLYX='" + dt.Rows[0]["dlyx"].ToString().Trim() + "'").Tables[0];

            //判断是否进行了重复确认
            object objqr = DbHelperSQL.GetSingle("select Q_SFYQR from AAA_ZBDBXXB where Number='" + dt.Rows[0]["num"].ToString().Trim() + "'");
            if (objqr != null && objqr.ToString().Trim() == "是")
            {
                //获得结果
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "cf";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "检测到已经在其他设备上进行了确认，无需重复操作！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "qr";
                return dsreturn;
 
            }

            string yhm = dtemp.Rows[0]["用户名"].ToString().Trim();
            string str = "UPDATE AAA_ZBDBXXB SET Q_QRSJ=GETDATE(),Q_SFYQR='是' WHERE Number='" + dt.Rows[0]["num"].ToString().Trim() + "'";
            List<string> list = new List<string>();
            list.Add(str);

            if (DbHelperSQL.ExecuteSqlTran(list) > 0)
            {
                //获得结果
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "确认成功！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "qr";

                //提醒
                string dfyx = dt.Rows[0]["dfyx"].ToString().Trim();
                string dfjslx = dt.Rows[0]["dfjslx"].ToString().Trim();

                DataTable df = DbHelperSQL.Query("SELECT B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,J_BUYJSBH AS 买家角色编号 FROM AAA_DLZHXXB WHERE B_DLYX='" + dfyx + "'").Tables[0];

                Hashtable ht = new Hashtable();
                ht["提醒对象登陆邮箱"] = dfyx;
                ht["提醒对象结算账户类型"] = df.Rows[0]["结算账户类型"].ToString().Trim();
                ht["提醒对象角色编号"] = df.Rows[0]["买家角色编号"].ToString().Trim();
                ht["提醒对象角色类型"] = dfjslx;
                ht["提醒内容文本"] = yhm + "对您上传的关于编号为" + dt.Rows[0]["htbh"].ToString().Trim() + "的电子购货合同的争议证明文件进行了确认，请进行查看。";
                ht["创建人"] = dt.Rows[0]["dlyx"].ToString().Trim();
                ht["type"] = "集合集合经销平台";
                List<Hashtable> ls = new List<Hashtable>();
                ls.Add(ht);

                //操作平台 
                Hashtable htyept = new Hashtable();
                htyept["type"] = "业务平台";
                htyept["模提醒模块名"] = "人工清盘";
                htyept["查看地址"] = "";
                htyept["提醒内容文本"] = yhm + "对关于编号为" + dt.Rows[0]["htbh"].ToString().Trim() + "的电子购货合同的争议证明文件进行了确认，请进行人工清盘处理。";
                ls.Add(htyept);              

                PublicClass2013.Sendmes(ls);

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "确认失败，请稍后重试！";
            }
        }
        else
        {

            //获得结果
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "确认失败，请稍后重试！";

            //附加数据表
        }
        return dsreturn;

 
    }
}