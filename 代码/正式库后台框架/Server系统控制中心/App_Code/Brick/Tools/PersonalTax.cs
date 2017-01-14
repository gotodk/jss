using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
namespace Hesion.Brick.Tools
{

/// <summary>
/// 个人所得税计算
/// </summary>
    public static class PersonalTax
    {
        /// <summary>
        /// 根据工资额，计算应缴纳的个人所得税
        /// </summary>
        /// <param name="Salary">工资额</param>
        /// <returns>应缴纳个人所得税的金额</returns>
        public static float GetTaxMoney(float Salary)
        {
            const float TaxStartCount = 1600;//起税额
            float TaxMoney = Salary - TaxStartCount;//应税额
            float Rate; //税率
            float DeductMoney;//扣除数

            //按照税率表计算税率和扣除数        
            if (TaxMoney <= 0)
            {
                Rate = 0;
                DeductMoney = 0;
            }
            else if (TaxMoney < 500)
            {
                Rate = 5;
                DeductMoney = 0;
            }
            else if (TaxMoney <= 2000)
            {
                Rate = 10;
                DeductMoney = 25;
            }
            else if (TaxMoney <= 5000)
            {
                Rate = 15;
                DeductMoney = 125;
            }
            else if (TaxMoney <= 20000)
            {
                Rate = 20;
                DeductMoney = 375;
            }
            else if (TaxMoney <= 40000)
            {
                Rate = 25;
                DeductMoney = 1375;
            }
            else if (TaxMoney <= 60000)
            {
                Rate = 30;
                DeductMoney = 3375;
            }
            else if (TaxMoney <= 80000)
            {
                Rate = 35;
                DeductMoney = 6375;
            }
            else if (TaxMoney <= 100000)
            {
                Rate = 40;
                DeductMoney = 10375;
            }
            else
            {
                Rate = 45;
                DeductMoney = 15375;
            }

            return (Salary * Rate / 100 - DeductMoney);
        }
    }
}
