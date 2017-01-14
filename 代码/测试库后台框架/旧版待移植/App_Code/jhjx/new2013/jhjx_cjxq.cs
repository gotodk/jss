using FMOP.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// jhjx_cjxq 的摘要说明
/// </summary>
public class jhjx_cjxq
{
	public jhjx_cjxq()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public DataSet jhjx_cjxx(DataSet dsreturn, DataTable ht)
    {
        string Strsql = "select  Z_SPBH as '商品编号' ,Z_SPMC as '商品名称' ,Z_JJDW as '计价单位',Z_DBSJ as '定标时间',Z_ZBSL as '定标数量',Z_ZBJG as '定标价格',Z_ZBJE as '定标金额',Z_HTQX as '合同期限',(select  isnull(sum(isnull(T_THSL,0)),0) from   AAA_THDYFHDXXB where ZBDBXXBBH = AAA_ZBDBXXB.Number and F_DQZT <>'撤销') as '已发货数量', (select  isnull(sum(ISNULL(T_THSL,0)),0)  as aa  from   AAA_THDYFHDXXB where ZBDBXXBBH =AAA_ZBDBXXB.Number  and (F_DQZT <>'撤销' and F_DQZT <>'未生成发货单' and F_DQZT <>'已生成发货单')) as '已提货数量', case  isnull (Z_QPZT,'') when '' then '---' when '未开始清盘' then '---' else  ISNULL(CONVERT(varchar(100), Z_QPKSSJ, 20),'---') end as 清盘时间,case when  isnull (Z_QPZT,'') =  '' then '---' when isnull (Z_QPZT,'') =  '未开始清盘' then '未开始清盘' when CONVERT(varchar(100), Z_QPKSSJ, 20)=CONVERT(varchar(100), Z_QPJSSJ, 20) then '自动清盘'  else '人工清盘' end  as '清盘类型' from  AAA_ZBDBXXB where (Z_HTZT = '定标' or Z_HTZT = '定标合同终止' or Z_HTZT = '定标合同到期'  or Z_HTZT = '定标执行完成') and datediff(YEAR,Z_DBSJ,getdate())=0 order by Z_DBSJ desc";
       DataSet dsreturnnew = DbHelperSQL.Query(Strsql);
        return dsreturnnew;
    }
}