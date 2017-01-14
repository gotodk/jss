using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// ShopCar 的摘要说明
/// </summary>
namespace FM.Web
{
	public class ShopCar
	{
		private string model = string.Empty;
		private int count = 1;
		private float dj = 0;
		private float money = 0;
        private string hjj= string.Empty;

		public ShopCar()
		{
		}

		/// <summary>
		/// 产品型号
		/// </summary>
		public string Model
		{
			get {
				return model;
			}
			set {
				model = value;
			}
		}
		/// <summary>
		/// 产品数量
		/// </summary>
		public int Count
		{
			get
			{
				return count;
			}
			set
			{
				count = value;
			}
		}
        /// <summary>
        /// 提取产品型号与绿与蓝的价格,三个参数
        /// </summary>

        public string HJJ
        {
            get
            {
                return hjj;
            }
            set
            {
                hjj = value;
            }
        }


		/// <summary>
		/// 统计零售价
		/// </summary>
		public float DJ
		{
			get
			{
				return dj;
			}
			set
			{
				dj = value;
			}
		}
		/// <summary>
		/// 金额
		/// </summary>
		public float Money
		{
			get
			{
				return money;
			}
			set
			{
				money = value;
			}
		}
	}
}