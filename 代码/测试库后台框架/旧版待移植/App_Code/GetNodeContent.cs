using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// GetNodeContent 的摘要说明
/// </summary>
public class GetNodeContent
{
    private string roleName;
    private string roleType;
    private string canAdd;
    private string canView;
    private string canModify;
    private string viewType;
    private string modifyType;

    public GetNodeContent(DataRow row)
    {
        roleName = row["roleName"].ToString();
        try
        {
            roleType = row["roleType"].ToString();
        }
        catch
        {
            roleType = "错误";
        }
        try
        {
            canAdd = row["canAdd"].ToString();
        }
        catch
        {
            canAdd = "false";
        }
        try
        {
            canView = row["canView"].ToString();
        }
        catch
        {
            canView = "false";
        }
        try
        {
            canModify = row["canModify"].ToString();
        }
        catch
        {
            canModify = "false";
        }
        try
        {
            viewType = row["viewType"].ToString();
        }
        catch
        {
            viewType = "0";
        }
        try
        {
            modifyType = row["modifyType"].ToString();
        }
        catch
        {
            modifyType = "0";
        }
    }
    public string getroleName
    {
        get
        {
            if (roleType == "3")
            { 
                roleName=Hesion .Brick .Core .Users .GetUserByNumber(roleName).Name;
            }
            return roleName;
        }
    }
    public string getroleType
    {
        get
        {
            if (roleType == "1")
            {
                roleType = "岗位";
            }
            else if (roleType == "2")
            {
                roleType = "上级岗位";
            }
            else if (roleType == "3")
            {
                roleType = "个人";
            }
            return roleType;

        }
    }
    public string getcanAdd
    {
        get
        {
            if (canAdd == "true")
            {
                canAdd = "有";
            }
            else
            {
                canAdd = "无";
            }
            return canAdd;
        }
    }
    public string getcanView
    {
        get
        {
            if (canView == "true")
            {
                canView = "有";
            }
            else
            {
                canView = "无";
            }
            return canView;
        }
    }
    public string getcanModify
    {
        get
        {
            if (canModify == "true")
            {
                canModify = "有";
            }
            else
            {
                canModify = "无";
            }
            return canModify;
        }
    }
    public string getviewType
    {
        get
        {
            switch (viewType)
            {
                case "0":
                    viewType = "无";
                    break;
                case "1":
                    viewType = "个人";
                    break;
                case "2":
                    viewType = "本部门";
                    break;
                case "3":
                    viewType = "全公司";
                    break;
            }
            return viewType;
        }
    }
    public string getmodifyType
    {
        get
        {
            switch (modifyType)
            {
                case "0":
                    modifyType = "无";
                    break;
                case "1":
                    modifyType = "个人";
                    break;
                case "2":
                    modifyType = "本部门";
                    break;
                case "3":
                    modifyType = "全公司";
                    break;
            }
            return modifyType;
        }
    }

}
