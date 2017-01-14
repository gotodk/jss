using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.FILE;
using System.Text;


public partial class Web_HR_Employees_List_Rusult : System.Web.UI.Page
{
    public string number;
    public string xm;
    public string ls;
    public string bm;
    public string ejbmz;
    public string gwmc;
    public string xl;
    public string zy;
    public string byyx;
    public string xb;
    public string mz;
    public string sfzh;
    public string jg;
    public string xx;
    public string csny;
    public DateTime csny1;
    public string zzmm;
    public string dyrdsj;
    public string xzz;
    public string jtdz;
    public string jjlxrybrgx;
    public string hkszd;
    public string hyzk;
    public string syzk;
    public string grdh;
    public string jtdh;
    public string jjlxr;
    public string jjlxrdh;
    public string wyyz;
    public string wysp;
    public string jsjsp;
    public string zc;
    public string zs;
    public string rzqpxkssj;
    public string rzqpxjssj;
    public string kcqkssj;
    public string kcqjssj;
    public string ldhtkssj;
    public string ldhtdqsj;
    public string ldhtqx;
    public string rzsj;
    public string zzsj;
    public string lzsj;
    public string zp;
    public DataSet znds;
    public DataSet rzzgds;
    public DataSet jyjlds;
    public DataSet gzjlds;
    public DataSet pxjlds;
    public DataSet jtzkds;
    public DataSet tgglds;
    public DataSet jfglds;
    public DataSet htjlds;
    public string picture;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["Number"] != null && Request["Number"] != "")
            {
                //string sql = "select * from hr_employees where Number ='" + Request["Number"] + "'";
                //2009-6-17修改，使日期只显示年月日
                StringBuilder sql1 = new StringBuilder();
                sql1.Append("SELECT Number,Employee_Name,LS,BM,EJBMZ,GWMC,XL1,ZY1,BYYX1,Employee_Sex,Employee_EthnicGroup,Employee_IDCardNo,JG,XX,convert(varchar(10),Employee_BirthDay,120) as Employee_BirthDay,");
                sql1.Append("Employee_Party, DYRDSJ,XZZ,JTZZ,SXRYBRGX,HKSZD,HYZK,SYZKNXTX,GRDH,JTDH,JJLXR,JJLXRDH,WYYZ, WYSP, JSJSP,ZC,ZS,");
                sql1.Append("convert(varchar(10),RZQPXQBDSJ,120) as RZQPXQBDSJ,convert(varchar(10),RZQPXQBDSJZ,120) as  RZQPXQBDSJZ, convert(varchar(10),KCQKSSJ,120) as KCQKSSJ,");
                sql1.Append("convert(varchar(10),KCQKSSJZ,120) as KCQKSSJZ,convert(varchar(10),RZSJ,120) as  RZSJ, convert(varchar(10),ZZSJ,120) as ZZSJ,convert(varchar(10),LZSJ,120) as LZSJ,convert(varchar(10),BHTKSSJ,120) as BHTKSSJ,convert(varchar(10),BHTDQSJ,120) as BHTDQSJ,BHTQX ");    
                sql1.Append(" FROM  HR_Employees where Number ='" + Request["Number"] + "'");
                string sql = sql1.ToString();

                DataSet ds = DbHelperSQL.Query(sql);
                number = ds.Tables[0].Rows[0]["Number"].ToString();
                xm = ds.Tables[0].Rows[0]["Employee_Name"].ToString();
                ls = ds.Tables[0].Rows[0]["LS"].ToString();
                bm = ds.Tables[0].Rows[0]["BM"].ToString();
                ejbmz = ds.Tables[0].Rows[0]["EJBMZ"].ToString();
                gwmc = ds.Tables[0].Rows[0]["GWMC"].ToString();
                xl = ds.Tables[0].Rows[0]["XL1"].ToString();
                zy = ds.Tables[0].Rows[0]["ZY1"].ToString();
                byyx = ds.Tables[0].Rows[0]["BYYX1"].ToString();
                xb = ds.Tables[0].Rows[0]["Employee_Sex"].ToString();
                mz = ds.Tables[0].Rows[0]["Employee_EthnicGroup"].ToString();
                sfzh = ds.Tables[0].Rows[0]["Employee_IDCardNo"].ToString();
                jg = ds.Tables[0].Rows[0]["JG"].ToString();
                xx = ds.Tables[0].Rows[0]["XX"].ToString();
                csny = ds.Tables[0].Rows[0]["Employee_BirthDay"].ToString();
                zzmm = ds.Tables[0].Rows[0]["Employee_Party"].ToString();
                dyrdsj = ds.Tables[0].Rows[0]["DYRDSJ"].ToString();
                xzz = ds.Tables[0].Rows[0]["XZZ"].ToString();
                jtdz = ds.Tables[0].Rows[0]["JTZZ"].ToString();
                jjlxrybrgx = ds.Tables[0].Rows[0]["SXRYBRGX"].ToString();//新增
                hkszd = ds.Tables[0].Rows[0]["HKSZD"].ToString();
                hyzk = ds.Tables[0].Rows[0]["HYZK"].ToString();
                syzk = ds.Tables[0].Rows[0]["SYZKNXTX"].ToString();
                grdh = ds.Tables[0].Rows[0]["GRDH"].ToString();
                jtdh = ds.Tables[0].Rows[0]["JTDH"].ToString();
                jjlxr = ds.Tables[0].Rows[0]["JJLXR"].ToString();
                jjlxrdh = ds.Tables[0].Rows[0]["JJLXRDH"].ToString();
                wyyz = ds.Tables[0].Rows[0]["WYYZ"].ToString();
                wysp = ds.Tables[0].Rows[0]["WYSP"].ToString();
                jsjsp = ds.Tables[0].Rows[0]["JSJSP"].ToString();
                zc = ds.Tables[0].Rows[0]["ZC"].ToString();
                zs = ds.Tables[0].Rows[0]["ZS"].ToString();      
                rzqpxkssj = ds.Tables[0].Rows[0]["RZQPXQBDSJ"].ToString();
                rzqpxjssj = ds.Tables[0].Rows[0]["RZQPXQBDSJZ"].ToString();
                kcqkssj = ds.Tables[0].Rows[0]["KCQKSSJ"].ToString();
                kcqjssj = ds.Tables[0].Rows[0]["KCQKSSJZ"].ToString();
                rzsj = ds.Tables[0].Rows[0]["RZSJ"].ToString();
                zzsj = ds.Tables[0].Rows[0]["ZZSJ"].ToString();
                lzsj = ds.Tables[0].Rows[0]["LZSJ"].ToString();
                ldhtkssj = ds.Tables[0].Rows[0]["BHTKSSJ"].ToString();
                ldhtdqsj = ds.Tables[0].Rows[0]["BHTDQSJ"].ToString();
                ldhtqx = ds.Tables[0].Rows[0]["BHTQX"].ToString();
                zndsset(Request["Number"]);
                rzdsset(Request["Number"]);
                jydsset(Request["Number"]);
                gzdsset(Request["Number"]);
                pxdsset(Request["Number"]);
                jtdsset(Request["Number"]);
                tgdsset(Request["Number"]);
                jfdsset(Request["Number"]);
                htdsset(Request["Number"]);
               
            }
        }
        //zp_show.aspx
        picture = "../upload/" + getpicture(Request["Number"]).ToString();
        zp = "<a href=zp_show.aspx?number=" + Request["Number"] + " target=_blank><img src=" + picture + "  width=110 height=125 /></a>";
        //Image1.ImageUrl = "xxx.aspx?number=xxx" + picture.ToString();
       
    }
    /// <summary>
    /// 取得子女状况
    /// </summary>
    /// <param name="prNunber">员工编号</param>
    public void zndsset(string prNunber)
    {
        znds = DbHelperSQL.Query("select EmployeeZN_Sex,convert(varchar(10),EmployeeZN_BirthDay,120) as EmployeeZN_BirthDay from HR_Employees_ZNZK where parentNumber ='" + prNunber + "'");
    }
    /// <summary>
    /// 取得任职资格证书名称
    /// </summary>
    /// <param name="prNunber"></param>
    public void rzdsset(string prNunber)
    {
        string sql = "select MC,convert(varchar(10),YXQQ,120) as YXQQ,convert(varchar(10),YXQZ,120) as YXQZ from HR_Employees_RZZGZS where parentNumber ='" + prNunber + "'";
        rzzgds = DbHelperSQL.Query(sql);
    }
    /// <summary>
    /// 取得教育经历
    /// </summary>
    /// <param name="HYBH">会员编号</param>
    public void jydsset(string HYBH)
    {
        string sql =" SELECT     m.QZSJ, m.YXMC, m.ZY, m.XL,m.XW, m.JYLX FROM   YGGXGL_JYJLB AS z INNER JOIN YGGXGL_JYJLB_JYJL AS m ON z.Number = m.parentNumber WHERE z.YGBH='" +HYBH + "'";
        jyjlds = DbHelperSQL.Query(sql);
    }
    /// <summary>
    /// 取得工作经历
    /// </summary>
    /// <param name="HYBH">会员编号</param>
    public void gzdsset(string HYBH)
    {
        string sql = " SELECT     m.DWMC, m.QZSJ, m.ZMR, m.ZW, m.LXDH, m.LZYY FROM  YGGXGL_GZJLB AS z INNER JOIN YGGXGL_GZJLB_GZJL AS m ON z.Number = m.parentNumber WHERE z.YGBH='" + HYBH + "'";
        gzjlds = DbHelperSQL.Query(sql);
    }
    /// <summary>
    /// 取得培训经历
    /// </summary>
    /// <param name="HYBH">会员编号</param>
    public void pxdsset(string HYBH)
    {
        string sql = " SELECT     m.PXSJ, m.PXXM, m.PXKC, m.PXFS FROM         YGGXGL_PXJLB AS z INNER JOIN YGGXGL_PXJLB_PXJL AS m ON z.Number = m.parentNumber WHERE z.YGBH='" + HYBH + "'";
        pxjlds = DbHelperSQL.Query(sql);
    }
    /// <summary>
    /// 取得家庭情况
    /// </summary>
    /// <param name="HYBH">会员编号</param>
    public void jtdsset(string HYBH)
    {
        string sql = " SELECT     m.GX, m.XM, m.LXDH, m.GZDW, m.GZGW FROM         YGGXGL_JTQKB AS z INNER JOIN  YGGXGL_JTQKB_JTQK AS m ON z.Number = m.parentNumber WHERE z.YGBH='" + HYBH + "'";
        jtzkds = DbHelperSQL.Query(sql);
    }
      /// <summary>
    /// 取得调岗管理
    /// </summary>
    /// <param name="HYBH">会员编号</param>
    public void tgdsset(string HYBH)
    {
        string sql = " SELECT  XM, YGW, TGRZZT, TWGW,convert(varchar(10),DGSJ,120) as DGSJ, TGLB,BZ FROM  RLZY_YGGX_TGGL WHERE YGBH='" + HYBH + "'";
        tgglds = DbHelperSQL.Query(sql);
    }
    /// <summary>
    /// 取得奖罚管理
    /// </summary>
    /// <param name="HYBH">会员编号</param>
    public void jfdsset(string HYBH)
    {
        string sql = " SELECT     SJ, JCSY, JCFS FROM  RLZY_YGGX_JCJLGL WHERE YGBH='" + HYBH + "'";
        jfglds = DbHelperSQL.Query(sql);
    }
    /// <summary>
    /// 取得合同记录2009-6-16
    /// </summary>
    /// <param name="HYBH">会员编号</param>
    public void htdsset(string HYBH)
    {
        string sql = " SELECT  convert(varchar(10),HTKSSJ,120) as HTKSSJ, convert(varchar(10),HTDQSJ,120) as HTDQSJ, LDHTQX,BZ  FROM  RLZY_YGGX_HTGL WHERE YGBH='" + HYBH + "'";
        htjlds = DbHelperSQL.Query(sql);
    }
    public string getpicture(string number)
    {
        string localName = string.Empty;
        string sourceName = string.Empty;
        string id = string.Empty;        
        DataSet result = null;
        string SqlCmd = " SELECT ID,SourceName,LocalName FROM FileUp where ModuleName='HR_Employees' and Number='" + number + "'";
        result = DbHelperSQL.Query(SqlCmd);
        if (result != null && result.Tables[0] != null && result.Tables[0].Rows.Count > 0)
        {
            localName = result.Tables[0].Rows[0]["LocalName"].ToString();
        }
        else
            localName = "thumberror.gif";
        return localName;
    }
   
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Employees_List.aspx");
    }
}

