using FMOP.DB;
using Hesion.Brick.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_JHJX_New2013_JHJX_SPMM_jhjx_wykfjuk : System.Web.UI.Page
{
    public DataSet ds = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Number"] != null && !Request.QueryString["Number"].ToString().Equals(""))
        {
            string StrNumber = Request.QueryString["Number"].ToString();
            InitPage(StrNumber);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "查询出错！");
        }
        
    }

    private void InitPage( string Number )
    {
        string StrSql = " select   LSCSSJ as 流水产生时间,YSLX as 运算类型,JE as 金额,XM as 项目,XZ as 性质 ,ZY as 摘要,SJLX as 数据类型, Number from AAA_ZKLSMXB where LYDH in(  select Number  from  AAA_THDYFHDXXB where  ZBDBXXBBH = '" + Number + "') and SJLX = '预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息') ";
        ds = DbHelperSQL.Query(StrSql);
    }
}