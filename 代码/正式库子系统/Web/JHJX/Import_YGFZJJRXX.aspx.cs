using Hesion.Brick.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Hesion.Brick.Core;
using System.Data.OleDb;
using FMOP.DB;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_Import_YGFZJJRXX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;

        if (!Page.IsPostBack)
        {
            btnValidate.Enabled = false;
            btnConvert.Enabled = false;
            btnExport.Enabled = false;
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    { 
        if (FileUpload2.HasFile)
        {
            try
            {
                string fileName = FileUpload2.FileName;
                hidFileName.Value = FileUpload2.FileName;              

                if (fileName.Trim().ToString() == "")
                {
                    MessageBox.Show(this, "请选择要导入的文件！");
                    return;
                }
                if (!fileName.EndsWith(".xls") && !fileName.EndsWith(".xlsx"))
                {
                    MessageBox.Show(this, "请选择后缀为.xls或.xlsx的Excel文件!");
                    return;
                }

                string serverpath = Server.MapPath(@"../JHJX/") + fileName;
                FileUpload2.SaveAs(serverpath);

                //将xls的数据转入dataset中
                string oleDBConnString = GetOleDBConnString(serverpath);
                DataSet dsInput = GetDataSet(oleDBConnString);
                
                //将上传的文件删除
                if (File.Exists(serverpath))
                    File.Delete(serverpath);

                //判断是否包含必须的三列
                if (!dsInput.Tables[0].Columns.Contains("员工工号") || !dsInput.Tables[0].Columns.Contains("员工姓名") || !dsInput.Tables[0].Columns.Contains("发展经纪人邮箱"))
                {
                    MessageBox.Show(this, "Excel格式错误：必须包含员工工号、员工姓名、发展经纪人邮箱三列！");
                    return;
                }
                if (dsInput.Tables[0].Columns[0].ColumnName.ToString() != "员工工号" || dsInput.Tables[0].Columns[1].ColumnName.ToString() != "员工姓名" || dsInput.Tables[0].Columns[2].ColumnName.ToString() != "发展经纪人邮箱")
                {
                    MessageBox.Show(this, "Excel格式错误：列顺序错误！");
                    return;
                }

                //给生成的ds增加列用于后续处理
                dsInput.Tables[0].Columns.Add("发展经纪人交易方名称");
                dsInput.Tables[0].Columns.Add("发展经纪人注册类别");
                dsInput.Tables[0].Columns.Add("数据验证");
                dsInput.Tables[0].Columns.Add("备注");                

                //将获取的数据绑定rpt
                if (dsInput.Tables[0].Rows.Count <= 0)
                {
                    IsConvert.Value = "0";
                    MessageBox.Show(this, "表中不存在任何数据！");
                    return;
                }
                rpt.DataSource = dsInput.Tables[0].DefaultView;
                rpt.DataBind();
                ViewState["dt"] = dsInput.Tables[0];
                IsConvert.Value = "1";
                btnValidate.Enabled = true;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "文件出错!" + ex.ToString());
            }
        }
        else
        {
            MessageBox.Show(this, "请选择文件!");
        }
    }
    /// <summary>
    /// get connection string
    /// </summary>
    /// <param name="fileName">file path</param>
    /// <returns></returns>
    private string GetOleDBConnString(string fileName)
    {
        string oleDBConnString="";
        //if (fileName.EndsWith(".xls"))
        //{
        //    oleDBConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;IMEX=1'";
        //}
        //if (fileName.EndsWith(".xlsx"))
        //{
            oleDBConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;IMEX=1'";
        //}
        return oleDBConnString;
    }
    /// <summary>
    /// 将excel生成dataset
    /// </summary>
    /// <param name="oleDBConnString"></param>
    /// <returns></returns>
    private DataSet GetDataSet(string oleDBConnString)
    {
        OleDbConnection oleDBConn = null;
        OleDbDataAdapter oleAdMaster = null;
        DataTable m_tableName = new DataTable();
        DataSet ds = new DataSet();

        oleDBConn = new OleDbConnection(oleDBConnString);
        oleDBConn.Open();
        m_tableName = oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        if (m_tableName != null && m_tableName.Rows.Count > 0)
        {
            m_tableName.TableName = m_tableName.Rows[0]["TABLE_NAME"].ToString();
        }
        string sqlMaster;
        sqlMaster = " SELECT * FROM [" + m_tableName.TableName + "]";
        oleAdMaster = new OleDbDataAdapter(sqlMaster, oleDBConn);
        oleAdMaster.Fill(ds, "m_tableName");
        oleAdMaster.Dispose();
        oleDBConn.Close();
        oleDBConn.Dispose();        
        return ds;
    }



    //验证数据信息
    protected void btnValidate_Click(object sender, EventArgs e)
    {
        //获取登录注册信息表中的数据，
        DataSet dsZC = DbHelperSQL.Query("select B_DLYX as 邮箱,I_JYFMC as 交易方名称,I_ZCLB as 注册类别,S_SFYBFGSSHTG as 是否审核 from AAA_DLZHXXB where B_JSZHLX='经纪人交易账户'");//不限制是不是业务拓展部

        //获取已经导入的邮箱信息
        DataSet dsYDR = DbHelperSQL.Query("select gljjrdlyx as 邮箱 from AAA_YGFZJJRXXB");

        //获取工号、姓名信息
        DataSet dsYGXX = DbHelperSQL.Query("select number as 工号,employee_name as 姓名 from HR_Employees where ygzt<>'离职'");

        //对当前导入的信息进行数据验证     
        DataTable dt = (DataTable)ViewState["dt"];
        bool CanImport = true;//确定是不是可以执行导入操作

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["数据验证"] = "成功";//数据验证默认值为成功

            if (dt.Rows[i]["员工工号"].ToString().Trim() == "" || dt.Rows[i]["员工姓名"].ToString().Trim() == "" || dt.Rows[i]["发展经纪人邮箱"].ToString().Trim() == "")
            {
                dt.Rows[i]["数据验证"] = "失败";
                dt.Rows[i]["备注"] = "信息不完整";
                CanImport = false;
            }
            else
            {
                //判断工号和姓名是否对应
                if (dt.Rows[i]["员工工号"].ToString().Trim().Length != 7)
                {
                    dt.Rows[i]["数据验证"] = "失败";
                    dt.Rows[i]["备注"] = "工号位数不对。";
                    CanImport = false;
                }
                else
                {
                    string yggh = dt.Rows[i]["员工工号"].ToString().Trim();
                    DataRow[] dr_ygxx = dsYGXX.Tables[0].Select("工号='" + yggh + "'");
                    if (dr_ygxx.Length <= 0)
                    {
                        dt.Rows[i]["数据验证"] = "失败";
                        dt.Rows[i]["备注"] = "工号不存在。";
                        CanImport = false;
                    }
                    else
                    {
                        string ygxm = dr_ygxx[0]["姓名"].ToString().Trim();
                        if (dt.Rows[i]["员工姓名"].ToString().Trim() != ygxm)
                        {
                            dt.Rows[i]["数据验证"] = "失败";
                            dt.Rows[i]["备注"] = "工号与姓名不对应。";
                            CanImport = false;
                        }
                    }
                }

                //验证该邮箱是否已经注册并且通过审核
                string dlyx = dt.Rows[i]["发展经纪人邮箱"].ToString();
                DataRow[] dr_zc = dsZC.Tables[0].Select("邮箱='" + dlyx + "'");
                if (dr_zc.Length <= 0)
                {
                    dt.Rows[i]["数据验证"] = "失败";
                    dt.Rows[i]["备注"] = dt.Rows[i]["备注"] + "经纪人交易账户中不存在该邮箱。";
                    CanImport = false;
                }
                else if (dr_zc[0]["是否审核"].ToString() == "否")
                {
                    dt.Rows[i]["数据验证"] = "失败";
                    dt.Rows[i]["备注"] = dt.Rows[i]["备注"] + "该邮箱尚未通过审核。";
                    CanImport = false;
                }
                else
                {
                    dt.Rows[i]["发展经纪人交易方名称"] = dr_zc[0]["交易方名称"].ToString();
                    dt.Rows[i]["发展经纪人注册类别"] = dr_zc[0]["注册类别"].ToString();
                }

                DataRow[] dr_ydr = dsYDR.Tables[0].Select("邮箱='" + dlyx + "'");
                if (dr_ydr.Length > 0)
                {
                    dt.Rows[i]["数据验证"] = "失败";
                    dt.Rows[i]["备注"] = dt.Rows[i]["备注"] + "该邮箱已导入过";
                    CanImport = false;
                }
            }
        }

        if (CanImport == true)
        {
            btnConvert.Enabled = true;
            btnExport.Enabled = true;
        }
        else
        {
            if (dt.Select("数据验证='成功'").Length <= 0)
            {
                IsConvert.Value = "3";
            }
            else
            {
                btnConvert.Enabled = true;
                btnExport.Enabled = true;
            }
        }

        //验证结束重新绑定一遍页面数据
        rpt.DataSource = dt.DefaultView ;
        rpt.DataBind();
        ViewState["dt"] = dt;
    }

    //将页面显示的数据导入数据库
    protected void btnConvert_Click(object sender, EventArgs e)
    {
        //信息验证通过之后，生成插入数据库的sql语句并插入
        ArrayList al = new ArrayList();
        DataTable dt = (DataTable)ViewState["dt"];
        string time = DateTime.Now.ToString();
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["数据验证"].ToString() == "成功")
            {
                //生成表的number
                WorkFlowModule WFM = new WorkFlowModule("AAA_YGFZJJRXXB");
                string KeyNumber = WFM.numberFormat.GetNextNumber();

                string sql_insert = "INSERT INTO [AAA_YGFZJJRXXB]([Number],[YGGH],[YGXM],[GLJJRJYFMC],[GLJJRDLYX],[GLJJRZCLB],[SFYX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES('" + KeyNumber + "','" + dr["员工工号"].ToString().Trim () + "','" + dr["员工姓名"].ToString().Trim () + "','" + dr["发展经纪人交易方名称"].ToString() + "','" + dr["发展经纪人邮箱"].ToString().Trim () + "','" + dr["发展经纪人注册类别"].ToString() + "','是',1,'" + User.Identity.Name.ToString() + "','" + time + "','" + time + "')";

                al.Add(sql_insert);
            }
        }
        try
        {
            DbHelperSQL.ExecSqlTran(al);
            MessageBox.ShowAndRedirect(this, "数据验证成功的部分，导入成功！", "Import_YGFZJJRXX.aspx");
        }
        catch
        {
            MessageBox.Show(this, "数据验证成功的部分，导入失败！");
            return;
        }
    }
    protected void btnShuaXin_Click(object sender, EventArgs e)
    {
        Response.Redirect("Import_YGFZJJRXX.aspx");
    }
    protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblJJRJYFMC = (Label)e.Item.FindControl("lblJJRJYFMC");
        if (lblJJRJYFMC.Text.Length > 10)
            lblJJRJYFMC.Text = lblJJRJYFMC.Text.Substring(0, 10) + "...";
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dt"];
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(ds, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "员工发展经纪人信息验证结果", "员工发展经纪人信息验证结果", 25);
    }
}