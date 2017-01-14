using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// SQLBuilder 的摘要说明
/// </summary>
public abstract class SQLBuilder
{
    /// <summary>
    /// 向sql语句添加where条件
    /// </summary>
    /// <param name="Sql">sql语句</param>
    /// <param name="Where">查询条件</param>
    /// <returns>包含查询条件的SQL语句</returns>
    public static string AddWhereToSQL(string Sql, ArrayList Wheres)
    {
        //将sql语句转换为小写，以方便取值
        string lowSql = Sql.ToLower();
        //找到最后一个from的位置，以最后一个from的位置作为基准查询点
        int fromPositon = lowSql.LastIndexOf("from");
        //如果sql语句不包含from，则直接返回null
        if (fromPositon <= 0)
            return null;
        //获取插入点位置
        int insertPos = getInsertPosition(lowSql, fromPositon);
        //通过arraylist中的各个查询条件，组合为where子句
        string where;
        where = BuildWhere(Wheres, Sql.IndexOf("where",fromPositon)>0);
        return Sql.Insert(insertPos, where);

    }


    /// <summary>
    /// 向sql语句添加where条件
    /// </summary>
    /// <param name="Sql">sql语句</param>
    /// <param name="Where">查询条件，各条件之间以and分割</param>
    /// <returns>包含查询条件的SQL语句</returns>
    public static string AddWhereToSQL(string Sql, string Where)
    {
        //String[] wheres=Where.Split("and".ToCharArray());
        String[] wheres = Regex.Split(Where, "and", RegexOptions.IgnoreCase);
        ArrayList tmpArrayWheres=new ArrayList(wheres);
        ArrayList arrayWheres = new ArrayList();
        foreach (string where in tmpArrayWheres)
        {
            if (!String.IsNullOrEmpty(where.Trim()))
            arrayWheres.Add(where);
        }

        return AddWhereToSQL(Sql, arrayWheres);

    }

    /// <summary>
    /// 找出where语句应该插入的位置
    /// </summary>
    /// <param name="sql">小写的sql语句</param>
    /// <returns>where语句应插入的位置</returns>
    private static int getInsertPosition(string sql, int fromPostion)
    {
        //int result;
        //获取order关键字的位置
        int orderPostion = sql.IndexOf("order",fromPostion);
        //获取group关键字的位置
        int groupPostion = sql.IndexOf("group", fromPostion);
        //如果没有order和group 直接返回sql语句的最末端

        /* 之前的版本
        if (orderPostion <= 0 && groupPostion <= 0)
            return sql.Length;
        //如果没有出现group，则返回order的位置
        else if (groupPostion <= 0)
        {
            return orderPostion;
        }
        //如果都出现了，则返回较靠前的位置
        else
        {
            return groupPostion > orderPostion ? orderPostion : groupPostion;
        }
        */

        /* 最新版本修改 2008/05/28 */
        if (orderPostion <= 0 && groupPostion <= 0)
        {
            return sql.Length;
        }
        else if (groupPostion <= 0)
        {
            return orderPostion;
        }
        else if (orderPostion <= 0)
        {
            return groupPostion;
        }
        else
        {
            return groupPostion > orderPostion ? orderPostion : groupPostion;
        }
    }

    /// <summary>
    /// 通过一组where条件，获取where语句
    /// </summary>
    /// <param name="Wheres">where条件组</param>
    /// <param name="HasWhere">sql语句当中是否已经包含where</param>
    /// <returns></returns>
    private static string BuildWhere(ArrayList Wheres, bool HasWhere)
    {
        if (Wheres == null)
            return "";
        
        string where="";
		int i = 0;
		for (; i <= Wheres.Count - 1; i++)
		{
			if (Wheres[i] != null && Wheres[i].ToString() != string.Empty)
			{
				//如果sql语句中包含where子句，则查询条件直接以and开头
				if (HasWhere)
				{
					where += " and " + (string)Wheres[i];
				}
				//否则以where开头
				else
				{
					where += " where " + (string)Wheres[i];
				}
				i++;
				break;
			}
		}
        
        //遍历从第二个开始的条件，组成where子句
        for (; i <= Wheres.Count - 1; i++)
        {
			if (Wheres[i] != null && Wheres[i].ToString() != string.Empty)
			{
				where += " and " + (string)Wheres[i];
			}
        }

        return where+" ";

    }


	/// <summary>
	/// 添加where条件
	/// </summary>
	/// <param name="Where">查询条件</param>
	/// <returns>查询条件</returns>
	public static string AddToWhere(ArrayList Wheres)
	{
		if (Wheres==null||Wheres.Count <= 0)
		{
			return string.Empty;
		}
		else
		{
			return BuildWhere(Wheres, false);
		}

	}

}
