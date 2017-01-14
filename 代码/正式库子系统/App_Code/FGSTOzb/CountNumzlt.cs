using System;
using System.Collections.Generic;
using System.Web;
using System.Collections ;
using FMOP.DB;
using System.Data;
/// <summary>
///CountNumzlt 的摘要说明
/// </summary>
public class CountNumzlt:CountNum
{
	public CountNumzlt()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public override Hashtable GetNum(string ssfgs)
    {
        double syNum = 0;
        string KHBH = DbHelperSQL.GetSingle("SELECT DYFGSBH FROM System_City_0 WHERE DYFGSMC='" + ssfgs.Trim() + "'").ToString();//分公司客户编号
        ssfgs = ssfgs.Trim();//分公司名称
        double gone = Convert.ToDouble(DbHelperSQL.GetSingle("SELECT SUM(XGSL) FROM FGS_DZGSZLWTCPCLJLB INNER JOIN FGS_DZGSZLWTCPCLJLB_XGXX ON parentNumber = Number WHERE FGSKHBH = '" + KHBH + "' AND XZFS='方式一' AND THFS <> '公司召回'"));//返回质量退还已退总数量
       // throw new NotImplementedException();
        Hashtable ht = new Hashtable();


        string StrGetsyed = " select THHCSED from  FGS_ZLTHQCEDB where  FGSMC='" + ssfgs.Trim() + "'";
        object obj = DbHelperSQL.GetSingle(StrGetsyed);
        if (obj != null)
        {
            syNum = (double)int.Parse(obj.ToString().Trim());
        }


        //分公司质量问题产品额度=上线时剩余分公司与服务商之间质量问题处理额度值＋上线时分公司ERP质量问题鼓库存数量＋上线后分公司富美硒鼓累计销货量的1.5%—上线后分公司与总部之间的质量问题处理的总数量（不包含公司召回的数量）。

        DataSet dsQC = DbHelperSQL.Query("SELECT TOP 1 ZLGCSED,ERPZLGKC FROM FGS_ZLTHQCEDB WHERE FGSMC='" + ssfgs + "' ORDER BY  QCSJ DESC,CREATETIME DESC");
        double initialSum = 0.00;
        double erpSum = 0.00;
        if (dsQC != null || dsQC.Tables[0].Rows.Count > 0)
        {
            initialSum = Convert.ToDouble(dsQC.Tables[0].Rows[0]["ZLGCSED"].ToString());
            erpSum = Convert.ToDouble(dsQC.Tables[0].Rows[0]["ERPZLGKC"].ToString());
        }
         //= Convert.ToDouble(DbHelperSQL.GetSingle("SELECT ZLGCSED FROM FGS_ZLTHQCEDB WHERE FGSMC='" + ssfgs + "' AND CN='" + DateTime.Now.Year.ToString() + "'"));//上线时剩余分公司与服务商之间质量问题处理额度值
        //double erpSum = Convert.ToDouble(DbHelperSQL.GetSingle("SELECT ERPZLGKC FROM FGS_ZLTHQCEDB WHERE FGSMC='" + ssfgs + "' AND CN='" + DateTime.Now.Year.ToString() + "'"));//上线时分公司ERP质量问题鼓库存数量

        double saled = 0;//累积销货总量

        //获取分公司部门编号
        string Strkhbh = "select ME001 from CMSME WHERE ME002 = '" + ssfgs.Trim() + "'";
        object objbh = GetErpData.GetSingle(Strkhbh, ssfgs);
        string FGSbh = "";
        if (objbh != null)
        {
            FGSbh = objbh.ToString().Trim();
        }

        //获取期初时间  DateTime.Now.ToString("yyyyMMddHHmmssfff")
        string StrGetDate = " select QCSJ from FGS_ZLTHQCEDB where FGSMC='" + ssfgs + "'";
        object objdate = DbHelperSQL.GetSingle(StrGetDate);
        string Strxhddatewhere = "";
        string Strsqdatewhere = "";
        if (obj != null)
        {
            Strxhddatewhere = " and a.TG003>='" + DateTime.Parse(objdate.ToString()).ToString("yyyyMMdd") + "'";
            Strsqdatewhere = " and CreateTime>='" + objdate.ToString() + "'";
        }
        else
        {
            Strxhddatewhere = " and 1!=1 ";
            Strsqdatewhere = " and 1!=1 ";
        }

        // 从 erp 获取 上线后分公司的总销售量
        //string StrgetErpNum = "select sum(isnull(TG033,0)) as 销货数量  from COPTG where TG001 = '2303' " + Strxhddatewhere + " and TG004='" + FGSbh.Trim() + "' group by TG004

        //正常销退单数据
        string StrgetErpNum = "select a.TG001,a.TG002,a.TG005,a.TG023 , ISNULL(b.TH008,0.00) AS '销货数量'  from COPTG as a left join COPTH as b on a.TG001=b.TH001 AND a.TG002=b.TH002 and a.TSFM_bsc=b.TSFM_bsc where   (a.TG001 = '2312' or a.TG001 = '2313' or a.TG001 = '2302'or a.TG001 = '2316' or a.TG001 = '2317' )  " + Strxhddatewhere + " AND a.TG023='Y'and a.TG005='" + FGSbh.Trim() + "' and b.TH008 !=0 ";
        DataSet dsxhdNew = GetErpData.GetDataSet(StrgetErpNum);
        // DataSet dsxhdNew = GetErpData.GetDataSet(StrSQlNew);
        if (dsxhdNew != null && dsxhdNew.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dsxhdNew.Tables[0].Rows)
            {
                saled += double.Parse(dr["销货数量"].ToString().Trim());
            }
        }



        
        double valToUse = (int)(saled * 0.015) + initialSum + erpSum - gone;//总销货的1.5%+期初+ERP质量退剩余-已处理 ,减掉已处理，这个是剩下的额度
        double valToUse_Sums = (int)(saled * 0.015) + initialSum + erpSum;//这是总额度
       // string reSum = "";//分公司质量问题产品额度
        //valToUse_Sums - gone = valToUse
        ht.Add("总额度", valToUse_Sums);
        ht.Add("可退数量", valToUse);
        ht.Add("已退总数量", gone);

        return ht;
    }


}