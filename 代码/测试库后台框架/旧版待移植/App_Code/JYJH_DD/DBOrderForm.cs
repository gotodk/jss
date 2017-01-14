using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Hesion.Brick.Core;
using FMOP.DB;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Hesion.Brick.Core.WorkFlow;


/// <summary>
/// CreateOrderForm 的摘要说明
/// </summary>

namespace FMOP.JYJHDD
{
    public class DBOrderForm
    {

        private string keyNumber = "";
        public string KeyNumber
        {
            get
            {
                return keyNumber;
            }
        }

        private string nextChecker = "";
        public string NextChecker
        {
            get
            {
                return nextChecker;
            }
        }

        private int checkState = 0;

        public int CheckState
        {
            get
            {
                return checkState;
            }
        }

        public DBOrderForm()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
           
        /// <summary>
        /// 取指定数据流
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public string GetReader(string cmdText, string FieldName)
        {
            string FieldText = "";
            try
            {
                SqlDataReader dr = DbHelperSQL.ExecuteReader(cmdText);
                if (dr.HasRows)
                {
                    dr.Read();
                    FieldText = dr[FieldName].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return FieldText;
        }

        /// <summary>
        /// 取订单模块主键
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public string GetKeyNumber(string module)
        {
            WorkFlowModule WFM = new WorkFlowModule(module);
            keyNumber = WFM.numberFormat.GetNextNumber();
            return keyNumber;
        }

        /// <summary>
        /// 取审核人
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public string GetChecker(string module,string userName)
        {
            string role = "";
            Hesion.Brick.Core.WorkFlow.WorkFlowModule  wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
            if (wf.check != null)
            {
                role = wf.check.GetFirstCheckRole(KeyNumber,userName);
            }
            else
            {
                role = "";
            }
            if (role != null && role != "")
            {
                nextChecker = role;
                checkState = 0;
            }
            else
            {
                nextChecker = "";
                checkState = 1;
            }
            return nextChecker;
        }

        public string getOrderListSql(string city, string orderDate, string clientNo)
        {
            StringBuilder cmdText = new StringBuilder();

            cmdText.Append(" SELECT ");
            cmdText.Append("    ZXYWCSZXYDCPJYJHB_JH.WLBM,ZXYWCSZXYDCPJYJHB_JH.CPPL,");
            cmdText.Append("    ZXYWCSZXYDCPJYJHB_JH.CPXH,ZXYWCSZXYDCPJYJHB_JH.YS,");
            cmdText.Append("    ZXYWCSZXYDCPJYJHB_JH.GG,ZXYWCSZXYDCPJYJHB_JH.DJ,");
            cmdText.Append("    ZXYWCSZXYDCPJYJHB_JH.SLDW,ZXYWCSZXYDCPJYJHB_JH.JE,");
            cmdText.Append("    system_Products.XHFL,system_Products.DW");
            cmdText.Append("    FROM   ");
            cmdText.Append("        ZXYWCSZXYDCPJYJHB ");
            cmdText.Append("    INNER JOIN ZXYWCSZXYDCPJYJHB_JH ON ZXYWCSZXYDCPJYJHB.Number = ZXYWCSZXYDCPJYJHB_JH.parentNumber");
            cmdText.Append("    LEFT JOIN system_Products on WLBM = system_Products.Number");
            cmdText.Append("    WHERE ZXYWCSZXYDCPJYJHB.CS='" + city + "'");
            cmdText.Append("    AND ZXYWCSZXYDCPJYJHB_JH.YQDHSJ='" + orderDate + "'");
            cmdText.Append("    AND ZXYWCSZXYDCPJYJHB_JH.KHBH='" + clientNo + "'");
            return cmdText.ToString();
        }

        /// <summary>
        /// 发送提醒
        /// </summary>
        public void SendMsg(string module,string userName,string KeyNumber)
        {
            string role;
            Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
            if (wf.warning != null)
            {
                wf.warning.CreateWarningToRole(KeyNumber, 1, userName);
            }

            if (wf.check != null)
            {
                role = wf.check.GetFirstCheckRole(KeyNumber, userName);
            }
            else
            {
                role = "";
            }
            //添加审核提醒
            if (role != null && role.Trim() != "")
            {
                string msg = string.Empty;
                msg = "单号为:" + KeyNumber + "的" + wf.property.Title + "已经填写完成,请尽快审核!";
                //发送提醒信息
                if (wf.warning != null)
                {
                    wf.warning.CreateWarningToJobName(KeyNumber, msg, module, userName, role, 1);
                }
            }
        }
    }
}
