using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using FMOP.DB;
using System.Data;
/// <summary>
///CountNumthh 的摘要说明
/// </summary>
public class CountNumthh:CountNum
{
	public CountNumthh()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public override Hashtable GetNum(string ssfgs)
    {
        //throw new NotImplementedException();
        Hashtable ht = new Hashtable();

        string StrSum = "0";// "调换货总数量" + ssfgs;
        string Strhave = "0";// "调换货已退总数量";
        double syNum = 0; //功能上线时分公司剩余的与服务商条换货的额度 
        double erpxhe = 0;  //上线后分公司的总销售量
        double Ytiaohun = 0;//已经调换数量
        //获取 功能上线时分公司剩余的与服务商条换货的额度 
        string StrGetsyed = " select top 1 THHCSED from  FGS_ZLTHQCEDB where  FGSMC='" + ssfgs.Trim() + "'   order by  QCSJ, CreateTime desc  ";
        object obj = DbHelperSQL.GetSingle(StrGetsyed);
        if (obj != null)
        {
            syNum = (double) int.Parse(obj.ToString().Trim());
        }
        //获取分公司部门编号
        string Strkhbh = "select ME001 from CMSME WHERE ME002 = '" + ssfgs.Trim() + "'";
        object objbh = GetErpData.GetSingle(Strkhbh,ssfgs);
        string FGSbh = "";
        if (objbh != null)
        {
            FGSbh = objbh.ToString().Trim();
        }


        //获取期初时间  DateTime.Now.ToString("yyyyMMddHHmmssfff")
        string StrGetDate = " select QCSJ from FGS_ZLTHQCEDB where FGSMC='" + ssfgs+"'";
        object  objdate = DbHelperSQL.GetSingle(StrGetDate);
        string Strxhddatewhere = "";
        string Strsqdatewhere = "";
        if (obj != null)
        {
            Strxhddatewhere = " and a.TG003>='" + DateTime.Parse(objdate.ToString()).ToString("yyyyMMdd") + "'";
            Strsqdatewhere = " and CreateTime>='" + objdate.ToString() + "'";
        }
        else
        {
            Strxhddatewhere =  "and  1!=1";
            Strsqdatewhere = "and  1!=1";
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
                 erpxhe += double.Parse(dr["销货数量"].ToString().Trim());
             }
         }

           //积分兑换销货单金额 但别 2314.2315 

         string StrSQljifen = " select a.TG001,a.TG002,a.TG005,a.TG023 , b.TH012,b.TH024 , isnull(b.TH024,0) as '金额'   from COPTG as a left join COPTH as b on a.TG001=b.TH001 AND a.TG002=b.TH002 and a.TSFM_bsc=b.TSFM_bsc where   1=1   and (a.TG001 = '2314' or a.TG001 = '2315') " + Strxhddatewhere + " AND a.TG023='Y'and TG005='" + FGSbh.Trim() + "' ";//ISNUMERIC(a.UDF01)

         DataSet dsjifen = GetErpData.GetDataSet(StrSQljifen);

         if (dsjifen != null && dsjifen.Tables[0].Rows.Count > 0)
         {
             foreach (DataRow dr in dsjifen.Tables[0].Rows)
             {
                 erpxhe += double.Parse(dr["金额"].ToString().Trim());
             }
         }


        // 获取已调换数量 

         string StrGetythsl = "select SUM( isnull(TRSL,0)) from  FGS_DZGSTHHCLJLB where FGSMC = '" + ssfgs.Trim() + "'" + Strsqdatewhere + " group by FGSMC";
        object objythNum = DbHelperSQL.GetSingle(StrGetythsl);
        if (objythNum != null)
        {
            Ytiaohun = (double)int.Parse(objythNum.ToString().Trim());
        }

        StrSum = (syNum + erpxhe * 0.05).ToString("0");
        Strhave = Ytiaohun.ToString("0");

        ht.Add("可退数量", StrSum);
        ht.Add("已退总数量", Strhave);
        return ht;
    }
}