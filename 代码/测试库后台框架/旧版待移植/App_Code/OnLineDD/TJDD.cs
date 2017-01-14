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
using System.Text;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;
using FMOP.DB;

/// <summary>
/// TJDD 的摘要说明
/// </summary>
namespace FMOP.OnLineDD
{
    public class TJDD
    {
        public TJDD()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private string _number;
        private string _xm;
        private string _gddh;
        private string _yddh;
        private string _dzyj;
        private string _dgmm;
        private string _qrmm;
        private string _shrxm;
        private string _gddh1;
        private string _yddh1;
        private string _dzyj1;
        private string _yzbm;
        private string _province;
        private string _city;
        private string _borough;
        private string _xxdz;
        private string _pssdzysx;
        private string _fkfs;
        private string _jsfs;
        private string _jsydje;
        private string _sfkjfp;
        private string _fplx;
        private decimal _hj;
        private string _kpxx;
        private string _nextchecker;
        private int _checkstate;
        private string _createuser;
        private string _createtime;
        private int _state;
        private string _dhsj;
    	private string _shagnwu;
        /// <summary>
        /// 
        /// </summary>
        public string Number
        {
            set { _number = value; }
            get { return _number; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string XM
        {
            set { _xm = value; }
            get { return _xm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GDDH
        {
            set { _gddh = value; }
            get { return _gddh; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YDDH
        {
            set { _yddh = value; }
            get { return _yddh; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DZYJ
        {
            set { _dzyj = value; }
            get { return _dzyj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DGMM
        {
            set { _dgmm = value; }
            get { return _dgmm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QRMM
        {
            set { _qrmm = value; }
            get { return _qrmm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SHRXM
        {
            set { _shrxm = value; }
            get { return _shrxm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GDDH1
        {
            set { _gddh1 = value; }
            get { return _gddh1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YDDH1
        {
            set { _yddh1 = value; }
            get { return _yddh1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DZYJ1
        {
            set { _dzyj1 = value; }
            get { return _dzyj1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YZBM
        {
            set { _yzbm = value; }
            get { return _yzbm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Province
        {
            set { _province = value; }
            get { return _province; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Borough
        {
            set { _borough = value; }
            get { return _borough; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string XXDZ
        {
            set { _xxdz = value; }
            get { return _xxdz; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PSSDZYSX
        {
            set { _pssdzysx = value; }
            get { return _pssdzysx; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FKFS
        {
            set { _fkfs = value; }
            get { return _fkfs; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string JSFS
        {
            set { _jsfs = value; }
            get { return _jsfs; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string JSYDJE
        {
            set { _jsydje = value; }
            get { return _jsydje; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SFKJFP
        {
            set { _sfkjfp = value; }
            get { return _sfkjfp; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FPLX
        {
            set { _fplx = value; }
            get { return _fplx; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal HJ
        {
            set { _hj = value; }
            get { return _hj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string KPXX
        {
            set { _kpxx = value; }
            get { return _kpxx; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NextChecker
        {
            set { _nextchecker = value; }
            get { return _nextchecker; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CheckState
        {
            set { _checkstate = value; }
            get { return _checkstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUser
        {
            set { _createuser = value; }
            get { return _createuser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }

        public string Dhsj
        {
            set
            {
                _dhsj = value;
            }
            get
            {
                return _dhsj;
            }
        }

    	public string ShangWu
    	{
			get { return _shagnwu; }
			set { _shagnwu = value; }
    	}

        /// <summary>
        /// 获取单号
        /// </summary>
        /// <returns></returns>
        public string GetNumber()
        {
            long No = 0;
            string year = System.DateTime.Now.ToString("yy");
            string month = System.DateTime.Now.ToString("MM");

            _number = DbHelperSQL.GetMaxNumber("Number", "OnLineOrder_bak");
            No = Convert.ToInt64(_number.Substring(4, 10));
            No = No + 1;
            _number = year + month + No.ToString("d10");
           return _number;
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public CommandInfo Add()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OnLineOrder_bak(");
			strSql.Append("Number,XM,GDDH,YDDH,DZYJ,DGMM,QRMM,SHRXM,GDDH1,YDDH1,DZYJ1,YZBM,Province,City,Borough,XXDZ,PSSDZYSX,FKFS,JSFS,JSYDJE,SFKJFP,FPLX,HJ,KPXX,NextChecker,CheckState,CreateUser,CreateTime,isHY,State,DHSJ,shangwu)");
            strSql.Append(" values (");
			strSql.Append("@Number,@XM,@GDDH,@YDDH,@DZYJ,@DGMM,@QRMM,@SHRXM,@GDDH1,@YDDH1,@DZYJ1,@YZBM,@Province,@City,@Borough,@XXDZ,@PSSDZYSX,@FKFS,@JSFS,@JSYDJE,@SFKJFP,@FPLX,@HJ,@KPXX,@NextChecker,@CheckState,@CreateUser,@CreateTime,@isHY,@State,@DHSJ,@shangwu)");
            SqlParameter[] parameters = {
					new SqlParameter("@Number", SqlDbType.VarChar,50),
					new SqlParameter("@XM", SqlDbType.VarChar,50),
					new SqlParameter("@GDDH", SqlDbType.VarChar,50),
					new SqlParameter("@YDDH", SqlDbType.VarChar,50),
					new SqlParameter("@DZYJ", SqlDbType.VarChar,100),
					new SqlParameter("@DGMM", SqlDbType.VarChar,50),
					new SqlParameter("@QRMM", SqlDbType.VarChar,50),
					new SqlParameter("@SHRXM", SqlDbType.VarChar,50),
					new SqlParameter("@GDDH1", SqlDbType.VarChar,50),
					new SqlParameter("@YDDH1", SqlDbType.VarChar,50),
					new SqlParameter("@DZYJ1", SqlDbType.VarChar,50),
					new SqlParameter("@YZBM", SqlDbType.VarChar,50),
					new SqlParameter("@Province", SqlDbType.VarChar,50),
					new SqlParameter("@City", SqlDbType.VarChar,50),
					new SqlParameter("@Borough", SqlDbType.VarChar,50),
					new SqlParameter("@XXDZ", SqlDbType.Text),
					new SqlParameter("@PSSDZYSX", SqlDbType.Text),
					new SqlParameter("@FKFS", SqlDbType.VarChar,50),
					new SqlParameter("@JSFS", SqlDbType.VarChar,50),
					new SqlParameter("@JSYDJE", SqlDbType.VarChar,50),
					new SqlParameter("@SFKJFP", SqlDbType.VarChar,50),
					new SqlParameter("@FPLX", SqlDbType.VarChar,50),
					new SqlParameter("@HJ", SqlDbType.Float,8),
					new SqlParameter("@KPXX", SqlDbType.Text),
					new SqlParameter("@NextChecker", SqlDbType.VarChar,50),
					new SqlParameter("@CheckState", SqlDbType.SmallInt,2),
					new SqlParameter("@CreateUser", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@isHY", SqlDbType.SmallInt,2),
					new SqlParameter("@State", SqlDbType.SmallInt,2),
                    new SqlParameter("@DHSJ", SqlDbType.DateTime),
					new SqlParameter("@shangwu", SqlDbType.VarChar,10)
                                        };
            parameters[0].Value = Number;
            parameters[1].Value = XM;
            parameters[2].Value = GDDH;
            parameters[3].Value = YDDH;
            parameters[4].Value = DZYJ;
            parameters[5].Value = DGMM;
            parameters[6].Value = QRMM;
            parameters[7].Value = SHRXM;
            parameters[8].Value = GDDH1;
            parameters[9].Value = YDDH1;
            parameters[10].Value = DZYJ1;
            parameters[11].Value = YZBM;
            parameters[12].Value = Province;
            parameters[13].Value = City;
            parameters[14].Value = Borough;
            parameters[15].Value = XXDZ;
            parameters[16].Value = PSSDZYSX;
            parameters[17].Value = FKFS;
            parameters[18].Value = JSFS;
            parameters[19].Value = JSYDJE;
            parameters[20].Value = SFKJFP;
            parameters[21].Value = FPLX;
            parameters[22].Value = HJ;
            parameters[23].Value = KPXX;
            parameters[24].Value = "";
            parameters[25].Value = 0;
            parameters[26].Value = CreateUser;
            parameters[27].Value = System.DateTime.Now;
            parameters[28].Value = 0;
			parameters[29].Value = 0;
            parameters[30].Value = Convert.ToDateTime(Dhsj);
        	parameters[31].Value = ShangWu;
            return MkCommandInfo(strSql.ToString(), parameters);
        }


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public CommandInfo Add2()
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("insert into OnLineOrder_bak(");
			strSql.Append("Number,XM,GDDH,YDDH,DZYJ,DGMM,QRMM,SHRXM,GDDH1,YDDH1,DZYJ1,YZBM,Province,City,Borough,XXDZ,PSSDZYSX,FKFS,JSFS,JSYDJE,SFKJFP,FPLX,HJ,KPXX,NextChecker,CheckState,CreateUser,CreateTime,isHY,State,DHSJ,shangwu)");
			strSql.Append(" values (");
			strSql.Append("@Number,@XM,@GDDH,@YDDH,@DZYJ,@DGMM,@QRMM,@SHRXM,@GDDH1,@YDDH1,@DZYJ1,@YZBM,@Province,@City,@Borough,@XXDZ,@PSSDZYSX,@FKFS,@JSFS,@JSYDJE,@SFKJFP,@FPLX,@HJ,@KPXX,@NextChecker,@CheckState,@CreateUser,@CreateTime,@isHY,@State,@DHSJ,@shangwu)");
			SqlParameter[] parameters = {
					new SqlParameter("@Number", SqlDbType.VarChar,50),
					new SqlParameter("@XM", SqlDbType.VarChar,50),
					new SqlParameter("@GDDH", SqlDbType.VarChar,50),
					new SqlParameter("@YDDH", SqlDbType.VarChar,50),
					new SqlParameter("@DZYJ", SqlDbType.VarChar,100),
					new SqlParameter("@DGMM", SqlDbType.VarChar,50),
					new SqlParameter("@QRMM", SqlDbType.VarChar,50),
					new SqlParameter("@SHRXM", SqlDbType.VarChar,50),
					new SqlParameter("@GDDH1", SqlDbType.VarChar,50),
					new SqlParameter("@YDDH1", SqlDbType.VarChar,50),
					new SqlParameter("@DZYJ1", SqlDbType.VarChar,50),
					new SqlParameter("@YZBM", SqlDbType.VarChar,50),
					new SqlParameter("@Province", SqlDbType.VarChar,50),
					new SqlParameter("@City", SqlDbType.VarChar,50),
					new SqlParameter("@Borough", SqlDbType.VarChar,50),
					new SqlParameter("@XXDZ", SqlDbType.Text),
					new SqlParameter("@PSSDZYSX", SqlDbType.Text),
					new SqlParameter("@FKFS", SqlDbType.VarChar,50),
					new SqlParameter("@JSFS", SqlDbType.VarChar,50),
					new SqlParameter("@JSYDJE", SqlDbType.VarChar,50),
					new SqlParameter("@SFKJFP", SqlDbType.VarChar,50),
					new SqlParameter("@FPLX", SqlDbType.VarChar,50),
					new SqlParameter("@HJ", SqlDbType.Float,8),
					new SqlParameter("@KPXX", SqlDbType.Text),
					new SqlParameter("@NextChecker", SqlDbType.VarChar,50),
					new SqlParameter("@CheckState", SqlDbType.SmallInt,2),
					new SqlParameter("@CreateUser", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@isHY", SqlDbType.SmallInt,2),
					new SqlParameter("@State", SqlDbType.SmallInt,2),
					new SqlParameter("@DHSJ", SqlDbType.DateTime),
					new SqlParameter("@shangwu", SqlDbType.VarChar,10)
										};
			parameters[0].Value = Number;
			parameters[1].Value = XM;
			parameters[2].Value = GDDH;
			parameters[3].Value = YDDH;
			parameters[4].Value = DZYJ;
			parameters[5].Value = DGMM;
			parameters[6].Value = QRMM;
			parameters[7].Value = SHRXM;
			parameters[8].Value = GDDH1;
			parameters[9].Value = YDDH1;
			parameters[10].Value = DZYJ1;
			parameters[11].Value = YZBM;
			parameters[12].Value = Province;
			parameters[13].Value = City;
			parameters[14].Value = Borough;
			parameters[15].Value = XXDZ;
			parameters[16].Value = PSSDZYSX;
			parameters[17].Value = FKFS;
			parameters[18].Value = JSFS;
			parameters[19].Value = JSYDJE;
			parameters[20].Value = SFKJFP;
			parameters[21].Value = FPLX;
			parameters[22].Value = HJ;
			parameters[23].Value = KPXX;
			parameters[24].Value = "";
			parameters[25].Value = 0;
			parameters[26].Value = CreateUser;
			parameters[27].Value = System.DateTime.Now;
			parameters[28].Value = 1;
			parameters[29].Value = 0;
			parameters[30].Value = Convert.ToDateTime(Dhsj);
			parameters[31].Value = ShangWu;
			return MkCommandInfo(strSql.ToString(), parameters);
		}


        private CommandInfo MkCommandInfo(string sql, SqlParameter[] parameter)
        {
            CommandInfo obj = new CommandInfo();
            obj.CommandText = sql;
            obj.Parameters = parameter;
            return obj;
        }
    }
}
