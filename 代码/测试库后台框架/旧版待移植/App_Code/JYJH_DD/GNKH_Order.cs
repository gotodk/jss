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
/// GNKH_Order 的摘要说明
/// </summary>
namespace FMOP.JYJHDD
{
    public class GNKH_Order
    {
        public GNKH_Order()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private string number;
        public string Number
        {
            set
            {
                number = value;
            }
            get
            {
                return number;
            }
        }

        private string cszx = string.Empty;
        public string CSZX
        {
            get
            {
                return cszx;
            }
            set
            {
                cszx = value;
            }
        }

        private string ddly = string.Empty;
        public string DDLY
        {
            get
            {
                return ddly;
            }

            set
            {
                ddly = value;
            }
        }

        private string media_source = string.Empty;
        public string MEDIA_SOURCE
        {
            get
            {
                return media_source;
            }
            set
            {
                media_source = value;
            }
        }
        private string ddzt = string.Empty;
        public string DDZT
        {
            get
            {
                return ddzt;
            }
            set
            {
                ddzt = value;
            }
        }

        private string jezj;
        public string GNKH_OrdersHJJE
        {
            get
            {
                return jezj;
            }
            set
            {
                jezj = value;
            }
        }

        private string zkhjezj;
        public string ZKHJEZJ
        {
            get
            {
                return zkhjezj;
            }
            set
            {
                zkhjezj = value;
            }
        }

        private string cust_ID = string.Empty;
        public string CUST_ID
        {
            get
            {
                return cust_ID;
            }
            set
            {
                cust_ID = value;
            }
        }

        private string cust_Name = string.Empty;
        public string CUST_NAME
        {
            get
            {
                return cust_Name;
            }
            set
            {
                cust_Name = value;
            }
        }

        private string linkman_Name = string.Empty;
        public string LINKMAN_NAME
        {
            get
            {
                return linkman_Name;
            }
            set
            {
                linkman_Name = value;
            }
        }

        private string lxrxb = string.Empty;
        public string LXRXB
        {
            get
            {
                return lxrxb;
            }
            set
            {
                lxrxb = value;
            }
        }

        private string linkman_Tel = string.Empty;
        public string LINKMAN_TEL
        {
            get
            {
                return linkman_Tel;
            }
            set
            {
                linkman_Tel = value;
            }
        }

        private string lxrdy = string.Empty;
        public string LXRDY
        {
            get
            {
                return lxrdy;
            }
            set
            {
                lxrdy = value;
            }
        }

        private string lxrdz = string.Empty;
        public string LXRDZ
        {
            get
            {
                return lxrdz;
            }
            set
            {
                lxrdz = value;
            }
        }

        private string fkfs = string.Empty;
        public string FKFS
        {
            get
            {
                return fkfs;
            }
            set
            {
                fkfs = value;
            }
        }

        private string fkrq = string.Empty;
        public string FKRQ
        {
            get
            {
                return fkrq;
            }
            set
            {
                fkrq = value;
            }
        }

        private string hkyd = string.Empty;
        public string HKYD
        {
            get
            {
                return hkyd;
            }
            set
            {
                hkyd = value;
            }
        }

        private string bcyf;
        public string BCYF
        {
            get
            {
                return bcyf;
            }
            set
            {
                bcyf = value;
            }
        }

        private string ljqk;
        public string LJQK
        {
            get
            {
                return ljqk;
            }
            set
            {
                ljqk = value;
            }
        }
        private string yddhsj = string.Empty;
        public string YDDHSJ
        {
            get
            {
                return yddhsj;
            }
            set
            {
                yddhsj = value;
            }
        }

        private string receive_Name = string.Empty;
        public string RECEIVE_NAME
        {
            get
            {
                return receive_Name;
            }
            set
            {
                receive_Name = value;
            }
        }

        private string shrxb = string.Empty;
        public string SHRXB
        {
            get
            {
                return shrxb;
            }
            set
            {
                shrxb = value;
            }
        }

        private string receive_Tel = string.Empty;
        public string RECEIVE_TEL
        {
            get
            {
                return receive_Tel;
            }
            set
            {
                receive_Tel = value;
            }
        }

        private string order_Address = string.Empty;
        public string ORDER_ADDRESS
        {
            get
            {
                return order_Address;
            }
            set
            {
                order_Address = value;
            }
        }

        private string receive_idcard = string.Empty;
        public string RECEIVE_IDCARD
        {
            get
            {
                return receive_idcard;
            }
            set
            {
                receive_idcard = value;
            }
        }

        private string yb = string.Empty;
        public string YB
        {
            get
            {
                return yb;
            }
            set
            {
                yb = value;
            }
        }

        private string qtyd = string.Empty;
        public string QTYD
        {
            get
            {
                return qtyd;
            }
            set
            {
                qtyd = value;
            }
        }

        private string fjxx = string.Empty;
        public string FJXX
        {
            get
            {
                return fjxx;
            }
            set
            {
                fjxx = value;
            }
        }

        private string nextChecker = "";
        public string NextChecker
        {
            get
            {
                return nextChecker;
            }
            set
            {
                nextChecker = value;
            }
        }

        private string userName = "";
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        private int checkState = 1;
        public int CheckState
        {
            get
            {
                return checkState;
            }
            set
            {
                checkState = value;
            }
        }


        /// <summary>
        /// 产生订单主表存库
        /// </summary>
        /// <returns></returns>
        public List<CommandInfo> Insert(List<CommandInfo> sqlTransList)
        {
            string cmdText = "";
            cmdText = @"INSERT INTO GNKH_Order ( 
                        Number ,CSZX ,DDLY ,MEDIA_SOURCE ,DDZT ,GNKH_OrdersHJJE ,ZKHJEZJ ,CUST_ID ,CUST_NAME ,
                        LINKMAN_NAME ,LXRXB ,LINKMAN_TEL ,LXRDY ,LXRDZ ,FKFS ,FKRQ ,HKYD ,BCYF ,LJQK ,YDDHSJ ,
                        RECEIVE_NAME ,SHRXB ,RECEIVE_TEL ,ORDER_ADDRESS ,RECEIVE_IDCARD ,YB ,QTYD ,FJXX ,
                        NextChecker ,CheckState ,CreateUser ,CreateTime
                        )
                        VALUES
                        (
                        @Number ,@CSZX ,@DDLY ,@MEDIA_SOURCE ,@DDZT ,@GNKH_OrdersHJJE ,@ZKHJEZJ ,@CUST_ID ,@CUST_NAME ,
                        @LINKMAN_NAME ,@LXRXB ,@LINKMAN_TEL ,@LXRDY ,@LXRDZ ,@FKFS ,@FKRQ ,@HKYD ,@BCYF ,@LJQK ,@YDDHSJ ,
                        @RECEIVE_NAME ,@SHRXB ,@RECEIVE_TEL ,@ORDER_ADDRESS ,@RECEIVE_IDCARD ,@YB ,@QTYD ,@FJXX ,@NextChecker ,
                        @CheckState ,@CreateUser ,getdate()
                        )";
            SqlParameter[] ParamArray = new SqlParameter[31];
            ParamArray[0] = new SqlParameter("@Number", SqlDbType.VarChar, 50);
            ParamArray[0].Value = number;

            ParamArray[1] = new SqlParameter("@CSZX", SqlDbType.VarChar, 50);
            ParamArray[1].Value = cszx;

            ParamArray[2] = new SqlParameter("@DDLY", SqlDbType.VarChar, 50);
            ParamArray[2].Value = ddly;

            ParamArray[3] = new SqlParameter("@MEDIA_SOURCE", SqlDbType.VarChar, 50);
            ParamArray[3].Value = media_source;

            ParamArray[4] = new SqlParameter("@DDZT", SqlDbType.VarChar, 50);
            ParamArray[4].Value = ddzt;

            ParamArray[5] = setParameter("float", "GNKH_OrdersHJJE", 6, jezj); //SetValue(ParamArray[5], jezj);

            ParamArray[6] = setParameter("float", "ZKHJEZJ", 6, zkhjezj);

            ParamArray[7] = new SqlParameter("@CUST_ID", SqlDbType.VarChar, 50);
            ParamArray[7].Value = cust_ID;

            ParamArray[8] = new SqlParameter("@CUST_NAME", SqlDbType.VarChar, 50);
            ParamArray[8].Value = cust_Name;

            ParamArray[9] = new SqlParameter("@LINKMAN_NAME", SqlDbType.VarChar, 50);
            ParamArray[9].Value = linkman_Name;

            ParamArray[10] = new SqlParameter("@LXRXB", SqlDbType.VarChar, 50);
            ParamArray[10].Value = lxrxb;

            ParamArray[11] = new SqlParameter("@LINKMAN_TEL", SqlDbType.VarChar, 50);
            ParamArray[11].Value = linkman_Tel;

            ParamArray[12] = new SqlParameter("@LXRDY", SqlDbType.VarChar, 50);
            ParamArray[12].Value = lxrdy;

            ParamArray[13] = new SqlParameter("@LXRDZ", SqlDbType.VarChar, 50);
            ParamArray[13].Value = lxrdz;

            ParamArray[14] = new SqlParameter("@FKFS", SqlDbType.VarChar, 50);
            ParamArray[14].Value = fkfs;

            ParamArray[15] = new SqlParameter("@FKRQ", SqlDbType.DateTime, 50);
            ParamArray[15].Value = fkrq;

            ParamArray[16] = new SqlParameter("@HKYD", SqlDbType.VarChar, 50);
            ParamArray[16].Value = hkyd;

            ParamArray[17] = setParameter("float", "BCYF", 6, bcyf);

            ParamArray[18] = setParameter("float", "LJQK", 6, ljqk);

            ParamArray[19] = new SqlParameter("@YDDHSJ", SqlDbType.DateTime);
            ParamArray[19].Value = yddhsj;  //日期

            ParamArray[20] = new SqlParameter("@RECEIVE_NAME", SqlDbType.VarChar, 50);
            ParamArray[20].Value = receive_Name;

            ParamArray[21] = new SqlParameter("@SHRXB", SqlDbType.VarChar, 50);
            ParamArray[21].Value = shrxb;

            ParamArray[22] = new SqlParameter("@RECEIVE_TEL", SqlDbType.VarChar, 50);
            ParamArray[22].Value = receive_Tel;

            ParamArray[23] = new SqlParameter("@ORDER_ADDRESS", SqlDbType.VarChar, 50);
            ParamArray[23].Value = order_Address;

            ParamArray[24] = new SqlParameter("@RECEIVE_IDCARD", SqlDbType.VarChar, 50);
            ParamArray[24].Value = receive_idcard;

            ParamArray[25] = new SqlParameter("@YB", SqlDbType.VarChar, 50);
            ParamArray[25].Value = yb;

            ParamArray[26] = new SqlParameter("@QTYD", SqlDbType.VarChar, 50);
            ParamArray[26].Value = qtyd;

            ParamArray[27] = new SqlParameter("@FJXX", SqlDbType.VarChar, 50);
            ParamArray[27].Value = fjxx;

            ParamArray[28] = new SqlParameter("@NextChecker", SqlDbType.VarChar, 50);
            ParamArray[28].Value = nextChecker;

            ParamArray[29] = new SqlParameter("@CheckState", SqlDbType.SmallInt);
            ParamArray[29].Value = checkState;

            ParamArray[30] = new SqlParameter("@CreateUser", SqlDbType.VarChar, 50);
            ParamArray[30].Value = userName;

            sqlTransList.Add(MkCommandInfo(cmdText, ParamArray));

            return sqlTransList;
        }

        /// <summary>
        /// 创建执行对象
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="paramArray"></param>
        private CommandInfo MkCommandInfo(string cmdText, SqlParameter[] paramArray)
        {
            CommandInfo commInfo = new CommandInfo();
            commInfo.CommandText = cmdText;
            commInfo.Parameters = paramArray;
            commInfo.cmdType = CommandType.Text;
            return commInfo;
        }

        private static SqlParameter setParameter(string fieldType, string FieldName, int length,string value)
        {
            SqlParameter parameter = null;
            switch (fieldType.ToLower())
            {
                case "float":
                    parameter = new SqlParameter("@" + FieldName, SqlDbType.Float);
                    if (value == "")
                    {
                        parameter.Value = DBNull.Value;
                    }
                    else
                    {
                        parameter.Value = value;
                    }
                    break;
                case "varchar":
                    parameter = new SqlParameter("@" + FieldName, SqlDbType.VarChar, length);
                    parameter.Value = value;
                    break;

                case "datetime":
                    parameter = new SqlParameter("@" + FieldName, SqlDbType.DateTime);
                    parameter.Value = value;
                    break;
            }
            return parameter;
        }

    }
}
