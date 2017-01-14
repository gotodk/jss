using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using ShareLiu;

/// <summary>
///paiming 的摘要说明
/// </summary>
public class paiming
{
	public paiming()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public DataSet JiFenPaimiang( string StrID,string SSBSC )
    {
        DataSet ds = null;
        DateTime dt = DateTime.Now;
        string SMonNowS = dt.Year.ToString() + "/" + dt.Month.ToString() + "/01 00:00:00.000";
        string SmonNowO = dt.AddMonths(1).Year.ToString() + "/" + dt.AddMonths(1).Month.ToString() + "/01";
        string SMonLast = dt.AddMonths(-1).Year.ToString() + "/" + dt.AddMonths(-1).Month.ToString().Trim() + "/01 00:00:00.000";
      
        

        /*本月日常积分  ,'' as '当月关键绩效考核得分排名'*/
        string StrAddjifennow = "select RANK() over(order by Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) desc)  as 'E1排名序号（按数值从大到小）', SSBSC as '所属办事处',0.00 as 'E1考核得分（权重20分）', isnull(Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ),0.00) as 'E1本月新增积分',0.00 as 'E2（当月日常积分/市场督导经理数量）','' as '市场督导经理数量','' as '市场督导经理人数' ,0 as 'E2排名序号（按数值从大到小)','' as 'E2考核得分（权重10分)',0.00 as 'E3（当月日常积分/上月新增日常积分）','' as '上月新增日常积分',0.00 as 'E3排名序号（按数值从大到小）',0.00 as 'E3考核得分（权重20分）',Sum(case BigClass  when 'A' then isnull(ChangeNum,0.00) else 0 end ) as '当月实际完成业绩积分（A）' ,0.00 as '当月业绩积分任务' ,'0.00%' as '当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）',0.00 as '当月业绩积分完成率考核得分（权重20分，不设上限）',Sum(case BigClass  when 'B' then isnull(ChangeNum,0.00) else 0 end ) as '当月新增支持积分（B）','' as '服务商支持情况考核得分（加分项，上限20分）',0.00 as '当月关键绩效考核得分'   from dbo.FWPT_FWSJFMXB where  FWSBH NOT LIKE '8%' and ChangeTime >='" + SMonNowS + "' and ChangeTime<'" + SmonNowO + "'  group by  SSBSC ";

        DataSet dsruchang = DbHelperSQL.Query(StrAddjifennow);

        if (dsruchang != null && dsruchang.Tables[0].Rows.Count > 0)
        {

            /*督导经理人数*/
          //string StrScddjlNum = "select isnull(DDJLRS,1) as '督导经理人数' ,SSBSC as '所属办事处',0.00 as 'E2（当月日常积分/市场督导经理数量）',0 as 'E2排名' from BSCDDJLNum "; //select SSBSC,DDJLRS from BSCDDJLNum
            string StrScddjlNum = "Select   SSBSC  as '所属办事处' ,  isnull(DDJLRS,1) as '督导经理人数' ,0.00 as 'E2（当月日常积分/市场督导经理数量）',0 as 'E2排名' from BSCDDJLNum  as a  where a.Number in(select top 1  Number  from BSCDDJLNum where SSBSC = a.SSBSC  order by  (NF+YF) desc  )";

            DataSet dsdudjjNum = DbHelperSQL.Query(StrScddjlNum);
            if (dsdudjjNum != null && dsdudjjNum.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsruchang.Tables[0].Rows)    
                {
                    /*写入E1考核得分*/
                    dr["E1考核得分（权重20分）"] = 20.00-(double.Parse(dr["E1排名序号（按数值从大到小）"].ToString().Trim())-1)*0.8;
                    double dbzhikeh = (double.Parse(dr["当月新增支持积分（B）"].ToString().Trim())/400.00) ;
                    if(dbzhikeh >20)
                    {
                        dbzhikeh = 20.00 ;
                    }
                    dr["服务商支持情况考核得分（加分项，上限20分）"] = dbzhikeh;
                      
                    foreach (DataRow drdudd in dsdudjjNum.Tables[0].Rows)
                    {
                        if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                        {

                            dr["市场督导经理人数"] = drdudd["督导经理人数"].ToString();

                            string StrsteE1 = dr["E1本月新增积分"].ToString().Trim();
                            string StrRS = drdudd["督导经理人数"].ToString();
                            
                            dr["E2（当月日常积分/市场督导经理数量）"] = (float.Parse(dr["E1本月新增积分"].ToString().Trim()) / float.Parse(drdudd["督导经理人数"].ToString())).ToString("#0.00");
                            drdudd["E2（当月日常积分/市场督导经理数量）"] = (float.Parse(dr["E1本月新增积分"].ToString().Trim()) / float.Parse(drdudd["督导经理人数"].ToString())).ToString("#0.00");
                                
                            
                        }
                    }
                }


                dsruchang.AcceptChanges();
                dsdudjjNum.AcceptChanges();



                /*排序*/

                 

                 DataView dv = dsdudjjNum.Tables[0].DefaultView;
                 dv.Sort = "E2（当月日常积分/市场督导经理数量)  desc";

                 DataTable dkt = dv.ToTable();
                 DataSet dsky = new DataSet();
                 dsky.Tables.Add(dkt);
                 DataSet dspaiming = CheckALL.DSAddID(dsky, "E2排名2");
                  if (dspaiming != null && dspaiming.Tables[0].Rows.Count > 0)
                  {
                      foreach (DataRow dr in dsruchang.Tables[0].Rows)
                      {
                          foreach (DataRow drdudd in dspaiming.Tables[0].Rows)
                          {
                              if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                              {

                                  dr["E2排名序号（按数值从大到小)"] = drdudd["E2排名2"].ToString();
                                  dr["E2考核得分（权重10分)"] = (10.00 - (double.Parse(drdudd["E2排名2"].ToString()) - 1) * 0.4).ToString("#0.00");
     
                              }
                          }
                      }
                  }




                 
                 
                

            }




            dsruchang.AcceptChanges();
            

            /*上月日常积分*/
            string StrAddjifenlast = "select SSBSC as '所属办事处',  isnull(Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ),0.00) as '新增积分', 0.00 as 'E3（当月日常积分/上月新增日常积分）' from dbo.FWPT_FWSJFMXB  where  FWSBH NOT LIKE '8%'  AND   ChangeTime >='" + SMonLast + "' and ChangeTime<'" + SMonNowS + "'  group by  SSBSC";
            DataSet dsADD = DbHelperSQL.Query(StrAddjifenlast);
            if (dsADD != null && dsADD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drjifen in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drlast in dsADD.Tables[0].Rows)
                    {
                        if (drjifen["所属办事处"].ToString().Trim() == drlast["所属办事处"].ToString().Trim())
                        {
                            drlast["E3（当月日常积分/上月新增日常积分）"] = (float.Parse(drjifen["E1本月新增积分"].ToString().Trim()) / float.Parse(drlast["新增积分"].ToString().Trim())).ToString("#0.0000");
                            drjifen["上月新增日常积分"] = drlast["新增积分"].ToString().Trim();

                        }
                    }
                }
            }
            dsADD.AcceptChanges();


            /*排序*/
            DataView dv2 = dsADD.Tables[0].DefaultView;
            dv2.Sort = "E3（当月日常积分/上月新增日常积分）  desc";

            DataTable dkt2 = dv2.ToTable();
            DataSet dsky2 = new DataSet();
            dsky2.Tables.Add(dkt2);
            DataSet dspaiming2 = CheckALL.DSAddID(dsky2, "E3排名");

            if (dspaiming2 != null && dspaiming2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drdudd in dspaiming2.Tables[0].Rows)
                    {
                        if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                        {

                            dr["E3（当月日常积分/上月新增日常积分）"] = drdudd["E3（当月日常积分/上月新增日常积分）"].ToString();
                            dr["E3排名序号（按数值从大到小）"] = drdudd["E3排名"].ToString();
                            dr["E3考核得分（权重20分）"] = (20.00 - (double.Parse(drdudd["E3排名"].ToString().Trim()) - 1) * 0.8).ToString("#0.00");
                        }
                    }
                }
            }

            dsruchang.AcceptChanges();


           /**********************************************************************************************************************************************/
               //添加办事处每月任务积分
            
            string StrGetMasTjifen = " select *,0.00 as 当月实际完成业绩积分,0.00 as '当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）' from BSCJIfenSet where (NF+YF)='" +dt.Year.ToString().Trim()+dt.Month.ToString().Trim() + "'";
            DataSet DsMastjifen = DbHelperSQL.Query(StrGetMasTjifen);

            if (DsMastjifen != null && DsMastjifen.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drrichang in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drRenwu in DsMastjifen.Tables[0].Rows)
                    {
                        if (drrichang["所属办事处"].ToString().Trim() == drRenwu["SSBSC"].ToString().Trim())
                        {
                            drrichang["当月业绩积分任务"] = drRenwu["YJFRW"].ToString().Trim();
                            drRenwu["当月实际完成业绩积分"] = drrichang["当月实际完成业绩积分（A）"].ToString().Trim();
                            drrichang["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"] = ((double.Parse(drrichang["当月实际完成业绩积分（A）"].ToString().Trim()) / double.Parse(drRenwu["YJFRW"].ToString().Trim()))*100).ToString("#0.00")+"%";
                            drRenwu["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"]=((double.Parse(drrichang["当月实际完成业绩积分（A）"].ToString().Trim()) / double.Parse(drRenwu["YJFRW"].ToString().Trim())));
                        }
                    }
                }
            }

            DsMastjifen.AcceptChanges() ;




            DataView dvdjNum = DsMastjifen.Tables[0].DefaultView;
            dvdjNum.Sort = " 当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）  desc";

            DataTable dktJNum = dvdjNum.ToTable();
            DataSet dskyJNum = new DataSet();
            dskyJNum.Tables.Add(dktJNum);
            DataSet dspaimingJnum = CheckALL.DSAddID(dskyJNum, " 任务积分完成率排名 ");

            //插入积分完成率排名

            foreach (DataRow drrichangpai in dsruchang.Tables[0].Rows)
            {
                foreach (DataRow drmastpaiming in dspaimingJnum.Tables[0].Rows)
                {
                    if (drrichangpai["所属办事处"].ToString().Trim() == drmastpaiming["SSBSC"].ToString().Trim())
                    {
                        drrichangpai["当月业绩积分完成率考核得分（权重20分，不设上限）"] = (double.Parse(drmastpaiming["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"].ToString().Trim()) * 20).ToString("#0.00");
                    }
                }
            }




            /**********************************************************************************************************************************************/

            dsruchang.AcceptChanges();


            /*************************************************************************************当月关键绩效考核得分**********************************************/
       
            foreach (DataRow drgjjxkhdf in dsruchang.Tables[0].Rows)
            {

                drgjjxkhdf["当月关键绩效考核得分"] = (double.Parse(drgjjxkhdf["E1排名序号（按数值从大到小）"].ToString().Trim()) * 0.5 + double.Parse(drgjjxkhdf["E3排名序号（按数值从大到小）"].ToString().Trim()) * 0.5).ToString("#0.00");
            }
            dsruchang.AcceptChanges();

            /********************************************************************************************************************************************************/

            DataView dv3 = dsruchang.Tables[0].DefaultView;
            dv3.Sort = "当月关键绩效考核得分  ";

            DataSet dshj = new DataSet();
            dshj.Tables.Add(dv3.ToTable());

             

         //   ds = dsruchang;

            DataSet dspaiming3 = CheckALL.DSAddID(dshj, "当月关键绩效考核得分排名");

            string Strbsc = ISbsc.isbsc(StrID);
            string StrGw = Hesion.Brick.Core.Users.GetUserByNumber(StrID).JobName.Trim();
            string StrBSc = "";

            if (Strbsc.IndexOf("办事处") > 0)
            {
                if (StrGw.IndexOf("总经理") <= 0)
                {
                    StrBSc = "  所属办事处='" + Strbsc.Trim() + "'";
                }
            }

            DataView dv4 = dspaiming3.Tables[0].DefaultView;
            dv4.RowFilter = StrBSc;
            DataSet ds4 = new DataSet();

            ds4.Tables.Add(dv4.ToTable());

            string strwhere = "";
            if (SSBSC.Trim() != "0" && SSBSC != "")
            {
                strwhere = "  所属办事处='" + SSBSC.Trim() + "'";
            }

            DataView dv5 = ds4.Tables[0].DefaultView;
            dv5.RowFilter = strwhere;
            DataSet ds5 = new DataSet();
            ds5.Tables.Add(dv5.ToTable());

            ds = ds5;


        }


        //string Strbsc = ISbsc.isbsc(StrID);
        //string StrBSc = "";

        //if (Strbsc.IndexOf("办事处") > 0)
        //{
        //    StrBSc = " and SSBSC='" + Strbsc.Trim() + "'";

        //}

        //DataView dv4 = ds.Tables[0].DefaultView;
        //dv4.RowFilter = "  SSBSC='" + Strbsc.Trim() + "'";
        //DataSet ds4 = new DataSet();

        //ds4.Tables.Add(dv4.ToTable());


        return ds;

    }
    public DataSet JiFenPaimiangrc(string StrID, string SSBSC)
    {
        DataSet ds = null;
        DateTime dt = DateTime.Now;
        string SMonNowS = dt.Year.ToString() + "/" + dt.Month.ToString() + "/01 00:00:00.000";
        string SmonNowO = dt.AddMonths(1).Year.ToString() + "/" + dt.AddMonths(1).Month.ToString() + "/01";
        string SMonLast = dt.AddMonths(-1).Year.ToString() + "/" + dt.AddMonths(-1).Month.ToString().Trim() + "/01 00:00:00.000";



        /*本月日常积分  ,'' as '当月关键绩效考核得分排名'*/
        string StrAddjifennow = "select RANK() over(order by Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) desc)  as 'E1排名序号（按数值从大到小）', SSBSC as '所属办事处',0.00 as 'E1考核得分（权重20分）', isnull(Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ),0.00) as 'E1本月新增积分',0.00 as 'E2（当月日常积分/市场督导经理数量）','' as '市场督导经理数量','' as '市场督导经理人数' ,0 as 'E2排名序号（按数值从大到小)','' as 'E2考核得分（权重10分)',0.00 as 'E3（当月日常积分/上月新增日常积分）','' as '上月新增日常积分',0.00 as 'E3排名序号（按数值从大到小）',0.00 as 'E3考核得分（权重20分）',Sum(case BigClass  when 'A' then isnull(ChangeNum,0.00) else 0 end ) as '当月实际完成业绩积分（A）' ,0.00 as '当月业绩积分任务' ,'0.00%' as '当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）',0.00 as '当月业绩积分完成率考核得分（权重20分，不设上限）',Sum(case BigClass  when 'B' then isnull(ChangeNum,0.00) else 0 end ) as '当月新增支持积分（B）','' as '服务商支持情况考核得分（加分项，上限20分）',0.00 as '当月关键绩效考核得分'   from dbo.FWPT_FWSJFMXB where  FWSBH NOT LIKE '8%' and  ChangeTime >='" + SMonNowS + "'  and BigClass='A' and ChangeTime<'" + SmonNowO + "'  group by  SSBSC ";

        DataSet dsruchang = DbHelperSQL.Query(StrAddjifennow);

        if (dsruchang != null && dsruchang.Tables[0].Rows.Count > 0)
        {

            /*督导经理人数*/
            //string StrScddjlNum = "select isnull(DDJLRS,1) as '督导经理人数' ,SSBSC as '所属办事处',0.00 as 'E2（当月日常积分/市场督导经理数量）',0 as 'E2排名' from BSCDDJLNum "; //select SSBSC,DDJLRS from BSCDDJLNum
            string StrScddjlNum = "Select   SSBSC  as '所属办事处' ,  isnull(DDJLRS,1) as '督导经理人数' ,0.00 as 'E2（当月日常积分/市场督导经理数量）',0 as 'E2排名' from BSCDDJLNum  as a  where a.Number in(select top 1  Number  from BSCDDJLNum where SSBSC = a.SSBSC  order by  (NF+YF) desc  )";

            DataSet dsdudjjNum = DbHelperSQL.Query(StrScddjlNum);
            if (dsdudjjNum != null && dsdudjjNum.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsruchang.Tables[0].Rows)
                {
                    /*写入E1考核得分*/
                    dr["E1考核得分（权重20分）"] = 20.00 - (double.Parse(dr["E1排名序号（按数值从大到小）"].ToString().Trim()) - 1) * 0.8;
                    double dbzhikeh = (double.Parse(dr["当月新增支持积分（B）"].ToString().Trim()) / 400.00);
                    if (dbzhikeh > 20)
                    {
                        dbzhikeh = 20.00;
                    }
                    dr["服务商支持情况考核得分（加分项，上限20分）"] = dbzhikeh;

                    foreach (DataRow drdudd in dsdudjjNum.Tables[0].Rows)
                    {
                        if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                        {

                            dr["市场督导经理人数"] = drdudd["督导经理人数"].ToString();

                            string StrsteE1 = dr["E1本月新增积分"].ToString().Trim();
                            string StrRS = drdudd["督导经理人数"].ToString();

                            dr["E2（当月日常积分/市场督导经理数量）"] = (float.Parse(dr["E1本月新增积分"].ToString().Trim()) / float.Parse(drdudd["督导经理人数"].ToString())).ToString("#0.00");
                            drdudd["E2（当月日常积分/市场督导经理数量）"] = (float.Parse(dr["E1本月新增积分"].ToString().Trim()) / float.Parse(drdudd["督导经理人数"].ToString())).ToString("#0.00");


                        }
                    }
                }


                dsruchang.AcceptChanges();
                dsdudjjNum.AcceptChanges();



                /*排序*/



                DataView dv = dsdudjjNum.Tables[0].DefaultView;
                dv.Sort = "E2（当月日常积分/市场督导经理数量)  desc";

                DataTable dkt = dv.ToTable();
                DataSet dsky = new DataSet();
                dsky.Tables.Add(dkt);
                DataSet dspaiming = CheckALL.DSAddID(dsky, "E2排名2");
                if (dspaiming != null && dspaiming.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsruchang.Tables[0].Rows)
                    {
                        foreach (DataRow drdudd in dspaiming.Tables[0].Rows)
                        {
                            if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                            {

                                dr["E2排名序号（按数值从大到小)"] = drdudd["E2排名2"].ToString();
                                dr["E2考核得分（权重10分)"] = (10.00 - (double.Parse(drdudd["E2排名2"].ToString()) - 1) * 0.4).ToString("#0.00");

                            }
                        }
                    }
                }








            }




            dsruchang.AcceptChanges();


            /*上月日常积分*/
            string StrAddjifenlast = "select SSBSC as '所属办事处',  isnull(Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ),0.00) as '新增积分', 0.00 as 'E3（当月日常积分/上月新增日常积分）' from dbo.FWPT_FWSJFMXB  where  FWSBH NOT LIKE '8%'  and BigClass='A'  AND   ChangeTime >='" + SMonLast + "' and ChangeTime<'" + SMonNowS + "'  group by  SSBSC";
            DataSet dsADD = DbHelperSQL.Query(StrAddjifenlast);
            if (dsADD != null && dsADD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drjifen in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drlast in dsADD.Tables[0].Rows)
                    {
                        if (drjifen["所属办事处"].ToString().Trim() == drlast["所属办事处"].ToString().Trim())
                        {
                            drlast["E3（当月日常积分/上月新增日常积分）"] = (float.Parse(drjifen["E1本月新增积分"].ToString().Trim()) / float.Parse(drlast["新增积分"].ToString().Trim())).ToString("#0.0000");
                            drjifen["上月新增日常积分"] = drlast["新增积分"].ToString().Trim();

                        }
                    }
                }
            }
            dsADD.AcceptChanges();


            /*排序*/
            DataView dv2 = dsADD.Tables[0].DefaultView;
            dv2.Sort = "E3（当月日常积分/上月新增日常积分）  desc";

            DataTable dkt2 = dv2.ToTable();
            DataSet dsky2 = new DataSet();
            dsky2.Tables.Add(dkt2);
            DataSet dspaiming2 = CheckALL.DSAddID(dsky2, "E3排名");

            if (dspaiming2 != null && dspaiming2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drdudd in dspaiming2.Tables[0].Rows)
                    {
                        if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                        {

                            dr["E3（当月日常积分/上月新增日常积分）"] = drdudd["E3（当月日常积分/上月新增日常积分）"].ToString();
                            dr["E3排名序号（按数值从大到小）"] = drdudd["E3排名"].ToString();
                            dr["E3考核得分（权重20分）"] = (20.00 - (double.Parse(drdudd["E3排名"].ToString().Trim()) - 1) * 0.8).ToString("#0.00");
                        }
                    }
                }
            }

            dsruchang.AcceptChanges();


            /**********************************************************************************************************************************************/
            //添加办事处每月任务积分

            string StrGetMasTjifen = " select *,0.00 as 当月实际完成业绩积分,0.00 as '当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）' from BSCJIfenSet where (NF+YF)='" + dt.Year.ToString().Trim() + dt.Month.ToString().Trim() + "'";
            DataSet DsMastjifen = DbHelperSQL.Query(StrGetMasTjifen);

            if (DsMastjifen != null && DsMastjifen.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drrichang in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drRenwu in DsMastjifen.Tables[0].Rows)
                    {
                        if (drrichang["所属办事处"].ToString().Trim() == drRenwu["SSBSC"].ToString().Trim())
                        {
                            drrichang["当月业绩积分任务"] = drRenwu["YJFRW"].ToString().Trim();
                            drRenwu["当月实际完成业绩积分"] = drrichang["当月实际完成业绩积分（A）"].ToString().Trim();
                            drrichang["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"] = ((double.Parse(drrichang["当月实际完成业绩积分（A）"].ToString().Trim()) / double.Parse(drRenwu["YJFRW"].ToString().Trim())) * 100).ToString("#0.00") + "%";
                            drRenwu["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"] = ((double.Parse(drrichang["当月实际完成业绩积分（A）"].ToString().Trim()) / double.Parse(drRenwu["YJFRW"].ToString().Trim())));
                        }
                    }
                }
            }

            DsMastjifen.AcceptChanges();




            DataView dvdjNum = DsMastjifen.Tables[0].DefaultView;
            dvdjNum.Sort = " 当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）  desc";

            DataTable dktJNum = dvdjNum.ToTable();
            DataSet dskyJNum = new DataSet();
            dskyJNum.Tables.Add(dktJNum);
            DataSet dspaimingJnum = CheckALL.DSAddID(dskyJNum, " 任务积分完成率排名 ");

            //插入积分完成率排名

            foreach (DataRow drrichangpai in dsruchang.Tables[0].Rows)
            {
                foreach (DataRow drmastpaiming in dspaimingJnum.Tables[0].Rows)
                {
                    if (drrichangpai["所属办事处"].ToString().Trim() == drmastpaiming["SSBSC"].ToString().Trim())
                    {
                        drrichangpai["当月业绩积分完成率考核得分（权重20分，不设上限）"] = (double.Parse(drmastpaiming["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"].ToString().Trim()) * 20).ToString("#0.00");
                    }
                }
            }




            /**********************************************************************************************************************************************/

            dsruchang.AcceptChanges();


            /*************************************************************************************当月关键绩效考核得分**********************************************/

            foreach (DataRow drgjjxkhdf in dsruchang.Tables[0].Rows)
            {

                drgjjxkhdf["当月关键绩效考核得分"] = (double.Parse(drgjjxkhdf["E1排名序号（按数值从大到小）"].ToString().Trim()) * 0.5 + double.Parse(drgjjxkhdf["E3排名序号（按数值从大到小）"].ToString().Trim()) * 0.5).ToString("#0.00");
            }
            dsruchang.AcceptChanges();

            /********************************************************************************************************************************************************/

            DataView dv3 = dsruchang.Tables[0].DefaultView;
            dv3.Sort = "当月关键绩效考核得分  ";

            DataSet dshj = new DataSet();
            dshj.Tables.Add(dv3.ToTable());



            //   ds = dsruchang;

            DataSet dspaiming3 = CheckALL.DSAddID(dshj, "当月关键绩效考核得分排名");

            string Strbsc = ISbsc.isbsc(StrID);
            string StrGw = Hesion.Brick.Core.Users.GetUserByNumber(StrID).JobName.Trim();
            string StrBSc = "";

            if (Strbsc.IndexOf("办事处") > 0)
            {
                if (StrGw.IndexOf("总经理") <= 0)
                {
                    StrBSc = "  所属办事处='" + Strbsc.Trim() + "'";
                }
            }

            DataView dv4 = dspaiming3.Tables[0].DefaultView;
            dv4.RowFilter = StrBSc;
            DataSet ds4 = new DataSet();

            ds4.Tables.Add(dv4.ToTable());

            string strwhere = "";
            if (SSBSC.Trim() != "0" && SSBSC != "")
            {
                strwhere = "  所属办事处='" + SSBSC.Trim() + "'";
            }

            DataView dv5 = ds4.Tables[0].DefaultView;
            dv5.RowFilter = strwhere;
            DataSet ds5 = new DataSet();
            ds5.Tables.Add(dv5.ToTable());

            ds = ds5;


        }


        //string Strbsc = ISbsc.isbsc(StrID);
        //string StrBSc = "";

        //if (Strbsc.IndexOf("办事处") > 0)
        //{
        //    StrBSc = " and SSBSC='" + Strbsc.Trim() + "'";

        //}

        //DataView dv4 = ds.Tables[0].DefaultView;
        //dv4.RowFilter = "  SSBSC='" + Strbsc.Trim() + "'";
        //DataSet ds4 = new DataSet();

        //ds4.Tables.Add(dv4.ToTable());


        return ds;

    }


    public DataSet JiFenPaimiangNew(string StrID, string Year, string Month, string SSBSC)
    {
         DataSet ds = null;
         DateTime dt = new DateTime(int.Parse(Year.Trim()), int.Parse(Month.Trim()),1);
         string SMonNowS = dt.Year.ToString() + "/" + dt.Month.ToString() + "/01 00:00:00.000";
         string SmonNowO = dt.AddMonths(1).Year.ToString() + "/" + dt.AddMonths(1).Month.ToString() + "/01";
         string SMonLast = dt.AddMonths(-1).Year.ToString() + "/" + dt.AddMonths(-1).Month.ToString().Trim() + "/01 00:00:00.000";
    



        

       string  StrDDJlrswhere =   Year.Trim() + Month.Trim();

        /*本月日常积分*/
        //string StrAddjifennow = "select DENSE_RANK() over(order by Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) desc)  as 'E1排名序号（按数值从大到小）', SSBSC as '所属办事处', Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) as 'E1本月新增积分','' as '市场督导经理数量',0.00 as 'E2（当月日常积分/市场督导经理数量）',0 as 'E2排名序号（按数值从大到小)',0.00 as 'E3（当月日常积分/上月新增日常积分）',0.00 as 'E3排名序号（按数值从大到小）',0.00 as '积分排名（E1*0.4+E2*0.2+E3*0.4）',0 as '积分排名序号（从小到大排列）' from dbo.FWPT_FWSJFMXB where ChangeTime >='" + SMonNowS + "' and ChangeTime<'" + SmonNowO + "' group by  SSBSC";
       /*本月日常积分  ,'' as '当月关键绩效考核得分排名'*/
       string StrAddjifennow = "select RANK() over(order by Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) desc)  as 'E1排名序号（按数值从大到小）', SSBSC as '所属办事处',0.00 as 'E1考核得分（权重20分）', Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00)  when '扣减积分' then -isnull(ChangeNum,0.00) else 0.00 end ) as 'E1本月新增积分',0.00 as 'E2（当月日常积分/市场督导经理数量）','' as '市场督导经理数量','' as '市场督导经理人数' ,0 as 'E2排名序号（按数值从大到小)','' as 'E2考核得分（权重10分)',0.00 as 'E3（当月日常积分/上月新增日常积分）','' as '上月新增日常积分',0.00 as 'E3排名序号（按数值从大到小）',0.00 as 'E3考核得分（权重20分）',Sum(case BigClass  when 'A' then isnull(ChangeNum,0.00) else 0 end ) as '当月实际完成业绩积分（A）' ,0.00 as '当月业绩积分任务' ,'0.00%' as '当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）',0.00 as '当月业绩积分完成率考核得分（权重20分，不设上限）',Sum(case BigClass  when 'B' then isnull(ChangeNum,0.00) else 0 end ) as '当月新增支持积分（B）','' as '服务商支持情况考核得分（加分项，上限20分）',0.00 as '当月关键绩效考核得分'   from dbo.FWPT_FWSJFMXB where  FWSBH NOT LIKE '8%' AND   ChangeTime >='" + SMonNowS + "' and ChangeTime<'" + SmonNowO + "' group by  SSBSC ";

        DataSet dsruchang = DbHelperSQL.Query(StrAddjifennow);

        if (dsruchang != null && dsruchang.Tables[0].Rows.Count > 0)
        {

            /*督导经理人数*/
            string StrScddjlNum = "Select   SSBSC  as '所属办事处' ,  isnull(DDJLRS,1) as '督导经理人数' ,0.00 as 'E2（当月日常积分/市场督导经理数量）',0 as 'E2排名' from BSCDDJLNum  as a  where a.Number in(select top 1  Number  from BSCDDJLNum where SSBSC = a.SSBSC and (NF+YF) <='"+StrDDJlrswhere + "' order by  (NF+YF) desc  )"; //select SSBSC,DDJLRS from BSCDDJLNum


            DataSet dsdudjjNum = DbHelperSQL.Query(StrScddjlNum);
            if (dsdudjjNum != null && dsdudjjNum.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsruchang.Tables[0].Rows)    
                {
                    dr["E1考核得分（权重20分）"] = 20.00 - (double.Parse(dr["E1排名序号（按数值从大到小）"].ToString().Trim()) - 1) * 0.8;
                    double dbzhikeh = (double.Parse(dr["当月新增支持积分（B）"].ToString().Trim()) / 400.00);
                    if (dbzhikeh > 20)
                    {
                        dbzhikeh = 20.00;
                    }
                    dr["服务商支持情况考核得分（加分项，上限20分）"] = dbzhikeh;

                   

                    foreach (DataRow drdudd in dsdudjjNum.Tables[0].Rows)
                    {
                        if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                        {

                            dr["市场督导经理人数"] = drdudd["督导经理人数"].ToString();
                            dr["E2（当月日常积分/市场督导经理数量）"] = float.Parse(dr["E1本月新增积分"].ToString().Trim()) / float.Parse(drdudd["督导经理人数"].ToString());
                            drdudd["E2（当月日常积分/市场督导经理数量）"] = float.Parse(dr["E1本月新增积分"].ToString().Trim()) / float.Parse(drdudd["督导经理人数"].ToString());
                                
                            
                        }
                    }
                }


                dsruchang.AcceptChanges();
                dsdudjjNum.AcceptChanges();



                /*排序*/

                 

                 DataView dv = dsdudjjNum.Tables[0].DefaultView;
                 dv.Sort = "E2（当月日常积分/市场督导经理数量)  desc";

                 DataTable dkt = dv.ToTable();
                 DataSet dsky = new DataSet();
                 dsky.Tables.Add(dkt);
                 DataSet dspaiming = CheckALL.DSAddID(dsky, "E2排名2");
                  if (dspaiming != null && dspaiming.Tables[0].Rows.Count > 0)
                  {
                      foreach (DataRow dr in dsruchang.Tables[0].Rows)
                      {
                          foreach (DataRow drdudd in dspaiming.Tables[0].Rows)
                          {
                              if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                              {

                                  dr["E2排名序号（按数值从大到小)"] = drdudd["E2排名2"].ToString();
                                  dr["E2考核得分（权重10分)"] = (10.00 - (double.Parse(drdudd["E2排名2"].ToString()) - 1) * 0.4).ToString("#0.00");
     
                              }
                          }
                      }
                  }


                 
                 
                

            }


            dsruchang.AcceptChanges();



        
            /*上月日常积分*/
            string StrAddjifenlast = "select SSBSC as '所属办事处', Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) as '新增积分', 0.00 as 'E3（当月日常积分/上月新增日常积分）' from dbo.FWPT_FWSJFMXB  where  FWSBH NOT LIKE '8%' AND  ChangeTime >='" + SMonLast + "' and ChangeTime<'" + SMonNowS + "'  group by  SSBSC";
            DataSet dsADD = DbHelperSQL.Query(StrAddjifenlast);
            if (dsADD != null && dsADD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drjifen in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drlast in dsADD.Tables[0].Rows)
                    {
                        if (drjifen["所属办事处"].ToString().Trim() == drlast["所属办事处"].ToString().Trim())
                        {
                            drlast["E3（当月日常积分/上月新增日常积分）"] = float.Parse(drjifen["E1本月新增积分"].ToString().Trim()) / float.Parse(drlast["新增积分"].ToString().Trim());
                            drjifen["上月新增日常积分"] = drlast["新增积分"].ToString().Trim();
                        }
                    }
                }
            }
            dsADD.AcceptChanges();


            /*排序*/
            DataView dv2 = dsADD.Tables[0].DefaultView;
            dv2.Sort = "E3（当月日常积分/上月新增日常积分）  desc";

            DataTable dkt2 = dv2.ToTable();
            DataSet dsky2 = new DataSet();
            dsky2.Tables.Add(dkt2);
            DataSet dspaiming2 = CheckALL.DSAddID(dsky2, "E3排名");

            if (dspaiming2 != null && dspaiming2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drdudd in dspaiming2.Tables[0].Rows)
                    {
                        if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                        {

                            dr["E3（当月日常积分/上月新增日常积分）"] = drdudd["E3（当月日常积分/上月新增日常积分）"].ToString();
                            dr["E3排名序号（按数值从大到小）"] = drdudd["E3排名"].ToString();
                            dr["E3考核得分（权重20分）"] = (20.00 - (double.Parse(drdudd["E3排名"].ToString().Trim()) - 1) * 0.8).ToString("#0.00");
  
                        }
                    }
                }
            }

            dsruchang.AcceptChanges();




            /**********************************************************************************************************************************************/
            //添加办事处每月任务积分

            string StrGetMasTjifen = " select *,0.00 as 当月实际完成业绩积分,0.00 as '当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）' from BSCJIfenSet where (NF+YF)='" + Year.ToString().Trim() + Month.ToString().Trim() + "'";
            DataSet DsMastjifen = DbHelperSQL.Query(StrGetMasTjifen);

            if (DsMastjifen != null && DsMastjifen.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drrichang in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drRenwu in DsMastjifen.Tables[0].Rows)
                    {
                        if (drrichang["所属办事处"].ToString().Trim() == drRenwu["SSBSC"].ToString().Trim())
                        {
                            drrichang["当月业绩积分任务"] = drRenwu["YJFRW"].ToString().Trim();
                            drRenwu["当月实际完成业绩积分"] = drrichang["当月实际完成业绩积分（A）"].ToString().Trim();
                            drrichang["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"] = ((double.Parse(drrichang["当月实际完成业绩积分（A）"].ToString().Trim()) / double.Parse(drRenwu["YJFRW"].ToString().Trim())) * 100).ToString("#0.00") + "%";
                            drRenwu["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"] = ((double.Parse(drrichang["当月实际完成业绩积分（A）"].ToString().Trim()) / double.Parse(drRenwu["YJFRW"].ToString().Trim())));
                        }
                    }
                }
            }

            DsMastjifen.AcceptChanges();




            DataView dvdjNum = DsMastjifen.Tables[0].DefaultView;
            dvdjNum.Sort = " 当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）  desc";

            DataTable dktJNum = dvdjNum.ToTable();
            DataSet dskyJNum = new DataSet();
            dskyJNum.Tables.Add(dktJNum);
            DataSet dspaimingJnum = CheckALL.DSAddID(dskyJNum, " 任务积分完成率排名 ");

            //插入积分完成率排名

            foreach (DataRow drrichangpai in dsruchang.Tables[0].Rows)
            {
                foreach (DataRow drmastpaiming in dspaimingJnum.Tables[0].Rows)
                {
                    if (drrichangpai["所属办事处"].ToString().Trim() == drmastpaiming["SSBSC"].ToString().Trim())
                    {
                        drrichangpai["当月业绩积分完成率考核得分（权重20分，不设上限）"] = (double.Parse(drmastpaiming["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"].ToString().Trim()) * 20).ToString("#0.00");
                    }
                }
            }




            /**********************************************************************************************************************************************/



            dsruchang.AcceptChanges();


            /*************************************************************************************当月关键绩效考核得分**********************************************/

            foreach (DataRow drgjjxkhdf in dsruchang.Tables[0].Rows)
            {

                drgjjxkhdf["当月关键绩效考核得分"] = (double.Parse(drgjjxkhdf["E1排名序号（按数值从大到小）"].ToString().Trim()) * 0.5 + double.Parse(drgjjxkhdf["E3排名序号（按数值从大到小）"].ToString().Trim()) * 0.5).ToString("#0.00");
               // drgjjxkhdf["当月关键绩效考核得分"] = (double.Parse(drgjjxkhdf["E1考核得分（权重20分）"].ToString().Trim()) + double.Parse(drgjjxkhdf["E2考核得分（权重10分)"].ToString().Trim()) + double.Parse(drgjjxkhdf["E3考核得分（权重20分）"].ToString().Trim()) + double.Parse(drgjjxkhdf["当月业绩积分完成率考核得分（权重20分，不设上限）"].ToString().Trim()) + double.Parse(drgjjxkhdf["服务商支持情况考核得分（加分项，上限20分）"].ToString().Trim())).ToString("#0.00");
            }
            dsruchang.AcceptChanges();

            /********************************************************************************************************************************************************/




            DataView dv3 = dsruchang.Tables[0].DefaultView;
            dv3.Sort = "当月关键绩效考核得分  ";

            DataSet dshj = new DataSet();
            dshj.Tables.Add(dv3.ToTable());

             


         //   ds = dsruchang;

            DataSet dspaiming3 = CheckALL.DSAddID(dshj, "当月关键绩效考核得分排名");

            string Strbsc = ISbsc.isbsc(StrID);
            string StrGw = Hesion.Brick.Core.Users.GetUserByNumber(StrID).JobName.Trim();
            string StrBSc = "";

            if (Strbsc.IndexOf("办事处") > 0)
            {
                if (StrGw.IndexOf("总经理") <= 0)
                {
                    StrBSc = "  所属办事处='" + Strbsc.Trim() + "'";
                }
            }

            DataView dv4 = dspaiming3.Tables[0].DefaultView;
            dv4.RowFilter = StrBSc;
            DataSet ds4 = new DataSet();

            ds4.Tables.Add(dv4.ToTable());




            string strwhere = "";
            if (SSBSC.Trim() != "0" && SSBSC != "")
            {
                strwhere = "  所属办事处='" + SSBSC.Trim() + "'";
            }

            DataView dv5 = ds4.Tables[0].DefaultView;
            dv5.RowFilter = strwhere;
            DataSet ds5 = new DataSet();
            ds5.Tables.Add(dv5.ToTable());

            ds = ds5;


        }


        //string Strbsc = ISbsc.isbsc(StrID);
        //string StrBSc = "";

        //if (Strbsc.IndexOf("办事处") > 0)
        //{
        //    StrBSc = " and SSBSC='" + Strbsc.Trim() + "'";

        //}

        //DataView dv4 = ds.Tables[0].DefaultView;
        //dv4.RowFilter = "  SSBSC='" + Strbsc.Trim() + "'";
        //DataSet ds4 = new DataSet();

        //ds4.Tables.Add(dv4.ToTable());


        return ds;
    }


    public DataSet JiFenPaimiangNewrc(string StrID, string Year, string Month, string SSBSC)
    {
        DataSet ds = null;
        DateTime dt = new DateTime(int.Parse(Year.Trim()), int.Parse(Month.Trim()), 1);
        string SMonNowS = dt.Year.ToString() + "/" + dt.Month.ToString() + "/01 00:00:00.000";
        string SmonNowO = dt.AddMonths(1).Year.ToString() + "/" + dt.AddMonths(1).Month.ToString() + "/01";
        string SMonLast = dt.AddMonths(-1).Year.ToString() + "/" + dt.AddMonths(-1).Month.ToString().Trim() + "/01 00:00:00.000";






        string StrDDJlrswhere = Year.Trim() + Month.Trim();

        /*本月日常积分*/
        //string StrAddjifennow = "select DENSE_RANK() over(order by Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) desc)  as 'E1排名序号（按数值从大到小）', SSBSC as '所属办事处', Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) as 'E1本月新增积分','' as '市场督导经理数量',0.00 as 'E2（当月日常积分/市场督导经理数量）',0 as 'E2排名序号（按数值从大到小)',0.00 as 'E3（当月日常积分/上月新增日常积分）',0.00 as 'E3排名序号（按数值从大到小）',0.00 as '积分排名（E1*0.4+E2*0.2+E3*0.4）',0 as '积分排名序号（从小到大排列）' from dbo.FWPT_FWSJFMXB where ChangeTime >='" + SMonNowS + "' and ChangeTime<'" + SmonNowO + "' group by  SSBSC";
        /*本月日常积分  ,'' as '当月关键绩效考核得分排名'*/
        string StrAddjifennow = "select RANK() over(order by Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) desc)  as 'E1排名序号（按数值从大到小）', SSBSC as '所属办事处',0.00 as 'E1考核得分（权重20分）', Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00)  when '扣减积分' then -isnull(ChangeNum,0.00) else 0.00 end ) as 'E1本月新增积分',0.00 as 'E2（当月日常积分/市场督导经理数量）','' as '市场督导经理数量','' as '市场督导经理人数' ,0 as 'E2排名序号（按数值从大到小)','' as 'E2考核得分（权重10分)',0.00 as 'E3（当月日常积分/上月新增日常积分）','' as '上月新增日常积分',0.00 as 'E3排名序号（按数值从大到小）',0.00 as 'E3考核得分（权重20分）',Sum(case BigClass  when 'A' then isnull(ChangeNum,0.00) else 0 end ) as '当月实际完成业绩积分（A）' ,0.00 as '当月业绩积分任务' ,'0.00%' as '当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）',0.00 as '当月业绩积分完成率考核得分（权重20分，不设上限）',Sum(case BigClass  when 'B' then isnull(ChangeNum,0.00) else 0 end ) as '当月新增支持积分（B）','' as '服务商支持情况考核得分（加分项，上限20分）',0.00 as '当月关键绩效考核得分'   from dbo.FWPT_FWSJFMXB where  FWSBH NOT LIKE '8%' AND   ChangeTime >='" + SMonNowS + "' and ChangeTime<'" + SmonNowO + "' and BigClass='A'  group by  SSBSC ";

        DataSet dsruchang = DbHelperSQL.Query(StrAddjifennow);

        if (dsruchang != null && dsruchang.Tables[0].Rows.Count > 0)
        {

            /*督导经理人数*/
            string StrScddjlNum = "Select   SSBSC  as '所属办事处' ,  isnull(DDJLRS,1) as '督导经理人数' ,0.00 as 'E2（当月日常积分/市场督导经理数量）',0 as 'E2排名' from BSCDDJLNum  as a  where a.Number in(select top 1  Number  from BSCDDJLNum where SSBSC = a.SSBSC and (NF+YF) <='" + StrDDJlrswhere + "' order by  (NF+YF) desc  )"; //select SSBSC,DDJLRS from BSCDDJLNum


            DataSet dsdudjjNum = DbHelperSQL.Query(StrScddjlNum);
            if (dsdudjjNum != null && dsdudjjNum.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsruchang.Tables[0].Rows)
                {
                    dr["E1考核得分（权重20分）"] = 20.00 - (double.Parse(dr["E1排名序号（按数值从大到小）"].ToString().Trim()) - 1) * 0.8;
                    double dbzhikeh = (double.Parse(dr["当月新增支持积分（B）"].ToString().Trim()) / 400.00);
                    if (dbzhikeh > 20)
                    {
                        dbzhikeh = 20.00;
                    }
                    dr["服务商支持情况考核得分（加分项，上限20分）"] = dbzhikeh;



                    foreach (DataRow drdudd in dsdudjjNum.Tables[0].Rows)
                    {
                        if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                        {

                            dr["市场督导经理人数"] = drdudd["督导经理人数"].ToString();
                            dr["E2（当月日常积分/市场督导经理数量）"] = float.Parse(dr["E1本月新增积分"].ToString().Trim()) / float.Parse(drdudd["督导经理人数"].ToString());
                            drdudd["E2（当月日常积分/市场督导经理数量）"] = float.Parse(dr["E1本月新增积分"].ToString().Trim()) / float.Parse(drdudd["督导经理人数"].ToString());


                        }
                    }
                }


                dsruchang.AcceptChanges();
                dsdudjjNum.AcceptChanges();



                /*排序*/



                DataView dv = dsdudjjNum.Tables[0].DefaultView;
                dv.Sort = "E2（当月日常积分/市场督导经理数量)  desc";

                DataTable dkt = dv.ToTable();
                DataSet dsky = new DataSet();
                dsky.Tables.Add(dkt);
                DataSet dspaiming = CheckALL.DSAddID(dsky, "E2排名2");
                if (dspaiming != null && dspaiming.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsruchang.Tables[0].Rows)
                    {
                        foreach (DataRow drdudd in dspaiming.Tables[0].Rows)
                        {
                            if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                            {

                                dr["E2排名序号（按数值从大到小)"] = drdudd["E2排名2"].ToString();
                                dr["E2考核得分（权重10分)"] = (10.00 - (double.Parse(drdudd["E2排名2"].ToString()) - 1) * 0.4).ToString("#0.00");

                            }
                        }
                    }
                }






            }


            dsruchang.AcceptChanges();




            /*上月日常积分*/
            string StrAddjifenlast = "select SSBSC as '所属办事处', Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) as '新增积分', 0.00 as 'E3（当月日常积分/上月新增日常积分）' from dbo.FWPT_FWSJFMXB  where  FWSBH NOT LIKE '8%' AND  BigClass='A' and ChangeTime >='" + SMonLast + "' and ChangeTime<'" + SMonNowS + "'  group by  SSBSC";
            DataSet dsADD = DbHelperSQL.Query(StrAddjifenlast);
            if (dsADD != null && dsADD.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drjifen in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drlast in dsADD.Tables[0].Rows)
                    {
                        if (drjifen["所属办事处"].ToString().Trim() == drlast["所属办事处"].ToString().Trim())
                        {
                            drlast["E3（当月日常积分/上月新增日常积分）"] = float.Parse(drjifen["E1本月新增积分"].ToString().Trim()) / float.Parse(drlast["新增积分"].ToString().Trim());
                            drjifen["上月新增日常积分"] = drlast["新增积分"].ToString().Trim();
                        }
                    }
                }
            }
            dsADD.AcceptChanges();


            /*排序*/
            DataView dv2 = dsADD.Tables[0].DefaultView;
            dv2.Sort = "E3（当月日常积分/上月新增日常积分）  desc";

            DataTable dkt2 = dv2.ToTable();
            DataSet dsky2 = new DataSet();
            dsky2.Tables.Add(dkt2);
            DataSet dspaiming2 = CheckALL.DSAddID(dsky2, "E3排名");

            if (dspaiming2 != null && dspaiming2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drdudd in dspaiming2.Tables[0].Rows)
                    {
                        if (dr["所属办事处"].ToString().Trim() == drdudd["所属办事处"].ToString().Trim())
                        {

                            dr["E3（当月日常积分/上月新增日常积分）"] = drdudd["E3（当月日常积分/上月新增日常积分）"].ToString();
                            dr["E3排名序号（按数值从大到小）"] = drdudd["E3排名"].ToString();
                            dr["E3考核得分（权重20分）"] = (20.00 - (double.Parse(drdudd["E3排名"].ToString().Trim()) - 1) * 0.8).ToString("#0.00");

                        }
                    }
                }
            }

            dsruchang.AcceptChanges();




            /**********************************************************************************************************************************************/
            //添加办事处每月任务积分

            string StrGetMasTjifen = " select *,0.00 as 当月实际完成业绩积分,0.00 as '当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）' from BSCJIfenSet where (NF+YF)='" + Year.ToString().Trim() + Month.ToString().Trim() + "'";
            DataSet DsMastjifen = DbHelperSQL.Query(StrGetMasTjifen);

            if (DsMastjifen != null && DsMastjifen.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drrichang in dsruchang.Tables[0].Rows)
                {
                    foreach (DataRow drRenwu in DsMastjifen.Tables[0].Rows)
                    {
                        if (drrichang["所属办事处"].ToString().Trim() == drRenwu["SSBSC"].ToString().Trim())
                        {
                            drrichang["当月业绩积分任务"] = drRenwu["YJFRW"].ToString().Trim();
                            drRenwu["当月实际完成业绩积分"] = drrichang["当月实际完成业绩积分（A）"].ToString().Trim();
                            drrichang["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"] = ((double.Parse(drrichang["当月实际完成业绩积分（A）"].ToString().Trim()) / double.Parse(drRenwu["YJFRW"].ToString().Trim())) * 100).ToString("#0.00") + "%";
                            drRenwu["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"] = ((double.Parse(drrichang["当月实际完成业绩积分（A）"].ToString().Trim()) / double.Parse(drRenwu["YJFRW"].ToString().Trim())));
                        }
                    }
                }
            }

            DsMastjifen.AcceptChanges();




            DataView dvdjNum = DsMastjifen.Tables[0].DefaultView;
            dvdjNum.Sort = " 当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）  desc";

            DataTable dktJNum = dvdjNum.ToTable();
            DataSet dskyJNum = new DataSet();
            dskyJNum.Tables.Add(dktJNum);
            DataSet dspaimingJnum = CheckALL.DSAddID(dskyJNum, " 任务积分完成率排名 ");

            //插入积分完成率排名

            foreach (DataRow drrichangpai in dsruchang.Tables[0].Rows)
            {
                foreach (DataRow drmastpaiming in dspaimingJnum.Tables[0].Rows)
                {
                    if (drrichangpai["所属办事处"].ToString().Trim() == drmastpaiming["SSBSC"].ToString().Trim())
                    {
                        drrichangpai["当月业绩积分完成率考核得分（权重20分，不设上限）"] = (double.Parse(drmastpaiming["当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）"].ToString().Trim()) * 20).ToString("#0.00");
                    }
                }
            }




            /**********************************************************************************************************************************************/



            dsruchang.AcceptChanges();


            /*************************************************************************************当月关键绩效考核得分**********************************************/

            foreach (DataRow drgjjxkhdf in dsruchang.Tables[0].Rows)
            {

                drgjjxkhdf["当月关键绩效考核得分"] = (double.Parse(drgjjxkhdf["E1排名序号（按数值从大到小）"].ToString().Trim()) * 0.5 + double.Parse(drgjjxkhdf["E3排名序号（按数值从大到小）"].ToString().Trim()) * 0.5).ToString("#0.00");
                // drgjjxkhdf["当月关键绩效考核得分"] = (double.Parse(drgjjxkhdf["E1考核得分（权重20分）"].ToString().Trim()) + double.Parse(drgjjxkhdf["E2考核得分（权重10分)"].ToString().Trim()) + double.Parse(drgjjxkhdf["E3考核得分（权重20分）"].ToString().Trim()) + double.Parse(drgjjxkhdf["当月业绩积分完成率考核得分（权重20分，不设上限）"].ToString().Trim()) + double.Parse(drgjjxkhdf["服务商支持情况考核得分（加分项，上限20分）"].ToString().Trim())).ToString("#0.00");
            }
            dsruchang.AcceptChanges();

            /********************************************************************************************************************************************************/




            DataView dv3 = dsruchang.Tables[0].DefaultView;
            dv3.Sort = "当月关键绩效考核得分  ";

            DataSet dshj = new DataSet();
            dshj.Tables.Add(dv3.ToTable());




            //   ds = dsruchang;

            DataSet dspaiming3 = CheckALL.DSAddID(dshj, "当月关键绩效考核得分排名");

            string Strbsc = ISbsc.isbsc(StrID);
            string StrGw = Hesion.Brick.Core.Users.GetUserByNumber(StrID).JobName.Trim();
            string StrBSc = "";

            if (Strbsc.IndexOf("办事处") > 0)
            {
                if (StrGw.IndexOf("总经理") <= 0)
                {
                    StrBSc = "  所属办事处='" + Strbsc.Trim() + "'";
                }
            }

            DataView dv4 = dspaiming3.Tables[0].DefaultView;
            dv4.RowFilter = StrBSc;
            DataSet ds4 = new DataSet();

            ds4.Tables.Add(dv4.ToTable());




            string strwhere = "";
            if (SSBSC.Trim() != "0" && SSBSC != "")
            {
                strwhere = "  所属办事处='" + SSBSC.Trim() + "'";
            }

            DataView dv5 = ds4.Tables[0].DefaultView;
            dv5.RowFilter = strwhere;
            DataSet ds5 = new DataSet();
            ds5.Tables.Add(dv5.ToTable());

            ds = ds5;


        }


        //string Strbsc = ISbsc.isbsc(StrID);
        //string StrBSc = "";

        //if (Strbsc.IndexOf("办事处") > 0)
        //{
        //    StrBSc = " and SSBSC='" + Strbsc.Trim() + "'";

        //}

        //DataView dv4 = ds.Tables[0].DefaultView;
        //dv4.RowFilter = "  SSBSC='" + Strbsc.Trim() + "'";
        //DataSet ds4 = new DataSet();

        //ds4.Tables.Add(dv4.ToTable());


        return ds;
    }


    /// <summary>
    /// 督导经理排名
    /// </summary>
    /// <param name="StrID"></param>
    /// <param name="Year"></param>
    /// <param name="Month"></param>
    /// <param name="StrSSBSC"></param>
    /// <returns></returns>

    public DataSet DDJLJiFenPaimiangNew(string StrID, string Year, string Month, string SSBSC,string Strddjlxm)
    {
        DataSet ds = null;
        DateTime dt = DateTime.Now;

        if (Year.Trim() != "0" && Month.Trim() != "0" && Year.Trim() != "" && Month.Trim() != "")
        {
            dt = new DateTime(int.Parse(Year.Trim()), int.Parse(Month.Trim()), 1);
        }
       
          
        string SMonNowS = dt.Year.ToString() + "/" + dt.Month.ToString() + "/01 00:00:00.000";
        string SmonNowO = dt.AddMonths(1).Year.ToString() + "/" + dt.AddMonths(1).Month.ToString() + "/01";
        string SMonLast = dt.AddMonths(-1).Year.ToString() + "/" + dt.AddMonths(-1).Month.ToString().Trim() + "/01 00:00:00.000";

        
        string StrDDJlrswhere = Year.Trim() + Month.Trim();

        /*本月日常积分*/
        //string StrAddjifennow = "select DENSE_RANK() over(order by Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) desc)  as 'E1排名序号（按数值从大到小）', SSBSC as '所属办事处', Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) as 'E1本月新增积分','' as '市场督导经理数量',0.00 as 'E2（当月日常积分/市场督导经理数量）',0 as 'E2排名序号（按数值从大到小)',0.00 as 'E3（当月日常积分/上月新增日常积分）',0.00 as 'E3排名序号（按数值从大到小）',0.00 as '积分排名（E1*0.4+E2*0.2+E3*0.4）',0 as '积分排名序号（从小到大排列）' from dbo.FWPT_FWSJFMXB where ChangeTime >='" + SMonNowS + "' and ChangeTime<'" + SmonNowO + "' group by  SSBSC";
        /*本月日常积分  ,'' as '当月关键绩效考核得分排名'*/
        //string StrAddjifennow = "select RANK() over(order by Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) desc)  as 'E1排名序号（按数值从大到小）', SSBSC as '所属办事处',DDJLGH as 工号, DDJLXM as 督导经理姓名,0.00 as 'E1考核得分（权重20分）', Sum(case ZJType when '增加积分' then isnull(ChangeNum,0.00) when '扣减积分' then -isnull(ChangeNum,0.00) end ) as 'E1本月新增积分',0.00 as 'E2（当月日常积分/市场督导经理数量）','' as '市场督导经理数量','' as '市场督导经理人数' ,0 as 'E2排名序号（按数值从大到小)','' as 'E2考核得分（权重10分)',0.00 as 'E3（当月日常积分/上月新增日常积分）','' as '上月新增日常积分',0.00 as 'E3排名序号（按数值从大到小）',0.00 as 'E3考核得分（权重20分）',Sum(case BigClass  when 'A' then isnull(ChangeNum,0.00) else 0 end ) as '当月实际完成业绩积分（A）' ,0.00 as '当月业绩积分任务' ,'0.00%' as '当月业绩积分完成率（当月实际业绩积分/当月业绩任务积分*100%）',0.00 as '当月业绩积分完成率考核得分（权重20分，不设上限）',Sum(case BigClass  when 'B' then isnull(ChangeNum,0.00) else 0 end ) as '当月新增支持积分（B）','' as '服务商支持情况考核得分（加分项，上限20分）',0.00 as '当月关键绩效考核得分'   from dbo.FWPT_FWSJFMXB where ChangeTime >='" + SMonNowS + "' and ChangeTime<'" + SmonNowO + "'" + SSBSC + " group by  DDJLGH ";

        string StrAddjifennow = "select  LTrim(RTRim(SSBSC)) as '所属办事处',RTRIM(LTrim(DDJLGH)) as 督导经理工号, Rtrim(Ltrim(DDJLXM)) as 督导经理姓名,Sum(case BigClass  when 'A' then isnull(ChangeNum,0.00) else 0.00 end ) as '该市场督导经理当月新增业绩积分（A）',Sum(case BigClass  when 'A' then isnull(ChangeNum,0.00) else 0.00 end )/50 as '当月新增业绩积分得分（不设上限）',Sum(case BigClass  when 'B' then isnull(ChangeNum,0.00) else 0.00 end ) as '该市场督导经理当月新增支持积分（B）',Sum(case BigClass  when 'B' then isnull(ChangeNum,0.00) else 0.00 end )/150 as '当月新增支持积分得分（上限为30分）',(Sum(case BigClass  when 'A' then isnull(ChangeNum,0.00) else 0.00 end )+Sum(case BigClass  when 'B' then isnull(ChangeNum,0.00) else 0.00 end )- Sum(case BigClass  when 'C' then isnull(ChangeNum,0.00) else 0.00 end ))as '该市场督导经理当月新增日常积分（M）' , RANK() over ( order by (Sum(case BigClass  when 'A' then isnull(ChangeNum,0.00) else 0.00 end )+Sum(case BigClass  when 'B' then isnull(ChangeNum,0.00) else 0.00 end )- Sum(case BigClass  when 'C' then isnull(ChangeNum,0.00) else 0.00 end )) desc ) as '该市场督导经理新增日常积分全国排名' ,0.00 as '加分（10分）',0.00 as '当月关键绩效考核得分',0 as 当月关键绩效考核得分排名  from FWPT_FWSJFMXB where  FWSBH NOT LIKE '8%' ADN ChangeTime >='" + SMonNowS + "' and ChangeTime<'" + SmonNowO + "' Group BY SSBSC,DDJLGH,DDJLXM ";
        DataSet dsruchang = DbHelperSQL.Query(StrAddjifennow);

        if (dsruchang != null && dsruchang.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dsruchang.Tables[0].Rows)
            {
                dr["当月新增业绩积分得分（不设上限）"] = double.Parse(dr["当月新增业绩积分得分（不设上限）"].ToString().Trim()).ToString("#0.00");

                dr["当月新增支持积分得分（上限为30分）"] = double.Parse(dr["当月新增支持积分得分（上限为30分）"].ToString().Trim()).ToString("#0.00");
                if (int.Parse(dr["该市场督导经理新增日常积分全国排名"].ToString().Trim()) <= 10)
                {
                    dr["加分（10分）"] = 10.00;
                }

                if (double.Parse(dr["当月新增支持积分得分（上限为30分）"].ToString().Trim()) >= 30.00)
                {
                          dr["当月新增支持积分得分（上限为30分）"] = 30.00 ;
                }


                dr["当月关键绩效考核得分"] = (double.Parse(dr["当月新增业绩积分得分（不设上限）"].ToString().Trim()) + double.Parse(dr["当月新增支持积分得分（上限为30分）"].ToString().Trim()) + double.Parse(dr["加分（10分）"].ToString().Trim())).ToString("#0.00");
            }

            dsruchang.AcceptChanges();

            DataView dv = dsruchang.Tables[0].DefaultView;
            dv.Sort = "当月关键绩效考核得分 desc ";
            

            DataSet dshj = new DataSet();
            dshj.Tables.Add(dv.ToTable());



            //排名处理
            for (int i = 0; i <dshj.Tables[0].Rows.Count;i++ )
            {
                // dsruchang.Tables[0].Rows[i - 1];
                if (i == 0)
                {
                    dshj.Tables[0].Rows[i]["当月关键绩效考核得分排名"] = 1;
                }
                else
                {
                    if (double.Parse(dshj.Tables[0].Rows[i]["当月关键绩效考核得分"].ToString().Trim()) == double.Parse(dshj.Tables[0].Rows[i - 1]["当月关键绩效考核得分"].ToString().Trim()))
                    {
                        dshj.Tables[0].Rows[i]["当月关键绩效考核得分排名"] = dshj.Tables[0].Rows[i - 1]["当月关键绩效考核得分排名"].ToString().Trim();
                    }
                    else
                    {
                        dshj.Tables[0].Rows[i]["当月关键绩效考核得分排名"] = i+1;
                    }
                }
            }


            dshj.AcceptChanges();

            DataSet dspaiming3 = dshj; //CheckALL.DSAddID(dshj, "当月关键绩效考核得分排名");

            string Strbsc = ISbsc.isbsc(StrID);
            string StrGw = Hesion.Brick.Core.Users.GetUserByNumber(StrID).JobName.Trim();
            string StrBSc = "";

            if (Strbsc.IndexOf("办事处") > 0)
            {
                if (StrGw.IndexOf("总经理") <= 0)
                {
                    StrBSc = "  所属办事处='" + Strbsc.Trim() + "'";
                }
            }

         

            DataView dv2 = dspaiming3.Tables[0].DefaultView;
            dv2.RowFilter = StrBSc;
            DataSet ds2 = new DataSet();

            ds2.Tables.Add(dv2.ToTable());

            string Strxm = "";
            if (Strddjlxm.Trim() != "")
            {
                Strxm = " 督导经理姓名 like '%" + Strddjlxm + "%'";
            }
            DataView dv3 = ds2.Tables[0].DefaultView;
            dv3.RowFilter = Strxm;

            DataSet ds3 = new DataSet();
            ds3.Tables.Add(dv3.ToTable());

            


            string strwhere = "";
            if (SSBSC.Trim() != "0" && SSBSC != "")
            {
                strwhere = "  所属办事处='" + SSBSC.Trim() + "'";
            }

            DataView dv5 = ds3.Tables[0].DefaultView;
            dv5.RowFilter = strwhere;
            DataSet ds5 = new DataSet();
            ds5.Tables.Add(dv5.ToTable());


            ds = ds5;

        }


        //string Strbsc = ISbsc.isbsc(StrID);
        //string StrBSc = "";

        //if (Strbsc.IndexOf("办事处") > 0)
        //{
        //    StrBSc = " and SSBSC='" + Strbsc.Trim() + "'";

        //}

        //DataView dv4 = ds.Tables[0].DefaultView;
        //dv4.RowFilter = "  SSBSC='" + Strbsc.Trim() + "'";
        //DataSet ds4 = new DataSet();

        //ds4.Tables.Add(dv4.ToTable());


        return ds;
    }
}