using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

/// <summary>
/// demoR 的摘要说明
/// </summary>
public class demoR
{
	public demoR()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 再处理的例子
    /// </summary>
    /// <param name="dsold">首次分页得到的关键数据</param>
    /// <returns>处理后的数据</returns>
    public DataSet chuli_demo(DataSet dsold)
    {
        DataSet dsreturn = null;

        try
        {
            //进行在处理,daold的值，正常情况，不可能为null,所以不需要进行判断
            //通常这里不建议用in进行再处理，需要循环按唯一键进行在处理。
            //复杂的情况需要进行分析和取舍，可以选择分页首次处理的大部分字段留空，这里循环直接填空。 
            //也可以在这里重新生成新的数据集。


             
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }

        //二次处理后若处理结果是空值，返回原来的数据
        if (dsreturn == null)
        {
            dsreturn = dsold;
        }
        return dsreturn; 
    }
 
}