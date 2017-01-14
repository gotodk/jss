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
/// CSKHFWZX_FHD_FHCPLB 的摘要说明
/// </summary>
public class CSKHFWZX_FHD_FHCPLB
{
	public CSKHFWZX_FHD_FHCPLB()
	{
	}
    #region Model
    private string _parentnumber;
    private string _yhsqdh;
    private string _xsdh;
    private string _cpbm;
    private string _cppm;
    private string _cpxh;
    private string _dw;
    private string _fhsl;
    private string _xssl;
    private string _bzdw;
    private string _bzsl;
    private string _bz;
    private int _type;
    /// <summary>
    /// 
    /// </summary>
    public string parentNumber
    {
        set { _parentnumber = value; }
        get { return _parentnumber; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string YHSQDH
    {
        set { _yhsqdh = value; }
        get { return _yhsqdh; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string XSDH
    {
        set { _xsdh = value; }
        get { return _xsdh; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string CPBM
    {
        set { _cpbm = value; }
        get { return _cpbm; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string CPPM
    {
        set { _cppm = value; }
        get { return _cppm; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string CPXH
    {
        set { _cpxh = value; }
        get { return _cpxh; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string DW
    {
        set { _dw = value; }
        get { return _dw; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string FHSL
    {
        set { _fhsl = value; }
        get { return _fhsl; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string XSSL
    {
        set { _xssl = value; }
        get { return _xssl; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string BZDW
    {
        set { _bzdw = value; }
        get { return _bzdw; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string BZSL
    {
        set { _bzsl = value; }
        get { return _bzsl; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string BZ
    {
        set { _bz = value; }
        get { return _bz; }
    }

    /// <summary>
    /// 
    /// </summary>
    public int TYPE
    {
        set { _type = value; }
        get { return _type; }
    }
    #endregion Model

    /// <summary>
    /// 增加一条数据
    /// </summary>
    public int Add()
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("insert into CSKHFWZX_FHD_FHCPLB(");
        strSql.Append("parentNumber,YHSQDH,XSDH,CPBM,CPPM,CPXH,DW,FHSL,XSSL,BZDW,BZSL,BZ,TYPE)");
        strSql.Append(" values (");
        strSql.Append("@parentNumber,@YHSQDH,@XSDH,@CPBM,@CPPM,@CPXH,@DW,@FHSL,@XSSL,@BZDW,@BZSL,@BZ,@TYPE)");
        strSql.Append(";select @@IDENTITY");
        SqlParameter[] parameters = {
					new SqlParameter("@parentNumber", SqlDbType.VarChar,50),
					new SqlParameter("@YHSQDH", SqlDbType.VarChar,50),
					new SqlParameter("@XSDH", SqlDbType.VarChar,50),
					new SqlParameter("@CPBM", SqlDbType.VarChar,50),
					new SqlParameter("@CPPM", SqlDbType.VarChar,50),
					new SqlParameter("@CPXH", SqlDbType.VarChar,50),
					new SqlParameter("@DW", SqlDbType.VarChar,50),
					new SqlParameter("@FHSL", SqlDbType.VarChar,50),
					new SqlParameter("@XSSL", SqlDbType.VarChar,50),
					new SqlParameter("@BZDW", SqlDbType.VarChar,50),
					new SqlParameter("@BZSL", SqlDbType.VarChar,50),
					new SqlParameter("@BZ", SqlDbType.VarChar,50),
                    new SqlParameter("@TYPE", SqlDbType.VarChar,50)
        };
        parameters[0].Value = parentNumber;
        parameters[1].Value = YHSQDH;
        parameters[2].Value = XSDH;
        parameters[3].Value = CPBM;
        parameters[4].Value = CPPM;
        parameters[5].Value = CPXH;
        parameters[6].Value = DW;
        parameters[7].Value = FHSL;
        parameters[8].Value = XSSL;
        parameters[9].Value = BZDW;
        parameters[10].Value = BZSL;
        parameters[11].Value = BZ;
        parameters[12].Value = TYPE;

        object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
        if (obj == null)
        {
            return 1;
        }
        else
        {
            return Convert.ToInt32(obj);
        }
    }
}
