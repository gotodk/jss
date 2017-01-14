using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class pingtaiservices_moban_BZH : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["moban"] == null || Request["Number"] == null)
        {
            Response.Write("系统错误");
            Response.End();
            return;
        }
        string moban = Request["moban"].ToString();//模拟模板名称
        string Number = Request["Number"].ToString();//单据编号（中标定标表编号）
        string oldstr = BuildHtm(HttpContext.Current.Server.MapPath(moban));

        string strSQL = " select '卖家名称'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_SELJSBH=b.T_YSTBDMJJSBH),'买家名称'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=b.Y_YSYDDMJJSBH),Z_HTBH 合同编号,convert(varchar(10),Z_DBSJ,120) 定标时间,Z_BZHJE 保证函金额,'保证函编号'='BH'+b.Number from AAA_ZBDBXXB b where b.Number='" + Number.Trim() + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);

        if (ds != null && ds.Tables[0].Rows.Count > 0 )
        {
            string newstr = "";
            //开始替换模板
            DateTime dataTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["定标时间"]);

            newstr = oldstr.Replace("[保证函编号]", ds.Tables[0].Rows[0]["保证函编号"].ToString());//暂时没有保证函编号
            newstr = newstr.Replace("[卖家名称]", ds.Tables[0].Rows[0]["卖家名称"].ToString());
            newstr = newstr.Replace("[买家名称]", ds.Tables[0].Rows[0]["买家名称"].ToString());
            newstr = newstr.Replace("[保证函大写金额]",chang(ds.Tables[0].Rows[0]["保证函金额"].ToString()));
            newstr = newstr.Replace("[保证函小写金额]", ds.Tables[0].Rows[0]["保证函金额"].ToString());
            newstr = newstr.Replace("[电子购货合同编号]", ds.Tables[0].Rows[0]["合同编号"].ToString());
            newstr = newstr.Replace("YYYY", dataTime.Year.ToString());
            newstr = newstr.Replace("MM", dataTime.Month.ToString());
            newstr = newstr.Replace("dd", dataTime.Day.ToString());
            Response.Write(newstr);
            Response.End();
        }
        else
        {
            Response.Write("系统错误");
            Response.End();
            return;
        }

        
    }

    /// <summary>
    ///将小写金额转换为大写金额
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    public string chang(string money)
        {
            //将小写金额转换成大写金额           
            double MyNumber = Convert.ToDouble(money);
            String[] MyScale = { "分", "角", "圆", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "兆", "拾", "佰", "仟" };
            String[] MyBase = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            String M = "";
            bool isPoint = false;
            if (money.IndexOf(".") != -1)
            {
                money = money.Remove(money.IndexOf("."), 1);
                isPoint = true;
            }
            for (int i = money.Length; i > 0; i--)
            {
                int MyData = Convert.ToInt16(money[money.Length - i].ToString());
                M += MyBase[MyData];
                if (isPoint == true)
                {
                    M += MyScale[i - 1];
                }
                else
                {
                    M += MyScale[i + 1];
                }
            }
            return M;
        }
    /// <summary>
    /// 根据模板读取数据库内容，无需创建其他列表，直接创建html
    /// </summary>
    /// <param name="strTmplPath">网页模板文件的路径</param>
    public string BuildHtm(string strTmplPath)
    {
        //取模板文件的内容
        System.Text.Encoding code = System.Text.Encoding.GetEncoding("gb2312");
        StreamReader sr = null;
        string str = "";
        try
        {
            sr = new StreamReader(strTmplPath, code);
            str = sr.ReadToEnd(); // 读取文件
            sr.Close();
        }
        catch (Exception exp)
        {
            return exp.ToString();
            sr.Close();
        }
        //string htmlfilename = this.GetFileSaveName(strEnName);//通过英文名获取保存后的文件名
        //替换变量标签

        //string strNew=str.Replace("[$nameChs$]", strChsName);
        //string str = str;
        //strNew = strNew.Replace("[$areaContect$]", "新的文本");//替换左侧页面导航
        return str;
    }

    public DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("提示文本");
        auto2.Columns.Add("附件信息1");
        auto2.Columns.Add("附件信息2");
        auto2.Columns.Add("附件信息3");
        auto2.Columns.Add("附件信息4");
        auto2.Columns.Add("附件信息5");
        ds.Tables.Add(auto2);
        return ds;
    }



}