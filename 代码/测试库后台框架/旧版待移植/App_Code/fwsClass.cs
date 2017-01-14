using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using FMOP.DB;
/// <summary>
///fwsClass 的摘要说明
/// </summary>
namespace ZTC
{
    public class fwsClass
    {
        private string fwsemail;
        private string fwsBH;
        public fwsClass()
        {
    
        }
        public fwsClass(string emails)
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
            this.fwsemail = emails;
        }
        /// <summary>
        /// 通过服务商emailID,得到服务商信息
        /// </summary>
        /// <returns>emailID</returns>
        public fwsInfo GetFwsInfo()
        {
            fwsInfo f = new fwsInfo();
            DataSet ds = DbHelperSQL.Query("select KHBH,DWQC,SSBSC,LXRXM,LXRGH,SZSF from  FWPT_YHXXB where DLYX='" + fwsemail.Trim() + "'");
            if (ds.Tables[0].Rows.Count>0)
            {
                if (ds.Tables[0].Rows[0][0] != null)
                {
                    f.fwsID = ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    f.fwsID= "";
                }
                //
                if (ds.Tables[0].Rows[0][1] != null)
                {
                    f.fwsName = ds.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    f.fwsName = "";
                }
               //
                if (ds.Tables[0].Rows[0][2] != null)
                {
                    f.fwsSSBSC = ds.Tables[0].Rows[0][2].ToString();
                }
                else
                {
                    f.fwsSSBSC= "";
                }
                //
                if (ds.Tables[0].Rows[0][3] != null)
                {
                    f.fwsLXR = ds.Tables[0].Rows[0][3].ToString();
                }
                else
                {
                    f.fwsLXR= "";
                }
                //
                if (ds.Tables[0].Rows[0][4] != null)
                {
                    f.fwsGH = ds.Tables[0].Rows[0][4].ToString();
                }
                else
                {
                    f.fwsGH= "";
                }
                //
                if (ds.Tables[0].Rows[0][5] != null)
                {
                    f.fwsSF= ds.Tables[0].Rows[0][5].ToString();
                }
                else
                {
                    f.fwsSF = "";
                }

            }
            else
            {
                f.fwsID = "";
                f.fwsName = "";
                f.fwsSSBSC = "";
                f.fwsLXR = "";
                f.fwsGH = "";
                f.fwsSF = "";
            }
            //得到服务商用户点数
            f.fwsUserHyNum = GetHYP(f.fwsID);
            //得到服务商A分
            f.fwsANum = GetA(f.fwsID);    

            //得到服务商扣分
            f.fwsKF = 0;
            //得到总分
            f.fwsZF = f.fwsANum * 1 + f.fwsUserHyNum / 100 - f.fwsKF;

            if (f.fwsZF >= 0 && f.fwsZF < 5000)
            {
                f.fwsXJ = 1;
                f.fwsDJ = "1星";
                f.fwsNextF =Math.Round(5000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_star1.gif";
            }
            else if (f.fwsZF > 5000 && f.fwsZF < 10000)
            {
                f.fwsXJ = 2;
                f.fwsDJ = "2星";
                f.fwsNextF =Math.Round(10000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_star2.gif";
            }
            else if (f.fwsZF >= 10000 && f.fwsZF < 30000)
            {
                f.fwsXJ = 3;
                f.fwsDJ = "3星";
                f.fwsNextF =Math.Round(30000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_star3.gif";
            }
            else if (f.fwsZF >= 30000 && f.fwsZF < 50000)
            {
                f.fwsXJ = 4;
                f.fwsDJ = "4星";
                f.fwsNextF =Math.Round(50000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_star4.gif";
            }
            else if (f.fwsZF >= 50000 && f.fwsZF < 100000)
            {
                f.fwsXJ = 5;
                f.fwsDJ = "5星";
                f.fwsNextF =Math.Round(100000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_star5.gif";
            }
            //以上是星星
            else if (f.fwsZF >= 100000 && f.fwsZF < 200000)
            {
                f.fwsXJ = 6;
                f.fwsDJ = "1钻";
                f.fwsNextF =Math.Round(200000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_zs1.gif";
            }
            else if (f.fwsZF >= 200000 && f.fwsZF < 300000)
            {
                f.fwsXJ = 7;
                f.fwsDJ = "2钻";
                f.fwsNextF =Math.Round(300000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_zs2.gif";
            }
            else if (f.fwsZF >= 300000 && f.fwsZF < 500000)
            {
                f.fwsXJ = 8;
                f.fwsDJ = "3钻";
                f.fwsNextF =Math.Round(500000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_zs3.gif";
            }
            else if (f.fwsZF >= 500000 && f.fwsZF < 800000)
            {
                f.fwsXJ = 9;
                f.fwsDJ = "4钻";
                f.fwsNextF =Math.Round(800000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_zs4.gif";
            }
            else if (f.fwsZF >= 800000 && f.fwsZF < 1000000)
            {
                f.fwsXJ = 10;
                f.fwsDJ = "5钻";
                f.fwsNextF =Math.Round(1000000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_zs5.gif";
            } //以上是钻石
            else if (f.fwsZF >= 1000000 && f.fwsZF < 1500000)
            {
                f.fwsXJ = 11;
                f.fwsDJ = "1冠";
                f.fwsNextF =Math.Round(1500000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_hg1.gif";
            }
            else if (f.fwsZF >= 1500000 && f.fwsZF < 2000000)
            {
                f.fwsXJ = 12;
                f.fwsDJ = "2冠";
                f.fwsNextF =Math.Round(2000000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_hg2.gif";
            }
            else if (f.fwsZF >= 2000000 && f.fwsZF < 2500000)
            {
                f.fwsXJ = 13;
                f.fwsDJ = "3冠";
                f.fwsNextF = Math.Round(2500000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_hg3.gif";
            }
            else if (f.fwsZF >= 2500000 && f.fwsZF < 3000000)
            {
                f.fwsXJ = 14;
                f.fwsDJ = "4冠";
                f.fwsNextF = Math.Round(3000000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_hg4.gif";
            }
            else if (f.fwsZF >= 3000000)
            {
                f.fwsXJ = 15;
                f.fwsDJ = "5冠";
                f.fwsNextF = 0;
                f.fwsStarPath = "images/fw_hg5.gif";
            }
            else
            {
                f.fwsXJ = 0;
                f.fwsDJ = "0星";
                f.fwsNextF = 0;
                f.fwsStarPath = "images/fw_starhui.gif";
            }
            return f;

        }
        /// <summary>
        /// 根据服务商编号得到服务上信息（重载方法）
        /// </summary>
        /// <param name="fwsBH">服务商编号</param>
        /// <returns></returns>
        public fwsInfo GetFwsInfo(string fwsBH)
        {
            fwsInfo f = new fwsInfo();
            DataSet ds = DbHelperSQL.Query("select KHBH,DWQC,SSBSC from  FWPT_YHXXB where KHBH='" + fwsBH.Trim() + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0] != null)
                {
                    f.fwsID = ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    f.fwsID = "";
                }
                //
                if (ds.Tables[0].Rows[0][1] != null)
                {
                    f.fwsName = ds.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    f.fwsName = "";
                }
                //
                if (ds.Tables[0].Rows[0][2] != null)
                {
                    f.fwsSSBSC = ds.Tables[0].Rows[0][2].ToString();
                }
                else
                {
                    f.fwsSSBSC = "";
                }
            }
            else
            {
                f.fwsID = "";
                f.fwsName = "";
                f.fwsSSBSC = "";
            }
            //得到服务商用户点数
            f.fwsUserHyNum = GetHYP(f.fwsID);
            //得到服务商A分
            f.fwsANum = GetA(f.fwsID);


            //得到服务商扣分
            f.fwsKF = 0;
            //得到总分
            f.fwsZF = f.fwsANum * 1 + f.fwsUserHyNum / 100 - f.fwsKF;

            if (f.fwsZF >= 0 && f.fwsZF < 5000)
            {
                f.fwsXJ = 1;
                f.fwsDJ = "1星";
                f.fwsNextF =Math.Round((5000 - f.fwsZF),2);
                f.fwsStarPath = "images/fw_star1.gif";
            }
            else if (f.fwsZF > 5000 && f.fwsZF < 10000)
            {
                f.fwsXJ = 2;
                f.fwsDJ = "2星";
                f.fwsNextF = Math.Round(10000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_star2.gif";
            }
            else if (f.fwsZF >= 10000 && f.fwsZF < 30000)
            {
                f.fwsXJ = 3;
                f.fwsDJ = "3星";
                f.fwsNextF = Math.Round(30000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_star3.gif";
            }
            else if (f.fwsZF >= 30000 && f.fwsZF < 50000)
            {
                f.fwsXJ = 4;
                f.fwsDJ = "4星";
                f.fwsNextF = Math.Round(50000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_star4.gif";
            }
            else if (f.fwsZF >= 50000 && f.fwsZF < 100000)
            {
                f.fwsXJ = 5;
                f.fwsDJ = "5星";
                f.fwsNextF = Math.Round(100000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_star5.gif";
            }
            //以上是星星
            else if (f.fwsZF >= 100000 && f.fwsZF < 200000)
            {
                f.fwsXJ = 6;
                f.fwsDJ = "1钻";
                f.fwsNextF = Math.Round(200000- f.fwsZF,2);
                f.fwsStarPath = "images/fw_zs1.gif";
            }
            else if (f.fwsZF >= 200000 && f.fwsZF < 300000)
            {
                f.fwsXJ = 7;
                f.fwsDJ = "2钻";
                f.fwsNextF = Math.Round(300000- f.fwsZF,2);
                f.fwsStarPath = "images/fw_zs2.gif";
            }
            else if (f.fwsZF >= 300000 && f.fwsZF < 500000)
            {
                f.fwsXJ = 8;
                f.fwsDJ = "3钻";
                f.fwsNextF =Math.Round(500000- f.fwsZF,2);
                f.fwsStarPath = "images/fw_zs3.gif";
            }
            else if (f.fwsZF >= 500000 && f.fwsZF < 800000)
            {
                f.fwsXJ = 9;
                f.fwsDJ = "4钻";
                f.fwsNextF = Math.Round(800000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_zs4.gif";
            }
            else if (f.fwsZF >= 800000 && f.fwsZF < 1000000)
            {
                f.fwsXJ = 10;
                f.fwsDJ = "5钻";
                f.fwsNextF = Math.Round(1000000 - f.fwsZF, 2);
                f.fwsStarPath = "images/fw_zs5.gif";
            } //以上是钻石
            else if (f.fwsZF >= 1000000 && f.fwsZF < 1500000)
            {
                f.fwsXJ = 11;
                f.fwsDJ = "1冠";
                f.fwsNextF = Math.Round(1500000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_hg1.gif";
            }
            else if (f.fwsZF >= 1500000 && f.fwsZF < 2000000)
            {
                f.fwsXJ = 12;
                f.fwsDJ = "2冠";
                f.fwsNextF = Math.Round(2000000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_hg2.gif";
            }
            else if (f.fwsZF >= 2000000 && f.fwsZF < 2500000)
            {
                f.fwsXJ = 13;
                f.fwsDJ = "3冠";
                f.fwsNextF = Math.Round(2500000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_hg3.gif";
            }
            else if (f.fwsZF >= 2500000 && f.fwsZF < 3000000)
            {
                f.fwsXJ = 14;
                f.fwsDJ = "4冠";
                f.fwsNextF = Math.Round(3000000 - f.fwsZF,2);
                f.fwsStarPath = "images/fw_hg4.gif";
            }
            else if (f.fwsZF >= 3000000)
            {
                f.fwsXJ = 15;
                f.fwsDJ = "5冠";
                f.fwsNextF =0;
                f.fwsStarPath = "images/fw_hg5.gif";
            }
            else
            {
                f.fwsXJ = 0;
                f.fwsDJ = "0星";
                f.fwsNextF =0;
                f.fwsStarPath = "images/fw_starhui.gif";
            }

            return f;

        }
        /// <summary>
        /// 得到所有用户活跃值总点数
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetHYP()
        {
            return DbHelperSQL.Query("select * from getFWSDS");
        }
       /// <summary>
       /// 得到单个服务商用户活跃值总点数
       /// </summary>
       /// <param name="fwsbh">服务商编号</param>
       /// <returns>double</returns>
        public double GetHYP(string fwsbh)
        {
            double r = 0.00;
            DataSet ds=DbHelperSQL.Query("select 点数 from getFWSDS where 服务商编号='" + fwsbh.Trim() + "'");
            if(ds!=null&& ds.Tables[0].Rows.Count>0)
            {
              r =Convert.ToDouble(ds.Tables[0].Rows[0][0]);
            }
            return r;
        }
        /// <summary>
        /// 得到所有服务商A分
        /// </summary>
        /// <returns></returns>
        public DataSet GetA()
        {
            return DbHelperSQL.Query("select FWSBH  as 服务商编号,isNull(sum(A),0) as A分 from FWPT_FWSJFZB group by FWSBH");
        }
        /// <summary>
        /// 得到某个服务商的A分
        /// </summary>
        /// <param name="fwsbh"></param>
        /// <returns></returns>
        public double GetA(string fwsbh)
        {
            double r = 0.00;
            DataSet ds = DbHelperSQL.Query("select isNull(sum(A),0) as A分 from FWPT_FWSJFZB where FWSBH='"+fwsbh.Trim()+"' group by FWSBH");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                r = Convert.ToDouble(ds.Tables[0].Rows[0][0]);
            }
            return r;
        }



    }
}