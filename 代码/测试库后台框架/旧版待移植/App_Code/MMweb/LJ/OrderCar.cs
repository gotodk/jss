using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///OrderSP类
///购物车单行数据类；liujie 2012-04-10
/// </summary>
namespace MMweb.LJ
{
 
    public class OrderCar
    {
        public OrderCar()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string spNum
        {

            get;
            set;
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string spName
        {
            get;
            set;
        }
        /// <summary>
        /// 主图片路径
        /// </summary>
        public string spImagePath
        {
            get;
            set;
        }
        /// <summary>
        /// 购物车单价
        /// </summary>
        public double spDJ
        {
            get;
            set;

        }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int spMnum
        {
            get;
            set;
        }
        /// <summary>
        /// 小计
        /// </summary>
        public double spXJSum
        {
            get;
            set;
        }
        /// <summary>
        /// 发票（有，无）
        /// </summary>
        public string spFP
        {
            get;
            set;
        }
        /// <summary>
        /// 购买运费
        /// </summary>
        public double spYF
        {
            get;
            set;

        }

    }
}