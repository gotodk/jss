using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

/// <summary>
///KHGX_selectEdit 用于客户档案搜索的管理
/// by galaxy
/// </summary>
public class KHGX_selectEdit
{
    private DataSet ds;


	public KHGX_selectEdit()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    #region Create Table

        public DataTable CreateTable(string tableName, string fieldList)
        {
            DataTable dt = new DataTable(tableName);
            DataColumn dc;
            string[] Fields = fieldList.Split(',');
            string[] FieldsParts;
            string Expression;
            foreach (string Field in Fields)
            {
                FieldsParts = Field.Trim().Split(" ".ToCharArray(), 3); // allow for spaces in the expression 
                // add fieldname and datatype 
                if (FieldsParts.Length == 2)
                {
                    dc = dt.Columns.Add(FieldsParts[0].Trim(), Type.GetType("System." + FieldsParts[1].Trim(), true, true));
                    dc.AllowDBNull = true;
                }
                else if (FieldsParts.Length == 3) // add fieldname, datatype, and expression 
                {
                    Expression = FieldsParts[2].Trim();
                    if (Expression.ToUpper() == "REQUIRED")
                    {
                        dc = dt.Columns.Add(FieldsParts[0].Trim(), Type.GetType("System." + FieldsParts[1].Trim(), true, true));
                        dc.AllowDBNull = false;
                    }
                    else
                    {
                        dc = dt.Columns.Add(FieldsParts[0].Trim(), Type.GetType("System." + FieldsParts[1].Trim(), true, true), Expression);
                    }
                }
                else
                {
                    return null;
                }
            }
            if (ds != null)
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }

        public DataTable CreateTable(string tableName, string fieldList, string keyFieldList)
        {
            DataTable dt = CreateTable(tableName, fieldList);
            string[] KeyFields = keyFieldList.Split(',');
            if (KeyFields.Length > 0)
            {
                DataColumn[] KeyFieldColumns = new DataColumn[KeyFields.Length];
                int i;
                for (i = 1; i == KeyFields.Length - 1; ++i)
                {
                    KeyFieldColumns[i] = dt.Columns[KeyFields[i].Trim()];
                }
                dt.PrimaryKey = KeyFieldColumns;
            }
            return dt;
        }

        #endregion
}
