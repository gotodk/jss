using System;
using System.Collections.Generic;
using System.Web;
using FMOP.DB;
using System.Data;
using Hesion.Brick.Core.WorkFlow;
using System.Collections;
/// <summary>
///LJ类空间
/// </summary>
namespace MMweb.LJ
{
    /// <summary>
    ///购物车动作类 刘杰 2010-04-12
    /// </summary>
    public class OrderAction
    {

        private CarLS cl;

        public OrderAction()
        {
           
        }
        /// <summary>
        /// 买家临时记录表构造
        /// </summary>
        /// <param name="cl"></param>
        public OrderAction(CarLS cl)
        {
            this.cl=cl;
        }
        /// <summary>
        /// 写入《买家购物车临时表》数据，需要使用带CarLS类参实例
        /// </summary>  
        /// <param name="saleNum">买入数量</param>
        /// <returns>是，否</returns>
        public Boolean AddOrder(int saleNum)
        {
            string sqls = "select isNull(sum(GMSL),0) from MM_BuyGWCLSBC where GMSPBH='" + cl.GMSPBH.Trim() + "'and BuyKHBH='" + cl.BuyKHBH.Trim() + "' and SaleKHBH='" + cl.SaleKHBH.Trim() + "'";
            DataSet ds =DbHelperSQL.Query(sqls);
            int gNum = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            if (gNum == 0)
            {
                //写入新的记录
                WorkFlowModule WFMJF = new WorkFlowModule("MM_BuyGWCLSBC");
                string KeyNumberJF = WFMJF.numberFormat.GetNextNumber();
                sqls = "INSERT INTO MM_BuyGWCLSBC(Number,BuyDLZH,BuyDLZHLX,BuyKHBH,SaleKHBH,GMSPBH,GMSL,CreateUser) VALUES";
                sqls += "('" + KeyNumberJF + "','" + cl.BuyDLZH.Trim() + "','" + cl.BuyDLZHLX.Trim() + "','" + cl.BuyKHBH.Trim() + "','" + cl.SaleKHBH.Trim() + "','" + cl.GMSPBH.Trim() + "','" + cl.GMSL + "','" + cl.BuyDLZH.Trim() + "')";
               
            }
            else
            {
                gNum = gNum + saleNum;
                sqls = "UPDATE MM_BuyGWCLSBC SET GMSL ='" + gNum + "' where GMSPBH='" + cl.GMSPBH.Trim() + "'and BuyKHBH='" + cl.BuyKHBH.Trim() + "' and SaleKHBH='" + cl.SaleKHBH.Trim() + "'";                        
            }
            int i = DbHelperSQL.ExecuteSql(sqls);
            if (i>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据“业务复合主键”删除《买家购物车临时表》一行数据（删除当前行）
        /// </summary>
        /// <param name="gmsspbh">购买商品编号</param>
        /// <param name="buykhbh">买家客户编号</param>
        /// <param name="salekhbh">卖家客户编号</param>
        /// <returns></returns>
        public Boolean DelOrderOne(string gmsspbh, string buykhbh, string salekhbh)
        {
            //删除《买家购物车临时表》          
            string CarLSSql = "delete from MM_BuyGWCLSBC  where GMSPBH='" + gmsspbh.Trim() + "'and BuyKHBH='" + buykhbh.Trim() + "' and SaleKHBH='" + salekhbh.Trim()+ "'";
            int i = DbHelperSQL.ExecuteSql(CarLSSql);
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      /// <summary>
      /// 根据“数据库主键”删除《买家购物车临时表》一行数据（删除当前行）
      /// </summary>
      /// <param name="numID">Number</param>
      /// <returns></returns>
        public Boolean DelOrderOne(string numID)
        {
            //删除《买家购物车临时表》          
            string CarLSSql = "delete from MM_BuyGWCLSBC  where Number='"+numID.Trim()+"'";
            int i = DbHelperSQL.ExecuteSql(CarLSSql);
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据“买家客户编号”删除该买家的购物车临时数据(删除买家整单）
        /// </summary>
        /// <param name="buykhbh">买家客户编号</param>
        /// <returns></returns>
        public Boolean DelThisOrder(string buykhbh)
        {
            //删除《买家购物车临时表》
            string CarLSSql = "delete from MM_BuyGWCLSBC  where  BuyKHBH='" + buykhbh + "'";    
            int i = DbHelperSQL.ExecuteSql(CarLSSql);
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       /// <summary>
        /// 根据“业务复合主键”更新该行的记录数据（更改当前行）
       /// </summary>
       /// <param name="gmsspbh">购买商品编号</param>
       /// <param name="buykhbh">买家客户编号</param>
       /// <param name="salekhbh">卖家客户编号</param>
       /// <param name="gNum">交易数量</param>
       /// <returns></returns>
        public Boolean UpdateOrder(string gmsspbh, string buykhbh, string salekhbh, int gNum)
        {
            string sqls = "UPDATE MM_BuyGWCLSBC SET GMSL ='" + gNum + "' where GMSPBH='" + gmsspbh.Trim() + "'and BuyKHBH='" + buykhbh.Trim() + "' and SaleKHBH='" + salekhbh.Trim() + "'";
            int i = DbHelperSQL.ExecuteSql(sqls);
            if (i>0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 根据“数据库主键”，更新该行的记录数据（更改当前行）
        /// </summary>
        /// <param name="numID">Number值（主键）</param>
        /// <returns></returns>
        public Boolean UpdateOrder(string numID,int gNum)
        {
            string sqls = "UPDATE MM_BuyGWCLSBC SET GMSL ='" + gNum + "' where Number='"+numID.Trim()+"'";
            int i = DbHelperSQL.ExecuteSql(sqls);
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 根据买入方客户编号,得到买入方购物车信息
        /// </summary>
        /// <param name="buyKHBH">买入方客户编号</param>
        /// <param name="lsItem">排序规则,如：“a,b,c”</param>
        /// <returns></returns>
        public DataSet SelectOrder(string buyKHBH,List<string> lsItem)
        {
            string sqls = "select * from MM_BuyGWCLSBC where ";
            string orderstr = " order by ";
            for (int i=0; i< lsItem.Count; i++)
            {
                if (i < lsItem.Count - 1)
                {
                    orderstr += lsItem[i].ToString().Trim() +",";
                }
                else
                {
                    orderstr += lsItem[i].ToString().Trim();
                }                    
            }    
            return DbHelperSQL.Query(sqls);
        }


    }
}