using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections;
using System.Data.SqlClient;

/// <summary>
/// DRAuthentication 的摘要说明
/// </summary>
public class DRAuthentication
{
    public DataSet dss;

	public DRAuthentication()
	{
        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT Title,AuthType,ModuleType,name, Configuration ");
        sql.Append("FROM system_Modules  ");
        DataSet moduleds = FMOP.DB.DbHelperSQL.Query(sql.ToString());
        DataTable dt = getAllTable();
        for (int i = 0; i < moduleds.Tables[0].Rows.Count; i++)
        {
            string xml = moduleds.Tables[0].Rows[i]["Configuration"].ToString();
            //创建XmlDataDocument对象
            XmlDataDocument xmldoc = new XmlDataDocument();
            xmldoc.LoadXml(xml);
            int AuthType = int.Parse(moduleds.Tables[0].Rows[i]["AuthType"].ToString());
            int ModuleType = int.Parse(moduleds.Tables[0].Rows[i]["ModuleType"].ToString());
            XmlNode Authentication = getAuthentication(xmldoc, ModuleType);//获取节点
            DataSet nodeds = getNodeds(Authentication);
            string title = moduleds.Tables[0].Rows[i]["Title"].ToString();
            string moduleTy = getmoduleType(ModuleType);
            //string name = moduleds.Tables[0].Rows[i]["name"].ToString();
            if (nodeds == null && AuthType==0)
            {
                dt.Rows.Add(title, moduleTy, "所有用户", "全公司", "有", "有", "有", "全公司", "全公司");
            }
            else if (nodeds!=null)
            {
                for (int j = 0; j < nodeds.Tables[0].Rows.Count; j++)
                {
                    GetNodeContent get = new GetNodeContent(nodeds.Tables[0].Rows[j]);
                    string roleName = get.getroleName;
                    string roleType = get.getroleType;
                    string canAdd = get.getcanAdd;
                    string canView = get.getcanView;
                    string canModify = get.getcanModify;
                    string viewType = get.getviewType;
                    string modifyType = get.getmodifyType;
                    dt.Rows.Add(title, moduleTy, roleName, roleType, canAdd, canView, canModify, viewType, modifyType);
                }
            }
            else if (nodeds == null)
            {
                dt.Rows.Add(title, moduleTy, "未设置", "未设置", "无", "无", "无", "未设置", "未设置");
            }

        }
        DataSet ddss = new DataSet();
        ddss.Tables.Add(dt);
        dss = ddss;
    }

    /// <summary>
    /// 得到各个模块的权限节点
    /// </summary>
    /// <param name="type">模块类型</param>
    /// <returns></returns>
    public XmlNode getAuthentication(XmlDataDocument docment, int type)
    {
        XmlNode node = null;
        switch (type)
        {
            case 1:
                if (docment.SelectSingleNode("//WorkFlowModule") != null && docment.SelectSingleNode("//Authentication") != null)
                {
                    node = docment.SelectSingleNode("//Authentication");
                }
                break;
            case 2:
                if (docment.SelectSingleNode("//ReportModule") != null && docment.SelectSingleNode("//Authentication") != null)
                {
                    node = docment.SelectSingleNode("//Authentication");
                }
                break;
            case 3:
                if (docment.SelectSingleNode("//ChartModule") != null && docment.SelectSingleNode("//Authentication") != null)
                {
                    node = docment.SelectSingleNode("//Authentication");
                }
                break;
            case 4:
                if (docment.SelectSingleNode("//AddOnModule") != null && docment.SelectSingleNode("//Authentication") != null)
                {
                    node = docment.SelectSingleNode("//Authentication");
                }
                break;
        }
        return node;
    }
    /// <summary>
    /// 把节点转换成DataSet；
    /// </summary>
    /// <param name="node">权限节点</param>
    /// <returns></returns>
    public DataSet getNodeds(XmlNode node)
    {
        DataSet ds = new DataSet();
        if (node == null || node.InnerXml == "")
        {
            ds = null;
            return ds;
        }
        string nodetext = "<Authentication>" + node.InnerXml + "</Authentication>";
        StringReader sr = new StringReader(nodetext);
        XmlTextReader xr = new XmlTextReader(sr);
        ds.ReadXml(xr);
        return ds;
    }
    //
    private DataTable getAllTable()
    {
        DataTable dt = new DataTable("tempt");
        dt.Columns.Add("Title", System.Type.GetType("System.String"));
        dt.Columns.Add("ModuleType", System.Type.GetType("System.String"));
        dt.Columns.Add("roleName", System.Type.GetType("System.String"));
        dt.Columns.Add("roleType", System.Type.GetType("System.String"));
        dt.Columns.Add("canAdd", System.Type.GetType("System.String"));
        dt.Columns.Add("canView", System.Type.GetType("System.String"));
        dt.Columns.Add("canModify", System.Type.GetType("System.String"));
        dt.Columns.Add("viewType", System.Type.GetType("System.String"));
        dt.Columns.Add("modifyType", System.Type.GetType("System.String"));
        DataRow drrow = dt.NewRow();
        return dt;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private string getmoduleType(int type)
    {
        string moduleTy = "";
        switch (type)
        {
            case 1:
                moduleTy = "工作流";
                break;
            case 2:
                moduleTy = "文字报表";
                break;
            case 3:
                moduleTy = "图表";
                break;
            case 4:
                moduleTy = "自定义模块";
                break;
        }
        return moduleTy;
    }
    public void getRow(DataRow row)
    {
        //row[
    }
    public DataSet getdss
    {
        get
        {
            return dss;
        }
    }
}
