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
/// <summary>
/// CSKHFWZX_FHD 的摘要说明
/// </summary>
public class CSKHFWZX_FHD
{
    public CSKHFWZX_FHD()
    { }
    #region Model
    private string _number;
    private string _ssfgs;
    private string _shr;
    private string _dz;
    private string _lxdh;
    private string _wlgsmc;
    private decimal _wlfyhj;
    private string _tjm3;
    private string _zlkg;
    private decimal _ysdj;
    private decimal _bey;
    private decimal _bfy;
    private string _qt;
    private DateTime _thsj;
    private DateTime _yqddsj;
    private string _thch;
    private string _thr;
    private string _createuser;
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
    public string SSFGS
    {
        set { _ssfgs = value; }
        get { return _ssfgs; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string SHR
    {
        set { _shr = value; }
        get { return _shr; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string DZ
    {
        set { _dz = value; }
        get { return _dz; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string LXDH
    {
        set { _lxdh = value; }
        get { return _lxdh; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string WLGSMC
    {
        set { _wlgsmc = value; }
        get { return _wlgsmc; }
    }
    /// <summary>
    /// 
    /// </summary>
    public decimal WLFYHJ
    {
        set { _wlfyhj = value; }
        get { return _wlfyhj; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string TJM3
    {
        set { _tjm3 = value; }
        get { return _tjm3; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string ZLKG
    {
        set { _zlkg = value; }
        get { return _zlkg; }
    }
    /// <summary>
    /// 
    /// </summary>
    public decimal YSDJ
    {
        set { _ysdj = value; }
        get { return _ysdj; }
    }
    /// <summary>
    /// 
    /// </summary>
    public decimal BEY
    {
        set { _bey = value; }
        get { return _bey; }
    }
    /// <summary>
    /// 
    /// </summary>
    public decimal BFY
    {
        set { _bfy = value; }
        get { return _bfy; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string QT
    {
        set { _qt = value; }
        get { return _qt; }
    }
    /// <summary>
    /// 
    /// </summary>
    public DateTime THSJ
    {
        set { _thsj = value; }
        get { return _thsj; }
    }
    /// <summary>
    /// 
    /// </summary>
    public DateTime YQDDSJ
    {
        set { _yqddsj = value; }
        get { return _yqddsj; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string THCH
    {
        set { _thch = value; }
        get { return _thch; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string THR
    {
        set { _thr = value; }
        get { return _thr; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string CreateUser
    {
        set { _createuser = value; }
        get { return _createuser; }
    }
    #endregion Model

    /// <summary>
    /// 增加一条数据
    /// </summary>
    public void Add()
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("insert into CSKHFWZX_FHD(");
        strSql.Append("Number,SSFGS,SHR,DZ,LXDH,WLGSMC,WLFYHJ,TJM3,ZLKG,YSDJ,BEY,BFY,QT,THSJ,YQDDSJ,THCH,THR,CreateUser)");
        strSql.Append(" values (");
        strSql.Append("@Number,@SSFGS,@SHR,@DZ,@LXDH,@WLGSMC,@WLFYHJ,@TJM3,@ZLKG,@YSDJ,@BEY,@BFY,@QT,@THSJ,@YQDDSJ,@THCH,@THR,@CreateUser)");
        SqlParameter[] parameters = {
					new SqlParameter("@Number", SqlDbType.VarChar,50),
					new SqlParameter("@SSFGS", SqlDbType.VarChar,50),
					new SqlParameter("@SHR", SqlDbType.VarChar,50),
					new SqlParameter("@DZ", SqlDbType.VarChar,50),
					new SqlParameter("@LXDH", SqlDbType.VarChar,50),
					new SqlParameter("@WLGSMC", SqlDbType.VarChar,50),
					new SqlParameter("@WLFYHJ", SqlDbType.Float,8),
					new SqlParameter("@TJM3", SqlDbType.VarChar,50),
					new SqlParameter("@ZLKG", SqlDbType.VarChar,50),
					new SqlParameter("@YSDJ", SqlDbType.Float,8),
					new SqlParameter("@BEY", SqlDbType.Float,8),
					new SqlParameter("@BFY", SqlDbType.Float,8),
					new SqlParameter("@QT", SqlDbType.VarChar,50),
					new SqlParameter("@THSJ", SqlDbType.DateTime),
					new SqlParameter("@YQDDSJ", SqlDbType.DateTime),
					new SqlParameter("@THCH", SqlDbType.VarChar,50),
					new SqlParameter("@THR", SqlDbType.VarChar,50),
					new SqlParameter("@CreateUser", SqlDbType.VarChar,50)};
        parameters[0].Value = Number;
        parameters[1].Value = SSFGS;
        parameters[2].Value = SHR;
        parameters[3].Value = DZ;
        parameters[4].Value = LXDH;
        parameters[5].Value = WLGSMC;
        parameters[6].Value = WLFYHJ;
        parameters[7].Value = TJM3;
        parameters[8].Value = ZLKG;
        parameters[9].Value = YSDJ;
        parameters[10].Value = BEY;
        parameters[11].Value = BFY;
        parameters[12].Value = QT;
        parameters[13].Value = THSJ;
        parameters[14].Value = YQDDSJ;
        parameters[15].Value = THCH;
        parameters[16].Value = THR;
        parameters[17].Value = CreateUser;

        DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
    }
}