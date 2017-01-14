using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using FMOP.DB;
using Hesion.Brick.Core;
/// <summary>
/// SendWarring 的摘要说明
/// </summary>
public class SendMessage
{
    private string userNo = "";
    private string jobNo = "";
    private ListBox listaWoke = null;
    public string UserNo
    {
        set
        {
            userNo = value;
        }
        get
        {
            return userNo;
        }
    }

    public string JobNo
    {
        set
        {
            jobNo = value;
        }
        get
        {
            return jobNo;
        }
    }

    public SendMessage()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public SendMessage(ListBox _listaWoke,string _userNo)
    {
        listaWoke = _listaWoke;
        userNo = _userNo;
    }

    /// <summary>
    /// 发送提醒
    /// </summary>
    public void send(string module,string Number,string context)
    {
        string jobNo, toUserNo;
        string awokeStr = "";
        string moduleUrl = module + "&Number=" + Number;
        DataSet result = new DataSet();
        ArrayList awokeList = new ArrayList();

        foreach (ListItem litem in listaWoke.Items)
        {
            awokeStr = litem.Value;
            jobNo = awokeStr.Split(':')[0];
            toUserNo = awokeStr.Split(':')[1];
            if (toUserNo == "all")
            {
                //查询人员列表
                result = getUserNo(jobNo);
                if (result != null && result.Tables[0] != null)
                {
                    foreach (DataRow dr in result.Tables[0].Rows)
                    {
                        toUserNo = dr["Number"].ToString();
                        awokeList.Add(getCmdText(moduleUrl, Number, context, toUserNo, userNo));
                    }
                }
            }
            else
            {
                awokeList.Add(getCmdText(moduleUrl, Number, context, toUserNo, userNo));
            }
        }

        try
        {
           DbHelperSQL.ExecuteSqlTran(awokeList);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    /// <summary>
    /// 根据岗位获取员工编号
    /// </summary>
    /// <param name="jobNo"></param>
    /// <returns></returns>
    private DataSet getUserNo(string jobNo)
    {
        DataSet result = new DataSet();
        string cmdText = " select Number from HR_Employees where JobNo='" + jobNo + "'";
        result = DbHelperSQL.Query(cmdText);
        return result;
    }
    
    /// <summary>
    /// 根据参数构造Sql年龄
    /// </summary>
    /// <param name="moduleUrl"></param>
    /// <param name="Number"></param>
    /// <param name="context"></param>
    /// <param name="toUser"></param>
    /// <param name="formUser"></param>
    /// <returns></returns>
    private string getCmdText(string moduleUrl, string Number, string context, string toUser, string formUser)
    {
        StringBuilder sqlcmd = new StringBuilder();
        sqlcmd.Append("insert into User_Warnings(Context,Module_Url,Grade,FromUser,ToUser)");
        sqlcmd.Append("values(");
        sqlcmd.Append("'" + context + "',");
        sqlcmd.Append("'" + moduleUrl + "',");
        sqlcmd.Append("1,");
        sqlcmd.Append("'" + formUser + "',");
        sqlcmd.Append("'" + toUser + "'");
        sqlcmd.Append(")");
        return sqlcmd.ToString();
    }
}
