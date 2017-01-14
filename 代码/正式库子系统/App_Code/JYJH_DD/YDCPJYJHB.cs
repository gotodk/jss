using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Hesion.Brick.Core;
using FMOP.DB;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

/// <summary>
/// YDCPJYJHB 的摘要说明
/// </summary>
namespace FMOP.JYJHDD
{
    public class YDCPJYJHB
    {
        private string city = "";
        private string clientNo = "";
        private string orderDate = "";

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }


        public string ClientNo
        {
            get
            {
                return clientNo;
            }
            set
            {
                clientNo = value;
            }
        }


        public string OrderDate
        {
            get
            {
                return orderDate;
            }
            set
            {
                orderDate = value;
            }
        }


        public YDCPJYJHB()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public List<CommandInfo> Update(List<CommandInfo> sqlTransList)
        {
            string cmdText = "";
            ArrayList aryNumberList = new ArrayList();

            aryNumberList = getNumber();
            if (aryNumberList != null && aryNumberList.Count > 0)
            {
                for (int index = 0; index < aryNumberList.Count; index++)
                {
                    cmdText = "UPDATE ZXYWCSZXYDCPJYJHB_JH set isMakeDD = 1 where id=@id";
                    SqlParameter[] ParamArray = new SqlParameter[1];
                    ParamArray[0] = new SqlParameter("@id",SqlDbType.Int,8);
                    ParamArray[0].Value = Convert.ToInt64(aryNumberList[index].ToString());
                    sqlTransList.Add(MkCommandInfo(cmdText.ToString(), ParamArray));
                }
            }
            return sqlTransList;
        }

        public ArrayList getNumber()
        {
            ArrayList aryNumber = new ArrayList();
            DataSet result = new DataSet();
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("select ZXYWCSZXYDCPJYJHB_JH.id from ZXYWCSZXYDCPJYJHB ");
            cmdText.Append(" INNER JOIN ZXYWCSZXYDCPJYJHB_JH ON ZXYWCSZXYDCPJYJHB.Number = ZXYWCSZXYDCPJYJHB_JH.parentNumber ");
            cmdText.Append(" where ZXYWCSZXYDCPJYJHB.CS='" +city +"' AND ZXYWCSZXYDCPJYJHB_JH.YQDHSJ='"+orderDate +"' AND ZXYWCSZXYDCPJYJHB_JH.KHBH='" + clientNo +"'");
            cmdText.Append(" and isMakeDD !=1");
            result = DbHelperSQL.Query(cmdText.ToString());
            if (result != null && result.Tables[0] != null)
            {
                foreach (DataRow dr in result.Tables[0].Rows)
                {
                    if (dr["id"].ToString() != "")
                    {
                        aryNumber.Add(dr["id"].ToString());
                    }
                }
            }
            return aryNumber;
        }

        private CommandInfo MkCommandInfo(string cmdText, SqlParameter[] paramArray)
        {
            CommandInfo commInfo = new CommandInfo();
            commInfo.CommandText = cmdText;
            commInfo.Parameters = paramArray;
            commInfo.cmdType = CommandType.Text;
            return commInfo;
        }
    }
}
