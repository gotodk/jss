using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.DB;
using FMOP.FILE;
using System.Text;
public partial class UpFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string number = string.Empty;
        string module = string.Empty;
        string id = string.Empty;
        string localeName = string.Empty;
        OperatingFile.upPath = Server.MapPath(Request.ApplicationPath + @"/Web/UpLoad/"); ;
        //判断是否可以接受到模块和主键单号
        if (!Page.IsPostBack)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["number"]) && !string.IsNullOrEmpty(Request["module"]))
                {
                    number = Request["number"].ToString();
                    module = Request["module"].ToString();
                    if (isDelete())
                    {
                        //执行删除操作
                        id = Request["id"].ToString();
                        localeName = Request["localName"].ToString();
                        if (delete(id))
                        {
                            OperatingFile.upPath = Server.MapPath(Request.ApplicationPath + @"/Web/UpLoad/");
                            OperatingFile.DeleteFile(localeName);
                            showFile(number,module);
                        }
                    }
                    else
                    {
                        number = Request["number"].ToString();
                        module = Request["module"].ToString();
                        showFile(number, module);
                    }
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
    private void showFile(string number,string module)
    {
        string localName = string.Empty;
        string sourceName = string.Empty;
        string id = string.Empty;
        StringBuilder strTable = new StringBuilder();
        DataSet result = null;
        //string SqlCmd = " SELECT ID,SourceName,LocalName FROM FileUp where Number='" + number + "'";
        string SqlCmd = " SELECT ID,SourceName,LocalName FROM FileUp where Number='" + number + "' and ModuleName='" + module + "'";
        result = DbHelperSQL.Query(SqlCmd);
        if (result != null && result.Tables[0] != null && result.Tables[0].Rows.Count > 0)
        {
            strTable.Append("已上传文件:&nbsp;");
        }

        foreach (DataRow rowIndex in result.Tables[0].Rows)
        {
            localName = rowIndex["LocalName"].ToString();
            strTable.Append(rowIndex["SourceName"].ToString());
            id = rowIndex["ID"].ToString();
            strTable.Append("&nbsp;&nbsp;"+"<a href =UpFile.aspx?localName=" + localName  + "&id=" + id + "&number=" + number+"&module=" + module+">删除</a>&nbsp;&nbsp;&nbsp;");
        }
        this.divFile.InnerHtml = strTable.ToString();
    }

    /// <summary>
    /// 从数据库中删除上传文件
    /// </summary>
    private bool delete(string ID)
    {
        //删除数据库中信息
        int rowAffect = 0;
        try
        {
            string delCmd = " DELETE FROM FileUp WHERE ID='" + ID + "'";
            rowAffect = DbHelperSQL.ExecuteSql(delCmd);
            if (rowAffect > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 判断是否为删除操作
    /// </summary>
    /// <returns></returns>
    private bool isDelete()
    {
        if (!string.IsNullOrEmpty(Request["localName"]) && !string.IsNullOrEmpty(Request["id"]))
        {
            return true;
        }

        return false;
    }
}
