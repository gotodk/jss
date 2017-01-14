using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Data;
using FMOP.DB;
using System.Text;
/// <summary>
///Countfgskcth 的摘要说明
/// </summary>
public class Countfgskcth : Countkcnum
{
	public Countfgskcth()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 获取分公司库存信息
    /// </summary>
    /// <param name="ssbsc">所属分公司</param>
    /// <param name="PH">批号</param>
    /// <param name="isgetlist">是否返回进货单列表</param>
    /// <returns>返回HashTable,包含两个键值“库存数量”，“进货单列表”（可选）</returns>
    public override System.Collections.Hashtable GetNum(string ssbsc, string PH, bool isgetlist)
    {
        //throw new NotImplementedException();
        Hashtable ht = new Hashtable();
        double Strkc = 0.0; 
        //获取仓库，库位信息
        //获取仓库
        string Strckbh = "";
        string Strkwei = "";
        string Strck = "select MC001 as 仓库编码,MC002 as 仓库名称 ,NI002 as 库位编码,NI003 as 库位名称 from CMSMC as a  left join CMSNI as b on a.MC001=b.NI001 where a.MC002 LIKE '%硒鼓成品仓库%' AND b.NI003 NOT LIKE  '%不良品区%'";
        DataSet dsck = GetErpData.GetDataSet(Strck, ssbsc);
        if (dsck != null && dsck.Tables[0].Rows.Count > 0)
        {
            Strckbh = dsck.Tables[0].Rows[0]["仓库编码"].ToString().Trim();
            foreach (DataRow dr in dsck.Tables[0].Rows)
            {
                Strkwei += "'" + dr["库位编码"].ToString().Trim() + "',";
            }

            Strkwei = "(" + Strkwei.Remove(Strkwei.Length - 1) + ")";
        }
        else
        {
            ht.Add("库存数量", "仓库信息不存在，请在erp中查看是否有本分公司对应成品库信息");
            return ht;
        }

        //获取库存数量
        DataView dsjhdlit = null;
        string GetckNum = "select sum(isnull(ML005,0)) as 数量, ML001 as 品号, ML004 as 批号,ML002 as 仓库,ML003 as 库位  from INVML  where  ML001='" + PH + "' and ML005 >0  and ML002='" + Strckbh + "' and ML003 in" + Strkwei + " group by ML001,ML004,ML002,ML003   ";
        DataSet dsckkc = GetErpData.GetDataSet(GetckNum, ssbsc);
        if (dsckkc != null && dsckkc.Tables[0].Rows.Count > 0)
        {
            foreach(DataRow drkc in dsckkc.Tables[0].Rows)
            {
              Strkc += double.Parse(drkc["数量"].ToString().Trim());
            }

            if (isgetlist) //判断是否需要返回进货单列表
            {

                dsjhdlit = dsGetjhblist(dsckkc, ssbsc);
               
            }


        }

        //2012.1.8 wyh 添加无进货单功能================================================================================================
        if (dsjhdlit == null || dsjhdlit.Count <= 0)
        {
            //直接从仓库查找数据
            string StrSQLCKNum = " select  '' as 单据时间 , ' ' as 单号, ML002 as 仓库, '' as 序号,ML003 as 库位,ML001 as 品号,MB002 as 品名, ML004 as 批号,MB003 as 规格 ,'0.00' as 单价 ,ML005 as 数量 ,'0' as 已用数量,ML005 as 可用数量 from INVML left join INVMB on INVML.ML001=INVMB.MB001  where  ML001='" + PH + "' and ML005 >0  and ML002='" + Strckbh + "' and ML003 in" + Strkwei;
            DataSet dscangkumx = GetErpData.GetDataSet(StrSQLCKNum, ssbsc);
            string Strxglx = "";
            string Strxh = "";
            //  string strjg = "0.00";
            object obj = null;
            if (dscangkumx != null && dscangkumx.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drckmx in dscangkumx.Tables[0].Rows)
                {
                    if (drckmx["品号"].ToString().Trim().StartsWith("151"))
                    {
                        Strxglx = "绿装";
                    }
                    else if (drckmx["品号"].ToString().Trim().StartsWith("150"))
                    {
                        Strxglx = "蓝装";
                    }
                    else if (drckmx["品号"].ToString().Trim().StartsWith("153"))
                    {
                        Strxglx = "红装";
                    }

                    Strxh = drckmx["品名"].ToString().Trim().Split(' ')[1].Trim();

                    string StrGEtjg = " select FGSTHJ from FGS_CPXHJGB where CPLB='" + Strxglx + "' and CPXH='" + Strxh + "' ";

                    obj = DbHelperSQL.GetSingle(StrGEtjg);
                    if (obj != null && obj.ToString().Trim() != "")
                    {
                        drckmx["单价"] = double.Parse(obj.ToString().Trim()).ToString("0.00");

                    }


                }

                dsjhdlit = dscangkumx.Tables[0].DefaultView;

            }

        }

        else
        {
            double dbaa= 0.00 ;
            foreach(DataRowView drv in dsjhdlit )
            {
                dbaa += double.Parse(drv["可用数量"].ToString().Trim());
            }
            if (dbaa < Strkc)
            {
                //直接从仓库查找数据
                string StrSQLCKNum = " select  '' as 单据时间 , ' ' as 单号, ML002 as 仓库, '' as 序号,ML003 as 库位,ML001 as 品号,MB002 as 品名, ML004 as 批号,MB003 as 规格 ,'0.00' as 单价 ,ML005 as 数量 ,'0' as 已用数量,ML005 as 可用数量 from INVML left join INVMB on INVML.ML001=INVMB.MB001  where  ML001='" + PH + "' and ML005 >0  and ML002='" + Strckbh + "' and ML003 in" + Strkwei;
                DataSet dscangkumx = GetErpData.GetDataSet(StrSQLCKNum, ssbsc);
                string Strxglx = "";
                string Strxh = "";
                //  string strjg = "0.00";
                object obj = null;
                if (dscangkumx != null && dscangkumx.Tables[0].Rows.Count > 0)
                {
                    DataTable dtnew = dsjhdlit.ToTable();
                    DataRow dradd = null;
                    foreach (DataRow drckmx in dscangkumx.Tables[0].Rows)
                    {
                        
                        if (drckmx["品号"].ToString().Trim().StartsWith("151"))
                        {
                            Strxglx = "绿装";
                        }
                        else if (drckmx["品号"].ToString().Trim().StartsWith("150"))
                        {
                            Strxglx = "蓝装";
                        }
                        else if (drckmx["品号"].ToString().Trim().StartsWith("153"))
                        {
                            Strxglx = "红装";
                        }

                        Strxh = drckmx["品名"].ToString().Trim().Split(' ')[1].Trim();

                        string StrGEtjg = " select FGSTHJ from FGS_CPXHJGB where CPLB='" + Strxglx + "' and CPXH='" + Strxh + "' ";

                        obj = DbHelperSQL.GetSingle(StrGEtjg);
                        if (obj != null && obj.ToString().Trim() != "")
                        {
                            drckmx["单价"] = double.Parse(obj.ToString().Trim()).ToString("0.00");

                        }
                        // select b.TG003 as 单据时间 , TH001+'_'+TH002 as 单号,TH009 as 仓库, TH003 as 序号,TH072 as 库位,TH004 as 品号,TH005 as 品名, TH010 as 批号,TH006 as 规格 ,TH018 as 单价 ,TH015 as 数量 ,'0' as 已用数量,TH015 as 可用数量    
                        //' ' as 单号, ML002 as 仓库, '' as 序号,ML003 as 库位,ML001 as 品号,MB002 as 品名, ML004 as 批号,MB003 as 规格 ,'0.00' as 单价 ,ML005 as 数量 ,'0' as 已用数量,ML005 as 可用数量
                        dradd = dtnew.NewRow();
                        dradd["单据时间"] = drckmx["单据时间"].ToString();
                        dradd["单号"] = drckmx["单号"].ToString();
                        dradd["仓库"] = drckmx["仓库"].ToString();
                        dradd["序号"] = drckmx["序号"].ToString();
                        dradd["库位"] = drckmx["库位"].ToString();
                        dradd["品号"] = drckmx["品号"].ToString();
                        dradd["品名"] = drckmx["品名"].ToString();
                        dradd["批号"] = drckmx["批号"].ToString();
                        dradd["规格"] = drckmx["规格"].ToString();
                        dradd["单价"] = drckmx["单价"].ToString();
                        dradd["数量"] = drckmx["数量"].ToString();
                        dradd["已用数量"] = drckmx["已用数量"].ToString();
                        dradd["可用数量"] = drckmx["可用数量"].ToString();

                        dtnew.Rows.Add(dradd);

                    }

                    dsjhdlit = dtnew.DefaultView; //dscangkumx.Tables[0].DefaultView;

                }
               
            }
        }

        //======================================================================================================================


        ht.Add("库存数量", Strkc.ToString("0"));
        ht.Add("进货单列表", dsjhdlit);
        //生成库存对应进货单

      


        //ht.Add("库存数量", Strckbh);
        //ht.Add("进货单列表", Strkwei);



        return ht;
    }


    private DataView dsGetjhblist(DataSet ds, string ssfgs)
    {
        DataView dv = null;
        DataSet dsjhdlist = null;
        StringBuilder Strbwhere = new StringBuilder() ;
        StringBuilder Strchukuwhere = new StringBuilder();
        StringBuilder StrBthd = new StringBuilder();
        //StringBuilder Strsqdwhere = new StringBuilder();
      //  string Strwhere = "";
        for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        {
            if (i == ds.Tables[0].Rows.Count - 1)
            {
                Strbwhere.Append( " ( TH004='" + ds.Tables[0].Rows[i]["品号"].ToString().Trim() + "' and TH010='" + ds.Tables[0].Rows[i]["批号"].ToString().Trim() + "' and TH009='" + ds.Tables[0].Rows[i]["仓库"].ToString().Trim() + "' and TH072='" + ds.Tables[0].Rows[i]["库位"].ToString().Trim() + "' )");

                Strchukuwhere.Append(" ( LA001='" + ds.Tables[0].Rows[i]["品号"].ToString().Trim() + "' and LA016='" + ds.Tables[0].Rows[i]["批号"].ToString().Trim() + "' and LA009='" + ds.Tables[0].Rows[i]["仓库"].ToString().Trim() + "' and LA023='" + ds.Tables[0].Rows[i]["库位"].ToString().Trim() + "' )");

                StrBthd.Append(" ( TJ004='" + ds.Tables[0].Rows[i]["品号"].ToString().Trim() + "' and TJ012='" + ds.Tables[0].Rows[i]["批号"].ToString().Trim() + "' and TJ011='" + ds.Tables[0].Rows[i]["仓库"].ToString().Trim() + "' and TJ046='" + ds.Tables[0].Rows[i]["库位"].ToString().Trim() + "' )");
               // Strsqdwhere.Append(" ( PH='" + ds.Tables[0].Rows[i]["品号"].ToString().Trim() + "' and PiHao='" + ds.Tables[0].Rows[i]["批号"].ToString().Trim() + "' and CK='" + ds.Tables[0].Rows[i]["仓库"].ToString().Trim() + "' and KW='" + ds.Tables[0].Rows[i]["库位"].ToString().Trim() + "' )");


            }
            else
            {
                Strbwhere.Append(" ( TH004='" + ds.Tables[0].Rows[i]["品号"].ToString().Trim() + "' and TH010='" + ds.Tables[0].Rows[i]["批号"].ToString().Trim() + "' and TH009='" + ds.Tables[0].Rows[i]["仓库"].ToString().Trim() + "' and TH072='" + ds.Tables[0].Rows[i]["库位"].ToString().Trim() + "' ) or ");


                Strchukuwhere.Append(" ( LA001='" + ds.Tables[0].Rows[i]["品号"].ToString().Trim() + "' and LA016='" + ds.Tables[0].Rows[i]["批号"].ToString().Trim() + "' and LA009='" + ds.Tables[0].Rows[i]["仓库"].ToString().Trim() + "' and LA023='" + ds.Tables[0].Rows[i]["库位"].ToString().Trim() + "' ) or ");


                StrBthd.Append(" ( TJ004='" + ds.Tables[0].Rows[i]["品号"].ToString().Trim() + "' and TJ012='" + ds.Tables[0].Rows[i]["批号"].ToString().Trim() + "' and TJ011='" + ds.Tables[0].Rows[i]["仓库"].ToString().Trim() + "' and TJ046='" + ds.Tables[0].Rows[i]["库位"].ToString().Trim() + "' ) or ");
               // Strsqdwhere.Append(" ( PH='" + ds.Tables[0].Rows[i]["品号"].ToString().Trim() + "' and PiHao='" + ds.Tables[0].Rows[i]["批号"].ToString().Trim() + "' and CK='" + ds.Tables[0].Rows[i]["仓库"].ToString().Trim() + "' and KW='" + ds.Tables[0].Rows[i]["库位"].ToString().Trim() + "' ) or ");

            }
 
        }

        //获取与库存对应的所有进货单
        string StrGetDs = " select b.TG003 as 单据时间 , TH001+'_'+TH002 as 单号,TH009 as 仓库, TH003 as 序号,TH072 as 库位,TH004 as 品号,TH005 as 品名, TH010 as 批号,TH006 as 规格 ,TH018 as 单价 ,TH015 as 数量 ,'0' as 已用数量,TH015 as 可用数量 from PURTH as a left join PURTG as b on a.TH001=b.TG001 and a.TH002=b.TG002 where TH001= '341' and ( " + Strbwhere.ToString().Trim() + ") order by b.TG003  ";
        dsjhdlist = GetErpData.GetDataSet(StrGetDs,ssfgs);
        if (dsjhdlist != null && dsjhdlist.Tables[0].Rows.Count > 0)
        {


           // 获得出库信息
            string Strchuk = " select sum(isnull(TJ009,0)) as 数量  ,TJ004 as 品号,TJ012 as 批号,TJ011 as 仓库,TJ046 as 库位,(TJ013+'_'+TJ014) as 单号  from PURTJ WHERE TJ001 like '35%' and TJ011 like 'fgs%' and TJ046 not like '%不良品区%' and  (" + StrBthd.ToString() + ")  group by TJ004,TJ011,TJ012,TJ046,(TJ013+'_'+TJ014) ";
                // "  select a.仓库,a.库位,a.批号,a.品号,SUM(a.数量) as 数量 from ( select LA001 as 品号 ,LA016 as 批号,LA009 as 仓库,LA023 as 库位,isnull(LA011,0) as 数量 from INVLA where LA005='-1' and (" + Strchukuwhere.ToString() + ")) as a group by a.品号,a.批号,a.仓库,a.库位 ";
            DataSet dschuku = GetErpData.GetDataSet(Strchuk,ssfgs);
            if (dschuku != null && dschuku.Tables[0].Rows.Count > 0)
            {
               //遍历删除已经退货的数据
                foreach (DataRow drtc in dschuku.Tables[0].Rows)
                {
                    double Num = double.Parse(drtc["数量"].ToString().Trim());
                    for (int i=0; i < dsjhdlist.Tables[0].Rows.Count; i++)
                    {
                        if (drtc["库位"].ToString().Trim().Equals(dsjhdlist.Tables[0].Rows[i]["库位"].ToString().Trim()) && drtc["仓库"].ToString().Trim().Equals(dsjhdlist.Tables[0].Rows[i]["仓库"].ToString().Trim()) && drtc["批号"].ToString().Trim().Equals(dsjhdlist.Tables[0].Rows[i]["批号"].ToString().Trim()) && drtc["品号"].ToString().Trim().Equals(dsjhdlist.Tables[0].Rows[i]["品号"].ToString().Trim()) && drtc["单号"].ToString().Trim().Equals(dsjhdlist.Tables[0].Rows[i]["单号"].ToString().Trim()))
                        {
                            if (Num > double.Parse(dsjhdlist.Tables[0].Rows[i]["数量"].ToString().Trim()))
                            {
                                Num = Num - double.Parse(dsjhdlist.Tables[0].Rows[i]["数量"].ToString().Trim());
                                dsjhdlist.Tables[0].Rows[i]["可用数量"] = "0";
                                dsjhdlist.Tables[0].Rows[i]["已用数量"] = double.Parse(dsjhdlist.Tables[0].Rows[i]["数量"].ToString().Trim());
                            }
                            else
                            {

                                dsjhdlist.Tables[0].Rows[i]["可用数量"] = double.Parse(dsjhdlist.Tables[0].Rows[i]["数量"].ToString().Trim()) - Num;
                                dsjhdlist.Tables[0].Rows[i]["已用数量"] = Num;
                                Num = 0;
                            }

                        }
                    }
                }//遍历删除已经退货的数据结束



            }

            dv = dsjhdlist.Tables[0].DefaultView;
            dv.RowFilter = " 可用数量 > 0 ";







        }



        return dv;
 
    }






}