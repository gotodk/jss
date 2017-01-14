using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using FMOP.DB;
using FMOP.FILE;
using System.Text;

public partial class Web_HR_zp_show : System.Web.UI.Page
{
    public DataSet result;
    public string picture;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["number"]))
                {
                   string number = Request["number"].ToString();
                   string module = "HR_Employees";
                    showFile(number, module);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// 显示上传的文件
    /// </summary>
    private void showFile(string number, string module)
    {
        string localName = string.Empty;
        string sourceName = string.Empty;
        string id = string.Empty;
        StringBuilder strTable = new StringBuilder();        
        string SqlCmd = " SELECT ID,SourceName,LocalName FROM FileUp where Number='" + number + "' and ModuleName='" + module + "'";
        result = DbHelperSQL.Query(SqlCmd);      
       
    }    
}
