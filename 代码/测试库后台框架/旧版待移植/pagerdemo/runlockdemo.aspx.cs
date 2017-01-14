using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
public partial class pagerdemo_runlockdemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Write("未锁定"+ System.DateTime.Now.ToShortTimeString()+"<br/>");

        //加锁
        if (ClassRunLock.LockBegin_ByUser("11111") != "1")
        {
            Response.Write("锁验证未通过，业务处理被取消<br/>");
            return; //取消业务处理
        }

        Response.Write("业务处理开始<br/>");
        //业务代码,模拟验证钱然后减钱的业务
        Thread.Sleep(15000);
        object obj = DbHelperSQL.GetSingle("select [a] from [1] where [a] > 0");
        if (obj != null)
        {
            DbHelperSQL.Query("update [1] set [a] = [a]-30");
        }
        else
        {
            Response.Write("模拟提示：钱不够了<br/>");
        }

        Response.Write("业务处理结束<br/>");

        //解锁
        ClassRunLock.LockEnd_ByUser("11111");

        Response.Write("解除锁定<br/>");
    }
}