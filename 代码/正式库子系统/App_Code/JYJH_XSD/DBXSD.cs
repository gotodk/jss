using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using FMOP.DB;

/// <summary>
/// DBXSD 的摘要说明
/// </summary>
namespace FMOP.JYJH_XSD
{
    public class DBXSD
    {
        public DBXSD()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private string keyNumber;
        private string _number;
        private string _khbh;
        private string _khmc;
        private string _lxr;
        private string _lxdh;
        private string _cszx;
        private string _tggcs;
        private string _khjl;
        private string _jehj;
        private string _xjyd;
        private string _smfwdh;
        private string _xgwx;
        private string _dyjwx;
        private string _kghs;
        private string _dhsj;
        private string _shr;
        private string _shrxb;
        private string _shrdh;
        private string _shdz;
        private string _shrsfz;
        private string _yb;
        private string _qtyd;
        private string _fkfs;
        private string _fkrq;
        private string _hkyd;
        private string _bcyfk;
        private string _ljyfk;
        private string _slr;
        private string _slrq;
        private string _zbwlfhsj;
        private string _wlfhfs;
        private string _csdhsj;
        private string _csrksj;
        private string _cscksj;
        private string _cspssj;
        private string _psr;
        private string _psfs;
        private string _xsdzt;
        private string _fjxx;
        private string _nextchecker;
        private string _checkstate;
        private string _createuser;
        private string ddh;
        private string _fwry;
        private string _fhfs;
        private int minue = 0;

        public int Minue
        {
            set
            {
                minue = value;
            }
            get
            {
                return minue;
            }
        }
        public string Number
        {
            set { _number = value; }
            get { return _number; }
        }
        public string FWRY
        {
            set { _fwry = value; }
            get { return _fwry; }
        }

        public string DDH
        {
            set { ddh = value; }
            get { return ddh; }
        }

        public string KHBH
        {
            set { _khbh = value; }
            get { return _khbh; }
        }

        public string KHMC
        {
            set { _khmc = value; }
            get { return _khmc; }
        }

        public string LXR
        {
            set { _lxr = value; }
            get { return _lxr; }
        }

        public string LXDH
        {
            set { _lxdh = value; }
            get { return _lxdh; }
        }

        public string CSZX
        {
            set { _cszx = value; }
            get { return _cszx; }
        }

        public string TGGCS
        {
            set { _tggcs = value; }
            get { return _tggcs; }
        }

        public string KHJL
        {
            set { _khjl = value; }
            get { return _khjl; }
        }

        public string JEHJ
        {
            set { _jehj = value; }
            get { return _jehj; }
        }

        public string XJYD
        {
            set { _xjyd = value; }
            get { return _xjyd; }
        }

        public string SMFWDH
        {
            set { _smfwdh = value; }
            get { return _smfwdh; }
        }

        public string XGWX
        {
            set { _xgwx = value; }
            get { return _xgwx; }
        }

        public string DYJWX
        {
            set { _dyjwx = value; }
            get { return _dyjwx; }
        }

        public string KGHS
        {
            set { _kghs = value; }
            get { return _kghs; }
        }

        public string DHSJ
        {
            set { _dhsj = value; }
            get { return _dhsj; }
        }

        public string SHR
        {
            set { _shr = value; }
            get { return _shr; }
        }

        public string SHRXB
        {
            set { _shrxb = value; }
            get { return _shrxb; }
        }

        public string SHRDH
        {
            set { _shrdh = value; }
            get { return _shrdh; }
        }

        public string SHDZ
        {
            set { _shdz = value; }
            get { return _shdz; }
        }

        public string SHRSFZ
        {
            set { _shrsfz = value; }
            get { return _shrsfz; }
        }

        public string YB
        {
            set { _yb = value; }
            get { return _yb; }
        }

        public string QTYD
        {
            set { _qtyd = value; }
            get { return _qtyd; }
        }

        public string FKFS
        {
            set { _fkfs = value; }
            get { return _fkfs; }
        }

        public string FKRQ
        {
            set { _fkrq = value; }
            get { return _fkrq; }
        }

        public string HKYD
        {
            set { _hkyd = value; }
            get { return _hkyd; }
        }

        public string BCYFK
        {
            set { _bcyfk = value; }
            get { return _bcyfk; }
        }

        public string LJYFK
        {
            set { _ljyfk = value; }
            get { return _ljyfk; }
        }

        public string SLR
        {
            set { _slr = value; }
            get { return _slr; }
        }

        public string SLRQ
        {
            set { _slrq = value; }
            get { return _slrq; }
        }

        public string ZBWLFHSJ
        {
            set { _zbwlfhsj = value; }
            get { return _zbwlfhsj; }
        }

        public string WLFHFS
        {
            set { _wlfhfs = value; }
            get { return _wlfhfs; }
        }

        public string CSDHSJ
        {
            set { _csdhsj = value; }
            get { return _csdhsj; }
        }
  
        public string CSRKSJ
        {
            set { _csrksj = value; }
            get { return _csrksj; }
        }

        public string CSCKSJ
        {
            set { _cscksj = value; }
            get { return _cscksj; }
        }

        public string CSPSSJ
        {
            set { _cspssj = value; }
            get { return _cspssj; }
        }

        public string PSR
        {
            set { _psr = value; }
            get { return _psr; }
        }

        public string PSFS
        {
            set { _psfs = value; }
            get { return _psfs; }
        }

        public string XSDZT
        {
            set { _xsdzt = value; }
            get { return _xsdzt; }
        }

        public string FJXX
        {
            set { _fjxx = value; }
            get { return _fjxx; }
        }

        public string NextChecker
        {
            set { _nextchecker = value; }
            get { return _nextchecker; }
        }

        public string CheckState
        {
            set { _checkstate = value; }
            get { return _checkstate; }
        }

        public string CreateUser
        {
            set { _createuser = value; }
            get { return _createuser; }
        }

        public string KeyNumber
        {
            set
            {
                keyNumber = value;
            }
            get
            {
                return keyNumber;
            }
        }
        public string FHFS
        {
            set { _fhfs = value; }
            get { return _fhfs; }
        }



        public int MakeXSD()
        {
            List<CommandInfo> sqlTransList = new List<CommandInfo>();
            int rowsAffect = 0;
            sqlTransList = InsertCPXSD(sqlTransList);
            sqlTransList = InsertCPXSD_DHYD(sqlTransList);
            sqlTransList = UpdateGNKH_Order(sqlTransList);
            sqlTransList = AddXSDTODDMapping(sqlTransList);
            sqlTransList = UpdateCPXSD(sqlTransList);
            rowsAffect = DbHelperSQL.ExecuteSqlTranWithIndentity(sqlTransList);
            return rowsAffect;
        }

        /// <summary>
        /// 新增销售单主表
        /// </summary>
        private List<CommandInfo> InsertCPXSD(List<CommandInfo> sqlTransList)
        {
            string cmdText = "";
            SqlParameter[] parameter = new SqlParameter[45];
            cmdText = @"
                        INSERT INTO YXSYB_CPXSD 
                        ( 
	                        Number ,DDH,FWRY, KHBH ,KHMC ,LXR ,LXDH ,CSZX ,TGGCS ,KHJL ,JEHJ ,XJYD ,
	                        SMFWDH ,XGWX ,DYJWX ,KGHS ,DHSJ ,SHR ,SHRXB ,SHRDH ,SHDZ ,SHRSFZ ,
	                        YB ,QTYD ,FKFS ,FKRQ ,HKYD ,BCYFK ,LJYFK ,SLR ,SLRQ ,ZBWLFHSJ ,
	                        WLFHFS ,CSDHSJ ,CSRKSJ ,CSCKSJ ,CSPSSJ ,PSR ,PSFS ,XSDZT ,FJXX ,
	                        NextChecker ,CheckState ,CreateUser ,CreateTime,CheckLimitTime,FHFS 
                        ) 
                        VALUES
                        (
	                        @Number ,@DDH,@FWRY,@KHBH ,@KHMC ,@LXR ,@LXDH ,@CSZX ,@TGGCS ,@KHJL ,@JEHJ ,@XJYD ,
	                        @SMFWDH ,@XGWX ,@DYJWX ,@KGHS ,@DHSJ ,@SHR ,@SHRXB ,@SHRDH ,@SHDZ ,@SHRSFZ ,
	                        @YB ,@QTYD ,@FKFS ,@FKRQ ,@HKYD ,@BCYFK ,@LJYFK ,@SLR ,@SLRQ ,@ZBWLFHSJ ,
	                        @WLFHFS ,@CSDHSJ ,@CSRKSJ ,@CSCKSJ ,@CSPSSJ ,@PSR ,@PSFS ,@XSDZT ,@FJXX ,@NextChecker ,
	                        @CheckState ,@CreateUser ,getdate(),dbo.addMinute(getdate(),"+ minue+"),@FHFS)";

            //设定字段值
            parameter[0] = setParameter("varchar", "Number", 50, _number);
            parameter[1] = setParameter("varchar", "KHBH", 50, _khbh);
            parameter[2] = setParameter("varchar", "KHMC", 50, _khmc);
            parameter[3] = setParameter("varchar", "LXR", 50,  _lxr);
            parameter[4] = setParameter("varchar", "LXDH", 50, _lxdh);
            parameter[5] = setParameter("varchar", "CSZX", 50, _cszx);
            parameter[6] = setParameter("varchar", "TGGCS", 50, _tggcs);
            parameter[7] = setParameter("varchar", "KHJL", 50, _khjl);
            parameter[8] = setParameter("float", "JEHJ", 50, _jehj);
            parameter[9] = setParameter("text", "XJYD", 50, _xjyd);
            parameter[10] = setParameter("varchar", "SMFWDH", 50, _smfwdh);
            parameter[11] = setParameter("varchar", "XGWX", 50, _xgwx);
            parameter[12] = setParameter("varchar", "DYJWX", 50, _dyjwx);
            parameter[13] = setParameter("varchar", "KGHS", 50, _kghs);
            parameter[14] = setParameter("varchar", "DHSJ", 50, _dhsj);
            parameter[15] = setParameter("varchar", "SHR", 50, _shr);
            parameter[16] = setParameter("varchar", "SHRXB", 50, _shrxb);
            parameter[17] = setParameter("varchar", "SHRDH", 50, _shrdh);
            parameter[18] = setParameter("varchar", "SHDZ", 50, _shdz);
            parameter[19] = setParameter("varchar", "SHRSFZ", 50, _shrsfz);
            parameter[20] = setParameter("varchar", "YB", 50, _yb);
            parameter[21] = setParameter("text", "QTYD", 50, _qtyd);
            parameter[22] = setParameter("varchar", "FKFS", 50, _fkfs);
            parameter[23] = setParameter("DateTime", "FKRQ", 50, _fkrq);
            parameter[24] = setParameter("text", "HKYD", 50, _hkyd);
            parameter[25] = setParameter("float", "BCYFK", 50, _bcyfk);
            parameter[26] = setParameter("float", "LJYFK", 50, _ljyfk);
            parameter[27] = setParameter("varchar", "SLR", 50, _slr);
            parameter[28] = setParameter("DateTime", "SLRQ", 50, _slrq);
            parameter[29] = setParameter("DateTime", "ZBWLFHSJ", 50, _zbwlfhsj);
            parameter[30] = setParameter("varchar", "WLFHFS", 50, _wlfhfs);
            parameter[31] = setParameter("DateTime", "CSDHSJ", 50, _csdhsj);
            parameter[32] = setParameter("DateTime", "CSRKSJ", 50, _csrksj);
            parameter[33] = setParameter("DateTime", "CSCKSJ", 50, _cscksj);
            parameter[34] = setParameter("DateTime", "CSPSSJ", 50, _cspssj);
            parameter[35] = setParameter("varchar", "PSR", 50, _psr);
            parameter[36] = setParameter("varchar", "PSFS", 50, _psfs);
            parameter[37] = setParameter("varchar", "XSDZT", 50, _xsdzt);
            parameter[38] = setParameter("text", "FJXX", 50, _fjxx);
            parameter[39] = setParameter("varchar", "NextChecker", 50, _nextchecker);
            parameter[40] = setParameter("smallInt", "CheckState", 50, _checkstate);
            parameter[41] = setParameter("varchar", "CreateUser", 50, _createuser);
            parameter[42] = setParameter("varchar", "DDH", 50, ddh);
            parameter[43] = setParameter("varchar", "FWRY", 50, _fwry);
            parameter[44] = setParameter("varchar", "FHFS", 50, _fhfs);
            sqlTransList.Add(MkCommandInfo(cmdText, parameter));
            return sqlTransList;
        }

        //修改销售单的审核时间
        private List<CommandInfo> UpdateCPXSD(List<CommandInfo> sqlTransList)
        {
            string cmdText = " Update YXSYB_CPXSD set CheckLimitTime=dbo.addMinute(getdate()," + minue +") where Number=@Number";
            SqlParameter[] parameter = new SqlParameter[1];
            parameter[0] = setParameter("varchar", "Number", 50, _number);
            sqlTransList.Add(MkCommandInfo(cmdText, parameter));
            return sqlTransList;
        }

        /// <summary>
        /// 新增销售单从表
        /// </summary>
        /// <param name="sqlTransList"></param>
        /// <returns></returns>
        private List<CommandInfo> InsertCPXSD_DHYD(List<CommandInfo> sqlTransList)
        {
            DataSet GNKH_OrdersSet = new DataSet();
            string cmdText = @"INSERT INTO YXSYB_CPXSD_DHYD 
                                ( 
	                                parentNumber,CPBM ,CPPL ,CPXH ,XHFL ,
	                                DYJPP ,DYJXH ,HCXH ,GG ,YS ,DJ ,SL ,
	                                DW ,JE ,BPFHSL ,BPFHJE
                                ) 
                                VALUES 
                                (
	                                @parentNumber ,@CPBM ,@CPPL ,@CPXH ,@XHFL ,@DYJPP ,
	                                @DYJXH ,@HCXH ,@GG ,@YS ,@DJ ,@SL ,@DW ,@JE ,@BPFHSL ,
	                                @BPFHJE
                                )";

            //循环列表
            GNKH_OrdersSet = getGNDH_OrdersList();
            if (GNKH_OrdersSet != null && GNKH_OrdersSet.Tables[0] != null)
            {
                foreach (DataRow dr in GNKH_OrdersSet.Tables[0].Rows)
                {
                    SqlParameter[] parameter = new SqlParameter[16];
                    //设定字段
                    parameter[0] = setParameter("varchar", "parentNumber", 50,_number);
                    parameter[1] = setParameter("varchar", "CPBM", 50, dr["SPBM"].ToString());
                    parameter[2] = setParameter("varchar", "CPPL", 50, dr["GOODS_TYPE"].ToString());
                    parameter[3] = setParameter("varchar", "CPXH", 50, dr["CPXH"].ToString());
                    parameter[4] = setParameter("varchar", "XHFL", 50, dr["XHFL"].ToString());
                    parameter[5] = setParameter("varchar", "DYJPP", 50, dr["DYJPP"].ToString());
                    parameter[6] = setParameter("varchar", "DYJXH", 50, dr["PRINT_TYPE"].ToString());
                    parameter[7] = setParameter("varchar", "HCXH", 50, dr["PRINT_NEEDS_TYPE"].ToString());
                    parameter[8] = setParameter("varchar", "GG", 50, dr["PRINT_NEEDS_STANDARD"].ToString());
                    parameter[9] = setParameter("varchar", "YS", 50, dr["PRINT_NEEDS_COLOR"].ToString());
                    parameter[10] = setParameter("float","DJ",6,dr["GOODS_PRICE"].ToString());
                    parameter[11] = setParameter("float", "SL", 6, dr["GOODS_COUNT"].ToString());
                    parameter[12] = setParameter("varchar", "DW", 50, dr["GOODS_UNIT"].ToString());
                    parameter[13] = setParameter("float", "JE", 6, dr["JE"].ToString());
                    parameter[14] = setParameter("float", "BPFHSL", 50, dr["GOODS_COUNT"].ToString());
                    parameter[15] = setParameter("float", "BPFHJE", 50, dr["JE"].ToString());
                    
                    sqlTransList.Add(MkCommandInfo(cmdText, parameter));
                }
            }
            return sqlTransList;
        }

        private DataSet getGNDH_OrdersList()
        {
            string cmdText = "";
            DataSet resultSet = new DataSet();
            cmdText = "select * from GNKH_Orders where parentNumber='" + keyNumber +"'";
            resultSet = DbHelperSQL.Query(cmdText);
            return resultSet;
        }

        /// <summary>
        /// 修改订单表中是否生成销售单的状态位
        /// </summary>
        /// <param name="sqlTransList"></param>
        /// <returns></returns>
        private List<CommandInfo> UpdateGNKH_Order(List<CommandInfo> sqlTransList)
        {
            string cmdText = "UPDATE GNKH_ORDER set ISMakeXSD=1,DDZT='订单成功' where Number=@Number";
            SqlParameter[] Parameter = new SqlParameter[1];

            Parameter[0] = new SqlParameter("@Number", SqlDbType.VarChar, 50);
            Parameter[0].Value = keyNumber;
            sqlTransList.Add(MkCommandInfo(cmdText,Parameter));
            return sqlTransList;
        }

        private List<CommandInfo> AddXSDTODDMapping(List<CommandInfo> sqlTransList)
        {
            string cmdText = "insert into XSDTODDMapping(XSDNumber,DDNumber) values (@XSDNumber,@DDNumber)";
            SqlParameter[] Parameter = new SqlParameter[2];

            Parameter[0] = new SqlParameter("@XSDNumber", SqlDbType.VarChar, 50);
            Parameter[0].Value = _number;
            Parameter[1] = new SqlParameter("@DDNumber", SqlDbType.VarChar, 50);
            Parameter[1].Value = keyNumber;
            sqlTransList.Add(MkCommandInfo(cmdText, Parameter));
            return sqlTransList;
        }

        private CommandInfo MkCommandInfo(string cmdText, SqlParameter[] paramArray)
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
                case "int":
                    parameter = new SqlParameter("@" + FieldName, SqlDbType.Int);
                    {
                        if (value == "")
                        {
                            parameter.Value = DBNull.Value;
                        }
                        else
                        {
                            parameter.Value = value;
                        }
                    }
                    break;

                case "smallint":
                    parameter = new SqlParameter("@" + FieldName, SqlDbType.SmallInt);
                    {
                        if (value == "")
                        {
                            parameter.Value = DBNull.Value;
                        }
                        else
                        {
                            parameter.Value = value;
                        }
                    }
                    break;
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
                case "text":
                    parameter = new SqlParameter("@" + FieldName, SqlDbType.Text);
                    parameter.Value = value;
                    break;
                case "datetime":
                    parameter = new SqlParameter("@" + FieldName, SqlDbType.DateTime);
                    if (value != "")
                    {
                        parameter.Value = value;
                    }
                    else
                    {
                        parameter.Value = null;
                    }
                    break;
                default:
                    parameter = new SqlParameter("@" + FieldName, SqlDbType.VarChar, length);
                    parameter.Value = value;
                    break;
            }
            return parameter;
        }
    }
}
