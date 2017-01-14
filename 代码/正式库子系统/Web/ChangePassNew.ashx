<%@ WebHandler Language="C#" Class="ChangePassNew" %>

using System;
using System.Web;
using FMOP.DB;

public class ChangePassNew : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (context.Request.Params["Methord"] != null)
        {
            bool b;
            string strBool = "";
            switch (context.Request.Params["Methord"].ToString())
            {
                case "PassWordCheck":
                    b = IsPassWordTrue(context.Request.Params["PassWordValue"].ToString(), context.Request.Params["UserName"].ToString());
                    if (b == true)
                    {
                        strBool = "true";
                    }
                    else
                    {
                        strBool = "false";

                    }

                    break;
                case "PassWordUpdate":
                    b = NewPassWordUpdate(context.Request.Params["PassWordValue"].ToString(), context.Request.Params["UserName"].ToString());
                    if (b == true)
                    {
                        strBool = "true";

                    }
                    else
                    {
                        strBool = "false";
                    }
                    break;
            }
            context.Response.Write(strBool);
        }
    }


    /// <summary>
    /// 判断密码是否正确
    /// </summary>
    /// <param name="oldPassWord"></param>
    /// <returns></returns>
    protected bool IsPassWordTrue(string oldPassWord,string userName)
    {
        string strSQL = "select a.Password from dbo.aspnet_Membership a , aspnet_Users b " +
              "where a.UserId=b.UserId and a.ApplicationId=b.ApplicationId and b.UserName='" +
               userName + "'";//根据账号查询密码
        object oldpw = DbHelperSQL.GetSingle(strSQL);
        if (oldpw != null && oldpw != "")
        {
            if (oldPassWord == oldpw.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="newPassWord"></param>
    /// <returns></returns>
    protected bool NewPassWordUpdate(string newPassWord,string userName)
    {
        string strUpateSQL = "UPDATE [dbo].[aspnet_Membership] SET [Password] = '" + newPassWord
                              + "' WHERE [UserId] = (select distinct UserId from aspnet_Users  where UserName='" + userName + "')";
        int i = DbHelperSQL.ExecuteSql(strUpateSQL);
        if (i > 0)
        {

            return true;
        }
        else
        {
            return false;
        }

    }
    
    
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}