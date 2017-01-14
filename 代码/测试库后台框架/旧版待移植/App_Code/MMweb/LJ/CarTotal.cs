using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using FMOP.DB;
using System.Collections;
/// <summary>
///购物车汇总类  刘杰 2012-04-12
/// </summary>
namespace MMweb.LJ
{
    public class CarTotal
    {

        private int totalnum = 0;         //数量汇总（分汇总或总汇总）
        private double totalmoney = 0.00; //金额汇总（分汇总或总汇总）


        #region 构造函数
        public CarTotal()
        {          
        }
        /// <summary>
        /// 根据账号类型得到买家购物车的总数量和总钱数（构造）
        /// </summary>
        /// <param name="buykhbh">买家客户编号</param>
        /// <param name="pType"></param>
        public CarTotal(string buykhbh, string pType)
        {
            GetTotal(buykhbh.Trim(), pType.Trim(), ""); //得到买家所有的
        }
        /// <summary>
        /// 根据账号类型得到买家购物车相应卖家（卖家超市）的分总数量和分总钱数
        /// </summary>
        /// <param name="buykhbh"></param>
        /// <param name="pType"></param>
        /// <param name="salekhbh"></param>
        public CarTotal(string buykhbh, string pType, string salekhbh)
        {
            GetTotal(buykhbh.Trim(), pType.Trim(), salekhbh.Trim());
        }
        #endregion
        #region 属性
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalNum
        {
            get
            {
                return totalnum;
            }

        }
        /// <summary>
        /// 总金额
        /// </summary>
        public double TotalMoney
        {
            get
            {
                return totalmoney;
            }

        }
        #endregion

        ///// <summary>
        ///// 根据买家客户编号得到所有的购买数量（不用了）
        ///// </summary>
        ///// <param name="kjkhbh"></param>
        ///// <returns></returns>
        //public int GetTotalNum(string kjkhbh)
        //{
        //    string sqls = "select isnuLl(sum(GMSl),0) from MM_BuyGWCLSBC where BuyKHBH='"+kjkhbh.Trim()+"'";
        //    DataSet ds = DbHelperSQL.Query(sqls);
        //    return Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

        //}

        #region 通过函数得到购物车汇总
        /// <summary>
        /// 根据买家客户编号得到购物车所有金钱
        /// </summary>
        /// <param name="buykhbh">卖家客户编号</param>
        /// <param name="pType">类型：普通（默认）、优惠、工会会员</param>
        /// <returns></returns>
        public Hashtable GetTotal(string buykhbh, string pType, string salekhbh)
        {
            Hashtable tb = new Hashtable();
            string sqls = "";
            switch (pType)
            {
                case "普通":
                    sqls = "select isNull(SUM(优惠价格*数量),0) as 总金额,isNUll(sum(数量),0) as 总数量 from view_FMWGWC  ";
                    break;
                case "优惠":
                    sqls = "select isNull(SUM(普通价格*数量),0) as 总金额,isNUll(SUM(数量),0) as 总数量 from view_FMWGWC   ";
                    break;
                case "工会会员":
                    sqls = "select isNull(SUM(工会会员价格*数量),0) as 总金额,isNull(SUM(数量),0) as 总数量 from view_FMWGWC ";
                    break;
                default:
                    sqls = "select SUM(isnull(普通价格,0)*isnull(数量,0)) as 总金额,SUM(数量) as 总数量 from view_FMWGWC ";
                    break;
            }
            sqls += " where 买家客户编号='" + buykhbh.Trim() + "'";
            if (salekhbh != "")
            {
                sqls += " and 卖家客户编号='" + salekhbh.Trim() + "'";
            }
            DataSet ds = DbHelperSQL.Query(sqls);
            this.totalnum = Convert.ToInt16(ds.Tables[0].Rows[0][1].ToString());
            this.totalmoney = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString()), 2);
            tb["总金额"] = this.totalmoney;
            tb["总数量"] = this.totalnum;
            return tb;

        }
        #endregion
   
    }
}