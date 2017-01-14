using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.DB;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
/// <summary>
/// GNKH_Orders 的摘要说明
/// </summary>
namespace FMOP.JYJHDD
{
    public class GNKH_Orders
    {
        public GNKH_Orders()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private string parentNumber = "";
        public string ParentNumber
        {
            get
            {
                return parentNumber;
            }
            set
            {
                parentNumber = value;
            }
        }

        private string spbm = "";
        public string SPBM
        {
            get
            {
                return spbm;
            }
            set
            {
                spbm = value;
            }
        }

        private string GOODS_Type = string.Empty;
        public string GOODS_TYPE
        {
            get
            {
                return GOODS_Type;
            }
            set
            {
                GOODS_Type = value;
            }
        }

        private string cpxh = string.Empty;
        public string CPXH
        {
            get
            {
                return cpxh;
            }
            set
            {
                cpxh = value;
            }
        }

        private string xhfl = string.Empty;
        public string XHFL
        {
            get
            {
                return xhfl;
            }
            set
            {
                xhfl = value;
            }
        }

        private string dyjpp = string.Empty;
        public string DYJPP
        {
            get
            {
                return dyjpp;
            }
            set
            {
                dyjpp = value;
            }
        }

        private string  print_Type = string.Empty;
        public string PRINT_TYPE
        {
            get
            {
                return print_Type;
            }
            set
            {
                print_Type = value;
            }
        }

        private string print_Needs_Type = string.Empty;
        public string PRINT_NEEDS_TYPE
        {
            get
            {
                return print_Needs_Type;
            }
            set
            {
                print_Needs_Type = value;
            }
        }

        private string print_Needs_Standard = string.Empty;
        public string PRINT_NEEDS_STANDARD
        {
            get
            {
                return print_Needs_Standard;
            }
            set
            {
                print_Needs_Standard = value;
            }
        }

        private string print_Needs_Color = string.Empty;
        public string PRINT_NEEDS_COLOR
        {
            get
            {
                return print_Needs_Color;
            }
            set
            {
                print_Needs_Color = value;
            }
        }

        private string goods_Unit = string.Empty;
        public string GOODS_UNIT
        {
            get
            {
                return goods_Unit;
            }
            set
            {
                goods_Unit = value;
            }
        }

        private string goods_Price = string.Empty;
        public string GOODS_PRICE
        {
            get
            {
                return goods_Price;
            }
            set
            {
                goods_Price = value;
            }
        }

        private string goods_Count = string.Empty;
        public string GOODS_COUNT
        {
            get
            {
                return goods_Count;
            }
            set
            {
                goods_Count = value;
            }
        }

        private string je = string.Empty;
        public string JE
        {
            get
            {
                return je;
            }
            set
            {
                je = value;
            }
        }

        /// <summary>
        /// 保存子表
        /// </summary>
        /// <returns></returns>
        public List<CommandInfo> insert( List<CommandInfo> sqlTransList)
        {
            StringBuilder cmdText = new StringBuilder();
            //添加参数
            SqlParameter[] ParamArray =new SqlParameter[11];
            cmdText.Append("insert into GNKH_Orders(parentNumber,SPBM,GOODS_TYPE,CPXH,XHFL,PRINT_NEEDS_STANDARD,PRINT_NEEDS_COLOR,GOODS_UNIT,GOODS_PRICE,GOODS_COUNT,JE)");
            cmdText.Append("Values(@parentNumber,@SPBM,@GOODS_TYPE,@CPXH,@XHFL,@PRINT_NEEDS_STANDARD,@PRINT_NEEDS_COLOR,@GOODS_UNIT,@GOODS_PRICE,@GOODS_COUNT,@JE)");

            ParamArray[0] = new SqlParameter("@parentNumber", SqlDbType.VarChar, 50);
            ParamArray[0].Value = parentNumber;

            ParamArray[1] = new SqlParameter("@SPBM", SqlDbType.VarChar, 50);
            ParamArray[1].Value = spbm;

            ParamArray[2] = new SqlParameter("@GOODS_TYPE", SqlDbType.VarChar, 50);
            ParamArray[2].Value = GOODS_Type;

            ParamArray[3] = new SqlParameter("@CPXH", SqlDbType.VarChar, 50);
            ParamArray[3].Value = cpxh;

            ParamArray[4] = new SqlParameter("@XHFL", SqlDbType.VarChar, 50);
            ParamArray[4].Value = xhfl;

            ParamArray[5] = new SqlParameter("@PRINT_NEEDS_STANDARD", SqlDbType.VarChar, 50);
            ParamArray[5].Value = print_Needs_Standard;

            ParamArray[6] = new SqlParameter("@PRINT_NEEDS_COLOR", SqlDbType.VarChar, 50);
            ParamArray[6].Value = print_Needs_Color;

            ParamArray[7] = new SqlParameter("@GOODS_UNIT", SqlDbType.VarChar, 50);
            ParamArray[7].Value = goods_Unit;

            ParamArray[8] = setParameter("float", "GOODS_PRICE", 6, goods_Price);
            ParamArray[9] = setParameter("float", "GOODS_COUNT", 6, goods_Count);

            ParamArray[10] = setParameter("float", "JE", 6, je);

            sqlTransList.Add(MkCommandInfo(cmdText.ToString(),ParamArray));
            return sqlTransList;
        }

        private CommandInfo MkCommandInfo(string cmdText,SqlParameter[] paramArray)
        {
            CommandInfo commInfo = new CommandInfo();
            commInfo.CommandText = cmdText;
            commInfo.Parameters = paramArray;
            commInfo.cmdType = CommandType.Text;
            return commInfo;
        }

        private static SqlParameter setParameter(string fieldType, string FieldName, int length, string value)
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
