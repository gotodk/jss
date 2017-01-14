using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using FMOP.DB;

namespace FMOP.CallCenter
{
    /// <summary>
    ///AddOrder 的摘要说明
    /// </summary>
    public static class AddOrder
    {
        /// <summary>
        /// 向呼叫中心系统传送一个订单
        /// </summary>
        /// <param name="Number">订单号</param>
        /// <returns>true=成功，false=失败</returns>
        public static bool Add(string Number)
        {
            if (string.IsNullOrEmpty(Number))
                return false;
            string xml = GetOrderXml(Number);
            if (string.IsNullOrEmpty(xml))
                return false;
            CallCenterOrder.CallCenterAddOrder1Service service = new CallCenterOrder.CallCenterAddOrder1Service();
            string result = service.addOrder(xml, "201");
            return result == "0";
        }

        /// <summary>
        /// 组合xml串
        /// </summary>
        /// <param name="Number">订单号</param>
        /// <returns>xml</returns>
        private static string GetOrderXml(string Number)
        {
            SqlDataReader dr = DbHelperSQL.ExecuteReader("select * from GNKH_Order where Number='" + Number + "'");
            if (!dr.HasRows)
            {
                dr.Close();
                return null;
            }
            dr.Read();
            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<?xml version=\"1.0\" encoding=\"GB2312\"?>");
            xml.AppendLine("<ORDER>");
            xml.AppendLine(GetXmlItem("ORDER_INFO_ID", dr["Number"].ToString()));
            xml.AppendLine(GetXmlItem("CUST_CITY", dr["CSZX"].ToString()));
            xml.AppendLine(GetXmlItem("ORDER_SOURCE", dr["DDLY"].ToString()));
            xml.AppendLine(GetXmlItem("MEDIA_SOURCE", dr["MEDIA_SOURCE"].ToString()));
            xml.AppendLine(GetXmlItem("ORDER_FEE", dr["GNKH_OrdersHJJE"].ToString()));
            xml.AppendLine(GetXmlItem("ORDER_MONEY", dr["ZKHJEZJ"].ToString()));
            xml.AppendLine(GetXmlItem("CUST_ID", dr["CUST_ID"].ToString()));
            xml.AppendLine(GetXmlItem("CUST_NAME", dr["CUST_NAME"].ToString()));
            xml.AppendLine(GetXmlItem("LINKMAN_NAME", dr["LINKMAN_NAME"].ToString()));
            xml.AppendLine(GetXmlItem("LINKMAN_TEL", dr["LINKMAN_TEL"].ToString()));
            xml.AppendLine(GetXmlItem("OEDER_ACCOUNT", dr["BCYF"].ToString()));
            xml.AppendLine(GetXmlItem("ORDER_DATE", DateTime.Now.Date.ToShortDateString()));
            xml.AppendLine(GetXmlItem("ARRIVE_AT_DATE", dr["YDDHSJ"].ToString()));
            xml.AppendLine(GetXmlItem("RECEIVE_NAME", dr["RECEIVE_NAME"].ToString()));
            xml.AppendLine(GetXmlItem("RECEIVE_TEL", dr["RECEIVE_TEL"].ToString()));
            xml.AppendLine(GetXmlItem("ORDER_ADDRESS", dr["ORDER_ADDRESS"].ToString()));
            xml.AppendLine(GetXmlItem("RECEIVE_IDCARD", dr["RECEIVE_IDCARD"].ToString()));
            xml.AppendLine(GetXmlItem("RECEIVE_DEP", dr["CSZX"].ToString()));
            xml.AppendLine(GetXmlItem("ORDER_AGENT", dr["CreateUser"].ToString()));
            xml.AppendLine(GetXmlItem("ORDER_TYPE", "直销"));
            xml.AppendLine(GetXmlItem("ORDER_REMARK", "无"));
            xml.AppendLine(GetXmlItem("IS_GET_INVOICE", "1"));
            xml.AppendLine(GetXmlItem("INVOICE_CONTENT", "1"));
            xml.AppendLine("<PRODINFO>");
            xml.AppendLine(GetOrderDetail(Number));
            xml.AppendLine("</PRODINFO>");
            xml.AppendLine("</ORDER>");
            dr.Close();
            return xml.ToString();
        }

        /// <summary>
        /// 组合子表xml串
        /// </summary>
        /// <param name="Number">单号</param>
        /// <returns>xml</returns>
        private static string GetOrderDetail(string Number)
        {
            StringBuilder xml = new StringBuilder();
            SqlDataReader dr = DbHelperSQL.ExecuteReader("select * from GNKH_Orders where parentNumber='" + Number + "'");
            while (dr.Read())
            {
                xml.AppendLine("<item>");
                if (!string.IsNullOrEmpty(dr["SPBM"].ToString()))
                    xml.AppendLine(GetXmlItem("GOODS_ID", dr["SPBM"].ToString()));
                if (!string.IsNullOrEmpty(dr["GOODS_TYPE"].ToString()))
                    xml.AppendLine(GetXmlItem("GOODS_NAME", dr["GOODS_TYPE"].ToString()));
                if (!string.IsNullOrEmpty(dr["CPXH"].ToString()))
                    xml.AppendLine(GetXmlItem("GOODS_TYPE", dr["CPXH"].ToString()));
                if (!string.IsNullOrEmpty(dr["DYJPP"].ToString()))
                    xml.AppendLine(GetXmlItem("PRINT_CARDS", dr["DYJPP"].ToString()));
                if (!string.IsNullOrEmpty(dr["PRINT_TYPE"].ToString()))
                    xml.AppendLine(GetXmlItem("PRINT_TYPE", dr["PRINT_TYPE"].ToString()));
                if (!string.IsNullOrEmpty(dr["PRINT_NEEDS_TYPE"].ToString()))
                    xml.AppendLine(GetXmlItem("PRINT_NEEDS_TYPE", dr["PRINT_NEEDS_TYPE"].ToString()));
                if (!string.IsNullOrEmpty(dr["PRINT_NEEDS_STANDARD"].ToString()))
                    xml.AppendLine(GetXmlItem("PRINT_NEEDS_STANDARD", dr["PRINT_NEEDS_STANDARD"].ToString()));
                if (!string.IsNullOrEmpty(dr["PRINT_NEEDS_COLOR"].ToString()))
                    xml.AppendLine(GetXmlItem("PRINT_NEEDS_COLOR", dr["PRINT_NEEDS_COLOR"].ToString()));
                if (!string.IsNullOrEmpty(dr["GOODS_UNIT"].ToString()))
                    xml.AppendLine(GetXmlItem("GOODS_UNIT", dr["GOODS_UNIT"].ToString()));
                if (!string.IsNullOrEmpty(dr["GOODS_PRICE"].ToString()))
                    xml.AppendLine(GetXmlItem("GOODS_PRICE", dr["GOODS_PRICE"].ToString()));
                if (!string.IsNullOrEmpty(dr["GOODS_COUNT"].ToString()))
                    xml.AppendLine(GetXmlItem("GOODS_COUNT", dr["GOODS_COUNT"].ToString()));
                if (!string.IsNullOrEmpty(dr["JE"].ToString()))
                    xml.AppendLine(GetXmlItem("GOODS_MONEY", dr["JE"].ToString()));
                xml.AppendLine("</item>");
            }
            dr.Close();
            return xml.ToString();
        }

        /// <summary>
        /// 通过键和值创建一个xml项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetXmlItem(string key, string value)
        {
            return "<" + key + ">" + value + "</" + key + ">";
        }
    }
}