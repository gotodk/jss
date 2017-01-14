using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///买家购物车临时表类
/// </summary>
namespace MMweb.LJ
{
    public class CarLS
    {
        private string buydlzh = "";
        private string buydlzhlx = "";
        private string buykhbh = "";
        private string salekhbh = "";
        private string gmspbh = "";
        private int gmsl = 0;
        public CarLS()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 全字段构造
        /// </summary>
        /// <param name="buydlzh">登录账号</param>
        /// <param name="buydlzhlx">买家登陆账号类型</param>
        /// <param name="buykhbh"> 买家客户编号</param>
        /// <param name="salekhbh">卖家客户编号</param>
        /// <param name="gmspbh">购买商品编号</param>
        /// <param name="gmsl">购买数量</param>
        public CarLS(string buydlzh, string buydlzhlx, string buykhbh, string salekhbh, string gmspbh, int gmsl)
        {

            this.buydlzh = buydlzh;
            this.buydlzhlx = buydlzhlx;
            this.buykhbh = buykhbh;
            this.salekhbh = salekhbh;
            this.gmspbh = gmspbh;
            this.gmsl = gmsl;
        }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string BuyDLZH
        {
            get
            {
                return buydlzh;
            }
            set
            {
                buydlzh = value;
            }
        }
        /// <summary>
        /// 买家登陆账号类型
        /// </summary>
        public string BuyDLZHLX
        {
            get
            {
                return buydlzhlx;
            }
            set
            {
                buydlzhlx = value;
            }
        }
        /// <summary>
        /// 买家客户编号
        /// </summary>
        public string BuyKHBH
        {
            get
            {
                return buykhbh;
            }
            set
            {
                buykhbh = value;
            }
        }
        /// <summary>
        /// 卖家客户编号
        /// </summary>
        public string SaleKHBH
        {
            get
            {
                return salekhbh;
            }
            set
            {
                salekhbh = value;
            }
        }
        /// <summary>
        /// 购买商品编号
        /// </summary>
        public string GMSPBH
        {
            get
            {
                return gmspbh;
            }
            set
            {
                gmspbh = value;
            }
        }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int GMSL
        {
            get
            {
                return gmsl;
            }
            set
            {
                gmsl = value;
            }
        }

    }
}