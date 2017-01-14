using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Configuration;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
using System.Threading;

public partial class Web_JHJX_New2013_JHJX_SPMM_QPDetail : System.Web.UI.Page
{
    static string selleremail = "", buyeremail = "", ZBDBXXBBH = "", zfje = "", zflyzh = "", zfmbzh = ""; 
    protected void Page_Load(object sender, EventArgs e)
    {

        ViewState["Number"] = "";
        ViewState["GoBackUrl"] = "";
        if (Request.QueryString["Number"] != null && Request.QueryString["Number"].ToString() != "")
        {
            ViewState["Number"] = Request.QueryString["Number"].ToString().Trim(); ;
        }
        if (Request.QueryString["GoBackUrl"] != null && Request.QueryString["GoBackUrl"].ToString() != "")
        {
            ViewState["GoBackUrl"] = Request.QueryString["GoBackUrl"].ToString();
        }

        string str = GetSql(ViewState["Number"].ToString().Trim());

        DataTable dt1 = DbHelperSQL.Query(str).Tables[0];
        if (dt1 != null && dt1.Rows.Count > 0)
        {
            DataRow dr = DbHelperSQL.Query(str).Tables[0].Rows[0];

            // DataRow[] drs = this.Context.Items["Text"] as DataRow[];
            if (dr != null)
            {
                InitialData(dr);

            }

        }
      

       
        this.txtPTYJ.ReadOnly = true;
           
      

    }

    protected string GetSql(string num)
    {
        return "SELECT  AAA_ZBDBXXB.Number AS 主键 ,Z_QPZT AS 清盘状态,Z_QPKSSJ AS 清盘开始时间,Z_QPJSSJ AS 清盘结束时间, Z_HTBH AS 电子购货合同编号,Z_HTJSRQ AS 合同结束日期,Z_SPMC AS 商品名称,Z_SPBH AS 商品编号,Z_GG AS 规格,Z_ZBSL AS 合同数量,Z_ZBJG AS 单价,Z_ZBJE AS 合同金额,Z_LYBZJJE AS 履约保证金金额,  CAST( SUM( T_DJHKJE/ZBDJ) AS numeric(18,2)) AS 争议数量 ,SUM(T_DJHKJE) AS  争议金额,'人工清盘' AS 清盘类型,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END AS 清盘原因,T_YSTBDDLYX AS 卖方邮箱,Y_YSYDDDLYX AS 买方邮箱,Q_ZMSCFDLYX AS 证明上传方邮箱,Q_SFYQR AS 是否已确认,Q_ZMWJLJ AS 证明文件路径, Q_ZFLYZH AS 转付来源账户, Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_CLYJ AS 处理依据 ,Q_QRSJ AS 确认时间  FROM AAA_THDYFHDXXB JOIN AAA_ZBDBXXB ON AAA_ZBDBXXB.Number=AAA_THDYFHDXXB.ZBDBXXBBH WHERE   Z_QPZT IN( '清盘中','清盘结束')  AND F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货', '撤销','卖家主动退货')AND Q_SFYQR='是' and AAA_ZBDBXXB.Number='"+num+"' GROUP BY  Z_HTBH,Z_HTJSRQ,Z_SPMC,Z_SPBH,Z_GG,Z_ZBSL,Z_ZBJG,Z_ZBJE,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END,AAA_ZBDBXXB.Number,Z_LYBZJJE,Z_QPZT,T_YSTBDDLYX ,Y_YSYDDDLYX,Z_QPKSSJ,Z_QPJSSJ,Q_ZMSCFDLYX,Q_SFYQR,Q_ZMWJLJ, Q_ZFLYZH , Q_ZFMBZH ,Q_ZFJE ,Q_CLYJ ,Q_QRSJ ";
    }

    //初始化页面数据
    protected void InitialData(DataRow dr)
    {
        divQRRQ.InnerText = dr["确认时间"].ToString().Trim();
        divHTJSRQ.InnerText = dr["合同结束日期"].ToString().Trim();
        divHTBH.InnerText = dr["电子购货合同编号"].ToString().Trim();
        divSPBH.InnerText = dr["商品编号"].ToString().Trim();
        divSPMC.InnerText = dr["商品名称"].ToString().Trim();
        divSPGG.InnerText = dr["规格"].ToString().Trim();
        divQPZT.InnerText = dr["清盘状态"].ToString().Trim();
        divHTSL.InnerText = dr["合同数量"].ToString().Trim();
        divDJ.InnerText = dr["单价"].ToString().Trim();
        divHTJE.InnerText = dr["合同金额"].ToString().Trim();
        divHKDJJE.InnerText = dr["争议金额"].ToString().Trim();
        divLYBZJ.InnerText = dr["履约保证金金额"].ToString().Trim();
        divZYSL.InnerText = dr["争议数量"].ToString().Trim();
        divZYJE.InnerText = dr["争议金额"].ToString().Trim();
        divQPYY.InnerText = dr["清盘原因"].ToString().Trim();
        divQPKSSJ.InnerText = dr["清盘开始时间"].ToString().Trim();
        divQPJSSJ.InnerText = dr["清盘结束时间"].ToString().Trim();
        selleremail = dr["卖方邮箱"].ToString().Trim();
        buyeremail = dr["买方邮箱"].ToString().Trim();
        ZBDBXXBBH = dr["主键"].ToString().Trim();
        zfje = dr["转付金额"].ToString().Trim();
        zflyzh = dr["转付来源账户"].ToString().Trim();
        zfmbzh = dr["转付目标账户"].ToString().Trim();

        divCLYJ.InnerText = dr["处理依据"].ToString().Trim();
        this.lwjck.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dr["证明文件路径"].ToString().Replace(@"\", "/");

        string buyer = DbHelperSQL.GetSingle("SELECT B_YHM FROM AAA_DLZHXXB WHERE B_DLYX='" + dr["买方邮箱"].ToString().Trim() + "'").ToString().Trim();
        string seller = DbHelperSQL.GetSingle("SELECT B_YHM FROM AAA_DLZHXXB WHERE B_DLYX='" + dr["卖方邮箱"].ToString().Trim() + "'").ToString().Trim();
        string scf = DbHelperSQL.GetSingle("SELECT B_YHM FROM AAA_DLZHXXB WHERE B_DLYX='" + dr["证明上传方邮箱"].ToString().Trim() + "'").ToString().Trim();
        txtCLSM.Text = seller + "卖家与" + buyer + "买家双方就" + dr["电子购货合同编号"].ToString().Trim() + "《电子购货合同》存在的所有争议已达成最终处理结果，现由" + scf + "上传证明文件。请贵平台按证明文件规定将" + dr["转付来源账户"].ToString().Trim() + "账户上的" + dr["转付金额"].ToString().Trim() + "元转付给" + dr["转付目标账户"].ToString().Trim() + "账户";

        txtJGQR.Text = seller + "卖家与" + buyer + "买家双方就" + dr["电子购货合同编号"].ToString().Trim() + "《电子购货合同》存在的所有争议已达成最终处理结果，现对" + scf + "上传的证明文件确认是真实的，证明文件中所表述的双方处理结果，是双方真实的意思表示";
        object obj = DbHelperSQL.GetSingle("  select PTCLYJ from AAA_RGQPCLB where ZBDBXXBBH='" + ZBDBXXBBH + "'");
        if (obj != null)
        { txtPTYJ.Text = obj.ToString(); }
    }
    
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(ViewState["GoBackUrl"].ToString());
    }
}