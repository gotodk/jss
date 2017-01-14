using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
///DeSqlzhuru 的摘要说明
/// </summary>
public class DeSqlzhuru
{
	public DeSqlzhuru()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public  bool ValidateQuery(Hashtable queryConditions)
    {
    //构造SQL的注入关键字符
    #region 字符
    string[] strBadChar = {"and ","and%20"
    ,"exec "
    ,"insert "
    ,"select "
    ,"delete "
    ,"update "
    ,"count "
    ,"or "
    ,"%20"
    //,"*"
    ,"%"
    ," :"
    ,"\'"
    ,"\""
    ,"chr "
    ,"mid "
    ,"master "
    ,"truncate "
    ,"char "
    ,"declare "
    ,"SiteName "
    ,"net user "
    ,"xp_cmdshell "
    ,"/add"
    ,"exec master.dbo.xp_cmdshell "
    ,"net localgroup administrators "};
    #endregion

    //构造正则表达式
    string str_Regex = ".*(";
    for (int i = 0; i < strBadChar.Length - 1; i++)
    {
        str_Regex += strBadChar[i] + "|";
    }
    str_Regex += strBadChar[strBadChar.Length - 1] + ").*";
    //避免查询条件中_list情况
    foreach (string str in queryConditions.Keys)
    {
    if(str.Substring(str.Length - 5)=="_list")
    {
    //去掉单引号检验
    str_Regex = str_Regex.Replace("|'|", "|");
    }
    string tempStr = queryConditions[str].ToString();
    if (Regex.Matches(tempStr.ToString(), str_Regex).Count > 0)
    {
    //有SQL注入字符
    return true;
    }
    }
    return false;
    }




}